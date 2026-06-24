namespace FocusTrack.UI
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            viewToolStripMenuItem = new ToolStripMenuItem();
            stopTrackingToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            viewToolStripMenuItem1 = new ToolStripMenuItem();
            dashboardToolStripMenuItem = new ToolStripMenuItem();
            historyToolStripMenuItem = new ToolStripMenuItem();
            settingsToolStripMenuItem = new ToolStripMenuItem();
            reportsToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem1 = new ToolStripMenuItem();
            helpToolStripMenuItem = new ToolStripMenuItem();
            aboutToolStripMenuItem = new ToolStripMenuItem();
            toolStrip1 = new ToolStrip();
            btnStartTracking = new ToolStripButton();
            btnStopTracking = new ToolStripButton();
            btnRefresh = new ToolStripButton();
            btnExport = new ToolStripButton();
            lblStatus = new StatusStrip();
            tabControlMain = new TabControl();
            tabDashboard = new TabPage();
            tabHistory = new TabPage();
            tabSettings = new TabPage();
            tabReports = new TabPage();
            notifyIconMain = new NotifyIcon(components);
            contextMenuStrip1 = new ContextMenuStrip(components);
            showToolStripMenuItem = new ToolStripMenuItem();
            hideToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem1 = new ToolStripMenuItem();
            menuStrip1.SuspendLayout();
            toolStrip1.SuspendLayout();
            tabControlMain.SuspendLayout();
            contextMenuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, viewToolStripMenuItem1, toolStripMenuItem1, helpToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1082, 28);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { viewToolStripMenuItem, stopTrackingToolStripMenuItem, exitToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(46, 24);
            fileToolStripMenuItem.Text = "File";
            // 
            // viewToolStripMenuItem
            // 
            viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            viewToolStripMenuItem.Size = new Size(224, 26);
            viewToolStripMenuItem.Text = "Start Tracking";
            // 
            // stopTrackingToolStripMenuItem
            // 
            stopTrackingToolStripMenuItem.Name = "stopTrackingToolStripMenuItem";
            stopTrackingToolStripMenuItem.Size = new Size(224, 26);
            stopTrackingToolStripMenuItem.Text = "Stop Tracking";
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(224, 26);
            exitToolStripMenuItem.Text = "Exit";
            // 
            // viewToolStripMenuItem1
            // 
            viewToolStripMenuItem1.DropDownItems.AddRange(new ToolStripItem[] { dashboardToolStripMenuItem, historyToolStripMenuItem, settingsToolStripMenuItem, reportsToolStripMenuItem });
            viewToolStripMenuItem1.Name = "viewToolStripMenuItem1";
            viewToolStripMenuItem1.Size = new Size(55, 24);
            viewToolStripMenuItem1.Text = "View";
            // 
            // dashboardToolStripMenuItem
            // 
            dashboardToolStripMenuItem.Name = "dashboardToolStripMenuItem";
            dashboardToolStripMenuItem.Size = new Size(165, 26);
            dashboardToolStripMenuItem.Text = "Dashboard";
            // 
            // historyToolStripMenuItem
            // 
            historyToolStripMenuItem.Name = "historyToolStripMenuItem";
            historyToolStripMenuItem.Size = new Size(165, 26);
            historyToolStripMenuItem.Text = "History";
            // 
            // settingsToolStripMenuItem
            // 
            settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            settingsToolStripMenuItem.Size = new Size(165, 26);
            settingsToolStripMenuItem.Text = "Settings";
            // 
            // reportsToolStripMenuItem
            // 
            reportsToolStripMenuItem.Name = "reportsToolStripMenuItem";
            reportsToolStripMenuItem.Size = new Size(165, 26);
            reportsToolStripMenuItem.Text = "Reports";
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(14, 24);
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { aboutToolStripMenuItem });
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new Size(55, 24);
            helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            aboutToolStripMenuItem.Size = new Size(133, 26);
            aboutToolStripMenuItem.Text = "About";
            // 
            // toolStrip1
            // 
            toolStrip1.ImageScalingSize = new Size(20, 20);
            toolStrip1.Items.AddRange(new ToolStripItem[] { btnStartTracking, btnStopTracking, btnRefresh, btnExport });
            toolStrip1.Location = new Point(0, 28);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new Size(1082, 27);
            toolStrip1.TabIndex = 1;
            toolStrip1.Text = "toolStrip1";
            // 
            // btnStartTracking
            // 
            btnStartTracking.DisplayStyle = ToolStripItemDisplayStyle.Text;
            btnStartTracking.Image = (Image)resources.GetObject("btnStartTracking.Image");
            btnStartTracking.ImageTransparentColor = Color.Magenta;
            btnStartTracking.Name = "btnStartTracking";
            btnStartTracking.Size = new Size(44, 24);
            btnStartTracking.Text = "Start";
            // 
            // btnStopTracking
            // 
            btnStopTracking.DisplayStyle = ToolStripItemDisplayStyle.Text;
            btnStopTracking.Image = (Image)resources.GetObject("btnStopTracking.Image");
            btnStopTracking.ImageTransparentColor = Color.Magenta;
            btnStopTracking.Name = "btnStopTracking";
            btnStopTracking.Size = new Size(44, 24);
            btnStopTracking.Text = "Stop";
            // 
            // btnRefresh
            // 
            btnRefresh.DisplayStyle = ToolStripItemDisplayStyle.Text;
            btnRefresh.Image = (Image)resources.GetObject("btnRefresh.Image");
            btnRefresh.ImageTransparentColor = Color.Magenta;
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(62, 24);
            btnRefresh.Text = "Refresh";
            // 
            // btnExport
            // 
            btnExport.DisplayStyle = ToolStripItemDisplayStyle.Text;
            btnExport.Image = (Image)resources.GetObject("btnExport.Image");
            btnExport.ImageTransparentColor = Color.Magenta;
            btnExport.Name = "btnExport";
            btnExport.Size = new Size(56, 24);
            btnExport.Text = "Export";
            // 
            // lblStatus
            // 
            lblStatus.ImageScalingSize = new Size(20, 20);
            lblStatus.Location = new Point(0, 631);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(1082, 22);
            lblStatus.TabIndex = 2;
            lblStatus.Text = " Ready";
            lblStatus.ItemClicked += statusStrip1_ItemClicked;
            // 
            // tabControlMain
            // 
            tabControlMain.Controls.Add(tabDashboard);
            tabControlMain.Controls.Add(tabHistory);
            tabControlMain.Controls.Add(tabSettings);
            tabControlMain.Controls.Add(tabReports);
            tabControlMain.Dock = DockStyle.Fill;
            tabControlMain.Location = new Point(0, 55);
            tabControlMain.Name = "tabControlMain";
            tabControlMain.SelectedIndex = 0;
            tabControlMain.Size = new Size(1082, 576);
            tabControlMain.TabIndex = 3;
            // 
            // tabDashboard
            // 
            tabDashboard.Location = new Point(4, 29);
            tabDashboard.Name = "tabDashboard";
            tabDashboard.Padding = new Padding(3);
            tabDashboard.Size = new Size(1074, 543);
            tabDashboard.TabIndex = 0;
            tabDashboard.Text = "Dashboard";
            tabDashboard.UseVisualStyleBackColor = true;
            tabDashboard.Click += tabPage1_Click;
            // 
            // tabHistory
            // 
            tabHistory.Location = new Point(4, 29);
            tabHistory.Name = "tabHistory";
            tabHistory.Padding = new Padding(3);
            tabHistory.Size = new Size(1074, 543);
            tabHistory.TabIndex = 1;
            tabHistory.Text = "History";
            tabHistory.UseVisualStyleBackColor = true;
            tabHistory.Click += tabPage2_Click;
            // 
            // tabSettings
            // 
            tabSettings.Location = new Point(4, 29);
            tabSettings.Name = "tabSettings";
            tabSettings.Padding = new Padding(3);
            tabSettings.Size = new Size(1074, 543);
            tabSettings.TabIndex = 2;
            tabSettings.Text = "Settings";
            tabSettings.UseVisualStyleBackColor = true;
            // 
            // tabReports
            // 
            tabReports.Location = new Point(4, 29);
            tabReports.Name = "tabReports";
            tabReports.Padding = new Padding(3);
            tabReports.Size = new Size(1074, 543);
            tabReports.TabIndex = 3;
            tabReports.Text = "Reports";
            tabReports.UseVisualStyleBackColor = true;
            // 
            // notifyIconMain
            // 
            notifyIconMain.ContextMenuStrip = contextMenuStrip1;
            notifyIconMain.Text = "FocusTrack\n";
            notifyIconMain.Visible = true;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.ImageScalingSize = new Size(20, 20);
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { showToolStripMenuItem, hideToolStripMenuItem, exitToolStripMenuItem1 });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(115, 76);
            // 
            // showToolStripMenuItem
            // 
            showToolStripMenuItem.Name = "showToolStripMenuItem";
            showToolStripMenuItem.Size = new Size(210, 24);
            showToolStripMenuItem.Text = "Show";
            // 
            // hideToolStripMenuItem
            // 
            hideToolStripMenuItem.Name = "hideToolStripMenuItem";
            hideToolStripMenuItem.Size = new Size(114, 24);
            hideToolStripMenuItem.Text = "Hide";
            // 
            // exitToolStripMenuItem1
            // 
            exitToolStripMenuItem1.Name = "exitToolStripMenuItem1";
            exitToolStripMenuItem1.Size = new Size(114, 24);
            exitToolStripMenuItem1.Text = "Exit";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1082, 653);
            Controls.Add(tabControlMain);
            Controls.Add(lblStatus);
            Controls.Add(toolStrip1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FocusTrack";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            tabControlMain.ResumeLayout(false);
            contextMenuStrip1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStrip toolStrip1;
        private StatusStrip lblStatus;
        private TabControl tabControlMain;
        private TabPage tabDashboard;
        private TabPage tabHistory;
        private NotifyIcon notifyIconMain;
        private ContextMenuStrip contextMenuStrip1;
        private TabPage tabSettings;
        private TabPage tabReports;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem viewToolStripMenuItem;
        private ToolStripMenuItem viewToolStripMenuItem1;
        private ToolStripMenuItem toolStripMenuItem1;
        private ToolStripMenuItem stopTrackingToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem dashboardToolStripMenuItem;
        private ToolStripMenuItem historyToolStripMenuItem;
        private ToolStripMenuItem settingsToolStripMenuItem;
        private ToolStripMenuItem reportsToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private ToolStripButton btnStartTracking;
        private ToolStripButton btnStopTracking;
        private ToolStripButton btnRefresh;
        private ToolStripButton btnExport;
        private ToolStripMenuItem showToolStripMenuItem;
        private ToolStripMenuItem hideToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem1;
    }
}
