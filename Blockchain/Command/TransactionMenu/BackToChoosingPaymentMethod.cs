using System;

using BotCore.SQL;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCore.Blockchain
{
	internal class BackToChoosingPaymentMethod : Advertising.Command.AbsCommand, IStandartCommand, ITransaction, ISplitName
	{
		public override System.String Name { set; get; } = CommandTextBlockchain.BackToChoosingPaymentMethod;

		private DataBase db = null;
		private User user = null;
		private CallbackQuery _message = null;
		private Transaction transaction = null;

		public override void Execute(TelegramBotClient botClient, System.Object message)
		{
			IStandartCommand standartCommand = new BackToChoosingPaymentMethod();
			ITransaction transaction = new BackToChoosingPaymentMethod();
			ISplitName splitName = new BackToChoosingPaymentMethod();

			if (standartCommand.SetCallbackQuery(message, out _message)) return;

			if (standartCommand.SetDataBase(out db)) return;

			if (standartCommand.SetUserAndCheckIsNull(botClient, _message, out user, db)) return;

			Int32 IdTransaction = splitName.GetNameSplit(Name);
			Name = CommandTextBlockchain.BackToChoosingPaymentMethod;

			if (transaction.GetTransaction(out this.transaction, IdTransaction, db)) return;

			ChangeTransaction();

			SendMessage(botClient);
		}

		public void SendMessage(TelegramBotClient botClient) => botClient.EditMessage(_message.From.Id, user.MessageID, "💰Выбор способа оплаты💰", "17 - AddUserInTransaction", user, replyMarkup: InlineButtonBlockchain.ChoosingPaymentMethod(transaction));

		private void ChangeTransaction()
		{
			transaction.SumPayNew = 0;
			transaction.PaymentId = 0;
			db.Save();
		}
	}
}