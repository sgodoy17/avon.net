namespace IdentiGo.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LOGPETITION",
                c => new
                    {
                        ID = c.Guid(nullable: false, identity: true),
                        PHONEANSWER = c.String(),
                        MESSAGE = c.String(),
                        CODEVALIDATION = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.LOGPETITION");
        }
    }
}
