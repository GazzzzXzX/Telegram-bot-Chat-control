using System;

using BotCore.SQL;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCore.Blockchain
{
	internal class CommissionPaymentSender : Advertising.Command.AbsCommand, IStandartCommand, ITransaction, ISplitName
	{
		public override System.String Name { set; get; } = CommandTextBlockchain.CommissionPaymentSender;

		private DataBase db = null;
		private User user = null;
		private CallbackQuery _message = null;
		private Transaction transaction = null;

		public override void Execute(TelegramBotClient botClient, System.Object message)
		{
			IStandartCommand standartCommand = new CommissionPaymentSender();
			ISplitName splitName = new CommissionPaymentSender();
			ITransaction transaction = new CommissionPaymentSender();

			if (standartCommand.SetCallbackQuery(message, out _message)) return;

			if (standartCommand.SetDataBase(out db)) return;

			if (standartCommand.SetUserAndCheckIsNull(botClient, _message, out user, db)) return;

			Int32 idTransaction = splitName.GetNameSplit(Name);
			Name = CommandTextBlockchain.CommissionPaymentSender;

			if (transaction.GetTransaction(out this.transaction, idTransaction, db)) return;

			ChangeTransaction();

			SendMessage(botClient);
		}

		public void SendMessage(TelegramBotClient botClient) => botClient.EditMessage(_message.From.Id, user.MessageID, "⚖Создание транзакции⚖\nКомиссия будет оплачиватся отправителем.", "17 - AddUserInTransaction", user, replyMarkup: InlineButtonBlockchain.TransactionCreationGuarantor(transaction));

		private void ChangeTransaction()
		{
			transaction.WhoCommissionPay = true;
			db.Save();
		}
	}
}