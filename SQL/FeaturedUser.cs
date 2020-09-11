using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BotCore
{
	internal class FeaturedUser
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public System.Int32 UserId { get; set; }

		public System.Int32 UserWhoAddedId { get; set; }
	}
}