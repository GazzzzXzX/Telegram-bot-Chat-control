using System;
using System.ComponentModel.DataAnnotations;

namespace BotCore.SQL
{
	internal class Income
	{
		[Key]
		public Int32 Id { get; set; }

		/// <summary>
		/// ID пользователя.
		/// </summary>
		public Int32 UserId { get; set; }

		/// <summary>
		/// ID канала в котором сидит пользователь.
		/// </summary>
		public Int64 ChannelId { get; set; }

		/// <summary>
		/// Сумма доната.
		/// </summary>
		public Single SumIncome { get; set; }

		/// <summary>
		/// Дата.
		/// </summary>
		public DateTime dateTime { get; set; }
	}
}