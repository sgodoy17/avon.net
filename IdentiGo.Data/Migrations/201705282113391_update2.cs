namespace IdentiGo.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update2 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.USERVALIDATION", newName: "NOMINATION");
            DropForeignKey("dbo.VALIDADORPLUS", "USERVALIDATIONID", "dbo.USERVALIDATION");
            DropForeignKey("dbo.PROSPECTA", "USERVALIDATIONID", "dbo.USERVALIDATION");
            DropIndex("dbo.VALIDADORPLUS", new[] { "USERVALIDATIONID" });
            DropIndex("dbo.PROSPECTA", new[] { "USERVALIDATIONID" });
            RenameColumn(table: "dbo.VALIDADORPLUS", name: "USERVALIDATIONID", newName: "NOMINATIONID");
            RenameColumn(table: "dbo.PROSPECTA", name: "USERVALIDATIONID", newName: "NOMINATIONID");
            CreateTable(
                "dbo.LOGIVR",
                c => new
                    {
                        ID = c.Guid(nullable: false, identity: true),
                        PHONEANSWER = c.String(),
                        MESSAGE = c.String(),
                        CAMPID = c.String(),
                        INPUT = c.Boolean(nullable: false),
                        DATE = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.LOGSMS",
                c => new
                    {
                        ID = c.Guid(nullable: false, identity: true),
                        PHONEANSWER = c.String(),
                        MESSAGE = c.String(),
                        CODEVALIDATION = c.String(),
                        INPUT = c.Boolean(nullable: false),
                        DATE = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.NOMINATION", "DATENOMINATION", c => c.DateTime());
            AddColumn("dbo.NOMINATION", "STATEDOCUMENT", c => c.Int(nullable: false));
            AddColumn("dbo.NOMINATION", "STAGEPROCESS", c => c.Int(nullable: false));
            AlterColumn("dbo.VALIDADORPLUS", "NOMINATIONID", c => c.Guid(nullable: false));
            AlterColumn("dbo.PROSPECTA", "NOMINATIONID", c => c.Guid(nullable: false));
            CreateIndex("dbo.VALIDADORPLUS", "NOMINATIONID");
            CreateIndex("dbo.PROSPECTA", "NOMINATIONID");
            AddForeignKey("dbo.VALIDADORPLUS", "NOMINATIONID", "dbo.NOMINATION", "ID", cascadeDelete: true);
            AddForeignKey("dbo.PROSPECTA", "NOMINATIONID", "dbo.NOMINATION", "ID", cascadeDelete: true);
            DropColumn("dbo.NOMINATION", "STAGEPROCCESS");
            DropTable("dbo.LOGPETITION");
        }
        
        public override void Down()
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
            
            AddColumn("dbo.NOMINATION", "STAGEPROCCESS", c => c.Int(nullable: false));
            DropForeignKey("dbo.PROSPECTA", "NOMINATIONID", "dbo.NOMINATION");
            DropForeignKey("dbo.VALIDADORPLUS", "NOMINATIONID", "dbo.NOMINATION");
            DropIndex("dbo.PROSPECTA", new[] { "NOMINATIONID" });
            DropIndex("dbo.VALIDADORPLUS", new[] { "NOMINATIONID" });
            AlterColumn("dbo.PROSPECTA", "NOMINATIONID", c => c.Guid());
            AlterColumn("dbo.VALIDADORPLUS", "NOMINATIONID", c => c.Guid());
            DropColumn("dbo.NOMINATION", "STAGEPROCESS");
            DropColumn("dbo.NOMINATION", "STATEDOCUMENT");
            DropColumn("dbo.NOMINATION", "DATENOMINATION");
            DropTable("dbo.LOGSMS");
            DropTable("dbo.LOGIVR");
            RenameColumn(table: "dbo.PROSPECTA", name: "NOMINATIONID", newName: "USERVALIDATIONID");
            RenameColumn(table: "dbo.VALIDADORPLUS", name: "NOMINATIONID", newName: "USERVALIDATIONID");
            CreateIndex("dbo.PROSPECTA", "USERVALIDATIONID");
            CreateIndex("dbo.VALIDADORPLUS", "USERVALIDATIONID");
            AddForeignKey("dbo.PROSPECTA", "USERVALIDATIONID", "dbo.USERVALIDATION", "ID");
            AddForeignKey("dbo.VALIDADORPLUS", "USERVALIDATIONID", "dbo.USERVALIDATION", "ID");
            RenameTable(name: "dbo.NOMINATION", newName: "USERVALIDATION");
        }
    }
}
