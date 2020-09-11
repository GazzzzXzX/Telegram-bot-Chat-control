using System;
using BotCore.Advertising;
using BotCore.Blockchain;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCore.Advertising.Command
{
	class ChuseAdmin : Advertising.Command.AbsCommand, IStandartCommand, ISplitName
	{
		public override System.String Name { set; get; } = CommandText.ChuseAdmin;

		private DataBase db = null;
		private User user = null;
		private User userTwo = null;
		private CallbackQuery _message = null;

		public override void Execute(TelegramBotClient botClient, System.Object message)
		{
			IStandartCommand standartCommand = new ChuseAdmin();
			ISplitName splitName = new ChuseAdmin();

			if (standartCommand.SetCallbackQuery(message, out _message)) return;

			if (standartCommand.SetDataBase(out db)) return;

			if (standartCommand.SetUserAndCheckIsNull(botClient, _message, out user, db)) return;

			Int32 id = splitName.GetNameSplit(Name);
			Name = CommandText.ChuseAdmin;
			if (IsBan.Ban(botClient, message))
			{
				if (SetUserTwo(id)) return;

				SendMessage(botClient);
			}
		}

		private Boolean SetUserTwo(Int32 id)
		{
			userTwo = db.GetUser(id);
			return userTwo == null;
		}

		public void SendMessage(TelegramBotClient botClient)
		{
			botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "Панель назначения", "280", user, replyMarkup: InlineButton.AdminPanelLvl3(userTwo));
		}
	}
}
