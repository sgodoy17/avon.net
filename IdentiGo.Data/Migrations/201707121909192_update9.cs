namespace IdentiGo.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update9 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.RiskLevelQuota", newName: "QuotaRiskLevel");
            DropPrimaryKey("dbo.QuotaRiskLevel");
            CreateTable(
                "dbo.NOMINATIONRESPONSE",
                c => new
                    {
                        ID = c.Guid(nullable: false, identity: true),
                        NOMINATIONID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.NOMINATION", t => t.NOMINATIONID, cascadeDelete: true)
                .Index(t => t.NOMINATIONID);
            
            AddPrimaryKey("dbo.QuotaRiskLevel", new[] { "Quota_Id", "RiskLevel_Id" });
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NOMINATIONRESPONSE", "NOMINATIONID", "dbo.NOMINATION");
            DropIndex("dbo.NOMINATIONRESPONSE", new[] { "NOMINATIONID" });
            DropPrimaryKey("dbo.QuotaRiskLevel");
            DropTable("dbo.NOMINATIONRESPONSE");
            AddPrimaryKey("dbo.QuotaRiskLevel", new[] { "RiskLevel_Id", "Quota_Id" });
            RenameTable(name: "dbo.QuotaRiskLevel", newName: "RiskLevelQuota");
        }
    }
}
