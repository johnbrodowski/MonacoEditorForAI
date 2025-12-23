using System.Text.Json.Serialization;
using MonacoEditor;

namespace AiEditorExample.Models;

/// <summary>
/// Command to insert a new line at a specific position
/// </summary>
public class InsertLineCommand : EditCommand
{
    [JsonPropertyName("type")]
    public override string Type => "insertLine";

    [JsonPropertyName("line")]
    public int Line { get; set; }

    [JsonPropertyName("text")]
    public string Text { get; set; } = string.Empty;

    public override int SortPriority => Line;

    public override async Task ExecuteAsync(MonacoEditorService editor)
    {
        // Insert text at the beginning of the specified line
        await editor.InsertTextAsync(Line, 1, Text + "\n");
    }

    public override string GetDescription()
    {
        return $"Insert line {Line}: \"{Text.Substring(0, Math.Min(50, Text.Length))}...\"";
    }
}
