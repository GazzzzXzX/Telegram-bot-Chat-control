using BotCore.Blockchain;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCore
{
	internal class SearchUsername : AbsCommand, IStandartCommand
	{
		public override System.String Name { set; get; } = CommandText.SearchUsername;

		private DataBase db = null;
		private User user = null;
		private CallbackQuery _message = null;

		public override void Execute(TelegramBotClient botClient, System.Object message)
		{
			IStandartCommand standartCommand = new SearchUsername();

			if (standartCommand.SetCallbackQuery(message, out _message)) return;

			if (standartCommand.SetDataBase(out db)) return;

			if (standartCommand.SetUserAndCheckIsNull(botClient, _message, out user, db)) return;

			ChangeUser();

			SendMessage(botClient);
		}

		private void ChangeUser()
		{
			user.Chain = 50;
			db.Save();
		}

		public void SendMessage(TelegramBotClient botClient)
		{
			InlineButton inlineButton = new InlineButton();
			botClient.EditMessage(_message.From.Id,
						user.MessageID,
						"Введите @username:\nТак же переслать сообщение от человека и мы его найдем:", "464",
						replyMarkup: inlineButton.BackToAccauntMenu);
		}
	}
}
