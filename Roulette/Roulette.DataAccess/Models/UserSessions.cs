using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Roulette.DataAccess.Models
{
    [Table("UserSessions", Schema = "Roulette")]
    public class UserSessions: BaseUpdatableEntity
    {
        public int Id { get; set; }
        public string AuthToken { get; set; }
        public DateTime? AuthExpiration { get; set; }
        public int UserId { get; set; }
        public virtual Users User { get; set; }
        public string AuthTokenSalt { get; set; }
        public string AuthDoubleSubmitSessionIdCookie { get; set; }
        public bool IsExpired { get; set; }
        public DateTime? HardAbsoluteExpirationTime { get; set; }
    }
}
