namespace MusaTheWelder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class specials : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "onSpecial", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "onSpecial");
        }
    }
}
