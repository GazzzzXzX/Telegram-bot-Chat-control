using System.Threading.Tasks;

using Quartz;

using Telegram.Bot;

namespace BotCore.Update
{
	public class DeletePinnedMessagesJob : IJob
	{
		public async Task Execute(IJobExecutionContext context)
		{
			JobDataMap data = context.MergedJobDataMap;

			TelegramBotClient client = (TelegramBotClient) data["client"];
			DataBase db = Singleton.GetInstance().Context;

			Channel[] channels = db.GetChannels();

			foreach (Channel channel in channels)
			{
				Telegram.Bot.Types.Chat channelInfo = await client.GetChatAsync(channel.IDChannel);

				if (channelInfo.PinnedMessage != null)
				{
					await client.UnpinChatMessageAsync(channelInfo.Id);
				}
			}
		}
	}
}