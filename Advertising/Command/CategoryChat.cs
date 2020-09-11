using System;
using BotCore.Blockchain;
using BotCore.SQL;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCore.Advertising.Command
{
	class CategoryChat: AbsCommand, IStandartCommand, ISplitName
	{
		public override System.String Name { set; get; } = "/CategoryChat";

		private DataBase db = null;
		private User user = null;
		private CallbackQuery _message = null;
		private Int32 split = 0;

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			IStandartCommand standartCommand = new CategoryChat();
			ISplitName splitName = new CategoryChat();

			if (standartCommand.SetCallbackQuery(message, out _message)) return;

			if (standartCommand.SetDataBase(out db)) return;

			if (standartCommand.SetUserAndCheckIsNull(botClient, _message, out user, db)) return;

			split = splitName.GetNameSplit(Name);
			Name = "/CategoryChat";
			
			SendMessage(botClient);
		}

		public void SendMessage(TelegramBotClient botClient)
		{
			botClient.EditMessage(user.ID, user.MessageID, "Выберите категорию или воспользуйтесь глобальным поиском по всем категориям сразу.", "58 - BackToTypeMessage", user,
			                      InlineButton.CategoryChat(split));
		}
	}
}