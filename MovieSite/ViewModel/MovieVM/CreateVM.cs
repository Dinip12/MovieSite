using MovieSite.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace MovieSite.ViewModel.MovieVM
{
    public class CreateVM
    {

        [Required(ErrorMessage = "This field is Required!")]
        public string Title { get; set; }
        [Required(ErrorMessage = "This field is Required!")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Movie category is required")]
        public MovieCategory MovieCategory { get; set; }

        [Required(ErrorMessage = "This field is Required!")]
        public IFormFile file { get; set; }
        [Required(ErrorMessage = "This field is Required!")]
        public string TrailerURL { get; set; }
        [Required(ErrorMessage = "This field is Required!")]
        public string Actors { get; set; }
        [Required(ErrorMessage = "This field is Required!")]
        public string ReleaseYear { get; set; }
        [Required(ErrorMessage = "This field is Required!")]
        public string Studio { get; set; }
    }
}
