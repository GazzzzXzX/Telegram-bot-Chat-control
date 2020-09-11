using System;
using BotCore.Advertising;
using BotCore.Blockchain;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCore
{
	class DownInAdmin0lvl : Advertising.Command.AbsCommand, IStandartCommand, ISplitName
	{
		public override System.String Name { set; get; } = CommandText.DownInAdmin0lvl;

		private DataBase db = null;
		private User user = null;
		private User userTwo = null;
		private CallbackQuery _message = null;

		public override void Execute(TelegramBotClient botClient, System.Object message)
		{
			IStandartCommand standartCommand = new DownInAdmin0lvl();
			ISplitName splitName = new DownInAdmin0lvl();

			if (standartCommand.SetCallbackQuery(message, out _message)) return;

			if (standartCommand.SetDataBase(out db)) return;

			if (standartCommand.SetUserAndCheckIsNull(botClient, _message, out user, db)) return;

			Int32 id = splitName.GetNameSplit(Name);
			Name = CommandText.DownInAdmin0lvl;
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

		public void SendMessage(TelegramBotClient botClient)
		{
			botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "Все привелегии с данного человека сняты!", "280", user, replyMarkup: Advertising.InlineButton.AdminPanelLvl3(userTwo));
		}

		private void ChangeUser()
		{
			user.Chain = (Int32)SetChain.SetPublicKeyUserTwo;
			userTwo.IsAdmin = 0;
			db.Save();
		}
	}
}
