using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieSite.Entity
{
	[Table("Users")]
	public class User
	{
		[Key]
		public int Id { get; set; }
		public string username { get; set; }
		public string password { get; set; }
		public string email { get; set; }
		public bool IsAdmin { get; set; }
	}
}
