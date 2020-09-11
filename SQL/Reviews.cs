using System.ComponentModel.DataAnnotations;

namespace BotCore
{
	internal class Reviews
	{
		[Key]
		public System.Int32 ID { get; set; }

		public System.String Name { get; set; }

		public System.String Description { get; set; }

		public System.Int32 IDSender { get; set; } //-- -ID отправителя
		public System.Int32 IDRecipient { get; set; } //-- ID получателя

		public System.Boolean TheEnd { get; set; }
	}
}