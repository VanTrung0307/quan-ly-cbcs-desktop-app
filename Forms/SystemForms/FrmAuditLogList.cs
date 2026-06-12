using QuanLyCBCS.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyCBCS.Forms.SystemForms
{
    public partial class FrmAuditLogList : Form
    {
        private readonly AuditLogService
            _auditService =
                new AuditLogService();
        public FrmAuditLogList()
        {
            InitializeComponent();
        }

        private async Task LoadAuditLogsAsync()
        {
            dgvAuditLogs.DataSource = null;

            dgvAuditLogs.DataSource =
                await _auditService
                    .GetAllAsync();
        }

        private async void FrmAuditLogList_Load(
            object sender,
            EventArgs e)
        {
            await LoadAuditLogsAsync();
        }

        private async void btnRefresh_Click(
            object sender,
            EventArgs e)
        {
            await LoadAuditLogsAsync();
        }
    }
}
