using System;
using System.Collections.Generic;
using System.Linq;

using BotCore.SQL;

namespace BotCore.WorkingTime
{
	internal class WorkingTimeAndDate
	{
		public static void Start(System.Int32 id, PostTemplate post)
		{
			Calendar.Calendar.AddUserCalendar(id);
			WorkingTimeBot.AddUserTime(id);
			if (Bot.users_calendar.Count == 0 || WorkingTimeBot.users_time.Count == 0)
			{
				Console.WriteLine("Error => WorkingTimeAndDate");
				return;
			}
		}

		public static void Clear(System.Int32 id)
		{
		}
	}

	internal class WorkingTimeBot
	{
		public static Dictionary<System.Int32, TimeModel> users_time = new Dictionary<Int32, TimeModel>();

		public static void AddUserTime(System.Int32 id)
		{
			if (users_time.Any(p => p.Key != id) || users_time.Count == 0)
			{
				users_time = new Dictionary<System.Int32, TimeModel>
				{
					{ id, new TimeModel() }
				};
			}
		}
	}

	internal class TimeModel
	{
		public List<PostTime> timeModels = new List<PostTime>();
		public PostTime TempTime;
		public PostTemplate PostTemplate;
		public List<DateTime> ChosesDates = new List<DateTime>();

		public void Start(List<DateTime> calendar, PostTemplate post)
		{
			ChosesDates = new List<DateTime>();
			for (System.Int32 i = 0; i < calendar.Count; i++)
			{
				ChosesDates.Add(calendar[i]);
			}
			timeModels = new List<PostTime>();
			for (System.Int32 i = 0; i < ChosesDates.Count; i++)
			{
				if (post.PostTime.Count != 0 || ChosesDates.Count != 0)
				{
					if (post.PostTime.Any(p => p.Time.Year == ChosesDates[i].Year && p.Time.Day == ChosesDates[i].Day && p.Time.Month == ChosesDates[i].Month))
					{
						timeModels = post.PostTime.ToList();
					}
				}
				else
				{
					timeModels = new List<PostTime>();
				}
			}
			PostTemplate = post;
		}

		public void Clear()
		{
			timeModels = new List<PostTime>();
			TempTime = null;
			PostTemplate = null;
			ChosesDates = new List<DateTime>();
		}

		public PostTime GetTime(System.String time) => timeModels.Where(p => p.Time.TimeOfDay.ToString() == time).FirstOrDefault();

		public void Add_DB(DateTime time)
		{
			DataBase db = Singleton.GetInstance().Context;
			// timeModels.Add(new PostTime { Time = time });
			for (Int32 i = 0; i < ChosesDates.Count; i++)
			{
				time = ChosesDates[i].AddHours(time.Hour);
				time = ChosesDates[i].AddMinutes(time.Minute);

				//  db.AddPostTimeForPostTemplate(PostTemplate, ,new PostTime() { Time = time });
			}
		}

		public void SubMinute(PostTime time) => time.Time.AddMinutes(5);

		public void AddTime(System.DateTime time) => timeModels.Add(new PostTime() { Time = time });

		public void DeleteDate(System.String time, PostTime postTime)
		{
			DataBase db = Singleton.GetInstance().Context;
			timeModels.Remove(timeModels.Where(p => p.Time.TimeOfDay.ToString() == time).FirstOrDefault());
			db._postTime.Remove(postTime);
		}

		public void PlusMinute(PostTime time) => time.Time.AddMinutes(5);

		public void PlusHour(PostTime time) => time.Time.AddHours(5);

		public void SubHour(PostTime postTime) => postTime.Time.AddHours(-1);
	}
}