using System;
using BotCore.Advertising;
using BotCore.Blockchain;

using Telegram.Bot;
using Telegram.Bot.Types;


namespace BotCore
{
	internal class AddAccauntCode : AbstractHandlerAdvertising, IStandartCommand
	{
		private User user = null;
		private DataBase db = null;
		private Message _message = null;

		public override System.Object Handle(System.Int32 request, TelegramBotClient botClient, System.Object message)
		{
			if (request == (Int32)SetChain.AddCodeUser)
			{
				IStandartCommand standartCommand = new AddAccauntBotUser();

				if (standartCommand.SetMessage(message, out _message)) return null;

				if (standartCommand.SetDataBase(out db)) return null;

				if (standartCommand.SetUserAndCheckIsNullMessage(botClient, _message, out user, db)) return null;

				if (SetCode(botClient)) return null;
				ChangeUser();

				SendMessage(botClient);

				return null;
			}
			else
			{
				return base.Handle(request, botClient, message);
			}
		}

		private void ChangeUser()
		{
			user.Chain = (Int32)SetChain.MessageUserInBot;
			db.Save();
		}

		private Boolean SetCode(TelegramBotClient botClient) => StartSession.SetCode(botClient, _message, _message.Text, user, db);

		public void SendMessage(TelegramBotClient botClient)
		{
			InlineButton inlineButton = new InlineButton();

			botClient.DeleteMessage(_message.From.Id, _message.MessageId, "63 - AddAccauntCode");
			botClient.EditMessage(_message.From.Id, user.MessageID, "Введите код: ", "62 - AddAccauntCode", user, inlineButton.BackToSetting);
		}
	}
}
