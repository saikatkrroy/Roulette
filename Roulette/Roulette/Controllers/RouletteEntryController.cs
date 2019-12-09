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

        public RouletteEntryController(IRepository<Logs> logRepository,
            IRepository<Numbers> numberRepository,
            IRepository<UserSessions> userSessionRepository,
            IRepository<RouletteEvents> rouletteEventsRepository,
            IRepository<Users> userRepository,
            IUnitOfWork unitOfWork)
        {
            _logRepository = logRepository;
            _numberRepository = numberRepository;
            _userSessionRepository = userSessionRepository;
            _userRepository = userRepository;
            _rouletteEventsRepository = rouletteEventsRepository;
            _unitofWork = unitOfWork;
        }
        
        [HttpGet]
        [Route("api/RouletteEntry/RetrieveHotNumber")]
        public List<Numbers> RetrieveHotNumber([FromBody]string userId)
        {
            List<Numbers> numbers = new List<Numbers>();
            IQueryable<Logs> logs=null;
            if (String.IsNullOrEmpty(userId))
                logs = _logRepository.Find();
            else
                logs = _logRepository.Find(l => l.UserSessionLogs.User.UserName == userId);
            var lastHundredLogs = (from log in logs
                                           orderby log.Id descending
                                           select log).Take(100).ToList();
            var hotNumbers = lastHundredLogs.GroupBy(c => c.NumberId).OrderByDescending(c => c.Count()).Take(4).ToList();
            foreach(var hotNumber in hotNumbers)
            {
                numbers.Add(hotNumber.First().Number);
            }
            return numbers ;
        }
        [HttpGet]
        [Route("api/RouletteEntry/RetrieveCoolNumber")]
        public List<Numbers> RetrieveCoolNumber([FromBody]string userId)
        {
            List<Numbers> numbers = new List<Numbers>();
            IQueryable<Logs> logs = null;
            if (String.IsNullOrEmpty(userId))
                logs = _logRepository.Find();
            else
                logs = _logRepository.Find(l => l.UserSessionLogs.User.UserName == userId);
            var lastHundredLogs = (from log in logs
                                   orderby log.Id descending
                                   select log).Take(100).ToList();
            var coolNumbers = lastHundredLogs.GroupBy(c => c.NumberId).OrderBy(c => c.Count()).Take(4).ToList();
            foreach (var coolNumber in coolNumbers)
            {
                numbers.Add(coolNumber.First().Number);
            }
            return numbers;

        }
        [HttpGet]
        [Route("api/RouletteEntry/RetrieveColorStats")]
        public IDictionary<string,float> RetrieveColorStats([FromBody]string userId)
        {
            IQueryable<Logs> logs = null;
            if (String.IsNullOrEmpty(userId))
                logs = _logRepository.Find();
            else
                logs = _logRepository.Find(l => l.UserSessionLogs.User.UserName == userId);
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
        [Route("api/RouletteEntry/RetrieveOddEvenStats")]
        public IDictionary<string,int> RetrieveOddEvenStats([FromBody]string userId)
        {
            IQueryable<Logs> logs = null;
            if (String.IsNullOrEmpty(userId))
                logs = _logRepository.Find();
            else
                logs = _logRepository.Find(l => l.UserSessionLogs.User.UserName == userId);
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
        [Route("api/RouletteEntry/RetrieveZeroPercentage")]
        public IDictionary<string, int> RetrieveZeroPercentage([FromBody]string userId)
        {
            IQueryable<Logs> logs = null;
            if (String.IsNullOrEmpty(userId))
                logs = _logRepository.Find();
            else
                logs = _logRepository.Find(l => l.UserSessionLogs.User.UserName == userId);
            IDictionary<string, int> zeroPercentage = new Dictionary<string, int>();

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
        public object RetrieveNumbers()
        {
            var numbers = _numberRepository.Find().ToList();
            var rouletteEvents = _rouletteEventsRepository.Find().ToList();
            return (numbers,rouletteEvents);

        }
        [HttpPost]
        [Route("api/RouletteEntry/CreateUserInput")]
        public void CreateUserInput([FromBody]BetModel betModel)
        {
            var userSession=_userSessionRepository.Find(us => us.AuthToken == Authorisation.AuthToken).Single();
            if (userSession == null)
                throw new Exception("Invalid User");

            var number = _numberRepository.FindSingleOrNull(n => n.Number == betModel.value);
            var rouletteEvent = _rouletteEventsRepository.Find(r =>r.EventName== betModel.rouletteEventName).Single();
            var log = new Logs()
            {
                NumberId=number.Id,
                UserId=userSession.User.Id,
                RouletteEventId= rouletteEvent.Id,
                BetPlaced= betModel.betPlaced,
                UserSessionLogId=Authorisation.UserSessionLogId
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
        [HttpGet]
        [Route("api/RouletteEntry/RetrieveUsers")]
        public List<string> RetrieveUsers()
        {
            List<string> logs = new List<string>();
            if (!String.IsNullOrEmpty(Authorisation.AuthToken))
                logs = _userRepository.Find().Select(u=>u.UserName).ToList();

            return logs;
        }
    }
}