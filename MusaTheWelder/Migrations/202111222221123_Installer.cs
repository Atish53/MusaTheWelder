namespace MusaTheWelder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Installer : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Installations",
                c => new
                    {
                        InstallationId = c.Int(nullable: false, identity: true),
                        SaleQuoteId = c.Int(nullable: false),
                        DateInstalled = c.String(),
                        isInstalled = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.InstallationId)
                .ForeignKey("dbo.SaleQuotes", t => t.SaleQuoteId, cascadeDelete: true)
                .Index(t => t.SaleQuoteId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Installations", "SaleQuoteId", "dbo.SaleQuotes");
            DropIndex("dbo.Installations", new[] { "SaleQuoteId" });
            DropTable("dbo.Installations");
        }
    }
}
