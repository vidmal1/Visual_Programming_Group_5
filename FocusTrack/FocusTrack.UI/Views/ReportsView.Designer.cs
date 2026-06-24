namespace FocusTrack.UI.Views
{
    partial class ReportsView
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
            lblReportsTitle = new Label();
            SuspendLayout();
            // 
            // lblReportsTitle
            // 
            lblReportsTitle.Dock = DockStyle.Top;
            lblReportsTitle.Font = new Font("Segoe UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblReportsTitle.Location = new Point(0, 0);
            lblReportsTitle.Name = "lblReportsTitle";
            lblReportsTitle.Size = new Size(296, 48);
            lblReportsTitle.TabIndex = 0;
            lblReportsTitle.Text = "Reports View";
            lblReportsTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // ReportsView
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(lblReportsTitle);
            Name = "ReportsView";
            Size = new Size(296, 150);
            ResumeLayout(false);
        }

        #endregion

        private Label lblReportsTitle;
    }
}
