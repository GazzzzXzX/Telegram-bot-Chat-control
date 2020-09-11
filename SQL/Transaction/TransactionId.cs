using System;
using System.ComponentModel.DataAnnotations;

namespace BotCore.SQL
{
	internal class TransactionId
	{
		[Key]
		public Int32 Id { get; set; }

		public String NameHashOne { get; set; }
	}
}
