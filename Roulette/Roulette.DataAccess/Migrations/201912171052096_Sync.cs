namespace Roulette.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Sync : DbMigration
    {
        public override void Up()
        {
            AlterColumn("Roulette.Logs", "BetPlaced", c => c.Double());
        }
        
        public override void Down()
        {
            AlterColumn("Roulette.Logs", "BetPlaced", c => c.Double(nullable: false));
        }
    }
}
