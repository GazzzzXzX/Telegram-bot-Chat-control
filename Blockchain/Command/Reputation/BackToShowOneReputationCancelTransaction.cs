using System.Collections.Generic;

using BotCore.SQL;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCore.Blockchain
{
	internal class BackToShowOneReputationCancelTransaction : Advertising.Command.AbsCommand, IStandartCommand, ITransactions
	{
		public override System.String Name { set; get; } = CommandTextBlockchain.BackToShowOneReputationCancelTransaction;

		private DataBase db = null;
		private User user = null;
		private CallbackQuery _message = null;
		private List<Transaction> transactions = null;

		public override void Execute(TelegramBotClient botClient, System.Object message)
		{
			IStandartCommand standartCommand = new BackToShowOneReputationCancelTransaction();
			ITransactions transactions = new BackToShowOneReputationCancelTransaction();

			if (standartCommand.SetCallbackQuery(message, out _message)) return;

			if (standartCommand.SetDataBase(out db)) return;

			if (standartCommand.SetUserAndCheckIsNull(botClient, _message, out user, db)) return;

			if (transactions.SetTransaction(out this.transactions, db)) return;

			SendMessage(botClient);
		}

		public void SendMessage(TelegramBotClient botClient) => botClient.EditMessage(_message.From.Id, user.MessageID, "❌Отмененные транзакции❌", "20 - BackToShowOneReputationConfirmTransaction", user, replyMarkup: InlineButtonBlockchain.ShowReputationCancelTransaction(transactions));
	}
}