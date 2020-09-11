using System.ComponentModel.DataAnnotations;

namespace BotCore
{
	internal class Complaint
	{
		[Key]
		public System.Int32 ID { get; set; }

		public System.Int32 IDRecipient { get; set; }

		public System.String Name { get; set; }

		public System.String Description { get; set; }

		public System.Boolean TheEnd { get; set; }

		public System.String TextRecipient { get; set; }
	}
}