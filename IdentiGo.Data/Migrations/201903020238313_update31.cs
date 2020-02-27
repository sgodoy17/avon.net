namespace IdentiGo.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update31 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CASHPAYMENT", "RESULT", c => c.Int(nullable: true));
            AddColumn("dbo.CASHPAYMENT", "DESCRIPTION", c => c.String(nullable: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CASHPAYMENT", "RESULT");
            DropColumn("dbo.CASHPAYMENT", "DESCRIPTION");
        }
    }
}
