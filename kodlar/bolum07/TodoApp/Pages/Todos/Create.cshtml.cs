using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TodoApp.Models;
using TodoApp.Services;

namespace TodoApp.Pages.Todos
{
    public class CreateModel : PageModel
    {
        private readonly ITodoStore _store;

        public CreateModel(ITodoStore store)
        {
            _store = store;
        }

        [BindProperty]
        public Todo Todo { get; set; } = new();
        public void OnGet()
        {

        }

        public IActionResult OnPost()
        {
            if(!ModelState.IsValid)
            {
                return Page();
            }

            _store.Add(Todo);
            TempData["Message"] = "Görev eklendi.";
            return RedirectToPage("Index");
        }
    }
}
