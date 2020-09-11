using System;

using BotCore.SQL;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCore.Blockchain
{
	internal class CancelThisTransactionUserTwo : Advertising.Command.AbsCommand, IStandartCommand, ITransaction, ISplitName
	{
		public override System.String Name { set; get; } = CommandTextBlockchain.CancelThisTransactionUserTwo;

		private DataBase db = null;
		private User user = null;
		private CallbackQuery _message = null;
		private Transaction transaction = null;

		public override void Execute(TelegramBotClient botClient, System.Object message)
		{
			IStandartCommand standartCommand = new CancelThisTransactionUserTwo();
			ITransaction transaction = new CancelThisTransactionUserTwo();
			ISplitName splitName = new CancelThisTransactionUserTwo();

			if (standartCommand.SetCallbackQuery(message, out _message)) return;

			if (standartCommand.SetDataBase(out db)) return;

			if (standartCommand.SetUserAndCheckIsNull(botClient, _message, out user, db)) return;

			Int32 IdTransaction = splitName.GetNameSplit(Name);
			Name = CommandTextBlockchain.CancelThisTransactionUserTwo;

			if (transaction.GetTransaction(out this.transaction, IdTransaction, db)) return;

			SendMessage(botClient);

			DeleteTransaction();
		}

		public void SendMessage(TelegramBotClient botClient)
		{
			botClient.EditMessage(_message.From.Id, user.MessageID, "🔐Гарант🔐", "18 - ConfirmMyTransaction", user, replyMarkup: InlineButtonBlockchain.GuarantorMeinMenu(user));

			botClient.SendText(transaction.UserSenderId, $"Транзакция была отменена пользоватлем: {user.FIO}");
		}

		private void DeleteTransaction()
		{
			db.DeleteTransaction(transaction);
			db.Save();
		}
	}
}