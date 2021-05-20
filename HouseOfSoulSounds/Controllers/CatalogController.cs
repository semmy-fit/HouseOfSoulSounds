using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HouseOfSoulSounds.Models.Domain;
using HouseOfSoulSounds.Models.Domain.Entities;
using HouseOfSoulSounds.Areas.Admin.Models;
using HouseOfSoulSounds.Models;
using Microsoft.EntityFrameworkCore;

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
        public IActionResult Index()
        {
            IQueryable<InstrumentItem> model = from b in dataManager.Instruments.Items
                                               select new InstrumentItem { Id = b.Id,CatalogId=b.CatalogId,Title = b.Title, TitleImagePath = b.TitleImagePath, Subtitle = b.Subtitle, Text = b.Text };

            IQueryable<EditCatalogsModel> model_catalog = from n in dataManager.Catalogs.Items
                                                  select new EditCatalogsModel { Id = n.Id, Title = n.Title };
            //var entity = new InstrumentItem {};
            foreach (var d in model_catalog)
            {
                //d.Id.ToString();
                //d.Title.ToString();
                d.InstrumentItems = dataManager.Catalogs.GetInstruments(d.Id);
            }
            foreach (var item in model)
            {
                item.Title.ToString();
                dataManager.Instruments.Items.Select(z => z.TitleImagePath == item.TitleImagePath);
                item.Subtitle.ToString();
                item.Text.ToString();

            }

        
            var data = new CatalogViewModel();
            data.editCatalogs = model_catalog;
            data.Instruments =model;

                return View(data);
        }

        public IActionResult Gutars()
        {
            IQueryable<InstrumentItem> instrumentItems = from b in dataManager.Instruments.Items
                                               select new InstrumentItem { Id = b.Id, CatalogId = b.CatalogId, Title = b.Title, TitleImagePath = b.TitleImagePath, Subtitle = b.Subtitle, Text = b.Text };

            IQueryable<EditCatalogsModel> editCatalogs = from n in dataManager.Catalogs.Items
                                                          select new EditCatalogsModel { Id = n.Id, Title = n.Title };
            //var entity = new InstrumentItem {};
            foreach (var d in editCatalogs)
            {
                //d.Id.ToString();
                //d.Title.ToString();
                d.InstrumentItems = dataManager.Catalogs.GetInstruments(d.Id);
            }
            foreach (var item in instrumentItems)
            {
                item.Title.ToString();
                dataManager.Instruments.Items.Select(z => z.TitleImagePath == item.TitleImagePath);
                item.Subtitle.ToString();
                item.Text.ToString();

            }


            var data = new CatalogViewModel();
            data.editCatalogs = editCatalogs;
            data.Instruments = instrumentItems;
            return View(data);
        }

        public IActionResult Flute()
        {
            IQueryable<InstrumentItem> items = from b in dataManager.Instruments.Items
                                               select new InstrumentItem { Id = b.Id, CatalogId = b.CatalogId, Title = b.Title, TitleImagePath = b.TitleImagePath, Subtitle = b.Subtitle, Text = b.Text };

            IQueryable<EditCatalogsModel> editCatalogs = from n in dataManager.Catalogs.Items
                                                          select new EditCatalogsModel { Id = n.Id, Title = n.Title };
            //var entity = new InstrumentItem {};
            foreach (var d in editCatalogs)
            {
                //d.Id.ToString();
                //d.Title.ToString();
                d.InstrumentItems = dataManager.Catalogs.GetInstruments(d.Id);
            }
            foreach (var item in items)
            {
                item.Title.ToString();
                dataManager.Instruments.Items.Select(z => z.TitleImagePath == item.TitleImagePath);
                item.Subtitle.ToString();
                item.Text.ToString();

            }


            var data = new CatalogViewModel();
            data.editCatalogs = editCatalogs;
            data.Instruments = items;
            return View(data);
        }
        public IActionResult Saxophone()
        {
            IQueryable<InstrumentItem> instruments = from b in dataManager.Instruments.Items
                                               select new InstrumentItem { Id = b.Id, CatalogId = b.CatalogId, Title = b.Title, TitleImagePath = b.TitleImagePath, Subtitle = b.Subtitle, Text = b.Text };

            IQueryable<EditCatalogsModel> editCatalogs = from n in dataManager.Catalogs.Items
                                                          select new EditCatalogsModel { Id = n.Id, Title = n.Title };
            //var entity = new InstrumentItem {};
            foreach (var d in editCatalogs)
            {
                //d.Id.ToString();
                //d.Title.ToString();
                d.InstrumentItems = dataManager.Catalogs.GetInstruments(d.Id);
            }
            foreach (var item in instruments)
            {
                item.Title.ToString();
                dataManager.Instruments.Items.Select(z => z.TitleImagePath == item.TitleImagePath);
                item.Subtitle.ToString();
                item.Text.ToString();

            }


            var data = new CatalogViewModel();
            data.editCatalogs = editCatalogs;
            data.Instruments = instruments;
            return View(data);
        }
    }
}

