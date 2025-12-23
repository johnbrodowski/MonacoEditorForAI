using System.Text.Json.Serialization;
using MonacoEditor;

namespace AiEditorExample.Models;

/// <summary>
/// Base class for all edit commands
/// </summary>
[JsonDerivedType(typeof(InsertLineCommand), "insertLine")]
[JsonDerivedType(typeof(ReplaceLineCommand), "replaceLine")]
[JsonDerivedType(typeof(ReplaceLineRangeCommand), "replaceLineRange")]
[JsonDerivedType(typeof(DeleteLineCommand), "deleteLine")]
[JsonDerivedType(typeof(DeleteLineRangeCommand), "deleteLineRange")]
[JsonDerivedType(typeof(HighlightLineRangeCommand), "highlightLineRange")]
[JsonDerivedType(typeof(ClearHighlightCommand), "clearHighlight")]
[JsonDerivedType(typeof(ToggleBookmarkCommand), "toggleBookmark")]
public abstract class EditCommand
{
    /// <summary>
    /// The type of command (used for JSON serialization)
    /// </summary>
    [JsonPropertyName("type")]
    public abstract string Type { get; }

    /// <summary>
    /// The primary line number for sorting (for bottom-to-top execution)
    /// </summary>
    public abstract int SortPriority { get; }

    /// <summary>
    /// Execute this command on the Monaco editor
    /// </summary>
    public abstract Task ExecuteAsync(MonacoEditor.MonacoEditorService editor);

    /// <summary>
    /// Get a description of this command for logging
    /// </summary>
    public abstract string GetDescription();
}
