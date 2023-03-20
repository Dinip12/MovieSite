namespace MovieSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Spelling : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Favourites", newName: "Favorites");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.Favorites", newName: "Favourites");
        }
    }
}
