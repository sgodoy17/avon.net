namespace IdentiGo.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update7 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.USER", "ACTIVE", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.USER", "ACTIVE");
        }
    }
}
