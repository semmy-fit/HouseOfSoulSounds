using System.Threading.Tasks;
using HouseOfSoulSounds.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using HouseOfSoulSounds.Models.Identity;
using HouseOfSoulSounds.Models.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System.IO;
using System;

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
        public async Task<IActionResult> Login(LoginModel model, string returnUrl, IFormFile titleImageFile)
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
                        string imagePath = default;
                        if (titleImageFile is not null)
                        {
                            //Assigning Unique Filename (Guid)
                            var uniqueFileName = Convert.ToString(Guid.NewGuid());
                            var FileExtension = Path.GetExtension(titleImageFile.FileName);
                            imagePath = uniqueFileName + FileExtension;
                        }
                        if (!string.IsNullOrEmpty(titleImageFile?.FileName))
                        {
                            string path = Path.Combine(Config.AvatarsPath, imagePath);
                            // сохраняем файл в папку Files в каталоге wwwroot
                            using (var fileStream = new FileStream(Config.WebRootPath + path, FileMode.Create))
                            {
                                await titleImageFile.CopyToAsync(fileStream);
                            }

                        }

                        if (string.IsNullOrEmpty(returnUrl))
                        {
                            var roles = await userManager.GetRolesAsync(user);
                            if (roles?.Contains(Config.RoleAdmin) == true)
                                returnUrl = "Account/EditRegister";
                            else returnUrl = "/Account/EditRegister";
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
