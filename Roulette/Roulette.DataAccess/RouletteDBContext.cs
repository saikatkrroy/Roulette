using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roulette.DataAccess
{
    public class RouletteDbContext : DbContext
    {
        public RouletteDbContext()
            : base("Name=RouletteDbContext")
        {

            var objectContext = (this as IObjectContextAdapter).ObjectContext;
            objectContext.CommandTimeout = 100;
        }

        public void RejectChanges()
        {
            var context = ((IObjectContextAdapter)this).ObjectContext;
            foreach (var change in ChangeTracker.Entries())
            {
                if (change.State == EntityState.Modified)
                {
                    context.Refresh(RefreshMode.StoreWins, change.Entity);
                }
                if (change.State == EntityState.Added)
                {
                    context.Detach(change.Entity);
                }
            }
        }
    }
}
