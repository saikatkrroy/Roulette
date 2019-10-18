namespace Roulette.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class allocateoddevenfactortozero : DbMigration
    {
        public override void Up()
        {
            Sql("update [Roulette].[Numbers] set OddEvenFactor='Even'where Number=0;");

        }

        public override void Down()
        {
        }
    }
}
