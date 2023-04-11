using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieSite.Entity
{
	[Table("Ratings")]
	public class Rating
	{
		[Key]
		public int Id { get; set; }
		public int UserId { get; set; }
		public int MovieId { get; set; }
		public int Rated { get; set; }
	}
}
