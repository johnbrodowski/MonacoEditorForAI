using AiEditorExample.Models;
using AiEditorExample.Services;
using MonacoEditor;

namespace AiEditorExample;

/// <summary>
/// Main form for AI-powered code editing with multiple Monaco Editor instances.
/// </summary>
public partial class AiEditorForm : Form
{
    private MonacoEditorManager? _editorManager;
    private AnthropicClient? _aiClient;
    private readonly CommandProcessor _commandProcessor;
    private readonly AiPromptBuilder _promptBuilder;

    public AiEditorForm()
    {
        InitializeComponent();
        _commandProcessor = new CommandProcessor();
        _promptBuilder = new AiPromptBuilder();
    }

    private async void AiEditorForm_Load(object sender, EventArgs e)
    {
        try
        {
            SetStatus("Initializing Monaco Editor...");

            _editorManager = new MonacoEditorManager(tabEditors, Application.StartupPath);

            string initialCode = @"using System;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(""Hello, World!"");
        }
    }
}";

            await _editorManager.CreateEditorAsync("Editor 1", initialCode, "csharp");

            SetStatus("Ready");
            btnSendToAi.Enabled = true;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error initializing editor: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            SetStatus("Error");
        }
    }

    private async void btnNewEditor_Click(object sender, EventArgs e)
    {
        if (_editorManager == null) return;

        try
        {
            var n = 1;
            string name;
            do { name = $"Editor {n++}"; } while (_editorManager.GetEditor(name) != null);
            SetStatus($"Creating {name}...");
            await _editorManager.CreateEditorAsync(name, "", "plaintext");
            SetStatus($"{name} ready");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error creating editor: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            SetStatus("Error");
        }
    }

    private async void btnRunTests_Click(object sender, EventArgs e)
    {
        if (_editorManager == null) return;

        btnRunTests.Enabled = false;
        SetStatus("Running editor tests...");
        AppendCommandLog("=== Editor Test Run ===");

        var runner = new EditorTestRunner();
        var results = await runner.RunAllTestsAsync(_editorManager);

        int passed = results.Count(r => r.Passed);

        foreach (var r in results)
        {
            AppendCommandLog($"{(r.Passed ? "✓" : "✗")} {r.Name}");
            if (!r.Passed)
            {
                AppendCommandLog($"    Expected: {r.Expected}");
                AppendCommandLog($"    Actual:   {r.Actual}");
            }
        }

        AppendCommandLog($"=== {passed}/{results.Count} passed ===");
        AppendCommandLog("");
        SetStatus($"Tests: {passed} passed, {results.Count - passed} failed");
        btnRunTests.Enabled = true;
    }

    private async void btnWatchTest_Click(object sender, EventArgs e)
    {
        if (_editorManager == null) return;

        btnWatchTest.Enabled = false;
        SetStatus("Running live edit test...");
        AppendCommandLog("=== Live Edit Test ===");

        var runner = new EditorTestRunner();
        var results = await runner.RunWatchableTestAsync(_editorManager);

        int passed = results.Count(r => r.Passed);

        foreach (var r in results)
        {
            AppendCommandLog($"{(r.Passed ? "✓" : "✗")} {r.Name}");
            if (!r.Passed)
            {
                AppendCommandLog($"    Expected: {r.Expected}");
                AppendCommandLog($"    Actual:   {r.Actual}");
            }
        }

        AppendCommandLog($"=== {passed}/{results.Count} passed ===");
        AppendCommandLog("");
        SetStatus($"Live test: {passed} passed, {results.Count - passed} failed");
        btnWatchTest.Enabled = true;
    }

    private async void btnAiSimTest_Click(object sender, EventArgs e)
    {
        if (_editorManager == null) return;

        btnAiSimTest.Enabled = false;
        SetStatus("Running AI simulation test...");
        AppendCommandLog("=== AI Command Simulation ===");

        var runner = new EditorTestRunner();
        var results = await runner.RunAiSimulationTestAsync(_editorManager);

        int passed = results.Count(r => r.Passed);

        foreach (var r in results)
        {
            AppendCommandLog($"{(r.Passed ? "✓" : "✗")} {r.Name}");
            if (!r.Passed)
            {
                AppendCommandLog($"    Expected: {r.Expected}");
                AppendCommandLog($"    Actual:   {r.Actual}");
            }
        }

        AppendCommandLog($"=== {passed}/{results.Count} passed ===");
        AppendCommandLog("");
        SetStatus($"AI sim: {passed} passed, {results.Count - passed} failed");
        btnAiSimTest.Enabled = true;
    }

    private async void btnLoadFile_Click(object sender, EventArgs e)
    {
        var editor = _editorManager?.GetActiveEditor();
        if (editor == null) return;

        using var openFileDialog = new OpenFileDialog
        {
            Filter = "All Files (*.*)|*.*|C# Files (*.cs)|*.cs|Text Files (*.txt)|*.txt",
            Title = "Select a file to load"
        };

        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
            try
            {
                SetStatus("Loading file...");
                await editor.LoadFromFileAsync(openFileDialog.FileName);
                SetStatus($"Loaded: {Path.GetFileName(openFileDialog.FileName)}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetStatus("Error");
            }
        }
    }

    private async void btnSaveFile_Click(object sender, EventArgs e)
    {
        var editor = _editorManager?.GetActiveEditor();
        if (editor == null) return;

        using var saveFileDialog = new SaveFileDialog
        {
            Filter = "All Files (*.*)|*.*|C# Files (*.cs)|*.cs|Text Files (*.txt)|*.txt",
            Title = "Save file"
        };

        if (saveFileDialog.ShowDialog() == DialogResult.OK)
        {
            try
            {
                SetStatus("Saving file...");
                await editor.SaveToFileAsync(saveFileDialog.FileName);
                SetStatus($"Saved: {Path.GetFileName(saveFileDialog.FileName)}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetStatus("Error");
            }
        }
    }

    private async void btnSendToAi_Click(object sender, EventArgs e)
    {
        var editor = _editorManager?.GetActiveEditor();
        if (editor == null) return;

        // Validate API key
        var apiKey = txtApiKey.Text.Trim();
        if (string.IsNullOrEmpty(apiKey))
        {
            MessageBox.Show("Please enter your Anthropic API key.", "API Key Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtApiKey.Focus();
            return;
        }

        // Validate instruction
        var instruction = txtInstruction.Text.Trim();
        if (string.IsNullOrEmpty(instruction))
        {
            MessageBox.Show("Please enter an instruction for the AI.", "Instruction Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtInstruction.Focus();
            return;
        }

        try
        {
            btnSendToAi.Enabled = false;
            var editorName = _editorManager!.GetActiveEditorName() ?? "Active Editor";
            SetStatus($"Getting code from {editorName}...");

            // Get code with line numbers from the active editor
            var codeWithLineNumbers = await _promptBuilder.GetCodeWithLineNumbersAsync(editor);

            SetStatus("Sending request to AI...");

            // Build request with multi-editor context
            var request = _promptBuilder.BuildRequest(
                codeWithLineNumbers,
                instruction,
                apiKey,
                editorName: editorName,
                allEditorNames: _editorManager!.GetEditorNames());

            // Send to AI
            _aiClient?.Dispose();
            _aiClient = new AnthropicClient(apiKey);
            var response = await _aiClient.SendMessageAsync(request);

            if (response == null)
            {
                throw new Exception("Received null response from AI");
            }

            var responseText = response.GetText();
            SetStatus("Processing AI response...");

            // Log the raw response for debugging
            AppendCommandLog($"[{editorName}] AI Response ({response.Usage.OutputTokens} tokens):");
            AppendCommandLog(responseText);
            AppendCommandLog("");

            // Process commands against the active editor
            var result = await _commandProcessor.ProcessCommandsAsync(
                responseText, editor, _editorManager!.GetEditorNames());

            // Display results
            if (result.Success)
            {
                AppendCommandLog($"✓ {result.Message}");
                foreach (var cmd in result.ExecutedCommands)
                {
                    AppendCommandLog($"  - {cmd}");
                }
                SetStatus($"Success: {result.CommandsExecuted} command(s) executed");
            }
            else
            {
                AppendCommandLog($"✗ {result.Message}");
                foreach (var error in result.Errors)
                {
                    AppendCommandLog($"  ERROR: {error}");
                }
                SetStatus("Completed with errors");
            }

            AppendCommandLog("");
        }
        catch (HttpRequestException ex)
        {
            MessageBox.Show($"Network error: {ex.Message}\n\nPlease check your internet connection and API key.", "Network Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            SetStatus("Network error");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            AppendCommandLog($"ERROR: {ex.Message}");
            SetStatus("Error");
        }
        finally
        {
            btnSendToAi.Enabled = true;
        }
    }

    // ── Tab close (right-click context menu) ──────────────────────────────

    private string? _rightClickedTabName;

    private void tabEditors_MouseDown(object sender, MouseEventArgs e)
    {
        if (e.Button != MouseButtons.Right) return;

        for (int i = 0; i < tabEditors.TabPages.Count; i++)
        {
            if (tabEditors.GetTabRect(i).Contains(e.Location))
            {
                _rightClickedTabName = tabEditors.TabPages[i].Text;
                // Prevent closing the very last tab
                menuCloseTab.Enabled = tabEditors.TabPages.Count > 1;
                contextMenuTab.Show(tabEditors, e.Location);
                return;
            }
        }

        _rightClickedTabName = null;
    }

    private void menuCloseTab_Click(object sender, EventArgs e)
    {
        if (_rightClickedTabName == null || _editorManager == null) return;

        var name = _rightClickedTabName;
        _rightClickedTabName = null;

        _editorManager.RemoveEditor(name);
        AppendCommandLog($"[Tab] Closed: {name}");
        SetStatus($"Closed: {name}");
    }

    private void btnClearLog_Click(object sender, EventArgs e)
    {
        txtCommandLog.Clear();
    }

    private void SetStatus(string status)
    {
        lblStatus.Text = $"Status: {status}";
        Application.DoEvents();
    }

    private void AppendCommandLog(string text)
    {
        txtCommandLog.AppendText(text + Environment.NewLine);
        txtCommandLog.SelectionStart = txtCommandLog.Text.Length;
        txtCommandLog.ScrollToCaret();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            components?.Dispose();
            _aiClient?.Dispose();
            _editorManager?.Dispose();
        }
        base.Dispose(disposing);
    }
}
