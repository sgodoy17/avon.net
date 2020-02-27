namespace IdentiGo.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AFFILIATIONTYPE",
                c => new
                    {
                        ID = c.Guid(nullable: false, identity: true),
                        NAME = c.String(),
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
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.QUEUE", t => t.QUEUEID, cascadeDelete: true)
                .Index(t => t.QUEUEID);
            
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
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.USERVALIDATION", t => t.USERVALIDATIONID, cascadeDelete: true)
                .Index(t => t.USERVALIDATIONID);
            
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
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.QUEUE", t => t.QUEUEID, cascadeDelete: true)
                .Index(t => t.QUEUEID);
            
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
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.QUEUE", t => t.QUEUEID, cascadeDelete: true)
                .Index(t => t.QUEUEID);
            
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
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.QUEUE", t => t.QUEUEID, cascadeDelete: true)
                .Index(t => t.QUEUEID);
            
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
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.QUEUE", t => t.QUEUEID, cascadeDelete: true)
                .Index(t => t.QUEUEID);
            
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
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.QUEUE", t => t.QUEUEID, cascadeDelete: true)
                .Index(t => t.QUEUEID);
            
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
                .PrimaryKey(t => t.RUAFID)
                .ForeignKey("dbo.RUAF", t => t.RUAFID)
                .Index(t => t.RUAFID);
            
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
                .PrimaryKey(t => t.RUAFID)
                .ForeignKey("dbo.RUAF", t => t.RUAFID)
                .Index(t => t.RUAFID);
            
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
                .PrimaryKey(t => t.RUAFID)
                .ForeignKey("dbo.RUAF", t => t.RUAFID)
                .Index(t => t.RUAFID);
            
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
                .PrimaryKey(t => t.RUAFID)
                .ForeignKey("dbo.RUAF", t => t.RUAFID)
                .Index(t => t.RUAFID);
            
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
                .PrimaryKey(t => t.RUAFID)
                .ForeignKey("dbo.RUAF", t => t.RUAFID)
                .Index(t => t.RUAFID);
            
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
                .PrimaryKey(t => t.RUAFID)
                .ForeignKey("dbo.RUAF", t => t.RUAFID)
                .Index(t => t.RUAFID);
            
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
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.QUEUE", t => t.QUEUEID, cascadeDelete: true)
                .Index(t => t.QUEUEID);
            
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
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.QUEUE", t => t.QUEUEID, cascadeDelete: true)
                .Index(t => t.QUEUEID);
            
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
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.QUEUE", t => t.QUEUEID, cascadeDelete: true)
                .Index(t => t.QUEUEID);
            
            CreateTable(
                "dbo.USERVALIDATION",
                c => new
                    {
                        ID = c.Guid(nullable: false, identity: true),
                        USERID = c.Guid(),
                        DOCUMENT = c.String(nullable: false),
                        NAME = c.String(),
                        LASTNAME = c.String(),
                        PHONENUMBER = c.String(),
                        PHONEANSWER = c.String(),
                        SCORE = c.String(),
                        DATECREATED = c.DateTime(nullable: false),
                        DATEUPDATE = c.DateTime(nullable: false),
                        DATELASTVALIDATION = c.DateTime(nullable: false),
                        STATE = c.Int(nullable: false),
                        STAGEPROCCESS = c.Int(nullable: false),
                        NUMBERINTENTBYDAY = c.Int(nullable: false),
                        TOTALNUMBERINTENT = c.Int(nullable: false),
                        CODEUSER = c.String(),
                        CODEVERIFICATION = c.String(),
                        CAMPAIGN = c.String(),
                        CODEDIVISION = c.String(),
                        CODEZONE = c.String(),
                        CODEUNIT = c.String(),
                        UNIT = c.String(),
                        RISKLEVELID = c.Guid(),
                        CODECAMPAING = c.String(),
                        TYPEPHONE = c.String(),
                        TYPEPROCESS = c.Int(nullable: false),
                        Company_Id = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.COMPANY", t => t.Company_Id)
                .ForeignKey("dbo.RISKLEVEL", t => t.RISKLEVELID)
                .ForeignKey("dbo.USER", t => t.USERID)
                .Index(t => t.USERID)
                .Index(t => t.RISKLEVELID)
                .Index(t => t.Company_Id);
            
            CreateTable(
                "dbo.COMPANY",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        NIT = c.String(nullable: false),
                        NAME = c.String(nullable: false),
                        IMAGE = c.String(),
                        COLOR = c.String(),
                        PUBLICKEY = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ROLE",
                c => new
                    {
                        ID = c.Guid(nullable: false, identity: true),
                        DISPLAYNAME = c.String(),
                        TYPEROLE = c.Int(nullable: false),
                        NAME = c.String(nullable: false, maxLength: 200),
                        Company_Id = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.COMPANY", t => t.Company_Id)
                .Index(t => t.NAME, unique: true, name: "RoleNameIndex")
                .Index(t => t.Company_Id);
            
            CreateTable(
                "dbo.USERROLE",
                c => new
                    {
                        USERID = c.Guid(nullable: false),
                        ROLEID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.USERID, t.ROLEID })
                .ForeignKey("dbo.ROLE", t => t.ROLEID, cascadeDelete: true)
                .ForeignKey("dbo.USER", t => t.USERID, cascadeDelete: true)
                .Index(t => t.USERID)
                .Index(t => t.ROLEID);
            
            CreateTable(
                "dbo.USER",
                c => new
                    {
                        ID = c.Guid(nullable: false, identity: true),
                        USERNAME = c.String(nullable: false, maxLength: 200),
                        NAME1 = c.String(nullable: false, maxLength: 15),
                        NAME2 = c.String(maxLength: 15),
                        LASTNAME1 = c.String(nullable: false, maxLength: 15),
                        LASTNAME2 = c.String(maxLength: 15),
                        IDTYPE = c.Int(nullable: false),
                        IDNUMBER = c.String(nullable: false, maxLength: 12),
                        ADDRESS1 = c.String(nullable: false, maxLength: 45),
                        BIRTHDATE = c.DateTime(nullable: false),
                        GENDER = c.Int(nullable: false),
                        ACTIVATIONDATE = c.DateTime(nullable: false),
                        LASTUPDATE = c.DateTime(nullable: false),
                        EMAIL = c.String(nullable: false, maxLength: 200),
                        EMAILCONFIRMED = c.Boolean(nullable: false),
                        PASSWORDHASH = c.String(),
                        SECURITYSTAMP = c.String(),
                        PHONENUMBER = c.String(),
                        PHONENUMBERCONFIRMED = c.Boolean(nullable: false),
                        TWOFACTORENABLED = c.Boolean(nullable: false),
                        LOCKOUTENDDATEUTC = c.DateTime(),
                        LOCKOUTENABLED = c.Boolean(nullable: false),
                        ACCESSFAILEDCOUNT = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => t.USERNAME, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.USERCLAIM",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        USERID = c.Guid(nullable: false),
                        CLAIMTYPE = c.String(),
                        CLAIMVALUE = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.USER", t => t.USERID, cascadeDelete: true)
                .Index(t => t.USERID);
            
            CreateTable(
                "dbo.USERLOGIN",
                c => new
                    {
                        LOGINPROVIDER = c.String(nullable: false, maxLength: 128),
                        PROVIDERKEY = c.String(nullable: false, maxLength: 128),
                        USERID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.LOGINPROVIDER, t.PROVIDERKEY, t.USERID })
                .ForeignKey("dbo.USER", t => t.USERID, cascadeDelete: true)
                .Index(t => t.USERID);
            
            CreateTable(
                "dbo.VALIDADORPLUS",
                c => new
                    {
                        ID = c.Guid(nullable: false, identity: true),
                        USERVALIDATIONID = c.Guid(),
                        IDENTIFICADORLINEA = c.String(),
                        TIPOIDENTIFICACION = c.String(),
                        CODIGOTIPOINDENTIFICACION = c.String(),
                        NUMEROIDENTIFICACION = c.String(),
                        NOMBRETITULAR = c.String(),
                        NOMBRECIIU = c.String(),
                        LUGAREXPEDICION = c.String(),
                        FECHAEXPEDICION = c.String(),
                        ESTADO = c.String(),
                        NUMEROINFORME = c.String(),
                        ESTADOTITULAR = c.String(),
                        RANGOEDAD = c.String(),
                        CODIGOCIIU = c.String(),
                        CODIGODEPARTAMENTO = c.String(),
                        CODIGOMUNICIPIO = c.String(),
                        FECHA = c.String(),
                        HORA = c.String(),
                        ENTIDAD = c.String(),
                        RESPUESTACONSULTA = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.USERVALIDATION", t => t.USERVALIDATIONID)
                .Index(t => t.USERVALIDATIONID);
            
            CreateTable(
                "dbo.PROSPECTA",
                c => new
                    {
                        ID = c.Guid(nullable: false, identity: true),
                        USERVALIDATIONID = c.Guid(),
                        CONSULTADARECIENTE = c.Boolean(),
                        GENEROINCONSISTENCIAS = c.Boolean(),
                        NOMBRETITULAR = c.String(),
                        NUMEROIDENTIFICACION = c.String(),
                        RESULTADO = c.String(),
                        TIPOIDENTIFICACION = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.USERVALIDATION", t => t.USERVALIDATIONID)
                .Index(t => t.USERVALIDATIONID);
            
            CreateTable(
                "dbo.QUESTIONFORM",
                c => new
                    {
                        ID = c.Guid(nullable: false, identity: true),
                        USERVALIDATIONID = c.Guid(nullable: false),
                        STATE = c.Int(nullable: false),
                        DATECREATE = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.USERVALIDATION", t => t.USERVALIDATIONID, cascadeDelete: true)
                .Index(t => t.USERVALIDATIONID);
            
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
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.QUESTION", t => t.QUESTIONID, cascadeDelete: true)
                .ForeignKey("dbo.QUESTIONFORM", t => t.QUESTIONFORMID, cascadeDelete: true)
                .Index(t => t.QUESTIONFORMID)
                .Index(t => t.QUESTIONID);
            
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
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AFFILIATIONTYPE", t => t.AFFILIATIONTYPEID)
                .ForeignKey("dbo.PAGE", t => t.PAGEID, cascadeDelete: true)
                .Index(t => t.PAGEID)
                .Index(t => t.AFFILIATIONTYPEID);
            
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
                "dbo.CONFIG",
                c => new
                    {
                        ID = c.Guid(nullable: false, identity: true),
                        NUMBERQUESTION = c.Int(nullable: false),
                        NUMBERQUESTIONVALID = c.Int(nullable: false),
                        NUMBERINTENTBYDOCUMENT = c.Int(nullable: false),
                        NUMBERINTENTBYDOCUMENTTOTAL = c.Int(nullable: false),
                        DAYLOKEDDOCUMENT = c.Int(nullable: false),
                        TIMEOUTVALIDATION = c.Int(nullable: false),
                        TIMEOUT = c.Int(nullable: false),
                        TIMEOUTUPDATE = c.Int(nullable: false),
                        COMPANYID = c.Int(),
                        PAGETEMP = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.COMPANY", t => t.COMPANYID)
                .Index(t => t.COMPANYID);
            
            CreateTable(
                "dbo.RISKLEVEL",
                c => new
                    {
                        ID = c.Guid(nullable: false, identity: true),
                        LEVEL = c.Int(nullable: false),
                        DESCRIPTION = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.QUOTA",
                c => new
                    {
                        ID = c.Guid(nullable: false, identity: true),
                        NAME = c.String(),
                        AMOUNT = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.BLACKLIST",
                c => new
                    {
                        ID = c.Guid(nullable: false, identity: true),
                        DOCUMENT = c.String(),
                        NAME = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.CITY",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        DEPARTMENTID = c.Int(nullable: false),
                        NAME = c.String(nullable: false, maxLength: 50),
                        IDALT = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.DEPARTMENT", t => t.DEPARTMENTID, cascadeDelete: true)
                .Index(t => t.DEPARTMENTID);
            
            CreateTable(
                "dbo.DEPARTMENT",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        COUNTRYID = c.Int(nullable: false),
                        NAME = c.String(nullable: false, maxLength: 100),
                        IDALT = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.COUNTRY", t => t.COUNTRYID, cascadeDelete: true)
                .Index(t => t.COUNTRYID);
            
            CreateTable(
                "dbo.COUNTRY",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        NAME = c.String(nullable: false, maxLength: 50),
                        IDALT = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.DIVISION",
                c => new
                    {
                        ID = c.Guid(nullable: false, identity: true),
                        CODE = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
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
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.QUEUE", t => t.QUEUEID, cascadeDelete: true)
                .Index(t => t.QUEUEID);
            
            CreateTable(
                "dbo.SECRETARYTRANSIT",
                c => new
                    {
                        ID = c.Guid(nullable: false, identity: true),
                        NAME = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.UNIT",
                c => new
                    {
                        ID = c.Guid(nullable: false, identity: true),
                        ZONEID = c.Guid(nullable: false),
                        CODE = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ZONE", t => t.ZONEID, cascadeDelete: true)
                .Index(t => t.ZONEID);
            
            CreateTable(
                "dbo.ZONE",
                c => new
                    {
                        ID = c.Guid(nullable: false, identity: true),
                        DIVISIONID = c.Guid(nullable: false),
                        CODE = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.DIVISION", t => t.DIVISIONID, cascadeDelete: true)
                .Index(t => t.DIVISIONID);
            
            CreateTable(
                "dbo.USERAFFILIATION",
                c => new
                    {
                        ID = c.Guid(nullable: false, identity: true),
                        NAME = c.String(),
                        AFFILIATIONTYPEID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AFFILIATIONTYPE", t => t.AFFILIATIONTYPEID, cascadeDelete: true)
                .Index(t => t.AFFILIATIONTYPEID);
            
            CreateTable(
                "dbo.VOTESITE",
                c => new
                    {
                        ID = c.Guid(nullable: false, identity: true),
                        NAME = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ConfigPage",
                c => new
                    {
                        Config_Id = c.Guid(nullable: false),
                        Page_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Config_Id, t.Page_Id })
                .ForeignKey("dbo.CONFIG", t => t.Config_Id, cascadeDelete: true)
                .ForeignKey("dbo.PAGE", t => t.Page_Id, cascadeDelete: true)
                .Index(t => t.Config_Id)
                .Index(t => t.Page_Id);
            
            CreateTable(
                "dbo.QuotaRiskLevel",
                c => new
                    {
                        Quota_Id = c.Guid(nullable: false),
                        RiskLevel_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Quota_Id, t.RiskLevel_Id })
                .ForeignKey("dbo.QUOTA", t => t.Quota_Id, cascadeDelete: true)
                .ForeignKey("dbo.RISKLEVEL", t => t.RiskLevel_Id, cascadeDelete: true)
                .Index(t => t.Quota_Id)
                .Index(t => t.RiskLevel_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.USERAFFILIATION", "AFFILIATIONTYPEID", "dbo.AFFILIATIONTYPE");
            DropForeignKey("dbo.UNIT", "ZONEID", "dbo.ZONE");
            DropForeignKey("dbo.ZONE", "DIVISIONID", "dbo.DIVISION");
            DropForeignKey("dbo.RUNT", "QUEUEID", "dbo.QUEUE");
            DropForeignKey("dbo.CITY", "DEPARTMENTID", "dbo.DEPARTMENT");
            DropForeignKey("dbo.DEPARTMENT", "COUNTRYID", "dbo.COUNTRY");
            DropForeignKey("dbo.ATTORNEY", "QUEUEID", "dbo.QUEUE");
            DropForeignKey("dbo.QUEUE", "USERVALIDATIONID", "dbo.USERVALIDATION");
            DropForeignKey("dbo.USERVALIDATION", "USERID", "dbo.USER");
            DropForeignKey("dbo.USERVALIDATION", "RISKLEVELID", "dbo.RISKLEVEL");
            DropForeignKey("dbo.QuotaRiskLevel", "RiskLevel_Id", "dbo.RISKLEVEL");
            DropForeignKey("dbo.QuotaRiskLevel", "Quota_Id", "dbo.QUOTA");
            DropForeignKey("dbo.QUESTIONFORM", "USERVALIDATIONID", "dbo.USERVALIDATION");
            DropForeignKey("dbo.QUESTIONOPTION", "QUESTIONFORMID", "dbo.QUESTIONFORM");
            DropForeignKey("dbo.QUESTIONOPTION", "QUESTIONID", "dbo.QUESTION");
            DropForeignKey("dbo.QUESTION", "PAGEID", "dbo.PAGE");
            DropForeignKey("dbo.ConfigPage", "Page_Id", "dbo.PAGE");
            DropForeignKey("dbo.ConfigPage", "Config_Id", "dbo.CONFIG");
            DropForeignKey("dbo.CONFIG", "COMPANYID", "dbo.COMPANY");
            DropForeignKey("dbo.QUESTION", "AFFILIATIONTYPEID", "dbo.AFFILIATIONTYPE");
            DropForeignKey("dbo.PROSPECTA", "USERVALIDATIONID", "dbo.USERVALIDATION");
            DropForeignKey("dbo.VALIDADORPLUS", "USERVALIDATIONID", "dbo.USERVALIDATION");
            DropForeignKey("dbo.USERVALIDATION", "Company_Id", "dbo.COMPANY");
            DropForeignKey("dbo.ROLE", "Company_Id", "dbo.COMPANY");
            DropForeignKey("dbo.USERROLE", "USERID", "dbo.USER");
            DropForeignKey("dbo.USERLOGIN", "USERID", "dbo.USER");
            DropForeignKey("dbo.USERCLAIM", "USERID", "dbo.USER");
            DropForeignKey("dbo.USERROLE", "ROLEID", "dbo.ROLE");
            DropForeignKey("dbo.SISBEN", "QUEUEID", "dbo.QUEUE");
            DropForeignKey("dbo.SIMIT", "QUEUEID", "dbo.QUEUE");
            DropForeignKey("dbo.SENA", "QUEUEID", "dbo.QUEUE");
            DropForeignKey("dbo.RUAFSEVERANCE", "RUAFID", "dbo.RUAF");
            DropForeignKey("dbo.RUAFRISKS", "RUAFID", "dbo.RUAF");
            DropForeignKey("dbo.RUAFPENSION", "RUAFID", "dbo.RUAF");
            DropForeignKey("dbo.RUAFHEALTH", "RUAFID", "dbo.RUAF");
            DropForeignKey("dbo.RUAFCOMPENSATION", "RUAFID", "dbo.RUAF");
            DropForeignKey("dbo.RUAFASSISTANCE", "RUAFID", "dbo.RUAF");
            DropForeignKey("dbo.RUAF", "QUEUEID", "dbo.QUEUE");
            DropForeignKey("dbo.REGISTRAR", "QUEUEID", "dbo.QUEUE");
            DropForeignKey("dbo.POLICEMAN", "QUEUEID", "dbo.QUEUE");
            DropForeignKey("dbo.FOSYGA", "QUEUEID", "dbo.QUEUE");
            DropForeignKey("dbo.CONTROLLERSHIP", "QUEUEID", "dbo.QUEUE");
            DropIndex("dbo.QuotaRiskLevel", new[] { "RiskLevel_Id" });
            DropIndex("dbo.QuotaRiskLevel", new[] { "Quota_Id" });
            DropIndex("dbo.ConfigPage", new[] { "Page_Id" });
            DropIndex("dbo.ConfigPage", new[] { "Config_Id" });
            DropIndex("dbo.USERAFFILIATION", new[] { "AFFILIATIONTYPEID" });
            DropIndex("dbo.ZONE", new[] { "DIVISIONID" });
            DropIndex("dbo.UNIT", new[] { "ZONEID" });
            DropIndex("dbo.RUNT", new[] { "QUEUEID" });
            DropIndex("dbo.DEPARTMENT", new[] { "COUNTRYID" });
            DropIndex("dbo.CITY", new[] { "DEPARTMENTID" });
            DropIndex("dbo.CONFIG", new[] { "COMPANYID" });
            DropIndex("dbo.QUESTION", new[] { "AFFILIATIONTYPEID" });
            DropIndex("dbo.QUESTION", new[] { "PAGEID" });
            DropIndex("dbo.QUESTIONOPTION", new[] { "QUESTIONID" });
            DropIndex("dbo.QUESTIONOPTION", new[] { "QUESTIONFORMID" });
            DropIndex("dbo.QUESTIONFORM", new[] { "USERVALIDATIONID" });
            DropIndex("dbo.PROSPECTA", new[] { "USERVALIDATIONID" });
            DropIndex("dbo.VALIDADORPLUS", new[] { "USERVALIDATIONID" });
            DropIndex("dbo.USERLOGIN", new[] { "USERID" });
            DropIndex("dbo.USERCLAIM", new[] { "USERID" });
            DropIndex("dbo.USER", "UserNameIndex");
            DropIndex("dbo.USERROLE", new[] { "ROLEID" });
            DropIndex("dbo.USERROLE", new[] { "USERID" });
            DropIndex("dbo.ROLE", new[] { "Company_Id" });
            DropIndex("dbo.ROLE", "RoleNameIndex");
            DropIndex("dbo.USERVALIDATION", new[] { "Company_Id" });
            DropIndex("dbo.USERVALIDATION", new[] { "RISKLEVELID" });
            DropIndex("dbo.USERVALIDATION", new[] { "USERID" });
            DropIndex("dbo.SISBEN", new[] { "QUEUEID" });
            DropIndex("dbo.SIMIT", new[] { "QUEUEID" });
            DropIndex("dbo.SENA", new[] { "QUEUEID" });
            DropIndex("dbo.RUAFSEVERANCE", new[] { "RUAFID" });
            DropIndex("dbo.RUAFRISKS", new[] { "RUAFID" });
            DropIndex("dbo.RUAFPENSION", new[] { "RUAFID" });
            DropIndex("dbo.RUAFHEALTH", new[] { "RUAFID" });
            DropIndex("dbo.RUAFCOMPENSATION", new[] { "RUAFID" });
            DropIndex("dbo.RUAFASSISTANCE", new[] { "RUAFID" });
            DropIndex("dbo.RUAF", new[] { "QUEUEID" });
            DropIndex("dbo.REGISTRAR", new[] { "QUEUEID" });
            DropIndex("dbo.POLICEMAN", new[] { "QUEUEID" });
            DropIndex("dbo.FOSYGA", new[] { "QUEUEID" });
            DropIndex("dbo.CONTROLLERSHIP", new[] { "QUEUEID" });
            DropIndex("dbo.QUEUE", new[] { "USERVALIDATIONID" });
            DropIndex("dbo.ATTORNEY", new[] { "QUEUEID" });
            DropTable("dbo.QuotaRiskLevel");
            DropTable("dbo.ConfigPage");
            DropTable("dbo.VOTESITE");
            DropTable("dbo.USERAFFILIATION");
            DropTable("dbo.ZONE");
            DropTable("dbo.UNIT");
            DropTable("dbo.SECRETARYTRANSIT");
            DropTable("dbo.RUNT");
            DropTable("dbo.DIVISION");
            DropTable("dbo.COUNTRY");
            DropTable("dbo.DEPARTMENT");
            DropTable("dbo.CITY");
            DropTable("dbo.BLACKLIST");
            DropTable("dbo.QUOTA");
            DropTable("dbo.RISKLEVEL");
            DropTable("dbo.CONFIG");
            DropTable("dbo.PAGE");
            DropTable("dbo.QUESTION");
            DropTable("dbo.QUESTIONOPTION");
            DropTable("dbo.QUESTIONFORM");
            DropTable("dbo.PROSPECTA");
            DropTable("dbo.VALIDADORPLUS");
            DropTable("dbo.USERLOGIN");
            DropTable("dbo.USERCLAIM");
            DropTable("dbo.USER");
            DropTable("dbo.USERROLE");
            DropTable("dbo.ROLE");
            DropTable("dbo.COMPANY");
            DropTable("dbo.USERVALIDATION");
            DropTable("dbo.SISBEN");
            DropTable("dbo.SIMIT");
            DropTable("dbo.SENA");
            DropTable("dbo.RUAFSEVERANCE");
            DropTable("dbo.RUAFRISKS");
            DropTable("dbo.RUAFPENSION");
            DropTable("dbo.RUAFHEALTH");
            DropTable("dbo.RUAFCOMPENSATION");
            DropTable("dbo.RUAFASSISTANCE");
            DropTable("dbo.RUAF");
            DropTable("dbo.REGISTRAR");
            DropTable("dbo.POLICEMAN");
            DropTable("dbo.FOSYGA");
            DropTable("dbo.CONTROLLERSHIP");
            DropTable("dbo.QUEUE");
            DropTable("dbo.ATTORNEY");
            DropTable("dbo.AFFILIATIONTYPE");
        }
    }
}
