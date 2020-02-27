namespace IdentiGo.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update20 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NOMINATIONHISTORIC",
                c => new
                {
                    ID = c.Guid(nullable: false, identity: true),
                    NOMINATIONID = c.Guid(nullable: false),
                    DATE = c.DateTime(nullable: false),
                    DATECREATED = c.DateTime(nullable: false),
                    CODEVIRIFICATION = c.String(),
                })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.NOMINATION", t => t.NOMINATIONID, cascadeDelete: true)
                .Index(t => t.NOMINATIONID);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NOMINATIONHISTORIC", "NOMINATIONID", "dbo.NOMINATION");
            DropIndex("dbo.NOMINATIONHISTORIC", new[] { "NOMINATIONID" });
            DropTable("dbo.NOMINATIONHISTORIC");
        }
    }
}
