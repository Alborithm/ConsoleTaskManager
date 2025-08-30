using ConsoleTaskManager.Models;

public class InMemoryTodoRepositry : ITodoRepository
{
  private readonly Dictionary<int, Todo> _todos = new();

  public void Add(Todo todo)
  {
    _todos.Add(_todos.Keys.LastOrDefault(0) + 1, todo);

    Console.WriteLine(_todos[1].Title);
  }

  public void Delete(int key)
  {
    throw new NotImplementedException();
  }

  public IEnumerable<Todo> GetAll()
  {
    throw new NotImplementedException();
  }

  public Todo GetTodo(int key)
  {
    throw new NotImplementedException();
  }

  public void Update(Todo todo)
  {
    throw new NotImplementedException();
  }
}