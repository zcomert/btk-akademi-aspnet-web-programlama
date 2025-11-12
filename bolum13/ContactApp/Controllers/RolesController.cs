using ContactApp.Models.Identity;
using ContactApp.Models.ViewModels;
using ContactApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContactApp.Controllers
{
    [Authorize(Roles ="Admin")]
    public class RolesController : Controller
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public async Task<IActionResult> Index()
        {
            var roles = await _roleService.GetRoles();
            return View(roles);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")] RoleViewModel roleViewModel)
        {
            if (ModelState.IsValid)
            {
                if(await _roleService.RoleExists(roleViewModel.Name!))
                {
                    ModelState.AddModelError(string.Empty, "Role zaten tanımlı!");
                    return View(roleViewModel);
                }

                var result = await _roleService.CreateRole(roleViewModel.Name!);
                if(result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(roleViewModel);
        }

        // GET: Roles/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var role = await _roleService.GetRoleById(id);
            if (role == null)
            {
                return NotFound();
            }
            var roleViewModel = new RoleViewModel
            {
                Id = role.Id,
                Name = role.Name
            };
            return View(roleViewModel);
        }

        // POST: Roles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name")] RoleViewModel roleViewModel)
        {
            if (id != roleViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var role = new ApplicationRole { Id = roleViewModel.Id, Name = roleViewModel.Name };
                var result = await _roleService.UpdateRole(role);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(roleViewModel);
        }

        // GET: Roles/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var role = await _roleService.GetRoleById(id);
            if (role == null)
            {
                return NotFound();
            }

            var roleViewModel = new RoleViewModel
            {
                Id = role.Id,
                Name = role.Name
            };
            return View(roleViewModel);
        }

        // POST: Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var result = await _roleService.DeleteRole(id);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
