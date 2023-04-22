using MovieSite.Data.Enums;
using MovieSite.Entity;

namespace MovieSite.ViewModel.MovieVM
{
	public class DetailsVM
	{
		public int id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public MovieCategory MovieCategory { get; set; }
		public IFormFile file { get; set; }
		public string TrailerURL { get; set; }
		public Favorite favorite { get; set; }
		public List<Comment> comments { get; set; }
		public int Votes { get; set; }
		public double Rating { get; set; }
		public string Actors { get; set; }
		public string ReleaseYear { get; set; }
		public string Studio { get; set; }
	}
}
