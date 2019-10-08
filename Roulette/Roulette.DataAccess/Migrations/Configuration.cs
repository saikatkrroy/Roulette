namespace Roulette.DataAccess.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Roulette.DataAccess.RouletteDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            AutomaticMigrationDataLossAllowed = false;
            CommandTimeout = Int32.MaxValue;
            ContextKey = "RouletteDb";
            MigrationsNamespace = GetType().Namespace;
        }

        protected override void Seed(Roulette.DataAccess.RouletteDbContext context)
        {
            base.Seed(context);
        }
    }
}
