using System.Text.Json.Serialization;
using MonacoEditor;

namespace AiEditorExample.Models;

/// <summary>
/// Command to replace a single line
/// </summary>
public class ReplaceLineCommand : EditCommand
{
    [JsonPropertyName("type")]
    public override string Type => "replaceLine";

    [JsonPropertyName("line")]
    public int Line { get; set; }

    [JsonPropertyName("text")]
    public string Text { get; set; } = string.Empty;

    public override int SortPriority => Line;

    public override async Task ExecuteAsync(MonacoEditor.MonacoEditorService editor)
    {
        await editor.ReplaceLineAsync(Line, Text);
    }

    public override string GetDescription()
    {
        return $"Replace line {Line}: \"{Text.Substring(0, Math.Min(50, Text.Length))}...\"";
    }
}
