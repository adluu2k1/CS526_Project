using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS526_Project
{
    public class Settings
    {
        public bool IsVietnamese { get; set; } = true;
        public bool IsDarkMode { get; set; } = false;
        public bool IsReminderForNextDayEnabled { get; set; } = false;
        public string BackupFolderPath { get; set; } = String.Empty;
        public string BackupFileName { get; set; } = "ToDo_Backup.zip";
        public bool IsAutoBackupEnabled { get; set; } = false;
    }
}
