using System;

using BotCore.SQL;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCore.Blockchain
{
	internal class CommissionPayment : Advertising.Command.AbsCommand, IStandartCommand, ITransaction, ISplitName
	{
		public override System.String Name { set; get; } = CommandTextBlockchain.CommissionPayment;

		private DataBase db = null;
		private User user = null;
		private CallbackQuery _message = null;
		private Transaction transaction = null;

		public override void Execute(TelegramBotClient botClient, System.Object message)
		{
			IStandartCommand standartCommand = new CommissionPayment();
			ISplitName splitName = new CommissionPayment();
			ITransaction transaction = new CommissionPayment();

			if (standartCommand.SetCallbackQuery(message, out _message)) return;

			if (standartCommand.SetDataBase(out db)) return;

			if (standartCommand.SetUserAndCheckIsNull(botClient, _message, out user, db)) return;

			Int32 idTransaction = splitName.GetNameSplit(Name);
			Name = CommandTextBlockchain.CommissionPayment;

			if (transaction.GetTransaction(out this.transaction, idTransaction, db)) return;

			SendMessage(botClient);
		}

		public void SendMessage(TelegramBotClient botClient) => botClient.EditMessage(_message.From.Id, user.MessageID, "💸Оплата комиссии💸\nКто оплачивает комиссию?", "17 - AddUserInTransaction", user, replyMarkup: InlineButtonBlockchain.CommissionPayment(transaction));
	}
}