namespace Roulette.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addupdateflag : DbMigration
    {
        public override void Up()
        {
            AddColumn("Roulette.Logs", "UpdateFlag", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("Roulette.Logs", "UpdateFlag");
        }
    }
}
