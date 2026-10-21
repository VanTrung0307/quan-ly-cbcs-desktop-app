namespace QuanLyCBCS.Forms.MissionForms
{
    partial class FrmMissionDetail
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMissionDetail));
            pnlHeader = new Panel();
            lblTitle = new Label();
            txtMissionName = new TextBox();
            cboMissionType = new ComboBox();
            dtpStartTime = new DateTimePicker();
            dtpEndTime = new DateTimePicker();
            txtLocation = new TextBox();
            rtbDescription = new RichTextBox();
            chkIsActive = new CheckBox();
            lblMissionName = new Label();
            lblMissionType = new Label();
            lblStart = new Label();
            lblEnd = new Label();
            lblLocation = new Label();
            lblDescription = new Label();
            btnCancel = new Button();
            btnSave = new Button();
            toolTipMain = new ToolTip(components);
            label1 = new Label();
            cboPriority = new ComboBox();
            pnlHeader.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(47, 69, 92);
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(882, 80);
            pnlHeader.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(254, 18);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(345, 41);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "THÔNG TIN CÔNG TÁC";
            // 
            // txtMissionName
            // 
            txtMissionName.Location = new Point(153, 139);
            txtMissionName.Name = "txtMissionName";
            txtMissionName.Size = new Size(280, 27);
            txtMissionName.TabIndex = 1;
            // 
            // cboMissionType
            // 
            cboMissionType.FormattingEnabled = true;
            cboMissionType.Items.AddRange(new object[] { "Bảo vệ mục tiêu", "Trưng dụng", "Tăng cường", "Huấn luyện", "Học tập", "Thi đấu", "Diễn tập", "Khác" });
            cboMissionType.Location = new Point(153, 199);
            cboMissionType.Name = "cboMissionType";
            cboMissionType.Size = new Size(202, 28);
            cboMissionType.TabIndex = 2;
            // 
            // dtpStartTime
            // 
            dtpStartTime.CustomFormat = "dd/MM/yyy HH:mm";
            dtpStartTime.Format = DateTimePickerFormat.Custom;
            dtpStartTime.Location = new Point(594, 142);
            dtpStartTime.Name = "dtpStartTime";
            dtpStartTime.Size = new Size(180, 27);
            dtpStartTime.TabIndex = 3;
            // 
            // dtpEndTime
            // 
            dtpEndTime.CustomFormat = "dd/MM/yyy HH:mm";
            dtpEndTime.Format = DateTimePickerFormat.Custom;
            dtpEndTime.Location = new Point(594, 200);
            dtpEndTime.Name = "dtpEndTime";
            dtpEndTime.Size = new Size(180, 27);
            dtpEndTime.TabIndex = 4;
            // 
            // txtLocation
            // 
            txtLocation.Location = new Point(153, 262);
            txtLocation.Name = "txtLocation";
            txtLocation.Size = new Size(202, 27);
            txtLocation.TabIndex = 5;
            // 
            // rtbDescription
            // 
            rtbDescription.Location = new Point(153, 360);
            rtbDescription.Name = "rtbDescription";
            rtbDescription.Size = new Size(566, 140);
            rtbDescription.TabIndex = 6;
            rtbDescription.Text = "";
            // 
            // chkIsActive
            // 
            chkIsActive.AutoSize = true;
            chkIsActive.Location = new Point(579, 330);
            chkIsActive.Name = "chkIsActive";
            chkIsActive.Size = new Size(140, 24);
            chkIsActive.TabIndex = 7;
            chkIsActive.Text = "Đang hoạt động";
            chkIsActive.UseVisualStyleBackColor = true;
            // 
            // lblMissionName
            // 
            lblMissionName.AutoSize = true;
            lblMissionName.Location = new Point(54, 142);
            lblMissionName.Name = "lblMissionName";
            lblMissionName.Size = new Size(93, 20);
            lblMissionName.TabIndex = 8;
            lblMissionName.Text = "Tên công tác";
            // 
            // lblMissionType
            // 
            lblMissionType.AutoSize = true;
            lblMissionType.Location = new Point(54, 204);
            lblMissionType.Name = "lblMissionType";
            lblMissionType.Size = new Size(98, 20);
            lblMissionType.TabIndex = 9;
            lblMissionType.Text = "Loại công tác";
            // 
            // lblStart
            // 
            lblStart.AutoSize = true;
            lblStart.Location = new Point(528, 147);
            lblStart.Name = "lblStart";
            lblStart.Size = new Size(60, 20);
            lblStart.TabIndex = 10;
            lblStart.Text = "Bắt đầu";
            // 
            // lblEnd
            // 
            lblEnd.AutoSize = true;
            lblEnd.Location = new Point(525, 202);
            lblEnd.Name = "lblEnd";
            lblEnd.Size = new Size(63, 20);
            lblEnd.TabIndex = 11;
            lblEnd.Text = "Kết thúc";
            // 
            // lblLocation
            // 
            lblLocation.AutoSize = true;
            lblLocation.Location = new Point(77, 265);
            lblLocation.Name = "lblLocation";
            lblLocation.Size = new Size(70, 20);
            lblLocation.TabIndex = 12;
            lblLocation.Text = "Địa điểm";
            // 
            // lblDescription
            // 
            lblDescription.AutoSize = true;
            lblDescription.Location = new Point(99, 363);
            lblDescription.Name = "lblDescription";
            lblDescription.Size = new Size(48, 20);
            lblDescription.TabIndex = 13;
            lblDescription.Text = "Mô tả";
            // 
            // btnCancel
            // 
            btnCancel.BackColor = Color.FromArgb(231, 76, 60);
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnCancel.ForeColor = Color.White;
            btnCancel.Location = new Point(440, 532);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(130, 40);
            btnCancel.TabIndex = 17;
            btnCancel.Text = "❌ Huỷ";
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += btnCancel_Click;
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.FromArgb(39, 174, 96);
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(275, 532);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(130, 40);
            btnSave.TabIndex = 16;
            btnSave.Text = "💾 Lưu";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            // 
            // toolTipMain
            // 
            toolTipMain.AutoPopDelay = 5000;
            toolTipMain.InitialDelay = 300;
            toolTipMain.ReshowDelay = 100;
            toolTipMain.ShowAlways = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(478, 265);
            label1.Name = "label1";
            label1.Size = new Size(110, 20);
            label1.TabIndex = 18;
            label1.Text = "Mức độ ưu tiên";
            // 
            // cboPriority
            // 
            cboPriority.DropDownStyle = ComboBoxStyle.DropDownList;
            cboPriority.FormattingEnabled = true;
            cboPriority.Location = new Point(594, 261);
            cboPriority.Name = "cboPriority";
            cboPriority.Size = new Size(180, 28);
            cboPriority.TabIndex = 19;
            // 
            // FrmMissionDetail
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(882, 603);
            Controls.Add(cboPriority);
            Controls.Add(label1);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(lblDescription);
            Controls.Add(lblLocation);
            Controls.Add(lblEnd);
            Controls.Add(lblStart);
            Controls.Add(lblMissionType);
            Controls.Add(lblMissionName);
            Controls.Add(chkIsActive);
            Controls.Add(rtbDescription);
            Controls.Add(txtLocation);
            Controls.Add(dtpEndTime);
            Controls.Add(dtpStartTime);
            Controls.Add(cboMissionType);
            Controls.Add(txtMissionName);
            Controls.Add(pnlHeader);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "FrmMissionDetail";
            Text = "THÔNG TIN CÔNG TÁC";
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel pnlHeader;
        private Label lblTitle;
        public TextBox txtMissionName;
        public ComboBox cboMissionType;
        public DateTimePicker dtpStartTime;
        public DateTimePicker dtpEndTime;
        public TextBox txtLocation;
        public RichTextBox rtbDescription;
        public CheckBox chkIsActive;
        private Label lblMissionName;
        private Label lblMissionType;
        private Label lblStart;
        private Label lblEnd;
        private Label lblLocation;
        private Label lblDescription;
        public Button btnCancel;
        public Button btnSave;
        public ToolTip toolTipMain;
        private Label label1;
        private ComboBox cboPriority;
    }
}