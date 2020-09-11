using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCore.Blockchain
{
	internal class BackToMeinMenu : Advertising.Command.AbsCommand, IStandartCommand
	{
		public override System.String Name { set; get; } = CommandTextBlockchain.BackToMeinMenu;

		private DataBase db = null;
		private User user = null;
		private CallbackQuery _message = null;

		public override void Execute(TelegramBotClient botClient, System.Object message)
		{
			IStandartCommand standartCommand = new BackToMeinMenu();

			if (standartCommand.SetCallbackQuery(message, out _message)) return;

			if (standartCommand.SetDataBase(out db)) return;

			if (standartCommand.SetUserAndCheckIsNull(botClient, _message, out user, db)) return;

			SendMessage(botClient);
		}

		public void SendMessage(TelegramBotClient botClient)
		{
			InlineButton inlineButton = new InlineButton();
			if (user.IsAdmin == 0)
			{
				botClient.EditMessage(_message.From.Id, user.MessageID, InfoUser.Info(user), "20 - GuarantorMeinMenu", user, replyMarkup: inlineButton.Accaunt);
			}
			else
			{
				botClient.EditMessage(_message.From.Id, user.MessageID, InfoUser.Info(user), "20 - GuarantorMeinMenu", user, replyMarkup: inlineButton.AdminAccaunt);
			}
		}
	}
}