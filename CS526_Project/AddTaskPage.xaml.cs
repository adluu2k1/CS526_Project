namespace CS526_Project;

public partial class AddTaskPage : ContentPage
{
    List<DatePicker> listNotiDatePickers = new List<DatePicker>();
    List<TimePicker> listNotiTimePickers = new List<TimePicker>();
    List<Layout> listNotiEntryWrapper = new List<Layout>();
    List<object> listBtnRemoveNoti = new List<object>();
    public AddTaskPage()
    {
        InitializeComponent();

        create_NotiControls();
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
        var btnRemoveNotiTime = new ImageButton() { Style = (Style)this.Resources["RemoveNotiButtonStyle"] };
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
        listNotiEntryWrapper.Add(NotiTimeEntryWrapper);
        listBtnRemoveNoti.Add(btnRemoveNotiTime);
    }

    private void create_btnAddNotiEntry()
    {
        var btnAddNotiTime = new ImageButton() { Style = (Style)this.Resources["AddNotiButtonStyle"] };
        btnAddNotiTime.Clicked += btnAddNotiTime_Clicked;
        NotiTimeWrapper.Children.Add(btnAddNotiTime);
    }

    private void remove_NotiTimeEntry(int i)
    {
        listNotiDatePickers.RemoveAt(i);
        listNotiTimePickers.RemoveAt(i);
        listNotiEntryWrapper.RemoveAt(i);
        listBtnRemoveNoti.RemoveAt(i);

        NotiTimeWrapper.Children.RemoveAt(i);
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

    }
}