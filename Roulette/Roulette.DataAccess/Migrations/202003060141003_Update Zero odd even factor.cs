namespace Roulette.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateZerooddevenfactor : DbMigration
    {
        public override void Up()
        {
            Sql("Update [Roulette].[Numbers] set [OddEvenFactor]=NULL where Number='0'");
        }
        
        public override void Down()
        {
        }
    }
}
