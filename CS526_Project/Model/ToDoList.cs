using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks

namespace CS526_Project.Model
{
    public class ToDoList
    {
        public int id { get; set; }
        public string Description { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public DateTime StartDate { get; set; } = DateTime.now;
        public DateTime EndDate { get; set; } = DateTime.Today.AddDays(1); // Deadline 1 day by default
        public bool Done { get; set; } = false;
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
