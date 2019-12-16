using System.ComponentModel.DataAnnotations.Schema;

namespace Roulette.DataAccess.Models
{
    [Table("Logs", Schema = "Roulette")]
    public class Logs: BaseUpdatableEntity
    {
        public int Id { get; set; }
        public int NumberId { get; set; }
        public int UserId { get; set; }
        public int RouletteEventId { get; set; }
        public int UserSessionLogId { get; set; }
        public double? BetPlaced { get; set; }
        public bool? UpdateFlag { get; set; }
        public virtual RouletteEvents RouletteEvent { get; set; }
        public virtual Numbers Number { get; set; }
        public virtual Users User { get; set; }
        public virtual UserSessionLog UserSessionLogs { get; set; }
    }
}
