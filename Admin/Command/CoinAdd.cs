﻿using System;
using BotCore.Blockchain;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCore
{
	internal class CoinAdd : AbsCommand, IStandartCommand
	{
		public override System.String Name { set; get; } = CommandText.CoinAdd;

		private DataBase db = null;
		private User user = null;
		private CallbackQuery _message = null;

		public override void Execute(TelegramBotClient botClient, System.Object message)
		{
			IStandartCommand standartCommand = new CoinAdd();

			if (standartCommand.SetCallbackQuery(message, out _message)) return;

			if (standartCommand.SetDataBase(out db)) return;

			if (standartCommand.SetUserAndCheckIsNull(botClient, _message, out user, db)) return;

			SendMessage(botClient);
		}

		public void SendMessage(TelegramBotClient botClient)
		{
			InlineButton inlineButton = new InlineButton();
			String Text = "Количество бонусов за добавленого человека:";
			botClient.EditMessage(user.ID, user.MessageID, Text, "39 - AddPhotoInDataBase", user, inlineButton.CoinAdd());
		}
	}
}
