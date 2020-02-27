namespace IdentiGo.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update16 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.CONFIG", "EMAILSEND");
            DropColumn("dbo.CONFIG", "HOST");
            DropColumn("dbo.CONFIG", "PORT");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CONFIG", "PORT", c => c.Int(nullable: false));
            AddColumn("dbo.CONFIG", "HOST", c => c.String());
            AddColumn("dbo.CONFIG", "EMAILSEND", c => c.String());
        }
    }
}
