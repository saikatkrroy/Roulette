
using Roulette.DataAccess.Models;
using Roulette.Security.Models;

namespace Roulette.Security.Interfaces
{
    public interface IAccountServices
    {
        UserSessions CreateNewUserSession(LoginModel user);
        Users CreateNewUser(LoginModel person, bool isEmailUsername = true);
        bool IsResetTokenValid(string resetToken);
        void DeleteExpiredSessions();
        void UpdatePassword(Users user, string newPassword);
        void Logoff(string authToken);
        string GetAuthTokenForProxyUser(Users user);
        string Login(LoginModel loginModel);
        string HashPassword(string password, string salt);

    }
}
