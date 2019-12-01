namespace Roulette.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addRouletteEvents : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Roulette.RouletteEvents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EventName = c.String(),
                        MinValue = c.Int(nullable: false),
                        MaxValue = c.Int(nullable: false),
                        CreatedByUserId = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(nullable: false),
                        UpdatedByUserId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("Roulette.Logs", "RouletteEventId", c => c.Int(nullable: false));
            AddColumn("Roulette.Logs", "BetPlaced", c => c.Double(nullable: false));
            CreateIndex("Roulette.Logs", "RouletteEventId");
            AddForeignKey("Roulette.Logs", "RouletteEventId", "Roulette.RouletteEvents", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("Roulette.Logs", "RouletteEventId", "Roulette.RouletteEvents");
            DropIndex("Roulette.Logs", new[] { "RouletteEventId" });
            DropColumn("Roulette.Logs", "BetPlaced");
            DropColumn("Roulette.Logs", "RouletteEventId");
            DropTable("Roulette.RouletteEvents");
        }
    }
}
