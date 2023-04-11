using MovieSite.Data.Enums;
using MovieSite.Entity;
using System.Linq.Expressions;
namespace MovieSite.ViewModel.MovieVM
{
	public class FilterVM
	{
		public string Title { get; set; }
		public string Description { get; set; }
		public MovieCategory Category { get; set; }
		public Expression<Func<Movie, bool>> GetFilter()
		{
			return i => (string.IsNullOrEmpty(Title) || i.Title.Contains(Title)) &&
						(string.IsNullOrEmpty(Description) || i.Description.Contains(Description)) &&
						(Category == default || i.movieCategory == Category);
		}
	}
}