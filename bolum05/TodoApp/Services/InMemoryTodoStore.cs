using System.Collections.Concurrent;
using System.Security.Cryptography.Xml;
using TodoApp.Models;

namespace TodoApp.Services;

public class InMemoryTodoStore : ITodoStore
{
    private readonly ConcurrentDictionary<Guid, Todo> _items = new();
    private readonly ILogger<InMemoryTodoStore> _logger;

    public InMemoryTodoStore(ILogger<InMemoryTodoStore> logger)
    {
        _logger = logger;
        Seed();
    }

   

    public void Add(Todo todo)
    {
        todo.Id = todo.Id == Guid.Empty ? Guid.NewGuid() : todo.Id;
        _items[todo.Id] = todo;
        _logger.LogInformation($"Görev eklendi: {todo.Title} ({todo.Id})");
    }

    public bool Delete(Guid id)
    {
        var ok = _items.TryRemove(id, out var todo);
        if (ok)
        {
            _logger.LogInformation($"Görev silindi: {todo.Title} ({todo.Id})");
        }
        return ok;
    }

    public Todo? Get(Guid id)
    {
        _items.TryGetValue(id, out var todo);
        return todo;
    }

    public IEnumerable<Todo> GetAll() => _items
        .Values
        .OrderBy(x => x.DueDate ?? DateTime.MaxValue);
    

    public IEnumerable<Todo> Search(string? term, 
        TodoPriority? priority, 
        bool? isDone, 
        bool? dueDateAsc)
    {
        IEnumerable<Todo> q = _items.Values;
        if (!string.IsNullOrEmpty(term))
        {
            var t = term.Trim();
            q = q.Where(x => (x.Title?.Contains(t, StringComparison.CurrentCultureIgnoreCase) ?? false)
                || (x.Description?.Contains(t, StringComparison.CurrentCultureIgnoreCase) ?? false));
        }   
        
        if(priority.HasValue)
        {
            q = q.Where(x => x.Priority == priority.Value);
        }

        if(isDone.HasValue)
        {
            q = q.Where(x => x.IsDone == isDone.Value);
        }

        q = dueDateAsc == false
            ? q.OrderByDescending(x => x.DueDate ?? DateTime.MinValue)
            : q.OrderBy(x => x.DueDate ?? DateTime.MaxValue);

        return q.ToList();
    }

    public bool Update(Todo todo)
    {
        if (!_items.ContainsKey(todo.Id))
            return false;
        _items[todo.Id] = todo;
        _logger.LogInformation($"Görev güncellendi. {todo.Title} ({todo.Id})");
        return true; 
    }

    private void Seed()
    {
        if (_items.Count > 0)
            return;

        var today = DateTime.Today;
        
        var samples = new[]
        {
            new Todo(){Title="Alışveriş yap", Description ="Süt, ekmek ,yumurta", Priority = TodoPriority.Medium, DueDate = today.AddDays(1), IsDone=false }
            new Todo { Title = "Sunum hazırla", Description = "Pazartesi toplantısı için slaytlar", Priority = TodoPriority.High, DueDate = today.AddDays(3), IsDone = false },
            new Todo { Title = "Spor", Description = "30 dk koşu", Priority = TodoPriority.Low, DueDate = today.AddDays(2), IsDone = true },
            new Todo { Title = "Araba bakımı", Description = "Yağ değişimi ve filtreler", Priority = TodoPriority.Medium, DueDate = today.AddDays(7), IsDone = false },
        };

        foreach (var item in samples)
        {
            Add(item);
        }
    }
}
