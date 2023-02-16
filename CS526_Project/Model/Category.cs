using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS526_Project.Model
{
    public class Category
    {
        [SQLite.PrimaryKey]
        public int Id { get; set; }
        [SQLite.Unique]
        public string Name { get; set; }
        public string Color_Hex { get; set; } = Colors.Black.ToHex();

    }
}
