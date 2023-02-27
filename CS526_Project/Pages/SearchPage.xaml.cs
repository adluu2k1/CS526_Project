using CS526_Project.Model;
using CS526_Project.UserControls;

namespace CS526_Project;

public partial class SearchPage : ContentPage
{
    private bool Tag_IsImportant = false;
    private bool Tag_IsNotImportant = false;
    private bool Tag_IsDone = false;
    private bool Tag_IsNotDone = false;

    public SearchPage()
    {
        InitializeComponent();
        
        ShowTask("");
        if (!App.Setting.IsVietnamese)
        {
            txtSearch.Placeholder = "Search";
            labelCompleted.Text = "Completed";
            labelImportant.Text = "Important";
            labelNotCompleted.Text = "Pending";
            labelNotImportant.Text = "Not Important";
        }
    }

    public void ShowTask(string keyword)
    {
        TaskViewWrapper.Children.Clear();

        foreach (var task in GetSearchResult(keyword))
        {
            TaskViewWrapper.Add(new TaskView(task, TaskViewWrapper));
        }
    }

    private List<ToDo_Task> GetSearchResult(string keyword)
    {
        var result = App.Database.GetAllTask().OrderBy(p => p.DeadlineTime).ToList();

        int i = 0;
        while (i < result.Count)
        {
            var task = result[i];
            if (!task.Name.Contains(keyword) &&
                !task.Description.Contains(keyword) &&
                !task.DeadlineTime.ToString().Contains(keyword))
            {
                bool TaskCategoriesHaveKeyword = false;
                foreach (var category in App.Database.GetAllCategories(task))
                {
                    if (category.Name.Contains(keyword))
                    {
                        TaskCategoriesHaveKeyword = true; break;
                    }
                }
                if (!TaskCategoriesHaveKeyword)
                {
                    result.RemoveAt(i); continue;
                }
            }
                
            if ((Tag_IsDone && !task.IsDone) || (Tag_IsNotDone && task.IsDone))
            {
                result.RemoveAt(i); continue;
            }

            bool task_IsImportant = App.Database.IsTaskImportant(task);
            if ((Tag_IsImportant && !task_IsImportant) || (Tag_IsNotImportant && task_IsImportant))
            {
                result.RemoveAt(i); continue;
            }

            i++;
        }

        return result;

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
        Tag_IsDone = e.Value;
        txtSearch_TextChanged(null, null);
    }

    private void checkImportant_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        Tag_IsImportant = e.Value;
        txtSearch_TextChanged(null, null);
    }

    private void checkNotDone_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        Tag_IsNotDone = e.Value;
        txtSearch_TextChanged(null, null);
    }

    private void checkNotImportant_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        Tag_IsNotImportant = e.Value;
        txtSearch_TextChanged(null, null);
    }

    private void checkDeadlineFilter_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {

    }
}
