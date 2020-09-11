using System;
using System.Linq;
using BotCore.Blockchain;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCore
{
	internal class DeleteFeatured : AbsCommand, IStandartCommand, ISplitName
	{
		public override System.String Name { set; get; } = CommandText.DeleteFeatured;

		private DataBase db = null;
		private User user = null;
		private User userTwo = null;
		private CallbackQuery _message = null;

		public override void Execute(TelegramBotClient botClient, System.Object message)
		{
			IStandartCommand standartCommand = new DeleteFeatured();
			ISplitName splitName = new DeleteFeatured();

			if (standartCommand.SetCallbackQuery(message, out _message)) return;

			if (standartCommand.SetDataBase(out db)) return;

			if (standartCommand.SetUserAndCheckIsNull(botClient, _message, out user, db)) return;

			Int32 idUser = splitName.GetNameSplit(Name);
			Name = CommandText.DeleteFeatured;

			Selected(idUser);

			SendMessage(botClient);
		}

		private void Selected(Int32 idUser)
		{
			FeaturedUserNew featured = db.GetFeaturedUsers(idUser);
			db._featuredUserNews.Remove(featured);
			db.Save();
			SelectedUser(featured.UserId);
		}

		private Boolean SelectedUser(Int32 id)
		{
			userTwo = db.GetUser(id);
			return userTwo == null;
		}

		public void SendMessage(TelegramBotClient botClient)
		{
			InlineButton inlineButton = new InlineButton();
			System.String temp = InfoUser.Search(userTwo, user);
			if (db._featuredUserNews.Any(p => p.UserId == userTwo.ID && p.UserWhoAddedId == user.ID))
			{
				if (user.IsAdmin > 0)
				{
					botClient.EditMessage(user.ID, user.MessageID, temp, "39 - AddPhotoInDataBase", user, inlineButton.InteractionUsers(user, true, db.GetFeaturedUsers(user, userTwo), isAdmin: true));
				}
				else
				{
					botClient.EditMessage(user.ID, user.MessageID, temp, "39 - AddPhotoInDataBase", user, inlineButton.InteractionUsers(user, true, db.GetFeaturedUsers(user, userTwo), isAdmin: false));
				}
			}
			else
			{
				if (user.IsAdmin > 0)
				{
					botClient.EditMessage(user.ID, user.MessageID, temp, "39 - AddPhotoInDataBase", user, inlineButton.InteractionUsers(user, false, isAdmin: true));
				}
				else
				{

					botClient.EditMessage(user.ID, user.MessageID, temp, "39 - AddPhotoInDataBase", user, inlineButton.InteractionUsers(user, false, isAdmin: false));
				}
			}
		}
	}
}
