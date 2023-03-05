using System.Diagnostics;

namespace CS526_Project.Pages;

public class DateButton : Button
{
    public DateTime Date { get; set; }
}

public partial class MonthViewPage : ContentPage
{
    DateTime FirstDay = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

    public MonthViewPage()
    {
        try
        {
            InitializeComponent();
            lbTodayDate.Text = DateTime.Now.ToLongDateString();
            AddGridDate();
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

    public void AddGridDate()
    {   
        var FirstMonday = FirstDay.AddDays(-(int)FirstDay.DayOfWeek + (int)DayOfWeek.Monday);
        lbMonth.Text = $"Tháng {FirstDay.Month} Năm {FirstDay.Year}";

        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                DateTime date = FirstMonday.AddDays((7 * i) + j);
                var listDeadlineDates = GetAllDeadlineDates(FirstDay.Month, FirstDay.Year);

                Label DT = new Label()
                {
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    ZIndex = 100,
                    Text = date.Day.ToString(),
                    IsEnabled = false
                };
                DateButton btnDT = new DateButton()
                {
                    Date = date,
                    Margin = new Thickness(5),
                    CornerRadius = 5,
                    IsVisible = false
                };
                btnDT.Clicked += btnDT_Clicked;
                Grid wrapper = new Grid();
                wrapper.Children.Add(btnDT);
                wrapper.Children.Add(DT);

                if (date.Month == FirstDay.Month)
                {
                    DT.IsEnabled = true;
                    btnDT.IsVisible = true;
                    if (date == DateTime.Today)
                    {
                        Border border = new()
                        {
                            Stroke = new SolidColorBrush(App.PrimaryColor),
                            StrokeThickness = 3,
                            BackgroundColor = Colors.Transparent
                        };
                        wrapper.Children.Add(border);
                    }
                    if (listDeadlineDates.Contains(date))
                    {
                        btnDT.BackgroundColor = App.SecondaryColor;
                        btnDT.Opacity = App.Setting.IsDarkMode ? 0.5 : 1;
                    }
                    else
                    {
                        btnDT.BackgroundColor = Colors.Transparent;
                    }
                }

                gridDate.Children.Add(wrapper);
                Grid.SetRow(wrapper, i);
                Grid.SetColumn(wrapper, j);
            }
        }
    }

    private List<DateTime> GetAllDeadlineDates(int month, int year)
    {
        var result = new List<DateTime>();

        foreach (var task in App.Database.GetAllTask())
        {
            if (task.DeadlineTime.Month == month && task.DeadlineTime.Year == year)
            {
                result.Add(task.DeadlineTime.Date);
            }
        }

        return result;
    }

    private void btnPrevious_Clicked(object sender, EventArgs e)
    {
        FirstDay = FirstDay.AddMonths(-1);
        gridDate.Children.Clear();
        AddGridDate();
    }

    private void btnNext_Clicked(object sender, EventArgs e)
    {
        FirstDay = FirstDay.AddMonths(1);
        gridDate.Children.Clear();
        AddGridDate();
    }

    private async void btnDT_Clicked(object sender, EventArgs e)
    {
        var btnDT = sender as DateButton;
        App.mainPage_SelectedDate = btnDT.Date;
        App.mainPage.MonthView_OnSelectDate(App.mainPage_SelectedDate);

        await Navigation.PopAsync();
    }
}