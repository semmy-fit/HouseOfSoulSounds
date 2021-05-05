using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HouseOfSoulSounds.Areas.Admin.Models;
using HouseOfSoulSounds.Models.Domain;
using HouseOfSoulSounds.Models.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System.IO;
using HouseOfSoulSounds.Helpers;
using Microsoft.EntityFrameworkCore;

namespace HouseOfSoulSounds.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CreatePageController : Controller
    {
        private readonly EFAppDbContext _context;

        private readonly DataManager dataManager;

        public CreatePageController(DataManager dataManager,EFAppDbContext context)
        {
            this.dataManager = dataManager;
            this._context = context;

        }
        [HttpPost]
        public IActionResult Edit(Guid id,NewPage page)
        {

            //var page = dataManager.Pages.Items.FirstOrDefault(c => c.Id == id);
           // var pages = dataManager.Pages.Items.Where(z=>z.Id==id);
            var entity = new NewPage() { Id = id,PageTitle=page.PageTitle,ImagePage=page.ImagePage,
                BaseText = page.BaseText,
                Saidbar_text =page.Saidbar_text}; 
            return View(entity);
    
         
        }
        public IActionResult Addname(string path_s,string str1, string str2, string[] lines)
        {
            
            string id = "";
            string authData = $"path_s: {path_s}";
            switch (path_s)
            {
                case "01":
                    string path = @"E:\Projects\semmy-fit\HouseOfSoulSounds\HouseOfSoulSounds\Areas\Admin\Controllers\" + str1;
                    path_s = path;
                    break;
                case "02":
                    string model_path = @"E:\Projects\semmy-fit\HouseOfSoulSounds\HouseOfSoulSounds\Areas\Admin\Models\" + str1;
                    path_s = model_path;
                    break;
                case "03":
                    var dir = Directory.CreateDirectory(@"E:\Projects\semmy-fit\HouseOfSoulSounds\HouseOfSoulSounds\Areas\Admin\Views\" + str2);
                    string view_page = dir + @"\" + str1;
                        path_s = view_page;
                     
                    break;
                };
            
            try
            {
                //switch (path)
                //{
                //    case "path":
                //        StreamWriter sw1 = new StreamWriter(path);
                //        string[] lines = new string[] { "using Microsoft.AspNetCore.Mvc;", "Строка2", "Строка3" };
                //        sw1.WriteLine(lines);
                //        break;
                //    case "model_path":
                //        StreamWriter sw2 = new StreamWriter(model_path);
                //        break;
                //    case "view_page":
                //        StreamWriter sw3 = new StreamWriter(view_page);
                //        break;

                //}
                //Pass the filepath and filename to the StreamWriter Constructor
                StreamWriter sw = new StreamWriter(path_s, true);
                //Write a line of text
            //    string[] lines = new string[] { "using Microsoft.AspNetCore.Mvc;", "using System;",
            //        "using System.Collections.Generic;","using System.Threading.Tasks;",
            //        "using System.Linq;","namespace HouseOfSoulSounds.Areas.Admin.Controllers {",
            //        "public class "+str2+": Controller {","public IActionResult Index() { return View();} }","}"



            //};
                foreach (string str in lines)
                {
                    sw.WriteLine(str);
                }
                //Close the file
                sw.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }

            return View(); 
        }
            public IActionResult Index(Guid id)
        {
            IQueryable<NewPage> pages = from m in dataManager.Pages.Items
                                        select new NewPage { Id = m.Id, PageTitle = m.PageTitle, BaseText = m.BaseText, ImagePage = m.ImagePage, Saidbar_text = m.Saidbar_text };
            foreach (var item in pages)
            {
                item.PageTitle.ToString();
                item.ImagePage.ToString();
                item.BaseText.ToString();
                item.Saidbar_text.ToString();

            }
            return View(pages);
        }
        [HttpPost]
        public async Task<IActionResult> NewPage(NewPage _page, IFormFile titleImageFile1)
        {
            if (ModelState.IsValid)
            {

                string imagePath3 = default;
                if (titleImageFile1 is not null)
                {
                    //Assigning Unique Filename (Guid)
                    var uniqueFileName = Convert.ToString(Guid.NewGuid());
                    var FileExtension = Path.GetExtension(titleImageFile1.FileName);
                    imagePath3 = uniqueFileName + FileExtension;
                }
                if (!string.IsNullOrEmpty(titleImageFile1?.FileName))
                {
                    if (titleImageFile1 is not null)
                    {
                        _page.ImagePage = imagePath3;
                        string path = Path.Combine(Config.ImagePagePath, imagePath3);
                        // сохраняем файл в папку Files в каталоге wwwroot
                        using (var fileStream = new FileStream(Config.WebRootPath + path, FileMode.Create))
                        {
                            await titleImageFile1.CopyToAsync(fileStream);
                        }
                    }
                }
            }
            var p = new NewPage
            {
                Id = _page.Id,
                PageTitle = _page.PageTitle,
                ImagePage = _page.ImagePage,
                BaseText = _page.BaseText,
                Saidbar_text = _page.Saidbar_text

            };
            dataManager.Pages.SaveItem(p);
            _context.SaveChanges();
            return RedirectToAction("", "", new { Areas = "Admin" });
        }

        [HttpPost]
        public async Task<IActionResult> AddFile3(IFormFile uploadedFile)
        {
            if (uploadedFile != null)
            {
                // путь к папке Files
                string path = "/Pages/" + uploadedFile.FileName;
                // сохраняем файл в папку Files в каталоге wwwroot
                using (var fileStream = new FileStream(Path.Combine(Config.WebRootPath, path), FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
                NewPage file = new NewPage{ ImagePage = path };
                _context.Pages.Add(file);
                _context.SaveChanges();
            }

            return RedirectToAction("Admin/Home/Index");
        }
    }
}
