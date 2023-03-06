using System.Diagnostics;

namespace CS526_Project.UserControls;

public partial class DayOfWeekButtonView : ContentView
{
    private Layout parent;
    public DateTime Date { get; private set; }
    public DayOfWeekButtonView(DateTime date, Layout parent)
    {
        InitializeComponent();
        Date = date;
        this.parent = parent;

        switch (date.DayOfWeek)
        {
            case DayOfWeek.Sunday: labelDayOfWeek.Text = App.Setting.IsVietnamese ? "CN" : "Su"; break;
            case DayOfWeek.Monday: labelDayOfWeek.Text = App.Setting.IsVietnamese ? "T2" : "Mo"; break;
            case DayOfWeek.Tuesday: labelDayOfWeek.Text = App.Setting.IsVietnamese ? "T3" : "Tu"; break;
            case DayOfWeek.Wednesday: labelDayOfWeek.Text = App.Setting.IsVietnamese ? "T4" : "We"; break;
            case DayOfWeek.Thursday: labelDayOfWeek.Text = App.Setting.IsVietnamese ? "T5" : "Th"; break;
            case DayOfWeek.Friday: labelDayOfWeek.Text = App.Setting.IsVietnamese ? "T6" : "Fr"; break;
            case DayOfWeek.Saturday: labelDayOfWeek.Text = App.Setting.IsVietnamese ? "T7" : "Sa"; break;

        }
        
        if (date.Day == 1)
        {
            if (date.Month == 1)
            {
                labelDate.Text = $"{date.Day}/{date.Month}\n{date.Year}";
            }
            else
            {
                labelDate.Text = $"{date.Day}/{date.Month}";
            }
            labelDate.FontSize = 13;
        }
        else
        {
            labelDate.Text = date.Day.ToString();
        }

        if (date == App.mainPage_SelectedDate)
        {
            btn.BackgroundColor = App.PrimaryColor;
            labelDate.TextColor = Colors.White;
            labelDayOfWeek.TextColor = Colors.White;
        }

        if (date == DateTime.Now.Date)
        {
            btn.BorderColor = App.PrimaryColor;
            btn.BorderWidth = 2;
        }
    }

    public void UnselectButton()
	{
		btn.BackgroundColor = App.SecondaryColor;
        labelDate.TextColor = Colors.Black;
        labelDayOfWeek.TextColor = Colors.Black;
    }

	public void UnselectAllButton()
	{
		foreach (DayOfWeekButtonView btnView in parent)
		{
            btnView.UnselectButton();
		}
	}

    private void Button_Clicked(object sender, EventArgs e)
    {
        try
        {
            UnselectAllButton();
            App.mainPage.TaskViewWrapper.Children.Clear();

            btn.BackgroundColor = App.PrimaryColor;
            labelDate.TextColor = Colors.White;
            labelDayOfWeek.TextColor = Colors.White;

            App.mainPage_SelectedDate = this.Date;
            App.mainPage.ShowTask(Date);
        }
#if DEBUG
        catch (Exception ex)
        {
            _ = Navigation.NavigationStack[Navigation.NavigationStack.Count - 1].DisplayAlert("Error", ex.Message, "OK");
            
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
}