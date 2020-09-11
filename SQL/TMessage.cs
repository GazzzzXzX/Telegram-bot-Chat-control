using System.ComponentModel.DataAnnotations;

namespace BotCore
{
	internal class TMessage
	{
		[Key]
		public System.Int32 ID { get; set; }

		[Required]
		public User user { get; set; }

		public System.Int32 Post { get; set; }

		public System.DateTime dateTime { get; set; }

		[Required]
		public Channel channel { get; set; }
	}
}