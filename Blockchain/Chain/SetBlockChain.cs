using System;
using System.Globalization;
using BotCore.Blockchain;
using Telegram.Bot;
using Telegram.Bot.Types;
using Transaction = BotCore.SQL.Transaction;

namespace BotCore.Advertising
{
	internal class SetBlockChain : AbstractHandlerAdvertising
	{
		private Transaction transaction = null;
		private DataBase db = null;
		private User user;
		private Decimal SumPay = 0;

		private void SendMessage(TelegramBotClient botClient, Message _message)
		{
			botClient.DeleteMessage(_message.From.Id, _message.MessageId, "26 - SetBlockChain");

			String text = $"✅Подтвердить✅\nОтправитель: {transaction.UserSender.FIO}\nКомиссия: ";
			text += transaction.WhoCommissionPay == true ? "отправитель" : "получатель";
			text += transaction.WhoCommissionPay == true ? $"\nСумма: {transaction.SumPayNew}\nВалюта: " : $"\nСумма: {_message.Text}\nВалюта: ";
			text += transaction.PaymentId == 1 ? "BTC" : transaction.PaymentId == 2 ? "USDT" : transaction.PaymentId == 3 ? "Ethereum" : transaction.PaymentId == 4 ? "Ripple" : "не выбрана!";

			switch (transaction.PaymentId)
			{
				case 1:
					text += "\n\nВведите номер кошелька(public key), это нужно для того что бы мы понимали куда отправлять деньги в случаи неудачной сделки!";
					break;
				case 3:
					text += "__\nНомер кошелька для отправки денег:__" + $"```{BlockchainManager.Instance.Settings.ethWalletAddress}``` " + "__(Текст можно скопировать тапом)__";
					break;
				case 4:
					text += "__\nНомер кошелька для отправки денег: __" + $"```{BlockchainManager.Instance.Settings.xrpWalletAddress}``` " + "__(Текст можно скопировать тапом)__";
					break;
			}


			if (transaction.PaymentId == 1 && transaction.UserRecipient == null)
			{

				botClient.EditMessage(_message.From.Id, user.MessageID, text, "50 - SetBlockChain", user, replyMarkup: InlineButtonBlockchain.ChoosingPaymentMethodToBack(transaction), IsMarkdown: true);
			}
			else
			{
				text += "__\n\nВведите HASH транзакции, этот параметр обязателен, мы должны понимать что именно вы отправили деньги на наш кошелек!\nВ противном случаи услуги гаранта для вас будут недоступны!__";
				botClient.EditMessage(_message.From.Id, user.MessageID, text, "55 - SetBlockChain", user, replyMarkup: InlineButtonBlockchain.SetTransactionMenu(transaction), IsMarkdown: true);
			}
		}

		private void ChangeUserChanin()
		{
			if (transaction.PaymentId == 1 && transaction.UserRecipient == null)
			{
				user.Chain = (Int32)SetChain.SetPublicKeyUserTwo;
			}
			else
			{
				user.Chain = (Int32)SetChain.SetIdTransaction;
			}
			db.Save();
		}

		private Boolean SelectChangePay(TelegramBotClient botClient, Message _message)
		{
			String Currency;

			if (_message.Text.Split(" ").Length > 1)
			{
				Currency = _message.Text.Split(" ")[0].Replace("[", "").Replace("]", "");

				Decimal.TryParse(_message.Text.Split(" ")[1], NumberStyles.Any, CultureInfo.InvariantCulture, out SumPay);
				if (Currency == "UAH")
				{
					if (!CheckIdTransaction(botClient, _message, 1)) return false;
				}
				else if (Currency == "USD")
				{
					if (!CheckIdTransaction(botClient, _message, 2)) return false;
				}
			}
			else
			{
				Decimal.TryParse(_message.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out SumPay);
				if (!CheckIdTransaction(botClient, _message)) return false;
			}
			return true;
		}

		private Boolean ConvertInUAH(TelegramBotClient botClient, Message _message)
		{
			CryptoCourse.ExchangeInfo cours = null;

			switch (transaction.PaymentId)
			{
				case 1:
				{
					cours = CryptoCourse.GetExchangeInfo("UAH", "BTC");
					break;
				}
				case 3:
				{
					cours = CryptoCourse.GetExchangeInfo("UAH", "ETH");
					break;
				}
				case 4:
				{
					cours = CryptoCourse.GetExchangeInfo("UAH", "XRP");
					break;
				}
			}


			Decimal perOne = (Decimal) (1 / cours.Last);
			if (!CheckSumPay(1, perOne)) return false;

			SumPay *= perOne;
			return true;
		}

		private Boolean ConvertInUSD(TelegramBotClient botClient, Message _message)
		{
			CryptoCourse.ExchangeInfo cours = null;
			switch (transaction.PaymentId)
			{
				case 1:
				{
					cours = CryptoCourse.GetExchangeInfo("USD", "BTC");
					break;
				}
				case 3:
				{
					cours = CryptoCourse.GetExchangeInfo("USD", "ETH");
					break;
				}
				case 4:
				{
					cours = CryptoCourse.GetExchangeInfo("USD", "XRP");
					break;
				}
				default:
					SumPay = 0;
					return false;
			}

			Decimal perOne = (Decimal) (1 / cours.Last);
			if (!CheckSumPay(2, perOne)) return false;

			SumPay *= perOne;
			SumPay = Math.Round(SumPay, 6);
			return true;
		}

		private Boolean Convert(TelegramBotClient botClient, Message _message)
		{
			CryptoCourse.ExchangeInfo cours = null;
			switch (transaction.PaymentId)
			{
				case 1:
				{
					cours = CryptoCourse.GetExchangeInfo("USD", "BTC");
					break;
				}
				case 3:
				{
					cours = CryptoCourse.GetExchangeInfo("USD", "ETH");
					break;
				}
				case 4:
				{
					cours = CryptoCourse.GetExchangeInfo("USD", "XRP");
					break;
				}
				default:
					SumPay = 0;
					return false;
			}

			Decimal perOne = (Decimal) (1 / cours.Last);
			if (!CheckSumPay(0, perOne)) return false;

			return true;
		}

		private Boolean CheckIdTransaction(TelegramBotClient botClient, Message _message, Int32 convert = 0)
		{
			if (convert == 1)
			{
				if (!ConvertInUAH(botClient, _message))
				{
					SendTextFalse(botClient, _message);
					return false;
				}
			}
			else if (convert == 2)
			{
				if (!ConvertInUSD(botClient, _message))
				{
					SendTextFalse(botClient, _message);
					return false;
				}
			}
			else
			{
				if (!Convert(botClient, _message))
				{
					SendTextFalse(botClient, _message);
					return false;
				}
			}

			SetPayInDataBase();
			return true;
		}

		private void SendTextFalse(TelegramBotClient botClient, Message _message)
		{
			botClient.DeleteMessage(_message.From.Id, _message.MessageId, "26 - SetBlockChain");

			String text = "Вы ввели недостаточно денег!\nМинимальная сумма поплнения 3 доллара или 100 грн!";

			botClient.EditMessage(_message.From.Id, user.MessageID, text, "50 - SetBlockChain", user, replyMarkup: InlineButtonBlockchain.ChoosingPaymentMethod(transaction));
		}

		private Boolean CheckSumPay(Int32 convert = 0, Decimal perOne = 0)
		{
			if (convert == 1)
			{
				if (SumPay < 100)
				{
					return false;
				}
			}
			else if (convert == 2)
			{
				if (SumPay < 3)
				{
					return false;
				}
			}
			else
			{
				perOne *= 3;
				if (perOne > SumPay)
				{
					return false;
				}
			}
			return true;
		}

		private void SetPayInDataBase()
		{
			transaction.SumPayNew = SumPay;

			Decimal fee =  BlockchainManager.Instance.Settings.transactionPercentFee;
			Decimal totalFee = transaction.SumPayNew * (fee / 100.0m);
			if (transaction.WhoCommissionPay == true)
			{
				transaction.SumPayNew += totalFee;
			}
			else
			{
				if (transaction.PaymentId != 1)
					transaction.SumPayNew -= totalFee;
			}
			db.Save();
		}

		private void ChangeTransactionAndUser()
		{
			transaction.PaymentId = 0;
			transaction.SumPayNew = 0;
			user.Chain = (Int32)SetChain.MessageUserInBot;
			db.Save();
		}

		public override System.Object Handle(System.Int32 request, TelegramBotClient botClient, System.Object message)
		{
			if (request == (System.Int32)SetChain.SetPayTransaction)
			{
				Message _message = message as Message;
				db = Singleton.GetInstance().Context;

				user = db.GetUser(_message.From.Id);
				if (IsNullDataBase.IsNull(botClient, _message, user)) return null;

				transaction = db.GetTransaction(user);
				if (IsNullDataBase.IsNull(botClient, _message, transaction)) return null;

				if (!SelectChangePay(botClient, _message))
				{
					ChangeTransactionAndUser();
					return null;
				}

				ChangeUserChanin();

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