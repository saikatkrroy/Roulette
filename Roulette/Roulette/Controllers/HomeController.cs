using Roulette.Security.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Roulette.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Authorisation.AuthToken == null)
                return RedirectToAction("Index", "Login");
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
    }
}