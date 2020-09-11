using System;
using System.Threading.Tasks;

using Quartz;

namespace BotCore
{
	internal class UpdatePost : IJob
	{
		public async Task Execute(IJobExecutionContext context)
		{
			DataBase db = Singleton.GetInstance().Context;
			TMessage[] tmessages = db.GetTMessages();
			Channel[] channels = db.GetChannels();
			
			foreach (Channel item in channels)
			{
				foreach (TMessage message in tmessages)
				{
					if (message.channel.IDChannel == item.IDChannel)
					{
						if (message.dateTime == System.DateTime.Today || message.Post == item.PostCount)
						{
							continue;
						}
						message.Post = item.PostCount;
						message.dateTime = System.DateTime.Today;
					}
				}
			}

			db.Save();
			
			UserMessage[] userMessages = db.GetUserMessages();

			foreach (UserMessage userMessage in userMessages)
			{
				if (userMessage.dateTime == System.DateTime.Today)
				{
					continue;
				}
				Singleton.GetInstance().Context._userMessages.Remove(userMessage);
			}
			db.Save();
		}
	}
}