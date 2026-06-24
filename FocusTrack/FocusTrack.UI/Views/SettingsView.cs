using FocusTrack.Business.DTOs;
using FocusTrack.Business.Services;

namespace FocusTrack.UI.Views
{
    public partial class SettingsView : UserControl
    {
        private readonly SettingsService _settingsService = new SettingsService();

        private Label lblTitle = null!;
        private DataGridView dgvCategories = null!;
        private DataGridView dgvGoals = null!;
        private DataGridView dgvIgnoreList = null!;
        private ComboBox cmbGoalCategory = null!;
        private NumericUpDown numGoalMinutes = null!;
        private Button btnSaveGoal = null!;
        private TextBox txtIgnoreApp = null!;
        private Button btnAddIgnore = null!;
        private Button btnRemoveIgnore = null!;

        public SettingsView()
        {
            InitializeComponent();

            BuildLayout();

            _ = LoadSettingsAsync();
        }

        private void BuildLayout()
        {
            Controls.Clear();

            lblTitle = new Label
            {
                Text = "Settings",
                Dock = DockStyle.Top,
                Height = 50,
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter
            };

            TableLayoutPanel mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 3,
                ColumnCount = 1,
                Padding = new Padding(10)
            };

            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 30));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 35));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 35));

            GroupBox categoryGroup = new GroupBox
            {
                Text = "Categories",
                Dock = DockStyle.Fill
            };

            dgvCategories = CreateGrid();
            categoryGroup.Controls.Add(dgvCategories);

            GroupBox goalGroup = new GroupBox
            {
                Text = "Daily Goals",
                Dock = DockStyle.Fill
            };

            Panel goalTopPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 45,
                Padding = new Padding(10)
            };

            cmbGoalCategory = new ComboBox
            {
                Left = 10,
                Top = 10,
                Width = 180,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            numGoalMinutes = new NumericUpDown
            {
                Left = 210,
                Top = 10,
                Width = 100,
                Minimum = 0,
                Maximum = 1440,
                Value = 60
            };

            btnSaveGoal = new Button
            {
                Text = "Save Goal",
                Left = 330,
                Top = 8,
                Width = 100,
                Height = 30
            };

            btnSaveGoal.Click += async (sender, e) => await SaveGoalAsync();

            goalTopPanel.Controls.Add(cmbGoalCategory);
            goalTopPanel.Controls.Add(numGoalMinutes);
            goalTopPanel.Controls.Add(btnSaveGoal);

            dgvGoals = CreateGrid();

            goalGroup.Controls.Add(dgvGoals);
            goalGroup.Controls.Add(goalTopPanel);

            GroupBox ignoreGroup = new GroupBox
            {
                Text = "Ignore List",
                Dock = DockStyle.Fill
            };

            Panel ignoreTopPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 45,
                Padding = new Padding(10)
            };

            txtIgnoreApp = new TextBox
            {
                Left = 10,
                Top = 10,
                Width = 220,
                PlaceholderText = "Application name e.g. explorer"
            };

            btnAddIgnore = new Button
            {
                Text = "Add",
                Left = 250,
                Top = 8,
                Width = 80,
                Height = 30
            };

            btnAddIgnore.Click += async (sender, e) => await AddIgnoreAppAsync();

            btnRemoveIgnore = new Button
            {
                Text = "Remove Selected",
                Left = 350,
                Top = 8,
                Width = 130,
                Height = 30
            };

            btnRemoveIgnore.Click += async (sender, e) => await RemoveSelectedIgnoreAppAsync();

            ignoreTopPanel.Controls.Add(txtIgnoreApp);
            ignoreTopPanel.Controls.Add(btnAddIgnore);
            ignoreTopPanel.Controls.Add(btnRemoveIgnore);

            dgvIgnoreList = CreateGrid();

            ignoreGroup.Controls.Add(dgvIgnoreList);
            ignoreGroup.Controls.Add(ignoreTopPanel);

            mainLayout.Controls.Add(categoryGroup, 0, 0);
            mainLayout.Controls.Add(goalGroup, 0, 1);
            mainLayout.Controls.Add(ignoreGroup, 0, 2);

            Controls.Add(mainLayout);
            Controls.Add(lblTitle);
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

        private async Task LoadSettingsAsync()
        {
            try
            {
                var categories = await _settingsService.GetCategoriesAsync();

                dgvCategories.DataSource = categories;

                cmbGoalCategory.DataSource = categories.ToList();
                cmbGoalCategory.DisplayMember = "Name";
                cmbGoalCategory.ValueMember = "Id";

                await LoadGoalsAsync();
                await LoadIgnoreListAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Error loading settings",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private async Task LoadGoalsAsync()
        {
            var goals = await _settingsService.GetDailyGoalsAsync();

            dgvGoals.DataSource = goals;
        }

        private async Task LoadIgnoreListAsync()
        {
            var ignoredApps = await _settingsService.GetIgnoredAppsAsync();

            dgvIgnoreList.DataSource = ignoredApps;
        }

        private async Task SaveGoalAsync()
        {
            try
            {
                if (cmbGoalCategory.SelectedValue == null)
                {
                    return;
                }

                int categoryId = (int)cmbGoalCategory.SelectedValue;
                int goalMinutes = (int)numGoalMinutes.Value;

                await _settingsService.SaveDailyGoalAsync(categoryId, goalMinutes);

                await LoadGoalsAsync();

                MessageBox.Show(
                    "Daily goal saved successfully.",
                    "Saved",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Error saving goal",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private async Task AddIgnoreAppAsync()
        {
            try
            {
                await _settingsService.AddIgnoredAppAsync(txtIgnoreApp.Text);

                txtIgnoreApp.Clear();

                await LoadIgnoreListAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Error adding ignored app",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private async Task RemoveSelectedIgnoreAppAsync()
        {
            try
            {
                if (dgvIgnoreList.CurrentRow?.DataBoundItem is not IgnoreListItemDto selectedItem)
                {
                    MessageBox.Show(
                        "Please select an ignored application first.",
                        "No selection",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );

                    return;
                }

                await _settingsService.RemoveIgnoredAppAsync(selectedItem.Id);

                await LoadIgnoreListAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Error removing ignored app",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
    }
}