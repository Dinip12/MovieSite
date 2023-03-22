namespace MovieSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class final : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Movies", "Studio", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Movies", "Studio");
        }
    }
}
