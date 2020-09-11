using System.ComponentModel.DataAnnotations;

namespace BotCore
{
	internal class FeaturedUserNew
	{
		[Key]
		public System.Int32 ID { get; set; }

		public System.Int32 UserId { get; set; }

		public System.Int32 UserWhoAddedId { get; set; }
	}
}