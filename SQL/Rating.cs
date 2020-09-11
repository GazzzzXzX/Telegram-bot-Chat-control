using System.ComponentModel.DataAnnotations;

namespace BotCore
{
	internal class Rating
	{
		[Key]
		public System.Int32 ID { get; set; }

		public System.Int32 IDUser { get; set; }
		public System.Int32 Star { get; set; }
	}
}