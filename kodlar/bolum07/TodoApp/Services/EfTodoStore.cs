using Microsoft.EntityFrameworkCore;
using TodoApp.Models;
using TodoApp.Repositories;

namespace TodoApp.Services
{
    public class EfTodoStore : ITodoStore
    {
        private readonly TodoDbContext _db;
        private readonly ILogger<EfTodoStore> _logger;

        public EfTodoStore(TodoDbContext db, ILogger<EfTodoStore> logger)
        {
            _db = db;
            _logger = logger;
        }

        public void Add(Todo todo)
        {
            if(todo.Id.Equals(Guid.Empty))
                todo.Id = Guid.NewGuid();
            _db.Todos.Add(todo);
            _db.SaveChanges();
            _logger.LogInformation($"Görev eklendi: {todo.Title}");
        }

        public bool Delete(Guid id)
        {
            var entity = _db
                .Todos
                .FirstOrDefault(x => x.Id.Equals(id));

            if(entity is null)
                return false;
            _db.Todos.Remove(entity);
            _db.SaveChanges();
            _logger.LogInformation($"Görev silindi: {entity.Title}");
            return true;
        }

        public Todo? Get(Guid id) => _db.Todos
            .AsNoTracking()
            .FirstOrDefault(x => x.Id.Equals(id));


        public IEnumerable<Todo> GetAll() => _db
            .Todos
            .AsNoTracking()
            .OrderBy(x => x.DueDate ?? DateTime.MaxValue)
            .ToList();
        

        public IEnumerable<Todo> Search(string? term, 
            TodoPriority? priority, 
            bool? isDone, 
            bool? dueDateAsc)
        {
            IQueryable<Todo> q = _db.Todos.AsNoTracking();

            if(!string.IsNullOrEmpty(term))
            {
                var t = term.Trim();
                q = q.Where(x => (x.Title != null && EF.Functions.Like(x.Title, $"%{t}%"))
                    || (x.Description != null && EF.Functions.Like(x.Description, $"%{t}%")));
            }

            if (priority.HasValue)
                q = q.Where(x => x.Priority.Equals(priority.Value));

            if (isDone.HasValue)
                q = q.Where(x => x.IsDone.Equals(isDone.Value));

            q = dueDateAsc == false
                ? q.OrderByDescending(x => x.DueDate ?? DateTime.MinValue)
                : q.OrderBy(x => x.DueDate ?? DateTime.MaxValue);

            return q.ToList();
        }

        public bool Update(Todo todo)
        {
            var exists = _db
                .Todos
                .Any(x => x.Id.Equals(todo.Id));

            if(!exists)
                return false;

            _db.Todos.Update(todo);
            _db.SaveChanges();
            _logger.LogInformation($"Görev güncellendi: {todo.Title}");
            return true;
        }
    }
}
