using MovieSite.Entity;
using System.ComponentModel;
using System.Linq.Expressions;
using MovieSite.Data.Enums;
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

