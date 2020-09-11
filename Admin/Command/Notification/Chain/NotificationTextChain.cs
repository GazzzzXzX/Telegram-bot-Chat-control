using System;
using BotCore.Advertising;
using BotCore.Blockchain;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCore
{
	public class NotificationTextChain : AbstractHandlerAdvertising, IStandartCommand
	{
		private User user = null;
		private DataBase db = null;
		private Message _message = null;
		private TextNotification _Text = null;

		public override System.Object Handle(System.Int32 request, TelegramBotClient botClient, System.Object message)
		{
			if (request == (Int32)SetChain.NotificationText)
			{
				IStandartCommand standartCommand = new AddAccauntBotUser();

				if (standartCommand.SetMessage(message, out _message)) return null;

				if (standartCommand.SetDataBase(out db)) return null;

				if (standartCommand.SetUserAndCheckIsNullMessage(botClient, _message, out user, db)) return null;
				
				ChangeUser();

				ChangeNotificztionText();

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

		private void ChangeNotificztionText()
		{
			_Text = new TextNotification {Text = _message.Text};

			TextNotification TextTemp = db.GetTextNotification(_Text);

			if (TextTemp == null)
			{
				db.SetValue(_Text);
			}

			ButtonAndTextNotication temp = db.GetButtonAndTextNotication(user);
			
			ButtonAndTextNotication tempText = db.GetButtonAndTextNotication(_Text.Text);
			if (tempText == null)
			{
				temp.Text = _Text;
			}
			else
			{
				temp.isWork = false;
				tempText.isWork = true;
				tempText.User = user;
			}

			db.Save();
		}

		public void SendMessage(TelegramBotClient botClient)
		{
			InlineButton inlineButton = new InlineButton();

			botClient.DeleteMessage(_message.From.Id, _message.MessageId, "63 - AddAccauntCode");

			String text = "Ваше сообщение:\n" + _message.Text;
			
			botClient.EditMessage(_message.From.Id, user.MessageID, text, "", user, inlineButton.NotificationBot());
		}
	}
}