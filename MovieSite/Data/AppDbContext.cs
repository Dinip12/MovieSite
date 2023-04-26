using MovieSite.Entity;
using System.Data.Entity;

namespace MovieSite.Data
{
	public class AppDbContext : DbContext
	{
		public DbSet<User> Users { get; set; }
		public DbSet<Movie> Movies { get; set; }
		public DbSet<Favorite> Favorites { get; set; }
		public DbSet<Comment> Comments { get; set; }
		public DbSet<Rating> Ratings { get; set; }
		public AppDbContext() : base("Data Source=SQL8005.site4now.net;Initial Catalog=db_a982c0_dinip12;User Id=db_a982c0_dinip12_admin;Password=159753134679g")
		{
			Users = this.Set<User>();
			Movies = this.Set<Movie>();
			Favorites = this.Set<Favorite>();
			Comments = this.Set<Comment>();
			Ratings = this.Set<Rating>();
		}
	}
}
