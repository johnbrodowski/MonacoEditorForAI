namespace AiEditorExample.Models;

/// <summary>
/// Result of processing a set of commands
/// </summary>
public class CommandResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public int CommandsExecuted { get; set; }
    public List<string> Errors { get; set; } = new();
    public List<string> ExecutedCommands { get; set; } = new();
}
