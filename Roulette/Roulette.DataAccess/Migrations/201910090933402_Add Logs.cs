namespace Roulette.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLogs : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                   "Roulette.Logs",
                   c => new
                   {
                       Id = c.Int(nullable: false, identity: true),
                       NumberId = c.Int(nullable: false),
                       CreatedByUserId = c.String(nullable: false, maxLength: 200),
                       UpdateDate = c.DateTime(nullable: false),
                       UpdatedByUserId = c.String(nullable: false, maxLength: 200),
                       CreateDate = c.DateTime(nullable: false),
                   })
                   .PrimaryKey(t => t.Id)
                   .Index(t => t.NumberId);
            AddForeignKey("Roulette.Logs", "NumberId", "Roulette.Numbers", "Id");
        }
        
        public override void Down()
        {
        }
    }
}
