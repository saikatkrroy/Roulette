using Roulette.DataAccess.Interfaces;
using Roulette.DataAccess.Models;
using Roulette.Security.Interfaces;
using Roulette.Security.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Roulette.Security.Services
{
    public class AccountServices : IAccountServices
    {
        IRepository<Users> _usersRepository;
        IRepository<UserSessions> _userSessionRepository;
        IUnitOfWork _unitOfWork;
        private const string PASSWORD_VALIDATION_REGEX = @"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)).{8,64}$";

        public AccountServices(IRepository<Users> usersRepository, IRepository<UserSessions> userSessionRepository, IUnitOfWork unitOfWork)
        {
            _usersRepository = usersRepository;
            _userSessionRepository = userSessionRepository;
            _unitOfWork = unitOfWork;
        }
        public Users CreateNewUser(LoginModel person, bool isEmailUsername = true)
        {
            ValidateUserNameAndPassword(person.Username, person.Password, isEmailUsername);
            string salt = CreateRandomToken();
            string pwdHash = HashPassword(person.Password, salt);
            var user = new Users()
            {
                UserName = person.Username,
                PasswordSalt = salt,
                Password = pwdHash,
            };
            return user;
        }

        public UserSessions CreateNewUserSession(LoginModel user)
        {
            throw new NotImplementedException();
        }

        public void DeleteExpiredSessions()
        {
            throw new NotImplementedException();
        }

        public string GetAuthTokenForProxyUser(Users user)
        {
            throw new NotImplementedException();
        }

        public string HashPassword(string password, string salt)
        {
            var bytes = Encoding.UTF8.GetBytes(password + salt);
            return Convert.ToBase64String(new SHA256Managed().ComputeHash(bytes));
        }

        public bool IsResetTokenValid(string resetToken)
        {
            throw new NotImplementedException();
        }

        public string Login(LoginModel loginModel)
        {
            Users user = _usersRepository.Find(u=>u.UserName==loginModel.Username).Single();

            if (user == null)
            {
                throw new Exception("INVALID USER NAME OR PASSWORD");
            }
            var authToken = HandleLoginRequest(user, loginModel.Password);

            _unitOfWork.SaveChanges();

            return authToken;
        }

        public void Logoff(string authToken)
        {
            throw new NotImplementedException();
        }

        public void UpdatePassword(Users user, string newPassword)
        {
            throw new NotImplementedException();
        }
        public void ValidateUserNameAndPassword(string username, string password, bool isEmailUsername = true)
        {
            ValidateUserName(username, isEmailUsername);
            ValidatePassword(password);
        }
        public void ValidateUserName(string username, bool isEmailUsername)
        {
            try
            {
                if (!isEmailUsername)
                {
                    if (username.Length < 6)
                        throw new Exception("INVALID USER NAME OR PASSWORD");
                }
            }
            catch (Exception)
            {
                throw new Exception("INVALID USER NAME OR PASSWORD");
            }
        }
        public void ValidatePassword(string password)
        {
            var pwdCheck = new Regex(PASSWORD_VALIDATION_REGEX);
            var match = pwdCheck.Match(password);
            if (!match.Success)
                throw new Exception("INVALID USER NAME OR PASSWORD");
        }
        public string CreateRandomToken()
        {
            var token = Guid.NewGuid().ToString("N");
            return token;
        }
        private string HandleLoginRequest(Users user, string password)
        {
            //BUGBUG: Repeat code - found in Recruiting Setup HandleLoginRequest
            ValidateLogin(user, password);

            var userSession = CreateNewUserSession(new LoginModel() { Username=user.UserName, Password=user.Password});

            _userSessionRepository.Insert(userSession);
            _usersRepository.Update(user);

            return userSession.AuthToken;
        }
        protected void ValidateLogin(Users user, string password)
        {
            if (HashPassword(password, user.PasswordSalt) != user.Password)
            {
                throw new Exception("INVALID USER NAME OR PASSWORD");
            }

        }
    }
}
