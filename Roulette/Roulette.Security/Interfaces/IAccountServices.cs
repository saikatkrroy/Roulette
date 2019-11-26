
using Roulette.DataAccess.Models;
using Roulette.Security.Models;

namespace Roulette.Security.Interfaces
{
    public interface IAccountServices
    {
        UserSessions CreateNewUserSession(LoginModel user);
        Users CreateNewUser(LoginModel person, bool isEmailUsername = true);
        void Logoff(string authToken);
        string GetAuthTokenForProxyUser(Users user);
        string Login(LoginModel loginModel);
        string HashPassword(string password, string salt);

    }
}
