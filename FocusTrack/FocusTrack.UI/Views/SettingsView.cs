using FocusTrack.Business.DTOs;
using FocusTrack.Business.Services;

namespace FocusTrack.UI.Views
{
    public partial class SettingsView : UserControl
    {
        private readonly SettingsService _settingsService = new SettingsService();
        private readonly ClassificationService _classificationService = new ClassificationService();

        private Label lblTitle = null!;

        private DataGridView dgvCategories = null!;

        private DataGridView dgvGoals = null!;
        private ComboBox cmbGoalCategory = null!;
        private NumericUpDown numGoalMinutes = null!;
        private Button btnSaveGoal = null!;

        private DataGridView dgvIgnoreList = null!;
        private TextBox txtIgnoreApp = null!;
        private Button btnAddIgnore = null!;
        private Button btnRemoveIgnore = null!;

        private DataGridView dgvClassificationRules = null!;
        private TextBox txtClassificationApp = null!;
        private ComboBox cmbClassificationCategory = null!;
        private Button btnSaveClassification = null!;
        private Button btnRemoveClassification = null!;

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
                RowCount = 4,
                ColumnCount = 1,
                Padding = new Padding(10)
            };

            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 20));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 25));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 25));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 30));

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

            GroupBox classificationGroup = new GroupBox
            {
                Text = "App Classification Rules",
                Dock = DockStyle.Fill
            };

            Panel classificationTopPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 45,
                Padding = new Padding(10)
            };

            txtClassificationApp = new TextBox
            {
                Left = 10,
                Top = 10,
                Width = 220,
                PlaceholderText = "App name e.g. chrome"
            };

            cmbClassificationCategory = new ComboBox
            {
                Left = 250,
                Top = 10,
                Width = 160,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            btnSaveClassification = new Button
            {
                Text = "Save Rule",
                Left = 430,
                Top = 8,
                Width = 100,
                Height = 30
            };

            btnSaveClassification.Click += async (sender, e) => await SaveClassificationRuleAsync();

            btnRemoveClassification = new Button
            {
                Text = "Remove Selected",
                Left = 550,
                Top = 8,
                Width = 130,
                Height = 30
            };

            btnRemoveClassification.Click += async (sender, e) => await RemoveSelectedClassificationRuleAsync();

            classificationTopPanel.Controls.Add(txtClassificationApp);
            classificationTopPanel.Controls.Add(cmbClassificationCategory);
            classificationTopPanel.Controls.Add(btnSaveClassification);
            classificationTopPanel.Controls.Add(btnRemoveClassification);

            dgvClassificationRules = CreateGrid();

            classificationGroup.Controls.Add(dgvClassificationRules);
            classificationGroup.Controls.Add(classificationTopPanel);

            mainLayout.Controls.Add(categoryGroup, 0, 0);
            mainLayout.Controls.Add(goalGroup, 0, 1);
            mainLayout.Controls.Add(ignoreGroup, 0, 2);
            mainLayout.Controls.Add(classificationGroup, 0, 3);

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

                cmbClassificationCategory.DataSource = categories.ToList();
                cmbClassificationCategory.DisplayMember = "Name";
                cmbClassificationCategory.ValueMember = "Id";

                await LoadGoalsAsync();
                await LoadIgnoreListAsync();
                await LoadClassificationRulesAsync();
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

        private async Task LoadClassificationRulesAsync()
        {
            var rules = await _classificationService.GetRulesAsync();

            dgvClassificationRules.DataSource = rules;
        }

        private async Task SaveGoalAsync()
        {
            try
            {
                if (cmbGoalCategory.SelectedValue == null)
                {
                    return;
                }

                int categoryId = Convert.ToInt32(cmbGoalCategory.SelectedValue);
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

        private async Task SaveClassificationRuleAsync()
        {
            try
            {
                if (cmbClassificationCategory.SelectedValue == null)
                {
                    return;
                }

                int categoryId = Convert.ToInt32(cmbClassificationCategory.SelectedValue);

                await _classificationService.SaveRuleAsync(
                    txtClassificationApp.Text,
                    categoryId
                );

                txtClassificationApp.Clear();

                await LoadClassificationRulesAsync();

                MessageBox.Show(
                    "Classification rule saved successfully.",
                    "Saved",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Error saving classification rule",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private async Task RemoveSelectedClassificationRuleAsync()
        {
            try
            {
                if (dgvClassificationRules.CurrentRow?.DataBoundItem is not AppClassificationRuleDto selectedRule)
                {
                    MessageBox.Show(
                        "Please select a classification rule first.",
                        "No selection",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );

                    return;
                }

                await _classificationService.RemoveRuleAsync(selectedRule.Id);

                await LoadClassificationRulesAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Error removing classification rule",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
    }
}