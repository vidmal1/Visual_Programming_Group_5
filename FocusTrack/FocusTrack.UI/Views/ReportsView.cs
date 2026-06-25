using FocusTrack.Business.DTOs;
using FocusTrack.Business.Services;
using System.Text;

namespace FocusTrack.UI.Views
{
    public partial class ReportsView : UserControl
    {
        private readonly ReportService _reportService = new ReportService();

        private Label lblTitle = null!;
        private Label lblSummary = null!;
        private DateTimePicker dtpFrom = null!;
        private DateTimePicker dtpTo = null!;
        private ComboBox cmbGroupBy = null!;
        private Button btnGenerate = null!;
        private Button btnExportCsv = null!;
        private DataGridView dgvReport = null!;

        private List<ReportRowDto> _currentReportRows = new List<ReportRowDto>();

        public ReportsView()
        {
            InitializeComponent();

            BuildLayout();

            _ = GenerateReportAsync();
        }

        private void BuildLayout()
        {
            Controls.Clear();

            lblTitle = new Label
            {
                Text = "Reports",
                Dock = DockStyle.Top,
                Height = 50,
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter
            };

            Panel filterPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 90,
                Padding = new Padding(10)
            };

            Label lblFrom = new Label
            {
                Text = "From:",
                Left = 10,
                Top = 15,
                Width = 45
            };

            dtpFrom = new DateTimePicker
            {
                Left = 60,
                Top = 10,
                Width = 130,
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Today
            };

            Label lblTo = new Label
            {
                Text = "To:",
                Left = 210,
                Top = 15,
                Width = 30
            };

            dtpTo = new DateTimePicker
            {
                Left = 245,
                Top = 10,
                Width = 130,
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Today
            };

            Label lblGroupBy = new Label
            {
                Text = "Group By:",
                Left = 400,
                Top = 15,
                Width = 70
            };

            cmbGroupBy = new ComboBox
            {
                Left = 475,
                Top = 10,
                Width = 150,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            cmbGroupBy.Items.Add("Application");
            cmbGroupBy.Items.Add("Category");
            cmbGroupBy.SelectedIndex = 0;

            btnGenerate = new Button
            {
                Text = "Generate Report",
                Left = 650,
                Top = 8,
                Width = 140,
                Height = 30
            };

            btnGenerate.Click += async (sender, e) => await GenerateReportAsync();

            btnExportCsv = new Button
            {
                Text = "Export CSV",
                Left = 810,
                Top = 8,
                Width = 110,
                Height = 30
            };

            btnExportCsv.Click += async (sender, e) => await ExportCsvAsync();

            lblSummary = new Label
            {
                Text = "Summary: 0 rows",
                Left = 10,
                Top = 55,
                Width = 700,
                Height = 25,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            filterPanel.Controls.Add(lblFrom);
            filterPanel.Controls.Add(dtpFrom);
            filterPanel.Controls.Add(lblTo);
            filterPanel.Controls.Add(dtpTo);
            filterPanel.Controls.Add(lblGroupBy);
            filterPanel.Controls.Add(cmbGroupBy);
            filterPanel.Controls.Add(btnGenerate);
            filterPanel.Controls.Add(btnExportCsv);
            filterPanel.Controls.Add(lblSummary);

            dgvReport = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AutoGenerateColumns = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            Controls.Add(dgvReport);
            Controls.Add(filterPanel);
            Controls.Add(lblTitle);
        }

        private async Task GenerateReportAsync()
        {
            try
            {
                string groupBy = cmbGroupBy.SelectedItem?.ToString() ?? "Application";

                _currentReportRows = await _reportService.GetReportAsync(
                    dtpFrom.Value,
                    dtpTo.Value,
                    groupBy
                );

                dgvReport.DataSource = _currentReportRows;

                int totalSeconds = _currentReportRows.Sum(row => row.TotalSeconds);
                int totalSessions = _currentReportRows.Sum(row => row.SessionCount);

                lblSummary.Text =
                    $"Summary: {_currentReportRows.Count} row(s) | {totalSessions} session(s) | Total: {FormatDuration(totalSeconds)}";

                lblTitle.Text = $"Reports - Grouped by {groupBy}";
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Error generating report",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private async Task ExportCsvAsync()
        {
            try
            {
                if (_currentReportRows.Count == 0)
                {
                    MessageBox.Show(
                        "No report data available to export.",
                        "Export",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );

                    return;
                }

                using SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "CSV files (*.csv)|*.csv",
                    Title = "Export Report",
                    FileName = $"FocusTrack_Report_{DateTime.Now:yyyyMMdd_HHmmss}.csv"
                };

                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                StringBuilder csvBuilder = new StringBuilder();

                csvBuilder.AppendLine("Group Name,Session Count,Total Seconds,Total Duration");

                foreach (var row in _currentReportRows)
                {
                    csvBuilder.AppendLine(
                        $"{EscapeCsv(row.GroupName)},{row.SessionCount},{row.TotalSeconds},{EscapeCsv(row.TotalDurationText)}"
                    );
                }

                await File.WriteAllTextAsync(
                    saveFileDialog.FileName,
                    csvBuilder.ToString(),
                    Encoding.UTF8
                );

                MessageBox.Show(
                    "Report exported successfully.",
                    "Export Complete",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Error exporting report",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private string EscapeCsv(string value)
        {
            if (value.Contains(",") || value.Contains("\"") || value.Contains("\n"))
            {
                return $"\"{value.Replace("\"", "\"\"")}\"";
            }

            return value;
        }

        private string FormatDuration(int totalSeconds)
        {
            TimeSpan time = TimeSpan.FromSeconds(totalSeconds);

            if (time.TotalHours >= 1)
            {
                return $"{(int)time.TotalHours}h {time.Minutes}m";
            }

            if (time.TotalMinutes >= 1)
            {
                return $"{time.Minutes}m {time.Seconds}s";
            }

            return $"{time.Seconds}s";
        }
    }
}