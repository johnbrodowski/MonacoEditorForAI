# About this code
This example was extracted from AGPA — my fully autonomous general-purpose agent (closed-source, ~150k LOC).

# Monaco Editor For AI

Seamless Monaco Editor (VS Code's core) integration for .NET AI/agent applications.

Built for real-time code generation, autonomous editing, and local LLM workflows — with streaming support, dynamic language switching, and file sync.

Battle-tested in production autonomous agents.
 

<img width="1201" height="628" alt="monaco" src="https://github.com/user-attachments/assets/13097625-887b-4d60-a2a1-b473b6ddcb03" />

This example demonstrates how to build an AI-powered code editor using **MonacoEditorService** and the **Anthropic Claude API**. The AI can edit code using a simple command-based system without requiring Anthropic's tool use feature.

## Features

- **Monaco Editor Integration**: Full-featured code editor powered by VS Code's Monaco Editor
- **AI-Powered Editing**: Claude AI can understand code and make targeted edits
- **Command-Based System**: Simple JSON command format for AI to execute edits
- **Bottom-to-Top Processing**: Commands are executed from highest to lowest line number to prevent line shifts
- **8 Command Types**:
  - `insertLine` - Insert a new line at specified position
  - `replaceLine` - Replace a single line
  - `replaceLineRange` - Replace multiple consecutive lines
  - `deleteLine` - Delete a single line
  - `deleteLineRange` - Delete a line range
  - `highlightLineRange` - Highlight lines for visual emphasis
  - `clearHighlight` - Clear all highlights
  - `toggleBookmark` - Toggle bookmarks on specific lines
- **Command Log**: See exactly what commands the AI executed
- **File Operations**: Load and save files
- **Built-in Test Suite**: Three test modes verify editor correctness without needing an API key

## Prerequisites

1. **.NET 10.0 SDK** or later
2. **Visual Studio 2022** (or compatible IDE)
3. **Anthropic API Key** - Get one at [console.anthropic.com](https://console.anthropic.com)
4. **Monaco Editor Files** - See setup instructions below
5. **WebView2 Runtime** - Usually pre-installed on Windows 11

## Setup Instructions

### 1. Clone and Build

```bash
git clone <repository-url>
cd MonacoEditorServiceWithAiEdit
dotnet build
```

### 2. Install Monaco Editor Files

The Monaco Editor files need to be available for the WebView2 control. You have two options:

#### Option A: Download Automatically (Recommended)

The `MonacoEditorService` can automatically download Monaco Editor files on first run:

```csharp
await MonacoEditorService.EnsureMonacoEditorFilesAsync();
```

This is already included in the example, so Monaco will be downloaded automatically.

#### Option B: Manual Installation

1. Download Monaco Editor from [Microsoft CDN](https://cdn.jsdelivr.net/npm/monaco-editor@latest/min/)
2. Extract to: `{YourProjectFolder}/bin/Debug/net10.0-windows/monaco-editor/`
3. Ensure the following structure:
   ```
   monaco-editor/
   ├── min/
   │   ├── vs/
   │   │   ├── editor/
   │   │   ├── base/
   │   │   └── loader.js
   ```

### 3. Get Anthropic API Key

1. Sign up at [console.anthropic.com](https://console.anthropic.com)
2. Create an API key
3. Keep it secure - never commit it to source control

### 4. Run the Example

```bash
cd AiEditorExample
dotnet run
```

Or open the solution in Visual Studio and run the **AiEditorExample** project.

## How to Use

### Basic Workflow

1. **Launch the Application**
   - The editor will initialize with sample C# code

2. **Enter Your API Key**
   - Paste your Anthropic API key in the "Anthropic API Key" field
   - This is stored in memory only (not saved to disk)

3. **Load or Write Code**
   - Use "Load File" to open an existing file
   - Or type/paste code directly in the editor

4. **Give the AI an Instruction**
   - Examples:
     - "Add error handling to the Main method"
     - "Add XML documentation comments to all methods"
     - "Refactor this code to use async/await"
     - "Add input validation to all public methods"
     - "Remove all Console.WriteLine statements"

5. **Send to AI**
   - Click "Send to AI"
   - Watch the command log to see what the AI is doing
   - The code will update in real-time

6. **Save Your Work**
   - Use "Save File" to save the edited code

### Example Instructions to Try

**Add Error Handling:**
```
Add try-catch error handling to the Main method with proper logging
```

**Add Documentation:**
```
Add XML documentation comments to all public methods
```

**Code Refactoring:**
```
Refactor the code to use dependency injection
```

**Add Validation:**
```
Add null checks and input validation to all method parameters
```

**Clean Up Code:**
```
Remove all commented-out code and unnecessary whitespace
```

**Add Logging:**
```
Add logging statements at the beginning and end of each method
```

**Highlight Important Code:**
```
Highlight the error handling code and add bookmarks to all TODO comments
```

**Mark Review Points:**
```
Add bookmarks to all public methods and highlight any sections that need optimization
```

## Testing

The sidebar includes three buttons that run tests against the live Monaco editor — no API key required.

### Run Editor Tests

Runs `EditorTestRunner.RunAllTestsAsync` — a fast, headless suite that opens a temporary tab, exercises every command type, and closes it. Results appear in the command log:

```
=== Editor Test Run ===
✓ Insert line
✓ Replace line
✓ Replace line range
✓ Delete line
✓ Delete line range
✓ Multi-command (bottom-to-top sort)
=== 6/6 passed ===
```

### Watch Live Edit Test

Runs `EditorTestRunner.RunWatchableTestAsync` — the same edits as above but with a ~900 ms delay between steps so you can watch each change happen in the editor tab. The tab stays open after the test so you can inspect the final state.

### Simulate AI Commands

Runs `EditorTestRunner.RunAiSimulationTestAsync` — feeds raw JSON strings through `CommandProcessor.ProcessCommandsAsync`, exactly as real Claude responses would arrive. Opens a `⚗ AI Sim` tab containing a C# `Calculator` class with intentional bugs and plays through five AI "turns":

| Turn | Commands in JSON | What changes |
|------|-----------------|--------------|
| 1 | `replaceLine` + `deleteLine` | Fixes `return a + b` → `return a + b;` and removes stale TODO in one multi-command response; bottom-to-top sort is exercised |
| 2 | `insertLine` | Adds argument validation inside `Add()` |
| 3 | `replaceLineRange` | Rewrites the entire `Multiply()` body with the same validation |
| 4 | `highlightLineRange` × 2, `toggleBookmark` × 2 | Marks both new validation lines |
| 5 | `clearHighlight` | Cleans up all decorations |

Turns 1–3 assert exact editor content via `GetAllTextAsync()`. Turns 4–5 verify that decoration commands do not corrupt the text.

### How Correctness Is Checked

After each step (or AI turn), the test runner calls `editor.GetAllTextAsync()`, normalizes line endings and trailing whitespace, and compares the result to a hardcoded expected string. A `TestCaseResult` records `Passed`, `Expected`, and `Actual` for every step. Any mismatch is printed in full to the command log so you can see exactly what diverged.

## Command Format Reference

The AI responds with JSON in this format:

```json
{
  "commands": [
    {
      "type": "insertLine",
      "line": 5,
      "text": "// New comment"
    },
    {
      "type": "replaceLine",
      "line": 10,
      "text": "int x = 5;"
    },
    {
      "type": "replaceLineRange",
      "startLine": 15,
      "endLine": 18,
      "text": "new code\nmore code\neven more"
    },
    {
      "type": "deleteLine",
      "line": 20
    },
    {
      "type": "deleteLineRange",
      "startLine": 25,
      "endLine": 30
    },
    {
      "type": "highlightLineRange",
      "startLine": 5,
      "endLine": 10
    },
    {
      "type": "clearHighlight"
    },
    {
      "type": "toggleBookmark",
      "line": 15
    }
  ]
}
```

### Command Types

| Command | Parameters | Description |
|---------|------------|-------------|
| `insertLine` | `line`, `text` | Insert new line at position |
| `replaceLine` | `line`, `text` | Replace single line content |
| `replaceLineRange` | `startLine`, `endLine`, `text` | Replace multiple lines (use `\n` for line breaks) |
| `deleteLine` | `line` | Delete single line |
| `deleteLineRange` | `startLine`, `endLine` | Delete line range (inclusive) |
| `highlightLineRange` | `startLine`, `endLine` | Highlight a range of lines for visual emphasis |
| `clearHighlight` | *(none)* | Clear all highlights from the editor |
| `toggleBookmark` | `line` | Toggle bookmark on a specific line |

### Important Notes

- **Line numbers are 1-based** (first line is 1, not 0)
- **Ranges are inclusive** (both startLine and endLine are included)
- **Multi-line text** uses `\n` as separator in the `text` field
- **Bottom-to-top execution** prevents line number shifts

## Architecture

### Project Structure

```
AiEditorExample/
├── Models/
│   ├── EditCommand.cs              # Base command class
│   ├── InsertLineCommand.cs
│   ├── ReplaceLineCommand.cs
│   ├── ReplaceLineRangeCommand.cs
│   ├── DeleteLineCommand.cs
│   ├── DeleteLineRangeCommand.cs
│   └── CommandResult.cs
├── Services/
│   ├── CommandProcessor.cs        # Parses and executes commands
│   ├── AiPromptBuilder.cs         # Builds AI prompts
│   └── EditorTestRunner.cs        # Fast, watchable, and AI-simulation tests
├── AiEditorForm.cs                 # Main UI
├── AnthropicClient.cs              # AI client
└── [Message models...]
```

### Key Components

**CommandProcessor**
- Parses JSON responses from AI
- Validates command structure
- Sorts commands bottom-to-top
- Executes commands sequentially

**AiPromptBuilder**
- Formats code with line numbers
- Builds system prompt teaching AI the command format
- Creates message requests

**EditCommand Classes**
- One class per command type
- Each implements `ExecuteAsync()` using MonacoEditorService
- `SortPriority` enables bottom-to-top execution

### Why Bottom-to-Top Processing?

When editing code by line number, changes at the beginning of the file can shift line numbers for subsequent edits:

```
Original:
1: line one
2: line two
3: line three

If we insert at line 1 first:
1: NEW LINE    ← inserted
2: line one    ← shifted down!
3: line two
4: line three

Now "line two" is at line 3, not line 2!
```

**Solution:** Execute commands from highest line number to lowest:

```
Original:
1: line one
2: line two
3: line three

Execute from bottom-to-top:
1. Delete line 3
2. Replace line 2
3. Insert at line 1

Line numbers remain valid throughout!
```

## Troubleshooting

### Monaco Editor Not Loading

**Problem:** White screen or "Monaco Editor not found" error

**Solutions:**
- Ensure Monaco Editor files are in the correct location
- Check that `EnsureMonacoEditorFilesAsync()` completes successfully
- Verify internet connection for automatic download
- Try manual installation (see Setup Instructions)

### API Errors

**Problem:** "Invalid API key" or network errors

**Solutions:**
- Verify your API key is correct
- Check that you have API credits available
- Ensure internet connection is active
- Check Anthropic API status page

### AI Returning Invalid JSON

**Problem:** "Failed to parse JSON response" error

**Solutions:**
- This is rare but can happen if the AI misunderstands the format
- Try rewording your instruction to be more specific
- Check the command log for the raw response
- The system prompt is designed to prevent this, but edge cases exist

### Commands Not Executing

**Problem:** "0 commands executed" message

**Solutions:**
- Check if the AI determined no changes were needed
- Verify the JSON response in the command log
- Try a more specific instruction
- Make sure the code actually needs the requested changes

### Line Number Issues

**Problem:** Commands executing on wrong lines

**Solutions:**
- This should not happen due to bottom-to-top processing
- If it does, please report as a bug
- Check if your code has unusual line endings (CR/LF vs LF)

## Extending the Example

### Add New Command Types

1. Create a new class inheriting from `EditCommand`
2. Implement `Type`, `SortPriority`, `ExecuteAsync()`, and `GetDescription()`
3. Add to `CommandProcessor.ParseSingleCommand()` switch statement
4. Update the system prompt in `AiPromptBuilder`

### Add Syntax Highlighting

Monaco Editor supports many languages. To change the language:

```csharp
// In AiEditorForm.cs, after InitializeAsync()
await _editorService.ExecuteScriptAsync(@"
    monaco.editor.setModelLanguage(editor.getModel(), 'python');
");
```

Supported languages: `csharp`, `javascript`, `typescript`, `python`, `java`, `cpp`, `json`, `xml`, `html`, `css`, and many more.

### Add Confirmation Dialog

Show commands before executing:

```csharp
// In btnSendToAi_Click, before ProcessCommandsAsync
var preview = string.Join("\n", commands.Select(c => c.GetDescription()));
var confirm = MessageBox.Show($"Execute these commands?\n\n{preview}",
    "Confirm", MessageBoxButtons.YesNo);
if (confirm != DialogResult.Yes) return;
```

### Add Undo/Redo

Store editor state before AI edits:

```csharp
private string _beforeEdit = "";

// Before ProcessCommandsAsync:
_beforeEdit = await _editorService.GetAllTextAsync();

// Add Undo button:
await _editorService.SetValueAsync(_beforeEdit);
```

## Performance Tips

- **Large Files**: For files > 1000 lines, consider showing a progress indicator
- **Multiple Edits**: The AI can return multiple commands in one response for efficiency
- **Token Usage**: Longer code = more input tokens. Monitor usage in the command log
- **Model Selection**: Use Claude Haiku for simple edits, Sonnet for complex refactoring

## Security Notes

- **API Key Storage**: This example stores the API key in memory only (not persisted)
- **For Production**: Use secure credential storage (Windows Credential Manager, Azure Key Vault, etc.)
- **Code Validation**: Consider adding validation before executing AI commands
- **Backup**: Always save your work before running AI edits on important code

## License

This project is licensed under the Apache License 2.0 - see the [LICENSE.txt](../LICENSE.txt) file for details.

## Contributing

See [CONTRIBUTING.md](../CONTRIBUTING.md) for guidelines on contributing to this project.

## Support

For issues, questions, or suggestions:
1. Check this README and the main project README
2. Review the [IMPLEMENTATION_PLAN.md](../IMPLEMENTATION_PLAN.md) for architecture details
3. Open an issue on GitHub

## Credits

- **Monaco Editor**: Microsoft ([monaco-editor](https://microsoft.github.io/monaco-editor/))
- **Anthropic Claude**: Anthropic ([anthropic.com](https://www.anthropic.com))
- **MonacoEditorService**: John Brodowski

## Example Output

```
AI Response (247 tokens):
{
  "commands": [
    {"type": "insertLine", "line": 8, "text": "        try"},
    {"type": "insertLine", "line": 9, "text": "        {"},
    {"type": "insertLine", "line": 11, "text": "        }"},
    {"type": "insertLine", "line": 12, "text": "        catch (Exception ex)"},
    {"type": "insertLine", "line": 13, "text": "        {"},
    {"type": "insertLine", "line": 14, "text": "            Console.WriteLine($\"Error: {ex.Message}\");"},
    {"type": "insertLine", "line": 15, "text": "        }"}
  ]
}

✓ Successfully executed 7 command(s)
  - Insert line 15: "        }"
  - Insert line 14: "            Console.WriteLine($\"Error: {ex.Message}\");"
  - Insert line 13: "        {"
  - Insert line 12: "        catch (Exception ex)"
  - Insert line 11: "        }"
  - Insert line 9: "        {"
  - Insert line 8: "        try"
```

---

**Happy AI-powered coding!** 🚀
