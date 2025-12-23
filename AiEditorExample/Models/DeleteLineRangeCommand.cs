using System.Text.Json.Serialization;
using MonacoEditor;

namespace AiEditorExample.Models;

/// <summary>
/// Command to delete a range of lines
/// </summary>
public class DeleteLineRangeCommand : EditCommand
{
    [JsonPropertyName("type")]
    public override string Type => "deleteLineRange";

    [JsonPropertyName("startLine")]
    public int StartLine { get; set; }

    [JsonPropertyName("endLine")]
    public int EndLine { get; set; }

    public override int SortPriority => StartLine;

    public override async Task ExecuteAsync(MonacoEditor.MonacoEditorService editor)
    {
        await editor.DeleteLineRangeAsync(StartLine, EndLine);
    }

    public override string GetDescription()
    {
        return $"Delete lines {StartLine}-{EndLine}";
    }
}
