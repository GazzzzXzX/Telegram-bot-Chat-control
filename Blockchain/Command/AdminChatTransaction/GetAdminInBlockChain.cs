using System;

using BotCore.SQL;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCore.Blockchain
{
	internal class GetAdminInBlockChain : Advertising.Command.AbsCommand, IStandartCommand, ITransaction, ISplitName
	{
		public override System.String Name { set; get; } = CommandTextBlockchain.GetAdminInBlockChain;

		private DataBase db = null;
		private User user = null;
		private CallbackQuery _message = null;
		private Transaction transaction = null;

		public override void Execute(TelegramBotClient botClient, System.Object message)
		{
			IStandartCommand standartCommand = new CancelMyTransaction();
			ITransaction transaction = new CancelMyTransaction();
			ISplitName splitName = new CancelMyTransaction();

			if (standartCommand.SetCallbackQuery(message, out _message)) return;

			if (standartCommand.SetDataBase(out db)) return;

			if (standartCommand.SetUserAndCheckIsNull(botClient, _message, out user, db)) return;

			Int32 IdTransaction = splitName.GetNameSplit(Name);
			Name = CommandTextBlockchain.GetAdminInBlockChain;

			if (transaction.GetTransaction(out this.transaction, IdTransaction, db)) return;

			SendMessage(botClient);
		}

		public void SendMessage(TelegramBotClient botClient)
		{
			String text = $"🛎Вызов администратора🛎\nОтправитель: {transaction.UserSender.FIO}\nПолучитель: {transaction.UserRecipient.FIO}\nКомиссия: ";
			text += transaction.WhoCommissionPay == true ? "получатель" : "отправитель";
			text += $"\nСумма: {transaction.SumPayNew}\nВалюта: ";
			text += transaction.PaymentId == 1 ? "BTC" : transaction.PaymentId == 2 ? "USDT" : transaction.PaymentId == 3 ? "Ethereum" : transaction.PaymentId == 4 ? "Ripple" : "не выбрана!";
			text += transaction.DescriptionCancelSender == "" ? "" : $"Отмена отправителя: {transaction.DescriptionCancelSender}";
			text += transaction.DescriptionCancelRecipient == "" ? "" : $"Отмена получателя: {transaction.DescriptionCancelRecipient}";
			text += "Вы уверены что хотите к данной транзакции привлечь администратора? Эта услуга платная и будет взыматься комиссия в размере 5% от суммы.";

			botClient.EditMessage(_message.From.Id, user.MessageID, text, "18 - ConfirmMyTransaction", user, replyMarkup: InlineButtonBlockchain.GetAdminInMyTransaction(transaction));
		}
	}
}