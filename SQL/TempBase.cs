using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BotCore
{
	internal class TempBase
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public System.Int32 ID { get; set; }

		public System.Int64 IDTwo { get; set; }
	}
}