using System;
using System.ComponentModel.DataAnnotations;

namespace BotCore
{
	internal class Category
	{
		[Key]
		public Int32 Id { get; set; }

		public String Name { get; set; }
	}
}
