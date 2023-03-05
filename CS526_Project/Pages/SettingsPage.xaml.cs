using CommunityToolkit.Maui.Storage;
using Plugin.LocalNotification;
using System.Diagnostics;

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

        if (App.Setting.BackupFolderPath != String.Empty)
        {
            txtBackupFolderPath.Text = App.Setting.BackupFolderPath;
            txtBackupFolderPath.IsVisible = true;
            labelFolderNotSetted.IsVisible = false;
        }
        txtBackupFileName.Text = App.Setting.BackupFileName;
	}

	private void ApplyLanguage()
	{
		if (App.Setting.IsVietnamese)
		{
            labelLanguage.Text = "Ngôn ngữ";
            labelTheme.Text = "Chủ đề";
            labelRemind.Text = "Nhắc nhở tôi lập kế hoạch cho ngày hôm sau";
            labelSettings.Text = "CÀI ĐẶT";
            labelBackupNow.Text = "Sao lưu và khôi phục dữ liệu";
            btnBackupNow.Text = "Sao lưu ngay";
            btnRestoreNow.Text = "Khôi phục ngay";
            labelBackupFolderPath.Text = "Đường dẫn đến thư mục sao lưu:";
            labelFolderNotSetted.Text = "Chưa thiết lập";
            btnChangeFolderPath.Text = btnChangeFileName.Text = "Thay đổi";
            labelBackupFileName.Text = "Tên file sao lưu:";
            labelAutoBackup.Text = "Tự động sao lưu dữ liệu";

            pickerTheme.ItemsSource = listTheme_vi;
            pickerLanguage.SelectedIndex = 0;
        }
        else
        {
            labelLanguage.Text = "Language";
            labelTheme.Text = "Theme";
            labelRemind.Text = "Remind me to plan my next day";
            labelSettings.Text = "SETTINGS";
            labelBackupNow.Text = "Backup and restore data";
            btnBackupNow.Text = "Backup now";
            btnRestoreNow.Text = "Restore now";
            labelBackupFolderPath.Text = "Path to backup folder:";
            labelFolderNotSetted.Text = "Not setted";
            btnChangeFolderPath.Text = btnChangeFileName.Text = "Change";
            labelBackupFileName.Text = "Backup file name:";
            labelAutoBackup.Text = "Auto backup data";

            pickerTheme.ItemsSource = listTheme_en;
            pickerLanguage.SelectedIndex = 1;
        }

        pickerTheme.SelectedIndex = App.Setting.IsDarkMode ? 1 : 0;
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

        if (App.Setting.IsAutoBackupEnabled)
        {
            try { App.BackupData(Path.Combine(App.Setting.BackupFolderPath, App.Setting.BackupFileName)); }
            catch (Exception) { }
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
        if (App.Setting.IsAutoBackupEnabled)
        {
            try { App.BackupData(Path.Combine(App.Setting.BackupFolderPath, App.Setting.BackupFileName)); }
            catch (Exception) { }
        }

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
        if (App.Setting.IsAutoBackupEnabled)
        {
            try { App.BackupData(Path.Combine(App.Setting.BackupFolderPath, App.Setting.BackupFileName)); }
            catch (Exception) { }
        }
    }

    private async void btnBackupNow_Clicked(object sender, EventArgs e)
    {
        if (App.Setting.BackupFolderPath == String.Empty)
        {
            string message = App.Setting.IsVietnamese ? "Vui lòng chọn đường dẫn đến thư mục sao lưu trước khi thực hiện sao lưu" :
                                                        "Please set the path to the backup folder before doing backup";
            await DisplayAlert(App.Setting.IsVietnamese ? "Lỗi" : "Error", message, "OK");
            return;
        }

        try
        {
            App.BackupData(Path.Combine(App.Setting.BackupFolderPath, App.Setting.BackupFileName));
            string message = App.Setting.IsVietnamese ? "Đường dẫn đến file sao lưu:\n" : "Path to backup file:\n";
            message += Path.Combine(App.Setting.BackupFolderPath, App.Setting.BackupFileName);
            await DisplayAlert(App.Setting.IsVietnamese ? "Sao lưu thành công" : "Backup successful", message, "OK");
        }
#if DEBUG
        catch (Exception ex)
        {
            _ = DisplayAlert("Error", ex.Message, "OK");
            if (Debugger.IsAttached)
            {
                Debug.Print(ex.ToString());
            }
        }
#else
        catch (Exception)
        {
            string message = App.Setting.IsVietnamese ?
                "Đã có lỗi xảy ra trong quá trình sao lưu dữ liệu. Vui lòng thử lại sau." :
                "An error has occurred while doing data backup. Please try again later.";
            await DisplayAlert(App.Setting.IsVietnamese ? "Không thể sao lưu dữ liệu" : "Cannot backup data", message, "OK");
        }
#endif
    }

    private async void btnRestoreNow_Clicked(object sender, EventArgs e)
    {
        if (App.Setting.BackupFolderPath == String.Empty)
        {
            string message = App.Setting.IsVietnamese ? "Vui lòng chọn đường dẫn đến thư mục sao lưu trước khi thực hiện khôi phục" :
                                                        "Please set the path to the backup folder before doing restoration";
            await DisplayAlert(App.Setting.IsVietnamese ? "Lỗi" : "Error", message, "OK");
            return;
        }

        try
        {
            App.RestoreData(Path.Combine(App.Setting.BackupFolderPath, App.Setting.BackupFileName));
            await DisplayAlert(App.Setting.IsVietnamese ? "Khôi phục thành công" : "Restoration successful", String.Empty, "OK");
            await ReloadPage();
        }
#if DEBUG
        catch (Exception ex)
        {
            _ = DisplayAlert("Error", ex.Message, "OK");
            if (Debugger.IsAttached)
            {
                Debug.Print(ex.ToString());
            }
        }
#else
        catch (Exception)
        {
            string message = App.Setting.IsVietnamese ? 
                "File sao lưu mà bạn đã thiết lập đường dẫn có thể không tồn tại hoặc đã bị hỏng." : 
                "The file of which path you setted may be corrupted or not existed.";
            await DisplayAlert(App.Setting.IsVietnamese ? "Không thể khôi phục dữ liệu" : "Cannot restore data", message, "OK");
        }
#endif
    }

    private async void btnChangeFolderPath_Clicked(object sender, EventArgs e)
    {
        var result = await FolderPicker.Default.PickAsync(new CancellationToken());
        if (result.IsSuccessful)
        {
            txtBackupFolderPath.Text = result.Folder.Path;
            txtBackupFolderPath.IsVisible = true;
            labelFolderNotSetted.IsVisible = false;
            App.Setting.BackupFolderPath = result.Folder.Path;
            App.SaveSettings();
            if (App.Setting.IsAutoBackupEnabled)
            {
                try { App.BackupData(Path.Combine(App.Setting.BackupFolderPath, App.Setting.BackupFileName)); }
                catch (Exception) { }
            }
        }
    }

    private async void btnChangeFileName_Clicked(object sender, EventArgs e)
    {
        string prompt_title = App.Setting.IsVietnamese ? "Đổi tên file sao lưu" : "Change backup file name";
        string prompt_message = App.Setting.IsVietnamese ? "Tên file kết thúc bằng đuôi mở rộng .zip" : "File name ends with .zip extension";
        string result = await DisplayPromptAsync(prompt_title, prompt_message, placeholder: "ToDo_Backup.zip", 
                                                initialValue: App.Setting.BackupFileName);

        if (result == null) return;
        if (result == String.Empty) result = "ToDo_Backup.zip";
        if (!result.EndsWith(".zip")) result += ".zip";

        try
        {
            var temp_path = Path.Combine(FileSystem.CacheDirectory, result);
            File.Create(temp_path);
            if (File.Exists(temp_path)) File.Delete(temp_path);
        }
#if DEBUG
        catch (Exception ex)
        {
            _ = DisplayAlert("Error", ex.Message, "OK");
            if (Debugger.IsAttached)
            {
                Debug.Print(ex.ToString());
            }
        }
#else
        catch (Exception)
        {
            if (App.Setting.IsVietnamese) await DisplayAlert("Lỗi", "Tên file sao lưu không hợp lệ", "OK");
            else await DisplayAlert("Error", "Backup file name is invalid", "OK");
            return;
        }
#endif

        App.Setting.BackupFileName = result;
        App.SaveSettings();
        if (App.Setting.IsAutoBackupEnabled)
        {
            try { App.BackupData(Path.Combine(App.Setting.BackupFolderPath, App.Setting.BackupFileName)); }
            catch (Exception) { }
        }

        txtBackupFileName.Text = result;
    }

    private void switchAutoBackup_Toggled(object sender, ToggledEventArgs e)
    {
        if (App.Setting.IsAutoBackupEnabled == (switchAutoBackup.IsToggled == true))
            return;

        App.Setting.IsAutoBackupEnabled = e.Value;
    }
}