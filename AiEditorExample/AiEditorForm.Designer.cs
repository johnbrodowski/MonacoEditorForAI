namespace AiEditorExample
{
    partial class AiEditorForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TabControl tabEditors;
        private Panel panelSidebar;
        private GroupBox groupFile;
        private Button btnNewEditor;
        private Button btnLoadFile;
        private Button btnSaveFile;
        private Button btnRunTests;
        private GroupBox groupAi;
        private Label lblApiKey;
        private TextBox txtApiKey;
        private Label lblInstruction;
        private TextBox txtInstruction;
        private Button btnSendToAi;
        private GroupBox groupLog;
        private TextBox txtCommandLog;
        private Button btnClearLog;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel lblStatus;

        private void InitializeComponent()
        {
            this.tabEditors = new System.Windows.Forms.TabControl();
            this.panelSidebar = new System.Windows.Forms.Panel();
            this.groupFile = new System.Windows.Forms.GroupBox();
            this.btnNewEditor = new System.Windows.Forms.Button();
            this.btnLoadFile = new System.Windows.Forms.Button();
            this.btnSaveFile = new System.Windows.Forms.Button();
            this.btnRunTests = new System.Windows.Forms.Button();
            this.groupAi = new System.Windows.Forms.GroupBox();
            this.lblApiKey = new System.Windows.Forms.Label();
            this.txtApiKey = new System.Windows.Forms.TextBox();
            this.lblInstruction = new System.Windows.Forms.Label();
            this.txtInstruction = new System.Windows.Forms.TextBox();
            this.btnSendToAi = new System.Windows.Forms.Button();
            this.groupLog = new System.Windows.Forms.GroupBox();
            this.txtCommandLog = new System.Windows.Forms.TextBox();
            this.btnClearLog = new System.Windows.Forms.Button();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.panelSidebar.SuspendLayout();
            this.groupFile.SuspendLayout();
            this.groupAi.SuspendLayout();
            this.groupLog.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            //
            // tabEditors
            //
            this.tabEditors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabEditors.Location = new System.Drawing.Point(0, 0);
            this.tabEditors.Name = "tabEditors";
            this.tabEditors.SelectedIndex = 0;
            this.tabEditors.Size = new System.Drawing.Size(784, 559);
            this.tabEditors.TabIndex = 0;
            //
            // panelSidebar
            //
            this.panelSidebar.BackColor = System.Drawing.Color.FromArgb(45, 45, 48);
            this.panelSidebar.Controls.Add(this.groupLog);
            this.panelSidebar.Controls.Add(this.groupAi);
            this.panelSidebar.Controls.Add(this.groupFile);
            this.panelSidebar.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelSidebar.Location = new System.Drawing.Point(784, 0);
            this.panelSidebar.Name = "panelSidebar";
            this.panelSidebar.Padding = new System.Windows.Forms.Padding(10);
            this.panelSidebar.Size = new System.Drawing.Size(400, 559);
            this.panelSidebar.TabIndex = 1;
            //
            // groupFile
            //
            this.groupFile.Controls.Add(this.btnRunTests);
            this.groupFile.Controls.Add(this.btnSaveFile);
            this.groupFile.Controls.Add(this.btnLoadFile);
            this.groupFile.Controls.Add(this.btnNewEditor);
            this.groupFile.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupFile.ForeColor = System.Drawing.Color.White;
            this.groupFile.Location = new System.Drawing.Point(10, 10);
            this.groupFile.Name = "groupFile";
            this.groupFile.Padding = new System.Windows.Forms.Padding(8);
            this.groupFile.Size = new System.Drawing.Size(380, 150);
            this.groupFile.TabIndex = 0;
            this.groupFile.TabStop = false;
            this.groupFile.Text = "File Operations";
            //
            // btnNewEditor
            //
            this.btnNewEditor.BackColor = System.Drawing.Color.FromArgb(16, 124, 16);
            this.btnNewEditor.FlatAppearance.BorderSize = 0;
            this.btnNewEditor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNewEditor.ForeColor = System.Drawing.Color.White;
            this.btnNewEditor.Location = new System.Drawing.Point(11, 27);
            this.btnNewEditor.Name = "btnNewEditor";
            this.btnNewEditor.Size = new System.Drawing.Size(358, 23);
            this.btnNewEditor.TabIndex = 0;
            this.btnNewEditor.Text = "+ New Editor";
            this.btnNewEditor.UseVisualStyleBackColor = false;
            this.btnNewEditor.Click += new System.EventHandler(this.btnNewEditor_Click);
            //
            // btnLoadFile
            //
            this.btnLoadFile.BackColor = System.Drawing.Color.FromArgb(0, 122, 204);
            this.btnLoadFile.FlatAppearance.BorderSize = 0;
            this.btnLoadFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoadFile.ForeColor = System.Drawing.Color.White;
            this.btnLoadFile.Location = new System.Drawing.Point(11, 57);
            this.btnLoadFile.Name = "btnLoadFile";
            this.btnLoadFile.Size = new System.Drawing.Size(358, 23);
            this.btnLoadFile.TabIndex = 1;
            this.btnLoadFile.Text = "Load File into Active Editor";
            this.btnLoadFile.UseVisualStyleBackColor = false;
            this.btnLoadFile.Click += new System.EventHandler(this.btnLoadFile_Click);
            //
            // btnSaveFile
            //
            this.btnSaveFile.BackColor = System.Drawing.Color.FromArgb(0, 122, 204);
            this.btnSaveFile.FlatAppearance.BorderSize = 0;
            this.btnSaveFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveFile.ForeColor = System.Drawing.Color.White;
            this.btnSaveFile.Location = new System.Drawing.Point(11, 86);
            this.btnSaveFile.Name = "btnSaveFile";
            this.btnSaveFile.Size = new System.Drawing.Size(358, 23);
            this.btnSaveFile.TabIndex = 2;
            this.btnSaveFile.Text = "Save Active Editor to File";
            this.btnSaveFile.UseVisualStyleBackColor = false;
            this.btnSaveFile.Click += new System.EventHandler(this.btnSaveFile_Click);
            //
            // btnRunTests
            //
            this.btnRunTests.BackColor = System.Drawing.Color.FromArgb(180, 90, 0);
            this.btnRunTests.FlatAppearance.BorderSize = 0;
            this.btnRunTests.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRunTests.ForeColor = System.Drawing.Color.White;
            this.btnRunTests.Location = new System.Drawing.Point(11, 115);
            this.btnRunTests.Name = "btnRunTests";
            this.btnRunTests.Size = new System.Drawing.Size(358, 23);
            this.btnRunTests.TabIndex = 3;
            this.btnRunTests.Text = "Run Editor Tests";
            this.btnRunTests.UseVisualStyleBackColor = false;
            this.btnRunTests.Click += new System.EventHandler(this.btnRunTests_Click);
            //
            // groupAi
            //
            this.groupAi.Controls.Add(this.btnSendToAi);
            this.groupAi.Controls.Add(this.txtInstruction);
            this.groupAi.Controls.Add(this.lblInstruction);
            this.groupAi.Controls.Add(this.txtApiKey);
            this.groupAi.Controls.Add(this.lblApiKey);
            this.groupAi.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupAi.ForeColor = System.Drawing.Color.White;
            this.groupAi.Location = new System.Drawing.Point(10, 160);
            this.groupAi.Name = "groupAi";
            this.groupAi.Padding = new System.Windows.Forms.Padding(8);
            this.groupAi.Size = new System.Drawing.Size(380, 240);
            this.groupAi.TabIndex = 1;
            this.groupAi.TabStop = false;
            this.groupAi.Text = "AI Code Editor";
            //
            // lblApiKey
            //
            this.lblApiKey.AutoSize = true;
            this.lblApiKey.Location = new System.Drawing.Point(11, 27);
            this.lblApiKey.Name = "lblApiKey";
            this.lblApiKey.Size = new System.Drawing.Size(92, 15);
            this.lblApiKey.TabIndex = 0;
            this.lblApiKey.Text = "Anthropic API Key:";
            //
            // txtApiKey
            //
            this.txtApiKey.Location = new System.Drawing.Point(11, 45);
            this.txtApiKey.Name = "txtApiKey";
            this.txtApiKey.PasswordChar = '*';
            this.txtApiKey.Size = new System.Drawing.Size(358, 23);
            this.txtApiKey.TabIndex = 1;
            //
            // lblInstruction
            //
            this.lblInstruction.AutoSize = true;
            this.lblInstruction.Location = new System.Drawing.Point(11, 81);
            this.lblInstruction.Name = "lblInstruction";
            this.lblInstruction.Size = new System.Drawing.Size(280, 15);
            this.lblInstruction.TabIndex = 2;
            this.lblInstruction.Text = "Instruction (e.g., \"Add error handling to Main method\"):";
            //
            // txtInstruction
            //
            this.txtInstruction.Location = new System.Drawing.Point(11, 99);
            this.txtInstruction.Multiline = true;
            this.txtInstruction.Name = "txtInstruction";
            this.txtInstruction.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtInstruction.Size = new System.Drawing.Size(358, 90);
            this.txtInstruction.TabIndex = 3;
            //
            // btnSendToAi
            //
            this.btnSendToAi.BackColor = System.Drawing.Color.FromArgb(0, 122, 204);
            this.btnSendToAi.Enabled = false;
            this.btnSendToAi.FlatAppearance.BorderSize = 0;
            this.btnSendToAi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSendToAi.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnSendToAi.ForeColor = System.Drawing.Color.White;
            this.btnSendToAi.Location = new System.Drawing.Point(11, 199);
            this.btnSendToAi.Name = "btnSendToAi";
            this.btnSendToAi.Size = new System.Drawing.Size(358, 30);
            this.btnSendToAi.TabIndex = 4;
            this.btnSendToAi.Text = "Send Active Editor to AI";
            this.btnSendToAi.UseVisualStyleBackColor = false;
            this.btnSendToAi.Click += new System.EventHandler(this.btnSendToAi_Click);
            //
            // groupLog
            //
            this.groupLog.Controls.Add(this.btnClearLog);
            this.groupLog.Controls.Add(this.txtCommandLog);
            this.groupLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupLog.ForeColor = System.Drawing.Color.White;
            this.groupLog.Location = new System.Drawing.Point(10, 400);
            this.groupLog.Name = "groupLog";
            this.groupLog.Padding = new System.Windows.Forms.Padding(8);
            this.groupLog.Size = new System.Drawing.Size(380, 159);
            this.groupLog.TabIndex = 2;
            this.groupLog.TabStop = false;
            this.groupLog.Text = "Command Log";
            //
            // txtCommandLog
            //
            this.txtCommandLog.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            this.txtCommandLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCommandLog.Font = new System.Drawing.Font("Consolas", 8.25F);
            this.txtCommandLog.ForeColor = System.Drawing.Color.LightGray;
            this.txtCommandLog.Location = new System.Drawing.Point(8, 24);
            this.txtCommandLog.Multiline = true;
            this.txtCommandLog.Name = "txtCommandLog";
            this.txtCommandLog.ReadOnly = true;
            this.txtCommandLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtCommandLog.Size = new System.Drawing.Size(364, 97);
            this.txtCommandLog.TabIndex = 0;
            //
            // btnClearLog
            //
            this.btnClearLog.BackColor = System.Drawing.Color.FromArgb(60, 60, 60);
            this.btnClearLog.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnClearLog.FlatAppearance.BorderSize = 0;
            this.btnClearLog.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearLog.ForeColor = System.Drawing.Color.White;
            this.btnClearLog.Location = new System.Drawing.Point(8, 121);
            this.btnClearLog.Name = "btnClearLog";
            this.btnClearLog.Size = new System.Drawing.Size(364, 29);
            this.btnClearLog.TabIndex = 1;
            this.btnClearLog.Text = "Clear Log";
            this.btnClearLog.UseVisualStyleBackColor = false;
            this.btnClearLog.Click += new System.EventHandler(this.btnClearLog_Click);
            //
            // statusStrip
            //
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.statusStrip.Location = new System.Drawing.Point(0, 559);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1184, 22);
            this.statusStrip.TabIndex = 2;
            this.statusStrip.Text = "statusStrip1";
            //
            // lblStatus
            //
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(73, 17);
            this.lblStatus.Text = "Status: Ready";
            //
            // AiEditorForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 581);
            this.Controls.Add(this.tabEditors);
            this.Controls.Add(this.panelSidebar);
            this.Controls.Add(this.statusStrip);
            this.Name = "AiEditorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AI-Powered Monaco Editor";
            this.Load += new System.EventHandler(this.AiEditorForm_Load);
            this.panelSidebar.ResumeLayout(false);
            this.groupFile.ResumeLayout(false);
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
