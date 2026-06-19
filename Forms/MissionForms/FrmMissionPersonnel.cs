using QuanLyCBCS.Models;
using QuanLyCBCS.Services;

namespace QuanLyCBCS.Forms.MissionForms
{
    public partial class FrmMissionPersonnel : Form
    {
        private readonly int _missionId;

        private readonly MissionPersonnelService
            _missionPersonnelService =
                new();

        private readonly PersonnelService
            _personnelService =
                new();
        public FrmMissionPersonnel(
            int missionId,
            string missionName)
        {
            InitializeComponent();

            _missionId = missionId;

            lblMissionName.Text =
                $"Công tác: {missionName}";
        }
    }
}
