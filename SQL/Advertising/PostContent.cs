using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BotCore.SQL
{
	internal class PostContent
	{
		[Key]
		public System.Int32 ID { get; set; }

		[ForeignKey("PostTemplate")]
		public Int32 PostTemplateId { get; set; }

		public PostTemplate PostTemplate { get; set; }

		public Int32 Order { get; set; }

		public Int32 MessageId { get; set; }
		//public Message Message { get; set; }
	}
}