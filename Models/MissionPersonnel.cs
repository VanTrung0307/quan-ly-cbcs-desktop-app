namespace QuanLyCBCS.Models
{
    public class MissionPersonnel
    {
        public int MissionPersonnelId
        {
            get;
            set;
        }

        public int MissionId
        {
            get;
            set;
        }

        public int PersonnelId
        {
            get;
            set;
        }

        public DateTime AssignedAt
        {
            get;
            set;
        }

        public virtual Mission? Mission
        {
            get;
            set;
        }

        public virtual Personnel? Personnel
        {
            get;
            set;
        }
    }
}