using System;

using BotCore.SQL;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCore.Blockchain
{
	/// <summary>
	/// Класс отвечающий за отправление денег отправителю!
	/// </summary>
	internal class GetMoneySenderAdminInBlockChain : Advertising.Command.AbsCommand, IStandartCommand, ITransaction, ISplitName
	{
		public override System.String Name { set; get; } = CommandTextBlockchain.GetMoneySenderAdminInBlockChain;

		private DataBase db = null;
		private User user = null;
		private User userAdmin = null;
		private CallbackQuery _message = null;
		private Transaction transaction = null;
		private Settings settings = null;
		private AddresBTC addresBTC = null;

		public override void Execute(TelegramBotClient botClient, System.Object message)
		{
			IStandartCommand standartCommand = new CancelMyTransaction();
			ITransaction transaction = new CancelMyTransaction();
			ISplitName splitName = new CancelMyTransaction();

			if (standartCommand.SetCallbackQuery(message, out _message)) return;

			if (standartCommand.SetDataBase(out db)) return;

			if (standartCommand.SetUserAndCheckIsNull(botClient, _message, out user, db)) return;

			SetUser();

			if (user.ID == userAdmin.ID)
			{
				SetSettings();
				if (IsNullDataBase.IsNull(botClient, _message, settings)) return;

				Int32 IdTransaction = splitName.GetNameSplit(Name);
				Name = CommandTextBlockchain.GetMoneySenderAdminInBlockChain;

				if (transaction.GetTransaction(out this.transaction, IdTransaction, db)) return;

				ChangeTransaction();

				if (CheckTransaction())
				{
					PaySender();
				}

				SendMessage(botClient);

				DeleteMessage(botClient);
			}
		}

		private void DeleteMessage(TelegramBotClient botClient) => botClient.DeleteMessage(settings.ChannelAdmin, _message.Message.MessageId, "51 -GetMoneySenderAdminInBlockChain");

		public void SendMessage(TelegramBotClient botClient)
		{
			String text = $"🛎Вызов администратора🛎\nОтправитель: {transaction.UserSender.FIO}\nПолучитель: {transaction.UserRecipient.FIO}\nКомиссия: ";
			text += transaction.WhoCommissionPay == true ? "получатель" : "отправитель";
			text += $"\nСумма: {transaction.SumPayNew}\nВалюта: ";
			text += transaction.PaymentId == 1 ? "BTC" : transaction.PaymentId == 2 ? "USDT" : transaction.PaymentId == 3 ? "Ethereum" : transaction.PaymentId == 4 ? "Ripple" : "не выбрана!";
			text += transaction.DescriptionCancelSender == "" ? "" : $"\nОтмена отправителя: {transaction.DescriptionCancelSender}";
			text += transaction.DescriptionCancelRecipient == "" ? "" : $"\nОтмена получателя: {transaction.DescriptionCancelRecipient}";
			text += $"Администратор @{user.Username} принял принял решение отправить деньги отправителю!";

			botClient.SendText(transaction.UserSender.ID, text);

			botClient.SendText(transaction.UserRecipient.ID, text);

			String textAdmin = $"🛎Вызов администратора🛎\nОтправитель: {transaction.UserSender.FIO}";
			textAdmin += transaction.UserSender.Username == "\nНет!" ? "" : $"\nНикнейм: @{transaction.UserSender.Username}";
			textAdmin += $"\nНомер телефона: {transaction.UserSender.Number}";
			textAdmin += $"\nПолучитель: {transaction.UserRecipient.FIO}";
			textAdmin += transaction.UserRecipient.Username == "\nНет!" ? "" : $"\nНикнейм: @{transaction.UserRecipient.Username}";
			textAdmin += $"\nНомер телефона: {transaction.UserRecipient.Number}";
			textAdmin += "\nКомиссия: ";
			textAdmin += transaction.WhoCommissionPay == true ? "получатель" : "отправитель";
			textAdmin += $"\nСумма: {transaction.SumPayNew}\nВалюта: ";
			textAdmin += transaction.PaymentId == 1 ? "BTC" : transaction.PaymentId == 2 ? "USDT" : transaction.PaymentId == 3 ? "Ethereum" : transaction.PaymentId == 4 ? "Ripple" : "не выбрана!";
			textAdmin += transaction.DescriptionCancelSender == "" ? "" : $"\nОтмена отправителя: {transaction.DescriptionCancelSender}";
			textAdmin += transaction.DescriptionCancelRecipient == "" ? "" : $"\nОтмена получателя: {transaction.DescriptionCancelRecipient}";
			text += $"Администратор @{user.Username} принял принял решение отправить деньги отправителю!";

			botClient.SendText(settings.ChannelAdmin, textAdmin);
		}

		public void SetSettings() => settings = db.GetSettings();

		private void SetUser() => userAdmin = db.GetUser(Convert.ToInt32(Name.Split(" ")[2]));

		private void ChangeTransaction()
		{
			transaction.IsPaySenderOrRecipiend = true;
			transaction.IsConfirmOrCancelUserSender = 1;
			transaction.IsConfirmOrCancelUserRecipient = 1;
			db.Save();
		}

		private Boolean CheckTransaction()
		{
			if (transaction.IsConfirmOrCancelUserRecipient == 1 && transaction.IsConfirmOrCancelUserSender == 1)
			{
				return true;
			}
			return false;
		}

		private async void PaySender()
		{
			switch (transaction.PaymentId)
			{
				case 1:
					addresBTC = db.GetAddresBTCInt(transaction.AddresBTCId);
					await BlockchainManager.Instance.SendBTC(transaction.PublicKeyWalletSender, transaction.SumPayNew - (transaction.SumPayNew * ((BlockchainManager.Instance.Settings.transactionPercentFee + BlockchainManager.Instance.Settings.transactionPercentFeeAdmin) / 100)), (transaction.SumPayNew * ((BlockchainManager.Instance.Settings.transactionPercentFee + BlockchainManager.Instance.Settings.transactionPercentFeeAdmin) / 100)), addresBTC);
					break;
				case 3:
					await BlockchainManager.Instance.SendETHBackByHash(transaction.IdTransaction, BlockchainManager.Instance.Settings.transactionPercentFeeAdmin + BlockchainManager.Instance.Settings.transactionPercentFee);
					break;
				case 4:
					await BlockchainManager.Instance.SendXRPBackByHash(transaction.IdTransaction, BlockchainManager.Instance.Settings.transactionPercentFeeAdmin + BlockchainManager.Instance.Settings.transactionPercentFee);
					break;
			}

		}
	}
}