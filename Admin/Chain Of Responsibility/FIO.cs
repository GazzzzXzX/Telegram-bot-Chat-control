using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BotCore.Advertising;
using Microsoft.EntityFrameworkCore;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCore
{
	internal static class AddNewChannel
	{
		public static void SetTmessage()
		{
			//Settings settings = Singleton.GetInstance().Context._settings.FirstOrDefault();
			DataBase db = Singleton.GetInstance().Context;
			Settings settings = db.GetSettings();
			Channel[] channel = db.GetChannels();
			foreach (Channel channels in channel)
			{
				TMessage tmessage = db.GetTMessage_ForChannel(channels);
				if (tmessage == null)
				{
					User[] ThisUsers = db.GetUsers();
					foreach (User users in ThisUsers)
					{
						db.Add_TMessage(new TMessage() { user = users, Post = settings.CountPost, dateTime = System.DateTime.Today, channel = channels });
					}
				}
			}
			db.Save();
		}
	}

	internal class FIO : AbstractHandler
	{
		internal async void SendMessage(TelegramBotClient botClient, Message _message)
		{
			DataBase db = Singleton.GetInstance().Context;
			try
			{
				User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return;
				user.FIO = _message.Text;

				ReplyButton replyButton = new ReplyButton();
				Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup temp = replyButton.SetNumber();
				try
				{
					await botClient.DeleteMessageAsync(_message.From.Id, _message.MessageId - 1);
				}
				catch{}

				await botClient.SendTextMessageAsync(_message.From.Id, "Поделитесь номером телефона при помощью кнопки в нижнем меню: ", replyMarkup: temp);

				user.Chain = 2;
				await Task.Delay(500);

				botClient.DeleteMessage(_message.From.Id, _message.MessageId, "Невозможно удалить сообщение класс FIO");
			}
			catch { }

			try
			{
				Settings settings = db.GetSettings();
				if (settings == null)
				{
					db.Add_Settings(new Settings() { CountPost = 3, ProcentMessage = 0.75, PasswordAdmin = "UBC_Admin", AddUser = 3 });
				}
			}
			catch { }

			db.Save();
		}

		public override System.Object Handle(System.Int32 request, TelegramBotClient botClient, System.Object message)
		{
			if (request == 1)
			{
				Message _message = message as Message;
				if (_message.Chat.Type == Telegram.Bot.Types.Enums.ChatType.Private)
				{
					SendMessage(botClient, _message);
				}

				return null;
			}
			else
			{
				return base.Handle(request, botClient, message);
			}
		}
	}

	internal class Number : AbstractHandler
	{
		private void SendMessage(TelegramBotClient botClient, Message _message)
		{
			DataBase db = Singleton.GetInstance().Context;
			User user = db.GetUser( _message.From.Id);

			botClient.DeleteMessage(_message.From.Id, _message.MessageId - 1, "104");
			botClient.DeleteMessage(_message.From.Id, _message.MessageId, "104");
			botClient.DeleteMessage(_message.From.Id, user.MessageID, "109");

			if (_message.Contact != null)
			{
				user.Number = _message.Contact.PhoneNumber;
				user.Chain = 22;
				InlineButton inlineButton = new InlineButton();
				botClient.SendText(_message.From.Id, "Поздравляем вы зарегистрировались в системе UBC, теперь вы можете продолжить взаимодествовать с чатами UBC\n" + InfoUser.Info(user), user, replyMarkup: inlineButton.Accaunt);

				Settings settings = db.GetSettings();
				Channel[] channels1 = db.GetChannels();
				foreach (Channel channels in channels1)
				{
					db.Add_TMessage(new TMessage() { user = user, Post = settings.CountPost, dateTime = System.DateTime.Today, channel = channels });
				}

				db.Save();
				return;
			}

			ReplyButton reply = new ReplyButton();

			botClient.SendText(_message.Chat.Id, "Вы можете только отправить номер теленфона через кнопку поделиться номером!", user, replyMarkup: reply.SetNumber());

			db.Save();
		}

		public override System.Object Handle(System.Int32 request, TelegramBotClient botClient, System.Object message)
		{
			if (request == 2)
			{
				Message _message = message as Message;
				if (_message.Chat.Type == Telegram.Bot.Types.Enums.ChatType.Private)
				{
					SendMessage(botClient, _message);
				}

				return null;
			}
			else
			{
				return base.Handle(request, botClient, message);
			}
		}
	}

	internal class Admin : AbstractHandler
	{
		private async void SendMessage(TelegramBotClient botClient, Message _message)
		{
			DataBase db = Singleton.GetInstance().Context;
			User user =  db.GetUser(_message.From.Id);
			InlineButton inlineButton = new InlineButton();
			try
			{
				botClient.DeleteMessage(_message.Chat.Id, _message.MessageId, "169");
				botClient.DeleteMessage(_message.Chat.Id, user.MessageID, "170");

				if (_message.Text == CommandText.PasswordAdmin)
				{
					Settings settings = db.GetSettings();
					if (settings == null)
					{
						db.Add_Settings(new Settings() { CountPost = 3, ProcentMessage = 0.75, PasswordAdmin = "UBC_Admin" });

						botClient.SendText(_message.From.Id, "Вы вошли как администратор!\nВведите постоянный пароль для администраторов: ", user);
						user.Chain = 23;
						user.ChatID = _message.From.Id;
						user.IsAdmin = 2;
						db.Save();
						return;
					}
					else
					{
						botClient.SendText(_message.From.Id, "Вы вошли как администратор! ", user, replyMarkup: inlineButton.AdminAccaunt);

						user.Chain = (System.Int32)SetChain.MessageUserInBot;
					}
					user.ChatID = _message.From.Id;
					user.IsAdmin = 2;
					db.Save();
					return;
				}
				else
				{
					CommandText.PasswordAdmin = null;
					System.Console.WriteLine("Пароль введен не верно!\nПароль удален!");
					user.IsAdmin = 0;
				}

				db.Save();
			}
			catch (System.Exception ex)
			{
				Log.Logging("Admin : AbstractHandler: " + ex);
				return;
			}
		}

		public override System.Object Handle(System.Int32 request, TelegramBotClient botClient, System.Object message)
		{
			if (request == 22)
			{
				Message _message = message as Message;

				SendMessage(botClient, _message);

				return null;
			}
			else
			{
				return base.Handle(request, botClient, message);
			}
		}
	}

	internal class SetAdminPassword : AbstractHandler
	{
		public override System.Object Handle(System.Int32 request, TelegramBotClient botClient, System.Object message)
		{
			DataBase db = Singleton.GetInstance().Context;
			Message _message = message as Message;

			botClient.DeleteMessage(_message.Chat.Id, _message.MessageId, "229");

			if (request == 23)
			{
				User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return null;

				botClient.DeleteMessage(_message.Chat.Id, _message.MessageId, "237");

				if (user.IsAdmin == 3)
				{
					db.Add_Settings(new Settings() { PasswordAdmin = _message.Text });
				}
				botClient.DeleteMessage(_message.Chat.Id, user.MessageID, "243");

				user.Chain = (System.Int32)SetChain.MessageUserInBot;
				db.Save();
				return user;
			}
			else
			{
				return base.Handle(request, botClient, message);
			}
		}
	}

	internal class ChaingeFIO : AbstractHandler
	{
		public override System.Object Handle(System.Int32 request, TelegramBotClient botClient, System.Object message)
		{
			DataBase db = Singleton.GetInstance().Context;
			if (request == 3)
			{
				Message _message = message as Message;
				InlineButton inlineButton = new InlineButton();

				User user =db.GetUser(_message.From.Id);
				user.FIO = _message.Text;
				user.Chain = (System.Int32)SetChain.MessageUserInBot;
				db.Save();

				System.String temp = InfoUser.Info(user);

				botClient.DeleteMessage(_message.Chat.Id, _message.MessageId, "274");

				try
				{
					botClient.EditMessageTextAsync(_message.Chat.Id, user.MessageID, temp, Telegram.Bot.Types.Enums.ParseMode.Html, replyMarkup: inlineButton.Accaunt);
				}
				catch(System.Exception ex)
				{
					Log.Logging("собощение не было найдено - ChaingeFIO - Command: " + ex);
				}

				return user;
			}
			else
			{
				return base.Handle(request, botClient, message);
			}
		}
	}

	internal class ChaingeNumber : AbstractHandler
	{
		public override System.Object Handle(System.Int32 request, TelegramBotClient botClient, System.Object message)
		{
			DataBase db = Singleton.GetInstance().Context;
			if (request == 4)
			{
				Message _message = message as Message;

				InlineButton inlineButton = new InlineButton();

				const System.String myReg1 = @"((\+38|8|\+3|\+ )[ ]?)?([(]?\d{3}[)]?[\- ]?)?(\d[ -]?){10,13}";
				if (Regex.IsMatch(_message.Text.Replace(" ", "").Replace("(", "").Replace(")", ""), myReg1, RegexOptions.IgnoreCase))
				{
					foreach (User tempUsers in Singleton.GetInstance().Context._users)
					{
						if (tempUsers.Number == _message.Text.Replace(" ", "").Replace("(", "").Replace(")", ""))
						{
							botClient.SendText(_message.Chat.Id, "Номер введен не верный или уже такой номер используется!", replyMarkup: inlineButton.BackToAccauntMenu);
							return null;
						}
					}
					User user =db.GetUser(_message.From.Id);

					user.Number = _message.Text.Replace(" ", "").Replace("(", "").Replace(")", "");
					user.Chain = (System.Int32)SetChain.MessageUserInBot;
					db.Save();
					System.String temp = "🚪Личный кабинет🚪\n🆔: "
						+ user.ID + "\n🖌ФИО: "
						+ user.FIO + "\n📱Номер: "
						+ user.Number + "\n⭐️Рейтинг: "
						+ user.SumRating;

					botClient.DeleteMessage(_message.Chat.Id, _message.MessageId, "326");
					botClient.EditMessageTextAsync(_message.Chat.Id, user.MessageID, temp, replyMarkup: inlineButton.Accaunt);
					return user;
				}
				botClient.SendText(_message.Chat.Id, "Номер введен не верный или уже такой номер используется!", replyMarkup: inlineButton.BackToAccauntMenu);

				return null;
			}
			else
			{
				return base.Handle(request, botClient, message);
			}
		}
	}

	internal class SearchIDUser : AbstractHandler
	{
		private User SearchUser(Message _message, User user = null)
		{
			DataBase db = Singleton.GetInstance().Context;
			if (_message.ForwardFrom != null)
			{
				if (user != null)
				{
					ChangeUser(user, _message);
				}
				return db.GetUser(_message.ForwardFrom.Id);
			}
			else if (_message.Text[0] == '@')
			{
				return db.GetUser(_message.Text.Replace("@", ""));
			}
			else
			{
				System.Int32 temp = 0;
				System.Int32.TryParse(_message.Text, out temp);
				return db.GetUser(temp);
			}
		}

		private void ChangeUser(User user, Message _message)
		{
			DataBase db = Singleton.GetInstance().Context;
			user.MessageComplaint = _message.Text;
			db.Save();
		}

		private async void SearchIDReviews(TelegramBotClient botClient, System.Object message, User user)
		{
			DataBase db = Singleton.GetInstance().Context;
			Message _message = message as Message;
			InlineButton inlineButton = new InlineButton();

			User userTwo = SearchUser(_message, user);

			botClient.DeleteMessage(_message.Chat.Id, _message.MessageId, "364");

			try
			{
				if (userTwo != null)
				{
					System.String temp = InfoUser.Search(userTwo, user);

					if (user.CountReviews > 0 || user.IsAdmin > 0)
					{
						botClient.EditMessage(_message.From.Id, user.MessageID, temp, "374", user,
							replyMarkup: inlineButton.InteractionUsers(userTwo, db._featuredUserNews.Any(p => p.UserId == userTwo.ID && p.UserWhoAddedId == user.ID), db.GetFeaturedUsers(user, userTwo), isAdmin: true));
					}
					else
					{
						botClient.EditMessage(_message.From.Id,
							user.MessageID,
							temp, "381", user,
							replyMarkup: inlineButton.InteractionUsersNoReviews);
					}
				}
				else
				{
					botClient.EditMessage(_message.From.Id,
						user.MessageID, "Мы не нашли данного участника.\nВведите ID или перешлите сообщение еще раз: ", "388", user, replyMarkup: inlineButton.BackToAccauntMenu);
					return;
				}
			}
			catch (System.Exception ex)
			{
				Log.Logging("SearchIDUser : AbstractHandler: " + ex);
				return;
			}

			user.Chain = (System.Int32)SetChain.MessageUserInBot;
			user.IDRecipient = userTwo.ID;
			db.Save();
		}

		private async void BanID(TelegramBotClient botClient, System.Object message, User user)
		{
			DataBase db = Singleton.GetInstance().Context;
			Message _message = message as Message;
			InlineButton inlineButton = new InlineButton();

			User userTwo = SearchUser(_message);

			botClient.DeleteMessage(_message.Chat.Id, _message.MessageId, "413");

			try
			{
				if (userTwo != null)
				{
					try
					{
						db.Add_TempBase(new TempBase() { ID = user.ID, IDTwo = userTwo.ID });
					}
					catch(System.Exception ex)
					{
						Log.Logging("Эти данные уже были добавлены в базу. " + ex);
					}
					System.String temp = InfoUser.Info(userTwo, user);

					botClient.EditMessage(_message.From.Id, user.MessageID,
						temp + "\nВведите количество дней для бана!", "428", user, replyMarkup: inlineButton.BackToAccauntMenu);
					user.Chain = 1052;
					user.IDRecipient = userTwo.ID;
				}
				else
				{
					botClient.EditMessage(_message.From.Id,
						user.MessageID, "Мы не нашли данного участника.\nВведите ID еще раз: ", "345", user, replyMarkup: inlineButton.BackToAccauntMenu);
					return;
				}
				db.Save();
			}
			catch (System.Exception ex)
			{
				Log.Logging("SearchIDUser : AbstractHandler: " + ex);
				return;
			}
		}

		private async void UnBanID(TelegramBotClient botClient, System.Object message, User user)
		{
			DataBase db = Singleton.GetInstance().Context;
			Message _message = message as Message;
			InlineButton inlineButton = new InlineButton();

			User userTwo = SearchUser(_message);

			botClient.DeleteMessage(_message.Chat.Id, _message.MessageId, "459");

			try
			{
				if (userTwo != null)
				{
					userTwo.BanDate = System.DateTime.Now;
					IsUnBan.ThisUnBan(botClient, userTwo);

					System.String temp = InfoUser.Info(userTwo, user);

					botClient.EditMessage(_message.From.Id,
						user.MessageID,
						temp + "\nУчастник разбанен!", "469", user, replyMarkup: inlineButton.BackToAccauntMenu);
				}
				else
				{
					botClient.EditMessage(_message.From.Id,
						user.MessageID, "Мы не нашли данного участника.\nВведите ID еще раз: ", "476", user, replyMarkup: inlineButton.BackToAccauntMenu);
					return;
				}
			}
			catch (System.Exception ex)
			{
				Log.Logging("SearchIDUser, void UnBanID() : AbstractHandler: " + ex);
				return;
			}
		}

		public override System.Object Handle(System.Int32 request, TelegramBotClient botClient, System.Object message)
		{
			DataBase db = Singleton.GetInstance().Context;
			if (request == 50) // Для отзывов
			{
				Message _message = message as Message;

				InlineButton inlineButton = new InlineButton();

				User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return null;

				SearchIDReviews(botClient, message, user);

				return user;
			}
			else if (request == 1050) // Бан
			{
				Message _message = message as Message;

				InlineButton inlineButton = new InlineButton();

				User user =db.GetUser(_message.From.Id);

				BanID(botClient, message, user);

				return user;
			}
			else if (request == 1150) // Разбан
			{
				Message _message = message as Message;

				InlineButton inlineButton = new InlineButton();

				User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return null;

				UnBanID(botClient, message, user);

				return user;
			}
			else
			{
				return base.Handle(request, botClient, message);
			}
		}
	}

	internal class SearchNumberUser : AbstractHandler
	{
		private User SearchUser(Message _message)
		{
			DataBase db = Singleton.GetInstance().Context;
			if (_message.ForwardFrom != null)
			{
				return db.GetUser(_message.From.Id);
			}
			else
			{
				return db.GetUserForNumber(_message.Text.Replace(" ", "").Replace("(", "").Replace(")", "").Replace("+", ""));
			}
		}

		private async void SearchIDReviews(TelegramBotClient botClient, System.Object message, User user)
		{
			Message _message = message as Message;
			InlineButton inlineButton = new InlineButton();
			User userTwo = SearchUser(_message);

			botClient.DeleteMessage(_message.Chat.Id, _message.MessageId, "558");
			DataBase db = Singleton.GetInstance().Context;
			try
			{
				if (userTwo != null)
				{
					System.String temp = InfoUser.Search(userTwo, user);

					if (user.CountReviews > 0 || user.IsAdmin > 0)
					{
						botClient.EditMessage(_message.From.Id,
							user.MessageID,
							temp, "569", user,
							replyMarkup: inlineButton.InteractionUsers(userTwo, db._featuredUserNews.Any(p => p.UserId == userTwo.ID && p.UserWhoAddedId == user.ID), db.GetFeaturedUsers(user, userTwo), isAdmin: true));
					}
					else
					{
						botClient.EditMessage(_message.From.Id,
							user.MessageID,
							temp, "576", user,
							replyMarkup: inlineButton.InteractionUsersNoReviews);
					}
				}
				else
				{
					botClient.EditMessage(_message.From.Id,
						user.MessageID, "Мы не нашли данного участника.\nВведите номер телефона еще раз: ", "583", user, replyMarkup: inlineButton.BackToAccauntMenu);
					return;
				}
			}
			catch (System.Exception ex)
			{
				Log.Logging("SearchNumberUser : AbstractHandler: " + ex);
				return;
			}

			user.Chain = (System.Int32)SetChain.MessageUserInBot;
			user.IDRecipient = userTwo.ID;
			db.Save();
		}

		private async void BanNumber(TelegramBotClient botClient, System.Object message, User user)
		{
			DataBase db = Singleton.GetInstance().Context;
			Message _message = message as Message;
			InlineButton inlineButton = new InlineButton();

			User userTwo = SearchUser(_message);

			botClient.DeleteMessage(_message.Chat.Id, _message.MessageId, "608");

			try
			{
				if (userTwo != null)
				{
					db.Add_TempBase(new TempBase() { ID = user.ID, IDTwo = userTwo.ID });

					System.String temp = InfoUser.Info(userTwo, user);

					botClient.EditMessage(_message.From.Id,
						user.MessageID,
						temp + "\nВведите количество дней для бана!", "619", user, replyMarkup: inlineButton.BackToAccauntMenu);
				}
				else
				{
					botClient.EditMessage(_message.From.Id,
						user.MessageID, "Мы не нашли данного участника.\nВведите номер еще раз: ", "624", user, replyMarkup: inlineButton.BackToAccauntMenu);
					return;
				}
			}
			catch (System.Exception ex)
			{
				Log.Logging("SearchIDUser : AbstractHandler: " + ex);
				return;
			}

			user.Chain = 1052;
			user.IDRecipient = userTwo.ID;
			db.Save();
		}

		private async void UnBanNumber(TelegramBotClient botClient, System.Object message, User user)
		{
			Message _message = message as Message;
			InlineButton inlineButton = new InlineButton();

			User userTwo = SearchUser(_message);

			botClient.DeleteMessage(_message.Chat.Id, _message.MessageId, "648");

			try
			{
				if (userTwo != null)
				{
					userTwo.BanDate = System.DateTime.Now;
					IsUnBan.ThisUnBan(botClient, user);

					System.String temp = InfoUser.Info(userTwo, user);

					botClient.EditMessage(_message.From.Id,
						user.MessageID,
						temp + "\nУчастник разбанен!", "661", user, replyMarkup: inlineButton.BackToAccauntMenu);
				}
				else
				{
					botClient.EditMessage(_message.From.Id,
						user.MessageID, "Мы не нашли данного участника.\nВведите номер еще раз: ", "667", user, replyMarkup: inlineButton.BackToAccauntMenu);
					return;
				}
			}
			catch (System.Exception ex)
			{
				Log.Logging("SearchIDUser, void UnBanID() : AbstractHandler: " + ex);
				return;
			}
		}

		public override System.Object Handle(System.Int32 request, TelegramBotClient botClient, System.Object message)
		{
			DataBase db = Singleton.GetInstance().Context;
			if (request == 51)
			{
				Message _message = message as Message;

				User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return null;

				SearchIDReviews(botClient, message, user);

				return user;
			}
			else if (request == 1051) // Бан
			{
				Message _message = message as Message;

				InlineButton inlineButton = new InlineButton();

				User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return null;
				BanNumber(botClient, message, user);

				return user;
			}
			else if (request == 1151) // Разбан
			{
				Message _message = message as Message;

				InlineButton inlineButton = new InlineButton();

				User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return null;
				UnBanNumber(botClient, message, user);

				return user;
			}
			else
			{
				return base.Handle(request, botClient, message);
			}
		}
	}

	internal class SetReviewsName : AbstractHandler
	{
		public override System.Object Handle(System.Int32 request, TelegramBotClient botClient, System.Object message)
		{
			DataBase db = Singleton.GetInstance().Context;
			if (request == 6)
			{
				Message _message = message as Message;
				InlineButton inlineButton = new InlineButton();

				User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return null;

				if (user.ID != user.IDRecipient)
				{
					System.String temp = _message.Text;

					db.Add_Reviews(new Reviews() { IDSender = user.ID, IDRecipient = user.IDRecipient, Description = temp, Name = user.FIO, TheEnd = false });

					botClient.DeleteMessage(_message.Chat.Id, _message.MessageId, "741");

					botClient.EditMessageTextAsync(_message.Chat.Id, user.MessageID, "Поставьте рейтинг: ", replyMarkup: inlineButton.SetRating);
				}
				else
				{
					botClient.DeleteMessage(_message.Chat.Id, _message.MessageId, "748");
					if (user.IsAdmin > 0)
					{
						botClient.EditMessageTextAsync(_message.Chat.Id, user.MessageID, "Вы не можете поставить отзыв самому себе!", replyMarkup: inlineButton.AdminAccaunt);
					}
					else
					{
						botClient.EditMessageTextAsync(_message.Chat.Id, user.MessageID, "Вы не можете поставить отзыв самому себе!", replyMarkup: inlineButton.Accaunt);
					}
				}
				user.Chain = (System.Int32)SetChain.MessageUserInBot;
				user.CountReviews -= 1;
				db.Save();
				return user;
			}
			else
			{
				return base.Handle(request, botClient, message);
			}
		}
	}

	internal class ChangeReviewsName : AbstractHandler
	{
		private DataBase db = Singleton.GetInstance().Context;

		public override System.Object Handle(System.Int32 request, TelegramBotClient botClient, System.Object message)
		{
			if (request == 62)
			{
				Message _message = message as Message;
				InlineButton inlineButton = new InlineButton();

				User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return null;
				Reviews reviews = db.GetReviews(_message.From.Id);
				User userTwo = db.GetUser(reviews.IDRecipient);

				reviews.Name = user.FIO;
				reviews.Description = _message.Text;
				reviews.TheEnd = true;
				user.Chain = (System.Int32)SetChain.MessageUserInBot;

				botClient.DeleteMessage(_message.Chat.Id, _message.MessageId, "786");

				botClient.EditMessageTextAsync(_message.Chat.Id, user.MessageID, "Вы оставили отзыв о " + userTwo.FIO + "\n" + reviews.Description + "\nРейтинг: " + System.Math.Round(userTwo.SumRating, 2), replyMarkup: inlineButton.Accaunt);
				Singleton.GetInstance().Context.Save();

				return user;
			}
			else
			{
				return base.Handle(request, botClient, message);
			}
		}
	}

	internal class SetSendComplaint : AbstractHandler
	{
		public override System.Object Handle(System.Int32 request, TelegramBotClient botClient, System.Object message)
		{
			DataBase db = Singleton.GetInstance().Context;
			if (request == 7)
			{
				Message _message = message as Message;
				InlineButton inlineButton = new InlineButton();

				User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return null;
				User userTwo = db.GetUser(user.IDRecipient);
				Settings settings = db.GetSettings();

				user.Chain = (System.Int32)SetChain.MessageUserInBot;

				if (user.ID != user.IDRecipient)
				{
					db.Add_Complaint(new Complaint() { IDRecipient = user.IDRecipient, Description = _message.Text, Name = user.FIO, TheEnd = true });
				}
				else
				{
					if (user.IsAdmin > 0)
					{
						botClient.EditMessageTextAsync(_message.Chat.Id, user.MessageID, "Вы не можете отправить жалобу самому себе!", replyMarkup: inlineButton.AdminAccaunt);
						db.Save();
						return null;
					}
					else
					{
						botClient.EditMessageTextAsync(_message.Chat.Id, user.MessageID, "Вы не можете отправить жалобу самому себе!", replyMarkup: inlineButton.Accaunt);
						db.Save();
						return null;
					}
				}

				if (user.IsAdmin == 0)
				{
					botClient.EditMessageTextAsync(_message.Chat.Id, user.MessageID, "Жалоба отправена!", replyMarkup: inlineButton.Accaunt);
				}
				else
				{
					botClient.EditMessageTextAsync(_message.Chat.Id, user.MessageID, "Жалоба отправена!", replyMarkup: inlineButton.AdminAccaunt);
				}

				//new Thread(() => SendAdmin(botClient, message, _message.Text, userTwo)).Start();

				Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup temp = inlineButton.AdminPanel(message, userTwo.ID, _message.Text);
				System.String Text = "Жалоба:\nID: " + userTwo.ID + "\nФИО: " + userTwo.FIO + "\nНомер: " + userTwo.Number + "\n\nТекст жалобы:\n" + _message.Text;
				Text += user.MessageComplaint != "" ? $"\n\nТекст сообщения: {user.MessageComplaint}" : "";

				ChangeUser(db, user);

				botClient.SendText(settings.ChannelAdmin, Text, replyMarkup: temp);
				botClient.DeleteMessage(_message.Chat.Id, _message.MessageId, "871");

				db.Save();
				return user;
			}
			else
			{
				return base.Handle(request, botClient, message);
			}
		}

		private void ChangeUser(DataBase db, User user)
		{
			user.MessageComplaint = "";
			db.Save();
		}
	}

	internal class SetSendAppeal : AbstractHandler
	{
		public override System.Object Handle(System.Int32 request, TelegramBotClient botClient, System.Object message)
		{
			DataBase db = Singleton.GetInstance().Context;
			if (request == 8)
			{
				Message _message = message as Message;
				InlineButton inlineButton = new InlineButton();

				User user = Singleton.GetInstance().Context._users.Where(p => p.ID == _message.From.Id).FirstOrDefault();
				db.Add_Ap(new Appeal() { IDSender = user.ID, Description = _message.Text, Name = user.FIO });
				user.Chain = (System.Int32)SetChain.MessageUserInBot;

				botClient.EditMessageTextAsync(_message.Chat.Id, user.MessageID, "Апелляция отправена!", replyMarkup: inlineButton.BanAccaunt);
				db.Save();

				Settings setting = db.GetSettings();
				Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup temp = inlineButton.AdminPanelAppeal(message, user.ID, _message.Text);
				botClient.SendText(setting.ChannelAdmin, "Апелляция!\n" + "ID: " + user.ID + "\nФИО: " + user.FIO + "\nНомер: " + user.Number + "\n" + _message.Text, replyMarkup: temp);
				botClient.ForwardMessageAsync(setting.ChannelAdmin, setting.ChannelAdmin, user.MessageBan);
				botClient.DeleteMessage(_message.Chat.Id, _message.MessageId, "903");

				return user;
			}
			else
			{
				return base.Handle(request, botClient, message);
			}
		}
	}

	internal class Ban : AbstractHandler
	{
		public override System.Object Handle(System.Int32 request, TelegramBotClient botClient, System.Object message)
		{
			DataBase db = Singleton.GetInstance().Context;
			Message _message = message as Message;
			if (request == 1052)
			{
				InlineButton inlineButton = new InlineButton();
				try
				{
					User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return null;
					TempBase tempBase = db.GetTempBase(user.ID);
					User userTwo = db.GetUser(tempBase.IDTwo);
					Settings settings = db.GetSettings();
					if (userTwo.IsAdmin <= 1)
					{
						if (userTwo.BanDate.Date < System.DateTime.Today)
						{
							userTwo.BanDate = System.DateTime.Now;
						}
						userTwo.BanDate = userTwo.BanDate.AddDays(System.Convert.ToInt32(_message.Text));
						userTwo.BanDescript = "Вы были забанены администратором группы!";
						IsBanUser.ThisBan(botClient, _message, userTwo, settings);

						user.Chain = (System.Int32)SetChain.MessageUserInBot;

						botClient.DeleteMessage(_message.Chat.Id, _message.MessageId, "940");
						botClient.DeleteMessage(_message.Chat.Id, user.MessageID, "941");
						botClient.SendText(_message.Chat.Id, InfoUser.Info(user), user, replyMarkup: inlineButton.AdminAccaunt);

						System.String text = "Пользователь " + userTwo.FIO + "\nID: " + userTwo.ID + "\nНомер: " + userTwo.Number + "\nБыл забанен администратором " + user.IsAdmin + " урованя " + user.FIO;

						botClient.SendText(settings.ChannelAdmin, text, userTwo, true);
					}
					else
					{
						user.IsAdmin = 0;
						user.Chain = (System.Int32)SetChain.MessageUserInBot;
						if (user.BanDate.Date < System.DateTime.Today)
						{
							user.BanDate = System.DateTime.Now;
						}
						user.BanDate = user.BanDate.AddDays(System.Convert.ToInt32(30));
						IsBanUser.ThisBan(botClient, _message, user, settings);

						Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup temp = inlineButton.AdminPanelAppeal(message, user.ID, _message.Text);
						botClient.SendText(settings.ChannelAdmin, "Админ " + user.FIO + "\nID: " + user.ID + "\nПопытался заблокировать администратора!\nС данного администратора снята админка, так же он был забанен во всех чатах! Если бан был выдан случайно пропишите /UbBan " + user.ID, user, true, replyMarkup: temp);

						botClient.EditMessageTextAsync(_message.Chat.Id, user.MessageID, "Вы попытались заблокировать администратора!", replyMarkup: inlineButton.BackToAccauntMenu);
					}
					db.RemoveTempBase(tempBase);

					db.Save();

					return user;
				}
				catch (System.Exception ex)
				{
					Log.Logging("Ban : AbstractHandler: " + ex);
					TempBase tempBase = Singleton.GetInstance().Context._tempBase.Where(p => p.ID == _message.From.Id).FirstOrDefault();
					if (tempBase != null)
					{
						db.RemoveTempBase(tempBase);
						db.Save();
					}

					return null;
				}
			}
			else
			{
				return base.Handle(request, botClient, message);
			}
		}
	}

	internal class KickUser : AbstractHandler
	{
		public override System.Object Handle(System.Int32 request, TelegramBotClient botClient, System.Object message)
		{
			DataBase db = Singleton.GetInstance().Context;
			if (request == 9)
			{
				Message _message = message as Message;
				try
				{
					User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return null;
					User userTwo = db.GetUser(_message.ForwardFrom.Id);

					IsKick.ThisKick(botClient, userTwo);
					botClient.DeleteMessage(_message.From.Id, _message.MessageId, "1008");

					Settings setting = db.GetSettings();
					System.String temp = "Администратор " + user.IsAdmin + " уровня: @" + user.Username + " кикнул пользователя!" + "\nID: " + userTwo.ID + "\nФИО: " + userTwo.FIO + "\nНомер: " + user.Number;

					botClient.SendText(setting.ChannelAdmin, temp, user, true);

					user.Chain = (System.Int32)SetChain.MessageUserInBot;
					db.Save();
					return user;
				}
				catch (System.Exception ex)
				{
					Log.Logging("Ban : AbstractHandler: " + ex);

					return null;
				}
			}
			else
			{
				return base.Handle(request, botClient, message);
			}
		}
	}

	internal class AdminAdd : AbstractHandler
	{
		public override System.Object Handle(System.Int32 request, TelegramBotClient botClient, System.Object message)
		{
			DataBase db = Singleton.GetInstance().Context;
			if (request == 1000)
			{
				Message _message = message as Message;
				InlineButton inlineButton = new InlineButton();
				try
				{
					User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return null;
					User userTwo = null;
					if (_message.ForwardFrom == null)
					{
						userTwo = db.GetUser(Convert.ToInt32(_message.Text));
					}
					else
					{
						userTwo = db.GetUser(_message.ForwardFrom.Id);
					}

					if (userTwo != null)
					{
						if (user.IsAdmin == 2)
						{
							userTwo.IsAdmin = 1;

							botClient.EditMessageTextAsync(_message.From.Id, user.MessageID, "Вы назначили нового администратора!", replyMarkup: inlineButton.AdminAccaunt);	
						}
						else if(user.IsAdmin == 3)
						{
							botClient.EditMessageTextAsync(_message.From.Id, user.MessageID, "Панель назначения", replyMarkup: Advertising.InlineButton.AdminPanelLvl3(userTwo));
						}
						user.Chain = (System.Int32)SetChain.MessageUserInBot;
						db.Save();
					}
					else
					{
						botClient.EditMessageTextAsync(_message.From.Id, user.MessageID, "Такого пользователя нет в база данных!", replyMarkup: inlineButton.AdminAccaunt);
					}
					return user;
				}
				catch (System.Exception ex)
				{
					Log.Logging("Ban : AbstractHandler: " + ex);

					return null;
				}
			}
			else
			{
				return base.Handle(request, botClient, message);
			}
		}
	}

	internal class AdminDelete : AbstractHandler
	{
		private void ChangeAdmin(User user, User userTwo, TelegramBotClient botClient, Message _message, InlineButton inlineButton)
		{
			if (user.IsAdmin == 3)
			{
				botClient.EditMessageTextAsync(_message.From.Id, user.MessageID, "Панель назначения",
				                               replyMarkup: Advertising.InlineButton.AdminPanelLvl3(userTwo));
			}
			else
			{
				botClient.EditMessageTextAsync(_message.From.Id, user.MessageID, "Вы сняли с должности администратора!", replyMarkup: inlineButton.AdminAccaunt);	
			}

			user.Chain = (System.Int32)SetChain.MessageUserInBot;
			DataBase db =  Singleton.GetInstance().Context;
			db.Save();
		}
		public override System.Object Handle(System.Int32 request, TelegramBotClient botClient, System.Object message)
		{
			DataBase db = Singleton.GetInstance().Context;
			if (request == 1001)
			{
				Message _message = message as Message;
				InlineButton inlineButton = new InlineButton();
				try
				{
					User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return null;
					User userTwo = null;
					if (_message.ForwardFrom == null)
					{
						userTwo = db.GetUser(Convert.ToInt32(_message.Text));
					}
					else
					{
						userTwo = db.GetUser(_message.ForwardFrom.Id);
					}
					if (userTwo != null)
					{
						Settings settings = db.GetSettings();
						if (user.IsAdmin == 3)
						{
							ChangeAdmin(user, userTwo, botClient, _message, inlineButton);
						}
						else if (userTwo.IsAdmin != 2 && userTwo.IsAdmin != 3)
						{
							ChangeAdmin(user, userTwo, botClient, _message, inlineButton);

							botClient.EditMessageTextAsync(settings.ChannelAdmin, user.MessageID, $"Администратор {user.IsAdmin} лвл  - @{user.Username} - снял администратора {userTwo.IsAdmin} лвл -  @{userTwo.Username} с должности ");
						}
						else
						{
							botClient.EditMessageTextAsync(_message.From.Id, user.MessageID, "Вы не моежете снять даного админисратора с должности!", replyMarkup: inlineButton.AdminAccaunt);
							botClient.EditMessageTextAsync(settings.ChannelAdmin, user.MessageID, $"Администратор {user.IsAdmin} лвл  - @{user.Username} - попытался снять администратора {userTwo.IsAdmin} лвл -  @{userTwo.Username} с должности ");
						}
					}
					else
					{
						botClient.EditMessageTextAsync(_message.From.Id, user.MessageID, "Такого пользователя нет в база данных!", replyMarkup: inlineButton.AdminAccaunt);
					}
					return user;
				}
				catch (System.Exception ex)
				{
					Log.Logging("Ban : AbstractHandler: " + ex);

					return null;
				}
			}
			else
			{
				return base.Handle(request, botClient, message);
			}
		}
	}

	internal class PostCount : AbstractHandler
	{
		private void CountPost(DataBase db, Settings settings)
		{
			TMessage[] messages = db.GetTMessages();
			Channel[] channels = db.GetChannels();

			foreach (TMessage item in messages)
			{
				item.Post = settings.CountPost;
			}

			foreach (Channel item in channels)
			{
				item.PostCount = settings.CountPost;
			}
			db.Save();
		}

		public override System.Object Handle(System.Int32 request, TelegramBotClient botClient, System.Object message)
		{
			DataBase db = Singleton.GetInstance().Context;
			if (request == 1002)
			{
				Message _message = message as Message;
				InlineButton inlineButton = new InlineButton();
				try
				{
					User user = Singleton.GetInstance().Context._users.Where(p => p.ID == _message.From.Id).FirstOrDefault();
					if (user.IsAdmin >= 2)
					{
						Settings settings = db.GetSettings();

						settings.CountPost = System.Convert.ToInt32(_message.Text);

						botClient.EditMessageTextAsync(_message.From.Id, user.MessageID, "Количество постов заданно: " + settings.CountPost, replyMarkup: inlineButton.AdminAccaunt);

						user.Chain = (System.Int32)SetChain.MessageUserInBot;

						Task.Run(() => CountPost(db, settings));

						db.Save();
					}

					return user;
				}
				catch (System.Exception ex)
				{
					Log.Logging("PostCount : AbstractHandler: " + ex);

					return null;
				}
			}
			else
			{
				return base.Handle(request, botClient, message);
			}
		}
	}

	internal class WordAdd : AbstractHandler
	{
		public override System.Object Handle(System.Int32 request, TelegramBotClient botClient, System.Object message)
		{
			DataBase db = Singleton.GetInstance().Context;
			if (request == 1003)
			{
				Message _message = message as Message;
				InlineButton inlineButton = new InlineButton();
				System.String temp = "h";
				User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return null;
				try
				{
					if (user.IsAdmin >= 1)
					{
						Word word = Singleton.GetInstance().Context._words.Where(p => p.word == _message.Text.ToLower()).FirstOrDefault();
						if (word == null)
						{
							db.Add_Words(new Word() { word = _message.Text.Replace(" ", "").ToLower() });
							temp = "Слово добавлено: ";
							Task<Message> mes = botClient.EditMessageTextAsync(_message.From.Id, user.MessageID, temp + _message.Text, replyMarkup: inlineButton.BackToSettingAdmin);
							user.MessageID = mes.Result.MessageId;
						}
						else
						{
							temp = "Такое слово уже есть в базе: ";
							Task<Message> mes = botClient.EditMessageTextAsync(_message.From.Id, user.MessageID, temp + _message.Text, replyMarkup: inlineButton.BackToSettingAdmin);
							user.MessageID = mes.Result.MessageId;
						}

						user.Chain = (System.Int32)SetChain.MessageUserInBot;
						db.Save();
					}

					return user;
				}
				catch (System.Exception ex)
				{
					Log.Logging("Ban : AbstractHandler: " + ex);
					botClient.SendText(_message.From.Id, temp + _message.Text, user, replyMarkup: inlineButton.BackToSettingAdmin);
					return null;
				}
			}
			else
			{
				return base.Handle(request, botClient, message);
			}
		}
	}

	internal class AddUsers : AbstractHandler
	{
		public override System.Object Handle(System.Int32 request, TelegramBotClient botClient, System.Object message)
		{
			DataBase db = Singleton.GetInstance().Context;
			if (request == 1004)
			{
				Message _message = message as Message;
				InlineButton inlineButton = new InlineButton();
				try
				{
					User user = Singleton.GetInstance().Context._users.Where(p => p.ID == _message.From.Id).FirstOrDefault();
					if (user.IsAdmin >= 2)
					{
						Settings settings = db.GetSettings();

						settings.AddUser = System.Convert.ToInt32(_message.Text);

						botClient.EditMessageTextAsync(_message.From.Id, user.MessageID, "Кол-во человек которых нужно добавить чтобы писать в чате : " + _message.Text, replyMarkup: inlineButton.BackToSettingAdmin);

						user.Chain = (System.Int32)SetChain.MessageUserInBot;
						db.Save();
					}

					return user;
				}
				catch (System.Exception ex)
				{
					Log.Logging("Ban : AbstractHandler: " + ex);

					return null;
				}
			}
			else
			{
				return base.Handle(request, botClient, message);
			}
		}
	}

	internal class FludUser : AbstractHandler
	{
		public override System.Object Handle(System.Int32 request, TelegramBotClient botClient, System.Object message)
		{
			DataBase db = Singleton.GetInstance().Context;
			if (request == 1005)
			{
				Message _message = message as Message;
				InlineButton inlineButton = new InlineButton();
				try
				{
					User user = Singleton.GetInstance().Context._users.Where(p => p.ID == _message.From.Id).FirstOrDefault();
					if (user.IsAdmin > 0)
					{
						Settings settings = db.GetSettings();

						System.String[] words = _message.Text.Split(new System.Char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
						System.Int32 temp = System.Convert.ToInt32(words[0]);
						System.Int32 temp2;
						if (words[1] != null)
						{
							temp2 = System.Convert.ToInt32(words[1]);
							settings.Timer = System.DateTime.Now.AddHours(temp).AddMinutes(temp2);
						}
						else
						{
							settings.Timer = System.DateTime.Now.AddHours(temp);
						}

						botClient.EditMessageTextAsync(_message.From.Id, user.MessageID, "Флуд начат!\n Флуд закончится: " + settings.Timer, replyMarkup: inlineButton.AdminAccaunt);

						user.Chain = (System.Int32)SetChain.MessageUserInBot;
						db.Save();
					}

					return user;
				}
				catch (System.Exception ex)
				{
					Log.Logging("PostCount : AbstractHandler: " + ex);
					return null;
				}
			}
			else
			{
				return base.Handle(request, botClient, message);
			}
		}
	}

	internal class ChannelAdd : AbstractHandler
	{
		public override System.Object Handle(System.Int32 request, TelegramBotClient botClient, System.Object message)
		{
			DataBase db = Singleton.GetInstance().Context;
			if (request == 1006)
			{
				Message _message = message as Message;
				InlineButton inlineButton = new InlineButton();
				try
				{
					User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return null;
					if (user.IsAdmin >= 2)
					{
						if (_message.ForwardFromChat != null)
						{
							if (db.GetChannel(_message.ForwardFromChat.Id) == null)
							{
								db.Add_Channel(new Channel() { IDChannel = _message.ForwardFromChat.Id, ChannelName = _message.ForwardFromChat.Title, InviteLink = "@" + _message.ForwardFromChat.Username });

								botClient.EditMessage(_message.From.Id, user.MessageID, "Канал добавлен в базу: ", "1326", user, replyMarkup: inlineButton.AdminAccaunt);
							}
							else
							{
								botClient.EditMessage(_message.From.Id, user.MessageID, "Данный чат уже есть в базе данных!", "1326", user, replyMarkup: inlineButton.AdminAccaunt);
							}
						}
					}
					user.Chain = (System.Int32)SetChain.MessageUserInBot;
					db.Save();
					return user;
				}
				catch (System.Exception ex)
				{
					Log.Logging("PostCount : AbstractHandler: " + ex);

					return null;
				}
			}
			else
			{
				return base.Handle(request, botClient, message);
			}
		}
	}

	internal class NewAdmin2Lvl : AbstractHandler
	{
		private async void Start(TelegramBotClient botClient, System.Object message)
		{
			DataBase db = Singleton.GetInstance().Context;
			Message _message = message as Message;
			InlineButton inlineButton = new InlineButton();
			try
			{
				User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return;
				Settings settings = db.GetSettings();

				botClient.DeleteMessage(_message.Chat.Id, user.MessageID, "1358");
				if (_message.Text == settings.PasswordAdmin)
				{
					botClient.SendText(_message.From.Id, "Заявление на пост администратора 2-го уровня подана!", user, replyMarkup: inlineButton.BackToAccauntMenu);
					//user.IsAdmin = 2;
					User[] users = db.GetUsers();
					foreach (User user1 in users)
					{
						if (user1.IsAdmin == 3)
						{
							botClient.SendText(user1.ID, $"Заявление на пост администратора 2-го уровня!\nID:{user.ID}\nФИО: {user.FIO}", user1, replyMarkup: inlineButton.OrderAdmin(user));
						}
					}
				}
				else if (_message.Text == settings.PasswordAdmin + "_lvl3")
				{
					botClient.SendText(_message.From.Id, "Теперь вы администратор 3-го лвл!", user, replyMarkup: inlineButton.BackToAccauntMenu);
					user.IsAdmin = 3;
				}
				else
				{
					botClient.SendText(_message.From.Id, "Пароль введен не верно!", user, replyMarkup: inlineButton.BackToAccauntMenu);
				}

				user.Chain = (System.Int32)SetChain.MessageUserInBot;
				db.Save();
				return;
			}
			catch (System.Exception ex)
			{
				Log.Logging("NewAdmin2Lvl : AbstractHandler: " + ex);
				return;
			}
		}

		public override System.Object Handle(System.Int32 request, TelegramBotClient botClient, System.Object message)
		{
			if (request == 1008)
			{
				Start(botClient, message);
				return null;
			}
			else
			{
				return base.Handle(request, botClient, message);
			}
		}
	}

	internal class WordDelete : AbstractHandler
	{
		private async void Start(TelegramBotClient botClient, Message _message)
		{
			DataBase db = Singleton.GetInstance().Context;
			InlineButton inlineButton = new InlineButton();
			Word words = await Singleton.GetInstance().Context._words.FirstOrDefaultAsync(p => p.word == _message.Text.Replace(" ", "").ToLower());
			User user = await Singleton.GetInstance().Context._users.FirstOrDefaultAsync(p => p.ID == _message.From.Id);

			if (words != null)
			{
				db.RemoveWord(words);
				try
				{
					Message mes = await botClient.EditMessageTextAsync(_message.From.Id, user.MessageID, "Слово удалено!", replyMarkup: inlineButton.BackToSettingAdmin);
					user.MessageID = mes.MessageId;
					db.Save();
				}
				catch (System.Exception)
				{
					//System.Console.WriteLine("WordDelete : AbstractHandler: " + ex);
					botClient.SendText(_message.From.Id, "Слово удалено!", user, replyMarkup: inlineButton.BackToSettingAdmin);
					return;
				}
			}
			else
			{
				try
				{
					botClient.EditMessage(_message.From.Id, user.MessageID, "Слово не найдено!", "1428", user, replyMarkup:
					inlineButton.BackToSettingAdmin);

					db.Save();
				}
				catch (System.Exception ex)
				{
					Log.Logging("WordDelete : AbstractHandler: " + ex);
					return;
				}
			}
			user.Chain = (System.Int32)SetChain.MessageUserInBot;
			db.Save();
		}

		public override System.Object Handle(System.Int32 request, TelegramBotClient botClient, System.Object message)
		{
			Message _message = message as Message;
			if (request == 1009)
			{
				Start(botClient, _message);
				return null;
			}
			else
			{
				return base.Handle(request, botClient, message);
			}
		}
	}
}