namespace Roulette.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alterusersessionlog : DbMigration
    {
        public override void Up()
        {
            AlterColumn("Roulette.UserSessionLog", "LoginTime", c => c.DateTime());
            AlterColumn("Roulette.UserSessionLog", "LogOutTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("Roulette.UserSessionLog", "LogOutTime", c => c.DateTime(nullable: false));
            AlterColumn("Roulette.UserSessionLog", "LoginTime", c => c.DateTime(nullable: false));
        }
    }
}
