using System.ComponentModel.DataAnnotations.Schema;

namespace Roulette.DataAccess.Models
{
    [Table("Logs", Schema = "Roulette")]
    public class Logs: BaseUpdatableEntity
    {
        public int Id { get; set; }
        public int NumberId { get; set; }
        public virtual Numbers Number { get; set; }
    }
}
