using MovieSite.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace MovieSite.ViewModel.MovieVM
{
	public class CreateVM
	{
		[Display(Name = "Title: ")]
		[Required(ErrorMessage = "This field is Required!")]
		public string Title { get; set; }
		[Display(Name = "Desciption: ")]
		[Required(ErrorMessage = "This field is Required!")]
		public string Description { get; set; }

		[Display(Name = "Select a category")]
		[Required(ErrorMessage = "Movie category is required")]
		public MovieCategory MovieCategory { get; set; }
		public IFormFile file { get; set; }
		public string TrailerURL { get; set; }
		public string Actors { get; set; }
		public string ReleaseYear { get; set; }
		public string Studio { get; set; }
	}
}
