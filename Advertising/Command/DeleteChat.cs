using System;
using BotCore.Blockchain;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCore.Advertising.Command
{
	internal class DeleteChat : AbsCommand, IStandartCommand
	{
		public override System.String Name { set; get; } = CommandText.DeleteChat;

		private DataBase db = null;
		private User user = null;
		private CallbackQuery _message = null;

		public override void Execute(TelegramBotClient botClient, System.Object message)
		{
			IStandartCommand standartCommand = new DeleteChat();

			if (standartCommand.SetCallbackQuery(message, out _message)) return;

			if (standartCommand.SetDataBase(out db)) return;

			if (standartCommand.SetUserAndCheckIsNull(botClient, _message, out user, db)) return;

			if (user.IsAdmin >= 2)
			{
				ChangeUser();

				SendMessage(botClient);
			}
		}

		private void ChangeUser()
		{
			user.Chain = (Int32)SetChain.DeleteChat;
			db.Save();
		}

		public void SendMessage(TelegramBotClient botClient)
		{
			BotCore.InlineButton inlineBatton = new BotCore.InlineButton();
			Settings settings = db.GetSettings();

			botClient.EditMessage(_message.From.Id, user.MessageID, "Введите имя канала: [@name]", "32 - AddAccauntBot", user, inlineBatton.BackToSetting);

		}
	}
}
