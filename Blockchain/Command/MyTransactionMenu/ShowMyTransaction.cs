using System.Collections.Generic;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCore.Blockchain
{
	internal class ShowMyTransaction : Advertising.Command.AbsCommand, IStandartCommand, ITransactions
	{
		public override System.String Name { set; get; } = CommandTextBlockchain.ShowMyTransaction;

		private DataBase db = null;
		private User user = null;
		private CallbackQuery _message = null;
		private List<BotCore.SQL.Transaction> transactions = null;

		public override void Execute(TelegramBotClient botClient, System.Object message)
		{
			IStandartCommand standartCommand = new ShowMyTransaction();
			ITransactions transactions = new ShowMyTransaction();

			if (standartCommand.SetCallbackQuery(message, out _message)) return;

			if (standartCommand.SetDataBase(out db)) return;

			if (standartCommand.SetUserAndCheckIsNull(botClient, _message, out user, db)) return;

			if (transactions.SetTransaction(out this.transactions, db)) return;
			Name = CommandTextBlockchain.ShowMyTransaction;

			SendMessage(botClient);
		}

		public void SendMessage(TelegramBotClient botClient) =>
			botClient.EditMessage(_message.From.Id, user.MessageID,
			                      "💼Мои транзакции💼\n[Имя транзакции✅] - транзакция которая должна подтвердится как успешная\n[Имя транзакции❌] - транзакция котораяя должна отменится\n[Имя транзакции] - транзакция которая находится в работе",
			                      "17 - AddUserInTransaction", user,
			                      replyMarkup: InlineButtonBlockchain.ShowMyTransaction(transactions, user));
	}
}