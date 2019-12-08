using Roulette.DataAccess.Interfaces;
using Roulette.DataAccess.Models;
using Roulette.Security.Helpers;
using Roulette.Security.Interfaces;
using Roulette.Security.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Results;

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
        public object Login(LoginModel loginModel)
        {
            var authToken=_accountServices.Login(loginModel);
            
            string host = System.Web.HttpContext.Current.Request.Url.Authority;
            var url = "http://" + host + "/Home/Index";
            Authorisation.AuthToken = authToken;
            return Request.CreateResponse(HttpStatusCode.OK,new { RedirectUrl=url,AuthToken=authToken});
        }
        [HttpPost]
        [Route("api/Account/Logoff")]
        public object Logoff()
        {
            
            _accountServices.Logoff(Authorisation.AuthToken);
            string host = System.Web.HttpContext.Current.Request.Url.Authority;
            var url = "http://" + host + "/Login/Index";
            Authorisation.AuthToken = null;
            return Request.CreateResponse(HttpStatusCode.OK, new { RedirectUrl = url });
        }
        [HttpPost]
        [Route("api/Account/CreateNewUser")]
        public Users CreateNewUser(LoginModel loginModel)
        {
            return _accountServices.CreateNewUser(loginModel);
        }
    }
}