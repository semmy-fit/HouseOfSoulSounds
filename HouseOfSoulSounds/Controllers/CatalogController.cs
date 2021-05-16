using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HouseOfSoulSounds.Models.Domain;
using HouseOfSoulSounds.Models.Domain.Entities;

namespace HouseOfSoulSounds.Controllers
{
    public class CatalogController : Controller
    {
        private readonly EFAppDbContext _context;

        private readonly DataManager dataManager;

        public CatalogController(DataManager dataManager, EFAppDbContext context)
        {
            this.dataManager = dataManager;
            this._context = context;
        }
        public IActionResult Index(Guid id)
        {
            IQueryable<InstrumentItem> model = from b in dataManager.Instruments.Items
                                               select new InstrumentItem { Id = b.Id, Title = b.Title, TitleImagePath = b.TitleImagePath, Subtitle = b.Subtitle, Text = b.Text };

            var entity = new InstrumentItem { Id = id, CatalogId = id };
            foreach (var item in model)
            {
                item.Title.ToString();
                dataManager.Instruments.Items.Select(z => z.TitleImagePath == item.TitleImagePath);
                item.Subtitle.ToString();
                item.Text.ToString();

            }

            return View(model);
        }
    }
}
