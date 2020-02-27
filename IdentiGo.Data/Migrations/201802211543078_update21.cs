namespace IdentiGo.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update21 : DbMigration
    {
        public override void Up()
        {
            RenameColumn("dbo.NOMINATIONHISTORIC", "CODEVIRIFICATION", "CODEVERIFICATION");
        }
        
        public override void Down()
        {
            RenameColumn("dbo.NOMINATIONHISTORIC", "CODEVERIFICATION", "CODEVIRIFICATION");
        }
    }
}
