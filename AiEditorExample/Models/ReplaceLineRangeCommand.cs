using System.Text.Json.Serialization;
using MonacoEditor;

namespace AiEditorExample.Models;

/// <summary>
/// Command to replace a range of lines
/// </summary>
public class ReplaceLineRangeCommand : EditCommand
{
    [JsonPropertyName("type")]
    public override string Type => "replaceLineRange";

    [JsonPropertyName("startLine")]
    public int StartLine { get; set; }

    [JsonPropertyName("endLine")]
    public int EndLine { get; set; }

    [JsonPropertyName("text")]
    public string Text { get; set; } = string.Empty;

    public override int SortPriority => StartLine;

    public override async Task ExecuteAsync(MonacoEditor.MonacoEditorService editor)
    {
        await editor.ReplaceLineRangeAsync(StartLine, EndLine, Text);
    }

    public override string GetDescription()
    {
        return $"Replace lines {StartLine}-{EndLine}: \"{Text.Substring(0, Math.Min(50, Text.Length))}...\"";
    }
}
