using System;
using BotCore.Advertising;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCore
{
	internal class AddPhotoChannel : AbstractHandlerAdvertising
	{
		private Channel channel = null;
		private User user = null;


		private Boolean SearchChannel(Message _message)
		{
			DataBase db = Singleton.GetInstance().Context;


			channel = db.GetChannel(_message.Text.Split(" ")[0]);
			if (channel != null) return false;

			return true;
		}

		private void ChangeChannel(Message _message)
		{
			DataBase db = Singleton.GetInstance().Context;
			channel.PhotoLink = _message.Text.Split(" ")[1];
			db.Save();
		}

		private void GetUser(Message _message)
		{
			DataBase db = Singleton.GetInstance().Context;
			user = db.GetUser(_message.From.Id);
		}

		internal void SendMessage(TelegramBotClient botClient, Message _message)
		{
			DataBase db = Singleton.GetInstance().Context;

			botClient.DeleteMessage(_message.From.Id, _message.MessageId, "33 - AddUserInTransaction");

			InlineButton inlineBatton = new InlineButton();

			botClient.EditMessage(user.ID, user.MessageID, "Ссылка успешно добавлена!", "49 - AddPhoto", user, replyMarkup: inlineBatton.SettingBotLvl2(user));

		}

		public override System.Object Handle(System.Int32 request, TelegramBotClient botClient, System.Object message)
		{
			if (request == (System.Int32)SetChain.SetPhoto)
			{
				Message _message = message as Message;

				if (SearchChannel(_message)) return null;

				GetUser(_message);

				ChangeChannel(_message);

				SendMessage(botClient, _message);

				return null;
			}
			else
			{
				return base.Handle(request, botClient, message);
			}
		}
	}
}
