using MovieSite.Entity;
using MovieSite.ViewModel.Shared;

namespace MovieSite.ViewModel.UserVM
{
	public class IndexVM
	{
		public List<User> Items { get; set; }
		public PagerVM Pager { get; set; }
		public FilterVM Filter { get; set; }
	}
}
