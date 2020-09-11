using System;
using System.ComponentModel.DataAnnotations;

namespace BotCore.SQL
{
	internal class ChannelAdmin
	{
		[Key]
		public Int32 Id { get; set; }

		/// <summary>
		/// ID канала в котором сидят админы.
		/// </summary>
		public Int64 ChannelId { get; set; }

		/// <summary>
		/// ID Администратора.
		/// </summary>
		public Int32 UserId { get; set; }
	}
}
