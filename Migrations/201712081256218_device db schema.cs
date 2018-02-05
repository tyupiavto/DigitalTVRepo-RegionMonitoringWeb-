namespace AdminPanelDevice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class devicedbschema : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Devices",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        IP = c.Double(nullable: false),
                        Ping = c.Int(nullable: false),
                        Frequency = c.String(),
                        DeviceTID = c.Int(nullable: false),
                        TowerID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.DeviceType",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Model = c.String(),
                        Manufacture = c.String(),
                        Type = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Line",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FromTID = c.Int(nullable: false),
                        ToTID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Tower",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        LattiTube = c.Double(nullable: false),
                        LongiTube = c.Double(nullable: false),
                        IP = c.Double(nullable: false),
                        Phone = c.Int(nullable: false),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Tower");
            DropTable("dbo.Line");
            DropTable("dbo.DeviceType");
            DropTable("dbo.Devices");
        }
    }
}
