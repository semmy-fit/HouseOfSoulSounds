﻿using Microsoft.AspNetCore.Mvc;
using HouseOfSoulSounds.Models.Domain;

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
            return View(dataManager.Instruments.Items);
        }
    }
}