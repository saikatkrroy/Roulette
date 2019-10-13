using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Roulette.DataAccess.Models
{
    [Table("Numbers", Schema = "Roulette")]
    public class Numbers: BaseUpdatableEntity
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public int ColorId { get; set; }
        public string OddEvenFactor { get; set; }
        public virtual Colors Color { get; set; }
        public virtual IList<Logs> Logs { get; set; }
    }
}
