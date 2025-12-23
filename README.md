# Monaco Editor Service

A powerful C# wrapper for integrating the **Monaco Editor** (the code editor that powers Visual Studio Code) into Windows Forms applications using WebView2.

## Features

- **Full Monaco Editor Integration** - Embed VS Code's editor directly in your Windows Forms apps
- **Rich API** - Comprehensive methods for editor manipulation and control
- **File Operations** - Load, save, and manipulate file content
- **Syntax Highlighting** - Support for all Monaco Editor languages
- **Text Manipulation** - Insert, replace, delete, and stream text
- **Line Operations** - Get, replace, and manipulate individual lines or ranges
- **Precise Range Operations** - Get, insert, delete, replace, and select text at exact character positions
- **Visual Decorations** - Highlight lines and add bookmarks
- **Cursor Management** - Set position and get current location
- **Streaming Support** - Stream text chunks for real-time updates (perfect for AI code generation)
- **Async/Await** - Full async support for smooth UI interactions

## Requirements

- **.NET 10.0** (or modify the project file for your target framework)
- **Windows Forms** application
- **WebView2 Runtime** (usually pre-installed on Windows 10/11)
- **Monaco Editor files** - Download from [Monaco Editor](https://microsoft.github.io/monaco-editor/)

## Installation

### 1. Add NuGet Package

```bash
# Coming soon - NuGet package publication pending
# For now, clone and reference the project directly
```

### 2. Clone and Reference

```bash
git clone https://github.com/johnbrodowski/Monaco-Editor-Service.git
```

Then add a project reference in your Windows Forms application.

### 3. Download Monaco Editor

Download Monaco Editor files from the [official website](https://microsoft.github.io/monaco-editor/) and place them in your application directory:

```
YourApp/
├── monaco-editor/
│   └── min/
│       └── vs/
│           ├── loader.js
│           ├── editor/
│           └── ...
```

## Quick Start

> **💡 Want a complete working example?** Check out [Examples/BasicEditorForm.cs](Examples/BasicEditorForm.cs) for a full Windows Forms application demonstrating all features!

### Basic Setup

```csharp
using MonacoEditor;
using Microsoft.Web.WebView2.WinForms;

public class MyForm : Form
{
    private WebView2 webView;
    private MonacoEditorService editorService;

    public MyForm()
    {
        // Initialize WebView2
        webView = new WebView2 { Dock = DockStyle.Fill };
        Controls.Add(webView);

        // Initialize Monaco Editor Service
        editorService = new MonacoEditorService(webView);

        InitializeEditor();
    }

    private async void InitializeEditor()
    {
        string appDirectory = Application.StartupPath;
        string initialCode = "// Start coding...\n";

        await editorService.InitializeAsync(
            appDirectory,
            initialCode,
            language: "csharp",
            width: 800,
            height: 600
        );

        // Wait for editor to be ready
        await editorService.EditorReady;

        // Now you can use the editor!
        await editorService.SetValueAsync("Console.WriteLine(\"Hello, Monaco!\");");
    }
}
```

## API Overview

### Initialization

| Method | Description |
|--------|-------------|
| `InitializeAsync(appDirectory, initialCode, language, width, height)` | Initialize the editor with initial content and settings |
| `EditorReady` | Task that completes when the editor is ready to use |

### File Operations

| Method | Description |
|--------|-------------|
| `GetAllTextAsync()` | Get all text from the editor |
| `SetValueAsync(value)` | Set the entire editor content |
| `SaveToFileAsync(filePath)` | Save editor content to a file |
| `LoadFromFileAsync(filePath)` | Load file content into the editor |
| `CountLinesAsync()` | Get the total number of lines |

### Line Operations

| Method | Description |
|--------|-------------|
| `GetLineTextAsync(lineNumber)` | Get text from a specific line |
| `GetLineRangeAsync(startLine, endLine)` | Get text from a range of lines |
| `ReplaceLineAsync(lineNumber, newText)` | Replace a single line |
| `ReplaceLineRangeAsync(startLine, endLine, newText)` | Replace a range of lines |
| `DeleteLineRangeAsync(startLine, endLine)` | Delete a range of lines |

### Text Editing

| Method | Description |
|--------|-------------|
| `InsertTextAsync(lineNumber, column, text)` | Insert text at a specific position |
| `GetSelectedTextAsync()` | Get currently selected text |

### Precise Range Operations

| Method | Description |
|--------|-------------|
| `GetTextInRangeAsync(startLine, startColumn, endLine, endColumn)` | Get text from a specific character range |
| `InsertTextAtRangeAsync(startLine, startColumn, endLine, endColumn, text)` | Insert text at a specific range, replacing any text in that range |
| `DeleteRangeAsync(startLine, startColumn, endLine, endColumn)` | Delete text in a specific character range |
| `ReplaceRangeAsync(startLine, startColumn, endLine, endColumn, newText)` | Replace text in a specific character range with new text |
| `SelectRangeAsync(startLine, startColumn, endLine, endColumn)` | Select and highlight a specific character range |

### Cursor and Position

| Method | Description |
|--------|-------------|
| `GetPositionAsync()` | Get current cursor position (line, column) |
| `SetCursorPositionAsync(lineNumber, column)` | Set cursor position and focus editor |

### Streaming (for Real-time Updates)

| Method | Description |
|--------|-------------|
| `BeginStreamAsync()` | Start a streaming session |
| `StreamChunkAsync(text)` | Stream a chunk of text to the editor |
| `EndStreamAsync()` | End the streaming session |

### Visual Decorations

| Method | Description |
|--------|-------------|
| `HighlightLineRangeAsync(startLine, endLine)` | Highlight a range of lines |
| `ClearHighlightAsync()` | Clear all highlights |
| `ToggleBookmarkAsync(lineNumber)` | Toggle a bookmark on a line |
| `ClearAllDecorationsAsync()` | Clear all highlights and bookmarks |
| `GetDecorationInfoAsync(lineNumber)` | Get decoration information for a line |

## Usage Examples

### Example 1: Load and Save Files

```csharp
// Load a C# file
await editorService.LoadFromFileAsync(@"C:\MyProject\Program.cs");

// Get the content
string code = await editorService.GetAllTextAsync();

// Modify and save
await editorService.InsertTextAsync(1, 1, "// Modified by MonacoEditorService\n");
await editorService.SaveToFileAsync(@"C:\MyProject\Program_Modified.cs");
```

### Example 2: Highlight and Navigate Code

```csharp
// Highlight lines 10-15
await editorService.HighlightLineRangeAsync(10, 15);

// Set cursor to line 12
await editorService.SetCursorPositionAsync(12, 1);

// Add a bookmark
await editorService.ToggleBookmarkAsync(12);

// Clear highlights
await editorService.ClearHighlightAsync();
```

### Example 3: Stream AI-Generated Code

Perfect for AI code generation or real-time updates:

```csharp
await editorService.BeginStreamAsync();

foreach (var codeChunk in aiGeneratedChunks)
{
    await editorService.StreamChunkAsync(codeChunk);
    await Task.Delay(50); // Smooth streaming effect
}

await editorService.EndStreamAsync();
```

### Example 4: Working with Selections

```csharp
// Get current cursor position
(int line, int column) = await editorService.GetPositionAsync();
Console.WriteLine($"Cursor at Line {line}, Column {column}");

// Get selected text
string selectedText = await editorService.GetSelectedTextAsync();
if (!string.IsNullOrEmpty(selectedText))
{
    Console.WriteLine($"Selected: {selectedText}");
}
```

### Example 5: Line Manipulation

```csharp
// Get a specific line
string line5 = await editorService.GetLineTextAsync(5);

// Get multiple lines
string lines10to20 = await editorService.GetLineRangeAsync(10, 20);

// Replace a line
await editorService.ReplaceLineAsync(15, "// This line was replaced");

// Delete lines
await editorService.DeleteLineRangeAsync(20, 25);
```

### Example 6: Precise Range Operations

Work with exact character positions for fine-grained control:

```csharp
// Get text from a specific character range (line 5 col 1 to line 7 col 30)
string rangeText = await editorService.GetTextInRangeAsync(5, 1, 7, 30);
Console.WriteLine($"Range text: {rangeText}");

// Replace text in a precise range
await editorService.ReplaceRangeAsync(10, 5, 10, 25, "new code here");

// Delete a specific range
await editorService.DeleteRangeAsync(15, 10, 15, 50);

// Select and highlight a range
await editorService.SelectRangeAsync(20, 1, 25, 40);

// Insert text at a range (replaces existing text in range)
await editorService.InsertTextAtRangeAsync(30, 1, 30, 1, "// Inserted text\n");
```

## Supported Languages

Monaco Editor supports syntax highlighting for many languages including:

- C# (`csharp`)
- JavaScript (`javascript`)
- TypeScript (`typescript`)
- Python (`python`)
- Java (`java`)
- C++ (`cpp`)
- HTML (`html`)
- CSS (`css`)
- JSON (`json`)
- XML (`xml`)
- And many more...

## Architecture

The service uses **WebView2** to host the Monaco Editor HTML/JavaScript environment and provides a clean C# API to interact with it. Communication happens through:

- **JavaScript injection** for sending commands to the editor
- **Web message passing** for receiving events from the editor
- **JSON serialization** for data exchange

## Thread Safety

All methods are async and should be called from the UI thread. The service handles the async communication with the WebView2 control.

## Cleanup

Don't forget to dispose of the service:

```csharp
protected override void OnFormClosing(FormClosingEventArgs e)
{
    editorService?.Dispose();
    base.OnFormClosing(e);
}
```

## Examples

The repository contains complete, working examples:

- **[BasicEditorForm.cs](Examples/BasicEditorForm.cs)** - A full Windows Forms application demonstrating:
  - Editor initialization with syntax highlighting
  - File load/save operations
  - Line highlighting and navigation
  - Bookmark management
  - Cursor position handling
  - Text insertion

- **[AiEditorExample](AiEditorExample/)** - AI-powered code editor combining MonacoEditorService with Anthropic Claude:
  - AI-assisted code editing with natural language instructions
  - Command-based editing system (insert, replace, delete, highlight, bookmark)
  - 8 command types for comprehensive code manipulation
  - Bottom-to-top command processing to prevent line shift issues
  - Real-time code modifications by AI
  - Visual feedback with highlights and bookmarks
  - Complete integration example with Anthropic API
  - See [AiEditorExample/README.md](AiEditorExample/README.md) for details

Each example includes detailed comments and demonstrates best practices.

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request. For major changes, please open an issue first to discuss what you would like to change.

See [CONTRIBUTING.md](CONTRIBUTING.md) for guidelines.

## License

This project is licensed under the Apache License 2.0 - see the [LICENSE.txt](LICENSE.txt) file for details.

## Acknowledgments

- **Monaco Editor** - Microsoft's excellent code editor
- **WebView2** - For enabling modern web content in Windows applications

## Support

If you encounter any issues or have questions:

- Open an [Issue](https://github.com/johnbrodowski/Monaco-Editor-Service/issues)
- Check existing issues for solutions
- Contribute improvements via Pull Requests

---

**Made with ❤️ for the .NET and Monaco Editor communities**
