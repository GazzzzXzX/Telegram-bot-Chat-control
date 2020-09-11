using System;
using System.Linq;
using BotCore.Advertising;
using BotCore.Blockchain;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCore
{
	internal class NotificationSetCaht: AbsCommand, IStandartCommand
	{
		public override System.String Name { set; get; } = CommandText.NotificationSetCaht;
	
		private DataBase db = null;
		private User user = null;
		private CallbackQuery _message = null;
		private ButtonAndTextNotication TextAndButton = null;
	
		public override void Execute(TelegramBotClient botClient, System.Object message)
		{
			IStandartCommand standartCommand = new NotificationChannelId();
	
			if (standartCommand.SetCallbackQuery(message, out _message)) return;
	
			if (standartCommand.SetDataBase(out db)) return;
	
			if (standartCommand.SetUserAndCheckIsNull(botClient, _message, out user, db)) return;
			
			SendMessage(botClient);
		}

		public void SendMessage(TelegramBotClient botClient)
		{
			InlineButton inlineButton = new InlineButton();
			
			String Text = "🔔Выберете чат🔔";
				
			botClient.EditMessage(user.ID, user.MessageID, Text, "", user,
			                      inlineButton.PublishNotificationAllOrOneChat());
			
			
		}
	}
}