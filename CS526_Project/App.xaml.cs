using CS526_Project.Model;
using Plugin.LocalNotification;

namespace CS526_Project;

public partial class App : Application
{
    public static DatabaseHelper Database { get; } = new DatabaseHelper();
    public static Settings Setting { get; } = new Settings();

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

        if (Database.FindCategory(0) == null)
        {
            var cat_important = new Category() { Id = 0, Name = "Quan trọng", Color_Hex = Colors.Red.ToHex() };
            if (!Setting.IsVietnamese) cat_important.Name = "Important";

            Database.AddCategory(cat_important);
        }

        mainPage = new();
        MainPage = new NavigationPage(mainPage);
    }


}
