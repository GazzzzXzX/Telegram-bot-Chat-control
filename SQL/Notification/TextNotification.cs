using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BotCore
{
	public class TextNotification
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public System.String Text { get; set; }
	}
}