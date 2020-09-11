using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BotCore
{
	internal class ButtonNotification
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public System.String Text { get; set; }
	}
}