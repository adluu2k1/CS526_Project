using CS526_Project.Model;

namespace CS526_Project.UserControls;

public partial class TaskView : ContentView
{
	private Layout parent;
	private ToDo_Task task;
	public TaskView(ToDo_Task task, Layout parent)
	{
		InitializeComponent();
		this.task = task;
		this.parent = parent;

		labelName.Text = task.Name;
		labelDeadline.Text = task.DeadlineTime.ToString();
		labelDescription.Text = task.Description;

		if (task.IsDone)
		{
			checkTaskDone.IsChecked = true;
		}

		if (labelDescription.Text == String.Empty)
		{
			labelDescription.IsVisible = false;
		}

		if (task.DeadlineTime.Date == DateTime.Now.Date)
		{
			labelDeadline.TextColor = Colors.Red;
		}

		AddCategoriesToView();
	}

    private void checkTaskDone_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
		if (e.Value == true)
		{
			task.IsDone = true;
			task.EndTime = DateTime.Now;
			App.Database.UpdateTask(task);

			TaskDetailView.Opacity = 0.3;
		}
		else
		{
			task.IsDone = false;
			task.EndTime = DateTime.MaxValue;
            App.Database.UpdateTask(task);

            TaskDetailView.Opacity = 1;
		}
    }

	private void AddCategoriesToView()
	{
		foreach (var categoryId in task.CategoryId)
		{
			var category = App.Database.FindCategory(categoryId);
			if (category != null)
			{
				var labelCategory = new Label() { Text = category.Name, TextColor = Color.FromArgb(category.Color_Hex) };
				var labelSeparator = new Label() { Text = "|", Margin = new Thickness(10, 0) };

				CategoriesWrapper.Add(labelCategory);
				CategoriesWrapper.Add(labelSeparator);
			}
		}
		if (CategoriesWrapper.Children.Count > 0)
		{
			CategoriesWrapper.RemoveAt(CategoriesWrapper.Children.Count - 1);
		}
		if (CategoriesWrapper.Children.Count == 0)
		{
			CategoriesWrapper.IsVisible = false;
		}
	}
}