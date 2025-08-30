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
  }

}

string PromptUser(string text)
{
  Console.WriteLine(text);
  return Console.ReadLine();
}

bool PromptTaskCreation()
{
  string Title;
  string Description;
  string DueDate;

  Console.WriteLine("Type the title:");
  Title = Console.ReadLine();

  Console.WriteLine("Type the description:");
  Description = Console.ReadLine();

  Console.WriteLine("Type the due date (yyyy/mm/dd):");
  DueDate = Console.ReadLine();

  return true;
}