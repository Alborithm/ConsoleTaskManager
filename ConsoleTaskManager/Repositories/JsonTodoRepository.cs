using ConsoleTaskManager.Models;
using System.Net;
using System.Reflection.Metadata;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ConsoleTaskManager.Repositories;

public class JsonTodoRepository : ITodoRepository
{
  // private readonly List<Todo> _todos;

  private static readonly JsonSerializerOptions _options = new()
  {
    WriteIndented = true
  };

  private static readonly string _filePath =
    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ConsoleTaskManager", "data.json");

  public JsonTodoRepository()
  {
    // _todos = JsonSerializer.Deserialize<List<Todo>>(_filePath);
  }

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
    throw new NotImplementedException();
  }

  public IEnumerable<Todo> GetAll()
  {
    using FileStream readStream = File.OpenRead(_filePath);
    var array = JsonSerializer.Deserialize<IEnumerable<Todo>>(readStream);
    return array.ToList();
  }

  public Todo GetTodo(int id)
  {
    using FileStream readStream = File.OpenRead(_filePath);
    return JsonSerializer.Deserialize<Todo>(readStream);
  }

  public void Update(Todo todo)
  {
    throw new NotImplementedException();
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

  private void Rename()
  {
    using FileStream fs = File.Open(_filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
    var existingTodos = new List<Todo>();
    if (fs.Length > 0) // If there are already contents
    {
      existingTodos = JsonSerializer.Deserialize<List<Todo>>(fs);
      // Empty the file stream for full rewrite
      fs.SetLength(0);
    }

  }
}