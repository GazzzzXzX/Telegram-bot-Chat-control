using System;

using BotCore.SQL;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCore.Blockchain
{
	internal class CommissionPaymentRecipient : Advertising.Command.AbsCommand, IStandartCommand, ITransaction, ISplitName
	{
		public override System.String Name { set; get; } = CommandTextBlockchain.CommissionPaymentRecipient;

		private DataBase db = null;
		private User user = null;
		private CallbackQuery _message = null;
		private Transaction transaction = null;

		public override void Execute(TelegramBotClient botClient, System.Object message)
		{
			IStandartCommand standartCommand = new CommissionPaymentRecipient();
			ISplitName splitName = new CommissionPaymentRecipient();
			ITransaction transaction = new CommissionPaymentRecipient();

			if (standartCommand.SetCallbackQuery(message, out _message)) return;

			if (standartCommand.SetDataBase(out db)) return;

			if (standartCommand.SetUserAndCheckIsNull(botClient, _message, out user, db)) return;

			Int32 idTransaction = splitName.GetNameSplit(Name);
			Name = CommandTextBlockchain.CommissionPaymentRecipient;

			if (transaction.GetTransaction(out this.transaction, idTransaction, db)) return;

			ChangeTransaction();

			SendMessage(botClient);
		}

		public void SendMessage(TelegramBotClient botClient) => botClient.EditMessage(_message.From.Id, user.MessageID, "⚖Создание транзакции⚖\nКомиссия будет оплачиватся получателем.", "17 - AddUserInTransaction", user, replyMarkup: InlineButtonBlockchain.TransactionCreationGuarantor(transaction));

		private void ChangeTransaction()
		{
			transaction.WhoCommissionPay = false;
			db.Save();
		}
	}
}