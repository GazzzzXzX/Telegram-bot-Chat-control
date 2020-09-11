using System;
using System.Linq;
using BotCore.Blockchain;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCore
{
	internal class MuteEver : AbsCommand, IStandartCommand, ISplitName
	{
		public override System.String Name { set; get; } = CommandText.MuteEver;

		private DataBase db = null;
		private User user = null;
		private User userTwo = null;
		private CallbackQuery _message = null;

		public override void Execute(TelegramBotClient botClient, System.Object message)
		{
			IStandartCommand standartCommand = new MuteEver();
			ISplitName splitName = new MuteEver();

			if (standartCommand.SetCallbackQuery(message, out _message)) return;

			if (standartCommand.SetDataBase(out db)) return;

			if (standartCommand.SetUserAndCheckIsNull(botClient, _message, out user, db)) return;

			Int32 idUser = splitName.GetNameSplit(Name);
			Name = CommandText.MuteEver;

			if (SelectedUser(idUser)) return;
			if (user.IsAdmin > 0)
			{
				BanUser(botClient);

				SendMessage(botClient);
			}
		}

		private Boolean SelectedUser(Int32 idUser)
		{
			userTwo = db.GetUser(idUser);
			return userTwo == null;
		}

		private void BanUser(TelegramBotClient botClient)
		{
			userTwo.BanDate = DateTime.MaxValue;
			userTwo.BanDescript = "Вы были забанены администрацией UBC!";
			userTwo.PayConfirm = false;
			userTwo.PayDate = System.DateTime.Today;
			db.Save();
			IsMute.ThisMut(botClient, user);
		}

		public void SendMessage(TelegramBotClient botClient)
		{
			InlineButton inlineButton = new InlineButton();
			if (user.IsAdmin == 3)
			{
				botClient.EditMessage(_message.From.Id, user.MessageID, "Пользователь замучен навсегда!", "36 - LimitedUser", replyMarkup: inlineButton.InteractionUsers(userTwo, db._featuredUserNews.Any(p => p.UserId == userTwo.ID && p.UserWhoAddedId == user.ID), db.GetFeaturedUsers(user, userTwo), isAdmin: true));
			}
			else if (userTwo.IsAdmin != 3 && userTwo.IsAdmin != 2)
			{
				botClient.EditMessage(_message.From.Id, user.MessageID, "Пользователь замучен навсегда!", "36 - LimitedUser", replyMarkup: inlineButton.InteractionUsers(userTwo, db._featuredUserNews.Any(p => p.UserId == userTwo.ID && p.UserWhoAddedId == user.ID), db.GetFeaturedUsers(user, userTwo), isAdmin: true));
			}
			else
			{
				user.IsAdmin = 0;
				userTwo.BanDate = System.DateTime.Today;
				if (user.BanDate.Date < System.DateTime.Today)
				{
					user.BanDate = System.DateTime.Now;
				}
				user.BanDate = user.BanDate.AddDays(30);
				db.Save();
				Settings settings = db.GetSettings();
				IsBanUser.ThisBan(botClient, _message, user, settings);

				System.String temp = "Администратор " + user.IsAdmin + " уровня: @" + user.Username + " пытался забанить другого администратора " + "\nID: " + userTwo.ID + "\nФИО: " + userTwo.FIO + "\nНомер: " + user.Number + "\nС данного администратора снята админка, так же он был забанен во всех чатах! Если бан был выдан случайно пропишите /UbBan " + user.ID;

				Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup answer = inlineButton.AdminPanelAppeal(_message, user.ID, _message.Data);
				botClient.SendText(settings.ChannelAdmin, temp, replyMarkup: answer);
			}
		}
	}
}
