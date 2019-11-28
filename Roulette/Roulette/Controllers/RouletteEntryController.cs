using Roulette.DataAccess;
using Roulette.DataAccess.Interfaces;
using Roulette.DataAccess.Models;
using Roulette.DataAccess.Services;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Roulette.Controllers
{
    public class RouletteEntryController : ApiController
    {
        IRepository<Logs> _logRepository { get; set; }
        IRepository<Numbers> _numberRepository { get; set; }
        IUnitOfWork _unitofWork { get; set; }
        //public RouletteEntryController()
        //{
        //    _logRepository =new Repository<Logs>(new RouletteDbContext());
        //    _numberRepository = new Repository<Numbers>(new RouletteDbContext());
        //    _unitofWork = new UnitOfWork(new RouletteDbContext());
        //}
        public RouletteEntryController(IRepository<Logs> logRepository,IRepository<Numbers> numberRepository, IUnitOfWork unitOfWork)
        {
            _logRepository = logRepository;
            _numberRepository = numberRepository;
            _unitofWork = unitOfWork;
        }
        [HttpGet]
        [Route("api/RouletteEntry/RetrieveHotNumber")]
        public List<Numbers> RetrieveHotNumber()
        {
            List<Numbers> numbers = new List<Numbers>();
            var logs = _logRepository.Find().ToList();
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
            var logs = _logRepository.Find().ToList();
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
            var logs = _logRepository.Find().ToList();
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
            var logs = _logRepository.Find().ToList();
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
            var logs = _logRepository.Find().ToList();
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
        public void CreateUserInput([FromBody]string value)
        {
            var number = _numberRepository.FindSingleOrNull(n => n.Number == value);
            var log = new Logs()
            {
                NumberId=number.Id
            };
            _logRepository.Insert(log);
            _unitofWork.SaveChanges();
        }
    }
}