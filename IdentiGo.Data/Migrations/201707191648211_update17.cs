namespace IdentiGo.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update17 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ZONE", "SELFSEND", c => c.Boolean(nullable: false, defaultValue: true));
            AddColumn("dbo.ZONE", "NUMBERTRYS", c => c.Int(nullable: false, defaultValue: 0));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ZONE", "SELFSEND");
            DropColumn("dbo.ZONE", "NUMBERTRYS");
        }
    }
}
