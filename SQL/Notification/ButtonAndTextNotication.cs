using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BotCore
{
	internal class ButtonAndTextNotication
	{
		[Key]
		public System.Int32 Id { get; set; }
		
		public User User { get; set; }
		
		public TextNotification Text { get; set; }
		public System.Boolean isWork { get; set; }
	}
}