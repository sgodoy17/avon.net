namespace IdentiGo.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update18 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LOGSMS", "DOCUMENT", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.LOGSMS", "DOCUMENT");
        }
    }
}
