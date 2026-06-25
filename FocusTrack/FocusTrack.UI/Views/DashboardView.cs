using FocusTrack.Business.DTOs;
using FocusTrack.Business.Services;
using System.Drawing;
using System.Windows.Forms;

namespace FocusTrack.UI.Views
{
    public partial class DashboardView : UserControl
    {
        private readonly DashboardService _dashboardService = new DashboardService();

        private Label lblTitle = null!;
        private Label lblTotalTime = null!;
        private Label lblSessionCount = null!;
        private Label lblMostUsedApp = null!;
        private Label lblProductivityScore = null!;
        private Label lblProductivePercent = null!;
        private Label lblNeutralPercent = null!;
        private Label lblDistractingPercent = null!;

        private DataGridView dgvAppUsage = null!;
        private DataGridView dgvCategoryUsage = null!;
        private DataGridView dgvGoalProgress = null!;

        private FlowLayoutPanel pnlChart = null!;
        private Button btnRefresh = null!;

        public DashboardView()
        {
            InitializeComponent();

            BuildLayout();

            _ = LoadDashboardAsync();
        }

        private void BuildLayout()
        {
            Controls.Clear();

            lblTitle = new Label
            {
                Text = "Dashboard",
                Dock = DockStyle.Top,
                Height = 50,
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter
            };

            Panel cardPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 120,
                Padding = new Padding(10)
            };

            lblTotalTime = CreateCardLabel("Today Total: 0s");
            lblSessionCount = CreateCardLabel("Sessions: 0");
            
            
            lblProductivePercent = CreateCardLabel("Productive: 0%");
            lblNeutralPercent = CreateCardLabel("Neutral: 0%");
            lblDistractingPercent = CreateCardLabel("Distracting: 0%");


            lblTotalTime.Left = 20;
            lblSessionCount.Left = 220;
            lblProductivePercent.Left = 420;
            lblNeutralPercent.Left = 620;
            lblDistractingPercent.Left = 820;

            cardPanel.Controls.Add(lblTotalTime);
            cardPanel.Controls.Add(lblSessionCount);
            cardPanel.Controls.Add(lblProductivePercent);
            cardPanel.Controls.Add(lblNeutralPercent);
            cardPanel.Controls.Add(lblDistractingPercent);

            Panel actionPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 50,
                Padding = new Padding(10)
            };

            btnRefresh = new Button
            {
                Text = "Refresh Dashboard",
                Width = 160,
                Height = 30,
                Dock = DockStyle.Right
            };

            btnRefresh.Click += async (sender, e) => await LoadDashboardAsync();

            actionPanel.Controls.Add(btnRefresh);

            TableLayoutPanel mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 2,
                ColumnCount = 1,
                Padding = new Padding(10)
            };

            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 35));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 65));

            GroupBox chartGroup = new GroupBox
            {
                Text = "Top App Usage Chart",
                Dock = DockStyle.Fill
            };

            pnlChart = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                AutoScroll = true,
                Padding = new Padding(10),
                WrapContents = false
            };

            chartGroup.Controls.Add(pnlChart);

            TableLayoutPanel tableLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 1
            };

            tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
            tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));
            tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));

            GroupBox appUsageGroup = new GroupBox
            {
                Text = "Today App Usage",
                Dock = DockStyle.Fill
            };

            dgvAppUsage = CreateGrid();
            appUsageGroup.Controls.Add(dgvAppUsage);

            GroupBox categoryUsageGroup = new GroupBox
            {
                Text = "Category Summary",
                Dock = DockStyle.Fill
            };

            dgvCategoryUsage = CreateGrid();
            categoryUsageGroup.Controls.Add(dgvCategoryUsage);

            GroupBox goalGroup = new GroupBox
            {
                Text = "Daily Goal Progress",
                Dock = DockStyle.Fill
            };

            dgvGoalProgress = CreateGrid();
            goalGroup.Controls.Add(dgvGoalProgress);

            tableLayout.Controls.Add(appUsageGroup, 0, 0);
            tableLayout.Controls.Add(categoryUsageGroup, 1, 0);
            tableLayout.Controls.Add(goalGroup, 2, 0);

            mainLayout.Controls.Add(chartGroup, 0, 0);
            mainLayout.Controls.Add(tableLayout, 0, 1);

            Controls.Add(mainLayout);
            Controls.Add(actionPanel);
            Controls.Add(cardPanel);
            Controls.Add(lblTitle);
        }

        private Label CreateCardLabel(string text)
        {
            return new Label
            {
                Text = text,
                Width = 180,
                Height = 80,
                Top = 15,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White
            };
        }

        private DataGridView CreateGrid()
        {
            return new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AutoGenerateColumns = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
        }

        private async Task LoadDashboardAsync()
        {
            try
            {
                var summary = await _dashboardService.GetTodaySummaryAsync();

                lblTotalTime.Text = $"Today Total\n{summary.TotalDurationText}";
                lblSessionCount.Text = $"Sessions\n{summary.SessionCount}";
                //lblMostUsedApp.Text = $"Most Used\n{summary.MostUsedApplication} - {summary.MostUsedDurationText}";
                //lblProductivityScore.Text = $"Productivity\n{summary.ProductivityScore}%";

                lblProductivePercent.Text = $"Productive\n{summary.ProductivePercentage}%";
                lblNeutralPercent.Text = $"Neutral\n{summary.NeutralPercentage}%";
                lblDistractingPercent.Text = $"Distracting\n{summary.DistractingPercentage}%";

                dgvAppUsage.DataSource = summary.AppUsages;
                dgvCategoryUsage.DataSource = summary.CategoryUsages;
                dgvGoalProgress.DataSource = summary.GoalProgresses;

                BuildAppUsageChart(summary.AppUsages);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Error loading dashboard",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void BuildAppUsageChart(List<AppUsageDto> appUsages)
        {
            pnlChart.Controls.Clear();

            if (appUsages.Count == 0)
            {
                Label emptyLabel = new Label
                {
                    Text = "No app usage data available for today.",
                    Width = 500,
                    Height = 40,
                    Font = new Font("Segoe UI", 11, FontStyle.Italic),
                    TextAlign = ContentAlignment.MiddleLeft
                };

                pnlChart.Controls.Add(emptyLabel);
                return;
            }

            int maxSeconds = appUsages.Max(app => app.TotalSeconds);

            foreach (var app in appUsages.Take(5))
            {
                int percentage = maxSeconds == 0
                    ? 0
                    : Math.Max(1, (int)((app.TotalSeconds / (double)maxSeconds) * 100));

                Panel rowPanel = new Panel
                {
                    Width = 900,
                    Height = 45,
                    Margin = new Padding(5)
                };

                Label appNameLabel = new Label
                {
                    Text = app.ApplicationName,
                    Left = 0,
                    Top = 10,
                    Width = 150,
                    Height = 25,
                    Font = new Font("Segoe UI", 10, FontStyle.Bold)
                };

                ProgressBar usageBar = new ProgressBar
                {
                    Left = 160,
                    Top = 10,
                    Width = 500,
                    Height = 25,
                    Minimum = 0,
                    Maximum = 100,
                    Value = percentage
                };

                Label durationLabel = new Label
                {
                    Text = app.DurationText,
                    Left = 680,
                    Top = 10,
                    Width = 120,
                    Height = 25,
                    Font = new Font("Segoe UI", 10),
                    TextAlign = ContentAlignment.MiddleLeft
                };

                rowPanel.Controls.Add(appNameLabel);
                rowPanel.Controls.Add(usageBar);
                rowPanel.Controls.Add(durationLabel);

                pnlChart.Controls.Add(rowPanel);
            }
        }

        private void lblDashboardTitle_Click(object sender, EventArgs e)
        {
            // Not used yet
        }
    }
}