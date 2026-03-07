using Microsoft.Web.WebView2.WinForms;
using System.Windows.Forms;

namespace MonacoEditor
{
    /// <summary>
    /// Manages multiple Monaco Editor instances hosted in a TabControl.
    /// Each tab contains an independent Monaco Editor with its own WebView2 control.
    /// </summary>
    public sealed class MonacoEditorManager : IDisposable
    {
        private readonly TabControl _tabControl;
        private readonly string _appDirectory;
        private readonly Dictionary<string, (MonacoEditorService Service, WebView2 WebView, TabPage Tab)> _editors = new();
        private int _editorCounter = 0;
        private bool _disposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="MonacoEditorManager"/> class.
        /// </summary>
        /// <param name="tabControl">The TabControl that will host editor tabs.</param>
        /// <param name="appDirectory">The application directory where Monaco Editor files are located.</param>
        public MonacoEditorManager(TabControl tabControl, string appDirectory)
        {
            _tabControl = tabControl ?? throw new ArgumentNullException(nameof(tabControl));
            _appDirectory = appDirectory ?? throw new ArgumentNullException(nameof(appDirectory));
        }

        /// <summary>
        /// Creates a new Monaco Editor instance in a new tab.
        /// </summary>
        /// <param name="name">The display name for the tab and the editor identifier.</param>
        /// <param name="initialCode">The initial code content for the editor.</param>
        /// <param name="language">The programming language for syntax highlighting (e.g., "csharp", "javascript").</param>
        /// <returns>The initialized <see cref="MonacoEditorService"/> for the new editor.</returns>
        public async Task<MonacoEditorService> CreateEditorAsync(string name, string initialCode = "", string language = "plaintext")
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Editor name cannot be empty.", nameof(name));

            if (_editors.ContainsKey(name))
                throw new InvalidOperationException($"An editor named '{name}' already exists.");

            var editorId = $"editor{++_editorCounter}";

            // Create a new tab page
            var tabPage = new TabPage(name);

            // Create a WebView2 control that fills the tab
            var webView = new WebView2
            {
                Dock = DockStyle.Fill
            };
            tabPage.Controls.Add(webView);

            // Add the tab and select it
            _tabControl.TabPages.Add(tabPage);
            _tabControl.SelectedTab = tabPage;

            // Create and initialize the editor service
            var service = new MonacoEditorService(webView);
            await service.InitializeAsync(_appDirectory, initialCode, language, editorId: editorId);
            await service.EditorReady;

            _editors[name] = (service, webView, tabPage);

            return service;
        }

        /// <summary>
        /// Gets the Monaco Editor service for the currently selected tab.
        /// </summary>
        /// <returns>The active <see cref="MonacoEditorService"/>, or <c>null</c> if no tab is selected.</returns>
        public MonacoEditorService? GetActiveEditor()
        {
            var selectedTab = _tabControl.SelectedTab;
            if (selectedTab == null) return null;

            foreach (var entry in _editors.Values)
            {
                if (entry.Tab == selectedTab)
                    return entry.Service;
            }

            return null;
        }

        /// <summary>
        /// Gets the Monaco Editor service for a named editor.
        /// </summary>
        /// <param name="name">The name of the editor to retrieve.</param>
        /// <returns>The <see cref="MonacoEditorService"/> for the specified editor, or <c>null</c> if not found.</returns>
        public MonacoEditorService? GetEditor(string name)
        {
            return _editors.TryGetValue(name, out var entry) ? entry.Service : null;
        }

        /// <summary>
        /// Gets the name of the currently active editor tab.
        /// </summary>
        /// <returns>The name of the active editor, or <c>null</c> if no tab is selected.</returns>
        public string? GetActiveEditorName()
        {
            var selectedTab = _tabControl.SelectedTab;
            if (selectedTab == null) return null;

            foreach (var kvp in _editors)
            {
                if (kvp.Value.Tab == selectedTab)
                    return kvp.Key;
            }

            return null;
        }

        /// <summary>
        /// Gets the names of all registered editors.
        /// </summary>
        public IReadOnlyList<string> GetEditorNames() => _editors.Keys.ToList().AsReadOnly();

        /// <summary>
        /// Gets the number of open editors.
        /// </summary>
        public int Count => _editors.Count;

        /// <summary>
        /// Removes the named editor and its tab from the TabControl.
        /// </summary>
        /// <param name="name">The name of the editor to remove.</param>
        /// <returns><c>true</c> if the editor was found and removed; otherwise <c>false</c>.</returns>
        public bool RemoveEditor(string name)
        {
            if (!_editors.TryGetValue(name, out var entry))
                return false;

            _tabControl.TabPages.Remove(entry.Tab);
            entry.Service.Dispose();
            entry.WebView.Dispose();
            entry.Tab.Dispose();
            _editors.Remove(name);

            return true;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;

            foreach (var entry in _editors.Values)
            {
                entry.Service.Dispose();
                entry.WebView.Dispose();
                entry.Tab.Dispose();
            }

            _editors.Clear();
        }
    }
}
