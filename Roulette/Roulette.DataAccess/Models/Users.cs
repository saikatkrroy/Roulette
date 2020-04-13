
using System.ComponentModel.DataAnnotations.Schema;

namespace Roulette.DataAccess.Models
{
    [Table("Users", Schema = "Roulette")]
    public class Users : BaseUpdatableEntity
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
    }
}
