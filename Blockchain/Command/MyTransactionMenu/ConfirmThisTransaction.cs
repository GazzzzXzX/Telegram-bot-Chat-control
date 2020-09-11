using System;
using System.Collections.Generic;

using BotCore.SQL;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCore.Blockchain
{
	internal class ConfirmThisTransaction : Advertising.Command.AbsCommand, IStandartCommand, ITransactions, ITransaction, ISplitName
	{
		public override System.String Name { set; get; } = CommandTextBlockchain.ConfirmThisTransaction;

		private DataBase db = null;
		private User user = null;
		private CallbackQuery _message = null;
		private List<Transaction> transactions = null;
		private Transaction transaction = null;
		private AddresBTC addresBTC = null;

		public override void Execute(TelegramBotClient botClient, System.Object message)
		{
			IStandartCommand standartCommand = new ConfirmThisTransaction();
			ITransactions transactions = new ConfirmThisTransaction();
			ITransaction transaction = new ConfirmThisTransaction();
			ISplitName splitName = new ConfirmThisTransaction();

			if (standartCommand.SetCallbackQuery(message, out _message)) return;

			if (standartCommand.SetDataBase(out db)) return;

			if (standartCommand.SetUserAndCheckIsNull(botClient, _message, out user, db)) return;

			if (transactions.SetTransaction(out this.transactions, db)) return;

			Int32 IdTransaction = splitName.GetNameSplit(Name);
			Name = CommandTextBlockchain.ConfirmThisTransaction;

			if (transaction.GetTransaction(out this.transaction, IdTransaction, db)) return;

			if (this.transaction.TransactionSuspension == false)
			{
				ChangeTransaction();

				if (CheckTransaction())
				{
					ChangeTransactionInConfirm();
					PayRecipient();
					SendMessageConfirm(botClient);
					return;
				}

				SendMessage(botClient);
				return;
			}
			SendMessageTrue(botClient);
		}

		private void SendMessageTrue(TelegramBotClient botClient) => botClient.EditMessage(_message.From.Id, user.MessageID, "🔐Гарант🔐\nВзаимодействие с данной транзакцией пока не возможно!\nИдёт проверка администратора!", "18 - ConfirmMyTransaction", user, replyMarkup: InlineButtonBlockchain.GuarantorMeinMenu(user));

		public void SendMessage(TelegramBotClient botClient) =>
			botClient.EditMessage(_message.From.Id, user.MessageID,
			                      "💼Мои транзакции💼\n[Имя транзакции✅] - транзакция которая должна подтвердится как успешная\n[Имя транзакции❌] - транзакция котораяя должна отменится\n[Имя транзакции] - транзакция которая находится в работе",
			                      "18 - ConfirmThisTransaction", user,
			                      replyMarkup: InlineButtonBlockchain.ShowMyTransaction(transactions, user));

		private Boolean CheckTransaction()
		{
			if (transaction.IsConfirmOrCancelUserRecipient == 2 && transaction.IsConfirmOrCancelUserSender == 2)
			{
				return true;
			}
			return false;
		}

		private void SendMessageConfirm(TelegramBotClient botClient)
		{
			botClient.DeleteMessage(user.ID, user.MessageID, "78 - ConfirmThisTransaction");
			botClient.SendText(transaction.UserRecipient.ID, "Транзакции была успешно одобрена обеими участниками!\n<b>Деньги отправлены получателю!</b>", transaction.UserRecipient, replyMarkup: InlineButtonBlockchain.GuarantorMeinMenu(user));

			botClient.SendText(transaction.UserSender.ID, "Транзакции была успешно одобрена обеими участниками!\n<b>Деньги отправлены получателю!</b>", transaction.UserSender, replyMarkup: InlineButtonBlockchain.GuarantorMeinMenu(user));
		}

		private void ChangeTransactionInConfirm()
		{
			transaction.IsPaySenderOrRecipiend = true;
			db.Save();
		}

		private async void PayRecipient()
		{
			switch (transaction.PaymentId)
			{
				case 1:
					addresBTC = db.GetAddresBTCInt(transaction.AddresBTCId);
					await BlockchainManager.Instance.SendBTC(transaction.PublicKeyWallet, transaction.SumPayNew - (transaction.SumPayNew * (BlockchainManager.Instance.Settings.transactionPercentFee / 100)), (transaction.SumPayNew * (BlockchainManager.Instance.Settings.transactionPercentFee / 100)), addresBTC);
					break;
				case 3:
					await BlockchainManager.Instance.SendETH(transaction.PublicKeyWallet, transaction.SumPayNew);
					break;
				case 4:
					await BlockchainManager.Instance.SendRipple(transaction.PublicKeyWallet, transaction.SumPayNew);
					break;
			}

		}

		private void ChangeTransaction()
		{
			if (transaction.UserSenderId == user.ID)
			{
				transaction.IsConfirmOrCancelUserSender = 2;
			}
			else
			{
				transaction.IsConfirmOrCancelUserRecipient = 2;
			}
			db.Save();
		}
	}
}