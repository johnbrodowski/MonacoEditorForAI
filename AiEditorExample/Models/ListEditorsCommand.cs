using MonacoEditor;

namespace AiEditorExample.Models;

/// <summary>
/// Command that reports all currently open editor tab names.
/// The AI can use this to discover available editors before targeting one.
/// No editor content is modified; the names are surfaced in the command log.
/// </summary>
public class ListEditorsCommand : EditCommand
{
    public override string Type => "listEditors";

    // Always sort last (no line-based priority needed)
    public override int SortPriority => 0;

    /// <summary>
    /// Populated by CommandProcessor before execution with the current editor names.
    /// </summary>
    internal IReadOnlyList<string> EditorNames { get; set; } = Array.Empty<string>();

    // No editor mutation — this command is query-only
    public override Task ExecuteAsync(MonacoEditorService editor) => Task.CompletedTask;

    public override string GetDescription() =>
        EditorNames.Count == 0
            ? "List editors: (none open)"
            : $"Open editors: {string.Join(", ", EditorNames)}";
}
