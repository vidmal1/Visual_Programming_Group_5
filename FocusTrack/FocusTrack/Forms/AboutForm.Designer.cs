namespace FocusTrack.Forms
{
    partial class AboutForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            btnColse = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Impact", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(149, 12);
            label1.Name = "label1";
            label1.Size = new Size(158, 37);
            label1.TabIndex = 0;
            label1.Text = "FocusTrack";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(186, 45);
            label2.Name = "label2";
            label2.Size = new Size(0, 20);
            label2.TabIndex = 1;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(149, 65);
            label3.Name = "label3";
            label3.Size = new Size(0, 20);
            label3.TabIndex = 2;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(118, 85);
            label4.Name = "label4";
            label4.Size = new Size(0, 20);
            label4.TabIndex = 3;
            label4.TextAlign = ContentAlignment.TopCenter;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.Location = new Point(33, 65);
            label5.Name = "label5";
            label5.Size = new Size(430, 100);
            label5.TabIndex = 4;
            label5.Text = "FocusTrack is a desktop productivity tracking application that \r\nmonitors application usage, helping users analyze their work \r\nhabits and improve productivity.\r\n\r\n\r\n";
            label5.Click += label5_Click;
            // 
            // btnColse
            // 
            btnColse.BackColor = Color.Navy;
            btnColse.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnColse.ForeColor = SystemColors.Window;
            btnColse.Location = new Point(397, 273);
            btnColse.Margin = new Padding(3, 4, 3, 4);
            btnColse.Name = "btnColse";
            btnColse.Size = new Size(86, 37);
            btnColse.TabIndex = 5;
            btnColse.Text = "Close";
            btnColse.UseVisualStyleBackColor = false;
            btnColse.Click += btnColse_Click;
            // 
            // AboutForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.AliceBlue;
            ClientSize = new Size(496, 348);
            Controls.Add(btnColse);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(3, 4, 3, 4);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "AboutForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "About FocusTrack";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Button btnColse;
    }
}