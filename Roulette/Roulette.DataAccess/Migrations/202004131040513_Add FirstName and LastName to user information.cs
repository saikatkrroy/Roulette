namespace Roulette.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFirstNameandLastNametouserinformation : DbMigration
    {
        public override void Up()
        {
            AddColumn("Roulette.Users", "FirstName", c => c.String());
            AddColumn("Roulette.Users", "LastName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("Roulette.Users", "LastName");
            DropColumn("Roulette.Users", "FirstName");
        }
    }
}
