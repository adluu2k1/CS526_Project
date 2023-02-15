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
		var ListAllTask = App.Database.GetAllTask();

		// TODO: filter by Due Date (Deadlinetime)
		foreach (var task in ListAllTask) 
		{
			if (DateTime.Compare(DateTime.Now, task.DeadlineTime) <=0);
			{
                TaskViewWrapper.Add(new TaskView(task, TaskViewWrapper));
            }
        }
		//foreach (var task in ListAllTask)
		//{
		//	TaskViewWrapper.Add(new TaskView(task, TaskViewWrapper));
		//}
	}

    private void btnSearch_Clicked(object sender, EventArgs e)
    {

    }

    private async void btnAdd_Clicked(object sender, EventArgs e)
    {
		await Navigation.PushAsync(new AddTaskPage());
    }

    private void btnSettings_Clicked(object sender, EventArgs e)
    {

    }
}

