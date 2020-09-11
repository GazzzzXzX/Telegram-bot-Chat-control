using System;

using BotCore.SQL;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCore.Blockchain
{
	internal class ConfirmMyTransaction : Advertising.Command.AbsCommand, IStandartCommand, ITransaction, ISplitName
	{
		public override System.String Name { set; get; } = CommandTextBlockchain.ConfirmMyTransaction;

		private DataBase db = null;
		private User user = null;
		private CallbackQuery _message = null;
		private Transaction transaction = null;

		public override void Execute(TelegramBotClient botClient, System.Object message)
		{
			IStandartCommand standartCommand = new ConfirmMyTransaction();
			ITransaction transaction = new ConfirmMyTransaction();
			ISplitName splitName = new ConfirmMyTransaction();

			if (standartCommand.SetCallbackQuery(message, out _message)) return;

			if (standartCommand.SetDataBase(out db)) return;

			if (standartCommand.SetUserAndCheckIsNull(botClient, _message, out user, db)) return;

			Int32 IdTransaction = splitName.GetNameSplit(Name);
			Name = CommandTextBlockchain.ConfirmMyTransaction;

			if (transaction.GetTransaction(out this.transaction, IdTransaction, db)) return;
			if (this.transaction.TransactionSuspension == false)
			{
				SendMessage(botClient);
				return;
			}
			SendMessageTrue(botClient);
		}

		private void SendMessageTrue(TelegramBotClient botClient) => botClient.EditMessage(_message.From.Id, user.MessageID, "🔐Гарант🔐\nВзаимодействие с данной транзакцией пока не возможно!\nИдёт проверка администратора!", "18 - ConfirmMyTransaction", user, replyMarkup: InlineButtonBlockchain.GuarantorMeinMenu(user));

		public void SendMessage(TelegramBotClient botClient)
		{
			String text = $"💼Мои транзакции💼\nОтправитель: {transaction.UserSender.FIO}\nПолучитель: {transaction.UserRecipient.FIO}\nКомиссия: ";
			text += transaction.WhoCommissionPay == true ? "получатель" : "отправитель";
			text += $"\nСумма: {transaction.SumPayNew}\nВалюта: ";
			text += transaction.PaymentId == 1 ? "BTC" : transaction.PaymentId == 2 ? "USDT" : transaction.PaymentId == 3 ? "Ethereum" : transaction.PaymentId == 4 ? "Ripple" : "не выбрана!";
			text += "\n\nВсе условия соблюдены?";

			botClient.EditMessage(_message.From.Id, user.MessageID, text, "18 - ConfirmMyTransaction", user, replyMarkup: InlineButtonBlockchain.ConfirmThisTransaction(transaction));
		}
	}
}