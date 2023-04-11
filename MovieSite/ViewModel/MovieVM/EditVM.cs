using MovieSite.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace MovieSite.ViewModel.MovieVM
{
	public class EditVM
	{
		public int ID { get; set; }
		[Display(Name = "Title: ")]
		[Required(ErrorMessage = "This field is Required!")]
		public string Title { get; set; }
		[Display(Name = "Desciption: ")]
		[Required(ErrorMessage = "This field is Required!")]
		public string Description { get; set; }
		public MovieCategory MovieCategory { get; set; }
		public string TrailerURL { get; set; }
		public string Actors { get; set; }
		public string ReleaseYear { get; set; }
		public string Studio { get; set; }
	}
}
