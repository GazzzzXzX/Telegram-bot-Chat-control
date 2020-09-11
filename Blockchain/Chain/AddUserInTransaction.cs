using System;

using BotCore.Blockchain;
using BotCore.SQL;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCore.Advertising
{
	internal class SetUserInTransaction : AbstractHandlerAdvertising
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

			User userTwo = SearchUser(_message);

			botClient.DeleteMessage(_message.Chat.Id, _message.MessageId, "33 - AddUserInTransaction");

			if (userTwo != null)
			{
				Transaction transaction = db.GetTransaction(user);
				transaction.AddUser = false;
				user.Chain = (Int32)SetChain.MessageUserInBot;
				db.Save();

				botClient.EditMessage(_message.From.Id, user.MessageID, "Ожидайте пока участник подтвердить действие.", "45 - AddUserInTransaction", user, replyMarkup: InlineButtonBlockchain.GuarantorMeinMenu(user));

				String text = $"✅Подтвердить✅\nОтправитель: {transaction.UserSender.FIO}\nКомиссия: ";
				text += transaction.WhoCommissionPay == true ? "получатель" : "отправитель";
				text += $"\nСумма: {transaction.SumPayNew}\nВалюта: ";
				text += transaction.PaymentId == 1 ? "BTC" : transaction.PaymentId == 2 ? "USDT" : transaction.PaymentId == 3 ? "Ethereum" : transaction.PaymentId == 4 ? "Ripple" : "не выбрана!";

				botClient.SendText(userTwo.ID, text, userTwo, replyMarkup: InlineButtonBlockchain.ConfirmOrCancelThisTransactionUserTwo(transaction));
			}
		}

		public override System.Object Handle(System.Int32 request, TelegramBotClient botClient, System.Object message)
		{
			if (request == (System.Int32)SetChain.SetUserInTransaction)
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