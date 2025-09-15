using ContactApp.Models;
using ContactApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContactApp.Controllers
{
    public class ContactsController : Controller
    {
        private readonly IContactRepository _repo;
        private readonly ILogger<ContactsController> _logger;

        public ContactsController(IContactRepository repo, 
            ILogger<ContactsController> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        [HttpGet("")]
        public IActionResult Index(string? q)
        {
            var items = _repo.GetAll();
            
            if(!string.IsNullOrEmpty(q))
            {
                var term = q.Trim();
                items = items.Where(c =>
                    (c.FirstName + " " + c.LastName).Contains(term, StringComparison.CurrentCultureIgnoreCase)
                        || c.FirstName.Contains(term, StringComparison.CurrentCultureIgnoreCase)
                        || c.LastName.Contains(term, StringComparison.CurrentCultureIgnoreCase));
            }
            ViewData["Title"] = "Kişiler";
            ViewBag.Query = q;
            return View(items.ToList());
        }

        public IActionResult Details(int id)
        {
            var contact = _repo.GetById(id);
            if (contact is null)
                return NotFoundView();
            ViewData["Title"] = "Kişi Görüntüleme";
            return View(contact);
        }

        private IActionResult NotFoundView()
        {
            Response.StatusCode = 404;
            ViewData["Title"] = "Bulunamadı";
            ViewBag.Message = "Kişi bulunamadı.";
            return View("NotFound");
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            ViewData["Title"] = "Yeni Kişi";
            return View(new Contact());
        }

        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Contact contact)
        {
            if (!ModelState.IsValid) 
            {
                ViewData["Title"] = "Yeni Kişi";
                return View(contact);
            }
            _repo.Add(contact);
            _logger.LogInformation($"Kişi eklendi: {contact.Id} {contact.FirstName} {contact.LastName}");
            TempData["Success"] = "Kayıt eklendi";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("edit/{id}")]
        public IActionResult Edit(int id)
        {
            var contact = _repo.GetById(id);
            if(contact is null)
                return NotFoundView();
            ViewData["Title"] = "Kişi Görüntüleme";
            return View(contact);
        }
        
        [HttpPost("edit/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Contact contact)
        {
            if(id != contact.Id)
            {
                ModelState.AddModelError(string.Empty, "Geçersiz istek!");
            }

            if(!ModelState.IsValid)
            {
                ViewData["Title"] = "Kişi Güncelleme";
                return View(contact);
            }

            var ok = _repo.Update(contact);
            if (!ok) 
                return NotFoundView();

            _logger.LogInformation($"Kişi güncellendi. Id: {contact.Id} {contact.FirstName} {contact.LastName}");
            TempData["Success"] = "Kayıt güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("delete/{id}")]
        public IActionResult Delete(int id)
        {
            var contact = _repo.GetById(id);
            if (contact is null)
                return NotFoundView();
            ViewData["Title"] = "Silme Onayı";
            return View(contact);
        }

        [HttpPost("delete/{id}")]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var ok = _repo.Delete(id);
            if(!ok)
            {
                return NotFoundView();
            }
            TempData["Success"] = "Kayıt silindi.";
            return RedirectToAction(nameof(Index));
        }
    }
}
