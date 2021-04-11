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

namespace HouseOfSoulSounds.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly DataManager dataManager;

        public HomeController(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }

        public IActionResult Index()
        {
            var catalogs = dataManager.Catalogs;

            IQueryable<EditCatalogsModel> model = from n in dataManager.Catalogs.Items
                                                   select new EditCatalogsModel { Id = n.Id, Title = n.Title };

            foreach(var item in model)
            {
                item.InstrumentItems = dataManager.Catalogs.GetInstruments(item.Id);
            }
            

            //model = model.Append(new EditCatalogsModel { Id = default, Title = "" });
            return View(model);
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