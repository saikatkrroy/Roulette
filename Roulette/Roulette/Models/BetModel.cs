using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Roulette.Models
{
    public class BetModel
    {
        public string value { get; set; }
        public string rouletteEventName { get; set; }
        public double betPlaced { get; set; }
    }
}