using System;
using BotCore.Advertising;
using BotCore.Blockchain;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCore
{
	internal class AddAccauntBot : AbsCommand, IStandartCommand
	{
		public override System.String Name { set; get; } = CommandText.AddAccaunt;

		private DataBase db = null;
		private User user = null;
		private CallbackQuery _message = null;

		public override void Execute(TelegramBotClient botClient, System.Object message)
		{
			IStandartCommand standartCommand = new AddAccauntBot();

			if (standartCommand.SetCallbackQuery(message, out _message)) return;

			if (standartCommand.SetDataBase(out db)) return;

			if (standartCommand.SetUserAndCheckIsNull(botClient, _message, out user, db)) return;

			ChangeUser();

			SendMessage(botClient);
		}

		private void ChangeUser()
		{
			user.Chain = (Int32)SetChain.AddAccauntUser;
			db.Save();
		}

		public void SendMessage(TelegramBotClient botClient)
		{
			InlineButton inlineBatton = new InlineButton();
			botClient.EditMessage(_message.From.Id, user.MessageID, "Введите номер: ", "32 - AddAccauntBot", user, inlineBatton.BackToSetting);
		}
	}
}
