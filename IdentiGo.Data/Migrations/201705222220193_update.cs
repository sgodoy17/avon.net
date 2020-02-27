namespace IdentiGo.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.QuotaRiskLevel", newName: "RiskLevelQuota");
            DropForeignKey("dbo.CONTROLLERSHIP", "QUEUEID", "dbo.QUEUE");
            DropForeignKey("dbo.FOSYGA", "QUEUEID", "dbo.QUEUE");
            DropForeignKey("dbo.POLICEMAN", "QUEUEID", "dbo.QUEUE");
            DropForeignKey("dbo.REGISTRAR", "QUEUEID", "dbo.QUEUE");
            DropForeignKey("dbo.RUAF", "QUEUEID", "dbo.QUEUE");
            DropForeignKey("dbo.RUAFASSISTANCE", "RUAFID", "dbo.RUAF");
            DropForeignKey("dbo.RUAFCOMPENSATION", "RUAFID", "dbo.RUAF");
            DropForeignKey("dbo.RUAFHEALTH", "RUAFID", "dbo.RUAF");
            DropForeignKey("dbo.RUAFPENSION", "RUAFID", "dbo.RUAF");
            DropForeignKey("dbo.RUAFRISKS", "RUAFID", "dbo.RUAF");
            DropForeignKey("dbo.RUAFSEVERANCE", "RUAFID", "dbo.RUAF");
            DropForeignKey("dbo.SENA", "QUEUEID", "dbo.QUEUE");
            DropForeignKey("dbo.SIMIT", "QUEUEID", "dbo.QUEUE");
            DropForeignKey("dbo.SISBEN", "QUEUEID", "dbo.QUEUE");
            DropForeignKey("dbo.QUESTION", "AFFILIATIONTYPEID", "dbo.AFFILIATIONTYPE");
            DropForeignKey("dbo.ConfigPage", "Config_Id", "dbo.CONFIG");
            DropForeignKey("dbo.ConfigPage", "Page_Id", "dbo.PAGE");
            DropForeignKey("dbo.QUESTION", "PAGEID", "dbo.PAGE");
            DropForeignKey("dbo.QUESTIONOPTION", "QUESTIONID", "dbo.QUESTION");
            DropForeignKey("dbo.QUESTIONOPTION", "QUESTIONFORMID", "dbo.QUESTIONFORM");
            DropForeignKey("dbo.QUESTIONFORM", "USERVALIDATIONID", "dbo.USERVALIDATION");
            DropForeignKey("dbo.QUEUE", "USERVALIDATIONID", "dbo.USERVALIDATION");
            DropForeignKey("dbo.ATTORNEY", "QUEUEID", "dbo.QUEUE");
            DropForeignKey("dbo.RUNT", "QUEUEID", "dbo.QUEUE");
            DropIndex("dbo.ATTORNEY", new[] { "QUEUEID" });
            DropIndex("dbo.QUEUE", new[] { "USERVALIDATIONID" });
            DropIndex("dbo.CONTROLLERSHIP", new[] { "QUEUEID" });
            DropIndex("dbo.FOSYGA", new[] { "QUEUEID" });
            DropIndex("dbo.POLICEMAN", new[] { "QUEUEID" });
            DropIndex("dbo.REGISTRAR", new[] { "QUEUEID" });
            DropIndex("dbo.RUAF", new[] { "QUEUEID" });
            DropIndex("dbo.RUAFASSISTANCE", new[] { "RUAFID" });
            DropIndex("dbo.RUAFCOMPENSATION", new[] { "RUAFID" });
            DropIndex("dbo.RUAFHEALTH", new[] { "RUAFID" });
            DropIndex("dbo.RUAFPENSION", new[] { "RUAFID" });
            DropIndex("dbo.RUAFRISKS", new[] { "RUAFID" });
            DropIndex("dbo.RUAFSEVERANCE", new[] { "RUAFID" });
            DropIndex("dbo.SENA", new[] { "QUEUEID" });
            DropIndex("dbo.SIMIT", new[] { "QUEUEID" });
            DropIndex("dbo.SISBEN", new[] { "QUEUEID" });
            DropIndex("dbo.QUESTIONFORM", new[] { "USERVALIDATIONID" });
            DropIndex("dbo.QUESTIONOPTION", new[] { "QUESTIONFORMID" });
            DropIndex("dbo.QUESTIONOPTION", new[] { "QUESTIONID" });
            DropIndex("dbo.QUESTION", new[] { "PAGEID" });
            DropIndex("dbo.QUESTION", new[] { "AFFILIATIONTYPEID" });
            DropIndex("dbo.RUNT", new[] { "QUEUEID" });
            DropIndex("dbo.ConfigPage", new[] { "Config_Id" });
            DropIndex("dbo.ConfigPage", new[] { "Page_Id" });
            DropPrimaryKey("dbo.RiskLevelQuota");
            CreateTable(
                "dbo.CAMPAING",
                c => new
                    {
                        ID = c.Guid(nullable: false, identity: true),
                        NUMBER = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.USERVALIDATION", "CAMPAINGID", c => c.Guid());
            AddColumn("dbo.USERVALIDATION", "DIVISIONID", c => c.Guid());
            AddColumn("dbo.USERVALIDATION", "ZONEID", c => c.Guid());
            AddColumn("dbo.USERVALIDATION", "UNITID", c => c.Guid());
            AddColumn("dbo.DIVISION", "NUMBER", c => c.String());
            AddColumn("dbo.UNIT", "NUMBER", c => c.String());
            AddColumn("dbo.ZONE", "NUMBER", c => c.String());
            AlterColumn("dbo.USERVALIDATION", "TYPEPHONE", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.RiskLevelQuota", new[] { "RiskLevel_Id", "Quota_Id" });
            CreateIndex("dbo.USERVALIDATION", "CAMPAINGID");
            CreateIndex("dbo.USERVALIDATION", "DIVISIONID");
            CreateIndex("dbo.USERVALIDATION", "ZONEID");
            CreateIndex("dbo.USERVALIDATION", "UNITID");
            AddForeignKey("dbo.USERVALIDATION", "CAMPAINGID", "dbo.CAMPAING", "ID");
            AddForeignKey("dbo.USERVALIDATION", "DIVISIONID", "dbo.DIVISION", "ID");
            AddForeignKey("dbo.USERVALIDATION", "UNITID", "dbo.UNIT", "ID");
            AddForeignKey("dbo.USERVALIDATION", "ZONEID", "dbo.ZONE", "ID");
            DropColumn("dbo.USERVALIDATION", "CAMPAIGN");
            DropColumn("dbo.USERVALIDATION", "CODEDIVISION");
            DropColumn("dbo.USERVALIDATION", "CODEZONE");
            DropColumn("dbo.USERVALIDATION", "CODEUNIT");
            DropColumn("dbo.USERVALIDATION", "UNIT");
            DropColumn("dbo.USERVALIDATION", "CODECAMPAING");
            DropColumn("dbo.DIVISION", "CODE");
            DropColumn("dbo.UNIT", "CODE");
            DropColumn("dbo.ZONE", "CODE");
            DropTable("dbo.ATTORNEY");
            DropTable("dbo.QUEUE");
            DropTable("dbo.CONTROLLERSHIP");
            DropTable("dbo.FOSYGA");
            DropTable("dbo.POLICEMAN");
            DropTable("dbo.REGISTRAR");
            DropTable("dbo.RUAF");
            DropTable("dbo.RUAFASSISTANCE");
            DropTable("dbo.RUAFCOMPENSATION");
            DropTable("dbo.RUAFHEALTH");
            DropTable("dbo.RUAFPENSION");
            DropTable("dbo.RUAFRISKS");
            DropTable("dbo.RUAFSEVERANCE");
            DropTable("dbo.SENA");
            DropTable("dbo.SIMIT");
            DropTable("dbo.SISBEN");
            DropTable("dbo.QUESTIONFORM");
            DropTable("dbo.QUESTIONOPTION");
            DropTable("dbo.QUESTION");
            DropTable("dbo.PAGE");
            DropTable("dbo.RUNT");
            DropTable("dbo.ConfigPage");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ConfigPage",
                c => new
                    {
                        Config_Id = c.Guid(nullable: false),
                        Page_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Config_Id, t.Page_Id });
            
            CreateTable(
                "dbo.RUNT",
                c => new
                    {
                        ID = c.Guid(nullable: false, identity: true),
                        NLICENSE = c.String(),
                        SITEEXPEDITIONLICENSE = c.String(),
                        EXPEDITIONDATE = c.String(),
                        STATE = c.String(),
                        RESTRITION = c.String(),
                        DETAIL = c.String(),
                        QUEUEID = c.Guid(nullable: false),
                        DOCUMENT = c.String(),
                        ERROR = c.String(),
                        NUMERROR = c.Int(nullable: false),
                        SUCCESS = c.Boolean(nullable: false),
                        CONSULT = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.PAGE",
                c => new
                    {
                        ID = c.Guid(nullable: false, identity: true),
                        NAME = c.String(),
                        URL = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.QUESTION",
                c => new
                    {
                        ID = c.Guid(nullable: false, identity: true),
                        TEXT = c.String(),
                        TYPEQUESTION = c.Int(nullable: false),
                        PAGEID = c.Guid(nullable: false),
                        AFFILIATIONTYPEID = c.Guid(),
                        NUMBEROPTION = c.Int(nullable: false),
                        TYPECONTROL = c.Int(nullable: false),
                        FIELD = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.QUESTIONOPTION",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        QUESTIONFORMID = c.Guid(nullable: false),
                        QUESTIONID = c.Guid(nullable: false),
                        TEXT = c.String(),
                        VALID = c.Boolean(nullable: false),
                        SELECTED = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.QUESTIONFORM",
                c => new
                    {
                        ID = c.Guid(nullable: false, identity: true),
                        USERVALIDATIONID = c.Guid(nullable: false),
                        STATE = c.Int(nullable: false),
                        DATECREATE = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.SISBEN",
                c => new
                    {
                        ID = c.Guid(nullable: false, identity: true),
                        SCORE = c.String(),
                        STATE = c.String(),
                        DEPARTMENT = c.String(),
                        MUNICIPALITY = c.String(),
                        AREA = c.String(),
                        TAB = c.String(),
                        MODIFICATIONDATE = c.String(),
                        QUEUEID = c.Guid(nullable: false),
                        DOCUMENT = c.String(),
                        ERROR = c.String(),
                        NUMERROR = c.Int(nullable: false),
                        SUCCESS = c.Boolean(nullable: false),
                        CONSULT = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.SIMIT",
                c => new
                    {
                        ID = c.Guid(nullable: false, identity: true),
                        VIOLATIONS = c.String(),
                        QUEUEID = c.Guid(nullable: false),
                        DOCUMENT = c.String(),
                        ERROR = c.String(),
                        NUMERROR = c.Int(nullable: false),
                        SUCCESS = c.Boolean(nullable: false),
                        CONSULT = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.SENA",
                c => new
                    {
                        ID = c.Guid(nullable: false, identity: true),
                        ADDRESS = c.String(),
                        PHONE = c.String(),
                        CELL = c.String(),
                        MAIL = c.String(),
                        BIRTHDATE = c.String(),
                        QUEUEID = c.Guid(nullable: false),
                        DOCUMENT = c.String(),
                        ERROR = c.String(),
                        NUMERROR = c.Int(nullable: false),
                        SUCCESS = c.Boolean(nullable: false),
                        CONSULT = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.RUAFSEVERANCE",
                c => new
                    {
                        RUAFID = c.Guid(nullable: false),
                        SEVERANCEREGIME = c.String(),
                        SEVERANCEADMINISTRATOR = c.String(),
                        SEVERANCEDATEAFFILIATION = c.String(),
                        SEVERANCESTATEAFFILIATE = c.String(),
                        SEVERANCEJOBLOCATION = c.String(),
                    })
                .PrimaryKey(t => t.RUAFID);
            
            CreateTable(
                "dbo.RUAFRISKS",
                c => new
                    {
                        RUAFID = c.Guid(nullable: false),
                        RISKSREGIME = c.String(),
                        RISKSADMINISTRATOR = c.String(),
                        RISKSDATEAFFILIATION = c.String(),
                        RISKSSTATEAFFILIATE = c.String(),
                        RISKSTYPEAFFILIATE = c.String(),
                        RISKSECONOMICACTIVITY = c.String(),
                        RISKSJOBLOCATION = c.String(),
                    })
                .PrimaryKey(t => t.RUAFID);
            
            CreateTable(
                "dbo.RUAFPENSION",
                c => new
                    {
                        RUAFID = c.Guid(nullable: false),
                        PENSIONREGIME = c.String(),
                        PENSIONADMINISTRATOR = c.String(),
                        PENSIONDATEAFFILIATION = c.String(),
                        PENSIONSTATEAFFILIATE = c.String(),
                        PENSIONSUBSIDIZEDAFFILIATE = c.String(),
                    })
                .PrimaryKey(t => t.RUAFID);
            
            CreateTable(
                "dbo.RUAFHEALTH",
                c => new
                    {
                        RUAFID = c.Guid(nullable: false),
                        HEALTHREGIME = c.String(),
                        HEALTHADMINISTRATOR = c.String(),
                        HEALTHDATEAFFILIATION = c.String(),
                        HEALTHSTATEAFFILIATE = c.String(),
                        HEALTHTYPEAFFILIATE = c.String(),
                        HEALTHLOCATION = c.String(),
                    })
                .PrimaryKey(t => t.RUAFID);
            
            CreateTable(
                "dbo.RUAFCOMPENSATION",
                c => new
                    {
                        RUAFID = c.Guid(nullable: false),
                        COMPENSATIONADMINISTRATOR = c.String(),
                        COMPENSATIONDATEAFFILIATION = c.String(),
                        COMPENSATIONSTATEAFFILIATE = c.String(),
                        COMPENSATIONMEMBERTYPEPOPULATION = c.String(),
                        COMPENSATIONTYPEAFFILIATE = c.String(),
                        COMPENSATIONJOBLOCATION = c.String(),
                    })
                .PrimaryKey(t => t.RUAFID);
            
            CreateTable(
                "dbo.RUAFASSISTANCE",
                c => new
                    {
                        RUAFID = c.Guid(nullable: false),
                        ASSISTANCEADMINISTRATOR = c.String(),
                        ASSISTANCEPROGRAM = c.String(),
                        ASSISTANCEDATEAFFILIATION = c.String(),
                        ASSISTANCESTATEAFFILIATE = c.String(),
                        ASSISTANCESTATEBENEFIT = c.String(),
                        ASSISTANCETYPEBENEFIT = c.String(),
                        ASSISTANCETYPESUNBSIDIO = c.String(),
                        ASSISTANCEDATELASTBENEFIT = c.String(),
                        ASSISTANCEVALUEBENEFIT = c.String(),
                        ASSISTANCETYPEAFFILIATE = c.String(),
                        ASSISTANCEECONOMICACTIVITY = c.String(),
                        ASSISTANCEDELIVERYLOCATIONBENEFIT = c.String(),
                    })
                .PrimaryKey(t => t.RUAFID);
            
            CreateTable(
                "dbo.RUAF",
                c => new
                    {
                        ID = c.Guid(nullable: false, identity: true),
                        QUEUEID = c.Guid(nullable: false),
                        DOCUMENT = c.String(),
                        ERROR = c.String(),
                        NUMERROR = c.Int(nullable: false),
                        SUCCESS = c.Boolean(nullable: false),
                        CONSULT = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.REGISTRAR",
                c => new
                    {
                        ID = c.Guid(nullable: false, identity: true),
                        DEPARTMENT = c.String(),
                        MUNICIPALITY = c.String(),
                        SINCE = c.String(),
                        ADDRESSSINCE = c.String(),
                        DATEREGISTRATION = c.String(),
                        TABLE = c.String(),
                        QUEUEID = c.Guid(nullable: false),
                        DOCUMENT = c.String(),
                        ERROR = c.String(),
                        NUMERROR = c.Int(nullable: false),
                        SUCCESS = c.Boolean(nullable: false),
                        CONSULT = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.POLICEMAN",
                c => new
                    {
                        ID = c.Guid(nullable: false, identity: true),
                        RESULT = c.String(),
                        QUEUEID = c.Guid(nullable: false),
                        DOCUMENT = c.String(),
                        ERROR = c.String(),
                        NUMERROR = c.Int(nullable: false),
                        SUCCESS = c.Boolean(nullable: false),
                        CONSULT = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.FOSYGA",
                c => new
                    {
                        ID = c.Guid(nullable: false, identity: true),
                        DEPARTMENT = c.String(),
                        MUNICIPALITY = c.String(),
                        STATEAFFILIATION = c.String(),
                        ENTITY = c.String(),
                        REGIME = c.String(),
                        DATEMEMBERSHIP = c.String(),
                        DATELASTNOVELTY = c.String(),
                        AFFILIATETYPE = c.String(),
                        QUEUEID = c.Guid(nullable: false),
                        DOCUMENT = c.String(),
                        ERROR = c.String(),
                        NUMERROR = c.Int(nullable: false),
                        SUCCESS = c.Boolean(nullable: false),
                        CONSULT = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.CONTROLLERSHIP",
                c => new
                    {
                        ID = c.Guid(nullable: false, identity: true),
                        RESULT = c.String(),
                        QUEUEID = c.Guid(nullable: false),
                        DOCUMENT = c.String(),
                        ERROR = c.String(),
                        NUMERROR = c.Int(nullable: false),
                        SUCCESS = c.Boolean(nullable: false),
                        CONSULT = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.QUEUE",
                c => new
                    {
                        ID = c.Guid(nullable: false, identity: true),
                        USERVALIDATIONID = c.Guid(nullable: false),
                        DOCUMENT = c.String(nullable: false),
                        ATTORNEY = c.Int(nullable: false),
                        CONTROLLERSSHIP = c.Int(nullable: false),
                        FOSYGA = c.Int(nullable: false),
                        POLICEMAN = c.Int(nullable: false),
                        REGISTRAR = c.Int(nullable: false),
                        RUAF = c.Int(nullable: false),
                        RUNT = c.Int(nullable: false),
                        SENA = c.Int(nullable: false),
                        SIMIT = c.Int(nullable: false),
                        SISBEN = c.Int(nullable: false),
                        FINISH = c.Boolean(nullable: false),
                        DATECREATED = c.DateTime(nullable: false),
                        DATEUPDATE = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ATTORNEY",
                c => new
                    {
                        ID = c.Guid(nullable: false, identity: true),
                        RESULT = c.String(),
                        QUEUEID = c.Guid(nullable: false),
                        DOCUMENT = c.String(),
                        ERROR = c.String(),
                        NUMERROR = c.Int(nullable: false),
                        SUCCESS = c.Boolean(nullable: false),
                        CONSULT = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.ZONE", "CODE", c => c.String());
            AddColumn("dbo.UNIT", "CODE", c => c.String());
            AddColumn("dbo.DIVISION", "CODE", c => c.String());
            AddColumn("dbo.USERVALIDATION", "CODECAMPAING", c => c.String());
            AddColumn("dbo.USERVALIDATION", "UNIT", c => c.String());
            AddColumn("dbo.USERVALIDATION", "CODEUNIT", c => c.String());
            AddColumn("dbo.USERVALIDATION", "CODEZONE", c => c.String());
            AddColumn("dbo.USERVALIDATION", "CODEDIVISION", c => c.String());
            AddColumn("dbo.USERVALIDATION", "CAMPAIGN", c => c.String());
            DropForeignKey("dbo.USERVALIDATION", "ZONEID", "dbo.ZONE");
            DropForeignKey("dbo.USERVALIDATION", "UNITID", "dbo.UNIT");
            DropForeignKey("dbo.USERVALIDATION", "DIVISIONID", "dbo.DIVISION");
            DropForeignKey("dbo.USERVALIDATION", "CAMPAINGID", "dbo.CAMPAING");
            DropIndex("dbo.USERVALIDATION", new[] { "UNITID" });
            DropIndex("dbo.USERVALIDATION", new[] { "ZONEID" });
            DropIndex("dbo.USERVALIDATION", new[] { "DIVISIONID" });
            DropIndex("dbo.USERVALIDATION", new[] { "CAMPAINGID" });
            DropPrimaryKey("dbo.RiskLevelQuota");
            AlterColumn("dbo.USERVALIDATION", "TYPEPHONE", c => c.String());
            DropColumn("dbo.ZONE", "NUMBER");
            DropColumn("dbo.UNIT", "NUMBER");
            DropColumn("dbo.DIVISION", "NUMBER");
            DropColumn("dbo.USERVALIDATION", "UNITID");
            DropColumn("dbo.USERVALIDATION", "ZONEID");
            DropColumn("dbo.USERVALIDATION", "DIVISIONID");
            DropColumn("dbo.USERVALIDATION", "CAMPAINGID");
            DropTable("dbo.CAMPAING");
            AddPrimaryKey("dbo.RiskLevelQuota", new[] { "Quota_Id", "RiskLevel_Id" });
            CreateIndex("dbo.ConfigPage", "Page_Id");
            CreateIndex("dbo.ConfigPage", "Config_Id");
            CreateIndex("dbo.RUNT", "QUEUEID");
            CreateIndex("dbo.QUESTION", "AFFILIATIONTYPEID");
            CreateIndex("dbo.QUESTION", "PAGEID");
            CreateIndex("dbo.QUESTIONOPTION", "QUESTIONID");
            CreateIndex("dbo.QUESTIONOPTION", "QUESTIONFORMID");
            CreateIndex("dbo.QUESTIONFORM", "USERVALIDATIONID");
            CreateIndex("dbo.SISBEN", "QUEUEID");
            CreateIndex("dbo.SIMIT", "QUEUEID");
            CreateIndex("dbo.SENA", "QUEUEID");
            CreateIndex("dbo.RUAFSEVERANCE", "RUAFID");
            CreateIndex("dbo.RUAFRISKS", "RUAFID");
            CreateIndex("dbo.RUAFPENSION", "RUAFID");
            CreateIndex("dbo.RUAFHEALTH", "RUAFID");
            CreateIndex("dbo.RUAFCOMPENSATION", "RUAFID");
            CreateIndex("dbo.RUAFASSISTANCE", "RUAFID");
            CreateIndex("dbo.RUAF", "QUEUEID");
            CreateIndex("dbo.REGISTRAR", "QUEUEID");
            CreateIndex("dbo.POLICEMAN", "QUEUEID");
            CreateIndex("dbo.FOSYGA", "QUEUEID");
            CreateIndex("dbo.CONTROLLERSHIP", "QUEUEID");
            CreateIndex("dbo.QUEUE", "USERVALIDATIONID");
            CreateIndex("dbo.ATTORNEY", "QUEUEID");
            AddForeignKey("dbo.RUNT", "QUEUEID", "dbo.QUEUE", "ID", cascadeDelete: true);
            AddForeignKey("dbo.ATTORNEY", "QUEUEID", "dbo.QUEUE", "ID", cascadeDelete: true);
            AddForeignKey("dbo.QUEUE", "USERVALIDATIONID", "dbo.USERVALIDATION", "ID", cascadeDelete: true);
            AddForeignKey("dbo.QUESTIONFORM", "USERVALIDATIONID", "dbo.USERVALIDATION", "ID", cascadeDelete: true);
            AddForeignKey("dbo.QUESTIONOPTION", "QUESTIONFORMID", "dbo.QUESTIONFORM", "ID", cascadeDelete: true);
            AddForeignKey("dbo.QUESTIONOPTION", "QUESTIONID", "dbo.QUESTION", "ID", cascadeDelete: true);
            AddForeignKey("dbo.QUESTION", "PAGEID", "dbo.PAGE", "ID", cascadeDelete: true);
            AddForeignKey("dbo.ConfigPage", "Page_Id", "dbo.PAGE", "ID", cascadeDelete: true);
            AddForeignKey("dbo.ConfigPage", "Config_Id", "dbo.CONFIG", "ID", cascadeDelete: true);
            AddForeignKey("dbo.QUESTION", "AFFILIATIONTYPEID", "dbo.AFFILIATIONTYPE", "ID");
            AddForeignKey("dbo.SISBEN", "QUEUEID", "dbo.QUEUE", "ID", cascadeDelete: true);
            AddForeignKey("dbo.SIMIT", "QUEUEID", "dbo.QUEUE", "ID", cascadeDelete: true);
            AddForeignKey("dbo.SENA", "QUEUEID", "dbo.QUEUE", "ID", cascadeDelete: true);
            AddForeignKey("dbo.RUAFSEVERANCE", "RUAFID", "dbo.RUAF", "ID");
            AddForeignKey("dbo.RUAFRISKS", "RUAFID", "dbo.RUAF", "ID");
            AddForeignKey("dbo.RUAFPENSION", "RUAFID", "dbo.RUAF", "ID");
            AddForeignKey("dbo.RUAFHEALTH", "RUAFID", "dbo.RUAF", "ID");
            AddForeignKey("dbo.RUAFCOMPENSATION", "RUAFID", "dbo.RUAF", "ID");
            AddForeignKey("dbo.RUAFASSISTANCE", "RUAFID", "dbo.RUAF", "ID");
            AddForeignKey("dbo.RUAF", "QUEUEID", "dbo.QUEUE", "ID", cascadeDelete: true);
            AddForeignKey("dbo.REGISTRAR", "QUEUEID", "dbo.QUEUE", "ID", cascadeDelete: true);
            AddForeignKey("dbo.POLICEMAN", "QUEUEID", "dbo.QUEUE", "ID", cascadeDelete: true);
            AddForeignKey("dbo.FOSYGA", "QUEUEID", "dbo.QUEUE", "ID", cascadeDelete: true);
            AddForeignKey("dbo.CONTROLLERSHIP", "QUEUEID", "dbo.QUEUE", "ID", cascadeDelete: true);
            RenameTable(name: "dbo.RiskLevelQuota", newName: "QuotaRiskLevel");
        }
    }
}
