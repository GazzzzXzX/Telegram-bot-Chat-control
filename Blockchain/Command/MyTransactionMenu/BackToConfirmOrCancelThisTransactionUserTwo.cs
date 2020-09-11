using System;
using BotCore.Advertising;
using BotCore.SQL;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCore.Blockchain
{
	internal class BackToConfirmOrCancelThisTransactionUserTwo : Advertising.Command.AbsCommand, IStandartCommand, ITransaction, ISplitName
	{
		public override System.String Name { set; get; } = CommandTextBlockchain.BackToConfirmOrCancelThisTransactionUserTwo;

		private DataBase db = null;
		private User user = null;
		private CallbackQuery _message = null;
		private Transaction transaction = null;

		public override void Execute(TelegramBotClient botClient, System.Object message)
		{
			IStandartCommand standartCommand = new BackToConfirmOrCancelThisTransactionUserTwo();
			ITransaction transaction = new BackToConfirmOrCancelThisTransactionUserTwo();
			ISplitName splitName = new BackToConfirmOrCancelThisTransactionUserTwo();

			if (standartCommand.SetCallbackQuery(message, out _message)) return;

			if (standartCommand.SetDataBase(out db)) return;

			if (standartCommand.SetUserAndCheckIsNull(botClient, _message, out user, db)) return;

			Int32 IdTransaction = splitName.GetNameSplit(Name);
			Name = CommandTextBlockchain.BackToConfirmOrCancelThisTransactionUserTwo;

			if (transaction.GetTransaction(out this.transaction, IdTransaction, db)) return;

			ChangeUser();

			SendMessage(botClient);
		}

		public void SendMessage(TelegramBotClient botClient)
		{
			String text = $"Отправитель: {transaction.UserSender.FIO}\nВалюта: ";
			text += transaction.PaymentId == 1 ? "BTC" : transaction.PaymentId == 2 ? "USDT" : transaction.PaymentId == 3 ? "Ethereum" : transaction.PaymentId == 4 ? "Ripple" : "не выбрана!";
			botClient.EditMessage(_message.From.Id, user.MessageID, $"💼Мои транзакции💼\nВведите номер кошелька <b>{text}</b> (public key):\n<b>Это важно так как мы должны знать на какой кошелек отправлять деньги!</b>", "18 - ConfirmMyTransaction", user, replyMarkup: InlineButtonBlockchain.ConfirmOrCancelThisTransactionUserTwo(transaction));
			//botClient.SendText(transaction.UserSenderId, $"Транзакция была подтверждена {user.FIO}");
		}

		private void ChangeUser()
		{
			user.Chain = (Int32)SetChain.SetPublicKeyUserTwo;
			db.Save();
		}

	}
}
