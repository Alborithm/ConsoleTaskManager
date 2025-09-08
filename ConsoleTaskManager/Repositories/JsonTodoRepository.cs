using ConsoleTaskManager.Models;
using System.Net;
using System.Reflection.Metadata;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ConsoleTaskManager.Repositories;

public class JsonTodoRepository : ITodoRepository
{

  private static readonly JsonSerializerOptions _options = new()
  {
    WriteIndented = true
  };

  private static readonly string _filePath =
    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ConsoleTaskManager", "data.json");

  public void Add(Todo todo)
  {
    using FileStream fs = new FileStream(_filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
    List<Todo> existingTodos;
    int IdAutoIncrement = 1; // default value
    if (fs.Length > 0)
    {
      existingTodos = JsonSerializer.Deserialize<List<Todo>>(fs)!;
      IdAutoIncrement = existingTodos.Last().Id + 1;
      fs.SetLength(0);
    }
    else
    {
      existingTodos = new();
    }

    todo.Id = IdAutoIncrement;
    existingTodos.Add(todo);

    JsonSerializer.Serialize(fs, existingTodos, _options);
  }

  public void Delete(int id)
  {
    using FileStream fs = File.Open(_filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
    if (fs.Length == 0)
      return;

    List<Todo> existingTodos = JsonSerializer.Deserialize<List<Todo>>(fs)!;
    existingTodos.RemoveAt(
      existingTodos.FindIndex(t => t.Id == id)
    );

    fs.SetLength(0);
    JsonSerializer.Serialize(fs, existingTodos, _options);
  }

  public IEnumerable<Todo> GetAll()
  {
    using FileStream fs = File.Open(_filePath, FileMode.Open, FileAccess.Read);
    if (fs.Length < 0)
    {
      return new List<Todo>();
    }
    return JsonSerializer.Deserialize<List<Todo>>(fs)!;
  }

  public Todo GetTodo(int id)
  {
    using FileStream fs = File.Open(_filePath, FileMode.OpenOrCreate, FileAccess.Read);
    if (fs.Length == 0)
      return new Todo();
    return JsonSerializer.Deserialize<List<Todo>>(fs)!.Find(t => t.Id == id)!;
  }

  public void Update(Todo todo)
  {
    using FileStream fs = File.Open(_filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
    if (fs.Length == 0)
      return;
    List<Todo> existingTodos = JsonSerializer.Deserialize<List<Todo>>(fs)!;

    var updatedTodo = existingTodos.Find(t => t.Id == todo.Id);
    updatedTodo.Title = todo.Title;
    updatedTodo.Description = todo.Description;
    updatedTodo.IsComplete = todo.IsComplete;
    updatedTodo.DueDate = todo.DueDate;

    fs.SetLength(0);
    JsonSerializer.Serialize(fs, existingTodos, _options);
  }

  private void TestSubjects()
  {
    var list = new List<Todo>
    {
      new Todo{
        Id = 1,
        Title = "Title 1",
        Description = "Description 1",
        DueDate = DateTime.Now,
      },
      new Todo{
        Id = 2,
        Title = "Title 2",
        Description = "Description 2",
        DueDate = DateTime.Now,
      },
      new Todo{
        Id = 3,
        Title = "Title 3",
        Description = "Description 3",
        DueDate = DateTime.Now,
      }
    };

    // This works fine when rewriting from zero
    // using FileStream fileStream = File.Create(_filePath);
    // JsonSerializer.Serialize(fileStream, list, _options);

    using FileStream fs = File.Open(_filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
    var readTodos = new List<Todo>();
    // When the file is new I need to check for a solution so that it can read the objects, this may be good enough
    if (fs.Length > 0)
    {
      readTodos = JsonSerializer.Deserialize<List<Todo>>(fs);
    }
    readTodos.AddRange(list);
    // This "empties" the stream in order to overwrite the data when updating it
    fs.SetLength(0);
    JsonSerializer.Serialize(fs, readTodos, _options);
    fs.Flush();
  }
}