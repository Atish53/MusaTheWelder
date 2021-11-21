namespace MusaTheWelder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Musas : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SaleQuotes",
                c => new
                    {
                        SaleQuoteId = c.Int(nullable: false, identity: true),
                        SaleId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        ProductPrice = c.Double(nullable: false),
                        QuoteInstructions = c.String(),
                        Status = c.String(),
                        QuotePrice = c.Double(nullable: false),
                        isAccepted = c.Boolean(nullable: false),
                        isDeclined = c.Boolean(nullable: false),
                        isPaid = c.Boolean(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.SaleQuoteId)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("dbo.Sales", t => t.SaleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.SaleId)
                .Index(t => t.ProductId)
                .Index(t => t.ApplicationUser_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SaleQuotes", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.SaleQuotes", "SaleId", "dbo.Sales");
            DropForeignKey("dbo.SaleQuotes", "ProductId", "dbo.Products");
            DropIndex("dbo.SaleQuotes", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.SaleQuotes", new[] { "ProductId" });
            DropIndex("dbo.SaleQuotes", new[] { "SaleId" });
            DropTable("dbo.SaleQuotes");
        }
    }
}
