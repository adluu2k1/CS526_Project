namespace CS526_Project
{
    public class Settings
    {
        public bool IsVietnamese { get; set; } = true;
        public bool IsDarkMode { get; set; } = App.Current.UserAppTheme == AppTheme.Dark;
        public bool IsReminderForNextDayEnabled { get; set; } = false;
        public string BackupFolderPath { get; set; } = String.Empty;
        public string BackupFileName { get; set; } = "ToDo_Backup.zip";
        public bool IsAutoBackupEnabled { get; set; } = false;
    }
}
