using FocusTrack.UI.Views;
using FocusTrack.Business.Services;
using System.Drawing;

namespace FocusTrack.UI
{
    public partial class MainForm : Form
    {
        private readonly TrackingService _trackingService = new TrackingService();
        private readonly NotificationService _notificationService = new NotificationService();
        private bool _isCheckingNotifications = false;
        private System.Windows.Forms.Timer _trackingTimer;
        private bool _isExitRequested = false;

        public MainForm()
        {
            InitializeComponent();

            LoadViews();
            SetupSystemTray();

            _trackingTimer = new System.Windows.Forms.Timer();
            _trackingTimer.Interval = 1000;
            _trackingTimer.Tick += TrackingTimer_Tick;

            btnStartTracking.Click += BtnStartTracking_Click;
            btnStopTracking.Click += BtnStopTracking_Click;
            btnRefresh.Click += BtnRefresh_Click;

            this.Resize += MainForm_Resize;

            lblStatus.Text = "FocusTrack ready.";
        }

        private void SetupSystemTray()
        {
            notifyIconMain.Icon = SystemIcons.Application;
            notifyIconMain.Text = "FocusTrack";
            notifyIconMain.Visible = true;

            ContextMenuStrip trayMenu = new ContextMenuStrip();

            ToolStripMenuItem showItem = new ToolStripMenuItem("Show");
            ToolStripMenuItem hideItem = new ToolStripMenuItem("Hide");
            ToolStripMenuItem exitItem = new ToolStripMenuItem("Exit");

            showItem.Click += (sender, e) => ShowFromTray();
            hideItem.Click += (sender, e) => HideToTray();
            exitItem.Click += async (sender, e) => await ExitApplicationAsync();

            trayMenu.Items.Add(showItem);
            trayMenu.Items.Add(hideItem);
            trayMenu.Items.Add(new ToolStripSeparator());
            trayMenu.Items.Add(exitItem);

            notifyIconMain.ContextMenuStrip = trayMenu;

            notifyIconMain.DoubleClick += (sender, e) => ShowFromTray();
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
            await CheckGoalNotificationsAsync();

            lblStatus.Text = "Tracking stopped and session saved.";
        }

        private async void BtnRefresh_Click(object? sender, EventArgs e)
        {
            await ShowCurrentActiveWindowAsync();
        }

        private async Task CheckGoalNotificationsAsync()
        {
            if (_isCheckingNotifications)
            {
                return;
            }

            try
            {
                _isCheckingNotifications = true;

                var alerts = await _notificationService.CheckGoalAlertsAsync();

                foreach (var alert in alerts)
                {
                    notifyIconMain.ShowBalloonTip(
                        3000,
                        "FocusTrack Goal Alert",
                        alert.Message,
                        ToolTipIcon.Info
                    );

                    lblStatus.Text = alert.Message;
                }
            }
            catch
            {
                // Notification errors should not stop tracking
            }
            finally
            {
                _isCheckingNotifications = false;
            }
        }
        private async void TrackingTimer_Tick(object? sender, EventArgs e)
        {
            await ShowCurrentActiveWindowAsync();
            await CheckGoalNotificationsAsync();
        }

        private async Task ShowCurrentActiveWindowAsync()
        {
            var windowInfo = await _trackingService.TrackCurrentWindowAsync();

            if (windowInfo.IsIgnored)
            {
                lblStatus.Text = $"Ignored: {windowInfo.ApplicationName}";
                return;
            }

            lblStatus.Text = $"Active: {windowInfo.ApplicationName} | {windowInfo.WindowTitle}";
        }

        private void MainForm_Resize(object? sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                HideToTray();
            }
        }

        private void HideToTray()
        {
            Hide();
            ShowInTaskbar = false;

            notifyIconMain.Visible = true;

            notifyIconMain.ShowBalloonTip(
                1000,
                "FocusTrack",
                "FocusTrack is still running in the system tray.",
                ToolTipIcon.Info
            );
        }

        private void ShowFromTray()
        {
            Show();
            ShowInTaskbar = true;
            WindowState = FormWindowState.Normal;
            Activate();
        }

        private async Task ExitApplicationAsync()
        {
            _isExitRequested = true;

            try
            {
                _trackingTimer.Stop();
                await _trackingService.StopTrackingAsync();
            }
            catch
            {
                // Ignore shutdown errors
            }

            notifyIconMain.Visible = false;

            Application.Exit();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (!_isExitRequested)
            {
                e.Cancel = true;
                HideToTray();
                return;
            }

            notifyIconMain.Visible = false;

            base.OnFormClosing(e);
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