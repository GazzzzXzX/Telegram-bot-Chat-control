using System;
using BotCore.Advertising;
using BotCore.Blockchain;
using BotCore.PhotoChannel;
using Telegram.Bot;
using Telegram.Bot.Types;


namespace BotCore
{
	class ThisIncome : Advertising.Command.AbsCommand, IStandartCommand
	{
		public override System.String Name { set; get; } = CommandText.ThisIncome;

		private DataBase db = null;
		private User user = null;
		private CallbackQuery _message = null;

		public override void Execute(TelegramBotClient botClient, System.Object message)
		{
			IStandartCommand standartCommand = new ThisIncome();

			if (standartCommand.SetCallbackQuery(message, out _message)) return;

			if (standartCommand.SetDataBase(out db)) return;

			if (standartCommand.SetUserAndCheckIsNull(botClient, _message, out user, db)) return;
			if (IsBan.Ban(botClient, message))
			{
				if (user.IsAdmin >= 2)
				{
					SendMessage(botClient);
				}
			}
		}

		public void SendMessage(TelegramBotClient botClient)
		{
			botClient.EditMessage(user.ID, user.MessageID, "Доходность", "39 - AddPhotoInDataBase", user, InlineButtonPhotoChannel.ThisIncome);
		}
	}
}
