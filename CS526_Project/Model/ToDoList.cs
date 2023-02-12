using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace CS526_Project.Model
{
    public class ToDo_Task
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime AddTime { get; set; } = DateTime.Now;
        public DateTime DeadlineTime { get; set; } = DateTime.Today.AddDays(1); // Deadline 1 day by default
        public DateTime EndTime { get; set; }
        public bool IsDone { get; set; } = false;

        public string str_CategoryId { get; set; } = "[]";
        [SQLite.Ignore]
        public List<int> CategoryId
        {
            get { return JsonSerializer.Deserialize(str_CategoryId, typeof(List<int>)) as List<int>; }
            //set { str_CategoryId = JsonSerializer.Serialize(value, typeof(List<int>)); }
        }

        public string str_NotificationTime { get; set; } = "[]";
        [SQLite.Ignore]
        public List<DateTime> NotificationTime
        {
            get { return JsonSerializer.Deserialize(str_NotificationTime, typeof(List<DateTime>)) as List<DateTime>; }
            //set { str_NotificationTime = JsonSerializer.Serialize(value, typeof(List<DateTime>)); }
        }
    }
}
