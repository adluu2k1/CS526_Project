using Plugin.LocalNotification;

namespace CS526_Project;

public partial class SettingsPage : ContentPage
{
    List<string> list_lang = new List<string>() { "Tiếng Việt", "English" };
    List<string> listTheme_en = new List<string>() { "Light", "Dark" };
    List<string> listTheme_vi = new List<string>() { "Sáng", "Tối" };

    public SettingsPage()
	{
		InitializeComponent();

		pickerLanguage.ItemsSource = list_lang;

        ApplyLanguage();

        if (App.Setting.IsReminderForNextDayEnabled)
        {
            switchRemind.IsToggled = true;
        }
	}

	private void ApplyLanguage()
	{
		if (App.Setting.IsVietnamese)
		{
            labelLanguage.Text = "Ngôn ngữ";
            labelTheme.Text = "Chủ đề";
            labelRemind.Text = "Nhắc nhở tôi lập kế hoạch cho ngày hôm sau";
            labelSettings.Text = "CÀI ĐẶT";

            pickerTheme.ItemsSource = listTheme_vi;
            pickerLanguage.SelectedIndex = 0;
        }
        else
        {
            labelLanguage.Text = "Language";
            labelTheme.Text = "Theme";
            labelRemind.Text = "Remind me to plan my next day";
            labelSettings.Text = "SETTINGS";

            pickerTheme.ItemsSource = listTheme_en;
            pickerLanguage.SelectedIndex = 1;
        }

        if (App.Setting.IsDarkMode)
        {
            pickerTheme.SelectedIndex = 1;
        }
        else
        {
            pickerTheme.SelectedIndex = 0;
        }
    }

    private async Task ReloadPage()
    {
        App.mainPage.Navigation.InsertPageBefore(new SettingsPage(), this);
        await App.mainPage.Navigation.PopAsync();
    }

    private void pickerLanguage_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (App.Setting.IsVietnamese == (pickerLanguage.SelectedItem as string != "English"))
            return;

		App.Setting.IsVietnamese = (pickerLanguage.SelectedItem as string != "English");
        App.SaveSettings();
        
		ApplyLanguage();

        if (switchRemind.IsToggled == true)
        {
            LocalNotificationCenter.Current.Cancel(0);
            _ = App.RegisterDailyReminder();
        }

        var old_mainPage = App.mainPage;
        App.mainPage_SelectedDate = DateTime.Now;
        App.mainPage = new MainPage();
        old_mainPage.Navigation.InsertPageBefore(App.mainPage, old_mainPage.Navigation.NavigationStack[1]);
        old_mainPage.Navigation.RemovePage(old_mainPage);
        
    }

    private void pickerTheme_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (App.Setting.IsDarkMode == (pickerTheme.SelectedIndex == 1))
            return;

        if (pickerTheme.SelectedIndex == 0)
        {
            App.Current.UserAppTheme = AppTheme.Light;
            App.Setting.IsDarkMode = false;
        }
        else
        {
            App.Current.UserAppTheme = AppTheme.Dark;
            App.Setting.IsDarkMode = true;
        }
        App.SaveSettings();
        _ = ReloadPage();
    }

    private void switchRemind_Toggled(object sender, ToggledEventArgs e)
    {
        if (App.Setting.IsReminderForNextDayEnabled == (switchRemind.IsToggled == true))
            return;

        if (e.Value == true)
        {
            App.Setting.IsReminderForNextDayEnabled = true;
            _ = App.RegisterDailyReminder();
        }
        else
        {
            LocalNotificationCenter.Current.Cancel(0);
            App.Setting.IsReminderForNextDayEnabled = false;
        }
        App.SaveSettings();
    }
}