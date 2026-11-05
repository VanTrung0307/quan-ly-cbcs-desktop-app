using System.Drawing;
using System.Windows.Forms;
using Color = System.Drawing.Color;
using Font = System.Drawing.Font;

namespace QuanLyCBCS.Forms.UserForms
{
    partial class FrmChangePassword
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            pnlPageHeader = new Panel();
            lblHeaderIcon = new Label();
            lblHeaderTitle = new Label();
            lblHeaderSubtitle = new Label();
            pnlContent = new Panel();
            pnlCard = new Panel();
            tableCard = new TableLayoutPanel();
            pnlIllustration = new Panel();
            pnlSeparator = new Panel();
            pnlFormArea = new Panel();
            lblOldIcon = new Label();
            lblNewIcon = new Label();
            lblConfirmIcon = new Label();
            lblOldCaption = new Label();
            lblNewCaption = new Label();
            lblConfirmCaption = new Label();
            var lblOldRequired = new Label();
            var lblNewRequired = new Label();
            var lblConfirmRequired = new Label();
            pnlOldRow = new Panel();
            pnlNewRow = new Panel();
            pnlConfirmRow = new Panel();
            txtOldPassword = new TextBox();
            txtNewPassword = new TextBox();
            txtConfirmPassword = new TextBox();
            btnToggleOld = new Button();
            btnToggleNew = new Button();
            btnToggleConfirm = new Button();
            btnSave = new Button();
            btnCancel = new Button();
            toolTipMain = new ToolTip(components);

            pnlPageHeader.SuspendLayout();
            pnlContent.SuspendLayout();
            pnlCard.SuspendLayout();
            tableCard.SuspendLayout();
            pnlFormArea.SuspendLayout();
            pnlOldRow.SuspendLayout();
            pnlNewRow.SuspendLayout();
            pnlConfirmRow.SuspendLayout();
            SuspendLayout();

            // Header banner.
            pnlPageHeader.Dock = DockStyle.Top;
            pnlPageHeader.Height = 100;
            pnlPageHeader.BackColor = Color.FromArgb(0, 86, 180);
            pnlPageHeader.Paint += pnlPageHeader_Paint;
            lblHeaderIcon.Font = new Font("Segoe MDL2 Assets", 34F);
            lblHeaderIcon.ForeColor = Color.White;
            lblHeaderIcon.Text = "\uE72E";
            lblHeaderIcon.TextAlign = ContentAlignment.MiddleCenter;
            lblHeaderIcon.Location = new Point(26, 14);
            lblHeaderIcon.Size = new Size(88, 72);
            lblHeaderIcon.BackColor = Color.Transparent;
            lblHeaderTitle.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            lblHeaderTitle.ForeColor = Color.White;
            lblHeaderTitle.Text = "ĐỔI MẬT KHẨU";
            lblHeaderTitle.Location = new Point(132, 13);
            lblHeaderTitle.Size = new Size(520, 45);
            lblHeaderTitle.BackColor = Color.Transparent;
            lblHeaderSubtitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblHeaderSubtitle.ForeColor = Color.White;
            lblHeaderSubtitle.Text = "Cập nhật mật khẩu tài khoản";
            lblHeaderSubtitle.Location = new Point(134, 57);
            lblHeaderSubtitle.Size = new Size(450, 27);
            lblHeaderSubtitle.BackColor = Color.Transparent;
            pnlPageHeader.Controls.Add(lblHeaderIcon);
            pnlPageHeader.Controls.Add(lblHeaderTitle);
            pnlPageHeader.Controls.Add(lblHeaderSubtitle);

            // Main content and centered card.
            pnlContent.Dock = DockStyle.Fill;
            pnlContent.BackColor = Color.FromArgb(239, 247, 255);
            pnlContent.Padding = new Padding(12);
            pnlCard.BackColor = Color.White;
            pnlCard.Size = new Size(1155, 455);
            pnlCard.Padding = new Padding(18);
            pnlCard.Paint += (sender, e) =>
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                using (var path = RoundedRect(
                    new Rectangle(0, 0, pnlCard.Width - 1, pnlCard.Height - 1), 20))
                using (var pen = new Pen(Color.FromArgb(226, 237, 250), 1F))
                    e.Graphics.DrawPath(pen, path);
            };

            tableCard.Dock = DockStyle.Fill;
            tableCard.BackColor = Color.White;
            tableCard.ColumnCount = 2;
            tableCard.RowCount = 1;
            tableCard.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 28F));
            tableCard.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 72F));
            tableCard.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            pnlIllustration.Dock = DockStyle.Fill;
            pnlIllustration.BackColor = Color.White;
            pnlIllustration.Paint += pnlIllustration_Paint;
            pnlSeparator.Dock = DockStyle.Right;
            pnlSeparator.Width = 1;
            pnlSeparator.BackColor = Color.FromArgb(155, 196, 255);
            pnlIllustration.Controls.Add(pnlSeparator);

            pnlFormArea.Dock = DockStyle.Fill;
            pnlFormArea.BackColor = Color.White;
            pnlFormArea.Padding = new Padding(18, 10, 18, 10);

            // Row 1.
            ConfigureRow(pnlOldRow, 0, lblOldIcon, "\uE8D7", lblOldCaption, "Mật khẩu cũ", lblOldRequired, txtOldPassword, "Nhập mật khẩu hiện tại", btnToggleOld);
            ConfigureRow(pnlNewRow, 1, lblNewIcon, "\uE72E", lblNewCaption, "Mật khẩu mới", lblNewRequired, txtNewPassword, "Nhập mật khẩu mới", btnToggleNew);
            ConfigureRow(pnlConfirmRow, 2, lblConfirmIcon, "\uE73E", lblConfirmCaption, "Xác nhận mật khẩu", lblConfirmRequired, txtConfirmPassword, "Nhập lại mật khẩu mới", btnToggleConfirm);

            btnSave.BackColor = Color.FromArgb(16, 174, 91);
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.ForeColor = Color.White;
            btnSave.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnSave.Text = string.Empty;
            btnSave.TextAlign = ContentAlignment.MiddleCenter;
            btnSave.Paint += btnSave_Paint;
            btnSave.Size = new Size(228, 54);
            btnSave.Region = new Region(RoundedRect(new Rectangle(0, 0, 228, 54), 12));
            btnSave.Location = new Point(150, 300);
            btnSave.Cursor = Cursors.Hand;
            btnSave.Click += btnSave_Click;

            btnCancel.BackColor = Color.FromArgb(255, 250, 250);
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.FlatAppearance.BorderColor = Color.FromArgb(239, 54, 65);
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.ForeColor = Color.FromArgb(235, 47, 57);
            btnCancel.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnCancel.Text = string.Empty;
            btnCancel.Paint += btnCancel_Paint;
            btnCancel.Size = new Size(228, 54);
            btnCancel.Region = new Region(RoundedRect(new Rectangle(0, 0, 228, 54), 12));
            btnCancel.Location = new Point(408, 300);
            btnCancel.Cursor = Cursors.Hand;
            btnCancel.Click += btnCancel_Click;

            pnlFormArea.Controls.Add(pnlOldRow);
            pnlFormArea.Controls.Add(pnlNewRow);
            pnlFormArea.Controls.Add(pnlConfirmRow);
            pnlFormArea.Controls.Add(btnSave);
            pnlFormArea.Controls.Add(btnCancel);

            tableCard.Controls.Add(pnlIllustration, 0, 0);
            tableCard.Controls.Add(pnlFormArea, 1, 0);
            pnlCard.Controls.Add(tableCard);
            pnlContent.Controls.Add(pnlCard);

            // Form settings.
            AcceptButton = btnSave;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(239, 247, 255);
            ClientSize = new Size(1360, 740);
            Controls.Add(pnlContent);
            Controls.Add(pnlPageHeader);
            Font = new Font("Segoe UI", 10F);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FrmChangePassword";
            StartPosition = FormStartPosition.Manual;
            Text = "Đổi mật khẩu";

            pnlPageHeader.ResumeLayout(false);
            pnlContent.ResumeLayout(false);
            pnlCard.ResumeLayout(false);
            tableCard.ResumeLayout(false);
            pnlFormArea.ResumeLayout(false);
            pnlOldRow.ResumeLayout(false);
            pnlOldRow.PerformLayout();
            pnlNewRow.ResumeLayout(false);
            pnlNewRow.PerformLayout();
            pnlConfirmRow.ResumeLayout(false);
            pnlConfirmRow.PerformLayout();
            ResumeLayout(false);
        }

        private void ConfigureRow(Panel row, int index, Label icon, string glyph,
            Label caption, string captionText, Label required, TextBox input, string placeholder, Button toggle)
        {
            row.BackColor = Color.Transparent;
            row.Paint += (sender, e) =>
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                using (var path = RoundedRect(row.ClientRectangle, 16))
                using (var brush = new SolidBrush(Color.FromArgb(235, 244, 255)))
                    e.Graphics.FillPath(brush, path);
            };
            row.Size = new Size(790, 68);
            row.Location = new Point(0, 8 + index * 81);
            row.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            icon.Font = new Font("Segoe MDL2 Assets", 25F);
            icon.ForeColor = Color.FromArgb(0, 101, 210);
            icon.Text = glyph;
            icon.TextAlign = ContentAlignment.MiddleCenter;
            icon.Location = new Point(12, 9);
            icon.Size = new Size(52, 50);

            caption.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            caption.ForeColor = Color.FromArgb(10, 43, 105);
            caption.Text = captionText;
            caption.TextAlign = ContentAlignment.MiddleLeft;
            caption.Location = new Point(78, 10);
            caption.Size = new Size(205, 48);
            caption.AutoEllipsis = true;

            required.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            required.ForeColor = Color.FromArgb(235, 45, 55);
            required.Text = "*";
            required.Location = new Point(278, 10);
            required.Size = new Size(18, 48);
            required.TextAlign = ContentAlignment.MiddleLeft;

            // Khung nhập bo góc, chứa TextBox và nút hiện/ẩn.
            var inputFrame = new Panel();
            inputFrame.Name = "pnlInputFrame" + index;
            inputFrame.BackColor = Color.FromArgb(235, 244, 255);
            inputFrame.Location = new Point(305, 9);
            inputFrame.Size = new Size(420, 50);
            inputFrame.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            inputFrame.Paint += (sender, e) =>
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                Rectangle bounds = new Rectangle(0, 0, inputFrame.Width - 1, inputFrame.Height - 1);
                using (var path = RoundedRect(bounds, 12))
                using (var brush = new SolidBrush(Color.White))
                using (var pen = new Pen(Color.FromArgb(218, 224, 232), 1F))
                {
                    e.Graphics.FillPath(brush, path);
                    e.Graphics.DrawPath(pen, path);
                }
            };

            input.Font = new Font("Segoe UI", 12F);
            input.ForeColor = Color.FromArgb(65, 78, 96);
            input.BackColor = Color.White;
            input.BorderStyle = BorderStyle.None;
            input.Location = new Point(13, 14);
            input.Size = new Size(340, 25);
            input.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            input.UseSystemPasswordChar = true;
            input.Margin = Padding.Empty;

            // Nút hiện/ẩn mật khẩu dùng biểu tượng mắt tự vẽ, không dùng glyph dễ sai mã.
            toggle.Font = new Font("Segoe UI", 9F);
            toggle.ForeColor = Color.FromArgb(45, 65, 95);
            toggle.BackColor = Color.White;
            toggle.FlatStyle = FlatStyle.Flat;
            toggle.FlatAppearance.BorderSize = 0;
            toggle.FlatAppearance.MouseDownBackColor = Color.White;
            toggle.FlatAppearance.MouseOverBackColor = Color.FromArgb(245, 249, 255);
            toggle.Text = string.Empty;
            toggle.Tag = input;
            toggle.Size = new Size(50, 40);
            toggle.Location = new Point(360, 5);
            toggle.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            toggle.Cursor = Cursors.Hand;
            toggle.TabStop = false;
            toggle.Paint += ToggleButton_Paint;
            if (index == 0) toggle.Click += btnToggleOld_Click;
            else if (index == 1) toggle.Click += btnToggleNew_Click;
            else toggle.Click += btnToggleConfirm_Click;

            inputFrame.Controls.Add(input);
            inputFrame.Controls.Add(toggle);
            row.Controls.Add(icon);
            row.Controls.Add(caption);
            row.Controls.Add(required);
            row.Controls.Add(inputFrame);

            row.Resize += (sender, e) =>
            {
                inputFrame.Width = Math.Max(260, row.ClientSize.Width - inputFrame.Left - 12);
                toggle.Left = inputFrame.ClientSize.Width - toggle.Width - 5;
                input.Width = Math.Max(120, toggle.Left - input.Left - 2);
            };
        }

        private Panel pnlPageHeader;
        private Label lblHeaderIcon;
        private Label lblHeaderTitle;
        private Label lblHeaderSubtitle;
        private Panel pnlContent;
        private Panel pnlCard;
        private TableLayoutPanel tableCard;
        private Panel pnlIllustration;
        private Panel pnlSeparator;
        private Panel pnlFormArea;
        private Panel pnlOldRow;
        private Panel pnlNewRow;
        private Panel pnlConfirmRow;
        private Label lblOldIcon;
        private Label lblNewIcon;
        private Label lblConfirmIcon;
        private Label lblOldCaption;
        private Label lblNewCaption;
        private Label lblConfirmCaption;
        private TextBox txtOldPassword;
        private TextBox txtNewPassword;
        private TextBox txtConfirmPassword;
        private Button btnToggleOld;
        private Button btnToggleNew;
        private Button btnToggleConfirm;
        private Button btnSave;
        private Button btnCancel;
        private ToolTip toolTipMain;
    }
}