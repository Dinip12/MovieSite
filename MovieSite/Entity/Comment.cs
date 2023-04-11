using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieSite.Entity
{
	[Table("Comments")]
	public class Comment
	{
		[Key]
		public int Id { get; set; }
		public int UserId { get; set; }
		public int MovieId { get; set; }
		public string Text { get; set; }
		public string Username { get; set; }
	}
}
