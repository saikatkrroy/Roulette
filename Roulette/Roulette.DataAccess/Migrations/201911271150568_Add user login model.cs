namespace Roulette.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Adduserloginmodel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Roulette.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        Password = c.String(),
                        PasswordSalt = c.String(),
                        CreatedByUserId = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(nullable: false),
                        UpdatedByUserId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "Roulette.UserSessions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AuthToken = c.String(),
                        AuthExpiration = c.DateTime(),
                        UserId = c.Int(nullable: false),
                        AuthTokenSalt = c.String(),
                        AuthDoubleSubmitSessionIdCookie = c.String(),
                        IsExpired = c.Boolean(nullable: false),
                        HardAbsoluteExpirationTime = c.DateTime(),
                        CreatedByUserId = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(nullable: false),
                        UpdatedByUserId = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Roulette.Users", t => t.UserId)
                .Index(t => t.UserId);
            
            AddColumn("Roulette.Logs", "UserId", c => c.Int(nullable: false));
            CreateIndex("Roulette.Logs", "UserId");
            AddForeignKey("Roulette.Logs", "UserId", "Roulette.Users", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("Roulette.UserSessions", "UserId", "Roulette.Users");
            DropForeignKey("Roulette.Logs", "UserId", "Roulette.Users");
            DropIndex("Roulette.UserSessions", new[] { "UserId" });
            DropIndex("Roulette.Logs", new[] { "UserId" });
            DropColumn("Roulette.Logs", "UserId");
            DropTable("Roulette.UserSessions");
            DropTable("Roulette.Users");
        }
    }
}
