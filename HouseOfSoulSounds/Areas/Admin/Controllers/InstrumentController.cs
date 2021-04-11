using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HouseOfSoulSounds.Models.Domain;
using HouseOfSoulSounds.Models.Domain.Entities;
using HouseOfSoulSounds.Helpers;

namespace HouseOfSoulSounds.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class InstrumentController : Controller
    {
        private readonly DataManager dataManager;
        public InstrumentController(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }
        [HttpPost]
        public IActionResult Edit(Guid id)
        {
            if (dataManager.Catalogs.GetItemById(id) is not null)
            {
                var entity = new InstrumentItem() { CatalogId = id };
                return View(entity);
            }
            return null;
        }

      
        public IActionResult Delete(Guid id)
        {
            dataManager.Instruments.DeleteItem(id);
            return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).CutController());
        }
    }
}