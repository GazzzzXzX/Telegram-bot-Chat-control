using BotCore.Blockchain;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCore.PhotoChannel
{
	internal class AnaliticsShow : Advertising.Command.AbsCommand, IStandartCommand
	{
		public override System.String Name { set; get; } = CommandText.AnaliticsShow;

		private DataBase db = null;
		private User user = null;
		private CallbackQuery _message = null;

		public override void Execute(TelegramBotClient botClient, System.Object message)
		{
			IStandartCommand standartCommand = new AnaliticsShow();

			if (standartCommand.SetCallbackQuery(message, out _message)) return;

			if (standartCommand.SetDataBase(out db)) return;

			if (standartCommand.SetUserAndCheckIsNull(botClient, _message, out user, db)) return;

			if (IsBan.Ban(botClient, message))
			{
				if (user.IsAdmin >= 1)
				{
					SendMessage(botClient);
				}
			}
		}

		public void SendMessage(TelegramBotClient botClient) => botClient.EditMessage(user.ID, user.MessageID, "💹Аналитика💹", "38 - AnaliticsShow", user, InlineButtonPhotoChannel.ShowAnalitics);
	}
}
