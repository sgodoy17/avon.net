namespace IdentiGo.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update4 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GROUPROLE",
                c => new
                    {
                        ID = c.Guid(nullable: false, identity: true),
                        NAME = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.GroupRoleRole",
                c => new
                    {
                        GroupRole_Id = c.Guid(nullable: false),
                        Role_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.GroupRole_Id, t.Role_Id })
                .ForeignKey("dbo.GROUPROLE", t => t.GroupRole_Id, cascadeDelete: true)
                .ForeignKey("dbo.ROLE", t => t.Role_Id, cascadeDelete: true)
                .Index(t => t.GroupRole_Id)
                .Index(t => t.Role_Id);
            
            CreateTable(
                "dbo.UserGroupRole",
                c => new
                    {
                        User_Id = c.Guid(nullable: false),
                        GroupRole_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_Id, t.GroupRole_Id })
                .ForeignKey("dbo.USER", t => t.User_Id, cascadeDelete: true)
                .ForeignKey("dbo.GROUPROLE", t => t.GroupRole_Id, cascadeDelete: true)
                .Index(t => t.User_Id)
                .Index(t => t.GroupRole_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserGroupRole", "GroupRole_Id", "dbo.GROUPROLE");
            DropForeignKey("dbo.UserGroupRole", "User_Id", "dbo.USER");
            DropForeignKey("dbo.GroupRoleRole", "Role_Id", "dbo.ROLE");
            DropForeignKey("dbo.GroupRoleRole", "GroupRole_Id", "dbo.GROUPROLE");
            DropIndex("dbo.UserGroupRole", new[] { "GroupRole_Id" });
            DropIndex("dbo.UserGroupRole", new[] { "User_Id" });
            DropIndex("dbo.GroupRoleRole", new[] { "Role_Id" });
            DropIndex("dbo.GroupRoleRole", new[] { "GroupRole_Id" });
            DropTable("dbo.UserGroupRole");
            DropTable("dbo.GroupRoleRole");
            DropTable("dbo.GROUPROLE");
        }
    }
}
