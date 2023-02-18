using CS526_Project.Model;
using Plugin.LocalNotification;
using System.Text.Json;
using System.Threading.Tasks;

namespace CS526_Project.UserControls;

public partial class EditTaskPage : ContentPage
{
    int taskId;

    List<DatePicker> listNotiDatePickers = new List<DatePicker>();
    List<TimePicker> listNotiTimePickers = new List<TimePicker>();
    List<object> listBtnRemoveNoti = new List<object>();

    List<HorizontalStackLayout> listCategoryEntryWrapper = new List<HorizontalStackLayout>();
    List<string> listCategoriesName = new List<string>();
    List<object> listBtnRemoveCategory = new List<object>();

    public EditTaskPage(ToDo_Task task)
	{
		InitializeComponent();
        taskId = task.id;

        foreach (var category in App.Database.GetAllCategories())
        {
            listCategoriesName.Add(category.Name);
        }
        listCategoriesName.Add(App.Setting.IsVietnamese ? "Thêm nhãn" : "Add tag");

        ImportTaskData(task);
        create_btnAddNotiEntry();
        create_btnAddCategory();

        if (!App.Setting.IsVietnamese)
        {
            labelTaskDetail.Text = "TASK DETAIL";
            labelTaskName.Text = "TASK NAME";
            txtName.Placeholder = "Task Name";
            labelDeadline.Text = "DEADLINE";
            labelNoDeadline.Text = "NO DEADLINE";
            labelDay.Text = "DAY";
            labelHour.Text = "HOUR";
            labelRemind.Text = "REMIND";
            labelLabel.Text = "LABEL";
            labelDescription.Text = "DESCRIPTON";
            txtDescription.Placeholder = "Description";
            btnSaveTask.Text = "SAVE CHANGES";
            btnDeleteTask.Text = "DELETE";
        }
    }

    private void ImportTaskData(ToDo_Task task)
    {
        txtName.Text = task.Name;
        txtDescription.Text = task.Description;

        if (task.DeadlineTime == DateTime.MaxValue)
        {
            checkNoDeadline.IsChecked = true;
        }
        else
        {
            dateDeadline.Date = task.DeadlineTime.Date;
            timeDeadline.Time = new TimeSpan(task.DeadlineTime.Hour, task.DeadlineTime.Minute, task.DeadlineTime.Second);
        }

        for (int i = 0; i < task.CategoryId.Count; i++)
        {
            create_Category_entry();

            var picker = listCategoryEntryWrapper[i].Children[0] as Picker;
            Category category = App.Database.FindCategory(task.CategoryId[i]);
            picker.SelectedItem = category.Name;
            picker.TextColor = Color.FromArgb(category.Color_Hex);
        }

        for (int i = 0; i < task.NotificationId.Count; i++)
        {
            create_NotiTime_entry();

            var dateTimeNoti = App.Database.FindNotification(task.NotificationId[i]).time;
            listNotiDatePickers[i].Date = dateTimeNoti.Date;
            listNotiTimePickers[i].Time = new TimeSpan
                (
                    dateTimeNoti.Hour,
                    dateTimeNoti.Minute,
                    dateTimeNoti.Second
                );
        }
    }

    private void create_NotiControls()
    {
        create_NotiTime_entry();
        create_btnAddNotiEntry();
    }
    private void create_NotiTime_entry()
    {
        var labelDate = new Label()
        {
            Style = (Style)this.Resources["DateTimeLabelStyle"],
            Text = App.Setting.IsVietnamese ? "NGÀY" : "DAY"
        };
        var dateNoti = new DatePicker()
        {
            Format = "d",
            Margin = new Thickness(0, 0, 30, 0)
        };
        var labelTime = new Label()
        {
            Style = (Style)this.Resources["DateTimeLabelStyle"],
            Text = App.Setting.IsVietnamese ? "GIỜ" : "HOUR"
        };
        var timeNoti = new TimePicker()
        {
            Format = "t",
            Margin = new Thickness(0, 0, 30, 0)
        };
        var btnRemoveNotiTime = new ImageButton() { Style = (Style)this.Resources["RemoveButtonStyle"] };
        btnRemoveNotiTime.Clicked += btnRemoveNotiTime_Clicked;

        var NotiTimeEntryWrapper = new HorizontalStackLayout();
        NotiTimeEntryWrapper.Children.Add(labelDate);
        NotiTimeEntryWrapper.Children.Add(dateNoti);
        NotiTimeEntryWrapper.Children.Add(labelTime);
        NotiTimeEntryWrapper.Children.Add(timeNoti);
        NotiTimeEntryWrapper.Children.Add(btnRemoveNotiTime);

        NotiTimeWrapper.Children.Add(NotiTimeEntryWrapper);

        listNotiDatePickers.Add(dateNoti);
        listNotiTimePickers.Add(timeNoti);
        listBtnRemoveNoti.Add(btnRemoveNotiTime);
    }
    private void create_btnAddNotiEntry()
    {
        var btnAddNotiTime = new ImageButton() { Style = (Style)this.Resources["AddButtonStyle"] };
        btnAddNotiTime.Clicked += btnAddNotiTime_Clicked;
        NotiTimeWrapper.Children.Add(btnAddNotiTime);
    }
    private void remove_NotiTimeEntry(int i)
    {
        listNotiDatePickers.RemoveAt(i);
        listNotiTimePickers.RemoveAt(i);
        listBtnRemoveNoti.RemoveAt(i);

        NotiTimeWrapper.Children.RemoveAt(i);
    }

    private void create_CategoryControls()
    {
        create_Category_entry();
        create_btnAddCategory();
    }
    private void create_Category_entry()
    {
        var CategoryPicker = new Picker
        {
            ItemsSource = listCategoriesName,
            Title = "Select a category",
            Margin = new Thickness(0, 0, 30, 0)
        };
        CategoryPicker.SelectedIndexChanged += OnSelectedIndexChanged;
        var btnRemoveCategory = new ImageButton() { Style = (Style)this.Resources["RemoveButtonStyle"] };
        btnRemoveCategory.Clicked += btnRemoveCategory_Clicked;

        var CategoryEntryWrapper = new HorizontalStackLayout();
        CategoryEntryWrapper.Children.Add(CategoryPicker);
        CategoryEntryWrapper.Children.Add(btnRemoveCategory);

        CategoriesWrapper.Children.Add(CategoryEntryWrapper);

        listCategoryEntryWrapper.Add(CategoryEntryWrapper);
        listBtnRemoveCategory.Add(btnRemoveCategory);
    }
    private void create_btnAddCategory()
    {
        var btnAddCategory = new ImageButton() { Style = (Style)this.Resources["AddButtonStyle"] };
        btnAddCategory.Clicked += btnAddCategory_Clicked;
        CategoriesWrapper.Children.Add(btnAddCategory);
    }
    private void remove_CategoryEntry(int i)
    {
        listCategoryEntryWrapper.RemoveAt(i);
        listBtnRemoveCategory.RemoveAt(i);

        CategoriesWrapper.Children.RemoveAt(i);
    }

    public void OnAddCategoryPageReturn(string category_name, Color category_color, int caller_IndexInWraper)
    {
        listCategoriesName[listCategoriesName.Count - 1] = category_name;
        listCategoriesName.Add(App.Setting.IsVietnamese ? "Thêm nhãn": "Add tag");

        for (int i = 0; i < listCategoryEntryWrapper.Count; i++)
        {
            if (i == caller_IndexInWraper)
            {
                listCategoryEntryWrapper[i].Children[0] = new Picker()
                {
                    ItemsSource = listCategoriesName,
                    Title = "Select a category",
                    Margin = new Thickness(0, 0, 30, 0),
                    SelectedIndex = listCategoriesName.Count - 2,
                    TextColor = category_color
                };
            }
            else
            {
                var picker = listCategoryEntryWrapper[i].Children[0] as Picker;
                int picker_selectedIndex = picker.SelectedIndex;
                Color picker_txtcolor = picker.TextColor;

                listCategoryEntryWrapper[i].Children[0] = new Picker()
                {
                    ItemsSource = listCategoriesName,
                    Title = "Select a category",
                    Margin = new Thickness(0, 0, 30, 0),
                    SelectedIndex = picker_selectedIndex,
                    TextColor = picker_txtcolor
                };
            }
        }
    }

    private void btnAddNotiTime_Clicked(object sender, EventArgs e)
    {
        NotiTimeWrapper.Children.RemoveAt(NotiTimeWrapper.Children.Count - 1);

        create_NotiControls();
    }

    private void btnRemoveNotiTime_Clicked(object sender, EventArgs e)
    {
        for (int i = 0; i < listBtnRemoveNoti.Count; i++)
        {
            if (listBtnRemoveNoti[i] == sender)
            {
                remove_NotiTimeEntry(i);
                return;
            }
        }
    }

    private void btnAddCategory_Clicked(object sender, EventArgs e)
    {
        CategoriesWrapper.Children.RemoveAt(CategoriesWrapper.Children.Count - 1);

        create_CategoryControls();
    }

    private void btnRemoveCategory_Clicked(object sender, EventArgs e)
    {
        for (int i = 0; i < listBtnRemoveCategory.Count; i++)
        {
            if (listBtnRemoveCategory[i] == sender)
            {
                remove_CategoryEntry(i);
                return;
            }
        }
    }

    private async void OnSelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = sender as Picker;

        if (picker.SelectedIndex == -1) return;
        if (picker.SelectedIndex == listCategoriesName.Count - 1)
        {
            var picker_index = -1;
            for (int i = 0; i < listCategoryEntryWrapper.Count; i++)
            {
                if (listCategoryEntryWrapper[i].Children[0] == picker)
                {
                    picker_index = i;
                    break;
                }
            }

            picker.SelectedIndex = -1;
            await Navigation.PushAsync(new AddCategoryPage(this, picker_index));
            return;
        }
        var picker_txtcolor = App.Database.GetAllCategories()[picker.SelectedIndex].Color_Hex;
        picker.TextColor = Color.FromArgb(picker_txtcolor);
    }

    private void checkNoDeadline_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (e.Value == true)
        {
            foreach (VisualElement element in DeadlineSettingWrapper.Children)
            {
                element.IsEnabled = false;
            }
        }
        else
        {
            foreach (VisualElement element in DeadlineSettingWrapper.Children)
            {
                element.IsEnabled = true;
            }
        }
    }

    private bool IsNameValid()
    {
        if (txtName.Text == null || txtName.Text.Replace(" ", "") == "")
        {
            return false;
        }

        return true;
    }

    private async void btnSaveTask_Clicked(object sender, EventArgs e)
    {
        // Check if task name is invalid
        if (!IsNameValid())
        {
            txtName.Text = string.Empty;
            txtName.Placeholder = App.Setting.IsVietnamese ? "* Ô này không thể để trống" : "* This box can't be blank";
            txtName.PlaceholderColor = Colors.Red;
            return;
        }

        var task = new ToDo_Task()
        {
            id = taskId,
            Name = txtName.Text,
            Description = txtDescription.Text,
            AddTime = DateTime.Now
        };

        if (!checkNoDeadline.IsChecked)
        {
            var date = dateDeadline.Date;
            var time = timeDeadline.Time;
            task.DeadlineTime = new DateTime(date.Year, date.Month, date.Day, time.Hours, time.Minutes, time.Seconds);
        }

        if (listNotiDatePickers.Count != 0)
        {
            List<int> notificationId = new List<int>();
            for (int i = 0; i < listNotiDatePickers.Count; i++)
            {
                var date = listNotiDatePickers[i].Date;
                var time = listNotiTimePickers[i].Time;
                var notification = new Notification()
                {
                    Id = App.Database.GenerateRandomNotificationId(),
                    time = new DateTime(date.Year, date.Month, date.Day, time.Hours, time.Minutes, time.Seconds)
                };
                App.Database.AddNotification(notification);
                notificationId.Add(notification.Id);
            }
            if (notificationId.Count != 0)
            {
                task.str_NotificationId = JsonSerializer.Serialize(notificationId, typeof(List<int>));
            }
        }

        if (listCategoryEntryWrapper.Count != 0)
        {
            List<int> categoryId = new List<int>();
            foreach (var wrapper in listCategoryEntryWrapper)
            {
                var picker = wrapper.Children[0] as Picker;
                if (!App.Database.IsCategoryNameTaken(picker.SelectedItem as string))
                {
                    App.Database.AddCategory(new Category()
                    {
                        Id = App.Database.GenerateRandomCategoryId(),
                        Name = picker.SelectedItem as string,
                        Color_Hex = picker.TextColor.ToHex()
                    });
                }
                var category = App.Database.FindCategory(picker.SelectedItem as string);
                if (category != null)
                {
                    categoryId.Add(category.Id);
                }
            }

            task.str_CategoryId = JsonSerializer.Serialize(categoryId, typeof(List<int>));
        }

        App.Database.UpdateTask(task);
        App.Database.DeleteObsoleteCategory();
        App.Database.DeleteObsoleteNotification();
        await RegisterAllNotification(task);

        for (int i = 0; i < Navigation.NavigationStack.Count - 1; i++)
        {
            var previous_page = Navigation.NavigationStack[i];
            if (previous_page.GetType() == typeof(MainPage))
            {
                App.mainPage.RefreshTaskViewWrapper();
            }
            else
            {
                SearchPage page = previous_page as SearchPage;
                page.txtSearch_TextChanged(null, null);
            }
        }

        await Navigation.PopAsync();
    }

    private async Task RegisterAllNotification(ToDo_Task task)
    {
        if (await LocalNotificationCenter.Current.AreNotificationsEnabled() == false)
        {
            await LocalNotificationCenter.Current.RequestNotificationPermission();
        }

        foreach (var notification in App.Database.GetAllNotifications(task))
        {
            var notireq = new NotificationRequest()
            {
                NotificationId = notification.Id,
                Title = task.Name,
                Schedule =
                {
                    NotifyTime = notification.time
                }
            };
            var time_remaining = task.DeadlineTime - notification.time;
            if (App.Setting.IsVietnamese)
            {
                if (time_remaining.TotalDays > 1) notireq.Description = $"Thời gian còn lại: {time_remaining.Days.ToString()} ngày";
                else notireq.Description = $"Thời gian còn lại: {time_remaining.ToString()}";
            }
            else
            {
                if (time_remaining.TotalDays > 1) notireq.Description = $"Time remaining: {time_remaining.Days.ToString()} days";
                else notireq.Description = $"Time remaining: {time_remaining.ToString()}";
            }

            await LocalNotificationCenter.Current.Show(notireq);
        }
    }

    private async void btnDeleteTask_Clicked(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert(App.Setting.IsVietnamese ? "Xóa nhiệm vụ?" : "Remove this task?", App.Setting.IsVietnamese ? "Bạn có muốn xóa nhiệm vụ này không?" : "Do you want to remove this task?", "Yes", "No");
        if (confirm)
        {
            App.Database.DeleteTask(App.Database.FindTask(taskId));
            App.Database.DeleteObsoleteCategory();
            for (int i = 0; i < Navigation.NavigationStack.Count - 1; i++)
            {
                var previous_page = Navigation.NavigationStack[i];
                if (previous_page.GetType() == typeof(MainPage))
                {
                    App.mainPage.RefreshTaskViewWrapper();
                }
                else
                {
                    SearchPage page = previous_page as SearchPage;
                    page.txtSearch_TextChanged(null, null);
                }
            }
            await Navigation.PopAsync();
        }
    }
}