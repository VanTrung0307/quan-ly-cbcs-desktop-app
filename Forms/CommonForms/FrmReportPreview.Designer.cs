namespace QuanLyCBCS.Forms.CommonForms
{
    partial class FrmReportPreview
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmReportPreview));
            label1 = new Label();
            rtbReport = new RichTextBox();
            panel1 = new Panel();
            btnClose = new Button();
            btnCopy = new Button();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.Dock = DockStyle.Top;
            label1.Font = new Font("Segoe UI Semibold", 22F, FontStyle.Bold);
            label1.ForeColor = Color.Black;
            label1.Location = new Point(0, 0);
            label1.Name = "label1";
            label1.Size = new Size(962, 70);
            label1.TabIndex = 0;
            label1.Text = "XEM BÁO CÁO";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // rtbReport
            // 
            rtbReport.BackColor = Color.White;
            rtbReport.BorderStyle = BorderStyle.None;
            rtbReport.DetectUrls = false;
            rtbReport.Dock = DockStyle.Fill;
            rtbReport.Font = new Font("Consolas", 11F);
            rtbReport.HideSelection = false;
            rtbReport.Location = new Point(0, 70);
            rtbReport.Margin = new Padding(10);
            rtbReport.Name = "rtbReport";
            rtbReport.ReadOnly = true;
            rtbReport.ScrollBars = RichTextBoxScrollBars.Vertical;
            rtbReport.Size = new Size(962, 583);
            rtbReport.TabIndex = 1;
            rtbReport.Text = "";
            rtbReport.WordWrap = false;
            // 
            // panel1
            // 
            panel1.Controls.Add(btnClose);
            panel1.Controls.Add(btnCopy);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 593);
            panel1.Name = "panel1";
            panel1.Size = new Size(962, 60);
            panel1.TabIndex = 2;
            // 
            // btnClose
            // 
            btnClose.BackColor = Color.FromArgb(231, 76, 60);
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new Font("Segoe UI Semibold", 9F);
            btnClose.ForeColor = Color.White;
            btnClose.Location = new Point(600, 12);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(120, 36);
            btnClose.TabIndex = 2;
            btnClose.Text = "Đóng";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;
            // 
            // btnCopy
            // 
            btnCopy.BackColor = Color.FromArgb(52, 152, 219);
            btnCopy.FlatAppearance.BorderSize = 0;
            btnCopy.FlatStyle = FlatStyle.Flat;
            btnCopy.Font = new Font("Segoe UI Semibold", 9F);
            btnCopy.ForeColor = Color.White;
            btnCopy.Location = new Point(250, 12);
            btnCopy.Name = "btnCopy";
            btnCopy.Size = new Size(120, 36);
            btnCopy.TabIndex = 0;
            btnCopy.Text = "📋 Sao chép";
            btnCopy.UseVisualStyleBackColor = false;
            btnCopy.Click += btnCopy_Click;
            // 
            // FrmReportPreview
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(962, 653);
            Controls.Add(panel1);
            Controls.Add(rtbReport);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            MinimumSize = new Size(900, 650);
            Name = "FrmReportPreview";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Xem báo cáo";
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Label label1;
        private RichTextBox rtbReport;
        private Panel panel1;
        private Button btnClose;
        private Button btnCopy;
    }
}