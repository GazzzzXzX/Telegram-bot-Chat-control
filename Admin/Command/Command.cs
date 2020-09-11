using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BotCore.Advertising;
using BotCore.Blockchain;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Payments;
using Telegram.Bot.Types.ReplyMarkups;

namespace BotCore
{
	internal static class IsBan
	{
		public static System.Boolean Ban(TelegramBotClient telegramBotClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase db = Singleton.GetInstance().Context;
			Settings settings = db.GetSettings();
			try
			{
				if (_message != null)
				{
					User user = db.GetUser(_message.From.Id);

					if (user.BanDate < System.DateTime.Now)
					{
						return true;
					}
					else
					{
						InlineButton inlineButton = new InlineButton();

						System.String temp = "🚪Личный кабинет🚪\n🆔: "
								+ user.ID + "\n🖌ФИО: " + user.FIO;
						temp += user.Username != "Нет!" ? "\n🧸Username: @" + user.Username : " ";
						temp += "\n📱Номер: "
								+ user.Number + "\n⭐️Рейтинг: "
								+ user.SumRating + "\n✖️Бан действует до: "
								+ user.BanDate.ToString("MM/dd/yyyy HH:mm:ss") + "\nПричина: "
								+ user.BanDescript
								+ "\n\nВы можете подать апелляцию, либо оплатить бан, после чего вы сразу будете разблокированы!\n\nСтоимость разбана: 100 грн\n\nТак же вы можете добавить в чат " + settings.AddUser + $" человек и вы так же будете разбанены!\n\nПожалуйста ознакомтесь с правилами нашего сообщества более детально, что бы в дальнейшем вы не получили бан/мут!";
						telegramBotClient.DeleteMessage(_message.From.Id, _message.Message.MessageId, "43 - IsBan");
						telegramBotClient.SendText(_message.From.Id, temp, user, replyMarkup: inlineButton.BanAccaunt);
						return false;
					}
				}
				else
				{
					Message mes = message as Message;

					User user = db.GetUser(mes.From.Id);

					telegramBotClient.DeleteMessage(mes.From.Id, mes.MessageId, "53 - Command");
					if (user.BanDate < System.DateTime.Now)
					{
						return true;
					}
					else
					{
						InlineButton inlineButton = new InlineButton();

						System.String temp = "🚪Личный кабинет🚪\n🆔: "
								+ user.ID + "\n🖌ФИО: " + user.FIO;
						temp += user.Username != "Нет!" ? "\n🧸Username: @" + user.Username : " ";
						temp += "\n📱Номер: "
								+ user.Number + "\n⭐️Рейтинг: "
								+ user.SumRating + "\n✖️Бан действует до: "
								+ user.BanDate + "\nПричина: "
								+ user.BanDescript
								+ "\n\nВы можете подать апелляцию, либо оплатить бан, после чего вы сразу будете разблокированы!\n\nСтоимость разбана: 100 грн\n\nТак же вы можете добавить в чат " + settings.AddUser + $" человек и вы так же будете разбанены!\n\nПожалуйста ознакомтесь с правилами нашего сообщества более детально, что бы в дальнейшем вы не получили бан/мут!";

						telegramBotClient.SendTextMessageAsync(mes.From.Id, temp, replyMarkup: inlineButton.BanAccaunt);
						return false;
					}
				}
			}
			catch (System.Exception ex)
			{
				Log.Logging("IsBan Bool Ban(): " + ex);
				return false;
			}
		}
	}
	internal class Start : AbsCommand, IStandartCommand
	{
		public override String Name { get; set; } = CommandText.Start;

		private Message _message = null;
		private DataBase db = null;
		private User user = null;
		private InlineButton inlineButton = new InlineButton();

		public override async void Execute(TelegramBotClient botClient, Object message)
		{
			IStandartCommand standartCommand = new Start();
			
			if(standartCommand.SetMessage(message, out _message)) return;
			
			if (standartCommand.SetDataBase(out db)) return;

			botClient.DeleteMessage(_message.From.Id, _message.MessageId, "Невозможно удалить сообщение - Класс Start");
			
			if (standartCommand.SetUserAndCheckIsNullMessage(botClient, _message, out user, db)) 
			{
				botClient.SendText(_message.From.Id,
				                   "Привет, " + _message.From.FirstName +
				                   "\nПожалуйста зарегистрируейтесь в системе UBC, данная операция требуется для того чтобы избежать мошенничества в чатах UBC и улучшить качество взаимодействия и работы в чатах.\nТак же для система отзывов и рейтинга о участниках UBC!",
				                   replyMarkup: inlineButton.Register);
				return;
			}

			if (user.FIO == null)
			{
				UpdateDB(1);
				SendMessage(botClient, $"Привет, {_message.From.FirstName}\nВведите ФИО:");
				return;
			}
			else if (user.Number == "0")
			{
				UpdateDB(2);
				
				ReplyButton replyButton = new ReplyButton();
				Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup temp = replyButton.SetNumber();
				
				botClient.SendText(_message.From.Id, $"Привет, {_message.From.FirstName}\nПоделитесь номером телефона при помощью кнопки в нижнем меню: ", user, replyMarkup: temp);
				return;
			}

			UpdateDB((Int32)SetChain.MessageUserInBot);
			SendMessage(botClient);
		}

		public void SendMessage(TelegramBotClient botClient)
		{
			System.String temp = InfoUser.Info(user);
			if (user.BanDate.Date < System.DateTime.Now)
			{
				if (user.IsAdmin > 0)
				{
					botClient.SendText(_message.From.Id, temp, user, replyMarkup: inlineButton.AdminAccaunt);
				}
				else
				{
					botClient.SendText(_message.From.Id, temp, user, replyMarkup: inlineButton.Accaunt);
				}
				db.Save();
			}
		}

		private void SendMessage(TelegramBotClient botClient, String message)
		{
			InlineButton inlineButton = new InlineButton();
			botClient.SendText(_message.From.Id,
			                   message, user);
		}

		private void UpdateDB(Int32 chain)
		{
			user.Chain = chain;
			db.Save();
		}
	}

	internal class Register : AbsCommand, IStandartCommand
	{
		public override String Name { get; set; } = CommandText.Register;
		
		private CallbackQuery _message = null;
		private DataBase db = null;
		private User user = null;
		private InlineButton inlineButton = new InlineButton();

		public override void Execute(TelegramBotClient botClient, Object message)
		{
			IStandartCommand standartCommand = new Register();
			
			if(standartCommand.SetCallbackQuery(message, out _message)) return;
			
			if (standartCommand.SetDataBase(out db)) return;

			if (standartCommand.SetUserAndCheckIsNull(botClient, _message, out user, db))
			{
				db.AddNewUser(_message);

				db.Save();
				
				if (standartCommand.SetUserAndCheckIsNull(botClient, _message, out user, db)) return;

				db.AddAdUser(user);
				db.Save();
			}

			botClient.DeleteMessage(_message.From.Id, _message.Message.MessageId, "Невозможно удалить сообщение класс Register!");
			
			SendMessage(botClient);
		}
		
		public async void Execute(TelegramBotClient botClient, Message _message)
		{
			DataBase DB = Singleton.GetInstance().Context;
			try
			{
				User user = DB.GetUser(_message.From.Id);

				user.Chain = 1;
				
				db.Save();

				botClient.DeleteMessage(_message.From.Id, _message.MessageId - 1, "145 - Command");
				
				botClient.SendText(_message.From.Id, "Привет, " + _message.From.FirstName + "\nВведите ФИО: ", user);
			}
			catch (System.Exception ex)
			{
				Log.Logging("Register : AbsCommand: " + ex);
			}

			try
			{
				botClient.DeleteMessage(_message.From.Id, _message.MessageId, "196 - Command");
			}
			catch{}
		}

		public void SendMessage(TelegramBotClient botClient)
		{
			botClient.SendText(_message.From.Id, "Привет, " + _message.From.FirstName + "\nВведите ФИО: ", user);
		}
	}

	internal class MeinMenu : AbsCommand
	{
		public override System.String Name { get; set; } = CommandText.MeinMenu;

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			Message _message = message as Message;
			DataBase db = Singleton.GetInstance().Context;
			try
			{
				User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return;
				botClient.DeleteMessage(_message.From.Id, _message.MessageId, "212 - Command");
				botClient.DeleteMessage(_message.From.Id, user.MessageID, "213 - Command");

				ReplyButton replyButton = new ReplyButton();
				ReplyKeyboardMarkup temp;
				if (user.BanDate.Date < System.DateTime.Now)
				{
					temp = replyButton.Menu();
				}
				else
				{
					temp = replyButton.BanMenu();
				}
				new Accaunt().Execute(botClient, _message);
				//botClient.SendText(user.ID, "❗️Меню❗️", replyMarkup: temp);

				user.Chain = (System.Int32)SetChain.MessageUserInBot;
				db.Save();
			}
			catch (System.Exception ex)
			{
				Log.Logging("MeinMenu : AbsCommand: " + ex);
			}
		}
	}

	internal class Accaunt : AbsCommand
	{
		public override System.String Name { get; set; } = CommandText.Accaunt;

		private InlineButton inlineButton = new InlineButton();

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			Message _message = message as Message;
			DataBase db = Singleton.GetInstance().Context;
			User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return;

			botClient.DeleteMessage(_message.From.Id, user.MessageID, "252 - Command");

			if (IsBan.Ban(botClient, message))
			{
				try
				{
					//--- Удаление пароля!
					if (user.Chain == 22)
					{
						CommandText.PasswordAdmin = null;
						user.Chain = (System.Int32)SetChain.MessageUserInBot;
						db.Save();
					}

					System.String temp = InfoUser.Info(user);
					if (user.BanDate.Date < System.DateTime.Now)
					{
						if (user.IsAdmin > 0)
						{
							botClient.SendText(_message.From.Id, temp, user, replyMarkup: inlineButton.AdminAccaunt);
						}
						else
						{
							botClient.SendText(_message.From.Id, temp, user, replyMarkup: inlineButton.Accaunt);
						}
						db.Save();
					}
				}
				catch (System.Exception ex)
				{
					Log.Logging("Accaunt : AbsCommand: " + ex);
				}
			}
		}
	}

	internal class CallAdmin : AbsCommand
	{
		public override System.String Name { get; set; } = CommandText.CallAdmin;

		private InlineButton inlineButton = new InlineButton();

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase db = Singleton.GetInstance().Context;
			User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return;

			botClient.DeleteMessage(_message.From.Id, user.MessageID, "301 - Command");

			if (user.Username == "Нет!" && _message.From.Username != null)
			{
				user.Username = _message.From.Username;
				db.Save();
			}

			if (IsBan.Ban(botClient, message))
			{
				if (user.Username != "Нет!")
				{
					botClient.SendText(_message.From.Id, "Вы можете связаться с администратором через данного бота @UBCSupport_Bot", user, replyMarkup: inlineButton.BackToAccauntMenu);
				}
				else
				{
					botClient.SendText(_message.From.Id, "Мы не можем отправить заявку администрации, у вас не установлен \"имя пользователя\" на аккаунте.\nЕго можно установить в настройках -> \"Имя пользователя\"", user, replyMarkup: inlineButton.BackToAccauntMenu);
				}
			}
		}
	}

	internal class CallAdminYes : AbsCommand
	{
		public override System.String Name { get; set; } = CommandText.CallAdminYes;

		private InlineButton inlineButton = new InlineButton();

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;

			DataBase db = Singleton.GetInstance().Context;
			User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return;

			Settings settings = db.GetSettings();
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					botClient.EditMessage(_message.From.Id, user.MessageID, "Ваша заявка оставлена, ожидайте скоро с вами свяжется администратор!\nХорошего вам дня!", "337", user, replyMarkup: inlineButton.BackToAccauntMenu);
					System.String temp = InfoUser.Info(user);
					botClient.SendText(settings.ChannelAdmin, "Заявка для связи с администратором!\n" + temp, user);
				}
				catch (System.Exception ex)
				{
					Log.Logging("CallAdminYes : AbsCommand: " + ex);
				}
			}
		}
	}

	internal class Close : AbsCommand
	{
		public override System.String Name { get; set; } = CommandText.Close;

		private InlineButton inlineButton = new InlineButton();

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase db = Singleton.GetInstance().Context;
			User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return;
			
			try
			{
				botClient.DeleteMessage(_message.From.Id, _message.Message.MessageId, "360 - Command");
				IsNullDataBase.DeleteAllMessage(botClient, _message);
				
			}
			catch(System.Exception ex)
			{
				IsNullDataBase.DeleteMessageUser(botClient, _message);
				Log.Logging("Close : AbsCommand: " + ex);
			}
			InlineButton inlineButton = new InlineButton();
			System.String temp = InfoUser.Info(user);
			if (user.IsAdmin > 0)
			{
				botClient.SendText(_message.From.Id, temp, user, replyMarkup: inlineButton.AdminAccaunt);
			}
			else
			{
				botClient.SendText(_message.From.Id, temp, user, replyMarkup: inlineButton.Accaunt);
			}
		}
	}

	internal class ChouseFIO : AbsCommand
	{
		public override System.String Name { get; set; } = CommandText.ChouseFIO;

		private InlineButton inlineButton = new InlineButton();

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase db = Singleton.GetInstance().Context;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return;
					user.Chain = 3;

					botClient.EditMessage(_message.From.Id,
						_message.Message.MessageId, "Ваше ФИО: " + user.FIO + "\nВведите новое ФИО: ", "383", user, replyMarkup: inlineButton.BackToAccauntMenu);
				}
				catch (System.Exception ex)
				{
					Log.Logging("ChouseFIO : AbsCommand: " + ex);
				}
				db.Save();
			}
		}
	}

	internal class ChouseNumber : AbsCommand
	{
		public override System.String Name { get; set; } = CommandText.ChouseNumber;

		private InlineButton inlineButton = new InlineButton();

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase db = Singleton.GetInstance().Context;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return;
					user.Chain = 4;

					botClient.EditMessage(_message.From.Id,
						_message.Message.MessageId, "Ваш номер телефона: " +
						user.Number + "\nВведите новый номер телефона: ", "412", user,
						replyMarkup: inlineButton.BackToAccauntMenu);
				}
				catch (System.Exception ex)
				{
					Log.Logging("ChouseNumber : AbsCommand: " + ex);
				}
				db.Save();
			}
		}
	}

	internal class MyReviews : AbsCommand
	{
		public override System.String Name { get; set; } = CommandText.Reviews;

		private InlineButton inlineButton = new InlineButton();

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					botClient.EditMessage(_message.From.Id,
						_message.Message.MessageId,
						"✉️Отзывы✉️", "439",
						replyMarkup: inlineButton.MyReviews);
				}
				catch (System.Exception ex)
				{
					Log.Logging("MyReviews : AbsCommand: " + ex);
				}
			}
		}
	}

	internal class SearchUsers : AbsCommand
	{
		public override System.String Name { get; set; } = CommandText.SearchUsers;

		private InlineButton inlineButton = new InlineButton();

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase db = Singleton.GetInstance().Context;
			if (IsBan.Ban(botClient, message))
			{
				User user = db.GetUser(_message.From.Id);
				ChangeUser(user, db);
				try
				{
					botClient.EditMessage(_message.From.Id,
						_message.Message.MessageId,
						"🔍Поиск участника🔍\nТак же можете переслать сообщение от учатника и мы его найдем:", "464",
						replyMarkup: inlineButton.SearchUsers);
				}
				catch (System.Exception ex)
				{
					Log.Logging("SearchUsers : AbsCommand: " + ex);
				}
			}
		}

		private void ChangeUser(User user, DataBase db)
		{
			user.Chain = 50;
			db.Save();
		}
	}

	internal class SearchID : AbsCommand
	{
		public override System.String Name { get; set; } = CommandText.SearchID;

		private InlineButton inlineButton = new InlineButton();

		public override void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase db = Singleton.GetInstance().Context;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return;

					ChangeUser(user, db);

					botClient.EditMessage(_message.From.Id,
							_message.Message.MessageId,
							"🔍Поиск участника🔍\nВведите ID или перешлите сообщения человека:", "498", user,
							replyMarkup: inlineButton.BackToAccauntMenu);
					user.MessageID = _message.Message.MessageId;
					db.Save();
					return;

				}
				catch (System.Exception ex)
				{
					Log.Logging("SearchID : AbsCommand: " + ex);
				}
				db.Save();
			}
		}

		private void ChangeUser(User user, DataBase db)
		{
			user.Chain = 50;
			db.Save();
		}
	}

	internal class SearchNumber : AbsCommand
	{
		public override System.String Name { get; set; } = CommandText.SearchNumber;

		private InlineButton inlineButton = new InlineButton();

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase db = Singleton.GetInstance().Context;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return;

					ChangeUser(user, db);

					botClient.EditMessage(_message.From.Id,
						_message.Message.MessageId,
						"🔍Поиск участника🔍\nВведите номер телефона или перешлите сообщения человека:", "551", user,
						replyMarkup: inlineButton.BackToAccauntMenu);
					user.MessageID = _message.Message.MessageId;
				}
				catch (System.Exception ex)
				{
					Log.Logging("SearchNumber : AbsCommand: " + ex);
				}
				db.Save();
			}
		}

		private void ChangeUser(User user, DataBase db)
		{
			user.Chain = 51;
			db.Save();
		}
	}

	internal class LeaveFeedback : AbsCommand
	{
		public override System.String Name { get; set; } = CommandText.LeaveFeedback;

		private InlineButton inlineButton = new InlineButton();

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase db = Singleton.GetInstance().Context;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return;
					user.Chain = 6;

					botClient.EditMessage(_message.From.Id,
						_message.Message.MessageId,
						"🧧Оставить отзыв🧧\nВведите текст:", "583", user,
						replyMarkup: inlineButton.BackToAccauntMenu);
				}
				catch (System.Exception ex)
				{
					Log.Logging("LeaveFeedback : AbsCommand: " + ex);
				}
				db.Save();
			}
		}
	}

	internal class CheckFeedback : AbsCommand
	{
		public override System.String Name { get; set; } = CommandText.CheckFeedback;

		private InlineButton inlineButton = new InlineButton();

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase db = Singleton.GetInstance().Context;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return;
					user.MessageID = _message.Message.MessageId;
					user.Chain = 54;
					db.Save();

					InlineKeyboardMarkup temp = inlineButton.ShowOtherReviews(message);

					botClient.EditMessage(_message.From.Id, _message.Message.MessageId,
						"🧧Отзывы🧧", "616", user,
						replyMarkup: temp);
				}
				catch (System.Exception ex)
				{
					Log.Logging("CheckFeedback : AbsCommand: " + ex);
				}
			}
		}
	}

	internal class SendComplaint : AbsCommand
	{
		public override System.String Name { get; set; } = CommandText.SendComplaint;

		private InlineButton inlineButton = new InlineButton();

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase db = Singleton.GetInstance().Context;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return;
					user.Chain = 7;
					db.Save();

					botClient.EditMessage(_message.From.Id, _message.Message.MessageId,
						"📖Жалоба📖\nЕсли вы хотите пожаловаться на конкретное сообщение вам нужно:\n\nНажмите на сообщение -> \"Копировать ссылку\" -> вставить ссылку\n\nНе забудьте описать суть жалобы!\nВведите текст: ", "647", user,
						replyMarkup: inlineButton.BackToAccauntMenu);
				}
				catch (System.Exception ex)
				{
					Log.Logging("SendComplaint : AbsCommand: " + ex);
				}
			}
		}
	}

	internal class SendAppeal : AbsCommand
	{
		public override System.String Name { get; set; } = CommandText.SendAppeal;

		private InlineButton inlineButton = new InlineButton();

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase db = Singleton.GetInstance().Context;
			try
			{
				User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return;
				user.Chain = 8;
				db.Save();

				botClient.EditMessage(_message.From.Id, _message.Message.MessageId,
					"📖Апелляция📖\nВведите текст: ", "676", user,
					replyMarkup: inlineButton.BackToAccauntMenu);
			}
			catch (System.Exception ex)
			{
				Log.Logging("SendAppeal : AbsCommand: " + ex);
			}
		}
	}

	internal class BackToAccauntMenu : AbsCommand
	{
		public override System.String Name { get; set; } = CommandText.BackToAccauntMenu;

		private InlineButton inlineButton = new InlineButton();

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase db = Singleton.GetInstance().Context;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return;
					Reviews reviews = db.GetReviews(_message);
					Reviews reviews1 = db.GetReviews(_message);
					TempBase tempBase = db.GetTempBase(user);

					if (tempBase != null)
					{
						db.RemoveTempBase(tempBase);
					}

					if (reviews != null)
					{
						reviews.TheEnd = true;
					}

					user.Chain = (System.Int32)SetChain.MessageUserInBot;

					db.Save();
					System.String temp = InfoUser.Info(user);
					if (user.IsAdmin >= 1)
					{
						try
						{
							await botClient.EditMessageTextAsync(_message.From.Id, _message.Message.MessageId, temp, replyMarkup: inlineButton.AdminAccaunt);
						}
						catch
						{
							botClient.DeleteMessage(_message.From.Id, user.MessageID, "694 - BackToAccauntMenu");
							botClient.SendText(_message.From.Id, temp, user, replyMarkup: inlineButton.AdminAccaunt);
						}
					}
					else
					{
						try
						{
							await botClient.EditMessageTextAsync(_message.From.Id, _message.Message.MessageId, temp, replyMarkup: inlineButton.Accaunt);
						}
						catch
						{
							botClient.DeleteMessage(_message.From.Id, user.MessageID, "706 - BackToAccauntMenu");
							botClient.SendText(_message.From.Id, temp, user, replyMarkup: inlineButton.Accaunt);
						}
					}
				}
				catch (System.Exception ex)
				{
					Log.Logging("BackToAccauntMenu : AbsCommand: " + ex);
				}
			}
		}
	}

	internal class BackToReviewsMenuBan : AbsCommand
	{
		public override System.String Name { get; set; } = CommandText.BackToReviewsMenuBan;

		private InlineButton inlineButton = new InlineButton();

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase db = Singleton.GetInstance().Context;
			try
			{
				User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return;
				user.Chain = (System.Int32)SetChain.MessageUserInBot;

				db.Save();

				botClient.DeleteMessage(_message.From.Id, _message.Message.MessageId, "758 - Command");
				System.String temp;
				if (user.BanDate.Date < System.DateTime.Now)
				{
					temp = InfoUser.Info(user);
					botClient.SendText(_message.From.Id, temp, user,
					replyMarkup: inlineButton.Accaunt);
				}
				else
				{
					IsBan.Ban(botClient, message);
				}
			}
			catch (System.Exception ex)
			{
				Log.Logging("BackToReviewsMenuBan : AbsCommand: " + ex);
			}
		}
	}

	internal class BackToReviewsMenu : AbsCommand
	{
		public override System.String Name { get; set; } = CommandText.BackToReviewsMenu;

		private InlineButton inlineButton = new InlineButton();

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase db = Singleton.GetInstance().Context;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return;
					user.Chain = (System.Int32)SetChain.MessageUserInBot;

					db.Save();

					botClient.EditMessage(_message.From.Id,
						_message.Message.MessageId,
						"✉️Отзывы✉️", "796", user,
						replyMarkup: inlineButton.MyReviews);
				}
				catch (System.Exception ex)
				{
					Log.Logging("BackToReviewsMenu : AbsCommand: " + ex);
				}
			}
		}
	}

	internal class BackToAccauntMenuAndDelete : AbsCommand
	{
		public override System.String Name { get; set; } = CommandText.BackToAccauntMenuAndDelete;

		private InlineButton inlineButton = new InlineButton();

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase db = Singleton.GetInstance().Context;

			User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					Reviews reviews = db.GetReviews(_message);
					if (reviews != null)
					{
						db.RemoveReviews(reviews);
					}

					System.String temp = InfoUser.Info(user);
					if (user.IsAdmin > 0)
					{
						botClient.EditMessage(_message.From.Id, _message.Message.MessageId, temp, "832", user, replyMarkup: inlineButton.AdminAccaunt);
					}
					else
					{
						botClient.EditMessage(_message.From.Id, _message.Message.MessageId, temp, "832", user, replyMarkup: inlineButton.Accaunt);
					}
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
			user.Chain = (System.Int32)SetChain.MessageUserInBot;

			db.Save();
		}
	}

	internal class BackToReviewsMenuAndDelete : AbsCommand
	{
		public override System.String Name { get; set; } = CommandText.BackToReviewsMenuAndDelete;

		private InlineButton inlineButton = new InlineButton();

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase db = Singleton.GetInstance().Context;
			User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return;
			user.Chain = (System.Int32)SetChain.MessageUserInBot;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					Reviews reviews = Singleton.GetInstance().Context._reviews.Where(p => p.IDSender == user.ID).FirstOrDefault(p => p.TheEnd == false);
					db.RemoveReviews(reviews);

					botClient.EditMessage(_message.From.Id,
						_message.Message.MessageId,
						"✉️Отзывы✉️", "837", user,
						replyMarkup: inlineButton.MyReviews);
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
			try
			{
				db.Save();
			}
			catch (System.Exception ex)
			{
				Log.Logging(ex);
			}
		}
	}

	internal class ShowMyReviews : AbsCommand
	{
		public override System.String Name { get; set; } = CommandText.ShowMyReviews;

		private InlineButton inlineButton = new InlineButton();

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase db = Singleton.GetInstance().Context;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return;

					user.Chain = 53;

					db.Save();

					InlineKeyboardMarkup temp = inlineButton.ShowMyReviews(message);
					botClient.EditMessage(_message.From.Id,
						_message.Message.MessageId,
						"✉️Отзывы✉️", "915", user,
						replyMarkup: temp);
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class ShowOtherReviews : AbsCommand
	{
		public override System.String Name { get; set; } = CommandText.ShowOtherReviews;

		private InlineButton inlineButton = new InlineButton();

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase db = Singleton.GetInstance().Context;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return;

					user.Chain = 54;

					db.Save();

					InlineKeyboardMarkup temp = inlineButton.ShowMyReviewsByUsers(message);
					botClient.EditMessage(_message.From.Id,
						_message.Message.MessageId,
						"✉️Отзывы✉️", "950", user,
						replyMarkup: temp);
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class SelectMyReviews
	{
		private InlineButton inlineButton = new InlineButton();

		public async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase db = Singleton.GetInstance().Context;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					System.String Name = _message.Data;

					System.String[] words = Name.Split(new System.Char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
					System.Int32 temp = System.Convert.ToInt32(words[0]);

					Reviews reviews = db.GetReviews(temp);
					User user = db.GetUser(reviews.IDRecipient);

					reviews.TheEnd = false;

					db.Save();

					botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "✉️Отзывы✉️\n" + "Получатель: " + user.FIO + "\nСодержание: " + reviews.Description, "986", user, replyMarkup: inlineButton.ChangeOrDeleteMyReviews);
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class DeleteMyReviews : AbsCommand
	{
		public override System.String Name { get; set; } = CommandText.DeleteMyReviews;

		private InlineButton inlineButton = new InlineButton();

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase db = Singleton.GetInstance().Context;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					Reviews reviews = db.GetReviews(_message);

					Singleton.GetInstance().Context._reviews.Remove(reviews);

					db.Save();

					InlineKeyboardMarkup temp = inlineButton.ShowMyReviews(message);
					botClient.EditMessage(_message.From.Id,
						_message.Message.MessageId,
						"✉️Отзывы✉️\nОтзыв был удален!", "1019",
						replyMarkup: temp);
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class ChangeMyReviews : AbsCommand
	{
		public override System.String Name { get; set; } = CommandText.ChangeMyReviews;

		private InlineButton inlineButton = new InlineButton();

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase db = Singleton.GetInstance().Context;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return;
					Reviews reviews = Singleton.GetInstance().Context._reviews.Where(p => p.IDSender == user.ID).FirstOrDefault(p => p.TheEnd == false);

					user.Chain = 62;

					botClient.EditMessage(_message.From.Id,
						_message.Message.MessageId,
						"✉️Отзывы✉️\n" + reviews.Description + "\nВведите новый отзыв:", "1050", user,
						replyMarkup: inlineButton.BackToReviewsMenu);
					user.MessageID = _message.Message.MessageId;
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
				db.Save();
			}
		}
	}

	internal class OneStar : AbsCommand
	{
		public override System.String Name { get; set; } = CommandText.OneStar;

		private InlineButton inlineButton = new InlineButton();

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase db = Singleton.GetInstance().Context;

			try
			{
				User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return;
				Reviews reviews = db.GetReviewsIDSender(user);
				User userTwo = db.GetUser(reviews);

				db.SetStar(userTwo, 1);
				reviews.TheEnd = true;

				db.Save();

				userTwo.SumRating = db.SetSumReting(userTwo);

				db.Save();

				System.String temp = InfoUser.Star(user, userTwo, reviews);
				if (user.IsAdmin > 0)
				{
					botClient.EditMessage(_message.From.Id, _message.Message.MessageId, temp, "1092", user, replyMarkup: inlineButton.AdminAccaunt);
				}
				else
				{
					botClient.EditMessage(_message.From.Id, _message.Message.MessageId, temp, "1096", user, replyMarkup: inlineButton.Accaunt);
				}

				db.Save();
			}
			catch (System.Exception ex)
			{
				Log.Logging(ex);
			}
		}
	}

	internal class TwoStar : AbsCommand
	{
		public override System.String Name { get; set; } = CommandText.TwoStar;

		private InlineButton inlineButton = new InlineButton();

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase db = Singleton.GetInstance().Context;
			try
			{
				User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return;
				Reviews reviews = db.GetReviewsIDSender(user);
				User userTwo = db.GetUser(reviews);
				db.SetStar(userTwo, 2);
				reviews.TheEnd = true;

				db.Save();
				userTwo.SumRating = db.SetSumReting(userTwo);
				db.Save();

				System.String temp = InfoUser.Star(user, userTwo, reviews);
				if (user.IsAdmin > 0)
				{
					botClient.EditMessage(_message.From.Id, _message.Message.MessageId, temp, "1132", user, replyMarkup: inlineButton.AdminAccaunt);
				}
				else
				{
					botClient.EditMessage(_message.From.Id, _message.Message.MessageId, temp, "1136", user, replyMarkup: inlineButton.Accaunt);
				}
				db.Save();
			}
			catch (System.Exception ex)
			{
				Log.Logging(ex);
			}
		}
	}

	internal class ThreeStar : AbsCommand
	{
		public override System.String Name { get; set; } = CommandText.ThreeStar;

		private InlineButton inlineButton = new InlineButton();

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase db = Singleton.GetInstance().Context;
			try
			{
				User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return;
				Reviews reviews = db.GetReviewsIDSender(user);
				User userTwo = db.GetUser(reviews);
				db.SetStar(userTwo, 3);
				reviews.TheEnd = true;

				db.Save();
				userTwo.SumRating = db.SetSumReting(userTwo);
				db.Save();

				System.String temp = InfoUser.Star(user, userTwo, reviews);
				if (user.IsAdmin > 0)
				{
					botClient.EditMessage(_message.From.Id, _message.Message.MessageId, temp, "1171", user, replyMarkup: inlineButton.AdminAccaunt);
				}
				else
				{
					botClient.EditMessage(_message.From.Id, _message.Message.MessageId, temp, "1175", user, replyMarkup: inlineButton.Accaunt);
				}
				db.Save();
			}
			catch (System.Exception ex)
			{
				Log.Logging(ex);
			}
		}
	}

	internal class FourStar : AbsCommand
	{
		public override System.String Name { get; set; } = CommandText.FourStar;

		private InlineButton inlineButton = new InlineButton();

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase db = Singleton.GetInstance().Context;
			try
			{
				User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return;
				Reviews reviews = db.GetReviewsIDSender(user);
				User userTwo = db.GetUser(reviews);
				db.SetStar(userTwo, 4);
				reviews.TheEnd = true;

				db.Save();
				userTwo.SumRating = db.SetSumReting(userTwo);
				db.Save();

				System.String temp = InfoUser.Star(user, userTwo, reviews);
				if (user.IsAdmin > 0)
				{
					botClient.EditMessage(_message.From.Id, _message.Message.MessageId, temp, "1210", user, replyMarkup: inlineButton.AdminAccaunt);
				}
				else
				{
					botClient.EditMessage(_message.From.Id, _message.Message.MessageId, temp, "1214", user, replyMarkup: inlineButton.Accaunt);
				}
				db.Save();
			}
			catch (System.Exception ex)
			{
				Log.Logging(ex);
			}
		}
	}

	internal class FiveStar : AbsCommand
	{
		public override System.String Name { get; set; } = CommandText.FiveStar;

		private InlineButton inlineButton = new InlineButton();

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase db = Singleton.GetInstance().Context;
			try
			{
				User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return;
				Reviews reviews = db.GetReviewsIDSender(user);
				User userTwo = db.GetUser(reviews);
				db.SetStar(userTwo, 5);
				reviews.TheEnd = true;

				db.Save();
				userTwo.SumRating = db.SetSumReting(userTwo);
				db.Save();

				System.String temp = InfoUser.Star(user, userTwo, reviews);
				if (user.IsAdmin > 0)
				{
					botClient.EditMessage(_message.From.Id, _message.Message.MessageId, temp, "1249", user, replyMarkup: inlineButton.AdminAccaunt);
				}
				else
				{
					botClient.EditMessage(_message.From.Id, _message.Message.MessageId, temp, "1253", user, replyMarkup: inlineButton.Accaunt);
				}
				db.Save();
			}
			catch (System.Exception ex)
			{
				Log.Logging(ex);
			}
		}
	}

	internal class SelectOtherReviews
	{
		private InlineButton inlineButton = new InlineButton();

		public async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase db = Singleton.GetInstance().Context;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					System.String Name = _message.Data;

					System.String[] words = Name.Split(new System.Char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
					System.Int32 temp = System.Convert.ToInt32(words[0]);

					Reviews reviews = db.GetReviews(temp);
					User user = db.GetUser(reviews.IDSender);
					if (user.IsAdmin > 0)
					{
						botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "✉️Отзывы✉️\n" + "Отправитель: " + user.FIO + "\n" + reviews.Description + "\n<i>Отзыв оставлен Администратором</i>", "1285", user, replyMarkup: inlineButton.BackToAccauntMenu);
					}
					else
					{
						botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "✉️Отзывы✉️\n" + "Отправитель: " + user.FIO + "\n" + reviews.Description, "1289", user, replyMarkup: inlineButton.BackToAccauntMenu);
					}
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class SetBan
	{
		private InlineButton inlineButton = new InlineButton();

		public async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase db = Singleton.GetInstance().Context;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					System.String Name = _message.Data;

					System.String[] words = Name.Split(new System.Char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
					System.Int32 temp = System.Convert.ToInt32(words[0]);
					Name = words[2];

					User user = db.GetUser(temp);
					User Admin = db.GetUser(_message.From.Id);
					db.SetTempDate(Admin, user);
					Settings settings = db.GetSettings();

					botClient.DeleteMessage(settings.ChannelAdmin, _message.Message.MessageId, "1327 - Command");
					botClient.SendText(Admin.ID, "Введите количество дней бана: ", Admin);

					Admin.Chain = 1052;

					db.Save();
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class SetAppeal
	{
		private InlineButton inlineButton = new InlineButton();

		public async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase db = Singleton.GetInstance().Context;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					System.String Name = _message.Data;

					System.String[] words = Name.Split(new System.Char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
					System.Int32 temp = System.Convert.ToInt32(words[0]);
					Name = words[2];

					User user = db.GetUser(temp);
					User Admin = db.GetUser(_message.From.Id);
					Settings settings = db.GetSettings();

					botClient.DeleteMessage(settings.ChannelAdmin, _message.Message.MessageId, "1364 - Command");
					System.String text = "Администратор " + Admin.IsAdmin + " уровня: @" + Admin.Username + " разбанил пользователя!" + "\nID: " + user.ID + "\nФИО: " + user.FIO + "\nНомер: " + user.Number;
					botClient.SendText(settings.ChannelAdmin, text);
					user.BanDate = System.DateTime.Now;
					IsUnBan.ThisUnBan(botClient, user);

					Admin.Chain = 0;

					db.Save();
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class SetCancel
	{
		private InlineButton inlineButton = new InlineButton();

		public async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase db = Singleton.GetInstance().Context;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					System.String Name = _message.Data;

					System.String[] words = Name.Split(new System.Char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
					System.Int32 temp = System.Convert.ToInt32(words[0]);
					Name = words[2];

					User user = db.GetUser(temp);
					User Admin = db.GetUser(_message.From.Id);
					Settings settings = db.GetSettings();

					botClient.DeleteMessage(settings.ChannelAdmin, _message.Message.MessageId, "1403 - Command");
					System.String text = "Пользователь " + user.FIO + "\nID: " + user.ID + "\nНомер: " + user.Number + "\nЖалоба отменена администратором " + Admin.IsAdmin + " урованя " + Admin.FIO + "\nПо причине: " + Name;

					botClient.SendText(settings.ChannelAdmin, text);
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class SetCancelAppeal
	{
		private InlineButton inlineButton = new InlineButton();

		public async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase db = Singleton.GetInstance().Context;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					System.String Name = _message.Data;

					System.String[] words = Name.Split(new System.Char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
					System.Int32 temp = System.Convert.ToInt32(words[0]);
					Name = words[2];

					User user = db.GetUser(temp);
					User Admin = db.GetUser(_message.From.Id);
					Settings settings = db.GetSettings();

					botClient.DeleteMessage(settings.ChannelAdmin, _message.Message.MessageId, "1437 - Command");
					System.String text = "Пользователь " + user.FIO + "\nID: " + user.ID + "\nНомер: " + user.Number + "\nАпелляция отменена администратором " + Admin.IsAdmin + " урованя " + Admin.FIO + "\nПо причине: " + Name;

					botClient.SendText(settings.ChannelAdmin, text);
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class ThisAdminChannel : AbsCommand
	{
		public override System.String Name { get; set; } = CommandText.ThisAdminChannel;

		private InlineButton inlineButton = new InlineButton();

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			Message _message = message as Message;
			DataBase db = Singleton.GetInstance().Context;
			try
			{
				User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return;
				if (user.IsAdmin >= 2)
				{
					Settings settings = db.GetSettings();
					if (settings != null)
					{
						settings.ChannelAdmin = _message.Chat.Id;
						db.Save();
						botClient.DeleteMessage(_message.Chat.Id, _message.MessageId, "1470 - Command");
						botClient.SendText(_message.Chat.Id, "Теперь это канал админов!");
						db.Save();
					}
					else
					{
						Singleton.GetInstance().Context._settings.Add(new Settings() { ChannelAdmin = _message.Chat.Id, PasswordAdmin = "UBC_Admin" });
						user.Chain = 23;
						botClient.SendText(_message.Chat.Id, "Теперь это канал админов!");
						botClient.SendText(_message.From.Id, "Введите пароль для админ панели!");
						db.Save();
					}
				}
			}
			catch (System.Exception ex)
			{
				Log.Logging(ex);
			}
		}
	}

	internal class BackToSetting : AbsCommand
	{
		public override System.String Name { get; set; } = CommandText.BackToSetting;

		private InlineButton inlineButton = new InlineButton();

		private void ChangeUser(User user, DataBase db)
		{
			user.Chain = (System.Int32)SetChain.MessageUserInBot;
			db.Save();
		}

		private void DeleteNotification(User user, DataBase db)
		{
			ButtonAndTextNotication delete = db.GetButtonAndTextNotication(user);
			NotificationChat[] notificationChats = db.GetNotificationChats();
				
			if (delete != null)
			{
				var temp = db.GetListCollectionButtonNotification(delete);

				foreach (CollectionButtonNotification notification in temp)
				{
					db.Remove(notification);
				}
				
				db.Remove(delete);
				db.Save();
			}

			if (notificationChats != null)
			{
				foreach (NotificationChat notificationChat in notificationChats)
				{
					db.Remove(notificationChat);
				}
				db.Save();
			}
		}

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase db = Singleton.GetInstance().Context;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return;
					ChangeUser(user, db);
					DeleteNotification(user, db);
					if (user.IsAdmin >= 2)
					{
						botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "⚙Настройки бота⚙", "1505", user, replyMarkup: inlineButton.SettingBotLvl2(user));
					}
					else if (user.IsAdmin == 1)
					{
						botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "⚙️Настройки бота⚙️", "1505", user, replyMarkup: inlineButton.SettingBot);
					}
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class SettingBot : AbsCommand
	{
		public override System.String Name { get; set; } = CommandText.SettingBot;

		private InlineButton inlineButton = new InlineButton();

		public override void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase db = Singleton.GetInstance().Context;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return;

					if (user.IsAdmin >= 2)
					{
						System.String temp = "\nКоманды:\n /Ban [ID] [кол-во дней] | /Ban - переслать сообщение.\n/UnBan [ID] | /UnBan - переслать сообщение.\n/Kick [ID] | /Kick - переслать сообщение.(Команда работает в чатах UBC)\n/ThisAdminChannel! - сделать данную группу для администраторов.\n/AddThisChannel! - добавить канал в базу данных.";
						botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "⚙️Настройки бота⚙️" + temp, "1538", user, replyMarkup: inlineButton.SettingBotLvl2(user));
					}
					else if (user.IsAdmin == 1)
					{
						System.String temp = "\nКоманды:\n /Ban [ID] [кол-во дней] | /Ban - переслать сообщение.\n/UnBan [ID] | /UnBan - переслать сообщение.\n/Kick [ID] | /Kick - переслать сообщение.(Команда работает в чатах UBC)";
						botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "⚙️Настройки бота⚙️" + temp, "1542", user, replyMarkup: inlineButton.SettingBot);
					}
					else
					{
						botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "Вы больше не адмнистратор!", "1547", user, replyMarkup: inlineButton.Accaunt);
					}
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class BanUser : AbsCommand
	{
		public override System.String Name { get; set; } = CommandText.BanUser;

		private InlineButton inlineButton = new InlineButton();

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase db = Singleton.GetInstance().Context;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return;
					if (user.IsAdmin > 0)
					{
						user.Chain = 1050;
						db.Save();
						botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "⚰️Забанить участника⚰️", "1576", user, replyMarkup: inlineButton.SearchAdminPanel);
					}
					else
					{
						botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "Вы больше не адмнистратор!", "1580", user, replyMarkup: inlineButton.Accaunt);
					}
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class UnBanUser : AbsCommand
	{
		public override System.String Name { get; set; } = CommandText.UnBanUser;

		private InlineButton inlineButton = new InlineButton();

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase db = Singleton.GetInstance().Context;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return;
					if (user.IsAdmin > 0)
					{
						user.Chain = 1150;
						db.Save();

						botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "💉Разбанить участника💉", "1610", user, replyMarkup: inlineButton.SearchAdminPanel);
					}
					else
					{
						botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "Вы больше не адмнистратор!", "1514", user, replyMarkup: inlineButton.Accaunt);
					}
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class Flud : AbsCommand
	{
		public override System.String Name { get; set; } = CommandText.Flud;

		private InlineButton inlineButton = new InlineButton();

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase db = Singleton.GetInstance().Context;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return;
					if (user.IsAdmin > 0)
					{
						Settings settings = db.GetSettings();
						//if (settings.Timer < System.DateTime.Now)
						//{
						//	user.Chain = 1005;
						//	botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "⏳Флуд⏳\nВведите время продолжительности флуда: ", replyMarkup: inlineButton.BackToSettingAdmin);
						//	user.MessageID = _message.Message.MessageId;
						//	db.Save();
						//}
						//else
						//{
						//	botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "⏳Флуд⏳", replyMarkup: inlineButton.FludAdmin);
						//}
						if (settings.IsBanOrKickOrMutFlud == 0)
						{
							botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "Выберете за что наказывать: ", "1656", user, replyMarkup: inlineButton.CountFludBanIsBan(message));
						}
						else if (settings.IsBanOrKickOrMutFlud == 1)
						{
							botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "Выберете за что наказывать: ", "1660", user, replyMarkup: inlineButton.CountFludBanIsKick(message));
						}
						else
						{
							botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "Выберете за что наказывать: ", "1664", user, replyMarkup: inlineButton.CountFludBanIsMut(message));
						}
					}
					else
					{
						botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "Вы больше не адмнистратор!", "1669", user, replyMarkup: inlineButton.Accaunt);
					}
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class CountBanFludMinus : AbsCommand
	{
		public override System.String Name { get; set; } = CommandText.CountBanFludMinus;

		private InlineButton inlineButton = new InlineButton();

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase db = Singleton.GetInstance().Context;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return;
					if (user.IsAdmin > 0)
					{
						Settings settings = db.GetSettings();
						if (settings.CountBanFlud != 0)
						{
							settings.CountBanFlud--;
							db.Save();

							if (settings.IsBanOrKickOrMutFlud == 0)
							{
								botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "Выберете за что наказывать: ", "1705", user, replyMarkup: inlineButton.CountFludBanIsBan(message));
							}
							if (settings.IsBanOrKickOrMutFlud == 1)
							{
								botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "Выберете за что наказывать: ", "1709", user, replyMarkup: inlineButton.CountFludBanIsKick(message));
							}
							else if (settings.IsBanOrKickOrMutFlud == 2)
							{
								botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "Выберете за что наказывать: ", "1713", user, replyMarkup: inlineButton.CountFludBanIsMut(message));
							}
						}
					}
					else
					{
						botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "Вы больше не адмнистратор!", "1720", user, replyMarkup: inlineButton.Accaunt);
					}
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class CountBanFludPlus : AbsCommand
	{
		public override System.String Name { get; set; } = CommandText.CountBanFludPlus;

		private InlineButton inlineButton = new InlineButton();

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase db = Singleton.GetInstance().Context;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return;
					if (user.IsAdmin > 0)
					{
						Settings settings = db.GetSettings();
						settings.CountBanFlud++;
						db.Save();

						if (settings.IsBanOrKickOrMutFlud == 0)
						{
							botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "Выберете за что наказывать: ", "1753", user, replyMarkup: inlineButton.CountFludBanIsBan(message));
						}
						else if (settings.IsBanOrKickOrMutFlud == 1)
						{
							botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "Выберете за что наказывать: ", "1757", user, replyMarkup: inlineButton.CountFludBanIsKick(message));
						}
						else if (settings.IsBanOrKickOrMutFlud == 2)
						{
							botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "Выберете за что наказывать: ", "1761", user, replyMarkup: inlineButton.CountFludBanIsMut(message));
						}
					}
					else
					{
						botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "Вы больше не адмнистратор!", "1766", user, replyMarkup: inlineButton.Accaunt);
					}
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class IsBanFlud : AbsCommand
	{
		public override System.String Name { get; set; } = CommandText.IsBanFlud;

		private InlineButton inlineButton = new InlineButton();

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase db = Singleton.GetInstance().Context;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return;
					if (user.IsAdmin > 0)
					{
						Settings settings = db.GetSettings();

						settings.IsBanOrKickOrMutFlud = 0;
						db.Save();

						botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "Выберете за что наказывать: ", "1800", user, replyMarkup: inlineButton.CountFludBanIsBan(message));
					}
					else
					{
						botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "Вы больше не адмнистратор!", "1806", user, replyMarkup: inlineButton.Accaunt);
					}
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class IsKickFlud : AbsCommand
	{
		public override System.String Name { get; set; } = CommandText.IsKickFlud;

		private InlineButton inlineButton = new InlineButton();

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase db = Singleton.GetInstance().Context;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return;
					if (user.IsAdmin > 0)
					{
						Settings settings = db.GetSettings();
						settings.IsBanOrKickOrMutFlud = 1;
						db.Save();

						botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "Выберете за что наказывать: ", "1838", user, replyMarkup: inlineButton.CountFludBanIsKick(message));
					}
					else
					{
						botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "Вы больше не адмнистратор!", "1843", user, replyMarkup: inlineButton.Accaunt);
					}
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class IsMutFlud : AbsCommand
	{
		public override System.String Name { get; set; } = CommandText.IsMutFlud;

		private InlineButton inlineButton = new InlineButton();

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase db = Singleton.GetInstance().Context;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return;
					if (user.IsAdmin > 0)
					{
						Settings settings = db.GetSettings();
						settings.IsBanOrKickOrMutFlud = 2;
						db.Save();

						botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "Выберете за что наказывать: ", "1875", user, replyMarkup: inlineButton.CountFludBanIsMut(message));
					}
					else
					{
						botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "Вы больше не адмнистратор!", "1880", user, replyMarkup: inlineButton.Accaunt);
					}
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class MathBan : AbsCommand
	{
		public override System.String Name { get; set; } = CommandText.MathBan;

		private InlineButton inlineButton = new InlineButton();

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase db = Singleton.GetInstance().Context;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return;
					if (user.IsAdmin > 0)
					{
						Settings settings = db.GetSettings();
						if (settings.IsBanOrKicOrMutkMat == 0)
						{
							botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "Выберете за что наказывать: ", "1911", user, replyMarkup: inlineButton.CountMatBanIsBan(message));
						}
						else if (settings.IsBanOrKicOrMutkMat == 1)
						{
							botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "Выберете за что наказывать: ", "1915", user, replyMarkup: inlineButton.CountMatBanIsKick(message));
						}
						else
						{
							botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "Выберете за что наказывать: ", "1919", user, replyMarkup: inlineButton.CountMatBanIsMut(message));
						}
					}
					else
					{
						botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "Вы больше не адмнистратор!", "1925", user, replyMarkup: inlineButton.Accaunt);
					}
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class CountBanMatMinus : AbsCommand
	{
		public override System.String Name { get; set; } = CommandText.CountBanMatMinus;

		private InlineButton inlineButton = new InlineButton();

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase db = Singleton.GetInstance().Context;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return;
					if (user.IsAdmin > 0)
					{
						Settings settings = db.GetSettings();
						if (settings.CountBanMat != 0)
						{
							settings.CountBanMat--;
							db.Save();

							if (settings.IsBanOrKicOrMutkMat == 0)
							{
								botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "Выберете за что наказывать: ", "1962", user, replyMarkup: inlineButton.CountMatBanIsBan(message));
							}
							else if (settings.IsBanOrKicOrMutkMat == 1)
							{
								botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "Выберете за что наказывать: ", "1966", user, replyMarkup: inlineButton.CountMatBanIsKick(message));
							}
							else if (settings.IsBanOrKicOrMutkMat == 2)
							{
								botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "Выберете за что наказывать: ", "1970", user, replyMarkup: inlineButton.CountMatBanIsMut(message));
							}
						}
					}
					else
					{
						botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "Вы больше не адмнистратор!", "1976", user, replyMarkup: inlineButton.Accaunt);
					}
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class CountBanMatPlus : AbsCommand
	{
		public override System.String Name { get; set; } = CommandText.CountBanMatPlus;

		private InlineButton inlineButton = new InlineButton();

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase db = Singleton.GetInstance().Context;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return;
					if (user.IsAdmin > 0)
					{
						Settings settings = db.GetSettings();
						settings.CountBanMat++;
						db.Save();
						if (settings.IsBanOrKicOrMutkMat == 0)
						{
							botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "Выберете за что наказывать: ", "2008", user, replyMarkup: inlineButton.CountMatBanIsBan(message));
						}
						else if (settings.IsBanOrKicOrMutkMat == 1)
						{
							botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "Выберете за что наказывать: ", "2012", user, replyMarkup: inlineButton.CountMatBanIsKick(message));
						}
						else if (settings.IsBanOrKicOrMutkMat == 2)
						{
							botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "Выберете за что наказывать: ", "2016", user, replyMarkup: inlineButton.CountMatBanIsMut(message));
						}
					}
					else
					{
						botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "Вы больше не адмнистратор!", "2021", user, replyMarkup: inlineButton.Accaunt);
					}
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class IsBanMat : AbsCommand
	{
		public override System.String Name { get; set; } = CommandText.IsBanMat;

		private InlineButton inlineButton = new InlineButton();

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase db = Singleton.GetInstance().Context;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return;
					if (user.IsAdmin > 0)
					{
						Settings settings = db.GetSettings();

						settings.IsBanOrKicOrMutkMat = 0;
						db.Save();

						botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "Выберете за что наказывать: ", "2055", user, replyMarkup: inlineButton.CountMatBanIsBan(message));
					}
					else
					{
						botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "Вы больше не адмнистратор!", "2055", user, replyMarkup: inlineButton.Accaunt);
					}
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class IsKickMat : AbsCommand
	{
		public override System.String Name { get; set; } = CommandText.IsKickMat;

		private InlineButton inlineButton = new InlineButton();

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase db = Singleton.GetInstance().Context;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return;
					if (user.IsAdmin > 0)
					{
						Settings settings = db.GetSettings();
						settings.IsBanOrKicOrMutkMat = 1;
						db.Save();

						botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "Выберете за что наказывать: ", "2093", user, replyMarkup: inlineButton.CountMatBanIsKick(message));
					}
					else
					{
						botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "Вы больше не адмнистратор!", "2098", user, replyMarkup: inlineButton.Accaunt);
					}
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class IsMutMat : AbsCommand
	{
		public override System.String Name { get; set; } = CommandText.IsMutMat;

		private InlineButton inlineButton = new InlineButton();

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase db = Singleton.GetInstance().Context;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return;
					if (user.IsAdmin > 0)
					{
						Settings settings = db.GetSettings();
						settings.IsBanOrKicOrMutkMat = 2;
						db.Save();

						botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "Выберете за что наказывать: ", "2130", user, replyMarkup: inlineButton.CountMatBanIsMut(message));
					}
					else
					{
						botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "Вы больше не адмнистратор!", "2134", user, replyMarkup: inlineButton.Accaunt);
					}
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class ProcentBan : AbsCommand
	{
		public override System.String Name { get; set; } = CommandText.ProcentBan;

		private InlineButton inlineButton = new InlineButton();

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase db = Singleton.GetInstance().Context;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return;
					if (user.IsAdmin > 0)
					{
						botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "Выберете за что наказывать: ", "2164", user, replyMarkup: inlineButton.CountProcentBanIsBan(message));
					}
					else
					{
						botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "Вы больше не адмнистратор!", "2169", user, replyMarkup: inlineButton.Accaunt);
					}
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class CountBanProcentMinus : AbsCommand
	{
		public override System.String Name { get; set; } = CommandText.CountBanProcentMinus;

		private InlineButton inlineButton = new InlineButton();

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase db = Singleton.GetInstance().Context;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return;
					if (user.IsAdmin > 0)
					{
						Settings settings = db.GetSettings();
						if (settings.ProcentMessage != 0.0)
						{
							settings.ProcentMessage -= 0.05;
							settings.ProcentMessage = System.Math.Round(settings.ProcentMessage, 2);
							db.Save();

							botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "Выберете за что наказывать: ", "2206", user, replyMarkup: inlineButton.CountProcentBanIsBan(message));
						}
					}
					else
					{
						botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "Вы больше не адмнистратор!", "2212", user, replyMarkup: inlineButton.Accaunt);
					}
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class CountBanProcentPlus : AbsCommand
	{
		public override System.String Name { get; set; } = CommandText.CountBanProcentPlus;

		private InlineButton inlineButton = new InlineButton();

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase db = Singleton.GetInstance().Context;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return;
					if (user.IsAdmin > 0)
					{
						Settings settings = db.GetSettings();
						if (settings.ProcentMessage != 1.00)
						{
							settings.ProcentMessage += 0.05;
							settings.ProcentMessage = System.Math.Round(settings.ProcentMessage, 2);

							db.Save();

							botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "Выберете за что наказывать: ", "2247", user, replyMarkup: inlineButton.CountProcentBanIsBan(message));
						}
					}
					else
					{
						botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "Вы больше не адмнистратор!", "2253", user, replyMarkup: inlineButton.Accaunt);
					}
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class LinkBan : AbsCommand
	{
		public override System.String Name { get; set; } = CommandText.LickBan;

		private InlineButton inlineButton = new InlineButton();

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase db = Singleton.GetInstance().Context;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return;
					if (user.IsAdmin > 0)
					{
						Settings settings = db.GetSettings();
						if (settings.IsBanOrKickOrMutLink == 0)
						{
							botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "Выберете за что наказывать: ", "2284", user, replyMarkup: inlineButton.CountLinkBanIsBan(message));
						}
						else if (settings.IsBanOrKickOrMutLink == 1)
						{
							botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "Выберете за что наказывать: ", "2288", user, replyMarkup: inlineButton.CountLinkBanIsKick(message));
						}
						else
						{
							botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "Выберете за что наказывать: ", "2292", user, replyMarkup: inlineButton.CountLinkBanIsMut(message));
						}
					}
					else
					{
						botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "Вы больше не адмнистратор!", "2297", user, replyMarkup: inlineButton.Accaunt);
					}
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class CountBanPLinkMinus : AbsCommand
	{
		public override System.String Name { get; set; } = CommandText.CountBanLinkMinus;

		private InlineButton inlineButton = new InlineButton();

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase db = Singleton.GetInstance().Context;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return;
					if (user.IsAdmin > 0)
					{
						Settings settings = db.GetSettings();
						if (settings.CountLink != 0)
						{
							settings.CountLink--;
							db.Save();

							if (settings.IsBanOrKickOrMutLink == 0)
							{
								botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "Выберете за что наказывать: ", "2333", user, replyMarkup: inlineButton.CountLinkBanIsBan(message));
							}
							else if (settings.IsBanOrKickOrMutLink == 1)
							{
								botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "Выберете за что наказывать: ", "2336", user, replyMarkup: inlineButton.CountLinkBanIsKick(message));
							}
							else if (settings.IsBanOrKickOrMutLink == 2)
							{
								botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "Выберете за что наказывать: ", "2341", user, replyMarkup: inlineButton.CountLinkBanIsMut(message));
							}
						}
					}
					else
					{
						botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "Вы больше не адмнистратор!", "2347", user, replyMarkup: inlineButton.Accaunt);
					}
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class CountBanLinkPlus : AbsCommand
	{
		public override System.String Name { get; set; } = CommandText.CountBanLinkPlus;

		private InlineButton inlineButton = new InlineButton();

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase db = Singleton.GetInstance().Context;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return;
					if (user.IsAdmin > 0)
					{
						Settings settings = db.GetSettings();

						settings.CountLink++;

						db.Save();
						if (settings.IsBanOrKickOrMutLink == 0)
						{
							botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "Выберете за что наказывать: ", "2381", user, replyMarkup: inlineButton.CountLinkBanIsBan(message));
						}
						else if (settings.IsBanOrKickOrMutLink == 1)
						{
							botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "Выберете за что наказывать: ", "2385", user, replyMarkup: inlineButton.CountLinkBanIsKick(message));
						}
						else if (settings.IsBanOrKickOrMutLink == 2)
						{
							botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "Выберете за что наказывать: ", "2389", user, replyMarkup: inlineButton.CountLinkBanIsMut(message));
						}
					}
					else
					{
						botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "Вы больше не адмнистратор!", "2394", user, replyMarkup: inlineButton.Accaunt);
					}
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class IsBanLink : AbsCommand
	{
		public override System.String Name { get; set; } = CommandText.IsBanLink;

		private InlineButton inlineButton = new InlineButton();

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase db = Singleton.GetInstance().Context;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return;
					if (user.IsAdmin > 0)
					{
						Settings settings = db.GetSettings();

						settings.IsBanOrKickOrMutLink = 0;
						db.Save();

						botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "Выберете за что наказывать: ", "1970", user, replyMarkup: inlineButton.CountLinkBanIsBan(message));
					}
					else
					{
						botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "Вы больше не адмнистратор!", "1970", user, replyMarkup: inlineButton.Accaunt);
					}
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class IsKickLink : AbsCommand
	{
		public override System.String Name { get; set; } = CommandText.IsKickLink;

		private InlineButton inlineButton = new InlineButton();

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase db = Singleton.GetInstance().Context;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return;
					if (user.IsAdmin > 0)
					{
						Settings settings = db.GetSettings();
						settings.IsBanOrKickOrMutLink = 1;
						db.Save();

						botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "Выберете за что наказывать: ", "1970", user, replyMarkup: inlineButton.CountLinkBanIsKick(message));
					}
					else
					{
						botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "Вы больше не адмнистратор!", "1970", user, replyMarkup: inlineButton.Accaunt);
					}
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class IsMutLink : AbsCommand
	{
		public override System.String Name { get; set; } = CommandText.IsMutLink;

		private InlineButton inlineButton = new InlineButton();

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase db = Singleton.GetInstance().Context;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return;
					if (user.IsAdmin > 0)
					{
						Settings settings = db.GetSettings();
						settings.IsBanOrKickOrMutLink = 2;
						db.Save();

						botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "Выберете за что наказывать: ", "1970", user, replyMarkup: inlineButton.CountLinkBanIsMut(message));
					}
					else
					{
						botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "Вы больше не адмнистратор!", "1970", user, replyMarkup: inlineButton.Accaunt);
					}
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class StopFlud : AbsCommand
	{
		public override System.String Name { get; set; } = CommandText.StopFlud;

		private InlineButton inlineButton = new InlineButton();

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase db = Singleton.GetInstance().Context;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return;
					if (user.IsAdmin > 0)
					{
						Settings settings = db.GetSettings();
						settings.Timer = System.DateTime.Now.AddHours(-1);

						botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "⏳Флуд остановлен⏳: ", "1970", user, replyMarkup: inlineButton.BackToSettingAdmin);
						user.MessageID = _message.Message.MessageId;
						db.Save();
					}
					else
					{
						botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "Вы больше не адмнистратор!", "1970", user, replyMarkup: inlineButton.Accaunt);
					}
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class AddAdmin : AbsCommand
	{
		public override System.String Name { get; set; } = CommandText.AddAdmin;

		private InlineButton inlineButton = new InlineButton();

		public override void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase db = Singleton.GetInstance().Context;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return;

					user.Chain = 1000;
					user.MessageID = _message.Message.MessageId;
					db.Save();
					System.String text = "";
					User[] users = db.GetUsers();
					System.Int32 count = 0;
					if (user.IsAdmin == 2)
					{
						foreach (User us in users)
						{
							if (us.IsAdmin > 0)
							{
								count++;
								text += count + ") " + us.FIO + " - " + "уровень: " + us.IsAdmin + ";\n";
							}
						}

						botClient.EditMessage(_message.From.Id, _message.Message.MessageId, text + "\nПерешлите сообщения человека или введите ID: ", "1970", user, replyMarkup: inlineButton.BackToSettingAdmin);
					}
					else if(user.IsAdmin == 3)
					{
						botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "Выберете администратора или перешлите сообщение или введите ID: ", "1970", user, replyMarkup: Advertising.InlineButton.AdminPanenLvl3Users());
					}
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class DeleteAdmin : AbsCommand
	{
		public override System.String Name { get; set; } = CommandText.DeleteAdmin;

		private InlineButton inlineButton = new InlineButton();

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase db = Singleton.GetInstance().Context;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return;

					user.Chain = 1001;
					user.MessageID = _message.Message.MessageId;
					db.Save();

					System.String text = "";
					User[] users = db.GetUsers();
					System.Int32 count = 0;
					if (user.IsAdmin == 2)
					{
						foreach (User us in users)
						{
							if (us.IsAdmin > 0)
							{
								count++;
								text += count + ") " + us.FIO + " - " + "уровень: " + us.IsAdmin + ";\n";
							}
						}

						botClient.EditMessage(_message.From.Id, _message.Message.MessageId, text + "\nПерешлите сообщения человека или введите ID: ", "1970", user, replyMarkup: inlineButton.BackToSettingAdmin);
					}
					else if (user.IsAdmin == 3)
					{
						botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "Выберете администратора или перешлите сообщение или введите ID: ", "1970", user, replyMarkup: Advertising.InlineButton.AdminPanenLvl3Users());
					}
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class CountPost : AbsCommand
	{
		public override System.String Name { get; set; } = CommandText.CountPost;

		private InlineButton inlineButton = new InlineButton();

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase db = Singleton.GetInstance().Context;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return;
					Settings settings = db.GetSettings();

					user.Chain = 1002;
					user.MessageID = _message.Message.MessageId;
					db.Save();

					botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "Количество постов в день: " + settings.CountPost + "\nВведите общее значение для всех чатов или выберете отдельный чат: ", "1970", user, replyMarkup: inlineButton.LimitedChannelMenu());
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class AddWord : AbsCommand
	{
		public override System.String Name { get; set; } = CommandText.AddWord;

		private InlineButton inlineButton = new InlineButton();

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase db = Singleton.GetInstance().Context;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return;
					System.String temp = "Запретные слова:\n ";
					user.Chain = 1003;
					user.MessageID = _message.Message.MessageId;
					db.Save();

					List<Word> words = Singleton.GetInstance().Context._words.ToList();

					foreach (Word i in words)
					{
						temp += i.word + "\n";
					}
					temp += "Введите запретное слово: ";
					botClient.EditMessage(_message.From.Id, _message.Message.MessageId, temp, "1970", user, replyMarkup: inlineButton.BackToSettingAdmin);
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class DeleteWord : AbsCommand
	{
		public override System.String Name { get; set; } = CommandText.DeleteWord;

		private InlineButton inlineButton = new InlineButton();

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase db = Singleton.GetInstance().Context;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return;
					System.String temp = "Запретные слова:\n ";
					user.Chain = 1009;

					List<Word> words = Singleton.GetInstance().Context._words.ToList();

					foreach (Word i in words)
					{
						temp += i.word + "\n";
					}
					temp += "Введите запретное слово для удаления: ";
					botClient.EditMessage(_message.From.Id, _message.Message.MessageId, temp, "1970", user, replyMarkup: inlineButton.BackToSettingAdmin);

					db.Save();
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class AddUser : AbsCommand
	{
		public override System.String Name { get; set; } = CommandText.AddUser;

		private InlineButton inlineButton = new InlineButton();

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase db = Singleton.GetInstance().Context;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return;

					user.Chain = 1004;
					user.MessageID = _message.Message.MessageId;
					db.Save();
					Settings settings = db.GetSettings();
					botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "Старое значение: " + settings.AddUser + "\nВведиете кол-во человек которых нужно добавить чтобы писать в чате: ", "1970", user, replyMarkup: inlineButton.BackToSettingAdmin);
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class PayBan : AbsCommand
	{
		public override System.String Name { get; set; } = CommandText.PayBan;

		private InlineButton inlineButton = new InlineButton();

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase db = Singleton.GetInstance().Context;
			try
			{
				User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return;
				botClient.DeleteMessage(_message.From.Id, _message.Message.MessageId, "2766 - PayBan");
				botClient.DeleteMessage(_message.From.Id, user.MessageID, "2779 - PayBan");
				LabeledPrice labledPrice = new LabeledPrice
				{
					Amount = 10,
					Label = "Бан"
				};
				List<LabeledPrice> price = new List<LabeledPrice>
				{
					labledPrice
				};
				//await botClient.SendInvoiceAsync(_message.From.Id, "Оплатите бан", "Описание", "Бан!", "i56982357197", "Ban", "UAH", price);

				Message mes = await botClient.SendInvoiceAsync(
				chatId: _message.From.Id,
				title: "Оплата за разблокировку",
				description: "Вы получили бан, что бы снять бан вам нужно оплатит 100 грн!",
				payload: "Pay is correct",
				providerToken: "635983722:LIVE:i29496402528",
				startParameter: "HEllo",
				currency: "UAH",
				prices: new[] { new LabeledPrice("price", 10000), },
				replyMarkup: inlineButton.Payment
				);
				user.MessageID = mes.MessageId;
				db.Save();
			}
			catch (System.Exception ex)
			{
				Log.Logging(ex);
			}
		}
	}

	internal class PayConfirm : AbsCommand
	{
		public override System.String Name { get; set; } = CommandText.PayConfirm;

		private InlineButton inlineButton = new InlineButton();

		private async void DeleteMessage(TelegramBotClient botClient, CallbackQuery _message)
		{
			for (Int32 i = 0; i < 20; i++)
			{
				botClient.DeleteMessage(_message.From.Id, _message.Message.MessageId - i, "2821 - PayConfirm");
				botClient.DeleteMessage(_message.From.Id, _message.Message.MessageId + i, "2821 - PayConfirm");
			}
		}

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase db = Singleton.GetInstance().Context;
			try
			{
				User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return;
				botClient.DeleteMessage(_message.From.Id, _message.Message.MessageId, "2766 - Command");

				await Task.Run(() => DeleteMessage(botClient, _message));

				Message mes = await botClient.SendInvoiceAsync(
				chatId: _message.From.Id,
				title: "Оплата абонимент на месяц к чатам UBC",
				description: @"Вы можете не добавлять 3-их людей в чаты, а просто купить абонимент. 

1.Q. Что это дает?
1.A. Снимает ограничение писать максимум 1 сообщение в день. 
2.Q. На какое время?
2.A. На месяц.
3. Если вы нарушите правила и будете забанены - абонимент снимается.",
				payload: "Pay Confirm User",
				providerToken: "635983722:LIVE:i29496402528",
				startParameter: "HEllo",
				currency: "UAH",
				prices: new[] { new LabeledPrice("price", 2500), },
				replyMarkup: inlineButton.PaymentUser
				);
				user.MessageID = mes.MessageId;
				db.Save();
			}
			catch (System.Exception ex)
			{
				Log.Logging(ex);
			}
		}
	}
}