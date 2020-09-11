using System;
using BotCore.Blockchain;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCore
{
	internal class CoinMinus : AbsCommand, IStandartCommand
	{
		public override System.String Name { set; get; } = CommandText.CoinMinus;

		private DataBase db = null;
		private User user = null;
		private CallbackQuery _message = null;

		public override void Execute(TelegramBotClient botClient, System.Object message)
		{
			IStandartCommand standartCommand = new CoinMinus();

			if (standartCommand.SetCallbackQuery(message, out _message)) return;

			if (standartCommand.SetDataBase(out db)) return;

			if (standartCommand.SetUserAndCheckIsNull(botClient, _message, out user, db)) return;

			if (user.IsAdmin >= 2)
			{
				ChangeSettings();
			}

			SendMessage(botClient);
		}

		private void ChangeSettings()
		{
			Settings settings = db.GetSettings();
			if (settings.Coin != 0)
			{
				settings.Coin -= 1;
				db.Save();
			}
		}

		public void SendMessage(TelegramBotClient botClient)
		{
			InlineButton inlineButton = new InlineButton();
			String Text = "Количество бонусов за добавленого человека:";
			botClient.EditMessage(user.ID, user.MessageID, Text, "39 - AddPhotoInDataBase", user, inlineButton.CoinAdd());
		}
	}
}
