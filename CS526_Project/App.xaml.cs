using CS526_Project.Model;
using Plugin.LocalNotification;
using System.Globalization;
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

        if (!File.Exists(FileSystem.AppDataDirectory + "/settings.json"))
        {
            Setting = new Settings();
            SaveSettings();
        }
        else
        {
            ImportSettings();
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
        string json_txt = File.ReadAllText(FileSystem.AppDataDirectory + "/settings.json");
        Setting = JsonSerializer.Deserialize(json_txt, typeof(Settings)) as Settings;
    }

    public static void SaveSettings()
    {
        string json_txt = JsonSerializer.Serialize(Setting, typeof(Settings));
        File.WriteAllText(FileSystem.AppDataDirectory + "/settings.json", json_txt);
    }
}
