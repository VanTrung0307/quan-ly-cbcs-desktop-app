namespace QuanLyCBCS.Forms.PersonnelForms
{
    partial class FrmPersonnelList
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
            txtSearch = new TextBox();
            btnAdd = new Button();
            btnEdit = new Button();
            btnDelete = new Button();
            btnRefresh = new Button();
            dgvPersonnel = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dgvPersonnel).BeginInit();
            SuspendLayout();
            // 
            // txtSearch
            // 
            txtSearch.Font = new Font("Segoe UI", 11F);
            txtSearch.Location = new Point(62, 56);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "Tìm kiếm CBCS...";
            txtSearch.Size = new Size(273, 32);
            txtSearch.TabIndex = 0;
            // 
            // btnAdd
            // 
            btnAdd.Location = new Point(361, 59);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(94, 29);
            btnAdd.TabIndex = 1;
            btnAdd.Text = "Thêm";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += btnAdd_Click;
            // 
            // btnEdit
            // 
            btnEdit.Location = new Point(487, 59);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new Size(94, 29);
            btnEdit.TabIndex = 2;
            btnEdit.Text = "Sửa";
            btnEdit.UseVisualStyleBackColor = true;
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(614, 59);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(94, 29);
            btnDelete.TabIndex = 3;
            btnDelete.Text = "Xóa";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            // 
            // btnRefresh
            // 
            btnRefresh.Location = new Point(694, 123);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(94, 29);
            btnRefresh.TabIndex = 4;
            btnRefresh.Text = "Làm mới";
            btnRefresh.UseVisualStyleBackColor = true;
            // 
            // dgvPersonnel
            // 
            dgvPersonnel.AllowUserToAddRows = false;
            dgvPersonnel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvPersonnel.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPersonnel.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvPersonnel.Location = new Point(1, 158);
            dgvPersonnel.MultiSelect = false;
            dgvPersonnel.Name = "dgvPersonnel";
            dgvPersonnel.ReadOnly = true;
            dgvPersonnel.RowHeadersVisible = false;
            dgvPersonnel.RowHeadersWidth = 51;
            dgvPersonnel.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPersonnel.Size = new Size(798, 293);
            dgvPersonnel.TabIndex = 5;
            dgvPersonnel.CellClick += dgvPersonnel_CellClick;
            dgvPersonnel.CellDoubleClick += dgvPersonnel_CellDoubleClick;
            // 
            // FrmPersonnelList
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(dgvPersonnel);
            Controls.Add(btnRefresh);
            Controls.Add(btnDelete);
            Controls.Add(btnEdit);
            Controls.Add(btnAdd);
            Controls.Add(txtSearch);
            Name = "FrmPersonnelList";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FrmPersonnelList";
            WindowState = FormWindowState.Maximized;
            Load += FrmPersonnelList_Load;
            ((System.ComponentModel.ISupportInitialize)dgvPersonnel).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtSearch;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnRefresh;
        private DataGridView dgvPersonnel;
    }
}