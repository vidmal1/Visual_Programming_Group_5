using FocusTrack.Business.Services;

namespace FocusTrack.UI.Views
{
    public partial class HistoryView : UserControl
    {
        private readonly HistoryService _historyService = new HistoryService();

        private DataGridView dgvSessions = null!;
        private Button btnReload = null!;
        private Label lblTitle = null!;

        public HistoryView()
        {
            InitializeComponent();

            BuildLayout();

            _ = LoadSessionsAsync();
        }

        private void BuildLayout()
        {
            Controls.Clear();

            lblTitle = new Label
            {
                Text = "History View",
                Dock = DockStyle.Top,
                Height = 50,
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter
            };

            Panel topPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 50,
                Padding = new Padding(10)
            };

            btnReload = new Button
            {
                Text = "Reload Sessions",
                Width = 150,
                Height = 30,
                Dock = DockStyle.Right
            };

            btnReload.Click += async (sender, e) => await LoadSessionsAsync();

            dgvSessions = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AutoGenerateColumns = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            topPanel.Controls.Add(btnReload);

            Controls.Add(dgvSessions);
            Controls.Add(topPanel);
            Controls.Add(lblTitle);
        }

        private async Task LoadSessionsAsync()
        {
            try
            {
                var sessions = await _historyService.GetAllSessionsAsync();

                dgvSessions.DataSource = sessions;

                lblTitle.Text = $"History View - {sessions.Count} session(s)";
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Error loading history",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
    }
}