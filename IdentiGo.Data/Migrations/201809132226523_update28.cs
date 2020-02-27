namespace IdentiGo.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update28 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NOMINATION", "AVONRISKLEVEL", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.NOMINATION", "AVONRISKLEVEL");
        }
    }
}
