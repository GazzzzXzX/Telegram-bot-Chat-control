using System;

using BotCore.Blockchain;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCore.Advertising
{
	internal class AdminCallBlockchainUser : AbstractHandlerAdvertising
	{
		private User SearchUser(Message _message)
		{
			DataBase db = Singleton.GetInstance().Context;
			if (_message.ForwardFrom != null)
			{
				return db.GetUser(_message.ForwardFrom.Id);
			}
			else
			{
				System.Int32 temp = 0;
				System.Int32.TryParse(_message.Text, out temp);
				return db.GetUser(temp);
			}
		}

		internal void SendMessage(TelegramBotClient botClient, Message _message)
		{
			DataBase db = Singleton.GetInstance().Context;
			User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return; // - заменить на другую таблицу

			botClient.DeleteMessage(_message.Chat.Id, _message.MessageId, "33 - AddUserInTransaction");

			botClient.EditMessage(_message.From.Id, user.MessageID, "Ваша заявка была отправлена администраторам, ожидайте в ближайшее время с вами свяжутся!", "45 - AddUserInTransaction", user, replyMarkup: InlineButtonBlockchain.GuarantorMeinMenu(user));

			String temp = $"Помощь по гаранту\n🆔: {user.ID}\n🖌ФИО: {user.FIO}\n📱Номер: {user.Number}\n";
			temp += user.Username != "Нет!" ? "🧸Юзернейм: @" + user.Username : "";
			temp += $"\nСообщение: \n{_message.Text}";

			Settings settings = db.GetSettings();

			botClient.SendText(settings.ChannelAdmin, temp);
		}

		public override System.Object Handle(System.Int32 request, TelegramBotClient botClient, System.Object message)
		{
			if (request == (System.Int32)SetChain.AdminCallBlockchain)
			{
				Message _message = message as Message;

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
