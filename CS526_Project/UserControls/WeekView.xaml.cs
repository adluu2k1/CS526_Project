using System.Diagnostics;

namespace CS526_Project.UserControls;

public partial class WeekView : ContentView
{
    public DateTime Monday { get; private set; }
    public WeekView(DateTime today)
	{
		InitializeComponent();

		Monday = today.AddDays(-(int)today.DayOfWeek + (int)DayOfWeek.Monday);
        for (int i = 0; i < 7; i++)
        {
            WeekViewWrapper.Children.Add(new DayOfWeekButtonView(Monday.AddDays(i), WeekViewWrapper));
        }
    }

    private void btnPrevious_Clicked(object sender, EventArgs e)
    {
        try
        {
            WeekViewWrapper.Children.Clear();
            Monday = Monday.AddDays(-7);

            for (int i = 0; i < 7; i++)
            {
                WeekViewWrapper.Children.Add(new DayOfWeekButtonView(Monday.AddDays(i), WeekViewWrapper));
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
            _ = Navigation.NavigationStack[Navigation.NavigationStack.Count - 1].DisplayAlert("Oops!", message, "OK");
        }
#endif
    }

    private void btnNext_Clicked(object sender, EventArgs e)
    {
        try
        {
            WeekViewWrapper.Children.Clear();
            Monday = Monday.AddDays(7);

            for (int i = 0; i < 7; i++)
            {
                WeekViewWrapper.Children.Add(new DayOfWeekButtonView(Monday.AddDays(i), WeekViewWrapper));
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
            _ = Navigation.NavigationStack[Navigation.NavigationStack.Count - 1].DisplayAlert("Oops!", message, "OK");
        }
#endif
    }
}