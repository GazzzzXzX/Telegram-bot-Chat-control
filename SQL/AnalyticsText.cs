using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BotCore
{
	internal class AnalyticsText
	{
		[Key]
		public Int32 Id { get; set; }

		public String AnaliticsTextAllChatId { get; set; }

		[ForeignKey("Channel")]
		public System.Int64 ChannelId { get; set; }

		public Channel channel { get; set; }

		public Int32 Count { get; set; }
	}
}
