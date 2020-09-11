using System;

using BotCore.Blockchain;
using BotCore.SQL;
using NBitcoin;
using Telegram.Bot;
using Telegram.Bot.Types;
using Transaction = BotCore.SQL.Transaction;

namespace BotCore.Advertising
{
	internal class SetPublicKeyUserTwo : AbstractHandlerAdvertising
	{
		private Transaction transaction = null;
		private DataBase db = null;
		private User user;
		private Message _message = null;
		private AddresBTC addresBTC = null;
		private void ChangeUserChanin()
		{
			user.Chain = (Int32)SetChain.MessageUserInBot;
			db.Save();
		}

		private void ChangeTransaction()
		{
			if (transaction.PaymentId == 1 && transaction.UserRecipient == null)
			{
				transaction.PublicKeyWalletSender = _message.Text;
			}
			else
			{
				transaction.IsConfirmOrCancelUserSender = 0;
				transaction.IsConfirmOrCancelUserRecipient = 0;
				transaction.AddUser = false;
				transaction.PublicKeyWallet = _message.Text;
			}
			db.Save();
		}
		private void GenerateKey()
		{
			BitcoinSecret temp = BlockchainManager.Instance.GenerateBTCKey();
			addresBTC = new AddresBTC
			{
				PrivateKey = temp.ToString(),
				PublickKey = temp.PubKey.GetAddress(Network.Main).ToString()
			};
			db.SetValue<AddresBTC>(new AddresBTC() { PrivateKey = temp.ToString(), PublickKey = temp.GetAddress().ToString() });
			db.Save();


		}
		private void SendMessage(TelegramBotClient botClient, Message _message)
		{
			botClient.DeleteMessage(_message.From.Id, _message.MessageId, "48 - SetBlockChain");

			String text = $"Отправитель: {transaction.UserSender.FIO}";
			text += transaction.UserRecipient != null ? $"\nПолучатель: {transaction.UserRecipient.FIO}" : "";
			text += "\nКомиссия: ";
			text += transaction.WhoCommissionPay == true ? "получатель" : "отправитель";
			text += $"\nСумма: {transaction.SumPayNew}\nВалюта: ";
			text += transaction.PaymentId == 1 ? "BTC" : transaction.PaymentId == 2 ? "USDT" : transaction.PaymentId == 3 ? "Ethereum" : transaction.PaymentId == 4 ? "Ripple" : "не выбрана!";

			if (transaction.PaymentId == 1 && transaction.UserRecipient == null)
			{
				GenerateKey();

				AddresBTC addres = db.GetAddresBTC(addresBTC.PrivateKey);

				transaction.AddresBTCId = addres.Id;
				user.Chain = (Int32)SetChain.SetIdTransaction;
				db.Save();
				text += "__\nНомер кошелька для отправки денег:__ " + "```" + addresBTC.PublickKey + "``` " + "(Текст можно скопировать тапом)";
				text += "__\n\nВведите HASH транзакции, этот параметр обязателен, мы должны понимать что именно вы отправили деньги на наш кошелек!\nВ противном случаи услуги гаранта для вас будут недоступны!__";
				botClient.SendText(_message.From.Id, text, user, replyMarkup: InlineButtonBlockchain.SetTransactionMenu(transaction), IsMarkdown: true);
			}
			else
			{
				botClient.EditMessage(_message.From.Id, user.MessageID, text, "55 - SetBlockChain", user, replyMarkup: InlineButtonBlockchain.SelectConfirmOrCancelThisTransaction(transaction));
				botClient.SendText(transaction.UserSenderId, $"Транзакция была подтверждена {user.FIO}");
			}
		}

		//TODO:	доделать провкрку.
		private void SetPublicKey()
		{

		}

		public override System.Object Handle(System.Int32 request, TelegramBotClient botClient, System.Object message)
		{
			if (request == (System.Int32)SetChain.SetPublicKeyUserTwo)
			{
				_message = message as Message;
				db = Singleton.GetInstance().Context;

				user = db.GetUser(_message.From.Id);
				if (IsNullDataBase.IsNull(botClient, _message, user)) return null;

				transaction = db.GetTransactionUserRecipient(user);
				if (IsNullDataBase.IsNull(botClient, _message, transaction))
				{
					transaction = db.GetTransactionUserSender(user);
					if (IsNullDataBase.IsNull(botClient, _message, transaction)) return null;
				}


				SetPublicKey();

				ChangeUserChanin();
				ChangeTransaction();

				SendMessage(botClient, _message);

				return null;
			}
			else
			{
				return base.Handle(request, botClient, message);
			}
		}
	}
}
