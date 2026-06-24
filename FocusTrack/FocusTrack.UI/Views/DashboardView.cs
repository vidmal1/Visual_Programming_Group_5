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
        private DataGridView dgvAppUsage = null!;
        private DataGridView dgvGoalProgress = null!;
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
            lblMostUsedApp = CreateCardLabel("Most Used: N/A");

            lblTotalTime.Left = 20;
            lblSessionCount.Left = 360;
            lblMostUsedApp.Left = 700;

            cardPanel.Controls.Add(lblTotalTime);
            cardPanel.Controls.Add(lblSessionCount);
            cardPanel.Controls.Add(lblMostUsedApp);

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

            TableLayoutPanel contentLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                Padding = new Padding(10)
            };

            contentLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55));
            contentLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45));

            GroupBox appUsageGroup = new GroupBox
            {
                Text = "Today App Usage",
                Dock = DockStyle.Fill
            };

            dgvAppUsage = CreateGrid();
            appUsageGroup.Controls.Add(dgvAppUsage);

            GroupBox goalGroup = new GroupBox
            {
                Text = "Daily Goal Progress",
                Dock = DockStyle.Fill
            };

            dgvGoalProgress = CreateGrid();
            goalGroup.Controls.Add(dgvGoalProgress);

            contentLayout.Controls.Add(appUsageGroup, 0, 0);
            contentLayout.Controls.Add(goalGroup, 1, 0);

            Controls.Add(contentLayout);
            Controls.Add(actionPanel);
            Controls.Add(cardPanel);
            Controls.Add(lblTitle);
        }

        private Label CreateCardLabel(string text)
        {
            return new Label
            {
                Text = text,
                Width = 300,
                Height = 80,
                Top = 15,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
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
                lblMostUsedApp.Text = $"Most Used\n{summary.MostUsedApplication} - {summary.MostUsedDurationText}";

                dgvAppUsage.DataSource = summary.AppUsages;
                dgvGoalProgress.DataSource = summary.GoalProgresses;
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

        private void lblDashboardTitle_Click(object sender, EventArgs e)
        {
            // Not used yet
        }
    }
}