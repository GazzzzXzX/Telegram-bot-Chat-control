using System.Collections.Generic;
using System.Linq;

using BotCore;

namespace Calendar
{
	internal class Calendar
	{
		//public List<System.DateTime> ChosesDates = new List<System.DateTime>();
		public System.Int32 year = new System.Int32();

		public System.Int32 month = new System.Int32();
		public System.Int32[,] calendar = new System.Int32[6, 7];
		public System.DateTime date { get; set; }

		public Calendar()
		{
			System.DateTime dateTime = System.DateTime.Today;
			year = dateTime.Year;
			month = dateTime.Month;
		}

		public static void AddUserCalendar(System.Int32 id)
		{
			if (Bot.users_calendar.Any(p => p.Key != id) || Bot.users_calendar.Count == 0)
			{
				Bot.users_calendar = new Dictionary<System.Int32, Calendar>
				{
					{ id, new Calendar() }
				};
			}
		}

		public void Start()
		{
			System.DateTime dateTime = System.DateTime.Today;
			year = dateTime.Year;
			month = dateTime.Month;
		}

		private System.Boolean CheckMonth(System.Int32 month, System.Int32 year)
		{
			System.DateTime dates = System.DateTime.Today;
			if (month == 0)
			{
				if (CheckYear(year - 1) == false)
				{
					return false;
				}
				else
				{
					month = 1;
					return true;
				}
			}

			if (month >= dates.Month)
			{
				return true;
			}
			else
			{
				if (CheckYear(year - 1) == true)
				{
					return true;
				}
			}
			return false;
		}

		private System.Boolean CheckYear(System.Int32 year)
		{
			System.DateTime dates = System.DateTime.Today;

			if (year >= dates.Year)
			{
				return true;
			}

			return false;
		}

		public void Next()
		{
			month++;
			if (month == 13)
			{
				year++;
				month = 1;
				date.AddYears(year);
			}
			date.AddMonths(month);
		}

		public void Back()
		{
			if (CheckYear(year) == false)
			{
				return;
			}

			if (CheckMonth(month - 1, year) == false)
			{
				return;
			}

			month--;
			if (month == 0)
			{
				year--;
				month = 12;
				date.AddYears(year);
			}
			date.AddMonths(month);
		}

		public void FillCalendar()
		{
			calendar = new System.Int32[6, 7];
			System.Int32 days = System.DateTime.DaysInMonth(year, month);
			System.Int32 currentDay = 1;
			System.Int32 dayOfWeek = (System.Int32)date.DayOfWeek;
			for (System.Int32 i = 0; i < calendar.GetLength(0); i++)
			{
				for (System.Int32 j = 0; j < calendar.GetLength(1) && currentDay - dayOfWeek + 1 <= days; j++)
				{
					if (i == dayOfWeek && i == 0 && dayOfWeek == 0)
					{
						if (j < 6)
						{
							calendar[i, j] = 0;
						}
						else if (j == 6)
						{
							calendar[i, j] = 1;
						}
					}
					else
					{
						calendar[i, j] = currentDay - dayOfWeek + 1;
						currentDay++;
					}
				}
			}
		}

		public System.String NameCalendar(System.DateTime dateTime)
		{
			System.String temp = "";
			switch (dateTime.Month)
			{
				case 1:
				{
					temp = "Январь " + dateTime.Year;
					break;
				}
				case 2:
				{
					temp = "Февраль " + dateTime.Year;
					break;
				}
				case 3:
				{
					temp = "Март " + dateTime.Year;
					break;
				}
				case 4:
				{
					temp = "Апрель " + dateTime.Year;
					break;
				}
				case 5:
				{
					temp = "Мая " + dateTime.Year;
					break;
				}
				case 6:
				{
					temp = "Июнь " + dateTime.Year;
					break;
				}
				case 7:
				{
					temp = "Июль " + dateTime.Year;
					break;
				}
				case 8:
				{
					temp = "Август " + dateTime.Year;
					break;
				}
				case 9:
				{
					temp = "Сентябрь " + dateTime.Year;
					break;
				}
				case 10:
				{
					temp = "Октябрь " + dateTime.Year;
					break;
				}
				case 11:
				{
					temp = "Ноябрь " + dateTime.Year;
					break;
				}
				case 12:
				{
					temp = "Декабрь " + dateTime.Year;
					break;
				}
			}
			return temp;
		}
	}
}