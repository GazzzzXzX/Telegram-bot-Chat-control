using System;
using System.Linq;
using BotCore.Advertising;
using BotCore.Blockchain;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCore
{
	internal  class NotificationChannelId : AbsCommand, IStandartCommand, ISplitNameInt64
	{
		public override System.String Name { set; get; } = CommandText.NotificationChannelId;
	
		private DataBase db = null;
		private User user = null;
		private CallbackQuery _message = null;
		private ButtonAndTextNotication TextAndButton = null;
		private Int64 KeyChannel;
	
		public override void Execute(TelegramBotClient botClient, System.Object message)
		{
			IStandartCommand standartCommand = new NotificationChannelId();
			ISplitNameInt64 splitNameInt64 = new NotificationChannelId();
	
			if (standartCommand.SetCallbackQuery(message, out _message)) return;
	
			if (standartCommand.SetDataBase(out db)) return;
	
			if (standartCommand.SetUserAndCheckIsNull(botClient, _message, out user, db)) return;

			KeyChannel = splitNameInt64.GetNameSplit(Name);

			Name = CommandText.NotificationChannelId;
			
			ChangeNotificationChat();
			
			SendMessage(botClient);
		}

		private void ChangeNotificationChat()
		{
			Channel channel = db.GetChannel(KeyChannel);
			ButtonAndTextNotication buttonAndTextNotication = db.GetButtonAndTextNotication(user);
			NotificationChat[] notificationChats = db.GetNotificationChats();
			NotificationChat notificationChat =
				notificationChats.FirstOrDefault(p => p.IdChannel == channel && p.IdNotification == buttonAndTextNotication.Id);
			
			if (notificationChat == null)
			{
				db.SetValue(new NotificationChat() { IdChannel = channel, IdNotification = buttonAndTextNotication.Id});
			}
			else
			{
				db.Remove(notificationChat);
			}
			db.Save();
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