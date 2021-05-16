using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseOfSoulSounds.Controllers
{
    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Gitars()
        {
            return View();
        }
        public IActionResult Headphones()
        {
            return View();
        }
        public IActionResult Violins()
        {
            return View();
        }
        public IActionResult Microphones()
        {
            return View();
        }
    }
}
