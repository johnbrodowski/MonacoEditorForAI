using System.Text;
using MonacoEditor;

using AiEditorExample.Models;
namespace AiEditorExample.Services;

/// <summary>
/// Builds prompts for AI with line-numbered code
/// </summary>
public class AiPromptBuilder
{
    /// <summary>
    /// Get code from editor with line numbers
    /// </summary>
    public async Task<string> GetCodeWithLineNumbersAsync(MonacoEditor.MonacoEditorService editor)
    {
        var allText = await editor.GetAllTextAsync();
        if (string.IsNullOrEmpty(allText))
        {
            return string.Empty;
        }

        var lines = allText.Split('\n');
        var sb = new StringBuilder();

        for (int i = 0; i < lines.Length; i++)
        {
            sb.AppendLine($"{i + 1}: {lines[i]}");
        }

        return sb.ToString();
    }

    /// <summary>
    /// Build the system prompt that teaches the AI how to use commands
    /// </summary>
    public string BuildSystemPrompt()
    {
        return @"You are an AI code editor assistant. You can edit code by responding with JSON commands.

The user will provide code with line numbers like this:
1: using System;
2: namespace Example
3: {
4:     class Program
5:     {

You must respond with ONLY valid JSON in this exact format:
{
  ""commands"": [
    {""type"": ""insertLine"", ""line"": 5, ""text"": ""// New comment""},
    {""type"": ""replaceLine"", ""line"": 10, ""text"": ""int x = 5;""},
    {""type"": ""replaceLineRange"", ""startLine"": 15, ""endLine"": 18, ""text"": ""new code\nmore code""},
    {""type"": ""deleteLine"", ""line"": 20},
    {""type"": ""deleteLineRange"", ""startLine"": 25, ""endLine"": 30},
    {""type"": ""highlightLineRange"", ""startLine"": 5, ""endLine"": 10},
    {""type"": ""clearHighlight""},
    {""type"": ""toggleBookmark"", ""line"": 15}
  ]
}

Available commands:
- insertLine: Insert new line at position (line number, text - use \n for multiple lines)
- replaceLine: Replace single line content (line number, text)
- replaceLineRange: Replace multiple lines (startLine, endLine, text - use \n for multiple lines)
- deleteLine: Delete single line (line number)
- deleteLineRange: Delete multiple lines (startLine, endLine - both inclusive)
- highlightLineRange: Highlight a range of lines for visual emphasis (startLine, endLine)
- clearHighlight: Clear all highlights (no parameters)
- toggleBookmark: Toggle bookmark on a specific line (line number)

CRITICAL RULES:
1. Line numbers are 1-based (first line is 1, not 0)
2. For insertLine and replaceLineRange, use \n in the ""text"" field to insert/replace multiple lines
3. NEVER use multiple insertLine commands at the same line - use ONE insertLine with \n instead
4. startLine and endLine are INCLUSIVE (both lines are included in the range)
5. Respond ONLY with valid JSON - no markdown, no explanations, no code blocks
6. If no changes are needed, return: {""commands"": []}
7. Do not include the line numbers in the ""text"" field
8. Preserve the exact indentation and formatting of the code

Example valid responses:

Single command:
{""commands"": [{""type"": ""replaceLine"", ""line"": 5, ""text"": ""    int result = 0;""}]}

Multiple commands:
{""commands"": [{""type"": ""deleteLine"", ""line"": 10}, {""type"": ""insertLine"", ""line"": 5, ""text"": ""// TODO: Fix this""}]}

No changes:
{""commands"": []}";
    }

    /// <summary>
    /// Build a complete message request for the AI
    /// </summary>
    public MessageRequest BuildRequest(string codeWithLineNumbers, string userInstruction, string apiKey)
    {
        var request = new MessageRequest
        {
            Model = ClaudeModels.Claude45Sonnet,
            MaxTokens = 4096,
            System = BuildSystemPrompt(),
            Messages = new List<Message>
            {
                Message.CreateUserMessage($@"Here is the code:

{codeWithLineNumbers}

User instruction: {userInstruction}

Respond with JSON commands only.")
            }
        };

        return request;
    }
}
