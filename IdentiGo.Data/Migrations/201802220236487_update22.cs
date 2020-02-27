namespace IdentiGo.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update22 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.NOMINATIONHISTORIC", "CODEVERIFICATION", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.NOMINATIONHISTORIC", "CODEVERIFICATION", c => c.String());
        }
    }
}
