using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace CS526_Project.Model
{
    public class DatabaseHelper
    {
        private string db_filepath = Path.Combine(FileSystem.AppDataDirectory, "database.db3");
        private SQLiteConnection db_connection;

        private void Init()
        {
            db_connection = new SQLiteConnection(db_filepath);
            db_connection.CreateTable<ToDo_Task>();
        }

        public void AddTask(ToDo_Task task)
        {
            if (db_connection == null)
                Init();

            if (task == null)
                throw new Exception("task is null");

            db_connection.Insert(task, typeof(ToDo_Task));

            
        }

        public List<ToDo_Task> GetAllTask()
        {
            if (db_connection == null)
                Init();
            return db_connection.Table<ToDo_Task>().ToList();
        }

        public void DeleteAllTask()
        {
            if (db_connection == null)
                Init();

            db_connection.DeleteAll<ToDo_Task>();
        }
    }
}
