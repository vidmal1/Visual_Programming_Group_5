namespace FocusTrack
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
            menuStrip1 = new MenuStrip();
            aboutToolStripMenuItem1 = new ToolStripMenuItem();
            exitToolStripMenuItem1 = new ToolStripMenuItem();
            toolStripMenuItem2 = new ToolStripSeparator();
            exitToolStripMenuItem1d = new ToolStripMenuItem();
            exportToExcelToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem1 = new ToolStripSeparator();
            exitToolStripMenuItem = new ToolStripMenuItem();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            lblCurrentApp = new ToolStripStatusLabel();
            statusStrip1 = new StatusStrip();
            tabSettings = new TabPage();
            grpApplicationClassification = new GroupBox();
            dataGridView1 = new DataGridView();
            colApplication = new DataGridViewTextBoxColumn();
            colCategory = new DataGridViewComboBoxColumn();
            groupBox3 = new GroupBox();
            lstIgnoreList = new ListBox();
            lblIgnoreInput = new Label();
            txtIgnoreAppName = new TextBox();
            btnRemoveFromIgnore = new Button();
            lblIgnoredApps = new Label();
            btnAddToIgnore = new Button();
            grpCategory = new GroupBox();
            btnRemoveFromCategories = new Button();
            btnAddCategory = new Button();
            txtCategoryName = new TextBox();
            lblCategoryInput = new Label();
            lblCategoriesList = new Label();
            btnSaveCategorySettings = new Button();
            nudDailyGoal = new NumericUpDown();
            lblMinutes = new Label();
            lstCategories = new ListBox();
            tabReports = new TabPage();
            exportToPdfToolStripMenuItem1 = new Button();
            exportToPdfToolStripMenuItem = new Button();
            label6 = new Label();
            label5 = new Label();
            label2 = new Label();
            label1 = new Label();
            dtpReportFrom = new DateTimePicker();
            dtpReportTo = new DateTimePicker();
            cboReportType = new ComboBox();
            btnGenerateReport = new Button();
            rdoTabularReport = new RadioButton();
            rdoChartReport = new RadioButton();
            pnlReportChartHost = new Panel();
            pnlReportGridHost = new Panel();
            dgvReports = new DataGridView();
            tabHistory = new TabPage();
            label8 = new Label();
            label7 = new Label();
            label4 = new Label();
            label3 = new Label();
            dtpFrom = new DateTimePicker();
            dtpTo = new DateTimePicker();
            txtAppFilter = new TextBox();
            cboCategoryFilter = new ComboBox();
            btnFilterHistory = new Button();
            dgvHistory = new DataGridView();
            tabDashboard = new TabPage();
            groupBox2 = new GroupBox();
            pnlChart = new Panel();
            groupBox1 = new GroupBox();
            lblTotalSessions = new Label();
            lblMostUsedApp = new Label();
            lblTotalTime = new Label();
            tabControl1 = new TabControl();
            menuStrip1.SuspendLayout();
            statusStrip1.SuspendLayout();
            tabSettings.SuspendLayout();
            grpApplicationClassification.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            groupBox3.SuspendLayout();
            grpCategory.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudDailyGoal).BeginInit();
            tabReports.SuspendLayout();
            pnlReportChartHost.SuspendLayout();
            pnlReportGridHost.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvReports).BeginInit();
            tabHistory.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvHistory).BeginInit();
            tabDashboard.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox1.SuspendLayout();
            tabControl1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { aboutToolStripMenuItem1, exitToolStripMenuItem1 });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(942, 28);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // aboutToolStripMenuItem1
            // 
            aboutToolStripMenuItem1.Name = "aboutToolStripMenuItem1";
            aboutToolStripMenuItem1.Size = new Size(67, 24);
            aboutToolStripMenuItem1.Text = "About";
            aboutToolStripMenuItem1.Click += aboutToolStripMenuItem_Click;
            // 
            // exitToolStripMenuItem1
            // 
            exitToolStripMenuItem1.Name = "exitToolStripMenuItem1";
            exitToolStripMenuItem1.Size = new Size(49, 24);
            exitToolStripMenuItem1.Text = "Exit";
            exitToolStripMenuItem1.Click += exitToolStripMenuItem1_Click;
            // 
            // toolStripMenuItem2
            // 
            toolStripMenuItem2.Name = "toolStripMenuItem2";
            toolStripMenuItem2.Size = new Size(115, 6);
            // 
            // exitToolStripMenuItem1d
            // 
            exitToolStripMenuItem1d.BackColor = Color.White;
            exitToolStripMenuItem1d.Name = "exitToolStripMenuItem1d";
            exitToolStripMenuItem1d.Size = new Size(118, 26);
            exitToolStripMenuItem1d.Text = "Exit";
            exitToolStripMenuItem1d.Click += exitToolStripMenuItem1_Click;
            // 
            // exportToExcelToolStripMenuItem
            // 
            exportToExcelToolStripMenuItem.Name = "exportToExcelToolStripMenuItem";
            exportToExcelToolStripMenuItem.Size = new Size(150, 22);
            exportToExcelToolStripMenuItem.Text = "Export to Excel";
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(147, 6);
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(150, 22);
            exitToolStripMenuItem.Text = "Exit";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(156, 20);
            toolStripStatusLabel1.Text = "Status: Tracking Active";
            // 
            // lblCurrentApp
            // 
            lblCurrentApp.Name = "lblCurrentApp";
            lblCurrentApp.Size = new Size(121, 20);
            lblCurrentApp.Text = "Current App: Idle";
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new Size(20, 20);
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel1, lblCurrentApp });
            statusStrip1.Location = new Point(0, 600);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(942, 26);
            statusStrip1.TabIndex = 2;
            statusStrip1.Text = "statusStrip1";
            // 
            // tabSettings
            // 
            tabSettings.Controls.Add(grpApplicationClassification);
            tabSettings.Controls.Add(groupBox3);
            tabSettings.Controls.Add(grpCategory);
            tabSettings.Location = new Point(4, 30);
            tabSettings.Name = "tabSettings";
            tabSettings.Size = new Size(934, 538);
            tabSettings.TabIndex = 3;
            tabSettings.Text = "Settings";
            tabSettings.UseVisualStyleBackColor = true;
            tabSettings.Click += tabSettings_Click;
            // 
            // grpApplicationClassification
            // 
            grpApplicationClassification.BackColor = Color.AliceBlue;
            grpApplicationClassification.Controls.Add(dataGridView1);
            grpApplicationClassification.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            grpApplicationClassification.Location = new Point(599, 16);
            grpApplicationClassification.Name = "grpApplicationClassification";
            grpApplicationClassification.Size = new Size(318, 500);
            grpApplicationClassification.TabIndex = 12;
            grpApplicationClassification.TabStop = false;
            grpApplicationClassification.Text = "APPLICATION CLASSIFICATION";
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.BackgroundColor = SystemColors.ButtonHighlight;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { colApplication, colCategory });
            dataGridView1.GridColor = SystemColors.InactiveCaption;
            dataGridView1.Location = new Point(7, 30);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.Size = new Size(305, 448);
            dataGridView1.TabIndex = 0;
            dataGridView1.CellContentClick += dataGridView1_CellContentClick;
            // 
            // colApplication
            // 
            colApplication.HeaderText = "Application";
            colApplication.MinimumWidth = 6;
            colApplication.Name = "colApplication";
            colApplication.ReadOnly = true;
            // 
            // colCategory
            // 
            colCategory.HeaderText = "Category";
            colCategory.Items.AddRange(new object[] { "Productive", "Neutral", "Distracting", "Communication", "Entertainment" });
            colCategory.MinimumWidth = 6;
            colCategory.Name = "colCategory";
            // 
            // groupBox3
            // 
            groupBox3.BackColor = Color.AliceBlue;
            groupBox3.Controls.Add(lstIgnoreList);
            groupBox3.Controls.Add(lblIgnoreInput);
            groupBox3.Controls.Add(txtIgnoreAppName);
            groupBox3.Controls.Add(btnRemoveFromIgnore);
            groupBox3.Controls.Add(lblIgnoredApps);
            groupBox3.Controls.Add(btnAddToIgnore);
            groupBox3.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBox3.Location = new Point(292, 16);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(269, 478);
            groupBox3.TabIndex = 6;
            groupBox3.TabStop = false;
            groupBox3.Text = "APPLICATION IGNORE LIST";
            groupBox3.Enter += groupBox3_Enter;
            // 
            // lstIgnoreList
            // 
            lstIgnoreList.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lstIgnoreList.FormattingEnabled = true;
            lstIgnoreList.Location = new Point(36, 239);
            lstIgnoreList.Name = "lstIgnoreList";
            lstIgnoreList.Size = new Size(197, 84);
            lstIgnoreList.TabIndex = 13;
            // 
            // lblIgnoreInput
            // 
            lblIgnoreInput.AutoSize = true;
            lblIgnoreInput.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblIgnoreInput.Location = new Point(17, 58);
            lblIgnoreInput.Name = "lblIgnoreInput";
            lblIgnoreInput.Size = new Size(55, 20);
            lblIgnoreInput.TabIndex = 7;
            lblIgnoreInput.Text = "Name:";
            // 
            // txtIgnoreAppName
            // 
            txtIgnoreAppName.Location = new Point(17, 91);
            txtIgnoreAppName.Name = "txtIgnoreAppName";
            txtIgnoreAppName.Size = new Size(230, 29);
            txtIgnoreAppName.TabIndex = 11;
            // 
            // btnRemoveFromIgnore
            // 
            btnRemoveFromIgnore.BackColor = Color.Gainsboro;
            btnRemoveFromIgnore.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnRemoveFromIgnore.ForeColor = Color.Navy;
            btnRemoveFromIgnore.Location = new Point(39, 344);
            btnRemoveFromIgnore.Name = "btnRemoveFromIgnore";
            btnRemoveFromIgnore.Size = new Size(194, 32);
            btnRemoveFromIgnore.TabIndex = 10;
            btnRemoveFromIgnore.Text = "Remove Selected";
            btnRemoveFromIgnore.UseVisualStyleBackColor = false;
            // 
            // lblIgnoredApps
            // 
            lblIgnoredApps.AutoSize = true;
            lblIgnoredApps.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblIgnoredApps.Location = new Point(17, 189);
            lblIgnoredApps.Name = "lblIgnoredApps";
            lblIgnoredApps.Size = new Size(137, 20);
            lblIgnoredApps.TabIndex = 12;
            lblIgnoredApps.Text = "Currently Ignored:";
            lblIgnoredApps.Click += lblIgnoredApps_Click;
            // 
            // btnAddToIgnore
            // 
            btnAddToIgnore.BackColor = Color.Gainsboro;
            btnAddToIgnore.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnAddToIgnore.ForeColor = Color.Navy;
            btnAddToIgnore.Location = new Point(53, 131);
            btnAddToIgnore.Name = "btnAddToIgnore";
            btnAddToIgnore.Size = new Size(160, 29);
            btnAddToIgnore.TabIndex = 11;
            btnAddToIgnore.Text = "+ Add to Ignore";
            btnAddToIgnore.UseVisualStyleBackColor = false;
            // 
            // grpCategory
            // 
            grpCategory.BackColor = Color.AliceBlue;
            grpCategory.Controls.Add(btnRemoveFromCategories);
            grpCategory.Controls.Add(btnAddCategory);
            grpCategory.Controls.Add(txtCategoryName);
            grpCategory.Controls.Add(lblCategoryInput);
            grpCategory.Controls.Add(lblCategoriesList);
            grpCategory.Controls.Add(btnSaveCategorySettings);
            grpCategory.Controls.Add(nudDailyGoal);
            grpCategory.Controls.Add(lblMinutes);
            grpCategory.Controls.Add(lstCategories);
            grpCategory.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            grpCategory.Location = new Point(8, 16);
            grpCategory.Name = "grpCategory";
            grpCategory.Size = new Size(267, 478);
            grpCategory.TabIndex = 0;
            grpCategory.TabStop = false;
            grpCategory.Text = "CATEGORY GOALS";
            grpCategory.Enter += grpCategory_Enter;
            // 
            // btnRemoveFromCategories
            // 
            btnRemoveFromCategories.BackColor = Color.Gainsboro;
            btnRemoveFromCategories.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnRemoveFromCategories.ForeColor = Color.Navy;
            btnRemoveFromCategories.Location = new Point(132, 417);
            btnRemoveFromCategories.Name = "btnRemoveFromCategories";
            btnRemoveFromCategories.Size = new Size(120, 25);
            btnRemoveFromCategories.TabIndex = 15;
            btnRemoveFromCategories.Text = "Remove ";
            btnRemoveFromCategories.UseVisualStyleBackColor = false;
            btnRemoveFromCategories.Click += btnRemoveFromCategories_Click;
            // 
            // btnAddCategory
            // 
            btnAddCategory.BackColor = Color.Gainsboro;
            btnAddCategory.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnAddCategory.ForeColor = Color.Navy;
            btnAddCategory.Location = new Point(22, 104);
            btnAddCategory.Name = "btnAddCategory";
            btnAddCategory.Size = new Size(210, 39);
            btnAddCategory.TabIndex = 14;
            btnAddCategory.Text = "+ Add Category";
            btnAddCategory.UseVisualStyleBackColor = false;
            btnAddCategory.Click += btnAddCategory_Click_1;
            // 
            // txtCategoryName
            // 
            txtCategoryName.Location = new Point(6, 58);
            txtCategoryName.Name = "txtCategoryName";
            txtCategoryName.Size = new Size(252, 29);
            txtCategoryName.TabIndex = 13;
            txtCategoryName.TextChanged += txtCategoryName_TextChanged;
            // 
            // lblCategoryInput
            // 
            lblCategoryInput.AutoSize = true;
            lblCategoryInput.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblCategoryInput.Location = new Point(0, 30);
            lblCategoryInput.Name = "lblCategoryInput";
            lblCategoryInput.Size = new Size(123, 20);
            lblCategoryInput.TabIndex = 12;
            lblCategoryInput.Text = "Category Name:";
            lblCategoryInput.Click += lblCategoryInput_Click;
            // 
            // lblCategoriesList
            // 
            lblCategoriesList.AutoSize = true;
            lblCategoriesList.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblCategoriesList.Location = new Point(0, 155);
            lblCategoriesList.Name = "lblCategoriesList";
            lblCategoriesList.Size = new Size(151, 20);
            lblCategoriesList.TabIndex = 1;
            lblCategoriesList.Text = "Allocated Category :";
            // 
            // btnSaveCategorySettings
            // 
            btnSaveCategorySettings.BackColor = Color.Gainsboro;
            btnSaveCategorySettings.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSaveCategorySettings.ForeColor = Color.Navy;
            btnSaveCategorySettings.Location = new Point(6, 417);
            btnSaveCategorySettings.Name = "btnSaveCategorySettings";
            btnSaveCategorySettings.Size = new Size(120, 25);
            btnSaveCategorySettings.TabIndex = 4;
            btnSaveCategorySettings.Text = "Save Goal";
            btnSaveCategorySettings.UseVisualStyleBackColor = false;
            btnSaveCategorySettings.Click += btnSaveCategorySettings_Click;
            // 
            // nudDailyGoal
            // 
            nudDailyGoal.Location = new Point(6, 372);
            nudDailyGoal.Name = "nudDailyGoal";
            nudDailyGoal.Size = new Size(98, 29);
            nudDailyGoal.TabIndex = 0;
            nudDailyGoal.ValueChanged += nudDailyGoal_ValueChanged;
            // 
            // lblMinutes
            // 
            lblMinutes.AutoSize = true;
            lblMinutes.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblMinutes.Location = new Point(6, 344);
            lblMinutes.Name = "lblMinutes";
            lblMinutes.Size = new Size(196, 20);
            lblMinutes.TabIndex = 3;
            lblMinutes.Text = "Daily Time Goal (Minutes):";
            // 
            // lstCategories
            // 
            lstCategories.FormattingEnabled = true;
            lstCategories.Location = new Point(6, 189);
            lstCategories.Name = "lstCategories";
            lstCategories.Size = new Size(257, 151);
            lstCategories.TabIndex = 2;
            lstCategories.SelectedIndexChanged += lstCategories_SelectedIndexChanged;
            // 
            // tabReports
            // 
            tabReports.BackColor = Color.AliceBlue;
            tabReports.Controls.Add(exportToPdfToolStripMenuItem1);
            tabReports.Controls.Add(exportToPdfToolStripMenuItem);
            tabReports.Controls.Add(label6);
            tabReports.Controls.Add(label5);
            tabReports.Controls.Add(label2);
            tabReports.Controls.Add(label1);
            tabReports.Controls.Add(dtpReportFrom);
            tabReports.Controls.Add(dtpReportTo);
            tabReports.Controls.Add(cboReportType);
            tabReports.Controls.Add(btnGenerateReport);
            tabReports.Controls.Add(rdoTabularReport);
            tabReports.Controls.Add(rdoChartReport);
            tabReports.Controls.Add(pnlReportChartHost);
            tabReports.Location = new Point(4, 30);
            tabReports.Name = "tabReports";
            tabReports.Size = new Size(934, 538);
            tabReports.TabIndex = 2;
            tabReports.Text = "Reports";
            // 
            // exportToPdfToolStripMenuItem1
            // 
            exportToPdfToolStripMenuItem1.BackColor = Color.SteelBlue;
            exportToPdfToolStripMenuItem1.ForeColor = SystemColors.Window;
            exportToPdfToolStripMenuItem1.Location = new Point(702, 95);
            exportToPdfToolStripMenuItem1.Name = "exportToPdfToolStripMenuItem1";
            exportToPdfToolStripMenuItem1.Size = new Size(203, 31);
            exportToPdfToolStripMenuItem1.TabIndex = 10;
            exportToPdfToolStripMenuItem1.Text = "Export to excel";
            exportToPdfToolStripMenuItem1.UseVisualStyleBackColor = false;
            exportToPdfToolStripMenuItem1.Click += button2_Click_1;
            // 
            // exportToPdfToolStripMenuItem
            // 
            exportToPdfToolStripMenuItem.BackColor = Color.SteelBlue;
            exportToPdfToolStripMenuItem.ForeColor = SystemColors.Window;
            exportToPdfToolStripMenuItem.Location = new Point(702, 60);
            exportToPdfToolStripMenuItem.Name = "exportToPdfToolStripMenuItem";
            exportToPdfToolStripMenuItem.Size = new Size(203, 29);
            exportToPdfToolStripMenuItem.TabIndex = 9;
            exportToPdfToolStripMenuItem.Text = "Export to pdf";
            exportToPdfToolStripMenuItem.UseVisualStyleBackColor = false;
            exportToPdfToolStripMenuItem.Click += button1_Click;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.BackColor = Color.Gainsboro;
            label6.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label6.ForeColor = Color.Navy;
            label6.Location = new Point(180, 11);
            label6.Name = "label6";
            label6.Size = new Size(93, 23);
            label6.TabIndex = 8;
            label6.Text = "END DATE";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.BackColor = Color.Gainsboro;
            label5.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.ForeColor = Color.Navy;
            label5.Location = new Point(24, 11);
            label5.Name = "label5";
            label5.Size = new Size(110, 23);
            label5.TabIndex = 7;
            label5.Text = "START DATE";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Gainsboro;
            label2.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.ForeColor = Color.Navy;
            label2.Location = new Point(520, 11);
            label2.Name = "label2";
            label2.Size = new Size(118, 23);
            label2.TabIndex = 6;
            label2.Text = "REPORT TYPE";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Gainsboro;
            label1.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.Navy;
            label1.Location = new Point(322, 11);
            label1.Name = "label1";
            label1.Size = new Size(62, 23);
            label1.TabIndex = 5;
            label1.Text = "FILTER";
            // 
            // dtpReportFrom
            // 
            dtpReportFrom.Format = DateTimePickerFormat.Short;
            dtpReportFrom.Location = new Point(24, 49);
            dtpReportFrom.Name = "dtpReportFrom";
            dtpReportFrom.Size = new Size(120, 29);
            dtpReportFrom.TabIndex = 0;
            // 
            // dtpReportTo
            // 
            dtpReportTo.Format = DateTimePickerFormat.Short;
            dtpReportTo.Location = new Point(180, 49);
            dtpReportTo.Name = "dtpReportTo";
            dtpReportTo.Size = new Size(120, 29);
            dtpReportTo.TabIndex = 1;
            // 
            // cboReportType
            // 
            cboReportType.DropDownStyle = ComboBoxStyle.DropDownList;
            cboReportType.Location = new Point(322, 49);
            cboReportType.Name = "cboReportType";
            cboReportType.Size = new Size(160, 29);
            cboReportType.TabIndex = 2;
            // 
            // btnGenerateReport
            // 
            btnGenerateReport.BackColor = Color.Navy;
            btnGenerateReport.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnGenerateReport.ForeColor = Color.White;
            btnGenerateReport.Location = new Point(702, 11);
            btnGenerateReport.Name = "btnGenerateReport";
            btnGenerateReport.Size = new Size(203, 43);
            btnGenerateReport.TabIndex = 3;
            btnGenerateReport.Text = "Generate Report";
            btnGenerateReport.UseVisualStyleBackColor = false;
            btnGenerateReport.Click += btnGenerateReport_Click_1;
            // 
            // rdoTabularReport
            // 
            rdoTabularReport.AutoSize = true;
            rdoTabularReport.ForeColor = SystemColors.WindowText;
            rdoTabularReport.Location = new Point(520, 81);
            rdoTabularReport.Name = "rdoTabularReport";
            rdoTabularReport.Size = new Size(101, 27);
            rdoTabularReport.TabIndex = 1;
            rdoTabularReport.Text = "Tabular ";
            rdoTabularReport.UseVisualStyleBackColor = true;
            // 
            // rdoChartReport
            // 
            rdoChartReport.AutoSize = true;
            rdoChartReport.ForeColor = SystemColors.WindowText;
            rdoChartReport.Location = new Point(520, 49);
            rdoChartReport.Name = "rdoChartReport";
            rdoChartReport.Size = new Size(136, 27);
            rdoChartReport.TabIndex = 2;
            rdoChartReport.Text = "Chart-Based";
            rdoChartReport.UseVisualStyleBackColor = true;
            // 
            // pnlReportChartHost
            // 
            pnlReportChartHost.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlReportChartHost.BackColor = Color.White;
            pnlReportChartHost.BorderStyle = BorderStyle.FixedSingle;
            pnlReportChartHost.Controls.Add(pnlReportGridHost);
            pnlReportChartHost.Location = new Point(16, 145);
            pnlReportChartHost.Name = "pnlReportChartHost";
            pnlReportChartHost.Size = new Size(899, 378);
            pnlReportChartHost.TabIndex = 4;
            pnlReportChartHost.Paint += pnlReportChartHost_Paint;
            // 
            // pnlReportGridHost
            // 
            pnlReportGridHost.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlReportGridHost.BackColor = Color.White;
            pnlReportGridHost.BorderStyle = BorderStyle.FixedSingle;
            pnlReportGridHost.Controls.Add(dgvReports);
            pnlReportGridHost.Location = new Point(13, 71);
            pnlReportGridHost.Name = "pnlReportGridHost";
            pnlReportGridHost.Size = new Size(857, 302);
            pnlReportGridHost.TabIndex = 3;
            // 
            // dgvReports
            // 
            dgvReports.ColumnHeadersHeight = 29;
            dgvReports.Location = new Point(-7, -29);
            dgvReports.Name = "dgvReports";
            dgvReports.RowHeadersWidth = 51;
            dgvReports.Size = new Size(865, 283);
            dgvReports.TabIndex = 0;
            // 
            // tabHistory
            // 
            tabHistory.BackColor = Color.AliceBlue;
            tabHistory.Controls.Add(label8);
            tabHistory.Controls.Add(label7);
            tabHistory.Controls.Add(label4);
            tabHistory.Controls.Add(label3);
            tabHistory.Controls.Add(dtpFrom);
            tabHistory.Controls.Add(dtpTo);
            tabHistory.Controls.Add(txtAppFilter);
            tabHistory.Controls.Add(cboCategoryFilter);
            tabHistory.Controls.Add(btnFilterHistory);
            tabHistory.Controls.Add(dgvHistory);
            tabHistory.Location = new Point(4, 30);
            tabHistory.Name = "tabHistory";
            tabHistory.Padding = new Padding(3);
            tabHistory.Size = new Size(934, 538);
            tabHistory.TabIndex = 1;
            tabHistory.Text = "History";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.BackColor = Color.Gainsboro;
            label8.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label8.ForeColor = Color.Navy;
            label8.Location = new Point(187, 15);
            label8.Name = "label8";
            label8.Size = new Size(93, 23);
            label8.TabIndex = 9;
            label8.Text = "END DATE";
            label8.Click += label8_Click;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.BackColor = Color.Gainsboro;
            label7.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label7.ForeColor = Color.Navy;
            label7.Location = new Point(30, 15);
            label7.Name = "label7";
            label7.Size = new Size(110, 23);
            label7.TabIndex = 8;
            label7.Text = "START DATE";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = Color.Gainsboro;
            label4.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.ForeColor = Color.Navy;
            label4.Location = new Point(516, 15);
            label4.Name = "label4";
            label4.Size = new Size(97, 23);
            label4.TabIndex = 7;
            label4.Text = "CATEGORY";
            label4.Click += label4_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.Gainsboro;
            label3.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.ForeColor = Color.Navy;
            label3.Location = new Point(339, 15);
            label3.Name = "label3";
            label3.Size = new Size(76, 23);
            label3.TabIndex = 6;
            label3.Text = "SEARCH";
            label3.Click += label3_Click;
            // 
            // dtpFrom
            // 
            dtpFrom.Format = DateTimePickerFormat.Short;
            dtpFrom.Location = new Point(30, 59);
            dtpFrom.Name = "dtpFrom";
            dtpFrom.Size = new Size(120, 29);
            dtpFrom.TabIndex = 0;
            dtpFrom.ValueChanged += dtpFrom_ValueChanged;
            // 
            // dtpTo
            // 
            dtpTo.Format = DateTimePickerFormat.Short;
            dtpTo.Location = new Point(187, 59);
            dtpTo.Name = "dtpTo";
            dtpTo.Size = new Size(120, 29);
            dtpTo.TabIndex = 1;
            dtpTo.ValueChanged += dtpTo_ValueChanged;
            // 
            // txtAppFilter
            // 
            txtAppFilter.Location = new Point(339, 59);
            txtAppFilter.Name = "txtAppFilter";
            txtAppFilter.Size = new Size(143, 29);
            txtAppFilter.TabIndex = 2;
            // 
            // cboCategoryFilter
            // 
            cboCategoryFilter.Location = new Point(516, 59);
            cboCategoryFilter.Name = "cboCategoryFilter";
            cboCategoryFilter.Size = new Size(160, 29);
            cboCategoryFilter.TabIndex = 3;
            // 
            // btnFilterHistory
            // 
            btnFilterHistory.BackColor = Color.Navy;
            btnFilterHistory.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnFilterHistory.ForeColor = SystemColors.Window;
            btnFilterHistory.Location = new Point(735, 33);
            btnFilterHistory.Name = "btnFilterHistory";
            btnFilterHistory.Size = new Size(157, 32);
            btnFilterHistory.TabIndex = 0;
            btnFilterHistory.Text = "Filter";
            btnFilterHistory.UseVisualStyleBackColor = false;
            // 
            // dgvHistory
            // 
            dgvHistory.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvHistory.ColumnHeadersHeight = 29;
            dgvHistory.Location = new Point(8, 133);
            dgvHistory.Name = "dgvHistory";
            dgvHistory.RowHeadersWidth = 51;
            dgvHistory.Size = new Size(920, 374);
            dgvHistory.TabIndex = 1;
            // 
            // tabDashboard
            // 
            tabDashboard.Controls.Add(groupBox2);
            tabDashboard.Controls.Add(groupBox1);
            tabDashboard.Location = new Point(4, 30);
            tabDashboard.Name = "tabDashboard";
            tabDashboard.Padding = new Padding(3);
            tabDashboard.Size = new Size(934, 538);
            tabDashboard.TabIndex = 0;
            tabDashboard.Text = "Dashboard";
            tabDashboard.UseVisualStyleBackColor = true;
            tabDashboard.Click += tabDashboard_Click;
            // 
            // groupBox2
            // 
            groupBox2.BackColor = Color.AliceBlue;
            groupBox2.Controls.Add(pnlChart);
            groupBox2.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBox2.Location = new Point(234, 6);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(694, 511);
            groupBox2.TabIndex = 4;
            groupBox2.TabStop = false;
            groupBox2.Text = "APPLICATION USAGE";
            groupBox2.Enter += groupBox2_Enter;
            // 
            // pnlChart
            // 
            pnlChart.BorderStyle = BorderStyle.FixedSingle;
            pnlChart.Location = new Point(6, 94);
            pnlChart.Name = "pnlChart";
            pnlChart.Size = new Size(682, 395);
            pnlChart.TabIndex = 0;
            pnlChart.Paint += pnlChart_Paint;
            // 
            // groupBox1
            // 
            groupBox1.BackColor = Color.AliceBlue;
            groupBox1.Controls.Add(lblTotalSessions);
            groupBox1.Controls.Add(lblMostUsedApp);
            groupBox1.Controls.Add(lblTotalTime);
            groupBox1.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBox1.Location = new Point(8, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(220, 504);
            groupBox1.TabIndex = 3;
            groupBox1.TabStop = false;
            groupBox1.Text = "SUMMARY OF THE DAY";
            // 
            // lblTotalSessions
            // 
            lblTotalSessions.AutoSize = true;
            lblTotalSessions.BackColor = Color.Gainsboro;
            lblTotalSessions.Font = new Font("Segoe UI", 9F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            lblTotalSessions.ForeColor = Color.Navy;
            lblTotalSessions.Location = new Point(6, 49);
            lblTotalSessions.Name = "lblTotalSessions";
            lblTotalSessions.Size = new Size(180, 20);
            lblTotalSessions.TabIndex = 0;
            lblTotalSessions.Text = "Total No. of Sessions : 0";
            lblTotalSessions.Click += lblTotalSessions_Click;
            // 
            // lblMostUsedApp
            // 
            lblMostUsedApp.AutoSize = true;
            lblMostUsedApp.BackColor = Color.Gainsboro;
            lblMostUsedApp.Font = new Font("Segoe UI", 9F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            lblMostUsedApp.ForeColor = Color.Navy;
            lblMostUsedApp.Location = new Point(6, 151);
            lblMostUsedApp.Name = "lblMostUsedApp";
            lblMostUsedApp.Size = new Size(162, 20);
            lblMostUsedApp.TabIndex = 2;
            lblMostUsedApp.Text = "Most Used App: None";
            // 
            // lblTotalTime
            // 
            lblTotalTime.AutoSize = true;
            lblTotalTime.BackColor = Color.Gainsboro;
            lblTotalTime.Font = new Font("Segoe UI", 9F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            lblTotalTime.ForeColor = Color.Navy;
            lblTotalTime.Location = new Point(6, 97);
            lblTotalTime.Name = "lblTotalTime";
            lblTotalTime.Size = new Size(178, 20);
            lblTotalTime.TabIndex = 1;
            lblTotalTime.Text = "Total Time Spent : 0 hrs";
            lblTotalTime.Click += lblTotalTime_Click;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabDashboard);
            tabControl1.Controls.Add(tabHistory);
            tabControl1.Controls.Add(tabReports);
            tabControl1.Controls.Add(tabSettings);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Font = new Font("Segoe UI Symbol", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            tabControl1.Location = new Point(0, 28);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(942, 572);
            tabControl1.TabIndex = 3;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(942, 626);
            Controls.Add(tabControl1);
            Controls.Add(statusStrip1);
            Controls.Add(menuStrip1);
            Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            ForeColor = SystemColors.ControlText;
            MainMenuStrip = menuStrip1;
            MinimumSize = new Size(960, 640);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Focus Track";
            Load += MainForm_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            tabSettings.ResumeLayout(false);
            grpApplicationClassification.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            grpCategory.ResumeLayout(false);
            grpCategory.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudDailyGoal).EndInit();
            tabReports.ResumeLayout(false);
            tabReports.PerformLayout();
            pnlReportChartHost.ResumeLayout(false);
            pnlReportGridHost.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvReports).EndInit();
            tabHistory.ResumeLayout(false);
            tabHistory.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvHistory).EndInit();
            tabDashboard.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            tabControl1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem exportToExcelToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem1;
        private GroupBox groupBox1;
        private ToolStripMenuItem bfr;
        private ToolStripSeparator toolStripMenuItem2;
        private ToolStripMenuItem exitToolStripMenuItem1d;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private ToolStripStatusLabel lblCurrentApp;
        private StatusStrip statusStrip1;
        private TabPage tabSettings;
        private TabPage tabReports;
        private TabPage tabHistory;
        private DateTimePicker dtpReportFrom;
        private DateTimePicker dtpReportTo;
        private ComboBox cboReportType;
        private Button btnGenerateReport;
        private RadioButton rdoTabularReport;
        private RadioButton rdoChartReport;
        private Panel pnlReportGridHost;
        private Panel pnlReportChartHost;
        private DataGridView dgvReports;
        private DateTimePicker dtpFrom;
        private DateTimePicker dtpTo;
        private TextBox txtAppFilter;
        private ComboBox cboCategoryFilter;
        private Button btnFilterHistory;
        private DataGridView dgvHistory;
        private TabPage tabDashboard;
        private Label lblTotalSessions;
        private TabControl tabControl1;
        private Label lblMostUsedApp;
        private Label lblTotalTime;
        private GroupBox groupBox2;
        private Panel pnlChart;
        private GroupBox grpCategory;
        private Label lblCategoriesList;
        private ListBox lstCategories;
        private Label lblMinutes;
        private NumericUpDown nudDailyGoal;
        private Button btnSaveCategorySettings;
        private Button btnRemoveFromIgnore;
        private Label lblIgnoreInput;
        private GroupBox groupBox3;
        private TextBox txtIgnoreAppName;
        private Button btnAddToIgnore;
        private Label lblIgnoredApps;
        private ListBox lstIgnoreList;
        private TextBox txtCategoryName;
        private Label lblCategoryInput;
        private Button btnAddCategory;
        private Button btnRemoveFromCategories;
        private GroupBox grpApplicationClassification;
        private DataGridView dataGridView1;
        private DataGridViewTextBoxColumn colApplication;
        private DataGridViewComboBoxColumn colCategory;
        private ToolStripMenuItem exportToPdfToolStripMenuItemd;
        private Label label2;
        private Label label1;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private ToolStripMenuItem aboutToolStripMenuItem1;
        private Button exportToPdfToolStripMenuItem1;
        private Button exportToPdfToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem1;
    }
}
