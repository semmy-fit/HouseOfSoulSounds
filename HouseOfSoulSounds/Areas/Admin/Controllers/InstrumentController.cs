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
using System.ComponentModel.DataAnnotations.Schema;

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

      


        //[HttpPost]
        //public  IActionResult Edit(Guid id,InstrumentItem instrument)
        //{

        //   // var catalog = dataManager.Catalogs.Items.FirstOrDefault(c => c.Id == id);           
        //       // var instrumentCatalog = context.InstrumentItems.Include(z => z.Title == z.Title);
        //        var entity = new InstrumentItem() {CatalogId = id, Catalog = instrument.Catalog };

        //        return View(entity);
         
        //}
        

        public  IActionResult Edit(Guid id)
        {
            if (id == default)
                return null;
             var catalog = dataManager.Catalogs.GetItemById(id);
            // var instrumentCatalog = context.InstrumentItems.Include(z => z.Title == z.Title);
            if (catalog is null)

            { 
                var data = dataManager.Instruments.GetItemById(id);
                return View(data);
            }

            //InstrumentItem entity = new() { CatalogId = id, Catalog = catalog };
            // var entity = dataManager.Instruments.GetItemById(id);

            var newins = new InstrumentItem() { CatalogId = id };
            return View(newins);
        }

        //[HttpPost]
        //public IActionResult Edit1(Guid id,InstrumentItem isnt)
        //{
        //     var catalog = dataManager.Catalogs.Items.FirstOrDefault(c => c.Id == id);
        //    // var catalog = dataManager.Instruments.Items.FirstOrDefault(c => c.Id == id);

        //    //if (dataManager.Catalogs.GetItemById(id) is not null)
        //    //{

        //    if (catalog is not null)
        //    {
        //        var instrumentCatalog1 = context.InstrumentItems.Include(z => z.Title == z.Title);

        //        var ent = new InstrumentItem() { CatalogId = id, Catalog = catalog };


        //        return RedirectToAction("Edit","Instrument",catalog);

        //    }




        //    return null;
        //}

        [HttpPost]
        public async Task<IActionResult> NewInstrument(InstrumentItem model,IFormFile titleImageFile)
        {      
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
                if (model.Id == model.CatalogId) model.Id = default;
                if (!string.IsNullOrEmpty(titleImageFile?.FileName))
                {
                    if (titleImageFile is not null)
                    {
                        model.TitleImagePath = imagePath2;
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

            dataManager.Instruments.SaveItem(model);

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