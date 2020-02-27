namespace IdentiGo.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class update30 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CASHPAYMENT",
                c => new
                {
                    ID = c.Guid(nullable: false, identity: true),
                    NOMINATIONID = c.Guid(nullable: false),
                    DOCUMENT = c.Int(nullable: false),
                    GENRE = c.String(nullable: false),
                    RISK = c.String(nullable: false),
                    BIRTHDATE = c.Int(nullable: false),
                    DIVISION = c.Int(nullable: false),
                    ZONE = c.Int(nullable: false),
                    UNIT = c.Int(nullable: false),
                    DATECREATED = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.NOMINATION", t => t.NOMINATIONID, cascadeDelete: true)
                .Index(t => t.NOMINATIONID);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CASHPAYMENT", "NOMINATIONID", "dbo.NOMINATION");
            DropIndex("dbo.CASHPAYMENT", new[] { "NOMINATIONID" });
            DropTable("dbo.CASHPAYMENT");
        }
    }
}
