namespace Roulette.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateInitialmodels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Roulette.Colors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        CreatedByUserId = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(nullable: false),
                        UpdatedByUserId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "Roulette.Numbers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Number = c.String(),
                        ColorId = c.Int(nullable: false),
                        OddEvenFactor = c.String(),
                        CreatedByUserId = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(nullable: false),
                        UpdatedByUserId = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Roulette.Colors", t => t.ColorId)
                .Index(t => t.ColorId);
            
            CreateTable(
                "Roulette.Logs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NumberId = c.Int(nullable: false),
                        CreatedByUserId = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(nullable: false),
                        UpdatedByUserId = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Roulette.Numbers", t => t.NumberId)
                .Index(t => t.NumberId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Roulette.Logs", "NumberId", "Roulette.Numbers");
            DropForeignKey("Roulette.Numbers", "ColorId", "Roulette.Colors");
            DropIndex("Roulette.Logs", new[] { "NumberId" });
            DropIndex("Roulette.Numbers", new[] { "ColorId" });
            DropTable("Roulette.Logs");
            DropTable("Roulette.Numbers");
            DropTable("Roulette.Colors");
        }
    }
}
