using System;

using BotCore.SQL;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCore.Blockchain
{
	internal class AddUserInTransaction : Advertising.Command.AbsCommand, IStandartCommand, ITransaction, ISplitName
	{
		public override System.String Name { set; get; } = CommandTextBlockchain.AddUserInTransaction;

		private DataBase db = null;
		private User user = null;
		private CallbackQuery _message = null;
		private Transaction transaction = null;

		public override void Execute(TelegramBotClient botClient, System.Object message)
		{
			IStandartCommand standartCommand = new AddUserInTransaction();
			ITransaction transaction = new AddUserInTransaction();
			ISplitName splitName = new AddUserInTransaction();

			if (standartCommand.SetCallbackQuery(message, out _message)) return;

			if (standartCommand.SetDataBase(out db)) return;

			if (standartCommand.SetUserAndCheckIsNull(botClient, _message, out user, db)) return;

			Int32 IdTransaction = splitName.GetNameSplit(Name);
			Name = CommandTextBlockchain.AddUserInTransaction;

			if (transaction.GetTransaction(out this.transaction, IdTransaction, db)) return;

			ChangeUser();

			SendMessage(botClient);
		}

		public void SendMessage(TelegramBotClient botClient) => botClient.EditMessage(_message.From.Id, user.MessageID, "🛅Добавление участника🛅\nВведите ID или перешлите сообщение:", "17 - AddUserInTransaction", user, replyMarkup: InlineButtonBlockchain.AddUserInTransactionToBack(transaction));

		private void ChangeUser()
		{
			user.Chain = (Int32)BotCore.Advertising.SetChain.SetUserInTransaction;
			db.Save();
		}
	}
}