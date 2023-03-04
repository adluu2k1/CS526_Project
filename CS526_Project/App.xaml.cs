using CS526_Project.Model;
using Plugin.LocalNotification;
using System.Diagnostics;
using System.Globalization;
using System.IO.Compression;
using System.Text.Json;

namespace CS526_Project;

public partial class App : Application
{
    public static DatabaseHelper Database { get; } = new DatabaseHelper();
    public static Settings Setting { get; private set; }

    public static MainPage mainPage;
    public static DateTime mainPage_SelectedDate = DateTime.Now.Date;

    public static Color PrimaryColor { get; private set; }
    public static Color SecondaryColor { get; private set; }
    public static Color TertiaryColor { get; private set; }
	public App()
	{
		InitializeComponent();

        var rd_colors = App.Current.Resources.MergedDictionaries.First();
        PrimaryColor = rd_colors["Primary"] as Color;
        SecondaryColor = rd_colors["Secondary"] as Color;
        TertiaryColor = rd_colors["Tertiary"] as Color;

        CultureInfo.CurrentCulture = new CultureInfo("vi-vn");
        CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("vi-vn");

        ImportSettings();

        if (Setting.IsDarkMode)
        {
            App.Current.UserAppTheme = AppTheme.Dark;
        }
        
        if (Database.FindCategory(0) == null)
        {
            var cat_important = new Category() { Id = 0, Name = "Quan trọng", Color_Hex = Colors.Red.ToHex() };
            if (!Setting.IsVietnamese) cat_important.Name = "Important";

            Database.AddCategory(cat_important);
        }

        mainPage = new();
        MainPage = new NavigationPage(mainPage);
    }

    public static void ImportSettings()
    {
        if (!File.Exists(FileSystem.AppDataDirectory + "/settings.json"))
        {
            Setting = new Settings();
            SaveSettings();
            return;
        }

        string json_txt = File.ReadAllText(FileSystem.AppDataDirectory + "/settings.json");
        Setting = JsonSerializer.Deserialize(json_txt, typeof(Settings)) as Settings;
    }

    public static void SaveSettings()
    {
        string json_txt = JsonSerializer.Serialize(Setting, typeof(Settings));
        File.WriteAllText(FileSystem.AppDataDirectory + "/settings.json", json_txt);

        if (Setting.IsAutoBackupEnabled)
        {
            try { BackupData(Path.Combine(App.Setting.BackupFolderPath, App.Setting.BackupFileName)); }
            catch (Exception) { }
        }
    }

    public static void BackupData(string targetPath)
    {
        string settingsPath = FileSystem.AppDataDirectory + "/settings.json";
        string dbPath = FileSystem.AppDataDirectory + "/database.db3";
        if (File.Exists(settingsPath) && File.Exists(dbPath))
        {
            if (File.Exists(targetPath))
                File.Delete(targetPath);

            using (var archive = ZipFile.Open(targetPath, ZipArchiveMode.Create))
            {
                archive.CreateEntryFromFile(settingsPath, "settings.json");
                archive.CreateEntryFromFile(dbPath, "database.db3");
            }
        }
    }

    public static void RestoreData(string zipPath)
    {
        bool IsZipFileValid = true;
        using (var archive = ZipFile.OpenRead(zipPath))
        {
            foreach (var entry in archive.Entries)
            {
                if (!String.Equals(entry.FullName, "settings.json") && !String.Equals(entry.FullName, "database.db3"))
                {
                    IsZipFileValid = false;
                    break;
                }
            }
            if (!IsZipFileValid)
                throw new Exception("zipPath zip is invalid");

            archive.ExtractToDirectory(FileSystem.AppDataDirectory, true);
            ImportSettings();
        }

        // Apply theme setting
        if (Setting.IsDarkMode) App.Current.UserAppTheme = AppTheme.Dark;
        else App.Current.UserAppTheme = AppTheme.Light;

        // Apply daily reminder setting
        if (Setting.IsReminderForNextDayEnabled)
        {
            _ = RegisterDailyReminder();
        }
        else
        {
            LocalNotificationCenter.Current.Cancel(0);
        }
    }

    public static async Task RegisterDailyReminder()
    {
        if (await LocalNotificationCenter.Current.AreNotificationsEnabled() == false)
        {
            await LocalNotificationCenter.Current.RequestNotificationPermission();
        }
        if (await LocalNotificationCenter.Current.AreNotificationsEnabled() == false)
        {
            Setting.IsReminderForNextDayEnabled = false;
            return;
        }

        var notireq = new NotificationRequest()
        {
            NotificationId = 0,
            Title = App.Setting.IsVietnamese ? "Đừng quên lập kế hoạch cho ngày mai nhé" : "Don't forget to plan for tomorrow",
            Description = App.Setting.IsVietnamese ? "Việc lập kế hoạch trước khi ngày mới bắt đầu sẽ giúp bạn chuẩn bị được tinh thần thép để hoàn thành tốt mọi việc" :
                                                    "Planning your next day before it begins will help you prepare a strong will to complete every tasks.",
            Schedule =
                {
                    NotifyTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 21, 0, 0),
                    RepeatType = NotificationRepeat.Daily
                }
        };
        
        await LocalNotificationCenter.Current.Show(notireq);
    }
}
