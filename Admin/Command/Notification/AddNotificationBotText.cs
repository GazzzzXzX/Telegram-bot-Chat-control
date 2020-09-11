using System;
using BotCore.Advertising;
using BotCore.Blockchain;
using RestSharp.Extensions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCore
{
	internal class AddNotificationBotText : AbsCommand, IStandartCommand
	{
		public override System.String Name { set; get; } = CommandText.AddNotificationBotText;
	
		private DataBase db = null;
		private User user = null;
		private CallbackQuery _message = null;
	
		public override void Execute(TelegramBotClient botClient, System.Object message)
		{
			IStandartCommand standartCommand = new CoinAdd();
	
			if (standartCommand.SetCallbackQuery(message, out _message)) return;
	
			if (standartCommand.SetDataBase(out db)) return;
	
			if (standartCommand.SetUserAndCheckIsNull(botClient, _message, out user, db)) return;

			ChangeUser();
			
			SendMessage(botClient);
		}

		private void ChangeUser()
		{
			user.Chain = (Int32)SetChain.NotificationText;
			db.Save();
		}
	
		public void SendMessage(TelegramBotClient botClient)
		{
			InlineButton inlineButton = new InlineButton();
			String       Text         = "🔔Введите текст:🔔";
			botClient.EditMessage(user.ID, user.MessageID, Text, "", user, inlineButton.BackToNotificationBotText);
		}
	}
}