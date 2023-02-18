using CS526_Project.UserControls;

namespace CS526_Project;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();

		var today = DateTime.Now.Date;
		labelToday.Text = $"NGÀY {today.Day} THÁNG {today.Month} NĂM {today.Year}";

		WeekViewWrapper.Content = new WeekView(today);

		ShowTask(today);
	}

	public void ShowTask(DateTime date)
	{
		var ListAllTask = App.Database.GetAllTask().OrderBy(p => p.DeadlineTime).ToList();
		foreach (var task in ListAllTask) 
		{
			if (task.DeadlineTime == DateTime.MaxValue && task.IsDone)
				break;
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

    private async void btnSearch_Clicked(object sender, EventArgs e)
    {
		await Navigation.PushAsync(new SearchPage());
    }

    private async void btnAdd_Clicked(object sender, EventArgs e)
    {
		await Navigation.PushAsync(new AddTaskPage());
    }

    private void btnSettings_Clicked(object sender, EventArgs e)
    {

    }


}
