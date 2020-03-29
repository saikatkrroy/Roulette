namespace Roulette.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRouletteEventName : DbMigration
    {
        public override void Up()
        {
            Sql("Update [Roulette].[RouletteEvents] set EventName='RA 01 (Min 2 \u20AC Max 20 \u20AC)' where EventName='RA 01'");
            Sql("Update [Roulette].[RouletteEvents] set EventName='RA 02 (Min 5 \u20AC Max 50 \u20AC)' where EventName='RA 02'");
            Sql("Update [Roulette].[RouletteEvents] set EventName='RA 11 (Min 2 \u20AC Max 20 \u20AC)' where EventName='RA 11'");
            Sql("Update [Roulette].[RouletteEvents] set EventName='RA 12 (Min 5 \u20AC Max 50 \u20AC)' where EventName='RA 12'");
        }
        
        public override void Down()
        {
        }
    }
}
