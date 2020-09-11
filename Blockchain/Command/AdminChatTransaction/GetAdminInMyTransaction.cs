using System;

using BotCore.SQL;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCore.Blockchain
{
	internal class GetAdminInMyTransaction : Advertising.Command.AbsCommand, IStandartCommand, ITransaction, ISplitName
	{
		public override System.String Name { set; get; } = CommandTextBlockchain.GetAdminInMyTransaction;

		private DataBase db = null;
		private User user = null;
		private CallbackQuery _message = null;
		private Transaction transaction = null;
		private Settings settings = null;

		public override void Execute(TelegramBotClient botClient, System.Object message)
		{
			IStandartCommand standartCommand = new CancelMyTransaction();
			ITransaction transaction = new CancelMyTransaction();
			ISplitName splitName = new CancelMyTransaction();

			if (standartCommand.SetCallbackQuery(message, out _message)) return;

			if (standartCommand.SetDataBase(out db)) return;

			if (standartCommand.SetUserAndCheckIsNull(botClient, _message, out user, db)) return;

			settings = db.GetSettings();

			if (IsNullDataBase.IsNull(botClient, _message, settings)) return;

			Int32 IdTransaction = splitName.GetNameSplit(Name);
			Name = CommandTextBlockchain.GetAdminInMyTransaction;

			if (transaction.GetTransaction(out this.transaction, IdTransaction, db)) return;

			SendMessage(botClient);
		}

		public void SendMessage(TelegramBotClient botClient)
		{
			String text = $"🛎Вызов администратора🛎\nОтправитель: {transaction.UserSender.FIO}\nПолучитель: {transaction.UserRecipient.FIO}\nКомиссия: ";
			text += transaction.WhoCommissionPay == true ? "получатель" : "отправитель";
			text += $"\nСумма: {transaction.SumPayNew}\nВалюта: ";
			text += transaction.PaymentId == 1 ? "BTC" : transaction.PaymentId == 2 ? "USDT" : transaction.PaymentId == 3 ? "Ethereum" : transaction.PaymentId == 4 ? "Ripple" : "не выбрана!";
			text += transaction.DescriptionCancelSender == "" ? "" : $"\nОтмена отправителя: {transaction.DescriptionCancelSender}";
			text += transaction.DescriptionCancelRecipient == "" ? "" : $"\nОтмена получателя: {transaction.DescriptionCancelRecipient}";
			text += "Сообщение администраторам было отправлено, ожидайте пока с вами свяжутся!";

			botClient.EditMessage(_message.From.Id, user.MessageID, text, "18 - ConfirmMyTransaction", user, replyMarkup: InlineButtonBlockchain.SelectConfirmOrCancelThisTransaction(transaction));

			String textAdmin = $"🛎Вызов администратора🛎\nОтправитель: {transaction.UserSender.FIO}";
			textAdmin += transaction.UserSender.Username == "\nНет!" ? "" : $"\nНикнейм: @{transaction.UserSender.Username}";
			textAdmin += $"\nНомер телефона: {transaction.UserSender.Number}";
			textAdmin += $"\nПолучитель: {transaction.UserRecipient.FIO}";
			textAdmin += transaction.UserRecipient.Username == "\nНет!" ? "" : $"\nНикнейм: @{transaction.UserRecipient.Username}";
			textAdmin += $"\nНомер телефона: {transaction.UserRecipient.Number}";
			textAdmin += "\nКомиссия: ";
			textAdmin += transaction.WhoCommissionPay == true ? "получатель" : "отправитель";
			textAdmin += $"\nСумма: {transaction.SumPayNew}\nВалюта: ";
			textAdmin += transaction.PaymentId == 1 ? "BTC" : transaction.PaymentId == 2 ? "USDT" : transaction.PaymentId == 3 ? "Ethereum" : transaction.PaymentId == 4 ? "Ripple" : "не выбрана!";
			textAdmin += transaction.DescriptionCancelSender == "" ? "" : $"\nОтмена отправителя: {transaction.DescriptionCancelSender}";
			textAdmin += transaction.DescriptionCancelRecipient == "" ? "" : $"\nОтмена получателя: {transaction.DescriptionCancelRecipient}";

			botClient.SendText(settings.ChannelAdmin, textAdmin, replyMarkup: InlineButtonBlockchain.SetAdminInTransaction(transaction));
		}
	}
}