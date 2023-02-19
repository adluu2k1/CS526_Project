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

        if (App.Setting.IsDarkMode)
        {
            pickerTheme.SelectedIndex = 1;
        }
        else
        {
            pickerTheme.SelectedIndex = 0;
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
	}

    private void pickerLanguage_SelectedIndexChanged(object sender, EventArgs e)
    {
		App.Setting.IsVietnamese = (pickerLanguage.SelectedItem as string != "English");
        App.SaveSettings();
		ApplyLanguage();
    }

    private void pickerTheme_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    private void switchRemind_Toggled(object sender, ToggledEventArgs e)
    {

    }
}