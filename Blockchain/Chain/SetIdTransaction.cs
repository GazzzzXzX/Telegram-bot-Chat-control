using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BotCore.Blockchain;
using BotCore.SQL;
using BotCore.TelegramClient;
using NBitcoin;
using Telegram.Bot;
using Telegram.Bot.Types;
using Transaction = BotCore.SQL.Transaction;

namespace BotCore.Advertising
{
	internal class SetIdTransaction : AbstractHandlerAdvertising
	{
		private Transaction transaction = null;
		private DataBase db = null;
		private User user;
		private Message _message = null;
		private AddresBTC addresBTC = null;

		private void SendMessage(TelegramBotClient botClient, Message _message)
		{
			botClient.DeleteMessage(_message.From.Id, _message.MessageId, "23 - SetIdTransaction");
			String text = $"Отправитель: {transaction.UserSender.FIO}\nКомиссия: ";
			text += transaction.WhoCommissionPay == true ? "получатель" : "отправитель";
			text += $"\nСумма: {transaction.SumPayNew}\nВалюта: ";
			text += transaction.PaymentId == 1 ? "BTC" : transaction.PaymentId == 2 ? "USDT" : transaction.PaymentId == 3 ? "Ethereum" : transaction.PaymentId == 4 ? "Ripple" : "не выбрана!";

			botClient.EditMessage(_message.From.Id, user.MessageID, text, "29 - SetIdTransaction", user, replyMarkup: InlineButtonBlockchain.TransactionCreationGuarantor(transaction));
		}

		private void ChangeUserChanin()
		{
			user.Chain = (Int32)SetChain.MessageUserInBot;
			db.Save();
		}

		private async Task<Boolean> CheckIdTransaction(TelegramBotClient botClient)
		{
			Transaction found = db.GetTransactionByHash(_message.Text);
			TransactionId errTran = db.GetFailedTransactionByHash(_message.Text);

			Boolean isAlreadyUsed = (found != null || errTran != null);

			if (isAlreadyUsed)
			{
				AddIdTransactionFalse();
				botClient.DeleteMessage(_message.From.Id, _message.MessageId, "48 - SetBlockChain");
				botClient.EditMessage(_message.From.Id, user.MessageID, "Данная транзакция уже была использованна или она не действительная!", "51 - SetIdTransaction", user, replyMarkup: InlineButtonBlockchain.TransactionCreationGuarantor(transaction));
				return false;
			}

			Money money = null;
			try
			{
				switch (transaction.PaymentId)
				{
					case 0:
					{
						return false;
					}
					case 1: // BTC
					{
						addresBTC = db.GetAddresBTCInt(transaction.AddresBTCId);
						money = await BlockchainManager.Instance.GetBTCTransactionMoney(_message.Text, addresBTC);
						break;
					}
					case 3: // Eth
					{
						money = await BlockchainManager.Instance.GetETHTransactionMoney(_message.Text);
						break;
					}
					case 4: // Ripple
					{
						money = await BlockchainManager.Instance.GetXRPTransactionMoney(_message.Text);
						break;
					}
				}
			}
			catch
			{
				AddIdTransactionIsNotBD();
				botClient.DeleteMessage(_message.From.Id, _message.MessageId, "48 - SetBlockChain");
				botClient.EditMessage(_message.From.Id, user.MessageID, "Данная транзакция не действительная или она не была подтвержденна, попробуйте ввести транзакцию позже!", "86 - SetIdTransaction", user, replyMarkup: InlineButtonBlockchain.TransactionCreationGuarantor(transaction));
				return false;
			}
			if (money == null)
			{
				AddIdTransactionFalse();
				botClient.DeleteMessage(_message.From.Id, _message.MessageId, "48 - SetBlockChain");
				botClient.EditMessage(_message.From.Id, user.MessageID, "Данная транзакция не действительная или она не была подтвержденна, попробуйте ввести транзакцию позже!", "92 - SetIdTransaction", user, replyMarkup: InlineButtonBlockchain.TransactionCreationGuarantor(transaction));
				return false;
			}
			Decimal dec = money.ToDecimal(MoneyUnit.BTC);
			Decimal percent = Convert.ToDecimal(0.15);
			if (dec < transaction.SumPayNew)
			{
				switch (transaction.PaymentId)
				{
					case 1:
					{
						addresBTC = db.GetAddresBTCInt(transaction.AddresBTCId);
						await BlockchainManager.Instance.SendBTC(transaction.PublicKeyWalletSender, dec, 0, addresBTC);
						break;
					}
					case 3:
					{
						await BlockchainManager.Instance.SendETHBackByHash(_message.Text);
						break;
					}
					case 4:
					{
						await BlockchainManager.Instance.SendXRPBackByHash(_message.Text);
						break;
					}
				}

				AddIdTransactionFalse();
				botClient.DeleteMessage(_message.From.Id, _message.MessageId, "48 - SetBlockChain");
				botClient.EditMessage(_message.From.Id, user.MessageID, "Вы отправили меньше денег нежелеи заявили, ваши деньги были возращены на ваш кошелек!", "122 - SetIdTransaction", user, replyMarkup: InlineButtonBlockchain.TransactionCreationGuarantor(transaction));
				return false;
			}

			AddIdTransaction();
			return true;
		}

		private void AddIdTransaction()
		{
			transaction.IdTransaction = _message.Text;
			transaction.CheckSendTransaction = true;
			transaction.IsPayGarantor = true;
			Decimal fee =  BlockchainManager.Instance.Settings.transactionPercentFee;
			Decimal totalFee = transaction.SumPayNew * (fee / 100.0m);
			if (transaction.WhoCommissionPay == true)
			{
				transaction.SumPayNew -= totalFee;
			}


			db.Save();
		}

		private void AddIdTransactionFalse()
		{
			transaction.CheckSendTransaction = false;
			transaction.IsPayGarantor = false;
			transaction.SumPayNew = 0;
			transaction.PaymentId = 0;

			db.AddFailedTransaction(new TransactionId()
			{
				NameHashOne = _message.Text
			});

			db.Save();
		}

		private void AddIdTransactionIsNotBD()
		{
			transaction.CheckSendTransaction = false;
			transaction.IsPayGarantor = false;
			transaction.SumPayNew = 0;
			transaction.PaymentId = 0;
			db.Save();
		}

		private Single ConvertInUAH()
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


			Single perOne = Convert.ToSingle(transaction.SumPayNew * Convert.ToDecimal(cours.Last));
			
			return perOne;
		}

		private void SetIncome(List<ChannelInfo> channel, Int32 id, Single total)
		{
			DataBase db = Singleton.GetInstance().Context;
			Int32 temp = channel.Count;
			foreach (var item in channel)
			{
				Income income = db.GetIncome(id, ((item.Channel.Id + 1000000000000) * -1));
				if (income == null)
				{
					db.SetValue<Income>(new Income() { ChannelId = (item.Channel.Id + 1000000000000) * -1, UserId = id, dateTime = System.DateTime.Today, SumIncome = total / temp });
				}
				else
				{
					income.SumIncome = (total / temp) + income.SumIncome;
				}
			}
			db.Save();
		}

		private void SetIncomeChannel(TelegramBotClient botClient)
		{
			DataBase db = Singleton.GetInstance().Context;

			var total = ConvertInUAH();

			List<ChannelInfo> channel = null;
			
			channel = StartSession.Test(db.GetChannelsList(), user.ID);
			
			Int32 temp = channel.Count;

			foreach (var item in channel)
			{
				IncomeChannel incomeChannel = db.GetIncomeChannels((item.Channel.Id + 1000000000000) * -1);
				if (incomeChannel == null)
				{
					db.SetValue<IncomeChannel>(new IncomeChannel() { ChannelId = (item.Channel.Id + 1000000000000) * -1, DateTime = System.DateTime.Today, SumIncome = total / temp });
				}
				else
				{
					incomeChannel.SumIncome = (total / temp) + incomeChannel.SumIncome;
				}
			}
			db.Save();
			SetIncome(channel, user.ID, total);
			SetIncomeChannelAdmin(channel, total, botClient);
		}

		private void SetIncomeChannelAdmin(List<ChannelInfo> channel, Single total, TelegramBotClient botClient)
		{
			DataBase db = Singleton.GetInstance().Context;
			total = (total * 20) / 100;
			foreach (var item in channel)
			{
				Int32 temp = item.Admins.Count;
				foreach (var admin in item.Admins)
				{
					IncomeChannelAdmin income = db.GetIncomeChannelsAdmin(admin.Id, ((item.Channel.Id + 1000000000000) * -1));
					if (admin.Id != botClient.BotId)
					{
						if (income == null)
						{
							db.SetValue<IncomeChannelAdmin>(new IncomeChannelAdmin() { ChannelId = (item.Channel.Id + 1000000000000) * -1, UserId = admin.Id, DateTime = System.DateTime.Today, SumIncome = total / temp });
						}
						else
						{
							income.SumIncome = (total / temp) + income.SumIncome;
						}
					}
				}
			}
			db.Save();
		}

		public override System.Object Handle(System.Int32 request, TelegramBotClient botClient, System.Object message)
		{
			if (request == (System.Int32)SetChain.SetIdTransaction)
			{
				_message = message as Message;
				db = Singleton.GetInstance().Context;

				user = db.GetUser(_message.From.Id);
				if (IsNullDataBase.IsNull(botClient, _message, user)) return null;

				transaction = db.GetTransaction(user);
				if (IsNullDataBase.IsNull(botClient, _message, transaction)) return null;

				ChangeUserChanin();

				if (!CheckIdTransaction(botClient).Result) return null;

				SendMessage(botClient, _message);

				Task.Run(() => SetIncomeChannel(botClient));

				return null;
			}
			else
			{
				return base.Handle(request, botClient, message);
			}
		}
	}
}