namespace ConsoleTaskManager.Models;

public class Todo
{
  public int Id { get; set; }
  public string Title { get; set; }
  public string Description { get; set; }
  public bool Status { get; set; } = false;
  public DateTime DueDate { get; set; } = DateTime.Today;
}