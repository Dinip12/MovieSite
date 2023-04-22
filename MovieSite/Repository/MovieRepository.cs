using MovieSite.Data;
using MovieSite.Entity;
using System.Data.Entity;
using System.Linq.Expressions;

namespace MovieSite.Repository
{
	public class MovieRepository
	{
		private readonly AppDbContext context;
		public MovieRepository()
		{
			context = new AppDbContext();
		}
		public void InsertMovie(Movie item)
		{
			Movie movie = new Movie();

			movie.Title = item.Title;
			movie.Description = item.Description;
			movie.movieCategory = item.movieCategory;
			movie.TrailerURL = item.TrailerURL;
			movie.fileName = item.fileName;
			movie.Actors = item.Actors;
			movie.ReleaseYear = item.ReleaseYear;
			movie.Studio = item.Studio;

			context.Movies.Add(movie);
			context.SaveChanges();
		}
		public void DeleteMovieByID(int id)
		{

			Movie movie = context.Movies.Find(id);

			context.Movies.Remove(movie);
			context.Ratings.RemoveRange(context.Ratings.Where(x => x.MovieId == id));
			context.Favorites.RemoveRange(context.Favorites.Where(x => x.MovieId == id));
			context.Comments.RemoveRange(context.Comments.Where(x => x.MovieId == id));
			context.SaveChanges();
		}

		public void UpdateMovie(Movie item)
		{

			Movie movie = context.Movies.Find(item.Id);

			movie.Id = item.Id;
			movie.Title = item.Title;
			movie.Description = item.Description;
			movie.movieCategory = item.movieCategory;
			movie.TrailerURL = item.TrailerURL;
			movie.Votes = item.Votes;
			movie.Actors = item.Actors;
			movie.ReleaseYear = item.ReleaseYear;
			movie.Studio = item.Studio;

			context.Entry(movie).State = EntityState.Modified;
			context.SaveChanges();
		}
		public int MovieCount(Expression<Func<Movie, bool>> filter = null)
		{
			IQueryable<Movie> query = context.Movies;

			if (filter != null)
				query = query.Where(filter);

			return query.Count();
		}

		public List<Movie> GetAll(Expression<Func<Movie, bool>> filter = null, int page = 1, int pageSize = int.MaxValue)
		{
			IQueryable<Movie> query = context.Movies;
			if (filter != null)
				query = query.Where(filter);

			return query.OrderBy(x => x.Id).Skip((page - 1) * pageSize).Take(pageSize).ToList();
		}
		public Movie GetById(int id)
		{
			return context.Movies.Find(id);
		}
	}
}
