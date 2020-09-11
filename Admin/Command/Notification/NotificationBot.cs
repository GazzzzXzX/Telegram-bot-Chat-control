using System;
using System.Collections.Generic;
using BotCore.Blockchain;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCore
{
	internal class NotificationBot : AbsCommand, IStandartCommand
	{
		public override System.String Name { set; get; } = CommandText.NotificationBot;
	
		private DataBase db = null;
		private User user = null;
		private CallbackQuery _message = null;
	
		public override void Execute(TelegramBotClient botClient, System.Object message)
		{
			IStandartCommand standartCommand = new CoinAdd();
	
			if (standartCommand.SetCallbackQuery(message, out _message)) return;
	
			if (standartCommand.SetDataBase(out db)) return;
	
			if (standartCommand.SetUserAndCheckIsNull(botClient, _message, out user, db)) return;

			SetNotification();
			
			SendMessage(botClient);
		}

		private void SetNotification()
		{
			db.SetValue<ButtonAndTextNotication>(new ButtonAndTextNotication() { User = user, isWork = true});
		}
	
		public void SendMessage(TelegramBotClient botClient)
		{
			InlineButton inlineButton = new InlineButton();
			String       Text         = "🔔Уведомление🔔";
			botClient.EditMessage(user.ID, user.MessageID, Text, "", user, inlineButton.NotificationBot());
		}
	}
}