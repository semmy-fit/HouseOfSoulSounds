using System.Threading.Tasks;
using HouseOfSoulSounds.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using HouseOfSoulSounds.Models.Identity;
using HouseOfSoulSounds.Models.Domain.Entities;

namespace HouseOfSoulSounds.Controllers
{
    public partial class AccountController
    {
        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View(new LoginModel());
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                User user = await userManager.FindByNameAsync(model.UserName);
                if (user != null)
                {
                    await signInManager.SignOutAsync();
                    Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                    if (result.Succeeded)
                    {
                        if (string.IsNullOrEmpty(returnUrl))
                        {
                            var roles = await userManager.GetRolesAsync(user);
                            if (roles?.Contains(Config.RoleAdmin) == true)
                                returnUrl = "/PersonalArea";
                            else returnUrl = "/";
                        }

                        return Redirect(returnUrl);
                    }
                }
                ModelState.AddModelError(nameof(Models.Identity.LoginModel.UserName), "Неверный логин или пароль");
            }
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            HttpContext.Response.Cookies.Delete(".AspNetCore.Cookies");
            return RedirectToAction("Index", "Home");
        }
    }
}
