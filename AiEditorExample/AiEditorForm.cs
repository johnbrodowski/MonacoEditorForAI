using System.Text.Json;
using AiEditorExample.Models;
using AiEditorExample.Services;
using MonacoEditor;

namespace AiEditorExample;

/// <summary>
/// Main form for AI-powered code editing with Monaco Editor
/// </summary>
public partial class AiEditorForm : Form
{
    private MonacoEditor.MonacoEditorService? _editorService;
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

            // Initialize Monaco Editor Service
            _editorService = new MonacoEditor.MonacoEditorService(webView);

            string appDirectory = Application.StartupPath;
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

            await _editorService.InitializeAsync(appDirectory, initialCode, "csharp");

            SetStatus("Ready");
            btnSendToAi.Enabled = true;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error initializing editor: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            SetStatus("Error");
        }
    }

    private async void btnLoadFile_Click(object sender, EventArgs e)
    {
        if (_editorService == null) return;

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
                await _editorService.LoadFromFileAsync(openFileDialog.FileName);
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
        if (_editorService == null) return;

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
                await _editorService.SaveToFileAsync(saveFileDialog.FileName);
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
        if (_editorService == null) return;

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
            SetStatus("Getting code with line numbers...");

            // Get code with line numbers
            var codeWithLineNumbers = await _promptBuilder.GetCodeWithLineNumbersAsync(_editorService);

            SetStatus("Sending request to AI...");

            // Build request
            var request = _promptBuilder.BuildRequest(codeWithLineNumbers, instruction, apiKey);

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
            AppendCommandLog($"AI Response ({response.Usage.OutputTokens} tokens):");
            AppendCommandLog(responseText);
            AppendCommandLog("");

            // Process commands
            var result = await _commandProcessor.ProcessCommandsAsync(responseText, _editorService);

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
                SetStatus($"Completed with errors");
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
        }
        base.Dispose(disposing);
    }
}
