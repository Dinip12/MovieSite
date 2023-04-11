using MovieSite.Entity;
using System.Data.Entity;
using System.Collections.Generic;

namespace MovieSite.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public AppDbContext() : base("Server=localhost\\sqlexpress;Database=MovieSite;Trusted_Connection=True;")
        {
            Users = this.Set<User>();
            Movies = this.Set<Movie>();
            Favorites = this.Set<Favorite>();
            Comments = this.Set<Comment>();
            Ratings = this.Set<Rating>();
        }
    }
}
