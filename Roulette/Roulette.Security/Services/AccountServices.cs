using Roulette.DataAccess.Interfaces;
using Roulette.DataAccess.Models;
using Roulette.Security.Helpers;
using Roulette.Security.Interfaces;
using Roulette.Security.Models;
using System;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

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
            var userSearch = _usersRepository.Find(u => u.UserName == person.Username);
            if (userSearch.Count() != 0)
                throw new Exception("Username already exists");
            ValidateUserNameAndPassword(person.Username, person.Password, isEmailUsername);
            string salt = CreateRandomToken();
            string pwdHash = HashPassword(person.Password, salt);
            var user = new Users()
            {
                UserName = person.Username,
                PasswordSalt = salt,
                Password = pwdHash,
            };
            _usersRepository.InsertAndSave(user);
            //_unitOfWork.SaveChanges();
            return user;
        }

        public UserSessions CreateNewUserSession(LoginModel user)
        {
            string token = CreateRandomToken();
            string tokenSalt = string.Empty;

            //use User create timestamp and web.config secret key to encrypt newly created token (guids...)
            string tokenEncryptKey = DateTime.Now + ConfigurationManager.AppSettings["ENCRYPT_LINK_KEY"];
            string encryptedToken = AesEncryptionHelper.Encrypt(token, tokenEncryptKey, ref tokenSalt);

            // use newly created token (guids...) and web.config secret to encrypt session id, use same salt as used by token
            string sessionCookieEncryptKey = token + ":" + ConfigurationManager.AppSettings["ENCRYPT_LINK_KEY"];
            string doubleSubmitSessionCookie = AesEncryptionHelper.Encrypt(Guid.NewGuid().ToString(), sessionCookieEncryptKey, ref tokenSalt);

            var userSession = new UserSessions
            {
                User = new Users() { UserName=user.Username,Password=user.Password},
                AuthToken = encryptedToken.Base64ToBase64URL(),  //since we may use this authToken in a URL later, let's make sure it's URL safe.
                AuthExpiration = DateTime.UtcNow.AddMinutes(12*60),
                IsExpired = false,
                HardAbsoluteExpirationTime = DateTime.UtcNow.AddMinutes(12*60),
                AuthTokenSalt = tokenSalt,
                AuthDoubleSubmitSessionIdCookie = doubleSubmitSessionCookie,
            };
            return userSession;
        }


        public string GetAuthTokenForProxyUser(Users user)
        {

            var userSession = CreateNewUserSession(new LoginModel() { Username = user.UserName, Password = user.Password });
            _userSessionRepository.Insert(userSession);

            return userSession.AuthToken;
        }

        public string HashPassword(string password, string salt)
        {
            var bytes = Encoding.UTF8.GetBytes(password + salt);
            return Convert.ToBase64String(new SHA256Managed().ComputeHash(bytes));
        }

        public string Login(LoginModel loginModel)
        {
            Users user = _usersRepository.Find(u=>u.UserName==loginModel.Username).Single();

            if (user == null)
            {
                throw new Exception("INVALID USER NAME OR PASSWORD");
            }
            var authToken = HandleLoginRequest(user, loginModel.Password);


            return authToken;
        }

        public void Logoff(string authToken)
        {
            if (String.IsNullOrEmpty(authToken))
            {
                throw new Exception("Invalid AuthToken");
            }
            var userSession = _userSessionRepository.EnsureFindSingle(u => u.AuthToken == authToken);
            _userSessionRepository.Delete(userSession);
            _unitOfWork.SaveChanges();
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
            ValidateLogin(user, password);

            var userSession = CreateNewUserSession(new LoginModel() { Username=user.UserName, Password=user.Password});

            _userSessionRepository.Insert(userSession);

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
