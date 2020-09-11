using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCore.Blockchain
{
	internal class BackToReputationUser : Advertising.Command.AbsCommand, IStandartCommand
	{
		public override System.String Name { set; get; } = CommandTextBlockchain.BackToReputationUser;

		private DataBase db = null;
		private User user = null;
		private CallbackQuery _message = null;

		public override void Execute(TelegramBotClient botClient, System.Object message)
		{
			IStandartCommand standartCommand = new BackToReputationUser();

			if (standartCommand.SetCallbackQuery(message, out _message)) return;

			if (standartCommand.SetDataBase(out db)) return;

			if (standartCommand.SetUserAndCheckIsNull(botClient, _message, out user, db)) return;

			SendMessage(botClient);
		}

		public void SendMessage(TelegramBotClient botClient) => botClient.EditMessage(_message.From.Id, user.MessageID, "🏆Репутация🏆", "17 - CancelTransactionReputationUser", user, replyMarkup: InlineButtonBlockchain.ReputationUser);
	}
}