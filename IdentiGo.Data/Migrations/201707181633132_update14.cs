namespace IdentiGo.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update14 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CONFIG", "EMAILSEND", c => c.String());
            AddColumn("dbo.CONFIG", "EMAILTO", c => c.String());
            AddColumn("dbo.CONFIG", "USERCIFIN", c => c.String());
            AddColumn("dbo.CONFIG", "PASSWORDCIFIN", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CONFIG", "PASSWORDCIFIN");
            DropColumn("dbo.CONFIG", "USERCIFIN");
            DropColumn("dbo.CONFIG", "EMAILTO");
            DropColumn("dbo.CONFIG", "EMAILSEND");
        }
    }
}
