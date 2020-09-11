using System;
using System.Linq;
using System.Threading.Tasks;

using Quartz;
using Telegram.Bot;

namespace BotCore
{
	public class UpdateMessageChat : IJob
	{
		protected TelegramBotClient BotClient { get; set; }
		
		public async Task Execute(IJobExecutionContext context)
		{
			BotClient = new TelegramBotClient("1046277477:AAHxIx5MkmYSEH9qIiGYlLMTEUcz5Nage6E");
			DataBase db = Singleton.GetInstance().Context;
			ChannelMessage[] channels = db.GetChannelMessages();
			DateTime date = DateTime.Now;

			if (channels.Any(p => p.DateMessageDelete <= date))
			{
				foreach (ChannelMessage channel in channels)
				{
					if (channel.DateMessageDelete <= date)
					{
						if (channel.MessageId != 0)
						{
							BotClient.DeleteMessage(channel.ChannelID, channel.MessageId,
							                        "Не удалось удалить сообщение в канале! Класс UpdateMessageChat!");

							Channel thisChannel = db.GetChannel(channel.ChannelID);
							thisChannel.IDMessage = 0;
							db._channelMessages.Remove(channel);
							db.Save();
						}
					}
				}
			}

		}
		
	}
}