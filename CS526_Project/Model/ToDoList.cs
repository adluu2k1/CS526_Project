using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS526_Project.Model
{
    public class ToDo_Task
    {
        public int id { get; set; }
        public string Description { get; set; } = string.Empty;
        public List<int> CategoryId { get; set; } = new List<int>();
        public DateTime AddTime { get; set; } = DateTime.Now;
        public List<DateTime> NotificationTime { get; set; } = new List<DateTime>();
        public DateTime DeadlineTime { get; set; } = DateTime.Today.AddDays(1); // Deadline 1 day by default
        public DateTime EndTime { get; set; }
        public bool IsDone { get; set; } = false;

        public void AddTask()
        {
            
        }
        public void EditTask()
        {

        }
        public void DeleteTask()
        {

        }
        public void AddColumn()
        {

        }
    }
}
