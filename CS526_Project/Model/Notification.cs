namespace CS526_Project.Model
{
    public class Notification
    {
        [SQLite.PrimaryKey]
        public int Id { get; set; }
        public DateTime time { get; set; }
    }
}
