using System.ComponentModel.DataAnnotations;

namespace BotCore
{
	internal class UserMessage
	{
		[Key]
		public System.Int32 ID { get; set; }

		public System.Int32 UserID { get; set; }

		public System.String SendMessage { get; set; }

		public System.DateTime dateTime { get; set; }
	}
}