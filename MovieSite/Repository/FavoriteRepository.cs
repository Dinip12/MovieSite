using MovieSite.Data;
using MovieSite.Entity;

namespace MovieSite.Repository
{
	public class FavoriteRepository
	{
		private readonly AppDbContext context;
		private readonly MovieRepository movieRepository;
		public FavoriteRepository()
		{
			movieRepository = new MovieRepository();
			context = new AppDbContext();
		}
		public void isFavorite(Favorite favorite)
		{
			if (favorite.Id > 0)
			{
				DeleteFavoriteByID(favorite.Id);
			}
			else
			{
				InsertFavorite(favorite);
			}
		}
		public void InsertFavorite(Favorite item)
		{
			Favorite favorite = new Favorite();

			favorite.UserId = item.UserId;
			favorite.MovieId = item.MovieId;

			context.Favorites.Add(favorite);
			context.SaveChanges();
		}
		public void DeleteFavoriteByID(int id)
		{


			context.Favorites.Remove(context.Favorites.Find(id));

			context.SaveChanges();
		}
		public Favorite FindFavorite(int movieid, int userId)
		{
			foreach (var item in context.Favorites)
			{
				if (item.MovieId == movieid && item.UserId == userId)
				{
					return item;
				}
			}
			return null;
		}
		public List<Movie> getAllFavorites(int userId)
		{
			return (from a in context.Movies
					join b in context.Favorites on a.Id equals b.MovieId
					where b.UserId == userId
					select a).ToList();
		}

	}
}
