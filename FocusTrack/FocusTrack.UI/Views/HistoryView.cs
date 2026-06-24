using FocusTrack.Business.Services;

namespace FocusTrack.UI.Views
{
    public partial class HistoryView : UserControl
    {
        private readonly HistoryService _historyService = new HistoryService();

        private DataGridView dgvSessions = null!;
        private Button btnReload = null!;
        private Button btnApplyFilters = null!;
        private Button btnClearFilters = null!;
        private Label lblTitle = null!;
        private DateTimePicker dtpFrom = null!;
        private DateTimePicker dtpTo = null!;
        private TextBox txtAppName = null!;
        private ComboBox cmbCategory = null!;

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

            txtAppName = new TextBox
            {
                Left = 395,
                Top = 10,
                Width = 180,
                PlaceholderText = "App name..."
            };

            cmbCategory = new ComboBox
            {
                Left = 595,
                Top = 10,
                Width = 150,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            cmbCategory.Items.Add("All Categories");
            cmbCategory.Items.Add("Productive");
            cmbCategory.Items.Add("Neutral");
            cmbCategory.Items.Add("Distracting");
            cmbCategory.SelectedIndex = 0;

            btnApplyFilters = new Button
            {
                Text = "Apply Filters",
                Left = 765,
                Top = 8,
                Width = 110,
                Height = 30
            };

            btnApplyFilters.Click += async (sender, e) => await ApplyFiltersAsync();

            btnClearFilters = new Button
            {
                Text = "Clear",
                Left = 885,
                Top = 8,
                Width = 80,
                Height = 30
            };

            btnClearFilters.Click += async (sender, e) => await ClearFiltersAsync();

            btnReload = new Button
            {
                Text = "Reload Sessions",
                Left = 10,
                Top = 50,
                Width = 150,
                Height = 30
            };

            btnReload.Click += async (sender, e) => await LoadSessionsAsync();

            filterPanel.Controls.Add(lblFrom);
            filterPanel.Controls.Add(dtpFrom);
            filterPanel.Controls.Add(lblTo);
            filterPanel.Controls.Add(dtpTo);
            filterPanel.Controls.Add(txtAppName);
            filterPanel.Controls.Add(cmbCategory);
            filterPanel.Controls.Add(btnApplyFilters);
            filterPanel.Controls.Add(btnClearFilters);
            filterPanel.Controls.Add(btnReload);

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

            Controls.Add(dgvSessions);
            Controls.Add(filterPanel);
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

        private async Task ApplyFiltersAsync()
        {
            try
            {
                int? categoryId = GetSelectedCategoryId();

                var sessions = await _historyService.GetFilteredSessionsAsync(
                    dtpFrom.Value,
                    dtpTo.Value,
                    txtAppName.Text.Trim(),
                    categoryId
                );

                dgvSessions.DataSource = sessions;

                lblTitle.Text = $"History View - {sessions.Count} filtered session(s)";
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Error applying filters",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private async Task ClearFiltersAsync()
        {
            dtpFrom.Value = DateTime.Today;
            dtpTo.Value = DateTime.Today;
            txtAppName.Clear();
            cmbCategory.SelectedIndex = 0;

            await LoadSessionsAsync();
        }

        private int? GetSelectedCategoryId()
        {
            return cmbCategory.SelectedIndex switch
            {
                1 => 1, // Productive
                2 => 2, // Neutral
                3 => 3, // Distracting
                _ => null
            };
        }
    }
}