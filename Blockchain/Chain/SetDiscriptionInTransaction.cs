using System;

using BotCore.Blockchain;
using BotCore.SQL;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCore.Advertising
{
	internal class SetDiscriptionInTransaction : AbstractHandlerAdvertising
	{
		private Transaction transaction = null;
		private DataBase db = null;
		private User user;
		private AddresBTC addresBTC = null;

		private void SendMessage(TelegramBotClient botClient, Message _message, Int32 request)
		{
			botClient.DeleteMessage(_message.From.Id, _message.MessageId, "48 - SetBlockChain");

			String text = $"❌Отменить❌\nОтправитель: {transaction.UserSender.FIO}\nПлучатель: {transaction.UserRecipient.FIO}\nКомиссия: ";
			text += transaction.WhoCommissionPay == true ? "получатель" : "отправитель";
			text += $"\nСумма: {transaction.SumPayNew}\nВалюта: ";
			text += transaction.PaymentId == 1 ? "BTC" : transaction.PaymentId == 2 ? "USDT" : transaction.PaymentId == 3 ? "Ethereum" : transaction.PaymentId == 4 ? "Ripple" : "не выбрана!";
			text += request == (Int32)SetChain.SetTransactionDiscription ? $"\nПричина отправителя: {transaction.DescriptionCancelSender}" : $"\nПричина получателя: {transaction.DescriptionCancelRecipient}";

			botClient.EditMessage(_message.From.Id, user.MessageID, text, "55 - SetBlockChain", user, replyMarkup: InlineButtonBlockchain.SelectConfirmOrCancelThisTransaction(transaction));

			if (request == (Int32)SetChain.SetTransactionDiscription)
			{
				botClient.SendText(transaction.UserRecipient.ID, $"Транзакция {transaction.Name} была отменена пользователем {user.FIO}.\nПричина {transaction.DescriptionCancelSender}");
			}
			else
			{
				botClient.SendText(transaction.UserSender.ID, $"Транзакция {transaction.Name} была отменена пользователем {user.FIO}.\nПричина {transaction.DescriptionCancelRecipient}");
			}
		}

		private void ChangeUserChanin()
		{
			user.Chain = (Int32)SetChain.MessageUserInBot;
			db.Save();
		}

		private void ChangeTransaction(Message _message, Int32 request)
		{
			if (request == (Int32)SetChain.SetTransactionDiscription)
			{
				transaction.DescriptionCancelSender = _message.Text;
				transaction.IsConfirmOrCancelUserSender = 1;
			}
			else if (request == (Int32)SetChain.SetTransactionDiscriptionUSerTwo)
			{
				transaction.DescriptionCancelRecipient = _message.Text;
				transaction.IsConfirmOrCancelUserRecipient = 1;
			}

			transaction.AddUser = false;
			db.Save();
		}

		private async void PaySender(TelegramBotClient botClient, Message _message)
		{
			if (transaction.IsConfirmOrCancelUserRecipient == 1 && transaction.IsConfirmOrCancelUserSender == 1)
			{
				switch (transaction.PaymentId)
				{
					case 1:
						addresBTC = db.GetAddresBTCInt(transaction.AddresBTCId);//dec - (dec * percent)

						await BlockchainManager.Instance.SendBTC(transaction.PublicKeyWallet, transaction.SumPayNew - (transaction.SumPayNew * (BlockchainManager.Instance.Settings.transactionPercentFee / 100)), (transaction.SumPayNew * (BlockchainManager.Instance.Settings.transactionPercentFee / 100)), addresBTC);
						break;
					case 3:
						await BlockchainManager.Instance.SendETH(transaction.PublicKeyWallet, transaction.SumPayNew);
						break;
					case 4:
						await BlockchainManager.Instance.SendRipple(transaction.PublicKeyWallet, transaction.SumPayNew);
						break;
				}

				String text = "Деньги были отправлены отправителю!";

				botClient.SendText(transaction.UserRecipient.ID, text);
				botClient.SendText(transaction.UserSender.ID, text);
				ChangeTransactionInPay();
			}
		}

		private void ChangeTransactionInPay()
		{
			transaction.IsPaySenderOrRecipiend = true;
			db.Save();
		}

		public override System.Object Handle(System.Int32 request, TelegramBotClient botClient, System.Object message)
		{
			if (request == (System.Int32)SetChain.SetTransactionDiscription || request == (System.Int32)SetChain.SetTransactionDiscriptionUSerTwo)
			{
				Message _message = message as Message;
				db = Singleton.GetInstance().Context;

				user = db.GetUser(_message.From.Id);
				if (IsNullDataBase.IsNull(botClient, _message, user)) return null;

				if (request == (System.Int32)SetChain.SetTransactionDiscription)
				{
					transaction = db.GetTransaction(user);
				}
				else
				{
					transaction = db.GetTransactionUserRecipient(user);
				}
				if (IsNullDataBase.IsNull(botClient, _message, transaction)) return null;

				ChangeTransaction(_message, request);

				ChangeUserChanin();

				SendMessage(botClient, _message, request);

				PaySender(botClient, _message);

				return null;
			}
			else
			{
				return base.Handle(request, botClient, message);
			}
		}
	}
}