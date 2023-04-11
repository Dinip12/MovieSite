using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieSite.Entity
{
	[Table("Favorites")]
	public class Favorite
	{
		[Key]
		public int Id { get; set; }
		public int UserId { get; set; }
		public int MovieId { get; set; }
	}
}
