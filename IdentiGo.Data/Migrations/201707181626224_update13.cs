namespace IdentiGo.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update13 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CONFIG", "COMPANYID", "dbo.COMPANY");
            DropIndex("dbo.CONFIG", new[] { "COMPANYID" });
            DropColumn("dbo.CONFIG", "NUMBERQUESTION");
            DropColumn("dbo.CONFIG", "NUMBERQUESTIONVALID");
            DropColumn("dbo.CONFIG", "COMPANYID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CONFIG", "COMPANYID", c => c.Int());
            AddColumn("dbo.CONFIG", "NUMBERQUESTIONVALID", c => c.Int(nullable: false));
            AddColumn("dbo.CONFIG", "NUMBERQUESTION", c => c.Int(nullable: false));
            CreateIndex("dbo.CONFIG", "COMPANYID");
            AddForeignKey("dbo.CONFIG", "COMPANYID", "dbo.COMPANY", "ID");
        }
    }
}
