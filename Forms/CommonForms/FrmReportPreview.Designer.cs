using System.Drawing;
using System.Windows.Forms;

namespace QuanLyCBCS.Forms.CommonForms
{
    partial class FrmReportPreview
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            pnlHeader = new Panel();
            label1 = new Label();
            btnHeaderClose = new Button();
            pnlReportContainer = new Panel();
            rtbReport = new RichTextBox();
            panel1 = new Panel();
            btnClose = new Button();
            btnCopy = new Button();

            pnlHeader.SuspendLayout();
            pnlReportContainer.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();

            pnlHeader.BackColor = Color.FromArgb(0, 91, 174);
            pnlHeader.Controls.Add(label1);
            pnlHeader.Controls.Add(btnHeaderClose);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(1280, 78);
            pnlHeader.TabIndex = 0;
            pnlHeader.Paint += pnlHeader_Paint;

            label1.BackColor = Color.Transparent;
            label1.Dock = DockStyle.Fill;
            label1.Font = new Font("Segoe UI", 21F, FontStyle.Bold, GraphicsUnit.Point);
            label1.ForeColor = Color.White;
            label1.Padding = new Padding(102, 0, 0, 0);
            label1.Name = "label1";
            label1.Text = "Xem báo cáo";
            label1.TextAlign = ContentAlignment.MiddleLeft;

            btnHeaderClose.BackColor = Color.Transparent;
            btnHeaderClose.Dock = DockStyle.Right;
            btnHeaderClose.FlatAppearance.BorderSize = 0;
            btnHeaderClose.FlatAppearance.MouseDownBackColor = Color.FromArgb(0, 65, 135);
            btnHeaderClose.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 82, 160);
            btnHeaderClose.FlatStyle = FlatStyle.Flat;
            btnHeaderClose.Font = new Font("Segoe UI Light", 30F, FontStyle.Regular, GraphicsUnit.Point);
            btnHeaderClose.ForeColor = Color.White;
            btnHeaderClose.Name = "btnHeaderClose";
            btnHeaderClose.Size = new Size(82, 78);
            btnHeaderClose.Text = "×";
            btnHeaderClose.UseVisualStyleBackColor = false;
            btnHeaderClose.Click += btnClose_Click;

            pnlReportContainer.BackColor = Color.White;
            pnlReportContainer.Controls.Add(rtbReport);
            pnlReportContainer.Dock = DockStyle.Fill;
            pnlReportContainer.Name = "pnlReportContainer";
            pnlReportContainer.Padding = new Padding(60, 20, 60, 20);
            pnlReportContainer.TabIndex = 1;

            rtbReport.BackColor = Color.White;
            rtbReport.BorderStyle = BorderStyle.None;
            rtbReport.Dock = DockStyle.Fill;
            rtbReport.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            rtbReport.ForeColor = Color.FromArgb(20, 37, 68);
            rtbReport.Name = "rtbReport";
            rtbReport.ReadOnly = true;
            rtbReport.ScrollBars = RichTextBoxScrollBars.Vertical;
            rtbReport.TabStop = false;
            rtbReport.Visible = false;

            panel1.BackColor = Color.FromArgb(239, 245, 254);
            panel1.Controls.Add(btnClose);
            panel1.Controls.Add(btnCopy);
            panel1.Dock = DockStyle.Bottom;
            panel1.Name = "panel1";
            panel1.Size = new Size(1280, 100);
            panel1.TabIndex = 2;
            panel1.Resize += panel1_Resize;
            panel1.Paint += panel1_Paint;

            btnCopy.BackColor = Color.FromArgb(0, 105, 220);
            btnCopy.FlatAppearance.BorderColor = Color.FromArgb(0, 105, 220);
            btnCopy.FlatAppearance.BorderSize = 1;
            btnCopy.FlatAppearance.MouseDownBackColor = Color.FromArgb(0, 78, 180);
            btnCopy.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 91, 200);
            btnCopy.FlatStyle = FlatStyle.Flat;
            btnCopy.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point);
            btnCopy.ForeColor = Color.White;
            btnCopy.Name = "btnCopy";
            btnCopy.Size = new Size(224, 58);
            btnCopy.Text = "Sao chép";
            btnCopy.AccessibleName = "Sao chép báo cáo";
            btnCopy.UseVisualStyleBackColor = false;
            btnCopy.Click += btnCopy_Click;

            btnClose.BackColor = Color.White;
            btnClose.FlatAppearance.BorderColor = Color.FromArgb(220, 35, 35);
            btnClose.FlatAppearance.BorderSize = 1;
            btnClose.FlatAppearance.MouseDownBackColor = Color.FromArgb(255, 235, 235);
            btnClose.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 244, 244);
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point);
            btnClose.ForeColor = Color.FromArgb(205, 38, 38);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(202, 58);
            btnClose.Text = "Đóng";
            btnClose.AccessibleName = "Đóng cửa sổ xem báo cáo";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;

            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.White;
            ClientSize = new Size(1280, 840);
            Controls.Add(pnlReportContainer);
            Controls.Add(panel1);
            Controls.Add(pnlHeader);
            FormBorderStyle = FormBorderStyle.None;
            MaximizeBox = false;
            MinimizeBox = false;
            MinimumSize = new Size(1100, 700);
            Name = "FrmReportPreview";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Xem báo cáo";

            pnlHeader.ResumeLayout(false);
            pnlReportContainer.ResumeLayout(false);
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        private Panel pnlHeader;
        private Label label1;
        private Button btnHeaderClose;
        private Panel pnlReportContainer;
        private RichTextBox rtbReport;
        private Panel panel1;
        private Button btnClose;
        private Button btnCopy;
    }
}