using System.Globalization;
using Serilog;
using ConsoleTaskManager.Models;
using System.Runtime.CompilerServices;

Log.Logger = new LoggerConfiguration()
  .WriteTo.Console()
  .WriteTo.File("log.txt",
    rollingInterval: RollingInterval.Day,
    rollOnFileSizeLimit: true)
  .CreateLogger();

ITodoRepository todoRepository = new InMemoryTodoRepositry();

// Remove later, this adds some test tasks to the repository
TestTasks();

try
{
  // The program goes here...
  ConsoleLoop();
  // const string name = "Serilog";
  // Log.Information("Hello, {Name}!", name);
  // throw new InvalidOperationException("Oops...");
}
catch (Exception ex)
{
  Log.Error(ex, "Unhandled exception");
}
finally
{
  await Log.CloseAndFlushAsync(); // ensure all logs written before app exits
}

void ConsoleLoop()
{
  ConsoleKeyInfo Key = new ConsoleKeyInfo();

  // The app flow
  while (Key.KeyChar != 'q') // q for quit
  {
    Console.WriteLine("What do you want to do?");
    Console.WriteLine("(1) Get ToDo's");
    Console.WriteLine("(2) Create a ToDo");
    Console.WriteLine("(3) Update ToDo");
    Console.WriteLine("(q) or Ctrl + C to quit");

    Key = Console.ReadKey();

    if (Key.KeyChar == '2')
    {
      var newTodo = new Todo();
      newTodo.Title = PromptUser("Task title:");
      newTodo.Description = PromptUser("Task description:");
      if (DateTime.TryParseExact(
        PromptUser("Task due date (yyyy/mm/dd):"),
        "yyyy/MM/dd",
        CultureInfo.InvariantCulture,
        DateTimeStyles.None,
        out DateTime result))
      {
        newTodo.DueDate = result;
      }

      todoRepository.Add(newTodo);
      Log.Information("Task created: Title: {@Title}, Description: {Description}",
        newTodo.Title,
        newTodo.Description);
    }

    if (Key.KeyChar == '1')
    {
      var todos = todoRepository.GetAll();
      Console.WriteLine();
      Console.WriteLine("{0,5}|{1,20}|{2,40}|{3,9}|{4,15}", "Id", "Title", "Description", "Completed", "Due Date");
      foreach (var todo in todos)
      {
        PrintTodo(todo);
      }
    }

  }

  string PromptUser(string text)
  {
    Console.WriteLine(text);
    return Console.ReadLine();
  }
}

void PrintTodo(Todo todo)
{
  Console.WriteLine("{0,-5}|{1,20}|{2,40}|{3,9}|{4,15}",
    todo.Id,
    todo.Title,
    todo.Description,
    todo.IsComplete ? "Yes" : "No",
    todo.DueDate.ToShortDateString());
}

void TestTasks()
{
  todoRepository.Add(new Todo
  {
    Title = "Task 1",
    Description = "Description 1",
  });
  todoRepository.Add(new Todo
  {
    Title = "Programmed task",
    Description = "Remeber to do this",
    DueDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day + 2)
  });
}