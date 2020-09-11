using System;

using BotCore.Advertising;
using BotCore.SQL;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCore.Blockchain
{
	internal class RipplePaymentMethod : Advertising.Command.AbsCommand, IStandartCommand, ITransaction, ISplitName
	{
		public override System.String Name { set; get; } = CommandTextBlockchain.RipplePaymentMethod;

		private DataBase db = null;
		private User user = null;
		private CallbackQuery _message = null;
		private Transaction transaction = null;

		public override void Execute(TelegramBotClient botClient, System.Object message)
		{
			IStandartCommand standartCommand = new RipplePaymentMethod();
			ISplitName splitName = new RipplePaymentMethod();
			ITransaction transaction = new RipplePaymentMethod();

			if (standartCommand.SetCallbackQuery(message, out _message)) return;

			if (standartCommand.SetDataBase(out db)) return;

			if (standartCommand.SetUserAndCheckIsNull(botClient, _message, out user, db)) return;

			Int32 idTransaction = splitName.GetNameSplit(Name);
			Name = CommandTextBlockchain.RipplePaymentMethod;

			if (transaction.GetTransaction(out this.transaction, idTransaction, db)) return;

			ChangeTransaction();
			ChangeUserChain();

			SendMessage(botClient);
		}

		public void SendMessage(TelegramBotClient botClient) => botClient.EditMessage(_message.From.Id, user.MessageID, "Ripple\nВведите сумму транзакции:\nВы можете ввести сумму в другой валюте, она будет автоматически сконвертирована:\n[USD] [Сумма] или [UAH] [Сумма]", "17 - AddUserInTransaction", user, replyMarkup: InlineButtonBlockchain.ChoosingPaymentMethodToBack(transaction));

		private void ChangeTransaction()
		{
			transaction.PaymentId = 4;
			db.Save();
		}

		private void ChangeUserChain()
		{
			transaction.PaymentId = 4;
			user.Chain = (Int32)SetChain.SetPayTransaction;
			db.Save();
		}
	}
}