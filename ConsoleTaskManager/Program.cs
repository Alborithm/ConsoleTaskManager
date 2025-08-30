using System.Globalization;
using ConsoleTaskManager.Models;

var myTask = new Todo()
{
  Id = 1,
  Title = "Create a class",
  Description = "Class to store todos",
  DueDate = new DateTime(2025, 10, 5),
};
Console.WriteLine("Hello, World!");

Console.WriteLine(myTask.Title);

ConsoleKeyInfo Key = new ConsoleKeyInfo();

// The app flow
while (Key.KeyChar != 'q')
{
  Console.WriteLine("What do you want to do?");
  Console.WriteLine("(1) Get ToDo's");
  Console.WriteLine("(2) Create a ToDo");
  Console.WriteLine("(3) Update ToDo");
  Console.WriteLine("(q) OR Ctrl + C to quit");

  Key = Console.ReadKey();

  if (Key.KeyChar == '2')
  {
    var newTodo = new Todo();
    string? input;
    Console.WriteLine("Type the Title:");
    input = Console.ReadLine()!;
    newTodo.Title = input.Trim() != "" ? input : "Default title";

    Console.WriteLine("Type de description:");
    input = Console.ReadLine()!;
    newTodo.Description = input.Trim() != "" ? input : "No Decription";

    Console.WriteLine("Type the due date in yyyy/mm/dd");
    input = Console.ReadLine();
    if (DateTime.TryParseExact(input, "yyyy/MM/dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
    {
      newTodo.DueDate = result;
    }
    else
    {
      newTodo.DueDate = DateTime.Today;
    }

    Console.WriteLine(newTodo.Title);
    Console.WriteLine(newTodo.Description);
    Console.WriteLine(newTodo.DueDate);
    Console.WriteLine(newTodo.IsComplete);
  }

}