using System;
using BotCore.Advertising;

using Telegram.Bot;
using Telegram.Bot.Types;
namespace BotCore
{
	internal class LinkChatPharase : AbstractHandlerAdvertising
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

		private void GetUser(Message _message)
		{
			DataBase db = Singleton.GetInstance().Context;
			user = db.GetUser(_message.From.Id);
		}

		internal void SendMessage(TelegramBotClient botClient, Message _message) => botClient.EditMessage(user.ID, user.MessageID, "Идет обработка, это может занят некоторое время...", "39 - AddPhotoInDataBase", user);

		private void ChangeUser()
		{
			DataBase db = Singleton.GetInstance().Context;
			user.Chain = (Int32)SetChain.MessageUserInBot;
			db.Save();
		}

		public override System.Object Handle(System.Int32 request, TelegramBotClient botClient, System.Object message)
		{
			if (request == (System.Int32)SetChain.LinkChatPharase)
			{
				Message _message = message as Message;
				DataBase db = Singleton.GetInstance().Context;

				if (SearchChannel(_message)) return null;

				GetUser(_message);

				ChangeUser();

				SendMessage(botClient, _message);

				AnaliticsPhrase[] analyticsTexts = db.GetAnaliticsPharse();

				PrintExel.GetAnaliticsInOneChatPharase(analyticsTexts, channel, botClient, _message, user);

				return null;
			}
			else
			{
				return base.Handle(request, botClient, message);
			}
		}
	}
}
