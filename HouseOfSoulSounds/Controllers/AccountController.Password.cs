using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HouseOfSoulSounds.Models.Identity;
using HouseOfSoulSounds.Helpers;
using HouseOfSoulSounds.Models;
using Microsoft.AspNetCore.Identity;
using HouseOfSoulSounds.Models.Domain.Entities;
using EmailService = HouseOfSoulSounds.Helpers.EmailService;

namespace HouseOfSoulSounds.Controllers
{
    public partial class AccountController
    {
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword(string email)
        {
            if (!string.IsNullOrEmpty(email))
                ViewBag.Email = email;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty,
                        $"\"{model.Email}\" не найден в базе данных. Проверьте корректность или пройдите регистрацию.");
                    return View(model);
                }
                return await PasswordEmailConfirm(user);
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code)
        {
            if (code is null)
               return RedirectToAction("Info", "Home", new InfoModel
                    {
                        Text = "Приносим извинения: повторите попытку."
                    });
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty,
                    $"\"{model.Email}\" не найден в базе данных. Проверьте корректность или пройдите регистрацию.");
                return View(model);
            }

            var result = await userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("Info", "Home", new InfoModel
                {
                    Title = "Смена пароля",
                    Text = "Пароль был успешно изменён!"
                });
            }
            foreach (var error in result.Errors)
            {
                switch (error.Code)
                {
                    case "InvalidToken":
                        return await PasswordEmailConfirm(user, true);
                }
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }

        async Task<IActionResult> PasswordEmailConfirm(User user, bool obsolete = false)
        {
            var code = await userManager.GeneratePasswordResetTokenAsync(user);
            var admin = await userManager.FindByNameAsync(Config.Admin);
            var callbackUrl = Url.Action("ResetPassword", "Account", new { UserId = user.Id, code }, protocol: HttpContext.Request.Scheme);
            var res = await EmailService.SendEmailAsync(
                user.UserName,
                user.Email,
                admin.Email,
                "Смена пароля",
                $"Для сброса пароля пройдите по ссылке: <a href='{callbackUrl}'>ссылка</a>");

            if (string.Empty == res)
                return RedirectToAction("Info", "Home", new InfoModel
                {
                    Title = obsolete ? "Код устарел": "Проверьте почту",
                    Text = "Для сброса пароля перейдите по ссылке в письме, отправленном на Ваш email."
                });
            return RedirectToAction("Info", "Home", new InfoModel
                {
                    Text = $"Возникли трудности: {res} Повторите попытку позже."
                });
        }
    }
}
