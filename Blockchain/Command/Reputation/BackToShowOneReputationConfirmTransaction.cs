using System.Collections.Generic;

using BotCore.SQL;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCore.Blockchain
{
	internal class BackToShowOneReputationConfirmTransaction : Advertising.Command.AbsCommand, IStandartCommand, ITransactions
	{
		public override System.String Name { set; get; } = CommandTextBlockchain.BackToShowOneReputationConfirmTransaction;

		private DataBase db = null;
		private User user = null;
		private CallbackQuery _message = null;
		private List<Transaction> transactions = null;

		public override void Execute(TelegramBotClient botClient, System.Object message)
		{
			IStandartCommand standartCommand = new BackToShowOneReputationConfirmTransaction();
			ITransactions transactions = new BackToShowOneReputationConfirmTransaction();

			if (standartCommand.SetCallbackQuery(message, out _message)) return;

			if (standartCommand.SetDataBase(out db)) return;

			if (standartCommand.SetUserAndCheckIsNull(botClient, _message, out user, db)) return;

			if (transactions.SetTransaction(out this.transactions, db)) return;

			SendMessage(botClient);
		}

		public void SendMessage(TelegramBotClient botClient) => botClient.EditMessage(_message.From.Id, user.MessageID, "✅Успешная транзакция✅", "20 - BackToShowOneReputationConfirmTransaction", user, replyMarkup: InlineButtonBlockchain.ShowReputationConfirmTransaction(transactions));
	}
}