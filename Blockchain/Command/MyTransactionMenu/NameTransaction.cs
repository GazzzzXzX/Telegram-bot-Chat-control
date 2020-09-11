using System;

using BotCore.SQL;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCore.Blockchain
{
	internal class NameTransaction : Advertising.Command.AbsCommand, IStandartCommand, ISplitName
	{
		public override System.String Name { set; get; } = CommandTextBlockchain.NameTransaction;

		private DataBase db = null;
		private User user = null;
		private CallbackQuery _message = null;
		private Transaction transaction = null;

		public override void Execute(TelegramBotClient botClient, System.Object message)
		{
			IStandartCommand standartCommand = new NameTransaction();
			ISplitName splitName = new NameTransaction();

			if (standartCommand.SetCallbackQuery(message, out _message)) return;

			if (standartCommand.SetDataBase(out db)) return;

			if (standartCommand.SetUserAndCheckIsNull(botClient, _message, out user, db)) return;

			Int32 IdTransaction = splitName.GetNameSplit(Name);
			Name = CommandTextBlockchain.NameTransaction;

			if (GetTransaction(IdTransaction)) return;

			SendMessage(botClient);
		}

		private void SendMessageTrue(TelegramBotClient botClient) => botClient.EditMessage(_message.From.Id, user.MessageID, "🔐Гарант🔐\nВзаимодействие с данной транзакцией пока не возможно!\nИдёт проверка администратора!", "18 - ConfirmMyTransaction", user, replyMarkup: InlineButtonBlockchain.GuarantorMeinMenu(user));

		private Boolean GetTransaction(Int32 IdTransaction)
		{
			transaction = db.GetTransaction(IdTransaction);
			return transaction == null;
		}

		public void SendMessage(TelegramBotClient botClient)
		{
			String temp = "Нет!";
			if (transaction.UserRecipient != null)
			{
				temp = transaction.UserRecipient.FIO;
			}
			String text = $"💼Мои транзакции💼\nОтправитель: {transaction.UserSender.FIO}\nПолучитель: {temp}\nКомиссия: ";
			text += transaction.WhoCommissionPay == true ? "получатель" : "отправитель";
			text += $"\nСумма: {transaction.SumPayNew}\nВалюта: ";
			text += transaction.PaymentId == 1 ? "BTC" : transaction.PaymentId == 2 ? "USDT" : transaction.PaymentId == 3 ? "Ethereum" : transaction.PaymentId == 4 ? "Ripple" : "не выбрана!";
			text += "\n\nВсе условия соблюдены?";

			botClient.EditMessage(_message.From.Id, user.MessageID, text, "17 - AddUserInTransaction", user, replyMarkup: InlineButtonBlockchain.SelectConfirmOrCancelThisTransaction(transaction));
		}
	}
}