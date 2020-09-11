using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BotCore
{
	internal class CollectionButtonNotification
	{
		[Key]
		public System.Int32 Id { get; set; }
		
		public ButtonAndTextNotication buttonAndTextNotification { get; set; }
		
		public ButtonNotification buttonNotification { get; set; }
	}
}