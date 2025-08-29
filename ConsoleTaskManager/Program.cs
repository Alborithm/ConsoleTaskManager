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