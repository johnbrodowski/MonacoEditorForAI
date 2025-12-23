using System.Text.Json.Serialization;
using MonacoEditor;

namespace AiEditorExample.Models;

/// <summary>
/// Command to toggle a bookmark on a specific line
/// </summary>
public class ToggleBookmarkCommand : EditCommand
{
    [JsonPropertyName("type")]
    public override string Type => "toggleBookmark";

    [JsonPropertyName("line")]
    public int Line { get; set; }

    public override int SortPriority => Line;

    public override async Task ExecuteAsync(MonacoEditor.MonacoEditorService editor)
    {
        await editor.ToggleBookmarkAsync(Line);
    }

    public override string GetDescription()
    {
        return $"Toggle bookmark on line {Line}";
    }
}
