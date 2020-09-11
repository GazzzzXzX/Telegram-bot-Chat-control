using System;
using System.Collections.Generic;
using System.Linq;

using BotCore.SQL;

using Telegram.Bot;

namespace BotCore
{
	internal static class IsDataTaken
	{
		public static void IsCheck(TelegramBotClient botClient, Int32 mes, List<PostTime> postTime)
		{
			DataBase db = Singleton.GetInstance().Context;
			PostTemplate[] postTemplate = db.GetPostTemplates();
			List<PostChannel> postChannels = db.GetPostChannels();

			foreach (PostTemplate post in postTemplate)
			{
				if (post.AdUser.UserId != mes)
				{
					foreach (PostTime time in post.PostTime)
					{
						foreach (PostTime item in postTime)
						{
							Boolean temp  = false;
							if (time.Time == item.Time)
							{
								foreach (PostChannel channel in postChannels)
								{
									if (post.PostChannel.Any(p => p.ChannelId == channel.ChannelId))
									{
										if (temp == false)
										{
											temp = true;
											db.DeletePostTime(time);

											botClient.SendTextMessageAsync(post.AdUserId, "ййй");
										}
									}
								}
							}
						}
					}
				}
			}
			db.Save();
		}
	}
}