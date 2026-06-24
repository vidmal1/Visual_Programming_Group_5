using FocusTrack.UI.Views;
using FocusTrack.Business.Services;

namespace FocusTrack.UI
{
    public partial class MainForm : Form
    {
        private readonly TrackingService _trackingService = new TrackingService();
        private System.Windows.Forms.Timer _trackingTimer;

        public MainForm()
        {
            InitializeComponent();

            LoadViews();

            _trackingTimer = new System.Windows.Forms.Timer();
            _trackingTimer.Interval = 1000;
            _trackingTimer.Tick += TrackingTimer_Tick;

            btnStartTracking.Click += BtnStartTracking_Click;
            btnStopTracking.Click += BtnStopTracking_Click;
            btnRefresh.Click += BtnRefresh_Click;

            lblStatus.Text = "FocusTrack ready.";
        }

        private void BtnStartTracking_Click(object? sender, EventArgs e)
        {
            _trackingTimer.Start();
            lblStatus.Text = "Tracking started.";
        }

        private async void BtnStopTracking_Click(object? sender, EventArgs e)
        {
            _trackingTimer.Stop();

            await _trackingService.StopTrackingAsync();

            lblStatus.Text = "Tracking stopped and session saved.";
        }

        private async void BtnRefresh_Click(object? sender, EventArgs e)
        {
            await ShowCurrentActiveWindowAsync();
        }


        private async void TrackingTimer_Tick(object? sender, EventArgs e)
        {
            await ShowCurrentActiveWindowAsync();
        }

        private async Task ShowCurrentActiveWindowAsync()
        {
            var windowInfo = await _trackingService.TrackCurrentWindowAsync();

            lblStatus.Text = $"Active: {windowInfo.ApplicationName} | {windowInfo.WindowTitle}";
        }

        private void LoadViews()
        {
            DashboardView dashboardView = new DashboardView();
            HistoryView historyView = new HistoryView();
            SettingsView settingsView = new SettingsView();
            ReportsView reportsView = new ReportsView();

            AddViewToTab(tabDashboard, dashboardView);
            AddViewToTab(tabHistory, historyView);
            AddViewToTab(tabSettings, settingsView);
            AddViewToTab(tabReports, reportsView);
        }

        private void AddViewToTab(TabPage tabPage, UserControl view)
        {
            view.Dock = DockStyle.Fill;
            tabPage.Controls.Clear();
            tabPage.Controls.Add(view);
        }

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            // Not used yet
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {
            // Not used yet
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {
            // Not used yet
        }
    }
}