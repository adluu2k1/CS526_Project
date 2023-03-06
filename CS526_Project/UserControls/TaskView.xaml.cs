using CS526_Project.Model;
using System.Diagnostics;

namespace CS526_Project.UserControls;

public partial class TaskView : ContentView
{
	private ToDo_Task task;
	public TaskView(ToDo_Task task, Layout parent)
	{
		InitializeComponent();
		this.task = task;

		labelName.Text = task.Name;
		labelDeadline.Text = task.DeadlineTime.ToShortDateString() + "  " + task.DeadlineTime.ToShortTimeString();
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

		if (task.DeadlineTime == DateTime.MaxValue)
		{
			labelDeadline.IsVisible = false;
			labelName.WidthRequest = labelName.MaximumWidthRequest;
		}

		AddCategoriesToView();
	}

    private void checkTaskDone_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
		try
		{
			if (e.Value == true)
			{
				task.IsDone = true;
				App.Database.UpdateTask(task);

				TaskDetailView.Opacity = 0.3;
			}
			else
			{
				task.IsDone = false;
				App.Database.UpdateTask(task);

				TaskDetailView.Opacity = 1;
			}

			if (App.Setting.IsAutoBackupEnabled)
			{
				try { App.BackupData(Path.Combine(App.Setting.BackupFolderPath, App.Setting.BackupFileName)); }
				catch (Exception) { }
			}
		}
#if DEBUG
        catch (Exception ex)
        {
            _ = Navigation.NavigationStack[Navigation.NavigationStack.Count-1].DisplayAlert("Error", ex.Message, "OK");
            if (Debugger.IsAttached)
            {
                Debug.Print(ex.ToString());
            }
        }
#else
        catch (Exception)
        {
            string message = "An unknown error has occurred while processing your request. Please try again later.";
            message += "\n\nIf the problem still persists, please report the issue at the folowing email:\n19521392@gm.uit.edu.vn";
            _ = Navigation.NavigationStack[Navigation.NavigationStack.Count-1].DisplayAlert("Oops!", message, "OK");
        }
#endif
    }

    private void AddCategoriesToView()
	{
		foreach (var categoryId in task.CategoryId)
		{
			var category = App.Database.FindCategory(categoryId);
			if (category != null)
			{
				var labelCategory = new Label() { Text = category.Id == 0 ? (App.Setting.IsVietnamese ? "Quan trọng" : "Important") : category.Name,
												TextColor = Color.FromArgb(category.Color_Hex) };
				var labelSeparator = new Label() { Text = "|", Margin = new Thickness(10, 0), TextColor = Colors.Black };

				CategoriesWrapper.Add(labelCategory);
				CategoriesWrapper.Add(labelSeparator);
			}
		}
		if (CategoriesWrapper.Children.Count > 0)
		{
			CategoriesWrapper.RemoveAt(CategoriesWrapper.Children.Count - 1);
		}
		else
		{
			CategoriesWrapper.IsVisible = false;
		}
	}

    private async void btnTaskView_Clicked(object sender, EventArgs e)
    {
		try
		{
			await App.mainPage.Navigation.PushAsync(new EditTaskPage(task));
		}
#if DEBUG
        catch (Exception ex)
        {
            _ = Navigation.NavigationStack[Navigation.NavigationStack.Count-1].DisplayAlert("Error", ex.Message, "OK");
            if (Debugger.IsAttached)
            {
                Debug.Print(ex.ToString());
            }
        }
#else
        catch (Exception)
        {
            string message = "An unknown error has occurred while processing your request. Please try again later.";
            message += "\n\nIf the problem still persists, please report the issue at the folowing email:\n19521392@gm.uit.edu.vn";
            _ = Navigation.NavigationStack[Navigation.NavigationStack.Count - 1].DisplayAlert("Oops!", message, "OK");
        }
#endif
    }
}