using System.Text.Json.Serialization;
using MonacoEditor;

namespace AiEditorExample.Models;

/// <summary>
/// Command to delete a single line
/// </summary>
public class DeleteLineCommand : EditCommand
{
    [JsonPropertyName("type")]
    public override string Type => "deleteLine";

    [JsonPropertyName("line")]
    public int Line { get; set; }

    public override int SortPriority => Line;

    public override async Task ExecuteAsync(MonacoEditor.MonacoEditorService editor)
    {
        await editor.DeleteLineRangeAsync(Line, Line);
    }

    public override string GetDescription()
    {
        return $"Delete line {Line}";
    }
}
