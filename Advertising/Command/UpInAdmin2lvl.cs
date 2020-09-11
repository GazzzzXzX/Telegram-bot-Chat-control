using System;
using BotCore.Advertising;
using BotCore.Blockchain;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCore
{
	class UpInAdmin2lvl : Advertising.Command.AbsCommand, IStandartCommand, ISplitName
	{
		public override System.String Name { set; get; } = CommandText.UpInAdmin2lvl;

		private DataBase db = null;
		private User user = null;
		private User userTwo = null;
		private CallbackQuery _message = null;

		public override void Execute(TelegramBotClient botClient, System.Object message)
		{
			IStandartCommand standartCommand = new UpInAdmin2lvl();
			ISplitName splitName = new UpInAdmin2lvl();

			if (standartCommand.SetCallbackQuery(message, out _message)) return;

			if (standartCommand.SetDataBase(out db)) return;

			if (standartCommand.SetUserAndCheckIsNull(botClient, _message, out user, db)) return;

			Int32 id = splitName.GetNameSplit(Name);
			Name = CommandText.UpInAdmin2lvl;
			if (IsBan.Ban(botClient, message))
			{
				if (SetUserTwo(id)) return;

				ChangeUser();

				SendMessage(botClient);
			}
		}

		private Boolean SetUserTwo(Int32 id)
		{
			userTwo = db.GetUser(id);
			return userTwo == null;
		}

		public async void SendMessage(TelegramBotClient botClient)
		{
			botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "Назначение 2 уровня администратора принято!", "280", user, replyMarkup: Advertising.InlineButton.AdminPanelLvl3(userTwo));
		}

		private void ChangeUser()
		{
			user.Chain = (Int32)SetChain.SetPublicKeyUserTwo;
			userTwo.IsAdmin = 2;
			db.Save();
		}
	}
}
