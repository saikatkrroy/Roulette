using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roulette.DataAccess.Models
{
    [Table("RouletteEvents", Schema = "Roulette")]
    public class RouletteEvents : BaseUpdatableEntity
    {
        public int Id { get; set; }
        public string EventName { get; set; }
        public int MinValue { get; set; }
        public int MaxValue { get; set; }
    }
}
