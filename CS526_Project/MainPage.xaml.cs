using CS526_Project.UserControls;

namespace CS526_Project;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();

		var today = DateTime.Now.Date;
		labelToday.Text = $"NGÀY {today.Day} THÁNG {today.Month} NĂM {today.Year}";

		WeekViewWrapper.Content = new WeekView(today);

		ShowTask(today);
	}

	public void ShowTask(DateTime date)
	{
		
	}

    private void btnSearch_Clicked(object sender, EventArgs e)
    {

    }

    private void btnAdd_Clicked(object sender, EventArgs e)
    {
		
    }

    private void btnSettings_Clicked(object sender, EventArgs e)
    {

    }
}

