using System;
using BotCore.Blockchain;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCore
{
	internal class LimitUser : AbsCommand, IStandartCommand, ISplitName
	{
		public override System.String Name { set; get; } = CommandText.LimitUser;

		private DataBase db = null;
		private User user = null;
		private User userTwo = null;
		private CallbackQuery _message = null;

		public override void Execute(TelegramBotClient botClient, System.Object message)
		{
			IStandartCommand standartCommand = new LimitUser();
			ISplitName splitName = new LimitUser();

			if (standartCommand.SetCallbackQuery(message, out _message)) return;

			if (standartCommand.SetDataBase(out db)) return;

			if (standartCommand.SetUserAndCheckIsNull(botClient, _message, out user, db)) return;

			Int32 idUser = splitName.GetNameSplit(Name);
			Name = CommandText.LimitUser;

			if (SelectedUser(idUser)) return;

			SendMessage(botClient);
		}

		private Boolean SelectedUser(Int32 idUser)
		{
			userTwo = db.GetUser(idUser);
			return userTwo == null;
		}

		public void SendMessage(TelegramBotClient botClient)
		{
			InlineButton inlineButton = new InlineButton();
			botClient.EditMessage(_message.From.Id, user.MessageID, "💥Ограничить пользователя💥", "36 - LimitedUser", replyMarkup: inlineButton.AdminPanelInMessage(userTwo));
		}
	}
}
