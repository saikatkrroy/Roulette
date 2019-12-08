namespace Roulette.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addUsersessionlogslogs : DbMigration
    {
        public override void Up()
        {
            AddColumn("Roulette.Logs", "UserSessionLogId", c => c.Int(nullable: false));
            CreateIndex("Roulette.Logs", "UserSessionLogId");
            AddForeignKey("Roulette.Logs", "UserSessionLogId", "Roulette.UserSessionLog", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("Roulette.Logs", "UserSessionLogId", "Roulette.UserSessionLog");
            DropIndex("Roulette.Logs", new[] { "UserSessionLogId" });
            DropColumn("Roulette.Logs", "UserSessionLogId");
        }
    }
}
