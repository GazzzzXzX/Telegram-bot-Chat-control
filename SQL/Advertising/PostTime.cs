using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BotCore.SQL
{
	internal class PostTime
	{
		[Key]
		public System.Int32 ID { get; set; }

		[ForeignKey("PostTemplate")]
		public Int32 PostTemplateId { get; set; }

		public System.Boolean UseTime { get; set; } = false;
		public System.Boolean IsDate { get; set; } = false;
		public System.Int32? IdDateTime { get; set; }
		public System.Boolean isWorked { get; set; } = false;

		public PostTemplate Template { get; set; }

		public DateTime Time { get; set; }
	}
}