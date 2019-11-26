
namespace Roulette.DataAccess.Models
{
    public class Users : BaseUpdatableEntity
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
