using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BotCore.SQL
{
	internal class PostChannel
	{
		[Key, Column(Order = 0), ForeignKey("PostTemplate")]
		public Int32 PostTemplateId { get; set; }

		public PostTemplate Template { get; set; }

		[Key, Column(Order = 1), ForeignKey("Channel")]
		public Int64 ChannelId { get; set; }

		public Channel Channel { get; set; }
	}
}