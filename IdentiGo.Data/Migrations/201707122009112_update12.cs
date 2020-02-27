namespace IdentiGo.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update12 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NOMINATIONRESPONSE", "STAGE", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.NOMINATIONRESPONSE", "STAGE");
        }
    }
}
