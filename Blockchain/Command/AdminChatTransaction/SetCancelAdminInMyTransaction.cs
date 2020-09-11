using System;

using BotCore.SQL;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCore.Blockchain
{
	internal class SetCancelAdminInMyTransaction : Advertising.Command.AbsCommand, IStandartCommand, ITransaction, ISplitName
	{
		public override System.String Name { set; get; } = CommandTextBlockchain.SetCancelAdminInMyTransaction;

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

			if (user.IsAdmin >= 2)
			{
				SetSettings();
				if (IsNullDataBase.IsNull(botClient, _message, settings)) return;

				Int32 IdTransaction = splitName.GetNameSplit(Name);
				Name = CommandTextBlockchain.SetCancelAdminInMyTransaction;

				if (transaction.GetTransaction(out this.transaction, IdTransaction, db)) return;

				SendMessage(botClient);

				DeleteMessage(botClient);
			}
		}

		private void DeleteMessage(TelegramBotClient botClient) => botClient.DeleteMessage(settings.ChannelAdmin, _message.Message.MessageId, "49 - SetConfirmAdminInBlockChain");

		public void SendMessage(TelegramBotClient botClient)
		{
			String text = $"🛎Вызов администратора🛎\nОтправитель: {transaction.UserSender.FIO}\nПолучитель: {transaction.UserRecipient.FIO}\nКомиссия: ";
			text += transaction.WhoCommissionPay == true ? "получатель" : "отправитель";
			text += $"\nСумма: {transaction.SumPayNew}\nВалюта: ";
			text += transaction.PaymentId == 1 ? "BTC" : transaction.PaymentId == 2 ? "USDT" : transaction.PaymentId == 3 ? "Ethereum" : transaction.PaymentId == 4 ? "Ripple" : "не выбрана!";
			text += transaction.DescriptionCancelSender == "" ? "" : $"\nОтмена отправителя: {transaction.DescriptionCancelSender}";
			text += transaction.DescriptionCancelRecipient == "" ? "" : $"\nОтмена получателя: {transaction.DescriptionCancelRecipient}";
			text += $"Администратор @{user.Username} отменил вашу заявку!";

			botClient.SendText(transaction.UserSender.ID, text);

			botClient.SendText(transaction.UserRecipient.ID, text);

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
			textAdmin += $"\n\nДанную заявку отменил администратор {user.FIO}";

			botClient.SendText(settings.ChannelAdmin, textAdmin);
		}

		public void SetSettings() => settings = db.GetSettings();
	}
}