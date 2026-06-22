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

namespace QuanLyCBCS.Forms.PersonnelForms
{
    public partial class FrmAdminDashboard : Form
    {

        private readonly DashboardService _service =
            new DashboardService();
        public FrmAdminDashboard()
        {
            InitializeComponent();
        }

        private async Task LoadDashboardAsync()
        {
            var data =
                await _service.GetDashboardAsync();

            lblTotalPersonnel.Text =
                $"Tổng CBCS: {data.TotalPersonnel}";

            lblSoldierCount.Text =
                $"Chiến sĩ: {data.SoldierCount}";

            lblOfficerCount.Text =
                $"Cán bộ: {data.OfficerCount}";

            lblTotalUsers.Text =
                $"Tổng TK: {data.TotalUsers}";

            lblAdminCount.Text =
                $"Admin: {data.AdminCount}";

            lblUserCount.Text =
                $"User: {data.UserCount}";

            lblLockedUsers.Text =
                $"Đang khóa: {data.LockedUsers}";

            lblTotalMission.Text =
                $"Tổng CT: {data.TotalMission}";

            lblActiveMission.Text =
                $"Đang diễn ra: {data.ActiveMission}";

            lblEndingMission.Text =
                $"Sắp kết thúc: {data.EndingMission}";

            lblFinishedMission.Text =
                $"Đã kết thúc: {data.FinishedMission}";

            lblLoginToday.Text =
                $"LOGIN: {data.LoginToday}";

            lblFailedLoginToday.Text =
                $"FAILED LOGIN: {data.FailedLoginToday}";

            lblLockToday.Text =
                $"LOCK ACCOUNT: {data.LockToday}";

            lblResetToday.Text =
                $"RESET PASSWORD: {data.ResetToday}";
        }

        private async void FrmAdminDashboard_Load(
            object sender,
            EventArgs e)
        {
            DashboardService service =
                new DashboardService();

            await LoadDashboardAsync();

            dgvRecentLogs.DataSource =
                await service.GetRecentLogsAsync();
        }

        private async void btnRefresh_Click(
            object sender,
            EventArgs e)
        {
            await LoadDashboardAsync();
        }
    }
}
