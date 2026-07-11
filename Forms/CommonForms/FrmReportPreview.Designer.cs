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
            label1.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            label1.Location = new Point(0, 0);
            label1.Name = "label1";
            label1.Size = new Size(800, 54);
            label1.TabIndex = 0;
            label1.Text = "XEM BÁO CÁO";
            label1.TextAlign = ContentAlignment.TopCenter;
            // 
            // rtbReport
            // 
            rtbReport.Dock = DockStyle.Fill;
            rtbReport.Font = new Font("Consolas", 11F);
            rtbReport.Location = new Point(0, 54);
            rtbReport.Name = "rtbReport";
            rtbReport.ReadOnly = true;
            rtbReport.Size = new Size(800, 396);
            rtbReport.TabIndex = 1;
            rtbReport.Text = "";
            rtbReport.WordWrap = false;
            // 
            // panel1
            // 
            panel1.Controls.Add(btnClose);
            panel1.Controls.Add(btnCopy);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 395);
            panel1.Name = "panel1";
            panel1.Size = new Size(800, 55);
            panel1.TabIndex = 2;
            // 
            // btnClose
            // 
            btnClose.Location = new Point(462, 14);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(94, 29);
            btnClose.TabIndex = 2;
            btnClose.Text = "Đóng";
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += btnClose_Click;
            // 
            // btnCopy
            // 
            btnCopy.Location = new Point(177, 14);
            btnCopy.Name = "btnCopy";
            btnCopy.Size = new Size(146, 29);
            btnCopy.TabIndex = 0;
            btnCopy.Text = "📋 Copy báo cáo";
            btnCopy.UseVisualStyleBackColor = true;
            btnCopy.Click += btnCopy_Click;
            // 
            // FrmReportPreview
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(panel1);
            Controls.Add(rtbReport);
            Controls.Add(label1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "FrmReportPreview";
            Text = "FrmReportPreview";
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