namespace DoToo.Models;

using SQLite;

public class TodoItem
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;
    public bool Completed { get; set; }

    public DateTime Due { get; set; }
}
