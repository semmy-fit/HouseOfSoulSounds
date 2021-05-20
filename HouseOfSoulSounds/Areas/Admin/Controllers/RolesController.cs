using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using HouseOfSoulSounds.Areas.Admin.Models;
using HouseOfSoulSounds.Helpers;
using HouseOfSoulSounds.Models;
using static HouseOfSoulSounds.Helpers.Config;
using HouseOfSoulSounds.Models.Domain.Entities;

namespace HouseOfSoulSounds.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;

        public RolesController(
            RoleManager<IdentityRole> roleManager, 
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

       public IActionResult Index() => View(userManager.Users.OrderBy(u => u.UserName));

       public async Task<IActionResult> Edit(string id)
        {
            User user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                var userRoles = await userManager.GetRolesAsync(user);
                var allRoles = roleManager.Roles.ToList();
                ChangeRoleModel model = new ChangeRoleModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    UserRoles = userRoles,
                    AllRoles = allRoles,
                    Blocked = user.Blocked ?? false

                };
                return View(model);
            }
            ModelState.AddModelError(String.Empty,
                "Выбранного пользователя больше не существует");
            return null;
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, List<string> roles)
        {
            User user = await userManager.FindByIdAsync(id);
            if (user != null)
            {

                var userRoles = await userManager.GetRolesAsync(user);
                var _roles = roles.Where(y => y != Blocked);
                var addRoles = _roles.Except(userRoles);
                var blocked = roles.Contains(Blocked);
                user.Blocked = blocked;
                await userManager.UpdateAsync(user);
              
                
                var removeRoles = userRoles.Except(_roles);
                bool add = addRoles.Any();
                bool remove = removeRoles.Any();

                if (add || remove)
                {
                    var isNotAdmin = await IsNotAdmin();
                    if (!(isNotAdmin is BadRequestResult))
                        return isNotAdmin;
                }

                if (add)
                {
                    if (!user.EmailConfirmed && 
                        (addRoles.Contains(RoleModer) ))
                    {
                        user.EmailConfirmed = true;
                        await userManager.UpdateAsync(user);
                    }
                    await userManager.AddToRolesAsync(user, addRoles);
                }
                if (remove) 
                {
                    if(removeRoles.Contains(RoleAdmin))
                    {
                        if(user.UserName == Config.Admin)
                        {
                            ModelState.AddModelError(String.Empty,
                                $"У пользователя \"{Config.Admin}\" роль \"{RoleAdmin}\" зарезервировано сервером, его нельзя удалить. Однако, другие роли можно удалять.");
                            return await Edit(id);
                        }
                        
                        if (User.Identity.Name == user.UserName)
                        {
                            await userManager.RemoveFromRolesAsync(user, removeRoles);
                            await signInManager.RefreshSignInAsync(user);
                            return RedirectToAction(
                                "Info",
                                "Home", 
                                new InfoModel
                                {
                                    Text = $"Вы лишили себя роли {RoleAdmin}"
                                });
                        }
                    }

                    await userManager.RemoveFromRolesAsync(user, removeRoles);
                }

                if (User.Identity.Name == user.UserName && (add || remove)) 
                    await signInManager.RefreshSignInAsync(user);
            }
            else
            {
                ModelState.AddModelError(string.Empty,
                    "Выбранного пользователя больше не существует");
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(string id)
        {
            User user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                var isNotAdmin = await IsNotAdmin();
                if (!(isNotAdmin is BadRequestResult))
                    return isNotAdmin;

                if (user.UserName == Config.Admin)
                {
                    ModelState.AddModelError(String.Empty,
                        $"Пользователь \"{Config.Admin}\" зарезервирован сервером, его нельзя удалить.");
                    return RedirectToAction("Index");
                }
                await userManager.DeleteAsync(await userManager.FindByIdAsync(id));
            }
            else
            {
                ModelState.AddModelError(string.Empty,
                    "Выбранного пользователя больше не существует");
            }
            return RedirectToAction("Index");
        }

        private async Task<IActionResult> IsNotAdmin()
        {
            var currentUser = await userManager.FindByNameAsync(User.Identity.Name);
            if (currentUser == null)
            {
                await signInManager.SignOutAsync();
                HttpContext.Response.Cookies.Delete(".AspNetCore.Cookies");

                return RedirectToAction(
                    "Info",
                    "Home",
                    new InfoModel
                    {
                        Text = "Ваш аккаунт был удалён"
                    });
            }

            var isAdmin = await userManager.GetRolesAsync(currentUser);

            if (!isAdmin.Any(x => x == RoleAdmin))
            {
                await signInManager.RefreshSignInAsync(currentUser);
                return RedirectToAction(
                    "Info",
                    "Home",
                    new InfoModel
                    {
                        Text = $"У Вас нет больше роли {RoleAdmin}, чтобы изменять права пользователям"
                    });
            }

            return BadRequest();
        }
    }
}
