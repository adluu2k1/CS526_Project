using CS526_Project.Model;
using Plugin.LocalNotification;
using static Android.Provider.ContactsContract.CommonDataKinds;

namespace CS526_Project;

public partial class App : Application
{
	public List<ToDo_Task> ToDo_List = new List<ToDo_Task>();
    public List<NotificationRequest> Notifications= new List<NotificationRequest>();
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
			task1.NotificationTime.Add(new DateTime(2023, 2, 14, 20, 0, 0));
			task1.IsDone = true;
		}
        ToDo_Task task2 = new();
        {
            task2.id = 2;
            task2.Description = "Test2";
            task2.CategoryId = new List<int> { 3 };
            task2.AddTime = new DateTime(2023, 1, 1, 0, 0, 0);
            task2.DeadlineTime = new DateTime(2023, 1, 22, 0, 0, 0);
            task2.NotificationTime.Add(new DateTime(2023, 2, 14, 20, 0, 10));
            task2.NotificationTime.Add(new DateTime(2023, 2, 14, 20, 0, 20));
        }
        ToDo_Task task3 = new();
        {
            task3.id = 3;
            task3.Description = "Test3";
            task3.CategoryId = new List<int> { 1, 2, 3 };
            task3.AddTime = new DateTime(2023, 1, 1, 0, 0, 0);
            task3.DeadlineTime = new DateTime(2023, 12, 31, 23, 59, 59);
            task3.NotificationTime.Add(new DateTime(2023, 2, 14, 20, 0, 30));
        }
        ToDo_List.Add(task1);
        ToDo_List.Add(task2);
        ToDo_List.Add(task3);

        MainPage = new AppShell();

        foreach(var task in ToDo_List)
        {
            foreach (var time in task.NotificationTime)
            {
                Random rnd = new Random();
                int num = rnd.Next();
                var request = new NotificationRequest
                {
                    NotificationId = task.id + num,
                    Title = task.Description,
                    Subtitle = "It's time!!!",
                    Description = "Let's go!!!",
                    BadgeNumber = 42,
                    Schedule = new NotificationRequestSchedule
                    {
                        NotifyTime = time,
                    }
                };

                Notifications.Add(request); 
            }
        }

        foreach (var noti in Notifications)
        {
            LocalNotificationCenter.Current.Show(noti);
        }
	}



}
