namespace MovieSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FinalDetails : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Movies", "Actors", c => c.String());
            AddColumn("dbo.Movies", "ReleaseYear", c => c.String());
            DropColumn("dbo.Ratings", "Votes");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Ratings", "Votes", c => c.Int(nullable: false));
            DropColumn("dbo.Movies", "ReleaseYear");
            DropColumn("dbo.Movies", "Actors");
        }
    }
}
