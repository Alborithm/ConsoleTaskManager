using ConsoleTaskManager.Models;

namespace ConsoleTaskManager.Repositories;

public class InMemoryTodoRepositry : ITodoRepository
{
  private readonly List<Todo> _todos = new();
  private int _autoIncrement = 0;

  public void Add(Todo todo)
  {
    todo.Id = ++_autoIncrement;
    _todos.Add(todo);
  }

  public void Delete(int id)
  {
    _todos.Remove(GetTodo(id));
  }

  public IEnumerable<Todo> GetAll()
  {
    return _todos;
  }

  public Todo GetTodo(int id)
  {
    return _todos.Find(t => t.Id == id);
  }

  public void Update(Todo todo)
  {
    Todo todoForUpdate = GetTodo(todo.Id);
    todoForUpdate = todo;
  }
}