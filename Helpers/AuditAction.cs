using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyCBCS.Helpers
{
    /// <summary>
    /// Danh sách Action chuẩn dùng cho Audit Log.
    /// Không sử dụng chuỗi ("Magic String") trực tiếp.
    /// </summary>
    public static class AuditAction
    {
        // ===========================
        // Authentication
        // ===========================

        public const string Login = "LOGIN";

        public const string Logout = "LOGOUT";

        public const string FailedLogin = "FAILED_LOGIN";

        public const string ChangePassword = "CHANGE_PASSWORD";

        public const string ResetPassword = "RESET_PASSWORD";

        public const string LockAccount = "LOCK_ACCOUNT";

        public const string AutoLogout = "AUTO_LOGOUT";


        // ===========================
        // Personnel
        // ===========================

        public const string AddPersonnel = "ADD_PERSONNEL";

        public const string UpdatePersonnel = "UPDATE_PERSONNEL";

        public const string DeletePersonnel = "DELETE_PERSONNEL";



        // ===========================
        // Mission
        // ===========================

        public const string AddMission = "ADD_MISSION";

        public const string UpdateMission = "UPDATE_MISSION";

        public const string DeleteMission = "DELETE_MISSION";

        public const string AssignMission = "ASSIGN_MISSION";

        public const string RemoveMissionAssignment = "REMOVE_MISSION_ASSIGNMENT";


        // ===========================
        // Permission
        // ===========================

        public const string UpdatePermission = "UPDATE_PERMISSION";



        // ===========================
        // User
        // ===========================

        public const string AddUser = "ADD_USER";

        public const string UpdateUser = "UPDATE_USER";

        public const string DeleteUser = "DELETE_USER";

        public const string UnlockAccount =
            "UNLOCK_ACCOUNT";

        public const string ProtectedAccount =
            "PROTECTED_ACCOUNT";

        // ===========================
        // Database
        // ===========================

        public const string BackupDatabase = "BACKUP_DATABASE";

        public const string RestoreDatabase = "RESTORE_DATABASE";



        // ===========================
        // Import / Export
        // ===========================

        public const string ExportExcel = "EXPORT_EXCEL";

        public const string ImportExcel = "IMPORT_EXCEL";



        // ===========================
        // Undo / Redo
        // ===========================

        public const string Undo = "UNDO";

        public const string Redo = "REDO";
    }
}
