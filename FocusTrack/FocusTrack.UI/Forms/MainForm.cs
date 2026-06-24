using FocusTrack.UI.Views;

namespace FocusTrack.UI
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            LoadViews();

            lblStatus.Text = "FocusTrack ready.";
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