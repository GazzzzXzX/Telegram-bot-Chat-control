using System;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace BotCore
{
	internal static class ExtentionsClient
	{
		public static Message messageDelete;
		public static async void DeleteMessage(this TelegramBotClient botClient, System.Int64 ChatOrFromId, System.Int32 messageId, System.String ex)
		{
			try
			{
				await botClient.DeleteMessageAsync(ChatOrFromId, messageId);
			}
			catch(Exception exception)
			{
				return;
			}
		}

		public static async void EditMessage(this TelegramBotClient botClient, System.Int64 ChatOrFromId, System.Int32 messageId, System.String text, System.String ex, User user = null, InlineKeyboardMarkup replyMarkup = null, System.Boolean IsMarkdown = false)
		{
			DataBase db = Singleton.GetInstance().Context;
			try
			{
				Message mes;
				if (replyMarkup == null)
				{
					if (IsMarkdown)
					{
						mes = await botClient.EditMessageTextAsync(ChatOrFromId, messageId, text, Telegram.Bot.Types.Enums.ParseMode.Markdown);
					}
					else
					{
						mes = await botClient.EditMessageTextAsync(ChatOrFromId, messageId, text, Telegram.Bot.Types.Enums.ParseMode.Html);
					}
				}
				else
				{
					if (IsMarkdown)
					{
						mes = await botClient.EditMessageTextAsync(ChatOrFromId, messageId, text, Telegram.Bot.Types.Enums.ParseMode.Markdown, replyMarkup: replyMarkup);
					}
					else
					{
						mes = await botClient.EditMessageTextAsync(ChatOrFromId, messageId, text, Telegram.Bot.Types.Enums.ParseMode.Html, replyMarkup: replyMarkup);
					}
				}
				if (user != null)
				{
					user.MessageID = mes.MessageId;
					db.Save();
				}
			}
			catch (System.Exception exception)
			{
				Log.Logging("Не удалось изменить текст: " + ex + " " + exception);
			}
		}

		public static async void SendText(this TelegramBotClient botClient, System.Int64 ChatOrFromId, System.String text, User user = null, System.Boolean isBan = false, IReplyMarkup replyMarkup = null, System.Boolean IsMarkdown = false)
		{
			try
			{
				DataBase db = Singleton.GetInstance().Context;
				Message mes;
				if (replyMarkup == null)
				{
					if (IsMarkdown)
					{
						mes = await botClient.SendTextMessageAsync(ChatOrFromId, text, Telegram.Bot.Types.Enums.ParseMode.Markdown);
					}
					else
					{

						mes = await botClient.SendTextMessageAsync(ChatOrFromId, text, Telegram.Bot.Types.Enums.ParseMode.Html);
					}
				}
				else
				{
					if (IsMarkdown)
					{
						mes = await botClient.SendTextMessageAsync(ChatOrFromId, text, Telegram.Bot.Types.Enums.ParseMode.Markdown, replyMarkup: replyMarkup);
					}
					else
					{
						mes = await botClient.SendTextMessageAsync(ChatOrFromId, text, Telegram.Bot.Types.Enums.ParseMode.Html, replyMarkup: replyMarkup);
					}
				}
				if (user != null && isBan == false)
				{
					user.MessageID = mes.MessageId;
					db.Save();
				}
				else if (user != null && isBan == true)
				{
					user.MessageBan = mes.MessageId;
					db.Save();
				}
			}
			catch
			{
			}
		}

		public static async void Kick(this TelegramBotClient botClient, System.Int64 channel, System.Int32 userid, System.String ex)
		{
			try
			{
				await botClient.KickChatMemberAsync(channel, userid);
			}
			catch
			{
				Log.Logging("Пользователь не кикнут: " + ex);
			}
		}
	}
}