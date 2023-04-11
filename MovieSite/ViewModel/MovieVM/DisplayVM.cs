using MovieSite.Data.Enums;
using MovieSite.Entity;
using MovieSite.ViewModel.Shared;

namespace MovieSite.ViewModel.MovieVM
{
	public class DisplayVM
	{
		public List<Movie> Movies { get; set; }
		public MovieCategory CurrentCategoryFilter { get; set; }
		public PagerVM Pager { get; set; }
		public FilterVM Filter { get; set; }
	}
}
