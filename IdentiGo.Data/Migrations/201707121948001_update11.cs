namespace IdentiGo.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update11 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NOMINATIONRESPONSE", "MESSAGE", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.NOMINATIONRESPONSE", "MESSAGE");
        }
    }
}
