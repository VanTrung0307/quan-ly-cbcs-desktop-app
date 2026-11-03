namespace QuanLyCBCS.Forms.MissionForms
{
    partial class FrmMissionDetail
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            pnlHeader = new Panel();
            lblHeaderIcon = new Label();
            lblTitle = new Label();
            btnClose = new Button();
            pnlBody = new Panel();
            lblMissionName = new Label();
            txtMissionName = new TextBox();
            lblMissionType = new Label();
            cboMissionType = new ComboBox();
            lblLocation = new Label();
            txtLocation = new TextBox();
            lblStart = new Label();
            dtpStartTime = new MissionDateTimePicker();
            lblEnd = new Label();
            dtpEndTime = new MissionDateTimePicker();
            lblPriority = new Label();
            cboPriority = new ComboBox();
            chkIsActive = new CheckBox();
            lblDescription = new Label();
            rtbDescription = new RichTextBox();
            pnlFooter = new Panel();
            btnSave = new Button();
            btnCancel = new Button();
            toolTipMain = new ToolTip(components);
            pnlHeader.SuspendLayout();
            pnlBody.SuspendLayout();
            pnlFooter.SuspendLayout();
            SuspendLayout();
            // pnlHeader
            pnlHeader.BackColor = Color.FromArgb(0, 83, 174);
            pnlHeader.Controls.Add(lblHeaderIcon);
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(btnClose);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(920, 68);
            pnlHeader.TabIndex = 0;
            // lblHeaderIcon
            lblHeaderIcon.Location = new Point(24, 10);
            lblHeaderIcon.Name = "lblHeaderIcon";
            lblHeaderIcon.Size = new Size(48, 48);
            lblHeaderIcon.TabIndex = 0;
            // lblTitle
            lblTitle.AutoSize = false;
            lblTitle.Font = new Font("Segoe UI", 20F, FontStyle.Bold, GraphicsUnit.Point);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(95, 10);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(650, 48);
            lblTitle.TabIndex = 1;
            lblTitle.Text = "THÔNG TIN CÔNG TÁC";
            lblTitle.TextAlign = ContentAlignment.MiddleLeft;
            // btnClose
            btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClose.BackColor = Color.Transparent;
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.ForeColor = Color.White;
            btnClose.Location = new Point(860, 8);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(48, 48);
            btnClose.TabIndex = 20;
            btnClose.Text = "×";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnCancel_Click;
            // pnlBody
            pnlBody.BackColor = Color.FromArgb(247, 250, 255);
            pnlBody.Controls.Add(lblMissionName);
            pnlBody.Controls.Add(txtMissionName);
            pnlBody.Controls.Add(lblMissionType);
            pnlBody.Controls.Add(cboMissionType);
            pnlBody.Controls.Add(lblLocation);
            pnlBody.Controls.Add(txtLocation);
            pnlBody.Controls.Add(lblStart);
            pnlBody.Controls.Add(dtpStartTime);
            pnlBody.Controls.Add(lblEnd);
            pnlBody.Controls.Add(dtpEndTime);
            pnlBody.Controls.Add(lblPriority);
            pnlBody.Controls.Add(cboPriority);
            pnlBody.Controls.Add(chkIsActive);
            pnlBody.Controls.Add(lblDescription);
            pnlBody.Controls.Add(rtbDescription);
            pnlBody.Dock = DockStyle.Fill;
            pnlBody.Location = new Point(0, 68);
            pnlBody.Name = "pnlBody";
            pnlBody.Size = new Size(920, 432);
            pnlBody.TabIndex = 1;
            // labels and fields - positions follow the supplied reference
            lblMissionName.Location = new Point(30, 28);
            lblMissionName.Name = "lblMissionName";
            lblMissionName.Size = new Size(140, 28);
            lblMissionName.TabIndex = 0;
            lblMissionName.Text = "Tên công tác";
            txtMissionName.Location = new Point(175, 24);
            txtMissionName.Name = "txtMissionName";
            txtMissionName.Size = new Size(210, 28);
            txtMissionName.TabIndex = 1;
            lblMissionType.Location = new Point(30, 84);
            lblMissionType.Name = "lblMissionType";
            lblMissionType.Size = new Size(140, 28);
            lblMissionType.TabIndex = 2;
            lblMissionType.Text = "Loại công tác";
            cboMissionType.DropDownStyle = ComboBoxStyle.DropDownList;
            cboMissionType.FormattingEnabled = true;
            cboMissionType.Location = new Point(175, 80);
            cboMissionType.Name = "cboMissionType";
            cboMissionType.Size = new Size(210, 28);
            cboMissionType.TabIndex = 3;
            lblLocation.Location = new Point(30, 140);
            lblLocation.Name = "lblLocation";
            lblLocation.Size = new Size(140, 28);
            lblLocation.TabIndex = 4;
            lblLocation.Text = "Địa điểm";
            txtLocation.Location = new Point(175, 136);
            txtLocation.Name = "txtLocation";
            txtLocation.Size = new Size(210, 28);
            txtLocation.TabIndex = 5;
            lblStart.Location = new Point(420, 28);
            lblStart.Name = "lblStart";
            lblStart.Size = new Size(150, 28);
            lblStart.TabIndex = 6;
            lblStart.Text = "Bắt đầu*";
            dtpStartTime.CustomFormat = "dd/MM/yyyy HH:mm";
            dtpStartTime.Format = DateTimePickerFormat.Custom;
            dtpStartTime.Location = new Point(580, 22);
            dtpStartTime.Name = "dtpStartTime";
            dtpStartTime.Size = new Size(210, 36);
            dtpStartTime.TabIndex = 7;
            lblEnd.Location = new Point(420, 84);
            lblEnd.Name = "lblEnd";
            lblEnd.Size = new Size(150, 28);
            lblEnd.TabIndex = 8;
            lblEnd.Text = "Kết thúc*";
            dtpEndTime.CustomFormat = "dd/MM/yyyy HH:mm";
            dtpEndTime.Format = DateTimePickerFormat.Custom;
            dtpEndTime.Location = new Point(580, 78);
            dtpEndTime.Name = "dtpEndTime";
            dtpEndTime.Size = new Size(210, 36);
            dtpEndTime.TabIndex = 9;
            lblPriority.Location = new Point(420, 140);
            lblPriority.Name = "lblPriority";
            lblPriority.Size = new Size(155, 28);
            lblPriority.TabIndex = 10;
            lblPriority.Text = "Mức độ ưu tiên";
            cboPriority.DropDownStyle = ComboBoxStyle.DropDownList;
            cboPriority.FormattingEnabled = true;
            cboPriority.Location = new Point(580, 136);
            cboPriority.Name = "cboPriority";
            cboPriority.Size = new Size(210, 28);
            cboPriority.TabIndex = 11;
            chkIsActive.AutoSize = true;
            chkIsActive.Location = new Point(580, 194);
            chkIsActive.Name = "chkIsActive";
            chkIsActive.Size = new Size(160, 24);
            chkIsActive.TabIndex = 12;
            chkIsActive.Text = "Đang hoạt động";
            chkIsActive.UseVisualStyleBackColor = true;
            lblDescription.Location = new Point(30, 252);
            lblDescription.Name = "lblDescription";
            lblDescription.Size = new Size(140, 28);
            lblDescription.TabIndex = 13;
            lblDescription.Text = "Mô tả";
            rtbDescription.Location = new Point(175, 238);
            rtbDescription.Name = "rtbDescription";
            rtbDescription.Size = new Size(685, 132);
            rtbDescription.TabIndex = 14;
            rtbDescription.Text = "";
            // pnlFooter
            pnlFooter.BackColor = Color.FromArgb(230, 242, 255);
            pnlFooter.Controls.Add(btnSave);
            pnlFooter.Controls.Add(btnCancel);
            pnlFooter.Dock = DockStyle.Bottom;
            pnlFooter.Location = new Point(0, 500);
            pnlFooter.Name = "pnlFooter";
            pnlFooter.Size = new Size(920, 70);
            pnlFooter.TabIndex = 2;
            // btnSave
            btnSave.BackColor = Color.FromArgb(0, 166, 91);
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(305, 15);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(142, 40);
            btnSave.TabIndex = 15;
            btnSave.Text = "";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            // btnCancel
            btnCancel.BackColor = Color.White;
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnCancel.ForeColor = Color.FromArgb(225, 35, 48);
            btnCancel.Location = new Point(463, 15);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(142, 40);
            btnCancel.TabIndex = 16;
            btnCancel.Text = "✕  Hủy";
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += btnCancel_Click;
            // Form
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.FromArgb(247, 250, 255);
            ClientSize = new Size(920, 570);
            Controls.Add(pnlBody);
            Controls.Add(pnlFooter);
            Controls.Add(pnlHeader);
            Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            Name = "FrmMissionDetail";
            StartPosition = FormStartPosition.CenterParent;
            Text = "THÔNG TIN CÔNG TÁC";
            pnlHeader.ResumeLayout(false);
            pnlBody.ResumeLayout(false);
            pnlBody.PerformLayout();
            pnlFooter.ResumeLayout(false);
            ResumeLayout(false);
        }
        #endregion

        private Panel pnlHeader;
        private Label lblHeaderIcon;
        private Label lblTitle;
        private Button btnClose;
        private Panel pnlBody;
        public TextBox txtMissionName;
        public ComboBox cboMissionType;
        public MissionDateTimePicker dtpStartTime;
        public MissionDateTimePicker dtpEndTime;
        public TextBox txtLocation;
        public RichTextBox rtbDescription;
        public CheckBox chkIsActive;
        private Label lblMissionName;
        private Label lblMissionType;
        private Label lblStart;
        private Label lblEnd;
        private Label lblLocation;
        private Label lblDescription;
        private Label lblPriority;
        public ComboBox cboPriority;
        private Panel pnlFooter;
        public Button btnCancel;
        public Button btnSave;
        public ToolTip toolTipMain;
    }
}