using Roulette.Security.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Roulette.Security.Interfaces;
using Roulette.Security.Services;

namespace Roulette.Controllers
{
    public class UserManagementController : Controller
    {
        private IAccountServices _accountServices;
        public UserManagementController()
        {
            _accountServices = new AccountServices();
        }
        // GET: UserManagement
        public ActionResult Index()
        {
            if (Authorisation.AuthToken == null)
                return RedirectToAction("Index", "Login");
            if(!_accountServices.ValidateUserAccess(Authorisation.AuthToken))
                return RedirectToAction("Index", "Home");
            return View();
        }
    }
}