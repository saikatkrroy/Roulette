namespace Roulette.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class makeuseridnullable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Roulette.Logs", "UserId", "Roulette.Users");
            DropForeignKey("Roulette.Logs", "UserSessionLogId", "Roulette.UserSessionLog");
            DropIndex("Roulette.Logs", new[] { "UserId" });
            DropIndex("Roulette.Logs", new[] { "UserSessionLogId" });
            AlterColumn("Roulette.Logs", "UserId", c => c.Int());
            AlterColumn("Roulette.Logs", "UserSessionLogId", c => c.Int());
            CreateIndex("Roulette.Logs", "UserId");
            CreateIndex("Roulette.Logs", "UserSessionLogId");
            AddForeignKey("Roulette.Logs", "UserId", "Roulette.Users", "Id");
            AddForeignKey("Roulette.Logs", "UserSessionLogId", "Roulette.UserSessionLog", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("Roulette.Logs", "UserSessionLogId", "Roulette.UserSessionLog");
            DropForeignKey("Roulette.Logs", "UserId", "Roulette.Users");
            DropIndex("Roulette.Logs", new[] { "UserSessionLogId" });
            DropIndex("Roulette.Logs", new[] { "UserId" });
            AlterColumn("Roulette.Logs", "UserSessionLogId", c => c.Int(nullable: false));
            AlterColumn("Roulette.Logs", "UserId", c => c.Int(nullable: false));
            CreateIndex("Roulette.Logs", "UserSessionLogId");
            CreateIndex("Roulette.Logs", "UserId");
            AddForeignKey("Roulette.Logs", "UserSessionLogId", "Roulette.UserSessionLog", "Id", cascadeDelete: true);
            AddForeignKey("Roulette.Logs", "UserId", "Roulette.Users", "Id", cascadeDelete: true);
        }
    }
}
