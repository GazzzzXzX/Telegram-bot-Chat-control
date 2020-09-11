using System;
using System.Linq;
using BotCore.Blockchain;
using Telegram.Bot;
using Telegram.Bot.Types;


namespace BotCore
{
	internal class AddFeatured : AbsCommand, IStandartCommand, ISplitName
	{
		public override System.String Name { set; get; } = CommandText.AddFeatured;

		private DataBase db = null;
		private User user = null;
		private User userTwo = null;
		private CallbackQuery _message = null;

		public override void Execute(TelegramBotClient botClient, System.Object message)
		{
			IStandartCommand standartCommand = new AddFeatured();
			ISplitName splitName = new AddFeatured();

			if (standartCommand.SetCallbackQuery(message, out _message)) return;

			if (standartCommand.SetDataBase(out db)) return;

			if (standartCommand.SetUserAndCheckIsNull(botClient, _message, out user, db)) return;

			Int32 idUser = splitName.GetNameSplit(Name);
			Name = CommandText.AddFeatured;

			if (SelectedUser(idUser)) return;

			Add();

			SendMessage(botClient);
		}

		private Boolean SelectedUser(Int32 idUser)
		{
			userTwo = db.GetUser(idUser);
			return userTwo == null;
		}

		private void Add()
		{
			db.SetValue<FeaturedUserNew>(new FeaturedUserNew() { UserId = userTwo.ID, UserWhoAddedId = user.ID });
			db.Save();
		}

		public void SendMessage(TelegramBotClient botClient)
		{
			InlineButton inlineButton = new InlineButton();
			System.String temp = InfoUser.Search(userTwo, user);
			if (db._featuredUserNews.Any(p => p.UserId == userTwo.ID && p.UserWhoAddedId == user.ID))
			{
				if (user.IsAdmin > 0)
				{
					botClient.EditMessage(user.ID, user.MessageID, temp, "39 - AddPhotoInDataBase", user, inlineButton.InteractionUsers(userTwo, true, db.GetFeaturedUsers(user, userTwo), isAdmin: true));
				}
				else
				{
					botClient.EditMessage(user.ID, user.MessageID, temp, "39 - AddPhotoInDataBase", user, inlineButton.InteractionUsers(userTwo, true, db.GetFeaturedUsers(user, userTwo)));
				}
			}
			else
			{
				if (user.IsAdmin > 0)
				{
					botClient.EditMessage(user.ID, user.MessageID, temp, "39 - AddPhotoInDataBase", user, inlineButton.InteractionUsers(userTwo, false, isAdmin: true));
				}
				else
				{
					botClient.EditMessage(user.ID, user.MessageID, temp, "39 - AddPhotoInDataBase", user, inlineButton.InteractionUsers(userTwo, false));
				}
			}
		}
	}
}
