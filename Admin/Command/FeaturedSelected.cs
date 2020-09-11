using System;
using BotCore.Blockchain;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCore
{
	internal class FeaturedSelected : AbsCommand, IStandartCommand, ISplitName
	{
		public override System.String Name { set; get; } = CommandText.FeaturedSelected;

		private DataBase db = null;
		private User user = null;
		private User userTwo = null;
		private CallbackQuery _message = null;

		public override void Execute(TelegramBotClient botClient, System.Object message)
		{
			IStandartCommand standartCommand = new FeaturedSelected();
			ISplitName splitName = new FeaturedSelected();

			if (standartCommand.SetCallbackQuery(message, out _message)) return;

			if (standartCommand.SetDataBase(out db)) return;

			if (standartCommand.SetUserAndCheckIsNull(botClient, _message, out user, db)) return;

			Int32 idUser = splitName.GetNameSplit(Name);
			Name = CommandText.FeaturedSelected;

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
			System.String temp = InfoUser.Search(userTwo, user);

			if (user.IsAdmin > 0)
			{
				botClient.EditMessage(user.ID, user.MessageID, temp, "39 - AddPhotoInDataBase", user, inlineButton.InteractionUsers(userTwo, true, db.GetFeaturedUsers(user, userTwo), isAdmin: true));
			}
			else
			{
				botClient.EditMessage(user.ID, user.MessageID, temp, "39 - AddPhotoInDataBase", user, inlineButton.InteractionUsers(userTwo, true, db.GetFeaturedUsers(user, userTwo)));
			}

		}
	}
}
