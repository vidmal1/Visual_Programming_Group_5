namespace FocusTrack.UI.Views
{
    partial class DashboardView
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblDashboardTitle = new Label();
            SuspendLayout();
            // 
            // lblDashboardTitle
            // 
            lblDashboardTitle.Dock = DockStyle.Top;
            lblDashboardTitle.Font = new Font("Segoe UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblDashboardTitle.Location = new Point(0, 0);
            lblDashboardTitle.Name = "lblDashboardTitle";
            lblDashboardTitle.Size = new Size(335, 40);
            lblDashboardTitle.TabIndex = 0;
            lblDashboardTitle.Text = "Dashboard View";
            lblDashboardTitle.TextAlign = ContentAlignment.MiddleCenter;
            lblDashboardTitle.Click += lblDashboardTitle_Click;
            // 
            // DashboardView
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(lblDashboardTitle);
            Name = "DashboardView";
            Size = new Size(335, 299);
            ResumeLayout(false);
        }

        #endregion

        private Label lblDashboardTitle;
    }
}
