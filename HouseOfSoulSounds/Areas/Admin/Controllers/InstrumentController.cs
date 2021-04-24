using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HouseOfSoulSounds.Models.Domain;
using HouseOfSoulSounds.Models.Domain.Entities;
using HouseOfSoulSounds.Helpers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using HouseOfSoulSounds.Areas.Admin.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace HouseOfSoulSounds.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class InstrumentController : Controller
    {
        private readonly EFAppDbContext context;
        
        private readonly DataManager dataManager;
        public InstrumentController(DataManager dataManager,EFAppDbContext context)
        {
            this.dataManager = dataManager;
            this.context = context;
        }
        [HttpPost]
        public  IActionResult Edit(Guid id,InstrumentItem inst)
        {
            //IQueryable<EditCatalogsModel> model = from n in dataManager.Catalogs.Items
            //                                      select new EditCatalogsModel { Id = n.Id, Title = n.Title };


            //if (dataManager.Catalogs.GetItemById(id) is not null)
            //{

        

            var instrumentCatalog = context.InstrumentItems.Include(z=>z.Title==z.Title);
            
            var entity = new InstrumentItem() { CatalogId = id, Catalog = inst.Catalog};
              
                return PartialView(entity);
            //}

            //return null;
        }

        [HttpPost]
        public async Task<IActionResult> NewInstrument(Guid id, Catalog catalog, InstrumentItem instrument, IFormFile titleImageFile)
        {
            //var instr = dataManager.Instruments.GetInstrumentInCatalog(id);
            //if (instr is null)
            //{
            //    string name = instrument.Catalog.Title;
            //    return name;
            //    }
            if (ModelState.IsValid)
            {

                string imagePath2 = default;
                if (titleImageFile is not null)
                {
                    //Assigning Unique Filename (Guid)
                    var uniqueFileName = Convert.ToString(Guid.NewGuid());
                    var FileExtension = Path.GetExtension(titleImageFile.FileName);
                    imagePath2 = uniqueFileName + FileExtension;
                }
                if (!string.IsNullOrEmpty(titleImageFile?.FileName))
                {
                    if (titleImageFile is not null)
                    {
                        instrument.TitleImagePath = imagePath2;
                        string path = Path.Combine(Config.TitleInstrumentPath, imagePath2);
                        // сохраняем файл в папку Files в каталоге wwwroot
                        using (var fileStream = new FileStream(Config.WebRootPath + path, FileMode.Create))
                        {
                            await titleImageFile.CopyToAsync(fileStream);
                        }
                    }
                }
            }
            //if (dataManager.Catalogs.GetItemById(id) is not null)
            //{
            //var X= dataManager.Catalogs.Items.Where(x => x.Title == instrument.Catalog.Title);
            // var d = dataManager.Instruments.Items;

            var entity = new InstrumentItem()
            {
                Catalog = catalog,

                Title = instrument.Title,
                Subtitle = instrument.Subtitle,
                Text = instrument.Text,
                CatalogId = instrument.CatalogId,
                TitleImagePath = instrument.TitleImagePath,
                MetaTitle = instrument.MetaTitle,
                MetaDescription = instrument.MetaDescription,
                MetaKeywords = instrument.MetaKeywords,
                DateAdded = instrument.DateAdded,
                Instruments = instrument.Instruments
            };


            //return View(entity);

            dataManager.Instruments.SaveItem(entity);

            context.SaveChanges();

            //}

            //// var instrument = dataManager.Instruments.GetInstrumentInCatalog();
            //_context.SaveChanges();
            return RedirectToAction("","",new { Areas="Admin"});
        }

        [HttpPost]
        public async Task<IActionResult> AddFile2(IFormFile uploadedFile)
        {
            if (uploadedFile != null)
            {
                // путь к папке Files
                string path = "/InstrumentItems/" + uploadedFile.FileName;
                // сохраняем файл в папку Files в каталоге wwwroot
                using (var fileStream = new FileStream(Path.Combine(Config.WebRootPath, path), FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
                InstrumentItem file = new InstrumentItem { TitleImagePath = path };
                context.InstrumentItems.Add(file);
                 context.SaveChanges();
            }

            return RedirectToAction("Admin/Home/Index");
        }
  
    }
}