namespace IdentiGo.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update8 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DIVISION", "NAME", c => c.String());
            AddColumn("dbo.UNIT", "NAME", c => c.String());
            AddColumn("dbo.ZONE", "NAME", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ZONE", "NAME");
            DropColumn("dbo.UNIT", "NAME");
            DropColumn("dbo.DIVISION", "NAME");
        }
    }
}
