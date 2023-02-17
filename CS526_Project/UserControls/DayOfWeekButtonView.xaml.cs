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

        if (date.DayOfWeek == DayOfWeek.Sunday)
        {
            labelDayOfWeek.Text = "CN";
        }
        else
        {
            labelDayOfWeek.Text = "T" + ((int)date.DayOfWeek + 1).ToString();
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
        UnselectAllButton();
        App.mainPage.TaskViewWrapper.Children.Clear();

		btn.BackgroundColor = App.PrimaryColor;
		labelDate.TextColor = Colors.White;
		labelDayOfWeek.TextColor = Colors.White;

        App.mainPage_SelectedDate = this.Date;
        App.mainPage.ShowTask(Date);
    }
}