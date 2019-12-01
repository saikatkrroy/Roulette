using Roulette.DataAccess;
using Roulette.DataAccess.Interfaces;
using Roulette.DataAccess.Models;
using Roulette.DataAccess.Services;
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
        IRepository<RouletteEvents> _rouletteEventsRepository { get; set; }
        IUnitOfWork _unitofWork { get; set; }
        //public RouletteEntryController()
        //{
        //    _logRepository =new Repository<Logs>(new RouletteDbContext());
        //    _numberRepository = new Repository<Numbers>(new RouletteDbContext());
        //    _unitofWork = new UnitOfWork(new RouletteDbContext());
        //}
        public RouletteEntryController(IRepository<Logs> logRepository,
            IRepository<Numbers> numberRepository,
            IRepository<UserSessions> userSessionRepository,
            IRepository<RouletteEvents> rouletteEventsRepository,
            IUnitOfWork unitOfWork)
        {
            _logRepository = logRepository;
            _numberRepository = numberRepository;
            _userSessionRepository = userSessionRepository;
            _rouletteEventsRepository = rouletteEventsRepository;
            _unitofWork = unitOfWork;
        }
        [HttpGet]
        [Route("api/RouletteEntry/RetrieveHotNumber")]
        public List<Numbers> RetrieveHotNumber()
        {
            List<Numbers> numbers = new List<Numbers>();
            var logs = _logRepository.Find();
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
        public List<Numbers> RetrieveCoolNumber()
        {
            List<Numbers> numbers = new List<Numbers>();
            var logs = _logRepository.Find();
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
        public IDictionary<string,float> RetrieveColorStats()
        {
            var logs = _logRepository.Find();
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
        public IDictionary<string,int> RetrieveOddEvenStats()
        {
            var logs = _logRepository.Find();
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
        public IDictionary<string, int> RetrieveZeroPercentage()
        {
            var logs = _logRepository.Find();
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
        [Route("api/RouletteEntry/RetrieveNumbers")]
        public List<Numbers> RetrieveNumbers()
        {
            var numbers = _numberRepository.Find().ToList();
            return numbers;

        }
        [HttpPost]
        [Route("api/RouletteEntry/CreateUserInput")]
        public void CreateUserInput([FromBody]string value,string authToken,string rouletteEventName,double betPlaced)
        {
            var userSession=_userSessionRepository.Find(us => us.AuthToken == authToken).Single();
            if (userSession == null)
                throw new Exception("Invalid User");

            var number = _numberRepository.FindSingleOrNull(n => n.Number == value);
            var rouletteEvent = _rouletteEventsRepository.Find(r =>r.EventName== rouletteEventName).Single();
            var log = new Logs()
            {
                NumberId=number.Id,
                Number=number,
                User=userSession.User,
                UserId=userSession.User.Id,
                RouletteEvent=rouletteEvent,
                RouletteEventId= rouletteEvent.Id,
                BetPlaced= betPlaced
            };
            _logRepository.Insert(log);
            _unitofWork.SaveChanges();
        }
        [HttpPut]
        [Route("api/RouletteEntry/UpdateUserInput")]
        public void UpdateUserInput([FromBody]string value,int existingEntry,string authToken)
        {
            var userSession = _userSessionRepository.Find(us => us.AuthToken == authToken).Single();
            if (userSession == null)
                throw new Exception("Invalid User");
            var logList = _logRepository.Find(l=>l.NumberId==existingEntry && l.UserId==userSession.UserId);
            var log = logList.ElementAt(logList.Count()-1);
            var number = _numberRepository.FindSingleOrNull(n => n.Number == value);
            log.Number = number;
            log.NumberId = number.Id;
            _logRepository.Update(log);
            _unitofWork.SaveChanges();
        }
        [HttpDelete]
        [Route("api/RouletteEntry/DeleteUserInput")]
        public void DeleteUserInput([FromBody]int existingEntry, string authToken)
        {
            var userSession = _userSessionRepository.Find(us => us.AuthToken == authToken).Single();
            if (userSession == null)
                throw new Exception("Invalid User");
            var logList = _logRepository.Find(l => l.NumberId == existingEntry && l.UserId == userSession.UserId);
            var log = logList.ElementAt(logList.Count() - 1);
            
            _logRepository.Delete(log);
            _unitofWork.SaveChanges();
        }
    }
}