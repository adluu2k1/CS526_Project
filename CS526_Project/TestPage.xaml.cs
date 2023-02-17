using CS526_Project.Model;
using System.Threading.Tasks;

namespace CS526_Project;

public partial class TestPage : ContentPage
{
	public TestPage()
	{
		InitializeComponent();
	}

    public void OnNewButtonClicked(object sender, EventArgs args)
    {
        App.Database.AddTask(App.ToDo_List[0]); App.Database.AddTask(App.ToDo_List[1]); App.Database.AddTask(App.ToDo_List[2]);
    }
    public void OnGetButtonClicked(object sender, EventArgs args)
    {
        statusMessage.Text = "";

        List<ToDo_Task> tasks = App.Database.GetAllTask();

        peopleList.Children.Clear();
        peopleList.RowDefinitions.Clear();

        for (int i = 0; i < tasks.Count; i++)
        {
            peopleList.RowDefinitions.Add(new RowDefinition(new GridLength(1, GridUnitType.Star)));

            Label id = new Label();
            id.Text = tasks[i].id.ToString();
            peopleList.Add(id, 0, i);

            Label des = new Label();
            des.Text = tasks[i].Description;
            peopleList.Add(des, 1, i);

            Label addtime = new Label();
            addtime.Text = tasks[i].AddTime.ToString();
            peopleList.Add(addtime, 2, i);

            Label DeadlineTimelb = new Label();
            DeadlineTimelb.Text = tasks[i].DeadlineTime.ToString();
            peopleList.Add(DeadlineTimelb, 3, i);

            if (tasks[i].EndTime != DateTime.MinValue)
            {
                Label EndTimelb = new Label();
                EndTimelb.Text = tasks[i].EndTime.ToString();
                peopleList.Add(EndTimelb, 4, i);
            }

            Label IsDonelb = new Label();
            IsDonelb.Text = tasks[i].IsDone.ToString();
            peopleList.Add(IsDonelb, 5, i);

            Label CategoryIdlb = new Label();
            CategoryIdlb.Text = "";
            foreach (var str in tasks[i].CategoryId)
            {
                CategoryIdlb.Text += str.ToString() + " ";
            }
            peopleList.Add(CategoryIdlb, 6, i);

            Label NotificationTimelb = new Label();
            NotificationTimelb.Text = "";
            foreach (var str in tasks[i].NotificationTime)
            {
                NotificationTimelb.Text += str.ToString() + " ";
            }
            peopleList.Add(NotificationTimelb, 7, i);

        }
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        App.Database.DeleteAllTasks();
    }

    private void btnQuery_Clicked(object sender, EventArgs e)
    {

    }
}