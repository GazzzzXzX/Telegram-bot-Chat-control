﻿using System;

using BotCore.SQL;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCore.Blockchain
{
	internal class NameTransactionConfirm : Advertising.Command.AbsCommand, IStandartCommand, ISplitName, ITransaction
	{
		public override System.String Name { set; get; } = CommandTextBlockchain.NameTransactionConfirm;

		private DataBase db = null;
		private User user = null;
		private CallbackQuery _message = null;
		private Transaction transaction = null;

		public override void Execute(TelegramBotClient botClient, System.Object message)
		{
			IStandartCommand standartCommand = new NameTransactionConfirm();
			ISplitName splitName = new NameTransactionConfirm();
			ITransaction transaction = new NameTransactionConfirm();

			if (standartCommand.SetCallbackQuery(message, out _message)) return;

			if (standartCommand.SetDataBase(out db)) return;

			if (standartCommand.SetUserAndCheckIsNull(botClient, _message, out user, db)) return;

			Int32 IdTransaction = splitName.GetNameSplit(Name);
			Name = CommandTextBlockchain.NameTransactionConfirm;

			if (transaction.GetTransaction(out this.transaction, IdTransaction, db)) return;

			SendMessage(botClient);
		}

		public void SendMessage(TelegramBotClient botClient)
		{
			String text = $"💼Мои транзакции💼\nОтправитель: {transaction.UserSender.FIO}\nПолучитель: {transaction.UserRecipient.FIO}\nКомиссия: ";
			text += transaction.WhoCommissionPay == true ? "получатель" : "отправитель";
			text += $"\nСумма: {transaction.SumPayNew}\nВалюта: ";
			text += transaction.PaymentId == 1 ? "BTC" : transaction.PaymentId == 2 ? "USDT" : transaction.PaymentId == 3 ? "Ethereum" : transaction.PaymentId == 4 ? "Ripple" : "не выбрана!";
			text += "\n\nВсе условия соблюдены?";

			botClient.EditMessage(_message.From.Id, user.MessageID, text, "17 - AddUserInTransaction", user, replyMarkup: InlineButtonBlockchain.SelectConfirmOrCancelThisTransaction(transaction));
		}
	}
}