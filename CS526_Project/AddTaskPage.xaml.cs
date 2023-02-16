using CS526_Project.Model;
using System.Text.Json;

namespace CS526_Project;

public partial class AddTaskPage : ContentPage
{
    List<DatePicker> listNotiDatePickers = new List<DatePicker>();
    List<TimePicker> listNotiTimePickers = new List<TimePicker>();
    List<object> listBtnRemoveNoti = new List<object>();

    List<string> listCategoriesNameInDb = new List<string>();
    List<object> listBtnRemoveCategory = new List<object>();

    public AddTaskPage()
    {
        foreach (var category in App.Database.GetAllCategories())
        {
            listCategoriesNameInDb.Add(category.Name);
        }
        listCategoriesNameInDb.Add("Thêm nhãn");

        InitializeComponent();

        create_NotiControls();
        create_btnAddCategory();
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
            Text = "NGÀY"
        };
        var dateNoti = new DatePicker()
        {
            Format = "d",
            Margin = new Thickness(0, 0, 30, 0)
        };
        var labelTime = new Label()
        {
            Style = (Style)this.Resources["DateTimeLabelStyle"],
            Text = "GIỜ"
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
            ItemsSource = listCategoriesNameInDb,
            Title = "Select a category",
            Margin = new Thickness(0, 0, 30, 0)
        };
        var btnRemoveCategory = new ImageButton() { Style = (Style)this.Resources["RemoveButtonStyle"] };
        btnRemoveCategory.Clicked += btnRemoveCategory_Clicked;

        var CategoryEntryWrapper = new HorizontalStackLayout();
        CategoryEntryWrapper.Children.Add(CategoryPicker);
        CategoryEntryWrapper.Children.Add(btnRemoveCategory);

        CategoriesWrapper.Children.Add(CategoryEntryWrapper);

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
        listBtnRemoveCategory.RemoveAt(i);

        CategoriesWrapper.Children.RemoveAt(i);
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

    private void btnAddTask_Clicked(object sender, EventArgs e)
    {
        if (!IsNameValid())
        {
            txtName.Text = string.Empty;
            txtName.Placeholder = "* Ô này không thể để trống";
            txtName.PlaceholderColor = Colors.Red;
            return;
        }

        var task = new ToDo_Task()
        {
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

        List<DateTime> list_NotiTime = new List<DateTime>();
        for (int i = 0; i < listNotiDatePickers.Count; i++)
        {
            var date = listNotiDatePickers[i].Date;
            var time = listNotiTimePickers[i].Time;
            list_NotiTime.Add(new DateTime(date.Year, date.Month, date.Day, time.Hours, time.Minutes, time.Seconds));
        }
        if (list_NotiTime.Count != 0)
        {
            task.str_NotificationTime = JsonSerializer.Serialize(list_NotiTime, typeof(List<DateTime>));
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
}