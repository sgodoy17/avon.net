namespace IdentiGo.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update19 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ZONE", "SENDCODE", c => c.Boolean(nullable: false, defaultValue: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ZONE", "SENDCODE");
        }
    }
}
