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
            string host = System.Web.HttpContext.Current.Request.Url.Authority;
            String url ="http://" + host + "/Home/Index";
            var authToken=_accountServices.Login(loginModel);
            
            if(loginModel.Username.ToLower().Contains("admin"))
                url = "http://" + host + "/UserManagement/Index";
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

        [HttpGet]
        [Route("api/Account/RetrieveUsers")]
        public IList<String> RetrieveUsers()
        {
            return _accountServices.RetrieveUsers();
        }
        [HttpDelete]
        [Route("api/Account/DeleteUser/{userId}")]
        public void DeleteUser([FromUri] string userId)
        {
             _accountServices.DeleteUser(userId);
        }
    }
}