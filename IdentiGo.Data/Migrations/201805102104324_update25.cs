namespace IdentiGo.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update25 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.USER", "CODEVERIFICATION", c => c.String());
        }

        public override void Down()
        {
            AlterColumn("dbo.USER", "CODEVERIFICATION", c => c.Int());
        }
    }
}
