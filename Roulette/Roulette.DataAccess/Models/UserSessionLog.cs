using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roulette.DataAccess.Models
{
    [Table("UserSessionLog", Schema = "Roulette")]
    public class UserSessionLog : BaseUpdatableEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime? LoginTime { get; set; }
        public DateTime? LogOutTime { get; set; }
        public virtual Users User { get; set; }
    }
}
