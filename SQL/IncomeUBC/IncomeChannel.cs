using System;
using System.ComponentModel.DataAnnotations;


namespace BotCore.SQL
{
	internal class IncomeChannel
	{
		[Key]
		public Int32 Id { get; set; }

		/// <summary>
		/// ID канала где отображается вся его прыбыль.
		/// </summary>
		public Int64 ChannelId { get; set; }

		/// <summary>
		/// Прибыль канала за определенный период времени (Месяц).
		/// </summary>
		public Single SumIncome { get; set; }

		/// <summary>
		/// Дата (Месяц).
		/// </summary>
		public DateTime DateTime { get; set; }
	}
}
