using System;
using BotCore.Advertising;
using BotCore.Blockchain;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCore
{
	internal class ButtounNotification : AbsCommand, IStandartCommand
	{
		public override System.String Name { set; get; } = CommandText.ButtounNotification;
	
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
			user.Chain = (Int32)SetChain.MessageUserInBot;
			db.Save();
		}
	
		public void SendMessage(TelegramBotClient botClient)
		{
			InlineButton inlineButton = new InlineButton();
			String       Text         = "🔔Выберете/Добавьте кнопку🔔";
			botClient.EditMessage(user.ID, user.MessageID, Text, "", user, inlineButton.NotificationBotButton());
		}
	}
}