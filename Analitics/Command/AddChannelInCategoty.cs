using System;
using BotCore.Advertising;
using BotCore.Blockchain;

using Telegram.Bot;
using Telegram.Bot.Types;


namespace BotCore.PhotoChannel
{
	internal class AddChannelInCategoty : Advertising.Command.AbsCommand, IStandartCommand
	{
		public override System.String Name { set; get; } = CommandText.AddChannelInCategoty;

		private DataBase db = null;
		private User user = null;
		private CallbackQuery _message = null;

		public override void Execute(TelegramBotClient botClient, System.Object message)
		{
			IStandartCommand standartCommand = new AddCategoty();

			if (standartCommand.SetCallbackQuery(message, out _message)) return;

			if (standartCommand.SetDataBase(out db)) return;

			if (standartCommand.SetUserAndCheckIsNull(botClient, _message, out user, db)) return;
			if (IsBan.Ban(botClient, message))
			{
				ChangeUser();

				SendMessage(botClient);
			}

		}

		public void SendMessage(TelegramBotClient botClient)
		{
			InlineButton inlineButton = new InlineButton();
			botClient.EditMessage(user.ID, user.MessageID, "[Название категории] [@группа]: ", "39 - AddPhotoInDataBase", user, inlineButton.BackToSettingAdmin);
		}

		private void ChangeUser()
		{
			user.Chain = (Int32)SetChain.AddChannelInCategory;
			db.Save();
		}
	}
}
