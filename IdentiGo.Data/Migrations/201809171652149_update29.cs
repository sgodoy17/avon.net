namespace IdentiGo.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update29 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.NOMINATION", "DOCUMENT", c => c.String(maxLength: 40));
            CreateIndex("dbo.NOMINATION", "DOCUMENT", unique: true);
        }
        
        public override void Down()
        {
            DropColumn("dbo.NOMINATION", "DOCUMENT");
            AlterColumn("dbo.NOMINATION", "DOCUMENT", c => c.String());
        }
    }
}
