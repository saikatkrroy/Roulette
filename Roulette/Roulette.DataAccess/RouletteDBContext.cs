using Roulette.DataAccess.Models;
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
        public DbSet<Colors> Colors { get; set; }
        public DbSet<Numbers> Numbers { get; set; }
        public DbSet<Logs> Logs { get; set; }
        public DbSet<Users> Users { get; set; }
        public RouletteDbContext()
            : base("Name=RouletteDbContext")
        {

            var objectContext = (this as IObjectContextAdapter).ObjectContext;
            objectContext.CommandTimeout = 100;
            objectContext.Connection.Close();
        }
        public override int SaveChanges()
        {
            var autoDetectChangesValue = Configuration.AutoDetectChangesEnabled;
            try
            {
                ChangeTracker.DetectChanges();
                Configuration.AutoDetectChangesEnabled = false;
                var changedEntities = ChangeTracker.Entries().Where(x => x.State != EntityState.Unchanged).ToList();
                if (changedEntities.Any())
                {
                    string userIdStr = UserIdStr; //prevent multiple calls to int->ToString()
                    foreach (var dbEntry in changedEntities)
                    {
                        UpdateCreateAndLastMaintDates(dbEntry, userIdStr);
                    }

                    // todo - temporary fix until we merge in Alan's optimizations 
                    ChangeTracker.DetectChanges();
                }

                return base.SaveChanges();
            }
            finally
            {
                Configuration.AutoDetectChangesEnabled = autoDetectChangesValue;
            }
        }
        private string UserIdStr
        {
            get
            {
                string userId = "Dev";
                if (String.IsNullOrEmpty(userId))
                {
                    userId = "Dev";
                }
                return userId.ToString();
            }
        }
        private static void UpdateCreateAndLastMaintDates(DbEntityEntry dbEntry, string userId)
        {
            var state = dbEntry.State;
            if (state == EntityState.Unchanged)
                return;

            var updatableEntity = dbEntry.Entity as BaseUpdatableEntity;
            if (updatableEntity == null) return;

            var now = DateTime.UtcNow;
            if (state == EntityState.Added)
            {
                dbEntry.Property(nameof(updatableEntity.CreateDate)).CurrentValue = now;
                dbEntry.Property(nameof(updatableEntity.CreatedByUserId)).CurrentValue = userId;
            }
            dbEntry.Property(nameof(updatableEntity.UpdateDate)).CurrentValue = now;
            dbEntry.Property(nameof(updatableEntity.UpdatedByUserId)).CurrentValue = userId;
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
