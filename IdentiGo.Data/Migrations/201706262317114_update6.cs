namespace IdentiGo.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update6 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.USER", "CODEVERIFICATION", c => c.Int());
            AddColumn("dbo.DIVISION", "CODE", c => c.String());
            AddColumn("dbo.UNIT", "CODE", c => c.String());
            AddColumn("dbo.ZONE", "CODE", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ZONE", "CODE");
            DropColumn("dbo.UNIT", "CODE");
            DropColumn("dbo.DIVISION", "CODE");
            DropColumn("dbo.USER", "CODEVERIFICATION");
        }
    }
}
