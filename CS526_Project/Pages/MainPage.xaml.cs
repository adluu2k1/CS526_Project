using CS526_Project.Pages;
using CS526_Project.UserControls;
using System.Globalization;

namespace CS526_Project;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();

		var today = DateTime.Now.Date;
		var selected_date = App.mainPage_SelectedDate;
		labelTodayDate.Text = App.Setting.IsVietnamese ? today.ToLongDateString() : ShowLongDateInEnglish(today);

		WeekViewWrapper.Content = new WeekView(selected_date);

		ShowTask(selected_date);

        if (!App.Setting.IsVietnamese)
		{
			labelToday.Text = "TODAY";
		}
		
    }

    public void MonthView_OnSelectDate(DateTime date)
    {
        TaskViewWrapper.Children.Clear();

        WeekViewWrapper.Content = new WeekView(date);
        ShowTask(date);
    }

    public void ShowTask(DateTime date)
	{
		var ListAllTask = App.Database.GetAllTask().OrderBy(p => p.DeadlineTime).ToList();
		foreach (var task in ListAllTask) 
		{
			if (task.DeadlineTime == DateTime.MaxValue && task.IsDone)
				continue;
			if (DateTime.Compare(date, task.DeadlineTime) <= 0)
			{
                TaskViewWrapper.Add(new TaskView(task, TaskViewWrapper));
            }
        }
	}

	public void RefreshTaskViewWrapper()
	{
		TaskViewWrapper.Children.Clear();
		ShowTask(App.mainPage_SelectedDate);
	}

	private string ShowLongDateInEnglish(DateTime date)
	{
        CultureInfo.CurrentCulture = new CultureInfo("en-us");
        string result = date.ToLongDateString();
        CultureInfo.CurrentCulture = new CultureInfo("vi-vn");
		return result;
    }

    private async void btnSearch_Clicked(object sender, EventArgs e)
    {
		await App.mainPage.Navigation.PushAsync(new SearchPage());
    }

    private async void btnAdd_Clicked(object sender, EventArgs e)
    {
		await App.mainPage.Navigation.PushAsync(new AddTaskPage());
    }

    private async void btnSettings_Clicked(object sender, EventArgs e)
    {
		await App.mainPage.Navigation.PushAsync(new SettingsPage());
    }

    private async void btnMonthView_Clicked(object sender, EventArgs e)
    {
        await App.mainPage.Navigation.PushAsync(new MonthViewPage());
    }
}
