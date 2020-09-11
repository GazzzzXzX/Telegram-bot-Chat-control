using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BotCore
{
	internal class NotificationChat
	{
		[Key]
		public System.Int32 Id { get; set; }
		
		public System.Int32 IdNotification { get; set; }
		
		public Channel IdChannel { get; set; }
	}
}