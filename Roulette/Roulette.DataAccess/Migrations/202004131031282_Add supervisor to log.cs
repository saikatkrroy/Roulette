namespace Roulette.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addsupervisortolog : DbMigration
    {
        public override void Up()
        {
            AddColumn("Roulette.Logs", "SupervisorId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("Roulette.Logs", "SupervisorId");
        }
    }
}
