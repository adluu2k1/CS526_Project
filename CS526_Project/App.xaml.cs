using CS526_Project.Model;

namespace CS526_Project;

public partial class App : Application
{
    public static DatabaseHelper Database { get; } = new DatabaseHelper();

	public static List<ToDo_Task> ToDo_List = new List<ToDo_Task>();
	public App()
	{
		InitializeComponent();

		ToDo_Task task1 = new();
		{
			task1.id = 1;
			task1.Description = "Test1";
			task1.CategoryId = new List<int> { 1, 2 };
            task1.AddTime = new DateTime(2023, 1, 17, 14, 0, 0);
            task1.DeadlineTime = new DateTime(2023, 2, 21, 9, 0, 0);
			task1.NotificationTime = new List<DateTime> { new DateTime(2023, 1, 31, 14, 0, 0) };
			task1.IsDone = true;
		}
        ToDo_Task task2 = new();
        {
            task2.id = 2;
            task2.Description = "Test2";
            task2.CategoryId = new List<int> { 3 };
            task2.AddTime = new DateTime(2023, 1, 1, 0, 0, 0);
            task2.DeadlineTime = new DateTime(2023, 1, 22, 0, 0, 0);
            task2.NotificationTime = new List<DateTime> { new DateTime(2023, 1, 21, 0, 0, 0),
                                                        new DateTime(2023, 1, 21, 20, 0, 0) };
        }
        ToDo_Task task3 = new();
        {
            task3.id = 3;
            task3.CategoryId = new List<int> { 1, 2, 3 };
            task3.AddTime = new DateTime(2023, 1, 1, 0, 0, 0);
            task3.DeadlineTime = new DateTime(2023, 12, 31, 23, 59, 59);
            task3.NotificationTime = new List<DateTime> { new DateTime(2023, 1, 17, 0, 0, 0) };
        }
        ToDo_List.Add(task1);
        ToDo_List.Add(task2);
        ToDo_List.Add(task3);

        MainPage = new AppShell();
	}
}
