namespace IdentiGo.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update24 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NOMINATIONHISTORIC", "CODEUSER", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.NOMINATIONHISTORIC", "CODEUSER");
        }
    }
}
