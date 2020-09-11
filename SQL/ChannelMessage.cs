using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BotCore
{
	public class ChannelMessage
	{
		[Key] 
		public Int32 Id { get; set; }
		
		public Int64 ChannelID {get; set; }
		
		public Int32 MessageId { get; set; }
		
		public DateTime DateMessageDelete { get; set; }
	}
}