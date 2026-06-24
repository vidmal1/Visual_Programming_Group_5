namespace FocusTrack.UI.Views
{
    partial class HistoryView
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
            lblHistoryTitle = new Label();
            SuspendLayout();
            // 
            // lblHistoryTitle
            // 
            lblHistoryTitle.Dock = DockStyle.Top;
            lblHistoryTitle.Font = new Font("Segoe UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblHistoryTitle.Location = new Point(0, 0);
            lblHistoryTitle.Name = "lblHistoryTitle";
            lblHistoryTitle.Size = new Size(454, 49);
            lblHistoryTitle.TabIndex = 0;
            lblHistoryTitle.Text = "History View";
            lblHistoryTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // HistoryView
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(lblHistoryTitle);
            Name = "HistoryView";
            Size = new Size(454, 369);
            ResumeLayout(false);
        }

        #endregion

        private Label lblHistoryTitle;
    }
}
