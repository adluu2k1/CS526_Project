using CS526_Project.Model;
using CS526_Project.UserControls;

namespace CS526_Project;

public partial class SearchPage : ContentPage
{
    private bool IsImportant { get; set; } = false;
    private bool IsDone { get; set; } = false;

    public SearchPage()
    {
        InitializeComponent();

        ShowTask("");
    }

    public void ShowTask(string keyword)
    {
        TaskViewWrapper.Children.Clear();

        foreach (var task in App.Database.GetAllTask().OrderBy(p => p.DeadlineTime).ToList())
        {
            if (IsTaskQualified(task, keyword))
            {
                TaskViewWrapper.Add(new TaskView(task, TaskViewWrapper));
            }
        }
    }

    private bool IsTaskQualified(ToDo_Task task, string keyword)
    {
        if (task.Name.Contains(keyword) ||
            task.Description.Contains(keyword) ||
            task.DeadlineTime.ToString().Contains(keyword))
        {
            if (checkAll.IsChecked == true) return true;
            else
            {
                if ((IsImportant && App.Database.IsTaskContainingCategory(task, "Important")) ||
                    (IsDone && task.IsDone) ||
                    (IsImportant && IsDone && App.Database.IsTaskContainingCategory(task, "Important") && task.IsDone))
                    return true;
            }
        }
        return false;
    }

    public void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
    {
        var keyword = "";
        if (txtSearch.Text != null)
            keyword= txtSearch.Text.Trim();
        ShowTask(keyword);
    }

    private void checkDone_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        IsDone = e.Value;
        txtSearch_TextChanged(null, null);
    }

    private void checkImportant_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        IsImportant = e.Value;
        txtSearch_TextChanged(null, null);
    }

    private void checkAll_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (e.Value == true)
        {
            IsImportant = false; IsDone = false;
        }
        else
        {
            if (checkDone.IsChecked == true) IsDone = true;
            if (checkImportant.IsChecked == true) IsImportant = true;
        }

        txtSearch_TextChanged(null, null);
    }
}
