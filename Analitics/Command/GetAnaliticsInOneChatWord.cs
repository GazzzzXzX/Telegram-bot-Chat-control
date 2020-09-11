using System;
using BotCore.Advertising;
using BotCore.Blockchain;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCore.PhotoChannel
{
	internal class GetAnaliticsInOneChatWord : Advertising.Command.AbsCommand, IStandartCommand
	{
		public override System.String Name { set; get; } = PhotoCommandName.GetAnaliticsInOneChatWord;

		private DataBase db = null;
		private User user = null;
		private CallbackQuery _message = null;

		public override void Execute(TelegramBotClient botClient, System.Object message)
		{
			IStandartCommand standartCommand = new GetAnaliticsInOneChatWord();

			if (standartCommand.SetCallbackQuery(message, out _message)) return;

			if (standartCommand.SetDataBase(out db)) return;

			if (standartCommand.SetUserAndCheckIsNull(botClient, _message, out user, db)) return;
			if (IsBan.Ban(botClient, message))
			{
				ChangeUser();

				SendMessage(botClient);
			}
		}

		public void SendMessage(TelegramBotClient botClient)
		{
			InlineButton inlineButton = new InlineButton();
			botClient.EditMessage(user.ID, user.MessageID, "Введите ссылку на чат: ", "39 - AddPhotoInDataBase", user);
		}

		private void ChangeUser()
		{
			user.Chain = (Int32)SetChain.LinkChat;
			db.Save();
		}
	}
}
