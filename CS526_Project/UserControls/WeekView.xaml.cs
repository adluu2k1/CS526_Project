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
        WeekViewWrapper.Children.Clear();
        Monday = Monday.AddDays(-7);

        for (int i = 0; i < 7; i++)
        {
            WeekViewWrapper.Children.Add(new DayOfWeekButtonView(Monday.AddDays(i), WeekViewWrapper));
        }
    }

    private void btnNext_Clicked(object sender, EventArgs e)
    {
        WeekViewWrapper.Children.Clear();
        Monday = Monday.AddDays(7);

        for (int i = 0; i < 7; i++)
        {
            WeekViewWrapper.Children.Add(new DayOfWeekButtonView(Monday.AddDays(i), WeekViewWrapper));
        }
    }
}