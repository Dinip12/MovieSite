namespace MovieSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _123 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Movies", "IsFavorite");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Movies", "IsFavorite", c => c.Boolean(nullable: false));
        }
    }
}
