using System.Threading.Tasks;
using HouseOfSoulSounds.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using HouseOfSoulSounds.Models.Identity;
using HouseOfSoulSounds.Models;
using HouseOfSoulSounds.Models.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using HouseOfSoulSounds.Models.Domain;
using System.Linq;
using System.Net.Http.Headers;
using System;

namespace HouseOfSoulSounds.Controllers
{
    [Authorize]
    public partial class AccountController
    {
        EFAppDbContext _context;
        IWebHostEnvironment _appEnvironment;

        

        [AllowAnonymous]
        public async Task<IActionResult> Register(string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated)
                return await EditRegister(returnUrl);
            ViewBag.returnUrl = returnUrl;
            return View(new RegisterModel());
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterModel model, IFormFile titleImageFile)
        { 
            if (ModelState.IsValid)
            {
                string imagePath = default;
                if (titleImageFile is not null)
                {
                    //Assigning Unique Filename (Guid)
                    var uniqueFileName = Convert.ToString(Guid.NewGuid());
                    var FileExtension = Path.GetExtension(titleImageFile.FileName);
                    imagePath = uniqueFileName + FileExtension;
                }
                User user = new()
                {
                    Email = model.Email,
                    UserName = model.UserName
                };

                
                var result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(titleImageFile?.FileName))
                    {
                        user = await userManager.FindByNameAsync(model.UserName);
                        if (user is not null)
                        {
                            user.ImagePath = imagePath;
                            result = await userManager.UpdateAsync(user);
                            if (result.Succeeded)
                            {
                                string path = Path.Combine(Config.AvatarsPath, imagePath);
                                // сохраняем файл в папку Files в каталоге wwwroot
                                using (var fileStream = new FileStream(Config.WebRootPath + path, FileMode.Create))
                                {
                                    await titleImageFile.CopyToAsync(fileStream);
                                }
                            }
                        }
                    }

                    // установка куки
                    await signInManager.SignInAsync(user, false);

                    return await SendConfirmEmail(user);
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
                            break;
                        case "InvalidToken":
                            error.Description = "Код устарел, запросите подтверждение повторно";
                            break;
                    }

                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        public async Task<IActionResult> RegisterEmailConfirm()
        {
            var user = await userManager.GetUserAsync(User);
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

            return await SendConfirmEmail(user);
        }


        [HttpPost]
        public async Task<IActionResult> AddFile(IFormFile uploadedFile)
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
                User file = new User {ImagePath = path };
                _context.Users.Add(file);
                _context.SaveChanges();
            }

            return RedirectToAction("Admin/Home/Index");
        }
    }
}
