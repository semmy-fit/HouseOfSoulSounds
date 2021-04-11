using System.Threading.Tasks;
using HouseOfSoulSounds.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HouseOfSoulSounds.Models.Identity;
using HouseOfSoulSounds.Models;
using System.IO;
using System.Linq;
using System;
using Microsoft.AspNetCore.Http;
using HouseOfSoulSounds.Models.Domain.Entities;

namespace HouseOfSoulSounds.Controllers
{
    [Authorize]
    public partial class AccountController
    {
        public async Task<IActionResult> EditRegister(string returnUrl)
        {
            var user = await userManager.GetUserAsync(User);
            if (user is null)
            {
                string name = User.Identity.Name;
                await signInManager.SignOutAsync();
                HttpContext.Response.Cookies.Delete(".AspNetCore.Cookies");
                return RedirectToAction("Info", "Home",
                    new InfoModel
                    {
                        Title = $"{name} не найден",
                        Text = "Приносим извинения: возможно Ваш Аккаунт был удалён или заблокирован модератором."
                    });
            }

            ViewBag.returnUrl = returnUrl;
            return View(new EditRegisterModel
            {
                UserName = user.UserName,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                ImagePath = user.ImagePath 
            });
        }

        [HttpPost]
        public async Task<IActionResult> EditRegister(EditRegisterModel model, IFormFile titleImageFile)
        {
            if (!User.Identity.IsAuthenticated)
                return await Register(ViewBag.returnUrl ?? "/");

            var user = await userManager.GetUserAsync(User);
            //var images = Directory.EnumerateFiles(model.TitleImagePath)
            //                .Select(fn => "ImagePath" + Path.GetFileName(fn));

            if (user is null)
            {
                await signInManager.SignOutAsync();
                HttpContext.Response.Cookies.Delete(".AspNetCore.Cookies");
                return RedirectToAction("Info", "Home",
                    new InfoModel
                    {
                        Title = "Пользователь не найден",
                        Text = "Приносим извинения: возможно Ваш Аккаунт был удалён или заблокирован модератором."
                    });
            }

            if (ModelState.IsValid)
            {
                string imagePath1 = default;
                if (titleImageFile is not null)
                {
                    //Assigning Unique Filename (Guid)
                    var uniqueFileName = Convert.ToString(Guid.NewGuid());
                    var FileExtension = Path.GetExtension(titleImageFile.FileName);
                    imagePath1 = uniqueFileName + FileExtension;
                }
                bool todoEmail = false, todoName = false;
                if (user.NormalizedEmail != model.Email.ToUpper())
                {
                    todoEmail = true;
                    user.Email = model.Email;
                    user.EmailConfirmed = false;
                }

                if (user.UserName != model.UserName)
                {
                    if (Config.Admin.ToUpper() == user.UserName.ToUpper())
                    {
                        ModelState.AddModelError(string.Empty,
                            $"Имя \"{Config.Admin}\" зарезервировано сервером, его нельзя изменять. Однако, пароль рекомендуется поменять, а email можно изменить.");
                        return View(model);
                    }
                    todoName = true;
                    user.UserName = model.UserName;
                }

                if (todoName || todoEmail)
                {
                    var result = await userManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {
                        if (!string.IsNullOrEmpty(titleImageFile?.FileName))
                        {
                            user = await userManager.FindByNameAsync(model.UserName);
                            if (user is not null)
                            {
                                user.ImagePath = imagePath1;
                                result = await userManager.UpdateAsync(user);
                             
                                    string path = Path.Combine(Config.AvatarsPath, imagePath1);
                                    // сохраняем файл в папку Files в каталоге wwwroot
                                    using (var fileStream = new FileStream(Config.WebRootPath + path, FileMode.Create))
                                    {
                                        await titleImageFile.CopyToAsync(fileStream);
                                    }
                                
                            }
                        }

                        if (todoName)
                            await signInManager.SignInAsync(user, false);
                        if (todoEmail && !user.EmailConfirmed)
                            return await SendConfirmEmail(user);
                        return Redirect(ViewBag.returnUrl ?? "/");
                    }

                    foreach (var error in result.Errors)
                    {
                        switch (error.Code)
                        {
                            case "InvalidEmail":
                                error.Description = "Не верно указан почтовый адрес";
                                break;
                            case "InvalidUserName":
                                error.Description = "Логин может состоять только из латинских букв и цифр";
                                break;
                            case "DuplicateUserName":
                                error.Description = "Пользователь с таким именем уже существует";
                                break;
                            case "DuplicateEmail":
                                error.Description = "Почтовый адрес привязан к другому пользователю";
                                model.Email = "";
                                break;
                        }
                        ModelState.AddModelError(string.Empty, error.Description);
                        return View(model);
                    }
                }

                return Redirect(ViewBag.returnUrl ?? "/");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddFile1(IFormFile uploadedFile)
        {
            if (uploadedFile != null)
            {
                // путь к папке Files
                string path = "/Users/" + uploadedFile.FileName;
                // сохраняем файл в папку Files в каталоге wwwroot
                using (var fileStream = new FileStream(Path.Combine(Config.WebRootPath, path), FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
                User file = new User { ImagePath = path };
                _context.Users.Add(file);
                _context.SaveChanges();
            }

            return RedirectToAction("Account/EditRegister");
        }
    }
}
