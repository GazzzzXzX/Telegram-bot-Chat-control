using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BotCore
{
	internal class AnaliticsTextAllChat
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public String NameId { get; set; }

		public Int32 Count { get; set; }
	}
}
