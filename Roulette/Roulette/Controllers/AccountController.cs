using Roulette.DataAccess.Models;
using Roulette.Security.Interfaces;
using Roulette.Security.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Roulette.Controllers
{
    public class AccountController : ApiController
    {
        IAccountServices _accountServices;
        public AccountController(IAccountServices accountServices)
        {
            _accountServices = accountServices;
        }
        [HttpPost]
        [Route("api/Account/Login")]
        public string Login(LoginModel loginModel)
        {
            return _accountServices.Login(loginModel);
        }
        [HttpPost]
        [Route("api/Account/Logoff")]
        public void Logoff(string authToken)
        {
            _accountServices.Logoff(authToken);
        }
        [HttpPost]
        [Route("api/Account/CreateNewUser")]
        public Users CreateNewUser(LoginModel loginModel)
        {
            return _accountServices.CreateNewUser(loginModel);
        }
    }
}