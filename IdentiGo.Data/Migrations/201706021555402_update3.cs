namespace IdentiGo.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.NOMINATION", "CODEVERIFICATION", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.NOMINATION", "CODEVERIFICATION", c => c.String());
        }
    }
}
