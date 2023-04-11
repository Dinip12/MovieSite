using MovieSite.Entity;
using System.ComponentModel;
using System.Linq.Expressions;

namespace MovieSite.ViewModel.UserVM
{
	public class FilterVM
	{
		[DisplayName("Username: ")]
		public string Username { get; set; }
		[DisplayName("Email: ")]
		public string Email { get; set; }
		public Expression<Func<User, bool>> GetFilter()
		{
			return i => (string.IsNullOrEmpty(Username) || i.username.Contains(Username)) &&
						(string.IsNullOrEmpty(Email) || i.email.Contains(Email));
		}
	}
}
