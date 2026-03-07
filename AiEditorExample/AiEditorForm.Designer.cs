namespace AiEditorExample
{
    partial class AiEditorForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TabControl tabEditors;
        private Panel panelSidebar;
        private GroupBox groupEditor;
        private GroupBox groupTests;
        private GroupBox groupAi;
        private GroupBox groupLog;
        private Button btnNewEditor;
        private Button btnLoadFile;
        private Button btnSaveFile;
        private Button btnRunTests;
        private Button btnWatchTest;
        private Button btnAiSimTest;
        private Label lblApiKey;
        private TextBox txtApiKey;
        private Label lblInstruction;
        private TextBox txtInstruction;
        private Button btnSendToAi;
        private TextBox txtCommandLog;
        private Button btnClearLog;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel lblStatus;

        private void InitializeComponent()
        {
            this.tabEditors       = new System.Windows.Forms.TabControl();
            this.panelSidebar     = new System.Windows.Forms.Panel();
            this.groupEditor      = new System.Windows.Forms.GroupBox();
            this.groupTests       = new System.Windows.Forms.GroupBox();
            this.groupAi          = new System.Windows.Forms.GroupBox();
            this.groupLog         = new System.Windows.Forms.GroupBox();
            this.btnNewEditor     = new System.Windows.Forms.Button();
            this.btnLoadFile      = new System.Windows.Forms.Button();
            this.btnSaveFile      = new System.Windows.Forms.Button();
            this.btnRunTests      = new System.Windows.Forms.Button();
            this.btnWatchTest     = new System.Windows.Forms.Button();
            this.btnAiSimTest     = new System.Windows.Forms.Button();
            this.lblApiKey        = new System.Windows.Forms.Label();
            this.txtApiKey        = new System.Windows.Forms.TextBox();
            this.lblInstruction   = new System.Windows.Forms.Label();
            this.txtInstruction   = new System.Windows.Forms.TextBox();
            this.btnSendToAi      = new System.Windows.Forms.Button();
            this.txtCommandLog    = new System.Windows.Forms.TextBox();
            this.btnClearLog      = new System.Windows.Forms.Button();
            this.statusStrip      = new System.Windows.Forms.StatusStrip();
            this.lblStatus        = new System.Windows.Forms.ToolStripStatusLabel();

            this.panelSidebar.SuspendLayout();
            this.groupEditor.SuspendLayout();
            this.groupTests.SuspendLayout();
            this.groupAi.SuspendLayout();
            this.groupLog.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();

            // ── tabEditors ───────────────────────────────────────────────
            this.tabEditors.Dock          = System.Windows.Forms.DockStyle.Fill;
            this.tabEditors.Location      = new System.Drawing.Point(0, 0);
            this.tabEditors.Name          = "tabEditors";
            this.tabEditors.SelectedIndex = 0;
            this.tabEditors.Size          = new System.Drawing.Size(784, 639);
            this.tabEditors.TabIndex      = 0;

            // ── panelSidebar ──────────────────────────────────────────────
            this.panelSidebar.BackColor = System.Drawing.Color.FromArgb(37, 37, 38);
            this.panelSidebar.Controls.Add(this.groupLog);
            this.panelSidebar.Controls.Add(this.groupAi);
            this.panelSidebar.Controls.Add(this.groupTests);
            this.panelSidebar.Controls.Add(this.groupEditor);
            this.panelSidebar.Dock     = System.Windows.Forms.DockStyle.Right;
            this.panelSidebar.Name     = "panelSidebar";
            this.panelSidebar.Padding  = new System.Windows.Forms.Padding(10, 10, 10, 10);
            this.panelSidebar.Size     = new System.Drawing.Size(400, 639);
            this.panelSidebar.TabIndex = 1;

            // ── groupEditor ───────────────────────────────────────────────
            this.groupEditor.Controls.Add(this.btnSaveFile);
            this.groupEditor.Controls.Add(this.btnLoadFile);
            this.groupEditor.Controls.Add(this.btnNewEditor);
            this.groupEditor.Dock      = System.Windows.Forms.DockStyle.Top;
            this.groupEditor.ForeColor = System.Drawing.Color.FromArgb(180, 180, 180);
            this.groupEditor.Name      = "groupEditor";
            this.groupEditor.Padding   = new System.Windows.Forms.Padding(8, 6, 8, 8);
            this.groupEditor.Size      = new System.Drawing.Size(380, 130);
            this.groupEditor.TabIndex  = 0;
            this.groupEditor.TabStop   = false;
            this.groupEditor.Text      = "Editor";

            // ── groupTests ────────────────────────────────────────────────
            this.groupTests.Controls.Add(this.btnAiSimTest);
            this.groupTests.Controls.Add(this.btnWatchTest);
            this.groupTests.Controls.Add(this.btnRunTests);
            this.groupTests.Dock      = System.Windows.Forms.DockStyle.Top;
            this.groupTests.ForeColor = System.Drawing.Color.FromArgb(180, 180, 180);
            this.groupTests.Name      = "groupTests";
            this.groupTests.Padding   = new System.Windows.Forms.Padding(8, 6, 8, 8);
            this.groupTests.Size      = new System.Drawing.Size(380, 130);
            this.groupTests.TabIndex  = 1;
            this.groupTests.TabStop   = false;
            this.groupTests.Text      = "Tests";

            // ── groupAi ───────────────────────────────────────────────────
            this.groupAi.Controls.Add(this.btnSendToAi);
            this.groupAi.Controls.Add(this.txtInstruction);
            this.groupAi.Controls.Add(this.lblInstruction);
            this.groupAi.Controls.Add(this.txtApiKey);
            this.groupAi.Controls.Add(this.lblApiKey);
            this.groupAi.Dock      = System.Windows.Forms.DockStyle.Top;
            this.groupAi.ForeColor = System.Drawing.Color.FromArgb(180, 180, 180);
            this.groupAi.Name      = "groupAi";
            this.groupAi.Padding   = new System.Windows.Forms.Padding(8, 6, 8, 8);
            this.groupAi.Size      = new System.Drawing.Size(380, 228);
            this.groupAi.TabIndex  = 2;
            this.groupAi.TabStop   = false;
            this.groupAi.Text      = "AI Assistant";

            // ── groupLog ──────────────────────────────────────────────────
            this.groupLog.Controls.Add(this.btnClearLog);
            this.groupLog.Controls.Add(this.txtCommandLog);
            this.groupLog.Dock      = System.Windows.Forms.DockStyle.Fill;
            this.groupLog.ForeColor = System.Drawing.Color.FromArgb(180, 180, 180);
            this.groupLog.Name      = "groupLog";
            this.groupLog.Padding   = new System.Windows.Forms.Padding(8, 6, 8, 8);
            this.groupLog.Size      = new System.Drawing.Size(380, 151);
            this.groupLog.TabIndex  = 3;
            this.groupLog.TabStop   = false;
            this.groupLog.Text      = "Command Log";

            // ── Shared button style helper (applied individually below) ──
            //    Height = 27, FlatStyle = Flat, BorderSize = 0, ForeColor = White

            // ── btnNewEditor ──────────────────────────────────────────────
            this.btnNewEditor.BackColor                  = System.Drawing.Color.FromArgb(16, 124, 16);
            this.btnNewEditor.FlatAppearance.BorderSize  = 0;
            this.btnNewEditor.FlatStyle                  = System.Windows.Forms.FlatStyle.Flat;
            this.btnNewEditor.ForeColor                  = System.Drawing.Color.White;
            this.btnNewEditor.Location                   = new System.Drawing.Point(11, 27);
            this.btnNewEditor.Name                       = "btnNewEditor";
            this.btnNewEditor.Size                       = new System.Drawing.Size(358, 27);
            this.btnNewEditor.TabIndex                   = 0;
            this.btnNewEditor.Text                       = "+ New Editor";
            this.btnNewEditor.UseVisualStyleBackColor    = false;
            this.btnNewEditor.Click += new System.EventHandler(this.btnNewEditor_Click);

            // ── btnLoadFile ───────────────────────────────────────────────
            this.btnLoadFile.BackColor                   = System.Drawing.Color.FromArgb(0, 99, 177);
            this.btnLoadFile.FlatAppearance.BorderSize   = 0;
            this.btnLoadFile.FlatStyle                   = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoadFile.ForeColor                   = System.Drawing.Color.White;
            this.btnLoadFile.Location                    = new System.Drawing.Point(11, 60);
            this.btnLoadFile.Name                        = "btnLoadFile";
            this.btnLoadFile.Size                        = new System.Drawing.Size(358, 27);
            this.btnLoadFile.TabIndex                    = 1;
            this.btnLoadFile.Text                        = "Load File";
            this.btnLoadFile.UseVisualStyleBackColor     = false;
            this.btnLoadFile.Click += new System.EventHandler(this.btnLoadFile_Click);

            // ── btnSaveFile ───────────────────────────────────────────────
            this.btnSaveFile.BackColor                   = System.Drawing.Color.FromArgb(0, 99, 177);
            this.btnSaveFile.FlatAppearance.BorderSize   = 0;
            this.btnSaveFile.FlatStyle                   = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveFile.ForeColor                   = System.Drawing.Color.White;
            this.btnSaveFile.Location                    = new System.Drawing.Point(11, 93);
            this.btnSaveFile.Name                        = "btnSaveFile";
            this.btnSaveFile.Size                        = new System.Drawing.Size(358, 27);
            this.btnSaveFile.TabIndex                    = 2;
            this.btnSaveFile.Text                        = "Save File";
            this.btnSaveFile.UseVisualStyleBackColor     = false;
            this.btnSaveFile.Click += new System.EventHandler(this.btnSaveFile_Click);

            // ── btnRunTests ───────────────────────────────────────────────
            this.btnRunTests.BackColor                   = System.Drawing.Color.FromArgb(148, 108, 0);
            this.btnRunTests.FlatAppearance.BorderSize   = 0;
            this.btnRunTests.FlatStyle                   = System.Windows.Forms.FlatStyle.Flat;
            this.btnRunTests.ForeColor                   = System.Drawing.Color.White;
            this.btnRunTests.Location                    = new System.Drawing.Point(11, 27);
            this.btnRunTests.Name                        = "btnRunTests";
            this.btnRunTests.Size                        = new System.Drawing.Size(358, 27);
            this.btnRunTests.TabIndex                    = 3;
            this.btnRunTests.Text                        = "Run Editor Tests";
            this.btnRunTests.UseVisualStyleBackColor     = false;
            this.btnRunTests.Click += new System.EventHandler(this.btnRunTests_Click);

            // ── btnWatchTest ──────────────────────────────────────────────
            this.btnWatchTest.BackColor                  = System.Drawing.Color.FromArgb(100, 48, 140);
            this.btnWatchTest.FlatAppearance.BorderSize  = 0;
            this.btnWatchTest.FlatStyle                  = System.Windows.Forms.FlatStyle.Flat;
            this.btnWatchTest.ForeColor                  = System.Drawing.Color.White;
            this.btnWatchTest.Location                   = new System.Drawing.Point(11, 60);
            this.btnWatchTest.Name                       = "btnWatchTest";
            this.btnWatchTest.Size                       = new System.Drawing.Size(358, 27);
            this.btnWatchTest.TabIndex                   = 4;
            this.btnWatchTest.Text                       = "Watch Live Test";
            this.btnWatchTest.UseVisualStyleBackColor    = false;
            this.btnWatchTest.Click += new System.EventHandler(this.btnWatchTest_Click);

            // ── btnAiSimTest ──────────────────────────────────────────────
            this.btnAiSimTest.BackColor                  = System.Drawing.Color.FromArgb(0, 106, 127);
            this.btnAiSimTest.FlatAppearance.BorderSize  = 0;
            this.btnAiSimTest.FlatStyle                  = System.Windows.Forms.FlatStyle.Flat;
            this.btnAiSimTest.ForeColor                  = System.Drawing.Color.White;
            this.btnAiSimTest.Location                   = new System.Drawing.Point(11, 93);
            this.btnAiSimTest.Name                       = "btnAiSimTest";
            this.btnAiSimTest.Size                       = new System.Drawing.Size(358, 27);
            this.btnAiSimTest.TabIndex                   = 5;
            this.btnAiSimTest.Text                       = "Simulate AI Commands";
            this.btnAiSimTest.UseVisualStyleBackColor    = false;
            this.btnAiSimTest.Click += new System.EventHandler(this.btnAiSimTest_Click);

            // ── lblApiKey ─────────────────────────────────────────────────
            this.lblApiKey.AutoSize  = true;
            this.lblApiKey.ForeColor = System.Drawing.Color.FromArgb(160, 160, 160);
            this.lblApiKey.Location  = new System.Drawing.Point(11, 27);
            this.lblApiKey.Name      = "lblApiKey";
            this.lblApiKey.TabIndex  = 0;
            this.lblApiKey.Text      = "Anthropic API Key";

            // ── txtApiKey ─────────────────────────────────────────────────
            this.txtApiKey.BackColor    = System.Drawing.Color.FromArgb(30, 30, 30);
            this.txtApiKey.BorderStyle  = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtApiKey.ForeColor    = System.Drawing.Color.FromArgb(204, 204, 204);
            this.txtApiKey.Location     = new System.Drawing.Point(11, 45);
            this.txtApiKey.Name         = "txtApiKey";
            this.txtApiKey.PasswordChar = '*';
            this.txtApiKey.Size         = new System.Drawing.Size(358, 23);
            this.txtApiKey.TabIndex     = 1;

            // ── lblInstruction ────────────────────────────────────────────
            this.lblInstruction.AutoSize  = true;
            this.lblInstruction.ForeColor = System.Drawing.Color.FromArgb(160, 160, 160);
            this.lblInstruction.Location  = new System.Drawing.Point(11, 82);
            this.lblInstruction.Name      = "lblInstruction";
            this.lblInstruction.TabIndex  = 2;
            this.lblInstruction.Text      = "Instruction";

            // ── txtInstruction ────────────────────────────────────────────
            this.txtInstruction.BackColor   = System.Drawing.Color.FromArgb(30, 30, 30);
            this.txtInstruction.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtInstruction.ForeColor   = System.Drawing.Color.FromArgb(204, 204, 204);
            this.txtInstruction.Location    = new System.Drawing.Point(11, 100);
            this.txtInstruction.Multiline   = true;
            this.txtInstruction.Name        = "txtInstruction";
            this.txtInstruction.ScrollBars  = System.Windows.Forms.ScrollBars.Vertical;
            this.txtInstruction.Size        = new System.Drawing.Size(358, 76);
            this.txtInstruction.TabIndex    = 3;

            // ── btnSendToAi ───────────────────────────────────────────────
            this.btnSendToAi.BackColor                  = System.Drawing.Color.FromArgb(0, 99, 177);
            this.btnSendToAi.Enabled                    = false;
            this.btnSendToAi.FlatAppearance.BorderSize  = 0;
            this.btnSendToAi.FlatStyle                  = System.Windows.Forms.FlatStyle.Flat;
            this.btnSendToAi.Font                       = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnSendToAi.ForeColor                  = System.Drawing.Color.White;
            this.btnSendToAi.Location                   = new System.Drawing.Point(11, 188);
            this.btnSendToAi.Name                       = "btnSendToAi";
            this.btnSendToAi.Size                       = new System.Drawing.Size(358, 30);
            this.btnSendToAi.TabIndex                   = 4;
            this.btnSendToAi.Text                       = "Send to AI";
            this.btnSendToAi.UseVisualStyleBackColor    = false;
            this.btnSendToAi.Click += new System.EventHandler(this.btnSendToAi_Click);

            // ── txtCommandLog ─────────────────────────────────────────────
            this.txtCommandLog.BackColor    = System.Drawing.Color.FromArgb(20, 20, 20);
            this.txtCommandLog.BorderStyle  = System.Windows.Forms.BorderStyle.None;
            this.txtCommandLog.Dock         = System.Windows.Forms.DockStyle.Fill;
            this.txtCommandLog.Font         = new System.Drawing.Font("Consolas", 8.25F);
            this.txtCommandLog.ForeColor    = System.Drawing.Color.FromArgb(180, 180, 180);
            this.txtCommandLog.Multiline    = true;
            this.txtCommandLog.Name         = "txtCommandLog";
            this.txtCommandLog.ReadOnly     = true;
            this.txtCommandLog.ScrollBars   = System.Windows.Forms.ScrollBars.Vertical;
            this.txtCommandLog.TabIndex     = 0;

            // ── btnClearLog ───────────────────────────────────────────────
            this.btnClearLog.BackColor                  = System.Drawing.Color.FromArgb(55, 55, 55);
            this.btnClearLog.Dock                       = System.Windows.Forms.DockStyle.Bottom;
            this.btnClearLog.FlatAppearance.BorderSize  = 0;
            this.btnClearLog.FlatStyle                  = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearLog.ForeColor                  = System.Drawing.Color.FromArgb(190, 190, 190);
            this.btnClearLog.Name                       = "btnClearLog";
            this.btnClearLog.Size                       = new System.Drawing.Size(364, 26);
            this.btnClearLog.TabIndex                   = 1;
            this.btnClearLog.Text                       = "Clear Log";
            this.btnClearLog.UseVisualStyleBackColor    = false;
            this.btnClearLog.Click += new System.EventHandler(this.btnClearLog_Click);

            // ── statusStrip ───────────────────────────────────────────────
            this.statusStrip.BackColor    = System.Drawing.Color.FromArgb(0, 122, 204);
            this.statusStrip.ForeColor    = System.Drawing.Color.White;
            this.statusStrip.SizingGrip  = false;
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { this.lblStatus });
            this.statusStrip.Name        = "statusStrip";
            this.statusStrip.Size        = new System.Drawing.Size(1184, 22);
            this.statusStrip.TabIndex    = 2;

            // ── lblStatus ─────────────────────────────────────────────────
            this.lblStatus.ForeColor = System.Drawing.Color.White;
            this.lblStatus.Name      = "lblStatus";
            this.lblStatus.Text      = "Ready";

            // ── AiEditorForm ──────────────────────────────────────────────
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor           = System.Drawing.Color.FromArgb(30, 30, 30);
            this.ClientSize          = new System.Drawing.Size(1184, 661);
            this.Controls.Add(this.tabEditors);
            this.Controls.Add(this.panelSidebar);
            this.Controls.Add(this.statusStrip);
            this.Font                = new System.Drawing.Font("Segoe UI", 9F);
            this.Name                = "AiEditorForm";
            this.StartPosition       = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text                = "AI-Powered Monaco Editor";
            this.Load += new System.EventHandler(this.AiEditorForm_Load);

            this.panelSidebar.ResumeLayout(false);
            this.groupEditor.ResumeLayout(false);
            this.groupTests.ResumeLayout(false);
            this.groupAi.ResumeLayout(false);
            this.groupAi.PerformLayout();
            this.groupLog.ResumeLayout(false);
            this.groupLog.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
