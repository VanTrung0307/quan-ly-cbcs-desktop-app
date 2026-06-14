namespace QuanLyCBCS.Forms.CommonForms
{
    partial class FrmLoading
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
            progressBar1 = new ProgressBar();
            pictureBox1 = new PictureBox();
            label2 = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.FromArgb(44, 62, 80);
            label1.Location = new Point(89, 135);
            label1.Name = "label1";
            label1.Size = new Size(284, 28);
            label1.TabIndex = 0;
            label1.Text = "ĐANG KHỞI TẠO HỆ THỐNG";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // progressBar1
            // 
            progressBar1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            progressBar1.Location = new Point(57, 184);
            progressBar1.MarqueeAnimationSpeed = 30;
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(334, 29);
            progressBar1.Style = ProgressBarStyle.Marquee;
            progressBar1.TabIndex = 1;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.logoCSCD;
            pictureBox1.Location = new Point(166, 12);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(120, 120);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 2;
            pictureBox1.TabStop = false;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 8F);
            label2.ForeColor = Color.Gray;
            label2.Location = new Point(162, 232);
            label2.Name = "label2";
            label2.Size = new Size(124, 19);
            label2.TabIndex = 3;
            label2.Text = "Quản lý CBCS v1.0";
            label2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // FrmLoading
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(450, 260);
            Controls.Add(label2);
            Controls.Add(pictureBox1);
            Controls.Add(progressBar1);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FrmLoading";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FrmLoading";
            TopMost = true;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private ProgressBar progressBar1;
        private PictureBox pictureBox1;
        private Label label2;
    }
}