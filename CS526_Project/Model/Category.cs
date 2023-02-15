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
        public List<int> TaskIDs { get; set; }

    }
}
