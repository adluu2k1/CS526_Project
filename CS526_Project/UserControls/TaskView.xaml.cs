using CS526_Project.Model;

namespace CS526_Project.UserControls;

public partial class TaskView : ContentView
{
	private Layout parent;
	public TaskView(ToDo_Task task, Layout parent)
	{
		InitializeComponent();
		this.parent = parent;

		labelName.Text = task.Name;
		labelDeadline.Text = task.DeadlineTime.ToString();
		labelDescription.Text = task.Description;

		if (parent.Count % 3 == 0)
		{
			ViewBackground.BackgroundColor = App.PrimaryColor;
		}
		else if (parent.Count % 3 == 1)
		{
			ViewBackground.BackgroundColor = App.SecondaryColor;
		}
		else
		{
			ViewBackground.BackgroundColor = App.TertiaryColor;
		}

		if (task.IsDone)
		{
			checkTaskDone.IsChecked = true;
		}
	}

    private void checkTaskDone_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
		if (e.Value == true)
		{
			TaskDetailView.Opacity = 0.3;
		}
		else
		{
			TaskDetailView.Opacity = 1;
		}
    }
}