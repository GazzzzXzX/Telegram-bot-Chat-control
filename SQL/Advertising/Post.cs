using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BotCore.SQL
{
	internal class Post
	{
		[Key]
		public Int32 MessageId { get; set; }

		[ForeignKey("PostTemplate")]
		public Int32 PostTemplateId { get; set; }

		public PostTemplate Template { get; set; }
		public Int64 ChannelId { get; set; }
	}
}