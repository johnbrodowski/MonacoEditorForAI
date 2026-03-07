using System.Text.Json;
using System.Text.Json.Serialization;
using AiEditorExample.Models;
using MonacoEditor;

namespace AiEditorExample.Services;

/// <summary>
/// Processes AI responses containing edit commands
/// </summary>
public class CommandProcessor
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() }
    };

    /// <summary>
    /// Process commands from AI response JSON.
    /// Pass <paramref name="editorNames"/> so that <c>listEditors</c> commands can report the open tabs.
    /// </summary>
    public async Task<CommandResult> ProcessCommandsAsync(
        string jsonResponse,
        MonacoEditor.MonacoEditorService editor,
        IReadOnlyList<string>? editorNames = null)
    {
        var result = new  CommandResult();

        try
        {
            // Strip markdown code fences if present
            jsonResponse = StripMarkdownCodeFences(jsonResponse);

            // Parse the JSON response
            var commands = ParseCommands(jsonResponse);

            if (commands.Count == 0)
            {
                result.Success = true;
                result.Message = "No commands to execute";
                return result;
            }

            // Sort commands bottom-to-top (highest line number first)
            var sortedCommands = SortBottomToTop(commands);

            // Execute commands
            foreach (var command in sortedCommands)
            {
                try
                {
                    if (command is ListEditorsCommand listCmd)
                        listCmd.EditorNames = editorNames ?? Array.Empty<string>();

                    await command.ExecuteAsync(editor);
                    result.CommandsExecuted++;
                    result.ExecutedCommands.Add(command.GetDescription());
                }
                catch (Exception ex)
                {
                    result.Errors.Add($"{command.GetDescription()}: {ex.Message}");
                }
            }

            result.Success = result.Errors.Count == 0;
            result.Message = result.Success
                ? $"Successfully executed {result.CommandsExecuted} command(s)"
                : $"Executed {result.CommandsExecuted} command(s) with {result.Errors.Count} error(s)";
        }
        catch (JsonException ex)
        {
            result.Success = false;
            result.Message = $"Failed to parse JSON response: {ex.Message}";
            result.Errors.Add(ex.Message);
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.Message = $"Unexpected error: {ex.Message}";
            result.Errors.Add(ex.Message);
        }

        return result;
    }

    /// <summary>
    /// Strip markdown code fences from JSON response
    /// </summary>
    private string StripMarkdownCodeFences(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return text;

        text = text.Trim();

        // Check if wrapped in markdown code fence
        if (text.StartsWith("```"))
        {
            // Find the end of the first line (```json or just ```)
            var firstLineEnd = text.IndexOf('\n');
            if (firstLineEnd > 0)
            {
                text = text.Substring(firstLineEnd + 1);
            }

            // Remove the closing ```
            if (text.EndsWith("```"))
            {
                text = text.Substring(0, text.Length - 3);
            }

            text = text.Trim();
        }

        return text;
    }

    /// <summary>
    /// Parse commands from JSON string
    /// </summary>
    private List< EditCommand> ParseCommands(string json)
    {
        var commands = new List<EditCommand>();

        // Try to parse as a response object with a "commands" array
        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        if (root.TryGetProperty("commands", out var commandsArray))
        {
            foreach (var commandElement in commandsArray.EnumerateArray())
            {
                var command = ParseSingleCommand(commandElement);
                if (command != null)
                {
                    commands.Add(command);
                }
            }
        }

        return commands;
    }

    /// <summary>
    /// Parse a single command from JSON element
    /// </summary>
    private EditCommand? ParseSingleCommand(JsonElement element)
    {
        if (!element.TryGetProperty("type", out var typeElement))
        {
            return null;
        }

        var type = typeElement.GetString();

        try
        {
            return type switch
            {
                "insertLine" => JsonSerializer.Deserialize< InsertLineCommand>(element.GetRawText(), JsonOptions),
                "replaceLine" => JsonSerializer.Deserialize<ReplaceLineCommand>(element.GetRawText(), JsonOptions),
                "replaceLineRange" => JsonSerializer.Deserialize<ReplaceLineRangeCommand>(element.GetRawText(), JsonOptions),
                "deleteLine" => JsonSerializer.Deserialize<DeleteLineCommand>(element.GetRawText(), JsonOptions),
                "deleteLineRange" => JsonSerializer.Deserialize<DeleteLineRangeCommand>(element.GetRawText(), JsonOptions),
                "highlightLineRange" => JsonSerializer.Deserialize<HighlightLineRangeCommand>(element.GetRawText(), JsonOptions),
                "clearHighlight" => JsonSerializer.Deserialize<ClearHighlightCommand>(element.GetRawText(), JsonOptions),
                "toggleBookmark" => JsonSerializer.Deserialize<ToggleBookmarkCommand>(element.GetRawText(), JsonOptions),
                "listEditors"   => JsonSerializer.Deserialize<ListEditorsCommand>(element.GetRawText(), JsonOptions),
                _ => null
            };
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Sort commands by line number descending (bottom-to-top)
    /// This prevents line number shifts from affecting subsequent edits
    /// </summary>
    private List< EditCommand> SortBottomToTop(List< EditCommand> commands)
    {
        return commands.OrderByDescending(c => c.SortPriority).ToList();
    }
}
