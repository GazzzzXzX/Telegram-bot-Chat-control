using System;

using BotCore.Advertising;
using BotCore.SQL;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCore.Blockchain
{
	internal class BackToSetMoneyCount : Advertising.Command.AbsCommand, IStandartCommand, ITransaction, ISplitName
	{
		public override System.String Name { set; get; } = CommandTextBlockchain.BackToSetMoneyCount;

		private DataBase db = null;
		private User user = null;
		private CallbackQuery _message = null;
		private Transaction transaction = null;

		public override void Execute(TelegramBotClient botClient, System.Object message)
		{
			IStandartCommand standartCommand = new BackToSetMoneyCount();
			ITransaction transaction = new BackToSetMoneyCount();
			ISplitName splitName = new BackToSetMoneyCount();

			if (standartCommand.SetCallbackQuery(message, out _message)) return;

			if (standartCommand.SetDataBase(out db)) return;

			if (standartCommand.SetUserAndCheckIsNull(botClient, _message, out user, db)) return;

			Int32 IdTransaction = splitName.GetNameSplit(Name);
			Name = CommandTextBlockchain.BackToSetMoneyCount;

			if (transaction.GetTransaction(out this.transaction, IdTransaction, db)) return;

			ChangeUser();

			ChangeTransaction();

			SendMessage(botClient);
		}

		public void SendMessage(TelegramBotClient botClient) => botClient.EditMessage(_message.From.Id, user.MessageID, "💰Выбор способа оплаты💰", "20 - BackToGuarantorMeinMenu", user, replyMarkup: InlineButtonBlockchain.ChoosingPaymentMethod(transaction));

		private void ChangeUser()
		{
			user.Chain = (Int32)SetChain.MessageUserInBot;
			db.Save();
		}

		private void ChangeTransaction()
		{
			transaction.SumPayNew = 0;
			transaction.CheckSendTransaction = false;
			db.Save();
		}
	}
}