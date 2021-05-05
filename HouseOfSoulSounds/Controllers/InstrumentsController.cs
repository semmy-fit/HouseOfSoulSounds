using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HouseOfSoulSounds.Models.Domain;
using HouseOfSoulSounds.Models.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace HouseOfSoulSounds.Controllers
{
    public class InstrumentsController : Controller
    {
        public DataManager _dataManager { get; set; }
        public EFAppDbContext _context { get; set; }
        public InstrumentsController(DataManager dataManager,EFAppDbContext context)
        {
            this._dataManager = dataManager;
            this._context = context;
        }
        public IActionResult Index(Guid id)
        {
            if(id != default)
            {
                var model = _dataManager.Instruments.GetItemById(id);

                return View("Show", model);
            }

            return View();
        }
        public IActionResult Welcome()
        {
            return View("gutar");
        }

    }
}
