namespace MusaTheWelder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class no : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Products", "onSpecial");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "onSpecial", c => c.Boolean(nullable: false));
        }
    }
}
