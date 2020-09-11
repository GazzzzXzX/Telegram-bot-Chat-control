using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCore
{
	internal class Kick : ICommandSlash
	{
		public System.String Name { get; } = "/Kick";

		private System.String Text { get; set; }

		public System.Boolean Equals(System.String CommandName)
		{
			if (CommandName == "/Kick" || CommandName == null)
			{
				Text = null;
				return Name.Equals(CommandName);
			}
			else
			{
				try
				{
					System.String[] words = CommandName.Split(new System.Char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);

					Text = CommandName.Replace(words[0], "");

					return Name.Equals(words[0]);
				}
				catch
				{
					return false;
				}
			}
		}

		public async void Execute(TelegramBotClient botClient, System.Object message)
		{
			Message _message = message as Message;
			try
			{
				DataBase db = Singleton.GetInstance().Context;
				User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return;
				Settings setting = db.GetSettings();

				botClient.DeleteMessage(_message.Chat.Id, _message.MessageId, "44");
				if (Text != null && user.IsAdmin > 0)
				{
					User userTwo = db.GetUser(System.Convert.ToInt32(Text));

					if (userTwo != null)
					{
						if (user.IsAdmin == 3)
						{
							userTwo.BanDate = System.DateTime.Now.AddDays(30);
							userTwo.BanDescript = "Вы были забанены администрацией UBC!";
							userTwo.PayConfirm = false;
							userTwo.PayDate = System.DateTime.Today;
							db.Save();
							IsKick.ThisKick(botClient, userTwo);

							System.String temp = "Администратор " + user.IsAdmin + " уровня: @" + user.Username + " кикнул пользователя!" + "\nID: " + userTwo.ID + "\nФИО: " + userTwo.FIO + "\nНомер: " + user.Number;

							botClient.SendText(setting.ChannelAdmin, temp, userTwo, true);
						}
						else if (userTwo.IsAdmin != 2 && userTwo.IsAdmin != 3)
						{
							userTwo.BanDate = System.DateTime.Now.AddDays(30);
							userTwo.BanDescript = "Вы были забанены администрацией UBC!";
							userTwo.PayConfirm = false;
							userTwo.PayDate = System.DateTime.Today;
							db.Save();
							IsKick.ThisKick(botClient, userTwo);

							System.String temp = "Администратор " + user.IsAdmin + " уровня: @" + user.Username + " кикнул пользователя!" + "\nID: " + userTwo.ID + "\nФИО: " + userTwo.FIO + "\nНомер: " + user.Number;

							botClient.SendText(setting.ChannelAdmin, temp, userTwo, true);
						}
						else
						{
							user.IsAdmin = 0;
							if (user.BanDate.Date < System.DateTime.Today)
							{
								user.BanDate = System.DateTime.Now;
							}
							user.BanDate = user.BanDate.AddDays(30);
							System.String temp = "Админ " + user.FIO + "\nID: " + user.ID + "\nПопытался заблокировать администратора!\nС данного администратора снята админка, так же он был забанен во всех чатах! Если бан был выдан случайно пропишите /UbBan " + user.ID;
							IsBanUser.ThisBan(botClient, _message, user, setting);

							botClient.SendText(setting.ChannelAdmin, temp, user, true);
						}
						db.Save();
					}
					else
					{
						botClient.SendText(user.ID, "Пользователь не найден!");
					}
				}
				else if (user.IsAdmin > 0)
				{
					botClient.SendText(user.ID, "Перешлите сообщения от пользователя которого хотите кикнуть!");
					user.Chain = 9;
				}
			}
			catch (System.Exception ex)
			{
				Log.Logging(ex);
			}
		}
	}

	internal class UserBan : ICommandSlash
	{
		public System.String Name { get; } = "/Ban";

		private System.String Text { get; set; }
		private System.Int32 Count { get; set; }

		public System.Boolean Equals(System.String CommandName)
		{
			if (CommandName == "/Ban" || CommandName == null)
			{
				Text = null;
				return Name.Equals(CommandName);
			}
			else
			{
				try
				{
					System.String[] words = CommandName.Split(new System.Char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
					Text = words[1];
					Count = System.Convert.ToInt32(words[2]);

					return Name.Equals(words[0]);
				}
				catch
				{
					return false;
				}
			}
		}

		public async void Execute(TelegramBotClient botClient, System.Object message)
		{
			Message _message = message as Message;
			InlineButton inlineButton = new InlineButton();
			try
			{
				DataBase db = Singleton.GetInstance().Context;
				User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return;

				botClient.DeleteMessage(_message.Chat.Id, _message.MessageId, "135");

				if (Text != null && user.IsAdmin > 0)
				{
					User userTwo = db.GetUser(System.Convert.ToInt32(Text));
					Settings setting = db.GetSettings();

					if (userTwo != null)
					{
						if (user.IsAdmin == 3)
						{
							if (userTwo.BanDate.Date < System.DateTime.Today)
							{
								userTwo.BanDate = System.DateTime.Now;
							}
							userTwo.BanDate = userTwo.BanDate.AddDays(Count);
							userTwo.BanDescript = "Вы были забанены администрацией UBC!";
							userTwo.PayConfirm = false;
							userTwo.PayDate = System.DateTime.Today;

							IsBanUser.ThisBan(botClient, _message, userTwo, setting);

							System.String temp = "Администратор " + user.IsAdmin + " уровня: @" + user.Username + " забанил пользователя на " + Count + " дней!" + "\nID: " + userTwo.ID + "\nФИО: " + userTwo.FIO + "\nНомер: " + user.Number;

							await botClient.SendTextMessageAsync(setting.ChannelAdmin, temp);
						}
						else if (userTwo.IsAdmin != 2 && userTwo.IsAdmin != 3)
						{
							if (userTwo.BanDate.Date < System.DateTime.Today)
							{
								userTwo.BanDate = System.DateTime.Now;
							}
							userTwo.BanDate = userTwo.BanDate.AddDays(Count);
							userTwo.BanDescript = "Вы были забанены администрацией UBC!";
							userTwo.PayConfirm = false;
							userTwo.PayDate = System.DateTime.Today;

							IsBanUser.ThisBan(botClient, _message, userTwo, setting);

							System.String temp = "Администратор " + user.IsAdmin + " уровня: @" + user.Username + " забанил пользователя на " + Count + " дней!" + "\nID: " + userTwo.ID + "\nФИО: " + userTwo.FIO + "\nНомер: " + user.Number;

							await botClient.SendTextMessageAsync(setting.ChannelAdmin, temp);
						}
						else
						{
							user.IsAdmin = 0;
							if (user.BanDate.Date < System.DateTime.Today)
							{
								user.BanDate = System.DateTime.Now;
							}
							user.BanDate = user.BanDate.AddDays(Count);
							IsBanUser.ThisBan(botClient, _message, user, setting);

							System.String temp = "Администратор " + user.IsAdmin + " уровня: @" + user.Username + " пытался забанить другого администратора на " + Count + " дней!" + "\nID: " + userTwo.ID + "\nФИО: " + userTwo.FIO + "\nНомер: " + user.Number + "\nС данного администратора снята админка, так же он был забанен во всех чатах! Если бан был выдан случайно пропишите /UbBan " + user.ID;

							Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup answer = inlineButton.AdminPanelAppeal(message, user.ID, _message.Text);
							await botClient.SendTextMessageAsync(setting.ChannelAdmin, temp, replyMarkup: answer);
						}

						db.Save();
					}
					else
					{
						try
						{
							Message mes = await botClient.EditMessageTextAsync(user.ID, user.MessageID, "Пользователь не найден!", replyMarkup: inlineButton.BackToSettingAdmin);
							user.MessageID = mes.MessageId;
						}
						catch
						{
							Message mes = await botClient.SendTextMessageAsync(user.ID, "Пользователь не найден!", replyMarkup: inlineButton.BackToSettingAdmin);
							user.MessageID = mes.MessageId;
						}
						db.Save();
					}
				}
				else if (Name == "/Ban" && user.IsAdmin > 0)
				{
					try
					{
						Message mes = await botClient.EditMessageTextAsync(user.ID, user.MessageID, "Перешлите сообщения от пользователя которого хотите забанить!", replyMarkup: inlineButton.BackToSettingAdmin);
						user.Chain = 1050;
						user.MessageID = mes.MessageId;
					}
					catch
					{
						Message mes = await botClient.SendTextMessageAsync(user.ID, "Перешлите сообщения от пользователя которого хотите забанить!", replyMarkup: inlineButton.BackToSettingAdmin);
						user.Chain = 1050;
						user.MessageID = mes.MessageId;
					}

					db.Save();
				}
			}
			catch (System.Exception ex)
			{
				Log.Logging(ex);
			}
		}
	}

	internal class UnUserBan : ICommandSlash
	{
		public System.String Name { get; } = "/UnBan";

		private System.String Text { get; set; }

		public System.Boolean Equals(System.String CommandName)
		{
			if (CommandName == "/UnBan" || CommandName == null)
			{
				Text = null;
				return Name.Equals(CommandName);
			}
			else
			{
				try
				{
					System.String[] words = CommandName.Split(new System.Char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
					Text = CommandName.Replace(words[0], "");

					return Name.Equals(words[0]);
				}
				catch
				{
					return false;
				}
			}
		}

		public async void Execute(TelegramBotClient botClient, System.Object message)
		{
			Message _message = message as Message;
			InlineButton inlineButton = new InlineButton();
			try
			{
				DataBase db = Singleton.GetInstance().Context;
				User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return;

				botClient.DeleteMessage(_message.Chat.Id, _message.MessageId, "258");

				if (Text != null && user.IsAdmin > 0)
				{
					User userTwo = db.GetUser(System.Convert.ToInt64(Text));

					if (userTwo != null)
					{
						userTwo.BanDate = System.DateTime.Now;
						IsUnBan.ThisUnBan(botClient, userTwo);
						db.Save();

						IsUnBan.ThisUnBan(botClient, userTwo);

						Settings setting = db.GetSettings();

						System.String temp = "Администратор " + user.IsAdmin + " уровня: @" + user.Username + " разбанил пользователя!" + "\nID: " + userTwo.ID + "\nФИО: " + userTwo.FIO + "\nНомер: " + user.Number;
						await botClient.SendTextMessageAsync(setting.ChannelAdmin, temp);
					}
					else
					{
						try
						{
							Message mes = await botClient.EditMessageTextAsync(user.ID, user.MessageID, "Пользователь не найден!", replyMarkup: inlineButton.BackToSettingAdmin);
							user.MessageID = mes.MessageId;
						}
						catch
						{
							Message mes = await botClient.SendTextMessageAsync(user.ID, "Пользователь не найден!", replyMarkup: inlineButton.BackToSettingAdmin);
							user.MessageID = mes.MessageId;
						}
						db.Save();
					}
				}
				else if (user.IsAdmin > 0)
				{
					try
					{
						Message mes = await botClient.EditMessageTextAsync(user.ID, user.MessageID, "Перешлите сообщения от пользователя которого хотите разбанить!", replyMarkup: inlineButton.BackToSettingAdmin);
						user.Chain = 1150;
						user.MessageID = mes.MessageId;
					}
					catch
					{
						Message mes = await botClient.SendTextMessageAsync(user.ID, "Перешлите сообщения от пользователя которого хотите разбанить!", replyMarkup: inlineButton.BackToSettingAdmin);
						user.Chain = 1150;
						user.MessageID = mes.MessageId;
					}

					db.Save();
				}
			}
			catch (System.Exception ex)
			{
				Log.Logging(ex);
			}
		}
	}

	internal class AddGroup : ICommandSlash
	{
		public System.String Name { get; } = "/AddThisGroup!";

		public System.Boolean Equals(System.String CommandName) => Name.Equals(CommandName);

		public async void Execute(TelegramBotClient botClient, System.Object message)
		{
			Message _message = message as Message;
			try
			{
				DataBase db = Singleton.GetInstance().Context;

				User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return;
				try
				{
					await botClient.DeleteMessageAsync(_message.Chat.Id, _message.MessageId);
				}
				catch { }

				if (user.IsAdmin >= 2)
				{
					Singleton.GetInstance().Context._channels.Add(new Channel() { IDChannel = _message.Chat.Id });
					db.Save();
					await botClient.SendTextMessageAsync(_message.From.Id, "Чат добавлен!");
				}
			}
			catch (System.Exception ex)
			{
				Log.Logging(ex);
			}
		}
	}

	internal class AddChannel : ICommandSlash
	{
		public System.String Name { get; } = "/AddThisChannel!";
		private InlineButton inlineButton = new InlineButton();

		public System.Boolean Equals(System.String CommandName) => Name.Equals(CommandName);

		public void Execute(TelegramBotClient botClient, System.Object message)
		{
			Message _message = message as Message;
			try
			{
				DataBase db = Singleton.GetInstance().Context;

				User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return;

				botClient.DeleteMessage(_message.Chat.Id, _message.MessageId, "376");

				if (user.IsAdmin >= 2)
				{
					user.Chain = 1006;

					botClient.SendText(_message.From.Id, "Перешлите сообщение от канала: ", user, replyMarkup: inlineButton.BackToAccauntMenu);
					db.Save();
				}
			}
			catch (System.Exception ex)
			{
				Log.Logging(ex);
			}
		}
	}

	internal class GetAdmin : ICommandSlash
	{
		public System.String Name { get; } = "/GetAdmin!";
		private InlineButton inlineButton = new InlineButton();

		public System.Boolean Equals(System.String CommandName) => Name.Equals(CommandName);

		public async void Execute(TelegramBotClient botClient, System.Object message)
		{
			Message _message = message as Message;
			DataBase db = Singleton.GetInstance().Context;

			try
			{
				User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return;

				botClient.DeleteMessage(_message.Chat.Id, _message.MessageId, "413");

				botClient.DeleteMessage(_message.Chat.Id, user.MessageID, "414");

				user.Chain = 1008;

				Message mes = await botClient.SendTextMessageAsync(_message.From.Id, "Введите пароль от админки: ", replyMarkup:inlineButton.BackToAccauntMenu);
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