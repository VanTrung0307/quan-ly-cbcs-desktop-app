using QuanLyCBCS.Enums;
using QuanLyCBCS.Models;
using QuanLyCBCS.Services;
using QuanLyCBCS.Helpers;

namespace QuanLyCBCS.Forms.Personnel
{
    public partial class FrmPersonnelDetail : Form
    {
        private readonly PersonnelService _service;

        private readonly FormMode _mode;

        private readonly int _personnelId;
        public FrmPersonnelDetail(
            FormMode mode,
            int personnelId = 0)
        {
            InitializeComponent();

            _service = new PersonnelService();

            _mode = mode;

            _personnelId = personnelId;
        }

        

        private void chkDangVien_CheckedChanged(
            object sender,
            EventArgs e)
        {
            dtDangDuBi.Enabled =
                chkDangVien.Checked;

            dtDangChinhThuc.Enabled =
                chkDangVien.Checked;
        }

        
    }
}
