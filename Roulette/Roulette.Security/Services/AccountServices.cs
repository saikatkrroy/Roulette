using Roulette.DataAccess;
using Roulette.DataAccess.Interfaces;
using Roulette.DataAccess.Models;
using Roulette.DataAccess.Services;
using Roulette.Security.Helpers;
using Roulette.Security.Interfaces;
using Roulette.Security.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
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
        IRepository<UserSessionLog> _userSessionLogRepository { get; set; }
        IRepository<Logs> _logRepository { get; set; }
        IUnitOfWork _unitOfWork { get; set; }
        private const string PASSWORD_VALIDATION_REGEX = @"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)).{8,64}$";

        public AccountServices()
        {
            RouletteDbContext rouletteDbContext = new RouletteDbContext();
            _usersRepository = new Repository<Users>(rouletteDbContext);
            _userSessionRepository = new Repository<UserSessions>(rouletteDbContext);
            _userSessionLogRepository = new Repository<UserSessionLog>(rouletteDbContext);
            _logRepository = new Repository<Logs>(rouletteDbContext);
            _unitOfWork = new UnitOfWork(rouletteDbContext);
        }
        public Users CreateNewUser(LoginModel person, bool isEmailUsername = true)
        {
            string salt = "";
            string pwdHash="";
            var userSearch = _usersRepository.Find(u => u.UserName == person.Username);
            if (userSearch.Count() != 0)
                throw new Exception("Username already exists");
            if (person.Username.ToLower().Contains("admin"))
            {
                ValidateUserNameAndPassword(person.Username, person.Password, isEmailUsername);
                salt = CreateRandomToken();
                pwdHash = HashPassword(person.Password, salt);
            }

            var user = new Users()
            {
                UserName = person.Username,
                FirstName = person.FirstName,
                LastName = person.LastName,
                PasswordSalt = salt,
                Password = pwdHash,
            };
            _usersRepository.InsertAndSave(user);
            //_unitOfWork.SaveChanges();
            return user;
        }

        public UserSessions CreateNewUserSession(int id,LoginModel user)
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
                UserId = id,
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

            var userSession = CreateNewUserSession(user.Id,new LoginModel() { Username = user.UserName, Password = user.Password });
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

            //Update UserSessionLogs for the current UserSession
            var userSession = _userSessionRepository.Find(us => us.AuthToken == authToken).Single();
            var updateBetLogs = _logRepository.Find(l => l.UserSessionLogs.UserId == userSession.UserId && l.UpdateFlag == true).ToList();
            updateBetLogs.ForEach(ubl=>ubl.UpdateFlag=false);
            updateBetLogs.ForEach(ubl=>_logRepository.Update(ubl));
            _unitOfWork.SaveChanges();
            var logs = _logRepository.Find(l=>l.UserSessionLogs.UserId == userSession.UserId && l.UserSessionLogs.LogOutTime == null ).ToList();
            logs = logs.Where(l => l.UserSessionLogs.LoginTime > DateTime.Today.AddDays(-1)).ToList();
            if (logs.Count > 0)
            {
                var userSessionLogId = logs.FirstOrDefault().UserSessionLogs.Id;
                var userSessionLogs = _userSessionLogRepository.Find(l => l.Id == userSessionLogId).ToList();
                userSessionLogs[0].LogOutTime = DateTime.Now;
                _unitOfWork.SaveChanges();
                
                //Resolve and create Filename for current usersession
                var directory = System.AppDomain.CurrentDomain.BaseDirectory;
                directory=directory.Substring(0, directory.LastIndexOf("Roulette\\"));
                string filePath = "";
                Directory.CreateDirectory(directory.ToString() + "UserBetReports\\" + userSession.User.UserName);
                filePath = directory.ToString() + "UserBetReports\\" + userSession.User.UserName + "\\" + userSessionLogs[0].Id + ".csv";

                // Prepare the values
                var allLines = (from log in logs
                                select new Object[]
                                {
                                    log.Supervisor==null?"No Supervisor":log.Supervisor.UserName,
                                    log.UserSessionLogs.LoginTime.ToString(),
                                    log.UserSessionLogs.LogOutTime.ToString(),
                                    log.BetPlaced.ToString(),
                                    log.Number.Number.ToString(),
                                    log.RouletteEvent.EventName.ToString(),
                                    log.CreateDate.ToString(),
                                    log.DeleteFlag==true?"Yes":"No"
                                }).ToList();

                // Build the file content
                var csv = new StringBuilder();
                csv.AppendLine(String.Join(",", "UserName,"+ logs.FirstOrDefault().User.UserName));
                csv.AppendLine(string.Join(",", "Supervisor,LogInTime,LogOutTime,BetPlaced,Number,RouletteEventName","Bet Placed Time","Deleted"));
                allLines.ForEach(line =>
                {
                    csv.AppendLine(string.Join(",", line));
                });
                File.WriteAllText(filePath, csv.ToString());
            }

            var logsToBeDeleted = logs.FindAll(l => l.DeleteFlag == true);
            logsToBeDeleted.ForEach(l=>_logRepository.Delete(l));
            //Remove current usersession
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
            if(user.UserName.ToLower().Contains("admin"))
                ValidateLogin(user, password);

            var userSession = CreateNewUserSession(user.Id,new LoginModel() { Username=user.UserName, Password=user.Password});
            var userSessionLog = new UserSessionLog()
            {
                UserId =userSession.UserId,
                LoginTime = DateTime.UtcNow
            };
            userSessionLog=_userSessionLogRepository.InsertAndSave(userSessionLog);
            Authorisation.UserSessionLogId = userSessionLog.Id;
            _userSessionRepository.Insert(userSession);
            _unitOfWork.SaveChanges();
            return userSession.AuthToken;
        }
        protected void ValidateLogin(Users user, string password)
        {
            if (HashPassword(password, user.PasswordSalt) != user.Password)
            {
                throw new Exception("INVALID USER NAME OR PASSWORD");
            }
        }

        public IList<String> RetrieveUsers()
        {
            return _usersRepository.Find(u=>!u.UserName.Equals("Admin")).Select(x => x.UserName).ToList();
        }
        public void DeleteUser(string userId)
        {
            var loggedInUser = _userSessionRepository.Find(us => us.AuthToken == Authorisation.AuthToken).Single();
            var user=_usersRepository.Find(u=>u.UserName==userId).Single();
            if(loggedInUser.Id==user.Id)
                throw new Exception("Can't delete yourself'");
            var userLog = _logRepository.Find(ul => ul.UserId == user.Id).ToList();
            userLog.ForEach(ul => ul.UserSessionLogId = null); 
            var userSession = _userSessionRepository.Find(us => us.UserId == user.Id).ToList();
            userSession.ForEach(us=>_userSessionRepository.Delete(us));
            var userSessionLog = _userSessionLogRepository.Find(usl => usl.UserId == user.Id).ToList();
            userSessionLog.ForEach(usl=>_userSessionLogRepository.Delete(usl));
            _usersRepository.Delete(user.Id);
            
            _unitOfWork.SaveChanges();
        }

        public bool ValidateUserAccess(string authToken)
        {
            var userSession=_userSessionRepository.Find(us => us.AuthToken.Equals(authToken)).Single();
            if (userSession.User.UserName.ToLower().Contains("admin"))
                return true;
            return false;
        }
    }
}
