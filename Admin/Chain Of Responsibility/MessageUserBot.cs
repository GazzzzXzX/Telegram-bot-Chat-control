using System;
using System.Linq;
using BotCore.Advertising;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCore
{
	internal class MessageUserBot : AbstractHandlerAdvertising
	{
		private User user = null;
		private User userTwo = null;

		private Boolean SetUser(TelegramBotClient botClient, Message _message, DataBase db)
		{
			user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return true;
			if (_message.ForwardFrom != null)
			{
				userTwo = db.GetUser(_message.ForwardFrom.Id); if (IsNullDataBase.IsNull(botClient, _message, userTwo)) return true;
			}
			else
			{
				return true;
			}
			return false;
		}

		private void SetMessage(Message _message)
		{
			DataBase db = Singleton.GetInstance().Context;
			user.MessageComplaint = _message.Text;
			user.IDRecipient = userTwo.ID;
			db.Save();
		}

		internal void SendMessage(TelegramBotClient botClient, Message _message)
		{
			DataBase db = Singleton.GetInstance().Context;

			Settings settings = db.GetSettings();

			InlineButton inlineBatton = new InlineButton();

			System.String temp = InfoUser.Search(userTwo, user);

			botClient.DeleteMessage(_message.From.Id, _message.MessageId, "33 - MessageUserBot");
			if (user.IsAdmin > 0)
			{
				botClient.EditMessage(_message.From.Id, user.MessageID, temp, "45 - MessageUserBot", user, replyMarkup: inlineBatton.InteractionUsers(userTwo, db._featuredUserNews.Any(p => p.UserId == userTwo.ID && p.UserWhoAddedId == user.ID), db.GetFeaturedUsers(user, userTwo), isAdmin: true));
			}
			else
			{
				botClient.EditMessage(_message.From.Id, user.MessageID, temp, "45 - MessageUserBot", user, replyMarkup: inlineBatton.InteractionUsers(userTwo, db._featuredUserNews.Any(p => p.UserId == userTwo.ID && p.UserWhoAddedId == user.ID), db.GetFeaturedUsers(user, userTwo), isAdmin: false));
			}
		}

		public override System.Object Handle(System.Int32 request, TelegramBotClient botClient, System.Object message)
		{
			if (request == (System.Int32)SetChain.MessageUserInBot)
			{
				Message _message = message as Message;
				DataBase db = Singleton.GetInstance().Context;

				if (SetUser(botClient, _message, db)) return null;

				SetMessage(_message);

				SendMessage(botClient, _message);

				return null;
			}
			else
			{
				return base.Handle(request, botClient, message);
			}
		}
	}
}
