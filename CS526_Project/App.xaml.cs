using CS526_Project.Model;
using System.Text.Json;
using Plugin.LocalNotification;
using static Android.Provider.ContactsContract.CommonDataKinds;

namespace CS526_Project;

public partial class App : Application
{
    public static DatabaseHelper Database { get; } = new DatabaseHelper();

    public static MainPage mainPage;
    public static DateTime mainPage_SelectedDate = DateTime.Now.Date;

    public static Color PrimaryColor { get; private set; }
    public static Color SecondaryColor { get; private set; }
    public static Color TertiaryColor { get; private set; }

	public static List<ToDo_Task> ToDo_List = new List<ToDo_Task>();
	public List<ToDo_Task> ToDo_List = new List<ToDo_Task>();
    public List<NotificationRequest> Notifications= new List<NotificationRequest>();
	public App()
	{
		InitializeComponent();

        var rd_colors = App.Current.Resources.MergedDictionaries.First();
        PrimaryColor = rd_colors["Primary"] as Color;
        SecondaryColor = rd_colors["Secondary"] as Color;
        TertiaryColor = rd_colors["Tertiary"] as Color;

        mainPage = new();
        MainPage = new NavigationPage(mainPage);
        
	}



}
