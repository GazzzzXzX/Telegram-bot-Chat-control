using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BotCore
{
	internal class CollectionPictureNotification
	{
		[Key]
		public System.Int32 Id { get; set; }
		
		public ButtonAndTextNotication buttonAndTextNotification { get; set; }
		
		public System.String Picture { get; set; }
	}
}