using System;
using BotCore.Advertising;
using BotCore.Blockchain;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCore
{
	public class NotificationButtonChain : AbstractHandlerAdvertising, IStandartCommand
	{
		private User user = null;
		private DataBase db = null;
		private Message _message = null;
		private ButtonNotification button = new ButtonNotification();
		private System.String Text = "Ваше сообщение:\n";

		public override System.Object Handle(System.Int32 request, TelegramBotClient botClient, System.Object message)
		{
			if (request == (Int32)SetChain.NotificationButton)
			{
				IStandartCommand standartCommand = new AddAccauntBotUser();

				if (standartCommand.SetMessage(message, out _message)) return null;

				if (standartCommand.SetDataBase(out db)) return null;

				if (standartCommand.SetUserAndCheckIsNullMessage(botClient, _message, out user, db)) return null;
				
				ChangeUser();

				ChangeNotificationButton();

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

		private void ChangeNotificationButton()
		{
			button.Text = _message.Text;

			ButtonNotification ButtonTemp = db.GetButtonNotification(button);
			
			if(ButtonTemp == null)
			{
				db.SetValue(button);
			}

			ButtonAndTextNotication temp = db.GetButtonAndTextNotication(user);

			if (temp.Text != null)
			{
				Text += temp.Text.Text + "\n";
			}

			Text += button.Text;
			
			db.SetValue(new CollectionButtonNotification() { buttonAndTextNotification = temp, buttonNotification = button});
			db.Save();
		}

		public void SendMessage(TelegramBotClient botClient)
		{
			InlineButton inlineButton = new InlineButton();

			botClient.DeleteMessage(_message.From.Id, _message.MessageId, "63 - AddAccauntCode");
			
			botClient.EditMessage(_message.From.Id, user.MessageID, Text, "", user, inlineButton.NotificationBot());
		}
	}
}