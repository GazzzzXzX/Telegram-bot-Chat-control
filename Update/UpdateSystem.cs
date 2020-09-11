using System;

using BotCore.Update;

using Quartz;
using Quartz.Impl;

using Telegram.Bot;

namespace BotCore
{
	internal class UpdateSystem
	{
		public void Start()
		{
			UpdatePost();
			UpdateCountReviews();
			UpdateCountPhoto();
			UpdateCountAddPeople();
			UpdateTemplate();
			UpdatePayConfirm();
			UpdateMessageDeleteChannel();
		}

		private async void UpdatePost()
		{
			IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();
			await scheduler.Start();
			
			IJobDetail job = JobBuilder.Create<UpdatePost>().Build();
			
			ITrigger trigger = TriggerBuilder.Create()  
			                                 .WithIdentity("trigger1", "group1")                 // идентифицируем триггер с именем и группой
			                                 .StartAt(DateTimeOffset.Now.AddMinutes(5))                                         // запуск сразу после начала выполнения
			                                 .WithSimpleSchedule(x => x                          // настраиваем выполнение действия
			                                                          .WithIntervalInMinutes(20) // через 24 час
			                                                          .RepeatForever())          // бесконечное повторение
			                                 .Build();                                           // создаем триггер
            
			await scheduler.ScheduleJob(job, trigger);
		}

		private async void UpdateCountReviews()
		{
			IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();
			await scheduler.Start();

			IJobDetail job = JobBuilder.Create<UpdateCountReviews>().Build();

			ITrigger trigger = TriggerBuilder.Create()  // создаем триггер
				.WithIdentity("trigger2", "group2")     // идентифицируем триггер с именем и группой
				.StartAt(DateTimeOffset.Now.AddMinutes(10)) // запуск сразу после начала выполнения
				.WithSimpleSchedule(x => x            // настраиваем выполнение действия
					.WithIntervalInHours(1)          // через 24 час
					.RepeatForever())                   // бесконечное повторение
				.Build();                               // создаем триггер

			await scheduler.ScheduleJob(job, trigger);        // начинаем выполнение работы
		}

		private async void UpdateCountPhoto()
		{
			IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();
			await scheduler.Start();

			IJobDetail job = JobBuilder.Create<UpdatePhoto>().Build();

			ITrigger trigger = TriggerBuilder.Create()  // создаем триггер
				.WithIdentity("trigger3", "group3")     // идентифицируем триггер с именем и группой
				.StartAt(DateTimeOffset.Now.AddMinutes(15)) // запуск сразу после начала выполнения
				.WithSimpleSchedule(x => x            // настраиваем выполнение действия
					.WithIntervalInHours(24)          // через 24 час
					.RepeatForever())                   // бесконечное повторение
				.Build();                               // создаем триггер

			await scheduler.ScheduleJob(job, trigger);        // начинаем выполнение работы
		}

		private async void UpdateCountAddPeople()
		{
			IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();
			await scheduler.Start();

			IJobDetail job = JobBuilder.Create<UpdateAddPeople>().Build();

			ITrigger trigger = TriggerBuilder.Create()  // создаем триггер
				.WithIdentity("trigger4", "group4")     // идентифицируем триггер с именем и группой
				.StartAt(DateTimeOffset.Now.AddMinutes(20)) // запуск сразу после начала выполнения
				.WithSimpleSchedule(x => x            // настраиваем выполнение действия
					.WithIntervalInHours(720)          // через 1 месяц
					.RepeatForever())                   // бесконечное повторение
				.Build();                               // создаем триггер

			await scheduler.ScheduleJob(job, trigger);        // начинаем выполнение работы
		}

		private async void UpdateTemplate()
		{
			IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();
			await scheduler.Start();

			IJobDetail job = JobBuilder.Create<UpdatePostTemplate>().Build();

			ITrigger trigger = TriggerBuilder.Create()  // создаем триггер
				.WithIdentity("trigger5", "group5")     // идентифицируем триггер с именем и группой
				.StartAt(DateTimeOffset.Now.AddMinutes(25)) // запуск сразу после начала выполнения
				.WithSimpleSchedule(x => x            // настраиваем выполнение действия
					.WithIntervalInMinutes(10)          // через 30 секунд
					.RepeatForever())                   // бесконечное повторение
				.Build();                               // создаем триггер

			await scheduler.ScheduleJob(job, trigger);        // начинаем выполнение работы
		}

		public static async void DeletePinnedMessages(TelegramBotClient client)
		{
			IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();
			await scheduler.Start();

			JobDataMap data = new JobDataMap
			{
				new System.Collections.Generic.KeyValuePair<String, Object>("client", client)
			};

			IJobDetail detail = JobBuilder.Create<DeletePinnedMessagesJob>().SetJobData(data).Build();

			ITrigger trigger = TriggerBuilder.Create()
				.WithIdentity("trigger6", "group6")
				.WithSchedule(CronScheduleBuilder
				.DailyAtHourAndMinute(11, 55)
					.InTimeZone(TimeZoneInfo.Local))
				.Build();

			await scheduler.ScheduleJob(detail, trigger);
		}

		private async void UpdatePayConfirm()
		{
			IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();
			await scheduler.Start();

			IJobDetail job = JobBuilder.Create<UpdatePay>().Build();

			ITrigger trigger = TriggerBuilder.Create()  // создаем триггер
				.WithIdentity("trigger7", "group7")     // идентифицируем триггер с именем и группой
				.StartAt(DateTimeOffset.Now.AddMinutes(30)) // запуск сразу после начала выполнения
				.WithSimpleSchedule(x => x            // настраиваем выполнение действия
					.WithIntervalInHours(12)          // через 24 час
					.RepeatForever())                   // бесконечное повторение
				.Build();                               // создаем триггер

			await scheduler.ScheduleJob(job, trigger);        // начинаем выполнение работы
		}

		private async void UpdateMessageDeleteChannel()
		{
			IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();
			await scheduler.Start();
            			
			IJobDetail job = JobBuilder.Create<UpdateMessageChat>().Build();

			ITrigger trigger = TriggerBuilder.Create()  
			                                 .WithIdentity("trigger8", "group8")               // идентифицируем триггер с именем и группой
			                                 .StartAt(DateTimeOffset.Now.AddMinutes(40)) // запуск сразу после начала выполнения
			                                 .WithSimpleSchedule(x => x                        // настраиваем выполнение действия
			                                                          .WithIntervalInSeconds(20) // через 24 час
			                                                          .RepeatForever())        // бесконечное повторение
			                                 .Build();                                         // создаем триггер
            
			await scheduler.ScheduleJob(job, trigger);
		}
	}
}