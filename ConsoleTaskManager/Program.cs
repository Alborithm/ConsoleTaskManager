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
  Key = Console.ReadKey();
  Console.WriteLine(Key.KeyChar);

}