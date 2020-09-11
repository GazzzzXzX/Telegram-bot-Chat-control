using System;
using System.ComponentModel.DataAnnotations;

namespace BotCore.SQL
{
	internal class IncomeChannelAdmin
	{
		[Key]
		public Int32 Id { get; set; }

		/// <summary>
		/// ID канала в котором сидит администратор.
		/// </summary>
		public Int64 ChannelId { get; set; }

		/// <summary>
		/// ID администратора.
		/// </summary>
		public Int32 UserId { get; set; }

		/// <summary>
		/// Сумма прибыли администратора.
		/// </summary>
		public Single SumIncome { get; set; }

		/// <summary>
		/// Дата прибыли.
		/// </summary>
		public DateTime DateTime { get; set; }
	}
}
