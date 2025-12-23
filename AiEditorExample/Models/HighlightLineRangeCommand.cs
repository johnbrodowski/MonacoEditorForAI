using System.Text.Json.Serialization;
using MonacoEditor;

namespace AiEditorExample.Models;

/// <summary>
/// Command to highlight a range of lines
/// </summary>
public class HighlightLineRangeCommand : EditCommand
{
    [JsonPropertyName("type")]
    public override string Type => "highlightLineRange";

    [JsonPropertyName("startLine")]
    public int StartLine { get; set; }

    [JsonPropertyName("endLine")]
    public int EndLine { get; set; }

    public override int SortPriority => StartLine;

    public override async Task ExecuteAsync(MonacoEditor.MonacoEditorService editor)
    {
        await editor.HighlightLineRangeAsync(StartLine, EndLine);
    }

    public override string GetDescription()
    {
        return $"Highlight lines {StartLine}-{EndLine}";
    }
}
