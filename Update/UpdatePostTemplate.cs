using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using BotCore.SQL;

using Quartz;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCore
{
	internal class UpdatePostTemplate : IJob
	{
		protected TelegramBotClient BotClient { get; set; }

		public async Task Execute(IJobExecutionContext context)
		{
			BotClient = new TelegramBotClient("1046277477:AAHxIx5MkmYSEH9qIiGYlLMTEUcz5Nage6E");
			DataBase db = Singleton.GetInstance().Context;
			PostTemplate[] postTemplate = db.GetPostTemplates();

			foreach (PostTemplate post in postTemplate)
			{
				if (post.IsPaid == true)
				{
					foreach (PostTime time in post.PostTime)
					{
						if (time.Time.Year == System.DateTime.Now.Year && time.Time.Month == System.DateTime.Now.Month
							&& time.Time.Day == System.DateTime.Now.Day && time.Time.Hour == System.DateTime.Now.Hour && time.Time.Minute == System.DateTime.Now.Minute && time.isWorked == false)
						{
							foreach (PostChannel channel in post.PostChannel)
							{
								try
								{
									IOrderedEnumerable<PostContent> orderedList = post.PostContent.OrderBy(c => c.Order);

									Message textMessage = null;
									List<InputMediaBase> inputMedias = new List<InputMediaBase>();
									foreach (PostContent content in orderedList)
									{
										Message forward = await BotClient.ForwardMessageAsync(CommandText.bufferChannelId, CommandText.bufferChannelId, content.MessageId);

										if (forward.Type != Telegram.Bot.Types.Enums.MessageType.Text)
										{
											InputMediaPhoto photo = new InputMediaPhoto(new InputMedia(forward.Photo[0].FileId));
											inputMedias.Add(photo);
										}
										else
										{
											textMessage = forward;
										}
									}

									try
									{
										Message[] messages =
											await BotClient.SendMediaGroupAsync(channel.ChannelId, inputMedias);

										foreach (Message item in messages)
										{
											db.Add<Post>(new Post()
											{
												Template  = post,
												ChannelId = item.Chat.Id,
												MessageId = item.MessageId
											});
										}

										Message message;
										message = await BotClient.ForwardMessageAsync(channel.ChannelId,
										                                              CommandText.bufferChannelId,
										                                              textMessage.MessageId);

										if (post.isPinnedMessage == (System.Int32)TypePostTime.PinnedMessage)
										{
											await BotClient.PinChatMessageAsync(channel.ChannelId, message.MessageId,
											                                    disableNotification: false);
										}
										else if (post.isPinnedMessage ==
										         (System.Int32)TypePostTime.PinnedMessageNotification)
										{
											await BotClient.PinChatMessageAsync(channel.ChannelId, message.MessageId,
											                                    disableNotification: true);
										}

										db.Add<Post>(new Post()
										{
											Template  = post,
											ChannelId = message.Chat.Id,
											MessageId = message.MessageId
										});
									}
									catch(System.Exception ex)
									{
										Log.Logging(ex);
									}

									time.isWorked = true;
									db.Save();
								}
								catch (System.Exception ex)
								{
									Log.Logging(ex);
								}
							}
						}
					}
				}
			}

			foreach (PostTemplate post in postTemplate)
			{
				System.Boolean result = true;
				foreach (PostTime time in post.PostTime)
				{
					if (time.isWorked == false)
					{
						result = false;
						break;
					}
				}

				if (result)
				{
					post.PostTime.Clear();
					post.IsPaid = false;
					post.IsValidated = false;
					post.IsOnValidation = false;
					db.Save();
				}
			}
		}
	}
}