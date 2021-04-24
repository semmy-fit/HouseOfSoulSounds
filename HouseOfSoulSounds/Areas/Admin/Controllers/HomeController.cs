using Microsoft.AspNetCore.Mvc;
using HouseOfSoulSounds.Models.Domain;
using System.Threading.Tasks;
using HouseOfSoulSounds.Models.Identity;
using HouseOfSoulSounds.Models.Domain.Entities;
using HouseOfSoulSounds.Areas.Admin.Models;
using System.Linq;
using System.Collections.Generic;
using System;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;
using Microsoft.AspNetCore.Http;
using System.IO;
using HouseOfSoulSounds.Helpers;
using HouseOfSoulSounds.Models.Domain.Repositories.EntityFramework;
using Microsoft.AspNetCore.Identity;

namespace HouseOfSoulSounds.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly EFAppDbContext _context;

        private readonly DataManager dataManager;

        public HomeController(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }

        public IActionResult Index()
        {

            // var catalogs = dataManager.Catalogs;

            IQueryable<EditCatalogsModel> model = from n in dataManager.Catalogs.Items
                                                  select new EditCatalogsModel { Id = n.Id, Title = n.Title };
            IQueryable<InstrumentItem> inst_model = from x in dataManager.Instruments.Items
                                                    select new InstrumentItem { CatalogId=x.CatalogId,Title = x.Title };


            //ViewModel two_model = new ViewModel {editCatalogs=model,editInstruments=inst_model };

            //var catalog = dataManager.Catalogs.Items;
            //catalog.AsQueryable();
            //var instrument = dataManager.Instruments.Items;
            //instrument.AsQueryable();
            //var two_model = new ViewModel {editCatalogs= (IQueryable<EditCatalogsModel>)catalog,editInstruments= (IQueryable<InstrumentItem>)instrument };

            foreach (var item in model)
            {
                item.InstrumentItems = dataManager.Catalogs.GetInstruments(item.Id);

            }


                foreach (var d in inst_model)
                {
                    d.Title.ToString();

                }
            
            var data = new ViewModel();
            data.editCatalogs = model.AsQueryable();
            data.editInstruments = inst_model.AsQueryable();
            
           

            //model = model.Append(new EditCatalogsModel { Id = default, Title = "" });
            return View(data);
        } 
        

        public IActionResult GetInstrumentAll()
        {

            IQueryable<InstrumentItem> inst_model = from x in dataManager.Instruments.Items
                                                                       select new InstrumentItem { Title = x.Title };
            //var instruments = inst_model.AsQueryable().Select(x => x.Title == x.Title);

            foreach (var item in inst_model)
            {
                var it = item.Title;

            }
         
            return View(inst_model);
        }

        public IActionResult DeleteInstrument(Guid id)
        {
            IQueryable<InstrumentItem> instrumentItems = from x in dataManager.Instruments.Items
                                                         select new InstrumentItem { Id = x.Id, CatalogId = x.Id };
            foreach (var item in instrumentItems)
            {
                id = item.Id;


            }

            var data = new ViewModel();
             //data.editCatalogs = (IQueryable<EditCatalogsModel>)instrumentItems.AsQueryable();
            dataManager.Instruments.DeleteItem(id);
            return RedirectToAction("", "", new { Areas = "Admin" });
        }
        [HttpPost]
        public IActionResult NewCatalog(Catalog Catalog)
        {

            //  Catalog.Title = newCatalog;
            dataManager.Catalogs.SaveItem(Catalog);
            return RedirectToAction("Index");
        }
 
        public IActionResult DeleteCatalog(Guid id)
        {

            dataManager.Catalogs.DeleteItem(id);
            return RedirectToAction("Index");
        }
    }
}