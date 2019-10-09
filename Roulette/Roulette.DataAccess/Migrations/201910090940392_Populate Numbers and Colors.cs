namespace Roulette.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopulateNumbersandColors : DbMigration
    {
        public override void Up()
        {
            Sql(@"SET IDENTITY_INSERT [Roulette].[Colors] ON;");
            Sql("insert into [Roulette].[Colors] (Id, Name,CreatedByUserId,UpdateDate,UpdatedByUserId,CreateDate) Values(1,'Red','Dev',GETDATE(),'Dev',GETDATE())");
            Sql("insert into [Roulette].[Colors] (Id, Name,CreatedByUserId,UpdateDate,UpdatedByUserId,CreateDate) Values(2,'Black','Dev',GETDATE(),'Dev',GETDATE())");
            Sql("insert into [Roulette].[Colors] (Id, Name,CreatedByUserId,UpdateDate,UpdatedByUserId,CreateDate) Values(3,'Green','Dev',GETDATE(),'Dev',GETDATE())");
            Sql(@"SET IDENTITY_INSERT [Roulette].[Colors] OFF;");
            Sql(@"SET IDENTITY_INSERT [Roulette].[Numbers] ON;");
            Sql("insert into [Roulette].[Numbers] (Id, Number,ColorId,OddEvenFactor,CreatedByUserId,UpdateDate,UpdatedByUserId,CreateDate) Values(0,'0',3,'NA','Dev',GETDATE(),'Dev',GETDATE())");
            Sql("insert into [Roulette].[Numbers] (Id, Number,ColorId,OddEvenFactor,CreatedByUserId,UpdateDate,UpdatedByUserId,CreateDate) Values(1,'1',1,'Odd','Dev',GETDATE(),'Dev',GETDATE())");
            Sql("insert into [Roulette].[Numbers] (Id, Number,ColorId,OddEvenFactor,CreatedByUserId,UpdateDate,UpdatedByUserId,CreateDate) Values(2,'2',2,'Even','Dev',GETDATE(),'Dev',GETDATE())");
            Sql("insert into [Roulette].[Numbers] (Id, Number,ColorId,OddEvenFactor,CreatedByUserId,UpdateDate,UpdatedByUserId,CreateDate) Values(3,'3',1,'Odd','Dev',GETDATE(),'Dev',GETDATE())");
            Sql("insert into [Roulette].[Numbers] (Id, Number,ColorId,OddEvenFactor,CreatedByUserId,UpdateDate,UpdatedByUserId,CreateDate) Values(4,'4',2,'Even','Dev',GETDATE(),'Dev',GETDATE())");
            Sql("insert into [Roulette].[Numbers] (Id, Number,ColorId,OddEvenFactor,CreatedByUserId,UpdateDate,UpdatedByUserId,CreateDate) Values(5,'5',1,'Odd','Dev',GETDATE(),'Dev',GETDATE())");
            Sql("insert into [Roulette].[Numbers] (Id, Number,ColorId,OddEvenFactor,CreatedByUserId,UpdateDate,UpdatedByUserId,CreateDate) Values(6,'6',2,'Even','Dev',GETDATE(),'Dev',GETDATE())");
            Sql("insert into [Roulette].[Numbers] (Id, Number,ColorId,OddEvenFactor,CreatedByUserId,UpdateDate,UpdatedByUserId,CreateDate) Values(7,'7',1,'Odd','Dev',GETDATE(),'Dev',GETDATE())");
            Sql("insert into [Roulette].[Numbers] (Id, Number,ColorId,OddEvenFactor,CreatedByUserId,UpdateDate,UpdatedByUserId,CreateDate) Values(8,'8',2,'Even','Dev',GETDATE(),'Dev',GETDATE())");
            Sql("insert into [Roulette].[Numbers] (Id, Number,ColorId,OddEvenFactor,CreatedByUserId,UpdateDate,UpdatedByUserId,CreateDate) Values(9,'9',1,'Odd','Dev',GETDATE(),'Dev',GETDATE())");
            Sql("insert into [Roulette].[Numbers] (Id, Number,ColorId,OddEvenFactor,CreatedByUserId,UpdateDate,UpdatedByUserId,CreateDate) Values(10,'10',2,'Even','Dev',GETDATE(),'Dev',GETDATE())");
            Sql("insert into [Roulette].[Numbers] (Id, Number,ColorId,OddEvenFactor,CreatedByUserId,UpdateDate,UpdatedByUserId,CreateDate) Values(11,'11',2,'Odd','Dev',GETDATE(),'Dev',GETDATE())");
            Sql("insert into [Roulette].[Numbers] (Id, Number,ColorId,OddEvenFactor,CreatedByUserId,UpdateDate,UpdatedByUserId,CreateDate) Values(12,'12',1,'Even','Dev',GETDATE(),'Dev',GETDATE())");
            Sql("insert into [Roulette].[Numbers] (Id, Number,ColorId,OddEvenFactor,CreatedByUserId,UpdateDate,UpdatedByUserId,CreateDate) Values(13,'13',2,'Odd','Dev',GETDATE(),'Dev',GETDATE())");
            Sql("insert into [Roulette].[Numbers] (Id, Number,ColorId,OddEvenFactor,CreatedByUserId,UpdateDate,UpdatedByUserId,CreateDate) Values(14,'14',1,'Even','Dev',GETDATE(),'Dev',GETDATE())");
            Sql("insert into [Roulette].[Numbers] (Id, Number,ColorId,OddEvenFactor,CreatedByUserId,UpdateDate,UpdatedByUserId,CreateDate) Values(15,'15',2,'Odd','Dev',GETDATE(),'Dev',GETDATE())");
            Sql("insert into [Roulette].[Numbers] (Id, Number,ColorId,OddEvenFactor,CreatedByUserId,UpdateDate,UpdatedByUserId,CreateDate) Values(16,'16',1,'Even','Dev',GETDATE(),'Dev',GETDATE())");
            Sql("insert into [Roulette].[Numbers] (Id, Number,ColorId,OddEvenFactor,CreatedByUserId,UpdateDate,UpdatedByUserId,CreateDate) Values(17,'17',2,'Odd','Dev',GETDATE(),'Dev',GETDATE())");
            Sql("insert into [Roulette].[Numbers] (Id, Number,ColorId,OddEvenFactor,CreatedByUserId,UpdateDate,UpdatedByUserId,CreateDate) Values(18,'18',1,'Even','Dev',GETDATE(),'Dev',GETDATE())");
            Sql("insert into [Roulette].[Numbers] (Id, Number,ColorId,OddEvenFactor,CreatedByUserId,UpdateDate,UpdatedByUserId,CreateDate) Values(19,'19',2,'Odd','Dev',GETDATE(),'Dev',GETDATE())");
            Sql("insert into [Roulette].[Numbers] (Id, Number,ColorId,OddEvenFactor,CreatedByUserId,UpdateDate,UpdatedByUserId,CreateDate) Values(20,'20',2,'Even','Dev',GETDATE(),'Dev',GETDATE())");
            Sql("insert into [Roulette].[Numbers] (Id, Number,ColorId,OddEvenFactor,CreatedByUserId,UpdateDate,UpdatedByUserId,CreateDate) Values(21,'21',1,'Odd','Dev',GETDATE(),'Dev',GETDATE())");
            Sql("insert into [Roulette].[Numbers] (Id, Number,ColorId,OddEvenFactor,CreatedByUserId,UpdateDate,UpdatedByUserId,CreateDate) Values(22,'22',2,'Even','Dev',GETDATE(),'Dev',GETDATE())");
            Sql("insert into [Roulette].[Numbers] (Id, Number,ColorId,OddEvenFactor,CreatedByUserId,UpdateDate,UpdatedByUserId,CreateDate) Values(23,'23',1,'Odd','Dev',GETDATE(),'Dev',GETDATE())");
            Sql("insert into [Roulette].[Numbers] (Id, Number,ColorId,OddEvenFactor,CreatedByUserId,UpdateDate,UpdatedByUserId,CreateDate) Values(24,'24',2,'Even','Dev',GETDATE(),'Dev',GETDATE())");
            Sql("insert into [Roulette].[Numbers] (Id, Number,ColorId,OddEvenFactor,CreatedByUserId,UpdateDate,UpdatedByUserId,CreateDate) Values(25,'25',1,'Odd','Dev',GETDATE(),'Dev',GETDATE())");
            Sql("insert into [Roulette].[Numbers] (Id, Number,ColorId,OddEvenFactor,CreatedByUserId,UpdateDate,UpdatedByUserId,CreateDate) Values(26,'26',2,'Even','Dev',GETDATE(),'Dev',GETDATE())");
            Sql("insert into [Roulette].[Numbers] (Id, Number,ColorId,OddEvenFactor,CreatedByUserId,UpdateDate,UpdatedByUserId,CreateDate) Values(27,'27',1,'Odd','Dev',GETDATE(),'Dev',GETDATE())");
            Sql("insert into [Roulette].[Numbers] (Id, Number,ColorId,OddEvenFactor,CreatedByUserId,UpdateDate,UpdatedByUserId,CreateDate) Values(28,'28',2,'Even','Dev',GETDATE(),'Dev',GETDATE())");
            Sql("insert into [Roulette].[Numbers] (Id, Number,ColorId,OddEvenFactor,CreatedByUserId,UpdateDate,UpdatedByUserId,CreateDate) Values(29,'29',2,'Odd','Dev',GETDATE(),'Dev',GETDATE())");
            Sql("insert into [Roulette].[Numbers] (Id, Number,ColorId,OddEvenFactor,CreatedByUserId,UpdateDate,UpdatedByUserId,CreateDate) Values(30,'30',1,'Even','Dev',GETDATE(),'Dev',GETDATE())");
            Sql("insert into [Roulette].[Numbers] (Id, Number,ColorId,OddEvenFactor,CreatedByUserId,UpdateDate,UpdatedByUserId,CreateDate) Values(31,'31',2,'Odd','Dev',GETDATE(),'Dev',GETDATE())");
            Sql("insert into [Roulette].[Numbers] (Id, Number,ColorId,OddEvenFactor,CreatedByUserId,UpdateDate,UpdatedByUserId,CreateDate) Values(32,'32',1,'Even','Dev',GETDATE(),'Dev',GETDATE())");
            Sql("insert into [Roulette].[Numbers] (Id, Number,ColorId,OddEvenFactor,CreatedByUserId,UpdateDate,UpdatedByUserId,CreateDate) Values(33,'33',2,'Odd','Dev',GETDATE(),'Dev',GETDATE())");
            Sql("insert into [Roulette].[Numbers] (Id, Number,ColorId,OddEvenFactor,CreatedByUserId,UpdateDate,UpdatedByUserId,CreateDate) Values(34,'34',1,'Even','Dev',GETDATE(),'Dev',GETDATE())");
            Sql("insert into [Roulette].[Numbers] (Id, Number,ColorId,OddEvenFactor,CreatedByUserId,UpdateDate,UpdatedByUserId,CreateDate) Values(35,'35',2,'Odd','Dev',GETDATE(),'Dev',GETDATE())");
            Sql("insert into [Roulette].[Numbers] (Id, Number,ColorId,OddEvenFactor,CreatedByUserId,UpdateDate,UpdatedByUserId,CreateDate) Values(36,'36',1,'Even','Dev',GETDATE(),'Dev',GETDATE())");
            Sql(@"SET IDENTITY_INSERT [Roulette].[Numbers] OFF;");

        }

        public override void Down()
        {
        }
    }
}
