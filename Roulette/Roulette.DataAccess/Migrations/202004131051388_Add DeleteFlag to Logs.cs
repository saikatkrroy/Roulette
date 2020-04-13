namespace Roulette.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDeleteFlagtoLogs : DbMigration
    {
        public override void Up()
        {
            AddColumn("Roulette.Logs", "DeleteFlag", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("Roulette.Logs", "DeleteFlag");
        }
    }
}
