namespace IdentiGo.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update23 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NOMINATIONHISTORIC", "PHONENUMBER", c => c.String());
            AddColumn("dbo.NOMINATIONHISTORIC", "PHONEANSWER", c => c.String());
            AddColumn("dbo.NOMINATIONHISTORIC", "STATE", c => c.Int());
            AddColumn("dbo.NOMINATIONHISTORIC", "STATEDOCUMENT", c => c.Int());
            AddColumn("dbo.NOMINATIONHISTORIC", "STAGEPROCESS", c => c.Int());
            AddColumn("dbo.NOMINATIONHISTORIC", "CAMPAINGID", c => c.Guid());
            CreateIndex("dbo.NOMINATIONHISTORIC", "CAMPAINGID");
            AddForeignKey("dbo.NOMINATIONHISTORIC", "CAMPAINGID", "dbo.CAMPAING", "ID");
            AddColumn("dbo.NOMINATIONHISTORIC", "DIVISIONID", c => c.Guid());
            CreateIndex("dbo.NOMINATIONHISTORIC", "DIVISIONID");
            AddForeignKey("dbo.NOMINATIONHISTORIC", "DIVISIONID", "dbo.DIVISION", "ID");
            AddColumn("dbo.NOMINATIONHISTORIC", "ZONEID", c => c.Guid());
            CreateIndex("dbo.NOMINATIONHISTORIC", "ZONEID");
            AddForeignKey("dbo.NOMINATIONHISTORIC", "ZONEID", "dbo.ZONE", "ID");
            AddColumn("dbo.NOMINATIONHISTORIC", "UNITID", c => c.Guid());
            CreateIndex("dbo.NOMINATIONHISTORIC", "UNITID");
            AddForeignKey("dbo.NOMINATIONHISTORIC", "UNITID", "dbo.UNIT", "ID");
        }
        
        public override void Down()
        {
            DropColumn("dbo.NOMINATIONHISTORIC", "PHONENUMBER");
            DropColumn("dbo.NOMINATIONHISTORIC", "PHONEANSWER");
            DropColumn("dbo.NOMINATIONHISTORIC", "STATE");
            DropColumn("dbo.NOMINATIONHISTORIC", "STATEDOCUMENT");
            DropColumn("dbo.NOMINATIONHISTORIC", "STAGEPROCESS");
            DropForeignKey("dbo.NOMINATIONHISTORIC", "CAMPAINGID", "dbo.CAMPAING");
            DropIndex("dbo.NOMINATIONHISTORIC", new[] { "CAMPAINGID" });
            DropColumn("dbo.NOMINATIONHISTORIC", "CAMPAINGID");
            DropForeignKey("dbo.NOMINATIONHISTORIC", "DIVISIONID", "dbo.DIVISION");
            DropIndex("dbo.NOMINATIONHISTORIC", new[] { "DIVISIONID" });
            DropColumn("dbo.NOMINATIONHISTORIC", "DIVISIONID");
            DropForeignKey("dbo.NOMINATIONHISTORIC", "ZONEID", "dbo.ZONE");
            DropIndex("dbo.NOMINATIONHISTORIC", new[] { "ZONEID" });
            DropColumn("dbo.NOMINATIONHISTORIC", "ZONEID");
            DropForeignKey("dbo.NOMINATIONHISTORIC", "UNITID", "dbo.UNIT");
            DropIndex("dbo.NOMINATIONHISTORIC", new[] { "UNITID" });
            DropColumn("dbo.NOMINATIONHISTORIC", "UNITID");
        }
    }
}
