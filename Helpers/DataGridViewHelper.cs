using System.Reflection;

namespace QuanLyCBCS.Helpers
{
    public static class DataGridViewHelper
    {
        public static void ApplyModernStyle(
            DataGridView dgv)
        {
            dgv.BorderStyle =
                BorderStyle.None;

            dgv.BackgroundColor =
                Color.White;

            dgv.DefaultCellStyle.Font =
                new Font(
                    "Segoe UI",
                    10);

            dgv.ColumnHeadersDefaultCellStyle.Font =
                new Font(
                    "Segoe UI",
                    10,
                    FontStyle.Bold);

            dgv.ColumnHeadersDefaultCellStyle.BackColor =
                Color.FromArgb(
                    44,
                    62,
                    80);

            dgv.ColumnHeadersDefaultCellStyle.ForeColor =
                Color.White;

            dgv.EnableHeadersVisualStyles =
                false;

            dgv.ColumnHeadersHeight =
                40;

            dgv.RowTemplate.Height =
                40;

            dgv.DefaultCellStyle.Padding =
                new Padding(3);

            dgv.AlternatingRowsDefaultCellStyle.BackColor =
                Color.FromArgb(
                    248,
                    249,
                    250);

            dgv.AllowUserToAddRows =
                false;

            dgv.ReadOnly =
                true;

            dgv.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dgv.MultiSelect =
                false;

            dgv.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.None;

            dgv.ScrollBars =
                ScrollBars.Both;

            dgv.RowHeadersVisible = false;

            dgv.AllowUserToResizeRows = false;

            dgv.AllowUserToResizeColumns = false;
        }

        public static void ConfigureColumnHeaders(
            DataGridView dgv)
        {
            SetHeaderText(
                dgv,
                "SoHieu",
                "Số hiệu");

            SetHeaderText(
                dgv,
                "HoTen",
                "Họ tên");

            SetHeaderText(
                dgv,
                "DonVi",
                "Đơn vị");

            SetHeaderText(
                dgv,
                "GioiTinh",
                "Giới tính");

            SetHeaderText(
                dgv,
                "NgaySinh",
                "Ngày sinh");

            SetHeaderText(
                dgv,
                "CapBacHam",
                "Cấp bậc");

            SetHeaderText(
                dgv,
                "ChucVu",
                "Chức vụ");

            SetHeaderText(
                dgv,
                "QueQuanMoi",
                "Quê quán");

            SetHeaderText(
                dgv,
                "HKTTMoi",
                "Hộ khẩu thường trú");

            SetHeaderText(
                dgv,
                "CCCD",
                "CCCD");

            SetHeaderText(
                dgv,
                "SoDienThoai",
                "Số điện thoại");
        }

        public static void ConfigureColumnWidths(
            DataGridView dgv)
        {
            SetColumnWidth(
                dgv,
                "STT",
                60);

            SetColumnWidth(
                dgv,
                "SoHieu",
                120);

            SetColumnWidth(
                dgv,
                "HoTen",
                250);

            SetColumnWidth(
                dgv,
                "DonVi",
                220);

            SetColumnWidth(
                dgv,
                "GioiTinh",
                90);

            SetColumnWidth(
                dgv,
                "NgaySinh",
                120);

            SetColumnWidth(
                dgv,
                "CapBacHam",
                140);

            SetColumnWidth(
                dgv,
                "ChucVu",
                180);

            SetColumnWidth(
                dgv,
                "QueQuanMoi",
                160);

            SetColumnWidth(
                dgv,
                "HKTTMoi",
                220);

            SetColumnWidth(
                dgv,
                "CCCD",
                140);

            SetColumnWidth(
                dgv,
                "SoDienThoai",
                130);
        }

        public static void HideTechnicalColumns(
            DataGridView dgv)
        {
            string[] hiddenColumns =
            {
                "PersonnelID",
                "HoTenKhongDau",
                "PersonnelType",
                "CreatedAt",
                "UpdatedAt"
            };

            foreach (string column
                in hiddenColumns)
            {
                if (dgv.Columns[column] != null)
                {
                    dgv.Columns[column]
                        .Visible = false;
                }
            }
        }

        public static void ConfigureDateFormats(
            DataGridView dgv)
        {
            string[] fullDateColumns =
            {
                "NgaySinh",
                "NgayCapCCCD",
                "NgayNhanCapBac",
                "NgayBoNhiem",
                "NgayVaoDangDuBi",
                "NgayVaoDangChinhThuc"
            };

            foreach (string column
                in fullDateColumns)
            {
                SetDateFormat(
                    dgv,
                    column,
                    "dd/MM/yyyy");
            }

            string[] monthYearColumns =
            {
                "ThangNamVaoNganh",
                "ThangNamNhanCongTac",
                "ThangNamTotNghiep"
            };

            foreach (string column
                in monthYearColumns)
            {
                SetDateFormat(
                    dgv,
                    column,
                    "MM/yyyy");
            }
        }

        public static void ConfigureVisibleColumns(
            DataGridView dgv)
        {
            foreach (DataGridViewColumn column
                in dgv.Columns)
            {
                column.Visible = false;
            }

            string[] visibleColumns =
            {
                "STT",
                "SoHieu",
                "HoTen",
                "DonVi",
                "GioiTinh",
                "NgaySinh",
                "CapBacHam",
                "ChucVu",
                "QueQuanMoi",
                "HKTTMoi",
                "CCCD",
                "SoDienThoai"
            };

            foreach (string column
                in visibleColumns)
            {
                if (dgv.Columns[column] != null)
                {
                    dgv.Columns[column]
                        .Visible = true;
                }
            }
        }

        private static void SetHeaderText(
            DataGridView dgv,
            string columnName,
            string headerText)
        {
            if (dgv.Columns[columnName] != null)
            {
                dgv.Columns[columnName]
                    .HeaderText =
                    headerText;
            }
        }

        private static void SetColumnWidth(
            DataGridView dgv,
            string columnName,
            int width)
        {
            if (dgv.Columns[columnName] != null)
            {
                dgv.Columns[columnName]
                    .Width =
                    width;
            }
        }

        private static void SetDateFormat(
            DataGridView dgv,
            string columnName,
            string format)
        {
            if (dgv.Columns[columnName] != null)
            {
                dgv.Columns[columnName]
                    .DefaultCellStyle.Format =
                    format;
            }
        }

        public static void EnableDoubleBuffer(
            DataGridView dgv)
        {
            typeof(DataGridView)
                .GetProperty(
                    "DoubleBuffered",
                    BindingFlags.Instance |
                    BindingFlags.NonPublic)
                ?.SetValue(
                    dgv,
                    true,
                    null);
        }

        public static void SetupDefaultStyle(
            DataGridView dgv)
        {
            dgv.BorderStyle =
                BorderStyle.None;

            dgv.BackgroundColor =
                Color.White;

            dgv.EnableHeadersVisualStyles =
                false;

            dgv.ColumnHeadersDefaultCellStyle
                .BackColor =
                ThemeHelper.PrimaryColor;

            dgv.ColumnHeadersDefaultCellStyle
                .ForeColor =
                Color.White;

            dgv.ColumnHeadersDefaultCellStyle
                .Font =
                new Font(
                    "Segoe UI",
                    10,
                    FontStyle.Bold);

            dgv.ColumnHeadersHeight = 40;

            dgv.RowTemplate.Height = 35;

            dgv.SelectionMode =
                DataGridViewSelectionMode
                    .FullRowSelect;

            dgv.MultiSelect = false;

            dgv.ReadOnly = true;

            dgv.AllowUserToAddRows = false;

            dgv.AllowUserToDeleteRows = false;

            dgv.AutoSizeRowsMode =
                DataGridViewAutoSizeRowsMode.None;

            dgv.AlternatingRowsDefaultCellStyle
                .BackColor =
                Color.FromArgb(
                    248,
                    249,
                    250);
        }
    }
}