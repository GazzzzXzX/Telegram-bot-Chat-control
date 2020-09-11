using System.Collections.Generic;

using BotCore.SQL;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCore.Blockchain
{
	internal class BackToShowMyTransaction : Advertising.Command.AbsCommand, IStandartCommand, ITransactions
	{
		public override System.String Name { set; get; } = CommandTextBlockchain.BackToShowMyTransaction;

		private DataBase db = null;
		private User user = null;
		private CallbackQuery _message = null;
		private List<Transaction> transactions = null;

		public override void Execute(TelegramBotClient botClient, System.Object message)
		{
			IStandartCommand standartCommand = new BackToShowMyTransaction();
			ITransactions transaction = new BackToShowMyTransaction();

			if (standartCommand.SetCallbackQuery(message, out _message)) return;

			if (standartCommand.SetDataBase(out db)) return;

			if (standartCommand.SetUserAndCheckIsNull(botClient, _message, out user, db)) return;

			if (transaction.SetTransaction(out transactions, db)) return;
			Name = CommandTextBlockchain.BackToShowMyTransaction;

			SendMessage(botClient);
		}

		public void SendMessage(TelegramBotClient botClient) =>
			botClient.EditMessage(_message.From.Id, user.MessageID,
			                      "💼Мои транзакции💼\n[Имя транзакции✅] - транзакция которая должна подтвердится как успешная\n[Имя транзакции❌] - транзакция котораяя должна отменится\n[Имя транзакции] - транзакция которая находится в работе",
			                      "18 - ConfirmMyTransaction", user,
			                      replyMarkup: InlineButtonBlockchain.ShowMyTransaction(transactions, user));
	}
}