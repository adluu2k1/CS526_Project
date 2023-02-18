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
            db_connection.CreateTable<Category>();
        }

        public void AddTask(ToDo_Task task)
        {
            if (db_connection == null)
                Init();

            if (task == null) throw new Exception("task is null");

            db_connection.Insert(task, typeof(ToDo_Task));
        }

        public void AddCategory(Category category)
        {
            if (db_connection == null)
                Init();

            if (category == null) throw new Exception("category is null");

            db_connection.Insert(category, typeof(Category));
        }

        public void UpdateTask(ToDo_Task task)
        {
            if (db_connection == null)
                Init();

            if (task == null) return;

            db_connection.Update(task, typeof(ToDo_Task));
        }

        public List<ToDo_Task> GetAllTask()
        {
            if (db_connection == null)
                Init();
            return db_connection.Table<ToDo_Task>().ToList();
        }

        public List<Category> GetAllCategories()
        {
            if (db_connection == null)
                Init();
            return db_connection.Table<Category>().ToList();
        }

        public bool IsTaskIdTaken(int id)
        {
            if (db_connection == null)
                Init();

            foreach (var task in GetAllTask())
            {
                if (task.id == id) return true;
            }
            return false;
        }

        public bool IsCategoryIdTaken(int id)
        {
            if (db_connection == null)
                Init();

            foreach (var category in GetAllCategories())
            {
                if (category.Id == id) return true;
            }
            return false;
        }

        public bool IsCategoryNameTaken(string name)
        {
            foreach (Category category in GetAllCategories())
            {
                if (category.Name == name)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsTaskContainingCategory(ToDo_Task task, string categoryName)
        {
            foreach (var id in task.CategoryId)
            {
                if (id == FindCategory(categoryName).Id) return true;
            }
            return false;
        }

        public int GenerateRandomTaskId()
        {
            int id = new Random().Next(1, short.MaxValue);
            if (IsTaskIdTaken(id)) id = GenerateRandomTaskId();
            return id;
        }

        public int GenerateRandomCategoryId()
        {
            int id = new Random().Next(1, short.MaxValue);
            if (IsCategoryIdTaken(id)) id = GenerateRandomCategoryId();
            return id;
        }

        public ToDo_Task FindTask(int id)
        {
            if (db_connection == null)
                Init();
            return db_connection.Find<ToDo_Task>(id);
        }

        public Category FindCategory(int id)
        {
            if (db_connection == null)
                Init();
            return db_connection.Find<Category>(id);
        }

        public Category FindCategory(string name)
        {
            if (db_connection == null)
                Init();
            foreach (var category in GetAllCategories())
            {
                if (category.Name == name)
                {
                    return category;
                }
            }
            return null;
        }

        public void DeleteTask(ToDo_Task task)
        {
            if (db_connection == null)
                Init();

            db_connection.Delete(task);
        }

        public void DeleteCategory(Category category)
        {
            if (db_connection == null)
                Init();

            db_connection.Delete(category);
        }

        public void DeleteObsoleteCategory()
        {
            if (db_connection == null)
                Init();

            foreach (var category in GetAllCategories())
            {
                bool IsCategoryInAnyTask = false;
                foreach (var task in GetAllTask())
                {
                    if (task.CategoryId.Contains(category.Id))
                    {
                        IsCategoryInAnyTask = true; break;
                    }
                }
                if (!IsCategoryInAnyTask)
                {
                    DeleteCategory(category);
                }
            }
        }

        public void DeleteAllTasks()
        {
            if (db_connection == null)
                Init();
            
            db_connection.DeleteAll<ToDo_Task>();
        }

        public void DeleteAllCategories()
        {
            if (db_connection == null)
                Init();

            db_connection.DeleteAll<Category>();
        }
    }
}
