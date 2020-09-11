using System;

using BotCore.SQL;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCore.Blockchain
{
	internal class ChoosingPaymentMethod : Advertising.Command.AbsCommand, IStandartCommand, ITransaction, ISplitName
	{
		public override System.String Name { set; get; } = CommandTextBlockchain.ChoosingPaymentMethod;

		private DataBase db = null;
		private User user = null;
		private CallbackQuery _message = null;
		private Transaction transaction = null;

		public override void Execute(TelegramBotClient botClient, System.Object message)
		{
			IStandartCommand standartCommand = new ChoosingPaymentMethod();
			ISplitName splitName = new ChoosingPaymentMethod();
			ITransaction transaction = new ChoosingPaymentMethod();

			if (standartCommand.SetCallbackQuery(message, out _message)) return;

			if (standartCommand.SetDataBase(out db)) return;

			if (standartCommand.SetUserAndCheckIsNull(botClient, _message, out user, db)) return;

			Int32 idTransaction = splitName.GetNameSplit(Name);
			Name = CommandTextBlockchain.ChoosingPaymentMethod;

			if (transaction.GetTransaction(out this.transaction, idTransaction, db)) return;

			SendMessage(botClient);
		}

		public void SendMessage(TelegramBotClient botClient) => botClient.EditMessage(_message.From.Id, user.MessageID, "💰Выбор способа оплаты💰", "20 - ChoosingPaymentMethod", user, replyMarkup: InlineButtonBlockchain.ChoosingPaymentMethod(transaction));
	}
}