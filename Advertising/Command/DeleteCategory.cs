using System;
using BotCore.Blockchain;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCore.Advertising.Command
{
	internal class DeleteCategory : AbsCommand, IStandartCommand
	{
		public override System.String Name { set; get; } = CommandText.DeleteCategory;

		private DataBase db = null;
		private User user = null;
		private CallbackQuery _message = null;

		public override void Execute(TelegramBotClient botClient, System.Object message)
		{
			IStandartCommand standartCommand = new DeleteCategory();

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
			user.Chain = (Int32)SetChain.DeleteCategory;
			db.Save();
		}

		public void SendMessage(TelegramBotClient botClient)
		{
			BotCore.InlineButton inlineBatton = new BotCore.InlineButton();

			botClient.EditMessage(_message.From.Id, user.MessageID, "Введите название категории: ", "32 - AddAccauntBot", user, inlineBatton.BackToSetting);

		}
	}
}
