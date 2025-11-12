using ContactApp.Models.Identity;
using ContactApp.Models.ViewModels;
using ContactApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ContactApp.Controllers;

[Authorize(Roles ="Admin")]
public class UsersController : Controller
{
    private readonly IUserService _userService;
    private readonly IRoleService _roleService;

    public UsersController(IUserService userService, IRoleService roleService)
    {
        _userService = userService;
        _roleService = roleService;
    }

    // GET : Users
    public async Task<IActionResult> Index()
    {
        var users = await _userService.GetUsers();
        return View(users);
    }

    // GET : Users/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST : Users/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("UserName, Email, Password")] UserViewModel userViewModel)
    {
        if(ModelState.IsValid)
        {
            if (await _userService.UserExists(userViewModel.UserName!))
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı zaten tanımlı!");
                return View(userViewModel);
            }

            var user = new ApplicationUser
            {
                UserName = userViewModel.UserName,
                Email = userViewModel.Email,
            };

            var result = await _userService
                .CreateUser(user, userViewModel.Password!);
            
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        return View(userViewModel);
    }

    // GET: Users/Edit/5
    public async Task<IActionResult> Edit(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var user = await _userService.GetUserById(id);
        if (user == null)
        {
            return NotFound();
        }
        var userViewModel = new UserViewModel
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email
        };
        return View(userViewModel);
    }

    // POST: Users/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, [Bind("Id,UserName,Email")] UserViewModel userViewModel)
    {
        if (id != userViewModel.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            var user = await _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }

            user.UserName = userViewModel.UserName;
            user.Email = userViewModel.Email;

            var result = await _userService.UpdateUser(user);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        return View(userViewModel);
    }

    // GET: Users/Delete/5
    public async Task<IActionResult> Delete(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var user = await _userService.GetUserById(id);
        if (user == null)
        {
            return NotFound();
        }

        var userViewModel = new UserViewModel
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email
        };
        return View(userViewModel);
    }

    // POST: Users/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        var result = await _userService.DeleteUser(id);
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

    // GET: Users/ManageRoles/5
    public async Task<IActionResult> ManageRoles(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var user = await _userService.GetUserById(id);
        if (user == null)
        {
            return NotFound();
        }

        var userRoles = await _userService.GetUserRoles(user);
        var allRoles = await _roleService.GetRoles();

        var rolesViewModel = allRoles.Select(role => new RoleViewModel
        {
            Id = role.Id,
            Name = role.Name
        }).ToList();

        var userRoleViewModel = new UserRoleViewModel
        {
            UserId = user.Id,
            UserName = user.UserName,
            UserRoles = userRoles.ToList(),
            Roles = rolesViewModel
        };

        return View(userRoleViewModel);
    }

    // POST: Users/ManageRoles/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ManageRoles(string id, UserRoleViewModel model)
    {
        if (id != model.UserId)
        {
            return NotFound();
        }

        var user = await _userService.GetUserById(id);
        if (user == null)
        {
            return NotFound();
        }

        var userRoles = await _userService.GetUserRoles(user);
        var selectedRoles = model.UserRoles ?? new List<string>();

        var rolesToAdd = selectedRoles.Except(userRoles);
        var rolesToRemove = userRoles.Except(selectedRoles);

        IdentityResult result;

        if (rolesToAdd.Any())
        {
            result = await _userService.AddUserToRoles(user, rolesToAdd);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }
        }

        if (rolesToRemove.Any())
        {
            result = await _userService.RemoveUserFromRoles(user, rolesToRemove);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }
        }

        return RedirectToAction(nameof(Index));
    }
}
