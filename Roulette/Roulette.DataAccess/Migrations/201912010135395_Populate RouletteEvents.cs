namespace Roulette.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopulateRouletteEvents : DbMigration
    {
        public override void Up()
        {
            Sql("insert into [Roulette].[RouletteEvents] (EventName,MinValue,MaxValue,CreatedByUserId,UpdateDate,UpdatedByUserId,CreateDate) Values('RA 01',2,20,'Dev',GETDATE(),'Dev',GETDATE())");
            Sql("insert into [Roulette].[RouletteEvents] (EventName,MinValue,MaxValue,CreatedByUserId,UpdateDate,UpdatedByUserId,CreateDate) Values('RA 11',5,50,'Dev',GETDATE(),'Dev',GETDATE())");
            Sql("insert into [Roulette].[RouletteEvents] (EventName,MinValue,MaxValue,CreatedByUserId,UpdateDate,UpdatedByUserId,CreateDate) Values('RA 02',2,20,'Dev',GETDATE(),'Dev',GETDATE())");
            Sql("insert into [Roulette].[RouletteEvents] (EventName,MinValue,MaxValue,CreatedByUserId,UpdateDate,UpdatedByUserId,CreateDate) Values('RA 12',5,50,'Dev',GETDATE(),'Dev',GETDATE())");

        }

        public override void Down()
        {
        }
    }
}
