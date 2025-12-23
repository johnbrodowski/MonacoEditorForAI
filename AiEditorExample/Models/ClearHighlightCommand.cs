using System.Text.Json.Serialization;
using MonacoEditor;

namespace AiEditorExample.Models;

/// <summary>
/// Command to clear all highlights
/// </summary>
public class ClearHighlightCommand : EditCommand
{
    [JsonPropertyName("type")]
    public override string Type => "clearHighlight";

    // Use 0 as sort priority since this doesn't affect specific lines
    public override int SortPriority => 0;

    public override async Task ExecuteAsync(MonacoEditor.MonacoEditorService editor)
    {
        await editor.ClearHighlightAsync();
    }

    public override string GetDescription()
    {
        return "Clear all highlights";
    }
}
