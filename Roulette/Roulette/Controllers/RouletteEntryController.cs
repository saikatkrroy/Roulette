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
        public RouletteEntryController()
        {
            _logRepository =new Repository<Logs>(new RouletteDbContext());
            _numberRepository = new Repository<Numbers>(new RouletteDbContext());
            _unitofWork = new UnitOfWork(new RouletteDbContext());
        }
        [HttpGet]
        [Route("api/RouletteEntry/RetrieveHotNumber")]
        public Numbers[] RetrieveHotNumber()
        {
            var logs = _logRepository.Find().ToList();
            var lastHundredLogs = (from log in logs
                                           orderby log.Id descending
                                           select log).Take(100).ToList();
            var hotNumbers = lastHundredLogs.GroupBy(c => c.NumberId).OrderByDescending(c => c.Count()).Take(4).ToList();
            Numbers[] numbers = { hotNumbers[0].ElementAt(0).Number, hotNumbers[1].ElementAt(0).Number, hotNumbers[2].ElementAt(0).Number, hotNumbers[3].ElementAt(0).Number };
            return numbers ;
        }
        [HttpGet]
        [Route("api/RouletteEntry/RetrieveCoolNumber")]
        public Numbers[] RetrieveCoolNumber()
        {
            var logs = _logRepository.Find().ToList();
            var lastHundredLogs = (from log in logs
                                   orderby log.Id descending
                                   select log).Take(100).ToList();
            var coolNumbers = lastHundredLogs.GroupBy(c => c.NumberId).OrderBy(c => c.Count()).Take(4).ToList();
            Numbers[] numbers = { coolNumbers[0].ElementAt(0).Number, coolNumbers[1].ElementAt(0).Number, coolNumbers[2].ElementAt(0).Number, coolNumbers[3].ElementAt(0).Number };
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
            var colorGroups = lastHundredLogs.GroupBy(c => c.Number.ColorId).OrderBy(c => c.Count()).ToList();
            IDictionary<string, float> colorStats = new Dictionary<string, float>();
            colorStats[colorGroups[0].ElementAt(0).Number.Color.Name] = (colorGroups[0].Count()*100)/(colorGroups[0].Count()+ colorGroups[1].Count());
            colorStats[colorGroups[1].ElementAt(0).Number.Color.Name] = 100- colorStats[colorGroups[0].ElementAt(0).Number.Color.Name];
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
            var oddEvenGroups = lastHundredLogs.GroupBy(c => c.Number.OddEvenFactor).OrderBy(c => c.Count()).ToList();
            IDictionary<string, int> oddEvenStats = new Dictionary<string, int>();
            oddEvenStats[oddEvenGroups[0].ElementAt(0).Number.OddEvenFactor] = (oddEvenGroups[0].Count() * 100) / (oddEvenGroups[0].Count() + oddEvenGroups[1].Count());
            oddEvenStats[oddEvenGroups[1].ElementAt(0).Number.OddEvenFactor] = 100- oddEvenStats[oddEvenGroups[0].ElementAt(0).Number.OddEvenFactor];
            return oddEvenStats;

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
            _logRepository.InsertAndSave(log);
            _unitofWork.SaveChanges();
        }
    }
}