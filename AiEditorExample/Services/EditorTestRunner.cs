using AiEditorExample.Models;
using MonacoEditor;

namespace AiEditorExample.Services;

/// <summary>
/// Result of a single test case
/// </summary>
public record TestCaseResult(string Name, bool Passed, string Expected, string Actual);

/// <summary>
/// Runs a battery of functional tests against the Monaco Editor command system.
/// Tests create their own tabs, apply edits, read back content from the live editor window,
/// and compare against expected text. No AI API key is required.
/// </summary>
public class EditorTestRunner
{
    private int _runCounter = 0;

    /// <summary>
    /// Runs all test cases and returns one result per test.
    /// Each test creates its own tab so results are visible in the UI.
    /// </summary>
    public async Task<List<TestCaseResult>> RunAllTestsAsync(MonacoEditorManager manager)
    {
        _runCounter++;
        var suffix = _runCounter > 1 ? $" ({_runCounter})" : "";
        var results = new List<TestCaseResult>();

        // Test 1: SetValueAsync round-trip
        results.Add(await RunTestAsync(manager, $"⚗ SetValue{suffix}",
            initialContent: null,
            applyEdits: async editor =>
            {
                await editor.SetValueAsync("Hello\nWorld");
            },
            expected: "Hello\nWorld"));

        // Test 2: InsertLine inserts at the correct position
        results.Add(await RunTestAsync(manager, $"⚗ InsertLine{suffix}",
            initialContent: "line1\nline2\nline3",
            applyEdits: async editor =>
            {
                await new InsertLineCommand { Line = 2, Text = "inserted" }.ExecuteAsync(editor);
            },
            expected: "line1\ninserted\nline2\nline3"));

        // Test 3: ReplaceLine replaces a single line
        results.Add(await RunTestAsync(manager, $"⚗ ReplaceLine{suffix}",
            initialContent: "line1\nline2\nline3",
            applyEdits: async editor =>
            {
                await new ReplaceLineCommand { Line = 2, Text = "replaced" }.ExecuteAsync(editor);
            },
            expected: "line1\nreplaced\nline3"));

        // Test 4: ReplaceLineRange replaces a range of lines with new content
        results.Add(await RunTestAsync(manager, $"⚗ ReplaceRange{suffix}",
            initialContent: "line1\nline2\nline3\nline4",
            applyEdits: async editor =>
            {
                await new ReplaceLineRangeCommand { StartLine = 2, EndLine = 3, Text = "newline" }.ExecuteAsync(editor);
            },
            expected: "line1\nnewline\nline4"));

        // Test 5: DeleteLine removes a single line
        results.Add(await RunTestAsync(manager, $"⚗ DeleteLine{suffix}",
            initialContent: "line1\nline2\nline3",
            applyEdits: async editor =>
            {
                await new DeleteLineCommand { Line = 2 }.ExecuteAsync(editor);
            },
            expected: "line1\nline3"));

        // Test 6: DeleteLineRange removes a range of lines
        results.Add(await RunTestAsync(manager, $"⚗ DeleteRange{suffix}",
            initialContent: "line1\nline2\nline3\nline4",
            applyEdits: async editor =>
            {
                await new DeleteLineRangeCommand { StartLine = 2, EndLine = 3 }.ExecuteAsync(editor);
            },
            expected: "line1\nline4"));

        // Test 7: Editing one editor does not affect another
        results.Add(await RunIsolationTestAsync(manager, $"⚗ Independence{suffix}"));

        return results;
    }

    private async Task<TestCaseResult> RunTestAsync(
        MonacoEditorManager manager,
        string testName,
        string? initialContent,
        Func<MonacoEditorService, Task> applyEdits,
        string expected)
    {
        try
        {
            var editor = await manager.CreateEditorAsync(testName, initialContent ?? "", "plaintext");

            // If initialContent was supplied separately (e.g., SetValue test), apply it now
            if (initialContent == null)
            {
                // applyEdits will set content itself
            }

            await applyEdits(editor);

            var actual = await editor.GetAllTextAsync();
            var normalizedActual = Normalize(actual);
            var normalizedExpected = Normalize(expected);
            var passed = normalizedActual == normalizedExpected;

            return new TestCaseResult(testName, passed, normalizedExpected, normalizedActual);
        }
        catch (Exception ex)
        {
            return new TestCaseResult(testName, false, expected, $"EXCEPTION: {ex.Message}");
        }
    }

    private async Task<TestCaseResult> RunIsolationTestAsync(MonacoEditorManager manager, string testName)
    {
        const string initialA = "aaa";
        const string initialB = "bbb";
        const string expectedA = "AAA";
        const string expectedB = "bbb"; // B must be unchanged

        try
        {
            var editorA = await manager.CreateEditorAsync($"{testName}-A", initialA, "plaintext");
            var editorB = await manager.CreateEditorAsync($"{testName}-B", initialB, "plaintext");

            // Edit only editor A
            await new ReplaceLineCommand { Line = 1, Text = "AAA" }.ExecuteAsync(editorA);

            var actualA = Normalize(await editorA.GetAllTextAsync());
            var actualB = Normalize(await editorB.GetAllTextAsync());

            var passed = actualA == expectedA && actualB == expectedB;
            var actual = $"A={actualA}, B={actualB}";
            var expected = $"A={expectedA}, B={expectedB}";

            return new TestCaseResult(testName, passed, expected, actual);
        }
        catch (Exception ex)
        {
            return new TestCaseResult(testName, false, $"A={expectedA}, B={expectedB}", $"EXCEPTION: {ex.Message}");
        }
    }

    /// <summary>
    /// Normalizes text for comparison: strips \r, trims trailing whitespace per line.
    /// </summary>
    private static string Normalize(string text)
    {
        if (string.IsNullOrEmpty(text)) return string.Empty;

        var lines = text.Replace("\r\n", "\n").Replace("\r", "\n").Split('\n');
        var trimmed = lines.Select(l => l.TrimEnd()).ToArray();

        // Remove trailing empty lines
        int last = trimmed.Length - 1;
        while (last > 0 && trimmed[last].Length == 0)
            last--;

        return string.Join("\n", trimmed[..(last + 1)]);
    }
}
