using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TodoApp.Models;
using TodoApp.Services;

namespace TodoApp.Pages.Todos
{
    public class DeleteModel : PageModel
    {
        private readonly ITodoStore _store;

        public DeleteModel(ITodoStore store)
        {
            _store = store;
        }

        public Todo? Todo { get; private set; }
        public IActionResult OnGet(Guid id)
        {
            Todo = _store.Get(id);
            if(Todo is null)
            {
                TempData["Message"] = "Silinecek görev yok!";
                return RedirectToPage("Index");
            }
            return Page();
        }

        public IActionResult OnPost(Guid id)
        {
            var ok = _store.Delete(id);
            TempData["Message"] = "Görev silindi.";
            return RedirectToPage("Index");
        }
    }
}
