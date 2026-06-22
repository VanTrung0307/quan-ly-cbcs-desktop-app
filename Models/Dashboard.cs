namespace QuanLyCBCS.Models
{
    public class Dashboard
    {
        // CBCS
        public int TotalPersonnel { get; set; }
        public int SoldierCount { get; set; }
        public int OfficerCount { get; set; }

        // User
        public int TotalUsers { get; set; }
        public int AdminCount { get; set; }
        public int UserCount { get; set; }
        public int LockedUsers { get; set; }

        // Mission
        public int TotalMission { get; set; }
        public int ActiveMission { get; set; }
        public int EndingMission { get; set; }
        public int FinishedMission { get; set; }

        // Security
        public int LoginToday { get; set; }
        public int FailedLoginToday { get; set; }
        public int LockToday { get; set; }
        public int ResetToday { get; set; }
    }
}