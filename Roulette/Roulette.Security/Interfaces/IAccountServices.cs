
using Roulette.DataAccess.Models;
using Roulette.Security.Models;
using System;
using System.Collections.Generic;

namespace Roulette.Security.Interfaces
{
    public interface IAccountServices
    {
        UserSessions CreateNewUserSession(int id,LoginModel user);
        Users CreateNewUser(LoginModel person, bool isEmailUsername = true);
        void Logoff(string authToken);
        string GetAuthTokenForProxyUser(Users user);
        string Login(LoginModel loginModel);
        string HashPassword(string password, string salt);
        IList<String> RetrieveUsers();
        void DeleteUser(string userId); 
    }
}
