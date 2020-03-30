namespace Roulette.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addadminusertosystem : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO [Roulette].[Users]([UserName],[Password],[PasswordSalt],[CreatedByUserId],[CreateDate],[UpdateDate],[UpdatedByUserId])VALUES('Admin','An++BolmP6iz0YBhXIskQI9wO0Y8n1mr48sdfKIbkPE=','49f791f0d073468585861c79d5ccd0b3','Dev',GETDATE(),GETDATE(),'Dev')");
        }
        
        public override void Down()
        {
        }
    }
}
