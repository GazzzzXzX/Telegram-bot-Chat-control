using BotCore.SQL;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCore.Blockchain
{
	internal class TransactionCreationGuarantor : Advertising.Command.AbsCommand, IStandartCommand
	{
		public override System.String Name { set; get; } = CommandTextBlockchain.TransactionCreationGuarantor;

		private DataBase db = null;
		private User user = null;
		private CallbackQuery _message = null;
		private Transaction transaction = null;

		public override void Execute(TelegramBotClient botClient, System.Object message)
		{
			IStandartCommand standartCommand = new TransactionCreationGuarantor();

			if (standartCommand.SetCallbackQuery(message, out _message)) return;

			if (standartCommand.SetDataBase(out db)) return;

			if (standartCommand.SetUserAndCheckIsNull(botClient, _message, out user, db)) return;

			CreateTransaction();

			SendMessage(botClient);
		}

		private void CreateTransaction()
		{
			transaction = new Transaction() { Name = user.FIO, UserSender = user, UserSenderId = user.ID, IsConfirmOrCancelUserSender = 0, AddUser = true, SumPay = 0 };
			db.SetValue<Transaction>(transaction);
			//db.SetTransaction(new Transaction() { Name = user.FIO, UserSender = user, UserSenderId = user.ID, IsConfirmOrCancelUserSender = 0, AddUser = true, SumPay = 0 });
			db.Save();
		}

		public void SendMessage(TelegramBotClient botClient) => botClient.EditMessage(_message.From.Id, user.MessageID, "⚖Создание транзакции⚖", "38 - TransactionCreationGuarantor", user, replyMarkup: InlineButtonBlockchain.TransactionCreationGuarantor(transaction));
	}
}