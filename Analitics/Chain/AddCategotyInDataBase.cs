using System;
using BotCore.Advertising;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCore
{
	internal class AddCategotyInDataBase : AbstractHandlerAdvertising
	{
		private User user = null;

		private void GetUser(Message _message)
		{
			DataBase db = Singleton.GetInstance().Context;
			user = db.GetUser(_message.From.Id);
		}

		public void SetNewCategoty(Message _message)
		{
			DataBase db = Singleton.GetInstance().Context;

			db.SetValue<Category>(new Category() { Name = _message.Text });
			user.Chain = (Int32)SetChain.MessageUserInBot;
			db.Save();
		}

		internal void SendMessage(TelegramBotClient botClient, Message _message)
		{
			DataBase db = Singleton.GetInstance().Context;

			botClient.DeleteMessage(_message.From.Id, _message.MessageId, "33 - AddUserInTransaction");

			InlineButton inlineBatton = new InlineButton();

			botClient.EditMessage(user.ID, user.MessageID, "Категория успешно добавлена!", "49 - AddPhoto", user, replyMarkup: inlineBatton.SettingBotLvl2(user));

		}

		public override System.Object Handle(System.Int32 request, TelegramBotClient botClient, System.Object message)
		{
			if (request == (System.Int32)SetChain.AddCategoty)
			{
				Message _message = message as Message;

				GetUser(_message);

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
