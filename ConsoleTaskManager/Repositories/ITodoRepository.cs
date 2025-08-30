// Repository pattern enables

// Separates data access logic from your business logic.
// Example: instead of directly writing CRUD inside your TodoTask class, you have a ITodoRepository interface that handles data storage.
// Makes it easier to switch between storage types (in-memory, SQL, MongoDB, file) without changing your business logic.
using ConsoleTaskManager.Models;

public interface ITodoRepository
{
  void Add(Todo todo);
  void Update(Todo todo);
  void Delete(int key);
  Todo GetTodo(int key);
  IEnumerable<Todo> GetAll();
}