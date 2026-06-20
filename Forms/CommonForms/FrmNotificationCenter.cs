using QuanLyCBCS.DTOs;
using QuanLyCBCS.Services;

namespace QuanLyCBCS.Forms.CommonForms
{
    public partial class FrmNotificationCenter : Form
    {
        private readonly NotificationService _notificationService;

        private List<NotificationDto> _notifications = new();
        public FrmNotificationCenter()
        {
            InitializeComponent();

            dgvNotifications.CellFormatting += dgvNotifications_CellFormatting;

            _notificationService =
                new NotificationService();
        }

        private async Task LoadNotificationsAsync()
        {
            _notifications =
                await _notificationService
                    .GetNotificationsAsync();

            dgvNotifications.DataSource = null;
            dgvNotifications.DataSource = _notifications;

            CustomizeGrid();

            foreach (var item in _notifications)
            {
                item.Type = item.Type switch
                {
                    "BIRTHDAY" => "🎂 Sinh nhật",

                    "MISSION_ENDING" => "⚠ Sắp kết thúc",

                    "MISSION_OVERDUE" => "❌ Quá hạn",

                    _ => item.Type
                };
            }
        }

        private async void FrmNotificationCenter_Load(
            object sender,
            EventArgs e)
        {
            await LoadNotificationsAsync();
        }

        private void CustomizeGrid()
        {
            if (dgvNotifications.Columns.Count == 0)
                return;

            dgvNotifications.Columns["Type"].HeaderText =
                "Loại";

            dgvNotifications.Columns["Title"].HeaderText =
                "Tiêu đề";

            dgvNotifications.Columns["Message"].HeaderText =
                "Nội dung";

            dgvNotifications.Columns["Date"].HeaderText =
                "Ngày";

            dgvNotifications.Columns["Date"]
                .DefaultCellStyle.Format =
                "dd/MM/yyyy";
        }

        private async void btnRefresh_Click(
            object sender,
            EventArgs e)
        {
            await LoadNotificationsAsync();
        }

        private void btnClose_Click(
            object sender,
            EventArgs e)
        {
            Close();
        }

        private void txtSearch_TextChanged(
            object sender,
            EventArgs e)
        {
            string keyword =
                txtSearch.Text.Trim().ToLower();

            var result =
                _notifications
                .Where(x =>
                    x.Title.ToLower().Contains(keyword)
                    ||
                    x.Message.ToLower().Contains(keyword))
                .ToList();

            dgvNotifications.DataSource = null;
            dgvNotifications.DataSource = result;
        }

        private void dgvNotifications_CellFormatting(
            object? sender,
            DataGridViewCellFormattingEventArgs e)
        {
            if (dgvNotifications.Rows[e.RowIndex].DataBoundItem
                is not NotificationDto notification)
                return;

            DataGridViewRow row =
                dgvNotifications.Rows[e.RowIndex];

            switch (notification.Type)
            {
                case "🎂 Sinh nhật":

                    row.DefaultCellStyle.BackColor =
                        Color.LightGreen;

                    break;

                case "⚠ Sắp kết thúc":

                    row.DefaultCellStyle.BackColor =
                        Color.Khaki;

                    break;

                case "❌ Quá hạn":

                    row.DefaultCellStyle.BackColor =
                        Color.LightCoral;

                    break;
            }
        }
    }
}
