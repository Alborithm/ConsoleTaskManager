using System.Globalization;
using Serilog;
using ConsoleTaskManager.Models;
using System.Runtime.CompilerServices;
using System.Data;
using ConsoleTaskManager.Enums;
using ConsoleTaskManager.Repositories;
using Serilog.Core;

Log.Logger = new LoggerConfiguration()
  .WriteTo.Console()
  .WriteTo.File("log.txt",
    rollingInterval: RollingInterval.Day,
    rollOnFileSizeLimit: true)
  .CreateLogger();

// ITodoRepository todoRepository = new InMemoryTodoRepositry();
ITodoRepository todoRepository = new JsonTodoRepository();

// Remove later, this adds some test tasks to the repository
// TestTasks();

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
      PrintTableHeader();
      foreach (var todo in todos)
      {
        PrintTodo(todo);
      }
    }

    // Update
    if (Key.KeyChar == '3')
    {
      Console.WriteLine("\n\n");
      Console.WriteLine("Type the ID of the ToDo you want to update");
      string UserInput = Console.ReadLine();
      UpdateOptions updateOption;
      // int TodoId;
      if (Int32.TryParse(UserInput, out int TodoId))
      {
        // Update sequence
        PrintTableHeader();
        Todo currentTodo = todoRepository.GetTodo(TodoId);
        PrintTodo(currentTodo);

        // User interaction with the ToDo
        PromptUserOtions(UserOptions.UPDATE);

        Key = Console.ReadKey();
        if (Int32.TryParse(Key.KeyChar.ToString(), out int selectedOption))
        {
          updateOption = (UpdateOptions)selectedOption;
          UpdateTodo(currentTodo, updateOption);
        }
        else
        {
          Console.WriteLine("Invalid option");
        }

      }
      else
      {
        Console.WriteLine("Invalid input");
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

void PrintTableHeader()
{
  Console.WriteLine("{0,5}|{1,20}|{2,40}|{3,9}|{4,15}", "Id", "Title", "Description", "Completed", "Due Date");
}

void PromptUserOtions(UserOptions option)
{
  switch (option)
  {
    case UserOptions.CREATE:
      throw new NotImplementedException("methods PromptUserOptions fr create not implemented");
      break;
    case UserOptions.UPDATE:
      Console.WriteLine("What is it you want to do with the task... \n");
      Console.WriteLine("(1) Set as Complete");
      Console.WriteLine("(2) Edit title and/or description");
      Console.WriteLine("(3) Edit due date");
      Console.WriteLine("(4) Delete ToDo"); // this will require user confirmation
      Console.WriteLine("(0) Go back");
      Console.WriteLine("");
      break;
    default:
      break;
  }
}

void UpdateTodo(Todo todo, UpdateOptions option)
{
  switch (option)
  {
    case UpdateOptions.SET_COMPLETE:
      todo.IsComplete = true;
      todoRepository.Update(todo);
      break;
    case UpdateOptions.EDIT_TITLE_DESCRIPTION:
      Console.WriteLine("Type new Title for update or 'Enter' to skip");

      string? newTitle = Console.ReadLine();
      newTitle = string.IsNullOrEmpty(newTitle) ? todo.Title : newTitle;

      Console.WriteLine("Type new Description for update or 'Enter' to skip");

      string? newDescription = Console.ReadLine();
      newDescription = string.IsNullOrEmpty(newDescription) ? todo.Description : newDescription;

      todo.Title = newTitle;
      todo.Description = newDescription;

      todoRepository.Update(todo);
      break;
    case UpdateOptions.EDIT_DUE_DATE:
      Console.WriteLine("Type new date as format (yyyy/mm/dd), \n" +
       "(1) To push one day later from the actual date\n" +
       "(2) Set is as urgent by setting it for today\n");

      string? userInput = Console.ReadLine();
      if (userInput == "1")
      {
        // push the date 1 day later
        todo.DueDate = todo.DueDate + TimeSpan.FromDays(1);
      }
      else if (userInput == "2")
      {
        // set it for today
        todo.DueDate = DateTime.Today;
      }
      else if (DateTime.TryParseExact(userInput, "yyyy/MM/dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime newDate))
      {
        todo.DueDate = newDate;
      }
      else
      {
        break;
      }
      todoRepository.Update(todo);
      break;
    case UpdateOptions.DELETE:
      Console.WriteLine($"Are you sure you want to delete ToDo '{todo.Title}'");
      Console.WriteLine("(1) Yes");
      Console.WriteLine("(2) No");
      var key = Console.ReadKey();
      if (key.KeyChar == '1')
      {
        todoRepository.Delete(todo.Id);
        Console.WriteLine("Deleted");
        Log.Information("Task deleted: Title: {@Title}, Id: {@Id} at: {@Timestamp}", todo.Title, todo.Id, DateTime.UtcNow);
      }
      else
      {
        Console.WriteLine("Deletion canceled");
      }
      break;
    default:
      throw new ArgumentException("Invalid Update option");
  }
}
