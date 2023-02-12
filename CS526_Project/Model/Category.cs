using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS526_Project.Model
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ToDo_Task> Tasks { get; set; }

        // Constructor
        public Category(int id, string name)
        {
            Id = id;
            Name = name;
            Tasks = new List<ToDo_Task>();
        }

    }
}
