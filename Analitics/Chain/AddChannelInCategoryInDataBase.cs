using System;
using BotCore.Advertising;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCore
{
	internal class AddChannelInCategoryInDataBase : AbstractHandlerAdvertising
	{
		private User user = null;
		private Channel channel = null;
		private Category category = null;

		private Boolean GetChannel(Message _message)
		{
			DataBase db = Singleton.GetInstance().Context;
			channel = db.GetChannel(_message.Text.Split(" ")[1]);

			if (channel == null) return true;

			return false;
		}

		private Boolean GetCategory(Message _message)
		{
			DataBase db = Singleton.GetInstance().Context;
			category = db.GetCategory(_message.Text.Split(" ")[0]);

			if (category == null) return true;

			return false;
		}

		private void GetUser(Message _message)
		{
			DataBase db = Singleton.GetInstance().Context;
			user = db.GetUser(_message.From.Id);
		}

		public void SetNewCategoty(Message _message)
		{
			DataBase db = Singleton.GetInstance().Context;

			channel.CategoryId = category.Id;

			user.Chain = (Int32)SetChain.MessageUserInBot;
			db.Save();
		}

		internal void SendMessage(TelegramBotClient botClient, Message _message, System.Int32 Ex = 0)
		{
			DataBase db = Singleton.GetInstance().Context;

			botClient.DeleteMessage(_message.From.Id, _message.MessageId, "33 - AddUserInTransaction");

			InlineButton inlineBatton = new InlineButton();
			if (Ex == 0)
			{
				botClient.EditMessage(user.ID, user.MessageID, "Категория успешно добавлена!", "49 - AddPhoto", user,
				                      replyMarkup: inlineBatton.SettingBotLvl2(user));
			}
			else if (Ex == 1)
			{
				botClient.EditMessage(user.ID, user.MessageID, "Ошибка: неверный формат группы!", "49 - AddPhoto", user,
				                      replyMarkup: inlineBatton.SettingBotLvl2(user));
			}
			else if (Ex == 2)
			{
				botClient.EditMessage(user.ID, user.MessageID, "Ошибка: неверный формат категории!", "49 - AddPhoto", user,
				                      replyMarkup: inlineBatton.SettingBotLvl2(user));
			}

		}

		public override System.Object Handle(System.Int32 request, TelegramBotClient botClient, System.Object message)
		{
			if (request == (System.Int32)SetChain.AddChannelInCategory)
			{
				Message _message = message as Message;

				GetUser(_message);

				if (GetChannel(_message))
				{
					SendMessage(botClient, _message, 1);
					return null;
				}

				if (GetCategory(_message))
				{
					SendMessage(botClient, _message, 2);
					return null;
				}

				if (user.IsAdmin >= 2)
				{
					SetNewCategoty(_message);

					SendMessage(botClient, _message);
				}

				return null;
			}
			else
			{
				return base.Handle(request, botClient, message);
			}
		}
	}
}
