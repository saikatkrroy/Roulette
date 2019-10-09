namespace Roulette.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addcolors : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Roulette.Colors",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(nullable: false, maxLength: 200),
                    CreatedByUserId = c.String(nullable: false, maxLength: 200),
                    UpdateDate = c.DateTime(nullable: false),
                    UpdatedByUserId = c.String(nullable: false, maxLength: 200),
                    CreateDate = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "Roulette.Numbers",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Number = c.String(nullable: false, maxLength: 200),
                    ColorId = c.Int(nullable: false),
                    OddEvenFactor = c.String(nullable: false, maxLength: 200),
                    CreatedByUserId = c.String(nullable: false, maxLength: 200),
                    UpdateDate = c.DateTime(nullable: false),
                    UpdatedByUserId = c.String(nullable: false, maxLength: 200),
                    CreateDate = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Roulette.Colors", t => t.ColorId)
                .Index(t => t.ColorId);
        }

        public override void Down()
        {

        }
    }
}
