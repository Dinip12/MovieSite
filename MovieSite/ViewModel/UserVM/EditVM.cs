using System.ComponentModel.DataAnnotations;

namespace MovieSite.ViewModel.UserVM
{
	public class EditVM
	{
		public int ID { get; set; }
		[Display(Name = "Username: ")]
		[Required(ErrorMessage = "This field is Required!")]
		public string Username { get; set; }
		[Display(Name = "Password: ")]
		[Required(ErrorMessage = "This field is Required!")]
		public string Password { get; set; }
		[Display(Name = "Email: ")]
		[Required(ErrorMessage = "This field is Required!")]
		public string Email { get; set; }
		public bool IsAdmin { get; set; }
	}
}
