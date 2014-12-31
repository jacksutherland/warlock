using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Warlock.Models;

namespace Warlock.Controllers
{
    public class HomeController : Controller
    {
        private DataContext db = new DataContext();

        public ActionResult Index()
        {
            return RedirectToAction("Index", "Series");
            //return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
