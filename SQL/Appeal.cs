using System.ComponentModel.DataAnnotations;

namespace BotCore
{
	internal class Appeal
	{
		[Key]
		public System.Int32 ID { get; set; }

		public System.Int32 IDSender { get; set; }

		public System.String Name { get; set; }

		public System.String Description { get; set; }
	}
}