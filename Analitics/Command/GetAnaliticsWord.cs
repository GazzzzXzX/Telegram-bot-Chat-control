﻿using BotCore.Blockchain;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCore.PhotoChannel
{
	internal class GetAnaliticsWord : Advertising.Command.AbsCommand, IStandartCommand
	{
		public override System.String Name { set; get; } = PhotoCommandName.GetAnaliticsWord;

		private DataBase db = null;
		private User user = null;
		private CallbackQuery _message = null;

		public override void Execute(TelegramBotClient botClient, System.Object message)
		{
			IStandartCommand standartCommand = new GetAnaliticsWord();

			if (standartCommand.SetCallbackQuery(message, out _message)) return;

			if (standartCommand.SetDataBase(out db)) return;

			if (standartCommand.SetUserAndCheckIsNull(botClient, _message, out user, db)) return;
			if (IsBan.Ban(botClient, message))
			{
				SendMessage(botClient);

				AnaliticsTextAllChat[] analyticsTexts = db.GetAnaliticsAllChat();

				PrintExel.GetAnaliticsWord(analyticsTexts, botClient, _message, user);
			}
		}

		public void SendMessage(TelegramBotClient botClient)
		{
			InlineButton inlineButton = new InlineButton();
			botClient.EditMessage(user.ID, user.MessageID, "Идет обработка, это может занят некоторое время...", "39 - AddPhotoInDataBase", user);
		}
	}
}
