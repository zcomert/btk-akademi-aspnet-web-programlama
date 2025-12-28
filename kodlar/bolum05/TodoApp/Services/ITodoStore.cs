using TodoApp.Models;

namespace TodoApp.Services;
public interface ITodoStore
{
    IEnumerable<Todo> GetAll();
    IEnumerable<Todo> Search(string? term, TodoPriority? priority, bool? isDone, bool? dueDateAsc);
    Todo? Get(Guid id);
    void Add(Todo todo);
    bool Update(Todo todo);
    bool Delete(Guid id);
}
