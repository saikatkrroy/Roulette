namespace Roulette.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Adduserssionlog : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Roulette.UserSessionLog",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        LoginTime = c.DateTime(nullable: false),
                        LogOutTime = c.DateTime(nullable: false),
                        CreatedByUserId = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(nullable: false),
                        UpdatedByUserId = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Roulette.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Roulette.UserSessionLog", "UserId", "Roulette.Users");
            DropIndex("Roulette.UserSessionLog", new[] { "UserId" });
            DropTable("Roulette.UserSessionLog");
        }
    }
}
