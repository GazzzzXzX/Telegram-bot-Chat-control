using System.ComponentModel.DataAnnotations;

namespace BotCore
{
	internal class PhotoDate
	{
		[Key]
		public System.Int32 ID { get; set; }

		public System.Int32 IDUser { get; set; }

		public System.Int64 IDChannel { get; set; }

		public System.DateTime timeMessage { get; set; }

		public System.Int32 MessageID { get; set; }
	}
}