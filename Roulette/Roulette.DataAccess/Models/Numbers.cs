using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roulette.DataAccess.Models
{
    public class Numbers
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public int ColorId { get; set; }
        public string OddEvenFactor { get; set; }
    }
}
