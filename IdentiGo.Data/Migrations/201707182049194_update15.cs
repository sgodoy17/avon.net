namespace IdentiGo.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update15 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CONFIG", "SUBJECT", c => c.String());
            AddColumn("dbo.CONFIG", "HOST", c => c.String());
            AddColumn("dbo.CONFIG", "PORT", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CONFIG", "PORT");
            DropColumn("dbo.CONFIG", "HOST");
            DropColumn("dbo.CONFIG", "SUBJECT");
        }
    }
}
