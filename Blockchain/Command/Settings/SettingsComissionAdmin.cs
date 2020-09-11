using System;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCore.Blockchain.Command.Settings
{
	internal class SettingsComissionAdmin : Advertising.Command.AbsCommand, IStandartCommand
	{
		public override System.String Name { set; get; } = CommandTextBlockchain.SettingsCommisionAdmin;

		private DataBase db = null;
		private User user = null;
		private CallbackQuery _message = null;
		private Decimal Value;

		public override void Execute(TelegramBotClient botClient, System.Object message)
		{
			IStandartCommand standartCommand = new SettingsAdminInBlockChain();

			if (standartCommand.SetCallbackQuery(message, out _message)) return;

			if (standartCommand.SetDataBase(out db)) return;

			if (standartCommand.SetUserAndCheckIsNull(botClient, _message, out user, db)) return;

			Value = BlockchainManager.Instance.Settings.transactionPercentFeeAdmin;//TODO

			SendMessage(botClient);
		}

		public void SendMessage(TelegramBotClient botClient) => botClient.EditMessage(_message.From.Id, user.MessageID, "🔐Гарант🔐", "20 - GuarantorMeinMenu", user, replyMarkup: InlineButtonBlockchain.SettingsCommisionAdmin(Value));
	}
}
