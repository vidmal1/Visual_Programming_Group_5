using FocusTrack.Forms;
using FocusTrack.Helpers;
using FocusTrack.Models;
using FocusTrack.Services;
using FocusTrack.Data;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.VisualElements;
using LiveChartsCore.SkiaSharpView.WinForms;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System.Drawing.Drawing2D;
using System.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using PdfSharp.Fonts;
using System.IO;

namespace FocusTrack
{
    public partial class MainForm : Form
    {
        private readonly IDashboardService dashboardService = new DashboardService();
        private readonly TrackingService trackingService;
        private readonly System.Windows.Forms.Timer dashboardRefreshTimer = new();
        private readonly Label totalTimeValue = CreateValueLabel();
        private readonly Label productiveValue = CreateValueLabel();
        private readonly Label neutralValue = CreateValueLabel();
        private readonly Label distractingValue = CreateValueLabel();
        private readonly Label sessionsValue = CreateValueLabel();
        private readonly Label longestSessionValue = CreateValueLabel();
        private readonly Label mostUsedAppValue = CreateValueLabel();
        private readonly Label statusLabel = new();
        private readonly Panel categoryChartPanel = new();
        private readonly Panel appChartPanel = new();
        private readonly FlowLayoutPanel goalProgressPanel = new();
        private readonly DataGridView recentSessionsGrid = new();
        private readonly CartesianChart reportBarChart = new();
        private readonly PieChart reportPieChart = new();
        private readonly Label reportEmptyStateLabel = new();
        private DashboardReport currentReport = new();
        private bool isRefreshing;
        private readonly IAppSessionRepository sessionRepository = new AppSessionRepository();
        private readonly List<AppCategory> settingsCategories = new();
        private readonly List<AppClassification> settingsClassifications = new();
        private readonly Button btnRemoveCategory = new();
        private readonly TextBox txtClassifiedAppName = new();
        private readonly ComboBox cboClassificationCategory = new();
        private readonly Button btnSaveClassification = new();
        private readonly Button btnRemoveClassification = new();
        private readonly ListBox lstClassifications = new();
        private readonly Button btnReloadSettings = new();
        private bool isBindingSettings;

        public MainForm()
        {
            InitializeComponent();

            try
            {
                GlobalFontSettings.FontResolver = new FocusTrackFontResolver();
            }
            catch (InvalidOperationException)
            {
                // Already registered
            }

            BuildLayout();
            ConfigureRefreshTimer();
            RegisterNavigationEvents();
            InitializeHistorySection();
            InitializeReportsSection();
            trackingService = new TrackingService();
            InitializeSettingsSection();
        }

        protected override async void OnShown(EventArgs e)
        {
            base.OnShown(e);
            trackingService.StartTracking();
            await RefreshDashboardAsync();
            dashboardRefreshTimer.Start();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            trackingService.StopTracking();
            trackingService.Dispose();
            dashboardRefreshTimer.Stop();
            dashboardRefreshTimer.Dispose();
            base.OnFormClosed(e);
        }


        private void InitializeHistorySection()
        {
            // Default date range: last 7 days
            dtpFrom.Value = DateTime.Now.Date.AddDays(-7);
            dtpTo.Value = DateTime.Now.Date.AddDays(1).AddSeconds(-1);

            // ── Professional DataGridView styling ──────────────────────────────
            dgvHistory.AllowUserToAddRows = false;
            dgvHistory.AllowUserToDeleteRows = false;
            dgvHistory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvHistory.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvHistory.ReadOnly = true;
            dgvHistory.MultiSelect = false;

            // Fonts
            dgvHistory.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);

            // Column header styling – standard windows look
            dgvHistory.EnableHeadersVisualStyles = true;
            dgvHistory.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            dgvHistory.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvHistory.ColumnHeadersDefaultCellStyle.Padding = new Padding(6, 0, 0, 0);
            dgvHistory.ColumnHeadersHeight = 38;
            dgvHistory.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            // Row height & cell padding
            dgvHistory.RowTemplate.Height = 32;
            dgvHistory.DefaultCellStyle.Padding = new Padding(6, 4, 6, 4);
            dgvHistory.DefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            dgvHistory.DefaultCellStyle.ForeColor = Color.FromArgb(30, 30, 50);
            dgvHistory.DefaultCellStyle.BackColor = Color.White;
            dgvHistory.DefaultCellStyle.SelectionBackColor = Color.FromArgb(63, 131, 248);   // bright blue
            dgvHistory.DefaultCellStyle.SelectionForeColor = Color.White;

            // Alternating row colour for readability
            dgvHistory.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 244, 255);
            dgvHistory.AlternatingRowsDefaultCellStyle.ForeColor = Color.FromArgb(30, 30, 50);

            // Grid lines & border
            dgvHistory.GridColor = Color.FromArgb(220, 225, 235);
            dgvHistory.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvHistory.BorderStyle = BorderStyle.None;

            // Row header visibility
            dgvHistory.RowHeadersVisible = false;
            // ──────────────────────────────────────────────────────────────────

            btnFilterHistory.Click += btnFilterHistory_Click;

            // Load categories for filter
            _ = LoadHistoryCategoriesAsync();

            // Initial load
            _ = btnFilterHistory_ClickAsync();
        }

        private void InitializeReportsSection()
        {
            dtpReportFrom.Value = DateTime.Now.Date.AddDays(-7);
            dtpReportTo.Value = DateTime.Now.Date.AddDays(1).AddSeconds(-1);

            cboReportType.Items.Clear();
            cboReportType.Items.Add("By Application");
            cboReportType.Items.Add("By Category");
            cboReportType.SelectedIndex = 0;

            rdoTabularReport.Checked = true;
            rdoTabularReport.CheckedChanged += ReportsViewModeChanged;
            rdoChartReport.CheckedChanged += ReportsViewModeChanged;

            ConfigureReportPanels();
            UpdateReportsVisibility();

            btnGenerateReport.Click += btnGenerateReport_Click;
            exportToExcelToolStripMenuItem.Click += exportToExcelToolStripMenuItem_Click;

            _ = RefreshReportAsync();
        }

        private void ConfigureReportPanels()
        {
            dgvReports.Dock = DockStyle.Fill;
            dgvReports.AllowUserToAddRows = false;
            dgvReports.AllowUserToDeleteRows = false;
            dgvReports.ReadOnly = true;
            dgvReports.MultiSelect = false;
            dgvReports.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvReports.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvReports.RowHeadersVisible = false;
            dgvReports.BackgroundColor = Color.White;
            dgvReports.BorderStyle = BorderStyle.None;

            // ── Professional styling (mirrors History table) ──────────────────
            // Fonts
            dgvReports.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);

            // Column header — standard windows look
            dgvReports.EnableHeadersVisualStyles = true;
            dgvReports.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            dgvReports.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvReports.ColumnHeadersDefaultCellStyle.Padding = new Padding(6, 0, 0, 0);
            dgvReports.ColumnHeadersHeight = 38;
            dgvReports.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            // Row height & cell padding
            dgvReports.RowTemplate.Height = 32;
            dgvReports.DefaultCellStyle.Padding = new Padding(6, 4, 6, 4);
            dgvReports.DefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            dgvReports.DefaultCellStyle.ForeColor = Color.FromArgb(30, 30, 50);
            dgvReports.DefaultCellStyle.BackColor = Color.White;
            dgvReports.DefaultCellStyle.SelectionBackColor = Color.FromArgb(63, 131, 248);
            dgvReports.DefaultCellStyle.SelectionForeColor = Color.White;

            // Alternating row colour
            dgvReports.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 244, 255);
            dgvReports.AlternatingRowsDefaultCellStyle.ForeColor = Color.FromArgb(30, 30, 50);

            // Grid lines & border
            dgvReports.GridColor = Color.FromArgb(220, 225, 235);
            dgvReports.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            // ─────────────────────────────────────────────────────────────────


            pnlReportGridHost.Controls.Clear();
            pnlReportGridHost.Visible = false;

            pnlReportChartHost.Controls.Clear();
            pnlReportChartHost.BackColor = Color.White;
            pnlReportChartHost.Padding = new Padding(12);

            if (dgvReports.Parent != null)
            {
                dgvReports.Parent.Controls.Remove(dgvReports);
            }

            dgvReports.Parent = pnlReportChartHost;
            dgvReports.Dock = DockStyle.Fill;
            dgvReports.Visible = false;

            reportBarChart.Dock = DockStyle.Fill;
            reportBarChart.Margin = new Padding(0);
            reportBarChart.Visible = false;

            reportPieChart.Dock = DockStyle.Fill;
            reportPieChart.Margin = new Padding(0);
            reportPieChart.Visible = false;

            reportEmptyStateLabel.Dock = DockStyle.Fill;
            reportEmptyStateLabel.Text = "Run a report to see the results here.";
            reportEmptyStateLabel.TextAlign = ContentAlignment.MiddleCenter;
            reportEmptyStateLabel.ForeColor = Color.FromArgb(75, 85, 99);
            reportEmptyStateLabel.Font = new Font("Segoe UI", 11F);
            reportEmptyStateLabel.Visible = true;

            pnlReportChartHost.Controls.Add(dgvReports);
            pnlReportChartHost.Controls.Add(reportBarChart);
            pnlReportChartHost.Controls.Add(reportPieChart);
            pnlReportChartHost.Controls.Add(reportEmptyStateLabel);
            reportEmptyStateLabel.BringToFront();
        }

        private void ReportsViewModeChanged(object? sender, EventArgs e)
        {
            UpdateReportsVisibility();
        }

        private void UpdateReportsVisibility()
        {
            pnlReportGridHost.Visible = false;
            pnlReportChartHost.Visible = true;
            dgvReports.Visible = rdoTabularReport.Checked;

            if (rdoTabularReport.Checked)
            {
                dgvReports.BringToFront();
            }
            else
            {
                if (reportPieChart.Visible) reportPieChart.BringToFront();
                else if (reportBarChart.Visible) reportBarChart.BringToFront();
                else reportEmptyStateLabel.BringToFront();
            }
        }

        private async Task LoadHistoryCategoriesAsync()
        {
            try
            {
                await using var db = new AppDbContext();
                var categories = await db.AppCategories.OrderBy(c => c.Name).ToListAsync();
                cboCategoryFilter.Items.Clear();
                cboCategoryFilter.Items.Add("All");
                foreach (var c in categories) cboCategoryFilter.Items.Add(c.Name);
                cboCategoryFilter.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to load : " + ex.Message);
            }
        }

        private async void btnFilterHistory_Click(object sender, EventArgs e)
        {
            await btnFilterHistory_ClickAsync();
        }

        private async Task btnFilterHistory_ClickAsync()
        {
            var from = dtpFrom.Value.Date;
            var to = dtpTo.Value.Date.AddDays(1);

            var sessions = await sessionRepository.GetSessionsAsync(from, to);

            var filtered = sessions.AsEnumerable();

            var appFilter = txtAppFilter.Text?.Trim();
            if (!string.IsNullOrEmpty(appFilter))
            {
                filtered = filtered.Where(s => s.AppName != null && s.AppName.IndexOf(appFilter, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            if (cboCategoryFilter.SelectedItem != null && cboCategoryFilter.SelectedItem.ToString() != "All")
            {
                var cat = cboCategoryFilter.SelectedItem.ToString();
                filtered = filtered.Where(s => string.Equals(s.CategoryName, cat, StringComparison.OrdinalIgnoreCase));
            }

            PopulateHistoryGrid(filtered);
        }

        private void PopulateHistoryGrid(IEnumerable<DashboardSessionRecord> sessions)
        {
            var table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("Start Time", typeof(DateTime));
            table.Columns.Add("End Time", typeof(DateTime));
            table.Columns.Add("Duration", typeof(string));
            table.Columns.Add("Application", typeof(string));
            table.Columns.Add("Window Title", typeof(string));
            table.Columns.Add("Category", typeof(string));

            foreach (var s in sessions)
            {
                var duration = s.EndTime - s.StartTime;
                var durationStr = string.Format("{0:%h}h {0:%m}m", duration);
                table.Rows.Add(s.Id, s.StartTime, s.EndTime, durationStr, s.AppName, s.WindowTitle, s.CategoryName);
            }

            dgvHistory.DataSource = table;

            // Column visibility, headers & sizing
            if (dgvHistory.Columns.Count > 0)
            {
                // Hide internal Id
                dgvHistory.Columns[0].Visible = false;

                // Professional column header labels
                dgvHistory.Columns[1].HeaderText = "Start Time";
                dgvHistory.Columns[2].HeaderText = "End Time";
                dgvHistory.Columns[3].HeaderText = "Duration";
                dgvHistory.Columns[4].HeaderText = "Application";
                dgvHistory.Columns[5].HeaderText = "Window Title";
                dgvHistory.Columns[6].HeaderText = "Category";

                // Date format
                dgvHistory.Columns[1].DefaultCellStyle.Format = "g";
                dgvHistory.Columns[2].DefaultCellStyle.Format = "g";

                // Column widths
                dgvHistory.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dgvHistory.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dgvHistory.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dgvHistory.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvHistory.Columns[4].FillWeight = 120;
                dgvHistory.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvHistory.Columns[5].FillWeight = 180;
                dgvHistory.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

                // Centre-align Duration and Category
                dgvHistory.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvHistory.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
        }

        private async void btnGenerateReport_Click(object sender, EventArgs e)
        {
            await RefreshReportAsync();
        }

        private async Task RefreshReportAsync()
        {
            btnGenerateReport.Enabled = false;
            UseWaitCursor = true;

            try
            {
                var fromDate = dtpReportFrom.Value.Date;
                var toDate = dtpReportTo.Value.Date;

                if (fromDate > toDate)
                {
                    (fromDate, toDate) = (toDate, fromDate);
                }

                var sessions = await sessionRepository.GetSessionsAsync(fromDate, toDate.AddDays(1));
                var reportType = cboReportType.SelectedItem?.ToString();
                if (string.IsNullOrWhiteSpace(reportType))
                {
                    ShowEmptyReportState("Choose a group by option.");
                    return;
                }

                var items = reportType == "By Category"
                    ? await BuildCategoryReportItemsAsync(sessions)
                    : await Task.Run(() => BuildReportItems(sessions, reportType).ToList());

                if (items.Count == 0)
                {
                    ShowEmptyReportState("No tracked sessions found for the selected date range.");
                    return;
                }

                BindReportsGrid(items, reportType);

                if (rdoTabularReport.Checked)
                {
                    ShowTabularReport();
                }
                else
                {
                    ShowChartReport(items, reportType);
                }
            }
            catch (Exception ex)
            {
                ShowEmptyReportState($"Unable to load the report: {ex.Message}");
            }
            finally
            {
                UseWaitCursor = false;
                btnGenerateReport.Enabled = true;
            }
        }

        private void exportToExcelToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            if (dgvReports.Rows.Count == 0)
            {
                MessageBox.Show("No report data available to export. Please generate a report first.", "Export to Excel", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var saveFileDialog = new SaveFileDialog
            {
                Filter = "Excel Workbook (*.xlsx)|*.xlsx",
                Title = "Export Report to Excel",
                FileName = $"FocusTrack_Report_{DateTime.Now:yyyy_MM_dd_HH_mm_ss}.xlsx"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Cursor = Cursors.WaitCursor;

                    using var workbook = new ClosedXML.Excel.XLWorkbook();
                    var worksheet = workbook.Worksheets.Add("Application Usage Report");

                    // Add headers
                    for (int col = 0; col < dgvReports.Columns.Count; col++)
                    {
                        worksheet.Cell(1, col + 1).Value = dgvReports.Columns[col].HeaderText;
                        worksheet.Cell(1, col + 1).Style.Font.Bold = true;
                        worksheet.Cell(1, col + 1).Style.Fill.BackgroundColor = ClosedXML.Excel.XLColor.LightGray;
                    }

                    // Add data rows
                    for (int row = 0; row < dgvReports.Rows.Count; row++)
                    {
                        for (int col = 0; col < dgvReports.Columns.Count; col++)
                        {
                            var cellValue = dgvReports.Rows[row].Cells[col].Value;
                            worksheet.Cell(row + 2, col + 1).Value = cellValue?.ToString() ?? "";
                        }
                    }

                    worksheet.Columns().AdjustToContents();
                    workbook.SaveAs(saveFileDialog.FileName);

                    Cursor = Cursors.Default;
                    MessageBox.Show("Report exported successfully!", "Export to Excel", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    Cursor = Cursors.Default;
                    MessageBox.Show($"An error occurred while exporting: {ex.Message}", "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void exportToPdfToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            if (dgvReports.Rows.Count == 0)
            {
                MessageBox.Show("No report data available to export. Please generate a report first.", "Export to PDF", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var saveFileDialog = new SaveFileDialog
            {
                Filter = "PDF Document (*.pdf)|*.pdf",
                Title = "Export Report to PDF",
                FileName = $"FocusTrack_Report_{DateTime.Now:yyyy_MM_dd_HH_mm_ss}.pdf"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Cursor = Cursors.WaitCursor;
                    ExportToPdfDirect(saveFileDialog.FileName);
                    Cursor = Cursors.Default;
                    MessageBox.Show("Report exported successfully as PDF!", "Export to PDF", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    Cursor = Cursors.Default;
                    MessageBox.Show($"An error occurred while exporting to PDF: {ex.Message}", "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public async Task RunTestExportAsync()
        {
            Console.WriteLine("[TRACE] RunTestExportAsync started.");
            SeedMockDataIfEmpty();

            // Run application report
            Console.WriteLine("[TRACE] Selecting cboReportType index 0.");
            cboReportType.SelectedIndex = 0; // By Application
            dtpReportFrom.Value = DateTime.Now.Date.AddDays(-30);
            dtpReportTo.Value = DateTime.Now.Date.AddDays(1).AddSeconds(-1);

            Console.WriteLine("[TRACE] Awaiting RefreshReportAsync for Application.");
            await RefreshReportAsync();

            // Trigger PDF export
            Console.WriteLine("[TRACE] Exporting Application Report to PDF...");
            ExportToPdfDirect(@"C:\Users\tharu\Desktop\FocusTrack_AppReport.pdf");

            // Run category report
            Console.WriteLine("[TRACE] Selecting cboReportType index 1.");
            cboReportType.SelectedIndex = 1; // By Category
            Console.WriteLine("[TRACE] Awaiting RefreshReportAsync for Category.");
            await RefreshReportAsync();
            Console.WriteLine("[TRACE] Exporting Category Report to PDF...");
            ExportToPdfDirect(@"C:\Users\tharu\Desktop\FocusTrack_CategoryReport.pdf");
            Console.WriteLine("[TRACE] RunTestExportAsync completed.");
        }

        private void SeedMockDataIfEmpty()
        {
            try
            {
                using var db = new AppDbContext();
                if (db.AppSessions.Any()) return;

                var productive = db.AppCategories.FirstOrDefault(c => c.Name == "Productive");
                if (productive == null)
                {
                    productive = new AppCategory { Name = "Productive", DailyGoalMinutes = 120 };
                    db.AppCategories.Add(productive);
                }

                var distracting = db.AppCategories.FirstOrDefault(c => c.Name == "Distracting");
                if (distracting == null)
                {
                    distracting = new AppCategory { Name = "Distracting", DailyGoalMinutes = 30 };
                    db.AppCategories.Add(distracting);
                }

                var neutral = db.AppCategories.FirstOrDefault(c => c.Name == "Neutral");
                if (neutral == null)
                {
                    neutral = new AppCategory { Name = "Neutral", DailyGoalMinutes = 0 };
                    db.AppCategories.Add(neutral);
                }

                var social = db.AppCategories.FirstOrDefault(c => c.Name == "Communication");
                if (social == null)
                {
                    social = new AppCategory { Name = "Communication", DailyGoalMinutes = 60 };
                    db.AppCategories.Add(social);
                }

                var utilities = db.AppCategories.FirstOrDefault(c => c.Name == "Entertainment");
                if (utilities == null)
                {
                    utilities = new AppCategory { Name = "Entertainment", DailyGoalMinutes = 0 };
                    db.AppCategories.Add(utilities);
                }

                db.SaveChanges();

                var random = new Random();
                var apps = new[] {
                    new { Name = "chrome", Title = "Google Search - Google Chrome", Category = productive },
                    new { Name = "devenv", Title = "FocusTrack (Running) - Microsoft Visual Studio", Category = productive },
                    new { Name = "explorer", Title = "File Explorer", Category = neutral },
                    new { Name = "discord", Title = "Discord #general", Category = distracting },
                    new { Name = "spotify", Title = "Spotify Premium", Category = neutral },
                    new { Name = "cmd", Title = "Administrator: Command Prompt", Category = neutral }
                };

                var now = DateTime.Now;
                for (int day = 0; day < 7; day++)
                {
                    var date = now.AddDays(-day).Date;
                    int sessionCount = random.Next(5, 12);
                    for (int s = 0; s < sessionCount; s++)
                    {
                        var app = apps[random.Next(apps.Length)];
                        var startHour = random.Next(8, 18);
                        var startMin = random.Next(0, 60);
                        var startTime = date.AddHours(startHour).AddMinutes(startMin);
                        var durationMin = random.Next(5, 45);
                        var endTime = startTime.AddMinutes(durationMin);

                        db.AppSessions.Add(new AppSession
                        {
                            AppName = app.Name,
                            WindowTitle = app.Title,
                            StartTime = startTime,
                            EndTime = endTime,
                            Category = app.Category
                        });
                    }
                }
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Mock seeding error: " + ex.Message);
            }
        }

        private void ExportToPdfDirect(string fileName)
        {
            // 1. Reconstruct Report Items from grid
            var items = new List<ReportItem>();
            foreach (DataGridViewRow row in dgvReports.Rows)
            {
                if (row.IsNewRow) continue;
                items.Add(new ReportItem
                {
                    Name = row.Cells["colName"].Value?.ToString() ?? "",
                    SessionCount = Convert.ToInt32(row.Cells["colSessions"].Value ?? 0),
                    TotalMinutes = Convert.ToDouble(row.Cells["colTotalMinutes"].Value ?? 0.0)
                });
            }

            // 2. Capture Chart Bitmap
            bool originalBarVisible = reportBarChart.Visible;
            bool originalPieVisible = reportPieChart.Visible;
            bool originalGridVisible = dgvReports.Visible;
            bool originalEmptyVisible = reportEmptyStateLabel.Visible;

            DockStyle originalBarDock = reportBarChart.Dock;
            DockStyle originalPieDock = reportPieChart.Dock;

            Size originalBarSize = reportBarChart.Size;
            Size originalPieSize = reportPieChart.Size;

            var reportType = cboReportType.SelectedItem?.ToString() ?? "By Application";
            Control chartControl = reportType == "By Category" ? (Control)reportPieChart : (Control)reportBarChart;

            // Temporarily bind and render the chart at high resolution for quality export
            ShowChartReport(items, reportType);
            chartControl.Dock = DockStyle.None;
            chartControl.Size = new Size(1000, 600);
            chartControl.Visible = true;
            chartControl.BringToFront();

            chartControl.Refresh();
            Application.DoEvents();

            using Bitmap chartBmp = new Bitmap(1000, 600);
            chartControl.DrawToBitmap(chartBmp, new Rectangle(0, 0, 1000, 600));

            // Restore original UI state
            reportBarChart.Dock = originalBarDock;
            reportPieChart.Dock = originalPieDock;
            reportBarChart.Size = originalBarSize;
            reportPieChart.Size = originalPieSize;

            reportBarChart.Visible = originalBarVisible;
            reportPieChart.Visible = originalPieVisible;
            dgvReports.Visible = originalGridVisible;
            reportEmptyStateLabel.Visible = originalEmptyVisible;

            if (originalGridVisible) dgvReports.BringToFront();
            else if (originalPieVisible) reportPieChart.BringToFront();
            else if (originalBarVisible) reportBarChart.BringToFront();
            else reportEmptyStateLabel.BringToFront();

            Application.DoEvents();

            // 3. Create PDF Document using PdfSharp
            using var document = new PdfDocument();
            document.Info.Title = "FocusTrack Productivity Report";

            var page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);

            try
            {
                // Styling definitions
                var primaryBrush = new XSolidBrush(XColor.FromArgb(37, 99, 235)); // Sleek Blue (#2563eb)
                var darkBrush = new XSolidBrush(XColor.FromArgb(31, 41, 55)); // Very dark gray (#1f2937)
                var grayBrush = new XSolidBrush(XColor.FromArgb(107, 114, 128)); // Muted gray (#6b7280)
                var gridPen = new XPen(XColor.FromArgb(229, 231, 235), 0.5); // Light line (#e5e7eb)
                var headerPen = new XPen(XColor.FromArgb(107, 114, 128), 1.0);

                // Fonts
                var titleFont = new XFont("Segoe UI", 20, XFontStyleEx.Bold);
                var headerFont = new XFont("Segoe UI", 12, XFontStyleEx.Bold);
                var subHeaderFont = new XFont("Segoe UI", 10, XFontStyleEx.Regular);
                var boldTextFont = new XFont("Segoe UI", 9, XFontStyleEx.Bold);
                var regularTextFont = new XFont("Segoe UI", 9, XFontStyleEx.Regular);

                // Render Header Area
                gfx.DrawString("FocusTrack Productivity Report", titleFont, primaryBrush, new XRect(40.0, 40.0, page.Width.Point - 80.0, 30.0), XStringFormats.TopLeft);

                var fromDate = dtpReportFrom.Value.Date;
                var toDate = dtpReportTo.Value.Date;
                string subtitle = $"Period: {fromDate:yyyy-MM-dd} to {toDate:yyyy-MM-dd}   |   Grouping: {reportType}";
                gfx.DrawString(subtitle, subHeaderFont, grayBrush, new XRect(40.0, 75.0, page.Width.Point - 80.0, 20.0), XStringFormats.TopLeft);

                gfx.DrawLine(gridPen, 40.0, 95.0, page.Width.Point - 40.0, 95.0);

                // Embed Chart Image in PDF
                using var ms = new MemoryStream();
                chartBmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                ms.Position = 0;
                using var xImage = XImage.FromStream(ms);

                double chartWidth = page.Width.Point - 80.0;
                double chartHeight = 240.0;
                gfx.DrawImage(xImage, 40.0, 110.0, chartWidth, chartHeight);

                // Data Breakdown Header
                double tableY = 370.0;
                gfx.DrawString("Data Breakdown", headerFont, darkBrush, new XRect(40.0, tableY, page.Width.Point - 80.0, 20.0), XStringFormats.TopLeft);
                tableY += 25.0;

                // Table Columns coordinates
                double colNameX = 40.0;
                double colSessionsX = page.Width.Point - 260.0;
                double colMinutesX = page.Width.Point - 160.0;
                double colTotalTimeX = page.Width.Point - 40.0;

                // Draw Table Header Row
                gfx.DrawLine(headerPen, 40.0, tableY, page.Width.Point - 40.0, tableY);
                tableY += 5.0;

                string colNameHeader = reportType == "By Category" ? "Category" : "Application";
                gfx.DrawString(colNameHeader, boldTextFont, darkBrush, new XRect(colNameX, tableY, 200.0, 20.0), XStringFormats.TopLeft);
                gfx.DrawString("Sessions", boldTextFont, darkBrush, new XRect(colSessionsX, tableY, 80.0, 20.0), XStringFormats.TopLeft);
                gfx.DrawString("Minutes", boldTextFont, darkBrush, new XRect(colMinutesX, tableY, 80.0, 20.0), XStringFormats.TopLeft);

                var rightFormat = new XStringFormat { Alignment = XStringAlignment.Far };
                gfx.DrawString("Total Time", boldTextFont, darkBrush, new XRect(colTotalTimeX - 100.0, tableY, 100.0, 20.0), rightFormat);

                tableY += 15.0;
                gfx.DrawLine(headerPen, 40.0, tableY, page.Width.Point - 40.0, tableY);
                tableY += 5.0;

                // Draw Table Rows
                foreach (var item in items)
                {
                    // Handle page overflow with automatic new page generation
                    if (tableY > page.Height.Point - 60.0)
                    {
                        gfx.Dispose();
                        page = document.AddPage();
                        gfx = XGraphics.FromPdfPage(page);

                        tableY = 40.0;
                        gfx.DrawLine(headerPen, 40.0, tableY, page.Width.Point - 40.0, tableY);
                        tableY += 5.0;
                        gfx.DrawString(colNameHeader, boldTextFont, darkBrush, new XRect(colNameX, tableY, 200.0, 20.0), XStringFormats.TopLeft);
                        gfx.DrawString("Sessions", boldTextFont, darkBrush, new XRect(colSessionsX, tableY, 80.0, 20.0), XStringFormats.TopLeft);
                        gfx.DrawString("Minutes", boldTextFont, darkBrush, new XRect(colMinutesX, tableY, 80.0, 20.0), XStringFormats.TopLeft);
                        gfx.DrawString("Total Time", boldTextFont, darkBrush, new XRect(colTotalTimeX - 100.0, tableY, 100.0, 20.0), rightFormat);
                        tableY += 15.0;
                        gfx.DrawLine(headerPen, 40.0, tableY, page.Width.Point - 40.0, tableY);
                        tableY += 5.0;
                    }

                    string displayName = item.Name;
                    if (displayName.Length > 40) displayName = displayName.Substring(0, 37) + "...";

                    gfx.DrawString(displayName, regularTextFont, darkBrush, new XRect(colNameX, tableY, colSessionsX - colNameX - 10.0, 20.0), XStringFormats.TopLeft);
                    gfx.DrawString(item.SessionCount.ToString(), regularTextFont, darkBrush, new XRect(colSessionsX, tableY, 80.0, 20.0), XStringFormats.TopLeft);
                    gfx.DrawString(Math.Round(item.TotalMinutes, 0).ToString(), regularTextFont, darkBrush, new XRect(colMinutesX, tableY, 80.0, 20.0), XStringFormats.TopLeft);

                    string totalTimeStr = FormatDuration(item.TotalMinutes);
                    gfx.DrawString(totalTimeStr, regularTextFont, darkBrush, new XRect(colTotalTimeX - 100.0, tableY, 100.0, 20.0), rightFormat);

                    tableY += 20.0;
                    gfx.DrawLine(gridPen, 40.0, tableY, page.Width.Point - 40.0, tableY);
                    tableY += 5.0;
                }

                // Add Page Footer
                gfx.DrawString("Report generated by FocusTrack", regularTextFont, grayBrush, new XRect(40.0, page.Height.Point - 35.0, page.Width.Point - 80.0, 20.0), XStringFormats.TopLeft);
            }
            finally
            {
                gfx.Dispose();
            }

            document.Save(fileName);
        }

        private static IEnumerable<ReportItem> BuildReportItems(IEnumerable<DashboardSessionRecord> sessions, string reportType)
        {
            if (reportType == "By Category")
            {
                return sessions
                    .GroupBy(session => string.IsNullOrWhiteSpace(session.CategoryName) ? "Uncategorized" : session.CategoryName.Trim())
                    .Select(group => new ReportItem
                    {
                        Name = group.Key,
                        SessionCount = group.Count(),
                        TotalMinutes = group.Sum(session => Math.Max(0, (session.EndTime - session.StartTime).TotalMinutes))
                    })
                    .OrderByDescending(item => item.TotalMinutes)
                    .ThenBy(item => item.Name);
            }

            return sessions
                .GroupBy(session => string.IsNullOrWhiteSpace(session.AppName) ? "Unknown app" : session.AppName.Trim())
                .Select(group => new ReportItem
                {
                    Name = group.Key,
                    SessionCount = group.Count(),
                    TotalMinutes = group.Sum(session => Math.Max(0, (session.EndTime - session.StartTime).TotalMinutes))
                })
                .OrderByDescending(item => item.TotalMinutes)
                .ThenBy(item => item.Name);
        }

        private async Task<List<ReportItem>> BuildCategoryReportItemsAsync(IEnumerable<DashboardSessionRecord> sessions)
        {
            var categories = new List<string>();

            try
            {
                await using var db = new AppDbContext();
                categories = await db.AppCategories
                    .Select(category => category.Name)
                    .OrderBy(name => name)
                    .ToListAsync();
            }
            catch
            {
                // Fall back to categories found in the session data.
            }

            if (categories.Count == 0)
            {
                categories = sessions
                    .Select(session => string.IsNullOrWhiteSpace(session.CategoryName) ? "Uncategorized" : session.CategoryName.Trim())
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .OrderBy(name => name)
                    .ToList();
            }

            var grouped = sessions
                .GroupBy(session => string.IsNullOrWhiteSpace(session.CategoryName) ? "Uncategorized" : session.CategoryName.Trim(), StringComparer.OrdinalIgnoreCase)
                .ToDictionary(group => group.Key, group => new
                {
                    Count = group.Count(),
                    Minutes = group.Sum(session => Math.Max(0, (session.EndTime - session.StartTime).TotalMinutes))
                }, StringComparer.OrdinalIgnoreCase);

            var items = categories
                .Select(categoryName => grouped.TryGetValue(categoryName, out var summary)
                    ? new ReportItem
                    {
                        Name = categoryName,
                        SessionCount = summary.Count,
                        TotalMinutes = summary.Minutes
                    }
                    : new ReportItem
                    {
                        Name = categoryName,
                        SessionCount = 0,
                        TotalMinutes = 0
                    })
                .Where(item => item.TotalMinutes > 0 || item.SessionCount > 0)
                .OrderByDescending(item => item.TotalMinutes)
                .ThenBy(item => item.Name)
                .ToList();

            return items;
        }

        private void BindReportsGrid(IReadOnlyList<ReportItem> items, string reportType)
        {
            dgvReports.SuspendLayout();
            dgvReports.Columns.Clear();
            dgvReports.Rows.Clear();
            dgvReports.AutoGenerateColumns = false;

            dgvReports.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colName",
                HeaderText = reportType == "By Category" ? "Category" : "Application",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            dgvReports.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colSessions",
                HeaderText = "Sessions",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });

            dgvReports.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colTotalMinutes",
                HeaderText = "Total Minutes",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });

            dgvReports.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colTotalTime",
                HeaderText = "Total Time",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });

            foreach (var item in items)
            {
                dgvReports.Rows.Add(item.Name, item.SessionCount, Math.Round(item.TotalMinutes, 0), FormatDuration(item.TotalMinutes));
            }

            dgvReports.ClearSelection();
            dgvReports.ResumeLayout();
        }

        private void ShowTabularReport()
        {
            pnlReportGridHost.Visible = false;
            pnlReportChartHost.Visible = true;
            dgvReports.Visible = true;
            dgvReports.BringToFront();
            dgvReports.Refresh();
        }

        private void ShowChartReport(IReadOnlyList<ReportItem> items, string reportType)
        {
            pnlReportGridHost.Visible = false;
            pnlReportChartHost.Visible = true;
            dgvReports.Visible = false;
            pnlReportChartHost.BringToFront();

            reportEmptyStateLabel.Visible = false;

            if (reportType == "By Category")
            {
                reportBarChart.Visible = false;
                reportPieChart.Visible = true;
                reportPieChart.LegendPosition = LiveChartsCore.Measure.LegendPosition.Right;
                reportPieChart.Series = items.Select(item => new PieSeries<double>
                {
                    Name = item.Name,
                    Values = new[] { item.TotalMinutes },
                    Fill = new SolidColorPaint(GetChartCategoryColor(item.Name)),
                    //MaxOuterRadius = 140
                }).ToArray();
            }
            else
            {
                reportPieChart.Visible = false;
                reportBarChart.Visible = true;
                reportBarChart.LegendPosition = LiveChartsCore.Measure.LegendPosition.Hidden;

                using var db = new AppDbContext();
                var categoryMap = db.AppCategories.ToDictionary(c => c.Id, c => c.Name);
                var appClassMap = db.AppClassifications.ToDictionary(c => c.AppName.ToLower(), c => c.CategoryId);

                var neutralCategory = categoryMap.FirstOrDefault(kvp => kvp.Value.Equals("neutral", StringComparison.OrdinalIgnoreCase));
                int neutralId = neutralCategory.Key != 0 ? neutralCategory.Key : (categoryMap.Keys.FirstOrDefault());

                var series = new ColumnSeries<double>
                {
                    Name = "",
                    Values = items.Select(item => item.TotalMinutes).ToArray(),
                    YToolTipLabelFormatter = (point) => $"{Math.Round(point.Model, 0)} min",
                    MaxBarWidth = 60,
                    Rx = 10,
                    Ry = 10
                };

                series.PointMeasured += (point) =>
                {
                    if (point.Visual is null) return;

                    var item = items[point.Index];
                    int catId = appClassMap.TryGetValue(item.Name.ToLower(), out var id) ? id : neutralId;
                    string catName = categoryMap.TryGetValue(catId, out var name) ? name : "Neutral";

                    point.Visual.Fill = new SolidColorPaint(GetChartCategoryColor(catName));
                };

                reportBarChart.Series = new ISeries[] { series };

                var axisTitlePaint = new SolidColorPaint(new SKColor(90, 90, 90));

                reportBarChart.XAxes = new[]
                {
                    new Axis
                    {
                        Name = "Applications",
                        NameTextSize = 12,
                        NamePaint = axisTitlePaint,
                        Labels = items.Select(item => item.Name.Length > 14 ? item.Name.Substring(0, 11) + "..." : item.Name).ToArray(),
                        LabelsRotation = 90,
                        MinStep = 1,
                        TextSize = 10
                    }
                };

                reportBarChart.YAxes = new[]
                {
                    new Axis
                    {
                        Name = "Minutes",
                        NameTextSize = 12,
                        NamePaint = axisTitlePaint,
                        TextSize = 10
                    }
                };
            }
        }

        private SKColor GetChartCategoryColor(string categoryName)
        {
            var c = GetCategoryColor(categoryName);
            return new SKColor(c.R, c.G, c.B, c.A);
        }

        private void ShowEmptyReportState(string message)
        {
            pnlReportGridHost.Visible = false;
            pnlReportChartHost.Visible = true;
            dgvReports.Visible = false;
            reportBarChart.Visible = false;
            reportPieChart.Visible = false;
            reportBarChart.Series = Array.Empty<ISeries>();
            reportPieChart.Series = Array.Empty<ISeries>();
            reportBarChart.LegendPosition = LiveChartsCore.Measure.LegendPosition.Hidden;
            reportPieChart.LegendPosition = LiveChartsCore.Measure.LegendPosition.Hidden;
            reportEmptyStateLabel.Text = message;
            reportEmptyStateLabel.Visible = true;
            reportEmptyStateLabel.BringToFront();
        }

        private void RegisterNavigationEvents()
        {

            // Dynamic safe check for the dashboard item
            var dashboardItem = this.MainMenuStrip?.Items.Find("dashboardToolStripMenuItem", true).FirstOrDefault();
            if (dashboardItem != null) dashboardItem.Click += (s, e) => { foreach (Control c in Controls) { if (c is TabControl t) t.SelectedIndex = 0; } };

            //if (this.historyToolStripMenuItem != null) this.historyToolStripMenuItem.Click += (s, e) => { foreach (Control c in Controls) { if (c is TabControl t) t.SelectedIndex = 1; } };
            //if (this.reportsToolStripMenuItem != null) this.reportsToolStripMenuItem.Click += (s, e) => { foreach (Control c in Controls) { if (c is TabControl t) t.SelectedIndex = 1; } };
            //if (this.settingsToolStripMenuItem != null) this.settingsToolStripMenuItem.Click += (s, e) => { foreach (Control c in Controls) { if (c is TabControl t) t.SelectedIndex = 2; } };

            if (this.exitToolStripMenuItem != null) this.exitToolStripMenuItem.Click += (s, e) => Application.Exit();
        }
        private void BuildLayout()
        {
            BackColor = Color.FromArgb(245, 247, 250);

            var tabs = new TabControl
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10F)
            };

            var dashboardTab = new TabPage("Dashboard")
            {
                BackColor = Color.FromArgb(245, 247, 250),
                Padding = new Padding(16)
            };
            var historyTab = new TabPage("History")
            {
                BackColor = Color.White,
                Padding = new Padding(16)
            };
            var settingsTab = new TabPage("Settings")
            {
                BackColor = Color.White,
                Padding = new Padding(16)
            };

            historyTab.Controls.Add(CreatePlaceholderLabel("History browser will list past sessions with date, app, and category filters."));
            settingsTab.Controls.Add(CreatePlaceholderLabel("Settings will manage categories, ignored applications, and daily category goals."));

            BuildDashboardTab(dashboardTab);

            tabs.TabPages.Add(dashboardTab);
            tabs.TabPages.Add(historyTab);
            tabs.TabPages.Add(settingsTab);
            Controls.Add(tabs);
        }

        private void BuildDashboardTab(TabPage dashboardTab)
        {
            var root = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 4,
                BackColor = Color.Transparent
            };
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 118));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 48));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 132));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 52));

            var summaryGrid = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 7,
                RowCount = 1,
                BackColor = Color.Transparent
            };

            for (var i = 0; i < 7; i++)
            {
                summaryGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14.28F));
            }

            summaryGrid.Controls.Add(CreateSummaryCard("Total today", totalTimeValue, Color.FromArgb(31, 41, 55)), 0, 0);
            summaryGrid.Controls.Add(CreateSummaryCard("Productive", productiveValue, Color.FromArgb(22, 163, 74)), 1, 0);
            summaryGrid.Controls.Add(CreateSummaryCard("Neutral", neutralValue, Color.FromArgb(37, 99, 235)), 2, 0);
            summaryGrid.Controls.Add(CreateSummaryCard("Distracting", distractingValue, Color.FromArgb(220, 38, 38)), 3, 0);
            summaryGrid.Controls.Add(CreateSummaryCard("Sessions", sessionsValue, Color.FromArgb(79, 70, 229)), 4, 0);
            summaryGrid.Controls.Add(CreateSummaryCard("Longest", longestSessionValue, Color.FromArgb(217, 119, 6)), 5, 0);
            summaryGrid.Controls.Add(CreateSummaryCard("Most used", mostUsedAppValue, Color.FromArgb(31, 41, 55)), 6, 0);

            var chartsGrid = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = Color.Transparent,
                Padding = new Padding(0, 12, 0, 12)
            };
            chartsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 42));
            chartsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 58));

            categoryChartPanel.Dock = DockStyle.Fill;
            categoryChartPanel.BackColor = Color.White;
            categoryChartPanel.Paint += CategoryChartPanelPaint;

            appChartPanel.Dock = DockStyle.Fill;
            appChartPanel.BackColor = Color.White;
            appChartPanel.Paint += AppChartPanelPaint;

            chartsGrid.Controls.Add(CreatePanelShell("Category distribution", categoryChartPanel), 0, 0);
            chartsGrid.Controls.Add(CreatePanelShell("Top applications", appChartPanel), 1, 0);

            var goalsShell = CreatePanelShell("Daily goals", goalProgressPanel);
            goalProgressPanel.Dock = DockStyle.Fill;
            goalProgressPanel.FlowDirection = FlowDirection.LeftToRight;
            goalProgressPanel.WrapContents = false;
            goalProgressPanel.AutoScroll = true;
            goalProgressPanel.BackColor = Color.White;

            var sessionsShell = CreatePanelShell("Recent sessions", recentSessionsGrid);
            ConfigureRecentSessionsGrid();

            statusLabel.Dock = DockStyle.Bottom;
            statusLabel.Height = 24;
            statusLabel.TextAlign = ContentAlignment.MiddleRight;
            statusLabel.ForeColor = Color.FromArgb(75, 85, 99);

            root.Controls.Add(summaryGrid, 0, 0);
            root.Controls.Add(chartsGrid, 0, 1);
            root.Controls.Add(goalsShell, 0, 2);
            root.Controls.Add(sessionsShell, 0, 3);

            dashboardTab.Controls.Add(root);
            dashboardTab.Controls.Add(statusLabel);
        }

        private void ConfigureRefreshTimer()
        {
            dashboardRefreshTimer.Interval = 30000;
            dashboardRefreshTimer.Tick += async (_, _) => await RefreshDashboardAsync();
        }

        private async Task RefreshDashboardAsync()
        {
            if (isRefreshing)
            {
                return;
            }

            try
            {
                isRefreshing = true;
                await LoadDashboardSummaryAsync();
                await LoadDashboardChartAsync();
                statusLabel.Text = $"Updated {DateTime.Now:HH:mm:ss} | Daily usage loaded";
            }
            finally
            {
                isRefreshing = false;
            }
        }

        private async Task LoadDashboardSummaryAsync()
        {
            var todaySessions = await Task.Run(() => StorageHelper.GetTodaySessions());

            var totalSessionsToday = todaySessions.Count;
            var totalMinutesToday = todaySessions.Sum(session => Math.Max(0, (session.EndTime - session.StartTime).TotalMinutes));
            var totalHoursToday = totalMinutesToday / 60.0;
            var mostUsedApp = totalSessionsToday > 0
                ? todaySessions
                    .GroupBy(session => string.IsNullOrWhiteSpace(session.AppName) ? "Unknown app" : session.AppName.Trim())
                    .OrderByDescending(group => group.Count())
                    .ThenBy(group => group.Key)
                    .First()
                    .Key
                : "None";

            lblTotalSessions.Text = $"Total Sessions Today: {totalSessionsToday}";
            lblTotalTime.Text = $"Total Time Today: {totalHoursToday:0.##} hrs";
            lblMostUsedApp.Text = $"Most Used App: {mostUsedApp}";
        }

        private void ApplyDashboardReport()
        {
            totalTimeValue.Text = FormatDuration(currentReport.TotalMinutes);
            productiveValue.Text = FormatDuration(currentReport.ProductiveMinutes);
            neutralValue.Text = FormatDuration(currentReport.NeutralMinutes);
            distractingValue.Text = FormatDuration(currentReport.DistractingMinutes);
            sessionsValue.Text = currentReport.SessionCount.ToString();
            longestSessionValue.Text = FormatDuration(currentReport.LongestSessionMinutes);
            mostUsedAppValue.Text = currentReport.MostUsedApp;

            RenderGoalProgress();
            BindRecentSessions();
            categoryChartPanel.Invalidate();
            appChartPanel.Invalidate();
            statusLabel.Text = $"Updated {DateTime.Now:HH:mm:ss} | Most used app: {currentReport.MostUsedApp}";
        }

        private void RenderGoalProgress()
        {
            goalProgressPanel.Controls.Clear();

            foreach (var category in currentReport.Categories)
            {
                var color = GetCategoryColor(category.CategoryName);
                var panel = new Panel
                {
                    Width = 330,
                    Height = 76,
                    Margin = new Padding(0, 0, 12, 0),
                    BackColor = Color.White
                };

                var name = new Label
                {
                    Text = category.CategoryName,
                    Location = new Point(0, 0),
                    Size = new Size(160, 24),
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    ForeColor = color
                };

                var value = new Label
                {
                    Text = category.DailyGoalMinutes > 0
                        ? $"{FormatDuration(category.TotalMinutes)} / {category.DailyGoalMinutes}m"
                        : $"{FormatDuration(category.TotalMinutes)} / no goal",
                    Location = new Point(170, 0),
                    Size = new Size(150, 24),
                    TextAlign = ContentAlignment.MiddleRight,
                    ForeColor = category.IsGoalExceeded ? Color.FromArgb(185, 28, 28) : Color.FromArgb(55, 65, 81)
                };

                var progress = new ProgressBar
                {
                    Location = new Point(0, 34),
                    Size = new Size(320, 18),
                    Maximum = 100,
                    Value = Math.Max(0, Math.Min(100, (int)Math.Round(category.GoalProgressPercent)))
                };

                var status = new Label
                {
                    Text = category.IsGoalExceeded ? "Goal exceeded" : "On track",
                    Location = new Point(0, 54),
                    Size = new Size(320, 20),
                    ForeColor = category.IsGoalExceeded ? Color.FromArgb(185, 28, 28) : Color.FromArgb(75, 85, 99)
                };

                panel.Controls.Add(name);
                panel.Controls.Add(value);
                panel.Controls.Add(progress);
                panel.Controls.Add(status);
                goalProgressPanel.Controls.Add(panel);
            }
        }

        private void BindRecentSessions()
        {
            recentSessionsGrid.DataSource = currentReport.RecentSessions
                .Select(session => new RecentSessionRow
                {
                    App = session.AppName,
                    WindowTitle = session.WindowTitle,
                    Category = session.CategoryName,
                    Start = session.StartTime.ToString("HH:mm"),
                    End = session.EndTime.ToString("HH:mm"),
                    Duration = FormatDuration(session.DurationMinutes)
                })
                .ToList();
        }

        private void CategoryChartPanelPaint(object? sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.Clear(Color.White);
            DrawPanelTitle(e.Graphics, categoryChartPanel.ClientRectangle, "Time by category");

            var chartSize = Math.Max(80, Math.Min(220, Math.Min(categoryChartPanel.Width / 2, categoryChartPanel.Height - 78)));
            var chartBounds = new Rectangle(24, 52, chartSize, chartSize);
            var total = currentReport.Categories.Sum(category => category.TotalMinutes);

            if (total <= 0)
            {
                DrawEmptyState(e.Graphics, categoryChartPanel.ClientRectangle);
                return;
            }

            float startAngle = -90;
            foreach (var category in currentReport.Categories.Where(category => category.TotalMinutes > 0))
            {
                var sweep = (float)(category.TotalMinutes / total * 360);
                using var brush = new SolidBrush(GetCategoryColor(category.CategoryName));
                e.Graphics.FillPie(brush, chartBounds, startAngle, sweep);
                startAngle += sweep;
            }

            var legendX = chartBounds.Right + 28;
            var legendY = 62;
            foreach (var category in currentReport.Categories)
            {
                using var brush = new SolidBrush(GetCategoryColor(category.CategoryName));
                e.Graphics.FillRectangle(brush, legendX, legendY + 4, 14, 14);
                e.Graphics.DrawString(
                    $"{category.CategoryName}: {FormatDuration(category.TotalMinutes)}",
                    Font,
                    Brushes.Black,
                    legendX + 22,
                    legendY);
                legendY += 30;
            }
        }

        private void AppChartPanelPaint(object? sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.Clear(Color.White);
            DrawPanelTitle(e.Graphics, appChartPanel.ClientRectangle, "Top apps today");

            if (currentReport.TopApplications.Count == 0)
            {
                DrawEmptyState(e.Graphics, appChartPanel.ClientRectangle);
                return;
            }

            var left = 140;
            var top = 58;
            var right = Math.Max(left + 80, appChartPanel.Width - 28);
            var rowHeight = 34;
            var maxMinutes = currentReport.TopApplications.Max(app => app.TotalMinutes);

            for (var i = 0; i < currentReport.TopApplications.Count; i++)
            {
                var app = currentReport.TopApplications[i];
                var y = top + i * rowHeight;
                var barWidth = maxMinutes > 0 ? (int)((right - left) * (app.TotalMinutes / maxMinutes)) : 0;

                e.Graphics.DrawString(TrimText(app.AppName, 18), Font, Brushes.Black, 20, y + 4);
                using var brush = new SolidBrush(Color.FromArgb(37, 99, 235));
                e.Graphics.FillRectangle(brush, left, y + 6, Math.Max(3, barWidth), 18);
                e.Graphics.DrawString(FormatDuration(app.TotalMinutes), Font, Brushes.DimGray, left + barWidth + 8, y + 4);
            }
        }

        private static Panel CreateSummaryCard(string title, Label valueLabel, Color accentColor)
        {
            var card = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Margin = new Padding(0, 0, 12, 0),
                Padding = new Padding(14)
            };

            var titleLabel = new Label
            {
                Text = title,
                Dock = DockStyle.Top,
                Height = 26,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(75, 85, 99)
            };

            valueLabel.Dock = DockStyle.Fill;
            valueLabel.ForeColor = accentColor;

            card.Controls.Add(valueLabel);
            card.Controls.Add(titleLabel);
            return card;
        }

        private static Control CreatePanelShell(string title, Control content)
        {
            var panel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Margin = new Padding(0, 0, 12, 0),
                Padding = new Padding(14)
            };

            var titleLabel = new Label
            {
                Text = title,
                Dock = DockStyle.Top,
                Height = 28,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(31, 41, 55)
            };

            content.Dock = DockStyle.Fill;
            panel.Controls.Add(content);
            panel.Controls.Add(titleLabel);
            return panel;
        }

        private static Label CreateValueLabel()
        {
            return new Label
            {
                Text = "0m",
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft,
                AutoEllipsis = true
            };
        }

        private static Label CreatePlaceholderLabel(string text)
        {
            return new Label
            {
                Dock = DockStyle.Fill,
                Text = text,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 11F),
                ForeColor = Color.FromArgb(75, 85, 99)
            };
        }

        private void ConfigureRecentSessionsGrid()
        {
            recentSessionsGrid.Dock = DockStyle.Fill;
            recentSessionsGrid.AutoGenerateColumns = true;
            recentSessionsGrid.ReadOnly = true;
            recentSessionsGrid.AllowUserToAddRows = false;
            recentSessionsGrid.AllowUserToDeleteRows = false;
            recentSessionsGrid.RowHeadersVisible = false;
            recentSessionsGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            recentSessionsGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            recentSessionsGrid.BackgroundColor = Color.White;
            recentSessionsGrid.BorderStyle = BorderStyle.None;
        }

        private static void DrawPanelTitle(Graphics graphics, Rectangle bounds, string title)
        {
            using var titleFont = new Font("Segoe UI", 10F, FontStyle.Bold);
            graphics.DrawString(title, titleFont, Brushes.Black, 18, 18);
            using var pen = new Pen(Color.FromArgb(229, 231, 235));
            graphics.DrawLine(pen, 18, 44, bounds.Width - 18, 44);
        }

        private static void DrawEmptyState(Graphics graphics, Rectangle bounds)
        {
            var message = "No tracked sessions for today";
            using var messageFont = new Font("Segoe UI", 9F);
            var size = graphics.MeasureString(message, messageFont);
            graphics.DrawString(
                message,
                messageFont,
                Brushes.DimGray,
                (bounds.Width - size.Width) / 2,
                (bounds.Height - size.Height) / 2);
        }

        private static string FormatDuration(double minutes)
        {
            if (minutes <= 0)
            {
                return "0m";
            }

            var roundedMinutes = (int)Math.Round(minutes);
            var hours = roundedMinutes / 60;
            var remainingMinutes = roundedMinutes % 60;

            return hours > 0 ? $"{hours}h {remainingMinutes}m" : $"{remainingMinutes}m";
        }

        private static Color GetCategoryColor(string categoryName)
        {
            if (categoryName.Equals("Productive", StringComparison.OrdinalIgnoreCase))
            {
                return Color.FromArgb(22, 163, 74);     // Green
            }
            if (categoryName.Equals("Distracting", StringComparison.OrdinalIgnoreCase))
            {
                return Color.FromArgb(220, 38, 38);     // Red
            }
            if (categoryName.Equals("Communication", StringComparison.OrdinalIgnoreCase))
            {
                return Color.FromArgb(6, 182, 212);     // Cyan / Light Blue
            }
            if (categoryName.Equals("Entertainment", StringComparison.OrdinalIgnoreCase))
            {
                return Color.FromArgb(168, 85, 247);    // Purple / Violet
            }
            
            // Neutral and any other unknown category -> Gray / White
            return Color.FromArgb(156, 163, 175);
        }

        private static string TrimText(string text, int maxLength)
        {
            if (text.Length <= maxLength)
            {
                return text;
            }

            return text[..Math.Max(0, maxLength - 3)] + "...";
        }

        private sealed class ReportItem
        {
            public string Name { get; set; } = string.Empty;
            public int SessionCount { get; set; }
            public double TotalMinutes { get; set; }
        }

        private sealed class RecentSessionRow
        {
            public string App { get; set; } = string.Empty;
            public string WindowTitle { get; set; } = string.Empty;
            public string Category { get; set; } = string.Empty;
            public string Start { get; set; } = string.Empty;
            public string End { get; set; } = string.Empty;
            public string Duration { get; set; } = string.Empty;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

        }

        private void tabSettings_Click(object sender, EventArgs e)
        {

        }

        private void tabDashboard_Click(object sender, EventArgs e)
        {

        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm aboutForm = new AboutForm();
            aboutForm.ShowDialog();
        }

        //private void btnSave_Click(object sender, EventArgs e)
        //{
        //    MessageBox.Show(
        //"Settings saved successfully!",
        //"FocusTrack",
        //MessageBoxButtons.OK,
        //MessageBoxIcon.Information);
        //}

        //private void btnCancel_Click(object sender, EventArgs e)
        //{
        //    nudDailyGoal.Value = 8;
        //    chkMinimizeToTray.Checked = false;
        //}

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            string category = Microsoft.VisualBasic.Interaction.InputBox(
        "Enter category name",
        "Add Category",
        "");

            if (!string.IsNullOrWhiteSpace(category))
            {
                //lstCategories.Items.Add(category);
            }
        }

        private void btnRemoveCategory_Click(object sender, EventArgs e)
        {
            //if (lstCategories.SelectedItem != null)
            //{
            // lstCategories.Items.Remove(lstCategories.SelectedItem);
            //}
        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show(
        "Settings saved successfully!",
        "FocusTrack",
        MessageBoxButtons.OK,
        MessageBoxIcon.Information);
        }

        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            //nudDailyGoal.Value = 8;
            //chkMinimizeToTray.Checked = false;
        }

        private async Task LoadDashboardChartAsync()
        {
            var todaySessions = await Task.Run(() => StorageHelper.GetTodaySessions());

            var appUsage = todaySessions
                .GroupBy(session => string.IsNullOrWhiteSpace(session.AppName) ? "Unknown app" : session.AppName.Trim())
                .Select(group => new
                {
                    AppName = group.Key,
                    TotalMinutes = group.Sum(session => Math.Max(0, (session.EndTime - session.StartTime).TotalMinutes))
                })
                .OrderByDescending(app => app.TotalMinutes)
                .ThenBy(app => app.AppName)
                .Take(20)
                .ToList();

            var maps = await Task.Run(() =>
            {
                using var db = new AppDbContext();
                var catMap = db.AppCategories.ToDictionary(c => c.Id, c => c.Name);
                var clsMap = db.AppClassifications.ToDictionary(c => c.AppName.ToLower(), c => c.CategoryId);
                return new { catMap, clsMap };
            });
            var categoryMap = maps.catMap;
            var appClassMap = maps.clsMap;

            var neutralCategory = categoryMap.FirstOrDefault(kvp => kvp.Value.Equals("neutral", StringComparison.OrdinalIgnoreCase));
            int neutralId = neutralCategory.Key != 0 ? neutralCategory.Key : (categoryMap.Keys.FirstOrDefault());

            var series = new ColumnSeries<double>
            {
                Name = "",
                Values = appUsage.Select(app => app.TotalMinutes).ToArray(),
                YToolTipLabelFormatter = (point) => $"{Math.Round(point.Model, 0)} min",
                MaxBarWidth = 80,
                Rx = 10,
                Ry = 10
            };

            series.PointMeasured += (point) =>
            {
                if (point.Visual is null) return;

                var app = appUsage[point.Index];
                int catId = appClassMap.TryGetValue(app.AppName.ToLower(), out var id) ? id : neutralId;
                string catName = categoryMap.TryGetValue(catId, out var name) ? name : "Neutral";

                point.Visual.Fill = new SolidColorPaint(GetChartCategoryColor(catName));
            };

            var axisTitlePaint = new SolidColorPaint(new SKColor(90, 90, 90));

            var chart = new CartesianChart
            {
                LegendPosition = LiveChartsCore.Measure.LegendPosition.Hidden,
                Series = appUsage.Count == 0
                    ? Array.Empty<ISeries>()
                    : new ISeries[] { series },
                XAxes = new[]
                {
                    new Axis
                    {
                        Name = "Applications",
                        NameTextSize = 12,
                        NamePaint = axisTitlePaint,
                        Labels = appUsage.Select(app => app.AppName.Length > 14 ? app.AppName.Substring(0, 11) + "..." : app.AppName).ToArray(),
                        LabelsRotation = 90,
                        MinStep = 1,
                        TextSize = 10
                    }
                },
                YAxes = new[]
                {
                    new Axis
                    {
                        Name = "Usage Time (Minutes)",
                        NameTextSize = 12,
                        NamePaint = axisTitlePaint,
                        TextSize = 10
                    }
                }
            };

            chart.Dock = DockStyle.Fill;

            pnlChart.Controls.Clear();
            pnlChart.Controls.Add(chart);
        }

        private void grpCategory_Enter(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void lblCategoryInput_Click(object sender, EventArgs e)
        {

        }

        // =========================================================================
        // UNIVERSITY MANDATORY FEATURE: REPOSITORY-DRIVEN ASYNCHRONOUS SETTINGS
        // =========================================================================
        private List<AppCategory> localizedCategories = new();

        private void InitializeSettingsSection()
        {
            // Bind Left GroupBox: Category Name Input & Daily Goal Threshold Allocations
            btnAddCategory.Click += async (s, e) => await AddCategoryAsync();
            btnSaveCategorySettings.Click += async (s, e) => await SaveCategoryGoalAsync();
            btnRemoveCategory.Click += async (s, e) => await RemoveCategoryAsync();

            // Bind Right GroupBox: Process Engine Restrictive Filter Options
            btnAddToIgnore.Click += async (s, e) => await AddAppToIgnoreListAsync();
            btnRemoveFromIgnore.Click += async (s, e) => await RemoveAppFromIgnoreListAsync();

            // Bind Application Classification Grid
            dataGridView1.CellValueChanged += async (s, e) => await DataGridView1_CellValueChangedAsync(s, e);
            dataGridView1.CurrentCellDirtyStateChanged += dataGridView1_CurrentCellDirtyStateChanged;
            dataGridView1.DataError += dataGridView1_DataError;

            // Perform non-blocking async payload retrieval during initialization
            _ = LoadSettingsDataAsync();
        }

        private void dataGridView1_CurrentCellDirtyStateChanged(object? sender, EventArgs e)
        {
            if (dataGridView1.IsCurrentCellDirty)
            {
                dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void dataGridView1_DataError(object? sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        private async Task DataGridView1_CellValueChangedAsync(object? sender, DataGridViewCellEventArgs e)
        {
            if (isBindingSettings) return;
            if (e.RowIndex < 0 || e.ColumnIndex != colCategory.Index) return;

            var row = dataGridView1.Rows[e.RowIndex];
            var appNameObj = row.Cells[colApplication.Index].Value;
            var categoryNameObj = row.Cells[colCategory.Index].Value;

            string? appName = appNameObj?.ToString();
            string? categoryName = categoryNameObj?.ToString();

            if (string.IsNullOrWhiteSpace(appName) || string.IsNullOrWhiteSpace(categoryName)) return;

            var category = localizedCategories.FirstOrDefault(c => c.Name == categoryName);
            if (category != null)
            {
                try
                {
                    await sessionRepository.SaveAppClassificationAsync(appName, category.Id);
                    statusLabel.Text = $"Updated classification: {appName} -> {categoryName}";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving app classification: {ex.Message}");
                }
            }
        }

        private async Task RemoveAppFromIgnoreListAsync()
        {
            if (lstIgnoreList.SelectedIndex == -1)
            {
                MessageBox.Show("Please select an application from the ignore list to remove.",
                                "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string? selectedApp = lstIgnoreList.SelectedItem?.ToString();
            if (string.IsNullOrWhiteSpace(selectedApp)) return;

            try
            {
                await sessionRepository.RemoveFromIgnoreListAsync(selectedApp);
                await LoadSettingsDataAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error removing app from ignore list: {ex.Message}");
            }
        }

        private async Task LoadSettingsDataAsync()
        {
            try
            {
                // Uses your existing repository instance variable
                var categoriesTask = sessionRepository.GetAllCategoriesAsync();
                var ignoreListTask = sessionRepository.GetIgnoreListAsync();
                var classificationsTask = sessionRepository.GetAppClassificationsAsync();

                await Task.WhenAll(categoriesTask, ignoreListTask, classificationsTask);

                localizedCategories = categoriesTask.Result;
                var ignoreList = ignoreListTask.Result;
                var classifications = classificationsTask.Result;

                // Thread-safe update context for UI controls
                lstCategories.Items.Clear();
                foreach (var category in localizedCategories)
                {
                    lstCategories.Items.Add($"{category.Name} ({category.DailyGoalMinutes}m)");
                }

                lstIgnoreList.Items.Clear();
                foreach (var app in ignoreList)
                {
                    lstIgnoreList.Items.Add(app);
                }

                // Update classification DataGridView
                isBindingSettings = true;
                try
                {
                    colCategory.Items.Clear();
                    foreach (var category in localizedCategories)
                    {
                        colCategory.Items.Add(category.Name);
                    }

                    dataGridView1.Rows.Clear();
                    foreach (var classification in classifications)
                    {
                        int rowIndex = dataGridView1.Rows.Add();
                        dataGridView1.Rows[rowIndex].Cells[colApplication.Index].Value = classification.AppName;
                        dataGridView1.Rows[rowIndex].Cells[colCategory.Index].Value = classification.Category?.Name ?? "Neutral";
                    }
                }
                finally
                {
                    isBindingSettings = false;
                }
            }
            catch (Exception ex)
            {
                statusLabel.Text = $"Failed to synchronize tracker metrics: {ex.Message}";
            }
        }

        private async Task AddCategoryAsync()
        {
            string categoryName = txtCategoryName.Text.Trim();
            if (string.IsNullOrWhiteSpace(categoryName)) return;

            // Check for duplicates locally first to avoid database constraint exceptions
            if (localizedCategories.Any(c => c.Name.Equals(categoryName, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show($"Category '{categoryName}' already exists.", "Duplicate Category", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                await sessionRepository.AddCategoryAsync(categoryName);
                txtCategoryName.Clear();
                await LoadSettingsDataAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding category: {ex.Message}");
            }
        }

        private async Task SaveCategoryGoalAsync()
        {
            if (lstCategories.SelectedIndex == -1) return;

            try
            {
                var selectedCategory = localizedCategories[lstCategories.SelectedIndex];
                int goalMinutes = (int)nudDailyGoal.Value;

                await sessionRepository.UpdateCategoryGoalAsync(selectedCategory.Id, goalMinutes);
                await LoadSettingsDataAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving goal: {ex.Message}");
            }
        }

        private async Task RemoveCategoryAsync()
        {
            // Validate that the user has actually highlighted an item in the ListBox
            if (lstCategories.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a category from the list to remove.",
                                "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Retrieve the localized object corresponding to the index
            var selectedCategory = localizedCategories[lstCategories.SelectedIndex];

            // Prevent removing core mandatory system classifications to avoid data tracking inconsistencies
            if (selectedCategory.Name == "Productive" || selectedCategory.Name == "Neutral" || selectedCategory.Name == "Distracting" || selectedCategory.Name == "Communication" || selectedCategory.Name == "Entertainment")
            {
                MessageBox.Show("Core tracking categories (Productive, Neutral, Distracting, Communication, Entertainment) cannot be removed.",
                                "System Restriction", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            // Confirm action with user
            var confirmation = MessageBox.Show($"Are you sure you want to delete the category '{selectedCategory.Name}'?\nAssociated application sessions will revert to Neutral classification.",
                                               "Confirm Removal", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmation != DialogResult.Yes) return;

            try
            {
                // Execute database removal completely off the UI thread via data layer repository
                await sessionRepository.RemoveCategoryAsync(selectedCategory.Id);

                // Clear any input selection and refresh lists thread-safely
                lstCategories.SelectedIndex = -1;
                await LoadSettingsDataAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error removing category from database: {ex.Message}",
                                "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task AddAppToIgnoreListAsync()
        {
            string appProcess = txtIgnoreAppName.Text.Trim();
            if (string.IsNullOrWhiteSpace(appProcess)) return;

            try
            {
                await sessionRepository.AddToIgnoreListAsync(appProcess);
                txtIgnoreAppName.Clear();
                await LoadSettingsDataAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error ignoring app: {ex.Message}");
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private async void btnRemoveFromCategories_Click(object sender, EventArgs e)
        {
            await RemoveCategoryAsync();
        }

        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void lblIgnoredApps_Click(object sender, EventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void nudDailyGoal_ValueChanged(object sender, EventArgs e)
        {

        }

        private void txtCategoryName_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnGenerateReport_Click_1(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void pnlChart_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lblTotalSessions_Click(object sender, EventArgs e)
        {

        }

        private void lblTotalTime_Click(object sender, EventArgs e)
        {

        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        // "Export to pdf" button on the Reports tab
        private void button1_Click(object sender, EventArgs e)
        {
            exportToPdfToolStripMenuItem_Click(sender, e);
        }

        // "Export to excel" button on the Reports tab
        private void button2_Click_1(object sender, EventArgs e)
        {
            exportToExcelToolStripMenuItem_Click(sender, e);
        }

        private void pnlReportChartHost_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {

        }

        private void btnAddCategory_Click_1(object sender, EventArgs e)
        {

        }

        private void btnSaveCategorySettings_Click(object sender, EventArgs e)
        {

        }

        private void lstCategories_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }

    public class FocusTrackFontResolver : IFontResolver
    {
        public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
        {
            if (familyName.Equals("Segoe UI", StringComparison.OrdinalIgnoreCase))
            {
                if (isBold && isItalic)
                    return new FontResolverInfo("SegoeUI-BoldItalic");
                else if (isBold)
                    return new FontResolverInfo("SegoeUI-Bold");
                else if (isItalic)
                    return new FontResolverInfo("SegoeUI-Italic");
                else
                    return new FontResolverInfo("SegoeUI-Regular");
            }
            
            if (isBold && isItalic)
                return new FontResolverInfo("Arial-BoldItalic");
            else if (isBold)
                return new FontResolverInfo("Arial-Bold");
            else if (isItalic)
                return new FontResolverInfo("Arial-Italic");
            else
                return new FontResolverInfo("Arial-Regular");
        }

        public byte[] GetFont(string faceName)
        {
            string fontPath = "";
            
            switch (faceName)
            {
                case "SegoeUI-Regular":
                    fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Fonts", "segoeui.ttf");
                    break;
                case "SegoeUI-Bold":
                    fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Fonts", "segoeuib.ttf");
                    break;
                case "SegoeUI-Italic":
                    fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Fonts", "segoeuii.ttf");
                    break;
                case "SegoeUI-BoldItalic":
                    fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Fonts", "segoeuiz.ttf");
                    break;
                    
                case "Arial-Regular":
                    fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Fonts", "arial.ttf");
                    break;
                case "Arial-Bold":
                    fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Fonts", "arialbd.ttf");
                    break;
                case "Arial-Italic":
                    fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Fonts", "ariali.ttf");
                    break;
                case "Arial-BoldItalic":
                    fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Fonts", "arialbi.ttf");
                    break;
            }

            if (File.Exists(fontPath))
            {
                return File.ReadAllBytes(fontPath);
            }
            
            string defaultPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Fonts", "segoeui.ttf");
            if (File.Exists(defaultPath))
            {
                return File.ReadAllBytes(defaultPath);
            }
            
            throw new InvalidOperationException($"Font face '{faceName}' not found.");
        }
    }
}
