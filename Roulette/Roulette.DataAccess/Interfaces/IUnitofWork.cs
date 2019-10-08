using System.Data.Entity.Infrastructure;

namespace Roulette.DataAccess.Interfaces
{
    public interface IUnitOfWork 
    {
        void SaveChanges();
        void RejectChanges();
        DbContextConfiguration Config { get; }
    }
}
