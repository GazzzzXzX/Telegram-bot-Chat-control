using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BotCore
{
	internal class Word
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public System.String word { get; set; }

		public System.String Link { get; set; }
	}
}