namespace IdentiGo.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update26 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NOMINATION", "SEX", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.NOMINATION", "SEX");
        }
    }
}
