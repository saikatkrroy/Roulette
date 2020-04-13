namespace Roulette.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Sync1 : DbMigration
    {
        public override void Up()
        {
            CreateIndex("Roulette.Logs", "SupervisorId");
            AddForeignKey("Roulette.Logs", "SupervisorId", "Roulette.Users", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("Roulette.Logs", "SupervisorId", "Roulette.Users");
            DropIndex("Roulette.Logs", new[] { "SupervisorId" });
        }
    }
}
