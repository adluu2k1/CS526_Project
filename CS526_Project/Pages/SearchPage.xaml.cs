﻿using CS526_Project.Model;
using CS526_Project.UserControls;
using Microsoft.Maui.Controls.Shapes;
using System.Diagnostics;

namespace CS526_Project;

public partial class SearchPage : ContentPage
{
    private bool Tag_IsImportant = false;
    private bool Tag_IsNotImportant = false;
    private bool Tag_IsDone = false;
    private bool Tag_IsNotDone = false;

    private List<object> listBtnRemoveCategory = new List<object>();

    public SearchPage()
    {
        try
        {
            InitializeComponent();

            ShowTask("");
            if (!App.Setting.IsVietnamese)
            {
                txtSearch.Placeholder = "Search";
                labelAdvance.Text = "Advanced Search";
                labelCompleted.Text = "Completed";
                labelImportant.Text = "Important";
                labelNotCompleted.Text = "Pending";
                labelNotImportant.Text = "Not Important";
                labelDeadlineFilter.Text = "Filter results by deadlines";
                labelDeadlineFrom.Text = "From:";
                labelDeadlineTo.Text = "To:";
                labelCategories.Text = "Filter results by tags";
                labelAddCategory.Text = "Add Tag";
            }
        }
#if DEBUG
        catch (Exception ex)
        {
            _ = DisplayAlert("Error", ex.Message, "OK");
            if (Debugger.IsAttached)
            {
                Debug.Print(ex.ToString());
            }
        }
#else
        catch (Exception)
        {
            string message = "An unknown error has occurred while loading this page. Please try again later.";
            message += "\n\nIf the problem still persists, please report the issue at the folowing email:\n19521392@gm.uit.edu.vn";
            _ = DisplayAlert("Oops!", message, "OK");
        }
#endif
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

            if (checkAdvance.IsChecked)
            {
                if ((Tag_IsDone && !task.IsDone) || (Tag_IsNotDone && task.IsDone))
                {
                    result.RemoveAt(i); continue;
                }

                bool task_IsImportant = App.Database.IsTaskImportant(task);
                if ((Tag_IsImportant && !task_IsImportant) || (Tag_IsNotImportant && task_IsImportant))
                {
                    result.RemoveAt(i); continue;
                }

                if (checkDeadlineFilter.IsChecked &&
                    (task.DeadlineTime < dateDeadlineFrom.Date || task.DeadlineTime > dateDeadlineTo.Date))
                {
                    result.RemoveAt(i); continue;
                }

                if (checkCategoryFilter.IsChecked)
                {
                    bool need_to_be_removed = false;
                    for (int j = 0; j < CategoriesFilterWrapper.Count - 1; j++)
                    {
                        var border = CategoriesFilterWrapper[j] as Border;
                        var view = border.Content as Layout;
                        var picker = view.Children[0] as Picker;
                        var category = App.Database.FindCategory(picker.SelectedItem as string);

                        if (category == null) continue;

                        if (!task.CategoryId.Contains(category.Id))
                        {
                            need_to_be_removed = true;
                            break;
                        }
                    }
                    if (need_to_be_removed) { result.RemoveAt(i); continue; }
                }
            }

            i++;
        }

        return result;

    }

    public void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            var keyword = "";
            if (txtSearch.Text != null)
                keyword = txtSearch.Text.Trim();
            ShowTask(keyword);
        }
#if DEBUG
        catch (Exception ex)
        {
            _ = DisplayAlert("Error", ex.Message, "OK");
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
            _ = DisplayAlert("Oops!", message, "OK");
        }
#endif
    }

    private void checkDone_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        try
        {
            Tag_IsDone = e.Value;
            txtSearch_TextChanged(null, null);
        }
#if DEBUG
        catch (Exception ex)
        {
            _ = DisplayAlert("Error", ex.Message, "OK");
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
            _ = DisplayAlert("Oops!", message, "OK");
        }
#endif
    }

    private void checkImportant_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        try
        {
            Tag_IsImportant = e.Value;
            txtSearch_TextChanged(null, null);
        }
#if DEBUG
        catch (Exception ex)
        {
            _ = DisplayAlert("Error", ex.Message, "OK");
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
            _ = DisplayAlert("Oops!", message, "OK");
        }
#endif
    }

    private void checkNotDone_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        try
        {
            Tag_IsNotDone = e.Value;
            txtSearch_TextChanged(null, null);
        }
#if DEBUG
        catch (Exception ex)
        {
            _ = DisplayAlert("Error", ex.Message, "OK");
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
            _ = DisplayAlert("Oops!", message, "OK");
        }
#endif
    }

    private void checkNotImportant_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        try
        {
            Tag_IsNotImportant = e.Value;
            txtSearch_TextChanged(null, null);
        }
#if DEBUG
        catch (Exception ex)
        {
            _ = DisplayAlert("Error", ex.Message, "OK");
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
            _ = DisplayAlert("Oops!", message, "OK");
        }
#endif
    }

    private void btnAddCategory_Clicked(object sender, EventArgs e)
    {
        try
        {
            List<string> categoriesStr = new List<string>();
            foreach (var category in App.Database.GetAllCategories())
            {
                if (category.Id == 0) continue;
                categoriesStr.Add(category.Name);
            }

            Picker categoryPicker = new Picker()
            {
                ItemsSource = categoriesStr,
                Title = App.Setting.IsVietnamese ? "Chọn nhãn" : "Select a tag",
                Margin = new Thickness(10, 0, 5, 0),
            };
            ImageButton btnRemove = new ImageButton()
            {
                Style = (Style)this.Resources["RemoveButtonStyle"],
                VerticalOptions = LayoutOptions.Center,
                Margin = new Thickness(5, 0, 10, 0)
            };
            categoryPicker.SelectedIndexChanged += categoryPicker_SelectedIndexChanged;
            btnRemove.Clicked += btnRemoveCategory_Clicked;

            HorizontalStackLayout view = new HorizontalStackLayout();
            view.Children.Add(categoryPicker);
            view.Children.Add(btnRemove);

            Border border = new Border()
            {
                BackgroundColor = Colors.Transparent,
                Stroke = App.Setting.IsDarkMode ? Colors.White : Colors.Black,
                StrokeShape = new RoundRectangle() { CornerRadius = new CornerRadius(10) },
                StrokeThickness = 1,
                Margin = new Thickness(0, 0, 10, 0)
            };
            border.Content = view;

            CategoriesFilterWrapper.Insert(CategoriesFilterWrapper.Count - 1, border);

            listBtnRemoveCategory.Add(btnRemove);
        }
#if DEBUG
        catch (Exception ex)
        {
            _ = DisplayAlert("Error", ex.Message, "OK");
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
            _ = DisplayAlert("Oops!", message, "OK");
        }
#endif
    }

    private void btnRemoveCategory_Clicked(object sender, EventArgs e)
    {
        try
        {
            for (int i = 0; i < listBtnRemoveCategory.Count; i++)
            {
                if (listBtnRemoveCategory[i] == sender)
                {
                    listBtnRemoveCategory.RemoveAt(i);
                    CategoriesFilterWrapper.Children.RemoveAt(i);
                }
            }

            txtSearch_TextChanged(null, null);
        }
#if DEBUG
        catch (Exception ex)
        {
            _ = DisplayAlert("Error", ex.Message, "OK");
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
            _ = DisplayAlert("Oops!", message, "OK");
        }
#endif
    }

    private void categoryPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            var picker = sender as Picker;

            foreach (var category in App.Database.GetAllCategories())
            {
                if (picker.SelectedItem as string == category.Name)
                {
                    picker.TextColor = Color.FromArgb(category.Color_Hex);
                    break;
                }
            }

            txtSearch_TextChanged(null, null);
        }
#if DEBUG
        catch (Exception ex)
        {
            _ = DisplayAlert("Error", ex.Message, "OK");
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
            _ = DisplayAlert("Oops!", message, "OK");
        }
#endif
    }

    private void checkDeadlineFilter_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        try
        {
            DeadlineFilterWrapper.IsVisible = e.Value;
            txtSearch_TextChanged(null, null);
        }
#if DEBUG
        catch (Exception ex)
        {
            _ = DisplayAlert("Error", ex.Message, "OK");
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
            _ = DisplayAlert("Oops!", message, "OK");
        }
#endif
    }

    private void checkAdvance_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        try
        {
            AdvanceSearchOptionsWrapper.IsVisible = e.Value;
            txtSearch_TextChanged(null, null);
        }
#if DEBUG
        catch (Exception ex)
        {
            _ = DisplayAlert("Error", ex.Message, "OK");
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
            _ = DisplayAlert("Oops!", message, "OK");
        }
#endif
    }

    private void checkCategoryFilter_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        try
        {
            var view = CategoriesFilterWrapper.Parent as View;
            view.IsVisible = e.Value;
            txtSearch_TextChanged(null, null);
        }
#if DEBUG
        catch (Exception ex)
        {
            _ = DisplayAlert("Error", ex.Message, "OK");
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
            _ = DisplayAlert("Oops!", message, "OK");
        }
#endif
    }

    private void dateDeadline_DateSelected(object sender, DateChangedEventArgs e)
    {
        try
        {
            txtSearch_TextChanged(null, null);
        }
#if DEBUG
        catch (Exception ex)
        {
            _ = DisplayAlert("Error", ex.Message, "OK");
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
            _ = DisplayAlert("Oops!", message, "OK");
        }
#endif
    }
}
