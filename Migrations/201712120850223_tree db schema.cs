namespace AdminPanelDevice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class treedbschema : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TreeInformation",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        OID = c.String(),
                        Mib = c.String(),
                        Syntax = c.String(),
                        Access = c.String(),
                        Status = c.String(),
                        DefVal = c.String(),
                        Indexes = c.String(),
                        Description = c.String(),
                        DeviceID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.DeviceType", t => t.DeviceID, cascadeDelete: true)
                .Index(t => t.DeviceID);
            
            CreateTable(
                "dbo.TreeStructure",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        OIDName = c.String(),
                        Child = c.Int(nullable: false),
                        Parrent = c.Int(nullable: false),
                        DeviceID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.DeviceType", t => t.DeviceID, cascadeDelete: true)
                .Index(t => t.DeviceID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TreeStructure", "DeviceID", "dbo.DeviceType");
            DropForeignKey("dbo.TreeInformation", "DeviceID", "dbo.DeviceType");
            DropIndex("dbo.TreeStructure", new[] { "DeviceID" });
            DropIndex("dbo.TreeInformation", new[] { "DeviceID" });
            DropTable("dbo.TreeStructure");
            DropTable("dbo.TreeInformation");
        }
    }
}
