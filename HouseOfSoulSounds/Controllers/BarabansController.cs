﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace HouseOfSoulSounds.Controllers
{
    public class BarabansController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}