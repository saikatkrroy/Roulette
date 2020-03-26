using Roulette.DataAccess;
using Roulette.DataAccess.Interfaces;
using Roulette.DataAccess.Models;
using Roulette.DataAccess.Services;
using Roulette.Models;
using Roulette.Security.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Roulette.Controllers
{
    public class RouletteEntryController : ApiController
    {
        IRepository<Logs> _logRepository { get; set; }
        IRepository<Numbers> _numberRepository { get; set; }
        IRepository<UserSessions> _userSessionRepository { get; set; }
        IRepository<Users> _userRepository { get; set; }
        IRepository<RouletteEvents> _rouletteEventsRepository { get; set; }
        IUnitOfWork _unitofWork { get; set; }

        public RouletteEntryController()
        {
            RouletteDbContext rouletteDbContext = new RouletteDbContext();
            _logRepository = new Repository<Logs>(rouletteDbContext);
            _numberRepository = new Repository<Numbers>(rouletteDbContext);
            _userSessionRepository = new Repository<UserSessions>(rouletteDbContext);
            _userRepository = new Repository<Users>(rouletteDbContext);
            _rouletteEventsRepository = new Repository<RouletteEvents>(rouletteDbContext);
            _unitofWork = new UnitOfWork(rouletteDbContext);
        }
        [HttpGet]
        [Route("api/RouletteEntry/UserInputUpdate")]
        public void UserInputUpdate()
        {
            var userSession = _userSessionRepository.Find(us => us.AuthToken == Authorisation.AuthToken).Single();
            Logs log = new Logs();
            var logList = _logRepository.Find(l => l.UserId == userSession.UserId && l.UserSessionLogs.LogOutTime == null);
            log = (from logs in logList
                        orderby logs.Id descending
                        select logs).Take(1).Single();
            log.UpdateFlag = true;
            _logRepository.Update(log);
            _unitofWork.SaveChanges();
        }
        [HttpGet]
        [Route("api/RouletteEntry/UserInputUpdateData")]
        public Logs UserInputUpdateData()
        {
            var userSession = _userSessionRepository.Find(us => us.AuthToken == Authorisation.AuthToken).Single();
            Logs log = new Logs();
            var logList = _logRepository.Find(l => l.UserId == userSession.UserId && l.UserSessionLogs.LogOutTime == null && l.UpdateFlag==true);
            if(logList.Count()>0)
                log = (from logs in logList
                       orderby logs.Id descending
                       select logs).Take(1).Single();
            return log;
        }
        [HttpGet]
        [Route("api/RouletteEntry/RetrieveHotNumber/{sessionFlag}")]
        public List<Numbers> RetrieveHotNumber([FromUri]bool sessionFlag)
        {
            var userSessions = _userSessionRepository.Find(us=>us.AuthToken==Authorisation.AuthToken);
            var userSession = (userSessions.Count() == 0) ? null : userSessions.Single();
            if (!sessionFlag)
                userSession = null;
            List<Numbers> numbers = new List<Numbers>();
            IQueryable<Logs> logs=null;
            if (String.IsNullOrEmpty(userSession?.User?.Id.ToString()))
                logs = _logRepository.Find();
            else
                logs = _logRepository.Find(l => l.UserSessionLogs.User.Id == userSession.User.Id);
            var lastHundredLogs = (from log in logs
                                           orderby log.Id descending
                                           select log).Take(100).ToList();
            var hotNumbers = lastHundredLogs.GroupBy(c => c.NumberId).OrderByDescending(c => c.Count()).Take(6).ToList();
            foreach(var hotNumber in hotNumbers)
            {
                numbers.Add(hotNumber.First().Number);
            }
            return numbers ;
        }
        [HttpGet]
        [Route("api/RouletteEntry/RetrieveCoolNumber/{sessionFlag}")]
        public List<Numbers> RetrieveCoolNumber([FromUri]bool sessionFlag)
        {
            var userSessions = _userSessionRepository.Find(us=>us.AuthToken==Authorisation.AuthToken);
            var userSession = (userSessions.Count() == 0) ? null : userSessions.Single();
            if (!sessionFlag)
                userSession = null;
            List<Numbers> numbers = new List<Numbers>();
            IQueryable<Logs> logs = null;
            if (String.IsNullOrEmpty(userSession?.User?.Id.ToString()))
                logs = _logRepository.Find();
            else
                logs = _logRepository.Find(l => l.UserSessionLogs.User.Id == userSession.User.Id);
            var lastHundredLogs = (from log in logs
                                   orderby log.Id descending
                                   select log).Take(100).ToList();
            var coolNumbers = lastHundredLogs.GroupBy(c => c.NumberId).OrderBy(c => c.Count()).Take(6).ToList();
            foreach (var coolNumber in coolNumbers)
            {
                numbers.Add(coolNumber.First().Number);
            }
            return numbers;

        }
        [HttpGet]
        [Route("api/RouletteEntry/LastTwelveBet/{sessionFlag}")]
        public List<Numbers> RetrieveLastTwelveBets([FromUri]bool sessionFlag)
        {
            var userSessions = _userSessionRepository.Find(us => us.AuthToken == Authorisation.AuthToken);
            var userSession = (userSessions.Count() == 0) ? null : userSessions.Single();
            if (!sessionFlag)
                userSession = null;
            List<Numbers> numbers = new List<Numbers>();
            IQueryable<Logs> logs = null;
            if (String.IsNullOrEmpty(userSession?.User?.Id.ToString()))
                logs = _logRepository.Find();
            else
                logs = _logRepository.Find(l => l.UserSessionLogs.User.Id == userSession.User.Id);
            var lastTwelveLogs = (from log in logs
                                   orderby log.Id descending
                                   select log).Take(12).ToList();
            foreach (var lastTwelveLog in lastTwelveLogs)
            {
                numbers.Add(lastTwelveLog.Number);
            }
            return numbers;
        }
        [HttpGet]
        [Route("api/RouletteEntry/RetrieveColorStats/{sessionFlag}")]
        public IDictionary<string,float> RetrieveColorStats([FromUri]bool sessionFlag)
        {
            var userSessions = _userSessionRepository.Find(us=>us.AuthToken==Authorisation.AuthToken);
            var userSession = (userSessions.Count() == 0) ? null : userSessions.Single();
            if (!sessionFlag)
                userSession = null;
            IQueryable<Logs> logs = null;
            if (String.IsNullOrEmpty(userSession?.User?.Id.ToString()))
                logs = _logRepository.Find();
            else
                logs = _logRepository.Find(l => l.UserSessionLogs.User.Id == userSession.User.Id);
            var loglist = logs.ToList();
            var lastHundredLogs = (from log in logs
                                   orderby log.Id descending
                                   select log).Take(100).ToList();
            IDictionary<string, float> colorStats = new Dictionary<string, float>();
            if (lastHundredLogs.Count() > 0)
            {
                var blackCount = lastHundredLogs.Count(lhl=>lhl.Number.Color.Name=="Black");
                var redCount = lastHundredLogs.Count(lhl=>lhl.Number.Color.Name=="Red");
                if ((blackCount + redCount) > 0)
                {
                    colorStats["Black"] = (blackCount * 100) / (blackCount + redCount);
                    colorStats["Red"] = 100 - colorStats["Black"];
                }
            }
           return colorStats;

        }
        [HttpGet]
        [Route("api/RouletteEntry/RetrieveOddEvenStats/{sessionFlag}")]
        public IDictionary<string,int> RetrieveOddEvenStats([FromUri]bool sessionFlag)
        {
            var userSessions = _userSessionRepository.Find(us=>us.AuthToken==Authorisation.AuthToken);
            var userSession = (userSessions.Count() == 0) ? null : userSessions.Single();
            if (!sessionFlag)
                userSession = null;
            IQueryable<Logs> logs = null;
            if (String.IsNullOrEmpty(userSession?.User?.Id.ToString()))
                logs = _logRepository.Find();
            else
                logs = _logRepository.Find(l => l.UserSessionLogs.User.Id == userSession.User.Id);
            var lastHundredLogs = (from log in logs
                                   orderby log.Id descending
                                   select log).Take(100).ToList();
            IDictionary<string, int> oddEvenStats = new Dictionary<string, int>();
            if (lastHundredLogs.Count() > 0)
            {
                var evenCount = lastHundredLogs.Count(lhl=>lhl.Number.OddEvenFactor=="Even");
                var OddCount = lastHundredLogs.Count(lhl=>lhl.Number.OddEvenFactor=="Odd");
                if ((evenCount + OddCount) > 0)
                {
                    oddEvenStats["Even"] = (evenCount * 100) / (evenCount + OddCount);
                    oddEvenStats["Odd"] = 100 - oddEvenStats["Even"];

                }
            }
            return oddEvenStats;
        }
        [HttpGet]
        [Route("api/RouletteEntry/RetrieveZeroPercentage/{sessionFlag}")]
        public IDictionary<string, int> RetrieveZeroPercentage([FromUri]bool sessionFlag)
        {
            var userSessions = _userSessionRepository.Find(us=>us.AuthToken==Authorisation.AuthToken);
            var userSession = (userSessions.Count()==0)?null: userSessions.Single();
            if (!sessionFlag)
                userSession = null;
            IQueryable<Logs> logs = null;
            if (String.IsNullOrEmpty(userSession?.User?.Id.ToString()))
                logs = _logRepository.Find();
            else
                logs = _logRepository.Find(l => l.UserSessionLogs.User.Id == userSession.User.Id);
            IDictionary<string, int> zeroPercentage = new Dictionary<string, int>();
            zeroPercentage["Zero"] = 0;
            var lastHundredLogs = (from log in logs
                                   orderby log.Id descending
                                   select log).Take(100).ToList();
            if (lastHundredLogs.Count() > 0)
            {
                var zeroCount = lastHundredLogs.Count(lhl => lhl.Number.Number=="0");
                zeroPercentage["Zero"] = ((zeroCount*100)/lastHundredLogs.Count);
            }

            return zeroPercentage;
        }
        [HttpGet]
        [Route("api/RouletteEntry/RetrieveData")]
        public object RetrieveData()
        {
            var numbers = _numberRepository.Find().ToList();
            var rouletteEvents = _rouletteEventsRepository.Find().ToList();
            return (numbers,rouletteEvents);

        }
        [HttpPost]
        [Route("api/RouletteEntry/CreateUserInput")]
        public void CreateUserInput([FromBody]BetModel betModel)
        {
            //var userSession=_userSessionRepository.Find(us => us.AuthToken == Authorisation.AuthToken).Single();
            //if (userSession == null)
            //    throw new Exception("Invalid User");

            var number = _numberRepository.FindSingleOrNull(n => n.Number == betModel.value);
            var rouletteEvent = _rouletteEventsRepository.Find(r =>r.EventName== betModel.rouletteEventName).Single();
            var log = new Logs()
            {
                NumberId = number.Id,
                RouletteEventId = rouletteEvent.Id,
                //SBetPlaced = betModel.betPlaced,
                UpdateFlag = false,
            };
            _logRepository.Insert(log);
            _unitofWork.SaveChanges();
        }
        [HttpPut]
        [Route("api/RouletteEntry/UpdateUserInput/{value}/{existingEntry}")]
        public void UpdateUserInput([FromUri]int value, [FromUri]int existingEntry)
        {
            var userSession = _userSessionRepository.Find(us => us.AuthToken == Authorisation.AuthToken).Single();
            if (userSession == null)
                throw new Exception("Invalid User");
            var logList = _logRepository.Find(l => l.NumberId == existingEntry && l.UserId == userSession.UserId);
            var log = (from logs in logList
                       orderby logs.Id descending
                       select logs).Take(1).Single();
            var number = _numberRepository.FindSingleOrNull(n => n.Number == value.ToString());
            log.NumberId = number.Id;
            log.UpdateFlag = false;
            _logRepository.Update(log);
            _unitofWork.SaveChanges();
        }
        [HttpDelete]
        [Route("api/RouletteEntry/DeleteUserInput/{existingEntry}")]
        public void DeleteUserInput([FromUri]string existingEntry)
        {
            var userSession = _userSessionRepository.Find(us => us.AuthToken == Authorisation.AuthToken).Single();
            if (userSession == null)
                throw new Exception("Invalid User");
            var logList = _logRepository.Find(l => l.Number.Number == existingEntry && l.UserId == userSession.UserId);
            var log = (from logs in logList
                       orderby logs.Id descending
                       select logs).Take(1).Single();

            _logRepository.Delete(log);
            _unitofWork.SaveChanges();
        }
        //[HttpGet]
        //[Route("api/RouletteEntry/RetrieveUsers")]
        //public List<string> RetrieveUsers()
        //{
        //    List<string> logs = new List<string>();
        //    if (!String.IsNullOrEmpty(Authorisation.AuthToken))
        //        logs = _userRepository.Find().Select(u=>u.UserName).ToList();

        //    return logs;
        //}
    }
}