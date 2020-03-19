using Roulette.Security.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Roulette.Controllers
{
    public class UserManagementController : Controller
    {
        // GET: UserManagement
        public ActionResult Index()
        {
            if (Authorisation.AuthToken == null)
                return RedirectToAction("Index", "Login");
            return View();
        }
    }
}