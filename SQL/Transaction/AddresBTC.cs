using System;
using System.ComponentModel.DataAnnotations;

namespace BotCore.SQL
{
	public class AddresBTC
	{
		[Key]
		public Int32 Id { get; set; }

		public String PrivateKey { get; set; }

		public String PublickKey { get; set; }
	}
}
