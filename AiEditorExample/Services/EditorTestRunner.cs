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
        TestCaseResult result;
        try
        {
            var editor = await manager.CreateEditorAsync(testName, initialContent ?? "", "plaintext");
            await applyEdits(editor);

            var actual = await editor.GetAllTextAsync();
            var normalizedActual = Normalize(actual);
            var normalizedExpected = Normalize(expected);
            var passed = normalizedActual == normalizedExpected;

            result = new TestCaseResult(testName, passed, normalizedExpected, normalizedActual);
        }
        catch (Exception ex)
        {
            result = new TestCaseResult(testName, false, expected, $"EXCEPTION: {ex.Message}");
        }
        finally
        {
            manager.RemoveEditor(testName);
        }
        return result;
    }

    private async Task<TestCaseResult> RunIsolationTestAsync(MonacoEditorManager manager, string testName)
    {
        const string initialA = "aaa";
        const string initialB = "bbb";
        const string expectedA = "AAA";
        const string expectedB = "bbb"; // B must be unchanged

        TestCaseResult result;
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

            result = new TestCaseResult(testName, passed, expected, actual);
        }
        catch (Exception ex)
        {
            result = new TestCaseResult(testName, false, $"A={expectedA}, B={expectedB}", $"EXCEPTION: {ex.Message}");
        }
        finally
        {
            manager.RemoveEditor($"{testName}-A");
            manager.RemoveEditor($"{testName}-B");
        }
        return result;
    }

    /// <summary>
    /// Simulates a realistic AI editing session by feeding raw JSON responses through the
    /// CommandProcessor, exactly as the real AI flow does. Runs on a single tab with a
    /// delay between turns so the edits are visible. Returns one result per AI turn.
    /// </summary>
    public async Task<List<TestCaseResult>> RunAiSimulationTestAsync(
        MonacoEditorManager manager,
        TimeSpan? stepDelay = null)
    {
        var delay = stepDelay ?? TimeSpan.FromMilliseconds(900);
        var results = new List<TestCaseResult>();
        var processor = new CommandProcessor();

        const string tabName = "⚗ AI Sim";
        manager.RemoveEditor(tabName);

        // Initial code: a Calculator with a missing semicolon and a stale TODO
        const string initial =
            "public class Calculator\n" +
            "{\n" +
            "    public int Add(int a, int b)\n" +
            "    {\n" +
            "        // TODO: validate inputs\n" +
            "        return a + b\n" +
            "    }\n" +
            "\n" +
            "    public int Multiply(int a, int b)\n" +
            "    {\n" +
            "        return a * b;\n" +
            "    }\n" +
            "}";

        var editor = await manager.CreateEditorAsync(tabName, initial, "csharp");

        async Task<TestCaseResult> Turn(string name, string json, string expected)
        {
            try
            {
                var cmdResult = await processor.ProcessCommandsAsync(json, editor);
                await Task.Delay(delay);

                if (!cmdResult.Success)
                    return new TestCaseResult(name, false, expected, $"COMMANDS FAILED: {cmdResult.Message}");

                var actual = Normalize(await editor.GetAllTextAsync());
                var exp = Normalize(expected);
                return new TestCaseResult(name, actual == exp, exp, actual);
            }
            catch (Exception ex)
            {
                return new TestCaseResult(name, false, expected, $"EXCEPTION: {ex.Message}");
            }
        }

        // Turn 1 – Fix the syntax error and remove the TODO in one response (multi-command,
        //          bottom-to-top sort means line 6 is replaced before line 5 is deleted)
        results.Add(await Turn("Fix syntax & remove TODO",
            """{"commands":[{"type":"replaceLine","line":6,"text":"        return a + b;"},{"type":"deleteLine","line":5}]}""",
            "public class Calculator\n" +
            "{\n" +
            "    public int Add(int a, int b)\n" +
            "    {\n" +
            "        return a + b;\n" +
            "    }\n" +
            "\n" +
            "    public int Multiply(int a, int b)\n" +
            "    {\n" +
            "        return a * b;\n" +
            "    }\n" +
            "}"));

        // Turn 2 – Insert input validation into Add() before the return
        results.Add(await Turn("Add validation to Add()",
            """{"commands":[{"type":"insertLine","line":5,"text":"        if (a < 0 || b < 0) throw new ArgumentException();"}]}""",
            "public class Calculator\n" +
            "{\n" +
            "    public int Add(int a, int b)\n" +
            "    {\n" +
            "        if (a < 0 || b < 0) throw new ArgumentException();\n" +
            "        return a + b;\n" +
            "    }\n" +
            "\n" +
            "    public int Multiply(int a, int b)\n" +
            "    {\n" +
            "        return a * b;\n" +
            "    }\n" +
            "}"));

        // Turn 3 – Rewrite Multiply() with the same validation (replaceLineRange)
        results.Add(await Turn("Add validation to Multiply()",
            """{"commands":[{"type":"replaceLineRange","startLine":9,"endLine":12,"text":"    public int Multiply(int a, int b)\n    {\n        if (a < 0 || b < 0) throw new ArgumentException();\n        return a * b;\n    }"}]}""",
            "public class Calculator\n" +
            "{\n" +
            "    public int Add(int a, int b)\n" +
            "    {\n" +
            "        if (a < 0 || b < 0) throw new ArgumentException();\n" +
            "        return a + b;\n" +
            "    }\n" +
            "\n" +
            "    public int Multiply(int a, int b)\n" +
            "    {\n" +
            "        if (a < 0 || b < 0) throw new ArgumentException();\n" +
            "        return a * b;\n" +
            "    }\n" +
            "}"));

        // Turn 4 – Highlight both new validation lines and bookmark them (no text change)
        const string afterTurn3 =
            "public class Calculator\n" +
            "{\n" +
            "    public int Add(int a, int b)\n" +
            "    {\n" +
            "        if (a < 0 || b < 0) throw new ArgumentException();\n" +
            "        return a + b;\n" +
            "    }\n" +
            "\n" +
            "    public int Multiply(int a, int b)\n" +
            "    {\n" +
            "        if (a < 0 || b < 0) throw new ArgumentException();\n" +
            "        return a * b;\n" +
            "    }\n" +
            "}";

        results.Add(await Turn("Highlight & bookmark new lines",
            """{"commands":[{"type":"highlightLineRange","startLine":5,"endLine":5},{"type":"highlightLineRange","startLine":11,"endLine":11},{"type":"toggleBookmark","line":5},{"type":"toggleBookmark","line":11}]}""",
            afterTurn3));

        // Turn 5 – Clear all highlights (no text change)
        results.Add(await Turn("Clear highlights",
            """{"commands":[{"type":"clearHighlight"}]}""",
            afterTurn3));

        return results;
    }

    /// <summary>
    /// Runs all edit operations sequentially on a single editor so you can watch each
    /// change happen. A short delay is inserted between steps for visibility.
    /// Returns one result per step.
    /// </summary>
    public async Task<List<TestCaseResult>> RunWatchableTestAsync(
        MonacoEditorManager manager,
        TimeSpan? stepDelay = null)
    {
        var delay = stepDelay ?? TimeSpan.FromMilliseconds(800);
        var results = new List<TestCaseResult>();

        _runCounter++;
        var suffix = _runCounter > 1 ? $" ({_runCounter})" : "";
        var tabName = $"⚗ Live{suffix}";

        manager.RemoveEditor(tabName); // close any previous run's tab
        var editor = await manager.CreateEditorAsync(tabName, "", "javascript");

        async Task<TestCaseResult> Step(string name, Func<Task> action, string expected)
        {
            try
            {
                await action();
                await Task.Delay(delay);
                var actual = Normalize(await editor.GetAllTextAsync());
                var exp = Normalize(expected);
                return new TestCaseResult(name, actual == exp, exp, actual);
            }
            catch (Exception ex)
            {
                return new TestCaseResult(name, false, expected, $"EXCEPTION: {ex.Message}");
            }
        }

        const string initial =
            "// Math utilities\n" +
            "function add(a, b) {\n" +
            "    return a + b;\n" +
            "}\n" +
            "\n" +
            "function multiply(a, b) {\n" +
            "    return a * b;\n" +
            "}";

        // Step 1 – SetValue: load the initial file
        results.Add(await Step("SetValue",
            () => editor.SetValueAsync(initial),
            initial));

        // Step 2 – InsertLine: add a TODO comment inside add(), before the return
        results.Add(await Step("InsertLine",
            () => new InsertLineCommand { Line = 3, Text = "    // TODO: add overflow check" }.ExecuteAsync(editor),
            "// Math utilities\n" +
            "function add(a, b) {\n" +
            "    // TODO: add overflow check\n" +
            "    return a + b;\n" +
            "}\n" +
            "\n" +
            "function multiply(a, b) {\n" +
            "    return a * b;\n" +
            "}"));

        // Step 3 – ReplaceLine: annotate multiply's return statement
        results.Add(await Step("ReplaceLine",
            () => new ReplaceLineCommand { Line = 8, Text = "    return a * b; // product" }.ExecuteAsync(editor),
            "// Math utilities\n" +
            "function add(a, b) {\n" +
            "    // TODO: add overflow check\n" +
            "    return a + b;\n" +
            "}\n" +
            "\n" +
            "function multiply(a, b) {\n" +
            "    return a * b; // product\n" +
            "}"));

        // Step 4 – ReplaceRange: rewrite multiply as subtract
        results.Add(await Step("ReplaceRange",
            () => new ReplaceLineRangeCommand { StartLine = 7, EndLine = 9, Text = "function subtract(a, b) {\n    return a - b;\n}" }.ExecuteAsync(editor),
            "// Math utilities\n" +
            "function add(a, b) {\n" +
            "    // TODO: add overflow check\n" +
            "    return a + b;\n" +
            "}\n" +
            "\n" +
            "function subtract(a, b) {\n" +
            "    return a - b;\n" +
            "}"));

        // Step 5 – DeleteLine: remove the TODO (decided it's not needed)
        results.Add(await Step("DeleteLine",
            () => new DeleteLineCommand { Line = 3 }.ExecuteAsync(editor),
            "// Math utilities\n" +
            "function add(a, b) {\n" +
            "    return a + b;\n" +
            "}\n" +
            "\n" +
            "function subtract(a, b) {\n" +
            "    return a - b;\n" +
            "}"));

        // Step 6 – DeleteRange: drop the blank line + subtract, keep only add
        results.Add(await Step("DeleteRange",
            () => new DeleteLineRangeCommand { StartLine = 5, EndLine = 8 }.ExecuteAsync(editor),
            "// Math utilities\n" +
            "function add(a, b) {\n" +
            "    return a + b;\n" +
            "}"));

        return results;
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
