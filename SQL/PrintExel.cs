using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BotCore.SQL;
using BotCore.TelegramClient;
using ExcelLibrary.SpreadSheet;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;

namespace BotCore
{
	internal static class PrintExel
	{
		private static async void SendMessage(TelegramBotClient botClient, CallbackQuery _message, User user, String text)
		{
			InlineButton inlineButton = new InlineButton();
			using (FileStream stream = System.IO.File.Open(AppDomain.CurrentDomain.BaseDirectory + "\\newdoc.xls", FileMode.Open))
			{
				InputOnlineFile iof = new InputOnlineFile(stream)
				{
					FileName = text + ".xls"
				};
				botClient.DeleteMessage(_message.From.Id, user.MessageID, "21 - PrintExel");
				await botClient.SendDocumentAsync(_message.From.Id, iof);
				System.String temp = InfoUser.Info(user);
				botClient.SendText(_message.From.Id, temp, user, replyMarkup: inlineButton.AdminAccaunt);
			}
		}

		private static async void SendMessage(TelegramBotClient botClient, Message _message, User user, String text)
		{
			InlineButton inlineButton = new InlineButton();
			using (FileStream stream = System.IO.File.Open(AppDomain.CurrentDomain.BaseDirectory + "\\newdoc.xls", FileMode.Open))
			{
				InputOnlineFile iof = new InputOnlineFile(stream)
				{
					FileName = text + ".xls"
				};
				botClient.DeleteMessage(_message.From.Id, user.MessageID, "21 - PrintExel");
				await botClient.SendDocumentAsync(_message.From.Id, iof);
				System.String temp = InfoUser.Info(user);
				botClient.SendText(_message.From.Id, temp, user, replyMarkup: inlineButton.AdminAccaunt);
			}
		}

		public static void GetAllUsers(TelegramBotClient botClient, CallbackQuery _message, User user)
		{
			DataBase db = Singleton.GetInstance().Context;
			//создание файла
			String    files     = AppDomain.CurrentDomain.BaseDirectory + "\\newdoc.xls";
			Workbook  workbook  = new Workbook();
			Worksheet worksheet = new Worksheet("Пользователи");
			worksheet.Cells[0, 0] = new Cell("ФИО");
			worksheet.Cells[0, 1] = new Cell("Username");
			worksheet.Cells[0, 2] = new Cell("Номер");
			worksheet.Cells[0, 3] = new Cell("Дата регистрации");

			User[] Users = db.GetUsers();
			
			IQueryable<User> temp = Users.OrderByDescending(p => p.DateRegister).AsQueryable();
			Int32 count = 1;
			foreach (User ThisUser in temp)
			{
				worksheet.Cells[count + 1, 0] = new Cell(ThisUser.FIO);
				worksheet.Cells[count + 1, 1] = new Cell(ThisUser.Username);
				worksheet.Cells[count + 1, 2] = new Cell(ThisUser.Number);
				worksheet.Cells[count + 1, 3] = new Cell(ThisUser.DateRegister.Date.ToString("dd/MM/yyyy"));
				count++;
			}
			
			workbook.Worksheets.Add(worksheet);
			workbook.Save(files);
			SendMessage(botClient, _message, user, "Пользователи");
		}
		
		/// <summary>
		/// Аналитика по доходности в группе.
		/// </summary>
		/// <param name="analiticsPhrases"></param>
		public static void GetAnaliticsIncomeChannel(IncomeChannel[] incomes, TelegramBotClient botClient, CallbackQuery _message, User user)
		{
			DataBase db = Singleton.GetInstance().Context;
			//создание файла
			String files = AppDomain.CurrentDomain.BaseDirectory + "\\newdoc.xls";
			Workbook workbook = new Workbook();
			Worksheet worksheet = new Worksheet("Аналитика доходов");
			worksheet.Cells[0, 0] = new Cell("Группа");
			worksheet.Cells[0, 1] = new Cell("Доход");
			worksheet.Cells[0, 2] = new Cell("Дата");
			worksheet.Cells[0, 3] = new Cell("Общая доходность за все группы");

			Int32 count = 1;
			Single incomeChannel = 0;

			IQueryable<IncomeChannel> temp = incomes.OrderByDescending(p => p.DateTime).AsQueryable();


			foreach (IncomeChannel income in temp)
			{
				Channel channel = db.GetChannel(income.ChannelId);
				worksheet.Cells[count + 1, 0] = new Cell(channel.ChannelName.ToString());
				worksheet.Cells[count + 1, 1] = new Cell(income.SumIncome.ToString());
				worksheet.Cells[count + 1, 2] = new Cell(income.DateTime.ToString("d"));
				incomeChannel += income.SumIncome;
				count++;
			}
			worksheet.Cells[1, 3] = new Cell(incomeChannel.ToString());
			workbook.Worksheets.Add(worksheet);
			workbook.Save(files);
			SendMessage(botClient, _message, user, "Аналитика доходов");
		}

		/// <summary>
		/// Аналитика по доходности в группе и людей.
		/// </summary>
		/// <param name="analiticsPhrases"></param>
		public static void GetAnaliticsIncome(Income[] incomes, TelegramBotClient botClient, CallbackQuery _message, User user)
		{
			DataBase db = Singleton.GetInstance().Context;
			//создание файла
			String files = AppDomain.CurrentDomain.BaseDirectory + "\\newdoc.xls";
			Workbook workbook = new Workbook();
			Worksheet worksheet = new Worksheet("Аналитика доходов от пользователей");
			worksheet.Cells[0, 0] = new Cell("Группа");
			worksheet.Cells[0, 1] = new Cell("Пользователь");
			worksheet.Cells[0, 2] = new Cell("Доход");
			worksheet.Cells[0, 3] = new Cell("Дата");
			worksheet.Cells[0, 4] = new Cell("Общая доходность");

			Int32 count = 1;
			Single incomeChannel = 0;

			IQueryable<Income> temp = incomes.OrderByDescending(p => p.dateTime).AsQueryable();


			foreach (Income income in temp)
			{
				Channel channel = db.GetChannel(income.ChannelId);
				User ThisUser = db.GetUser(income.UserId);
				worksheet.Cells[count + 1, 0] = new Cell(channel.ChannelName.ToString());
				worksheet.Cells[count + 1, 1] = new Cell(ThisUser.FIO.ToString());
				worksheet.Cells[count + 1, 2] = new Cell(income.SumIncome.ToString());
				worksheet.Cells[count + 1, 3] = new Cell(income.dateTime.ToString("d"));
				incomeChannel += income.SumIncome;
				count++;
			}
			worksheet.Cells[1, 4] = new Cell(incomeChannel.ToString());
			workbook.Worksheets.Add(worksheet);
			workbook.Save(files);
			SendMessage(botClient, _message, user, "Аналитика доходов от пользователей");
		}

		/// <summary>
		/// Аналитика по доходности в группе и администраторов.
		/// </summary>
		/// <param name="analiticsPhrases"></param>
		public static void GetAnaliticsIncomeAdmin(IncomeChannelAdmin[] incomes, TelegramBotClient botClient, CallbackQuery _message, User user)
		{
			DataBase db = Singleton.GetInstance().Context;
			//создание файла
			String files = AppDomain.CurrentDomain.BaseDirectory + "\\newdoc.xls";
			Workbook workbook = new Workbook();
			Worksheet worksheet = new Worksheet("Аналитика зарплат администрации");
			worksheet.Cells[0, 0] = new Cell("Группа");
			worksheet.Cells[0, 1] = new Cell("Администратор");
			worksheet.Cells[0, 2] = new Cell("Доход");
			worksheet.Cells[0, 3] = new Cell("Дата");
			worksheet.Cells[0, 4] = new Cell("Общая зарплата");

			Int32 count = 1;
			Single incomeChannel = 0;

			IQueryable<IncomeChannelAdmin> temp = incomes.OrderByDescending(p => p.DateTime).AsQueryable();


			foreach (IncomeChannelAdmin income in temp)
			{
				Channel channel = db.GetChannel(income.ChannelId);
				User ThisUser = db.GetUser(income.UserId);
				worksheet.Cells[count + 1, 0] = new Cell(channel.ChannelName.ToString());
				worksheet.Cells[count + 1, 1] = new Cell(ThisUser.FIO.ToString());
				worksheet.Cells[count + 1, 2] = new Cell(income.SumIncome.ToString());
				worksheet.Cells[count + 1, 3] = new Cell(income.DateTime.ToString("d"));
				incomeChannel += income.SumIncome;
				count++;
			}
			worksheet.Cells[1, 4] = new Cell(incomeChannel.ToString());
			workbook.Worksheets.Add(worksheet);
			workbook.Save(files);
			SendMessage(botClient, _message, user, "Аналитика зарплат администрации");
		}

		/// <summary>
		/// Аналитика по добавленых пользователей
		/// </summary>
		/// <param name="analiticsPhrases"></param>
		public static async void GetAnaliticsInUserAdd(InvitedUser[] invitedUsers, TelegramBotClient botClient, CallbackQuery _message, User user)
		{
			DataBase db = Singleton.GetInstance().Context;
			//
			
			/*var client = new TLSharp.Core.TelegramClient(833968, "a6c65fd9460c44e42c5268b5601c849f");
			var isAuth = client.IsUserAuthorized();
			Console.WriteLine($"Файл авторизации существует: {isAuth}");
			client.ConnectAsync().Wait();
			
			ModelParserInfo temp2 = WorkingTelegramClient.ParserInfoAboutChannelAndSuperGroup("BuySellCarUBC", client).GetAwaiter().GetResult();

			foreach (var u2s in temp2.ChannelInfo.Users)
			{
				Console.WriteLine(u2s.FirstName);
			}*/
			
			//
			
			//создание файла
			String files = AppDomain.CurrentDomain.BaseDirectory + "\\newdoc.xls";
			Workbook workbook = new Workbook();
			Worksheet worksheet = new Worksheet("Аналитика по добавлению человек");
			worksheet.Cells[0, 0] = new Cell("Добавленый пользователь");
			worksheet.Cells[0, 1] = new Cell("Кто добавил");
			worksheet.Cells[0, 2] = new Cell("Группа");

			Int32 count = 1;
			Channel[] channels = db.GetChannels();

			IQueryable<InvitedUser> temp = invitedUsers.OrderByDescending(p => p.UserAddedId).AsQueryable();


			foreach (InvitedUser invitedUser in temp)
			{
				User Added = db.GetUser(invitedUser.UserAddedId);
				User WhoAdded = db.GetUser(invitedUser.UserWhoAddedId);

				if (Added != null)
				{
					worksheet.Cells[count, 0] = new Cell(value: Added.FIO + " " + Added.ID);
				}

				if (WhoAdded != null)
				{
					worksheet.Cells[count, 1] = new Cell(value: WhoAdded.FIO + " " + WhoAdded.ID);
				}

				if (invitedUser.ChannelId != null)
				{
					Channel channel = db.GetChannel(invitedUser.ChannelId);
					if (channel != null)
					{
						worksheet.Cells[count, 2] = new Cell(channel.ChannelName);
					}
				}
				

				count++;
				
			}

			workbook.Worksheets.Add(worksheet);
			workbook.Save(files);
			SendMessage(botClient, _message, user, "Аналитика по добавлению человек");
		}

		/// <summary>
		/// Аналитика по всем чатам - фразы
		/// </summary>
		/// <param name="analiticsPhrases"></param>
		public static void GetAnaliticsInAllChatPharase(AnaliticsPhrase[] analiticsPhrases, TelegramBotClient botClient, CallbackQuery _message, User user)
		{
			DataBase db = Singleton.GetInstance().Context;
			//создание файла
			String files = AppDomain.CurrentDomain.BaseDirectory + "\\newdoc.xls";
			Workbook workbook = new Workbook();
			Worksheet worksheet = new Worksheet("Аналитика по всем чатам");
			worksheet.Cells[0, 0] = new Cell("Название чата");
			worksheet.Cells[0, 1] = new Cell("Текст");
			worksheet.Cells[0, 2] = new Cell("Количество");

			Int32 count = 1;
			Channel[] channels = db.GetChannels();

			IQueryable<AnaliticsPhrase> temp = analiticsPhrases.OrderByDescending(p => p.Count).AsQueryable();

			foreach (Channel item in channels)
			{
				worksheet.Cells[count, 1] = new Cell(item.ChannelName, "Text");
				count++;
				foreach (AnaliticsPhrase channel in temp)
				{
					if (item.IDChannel == channel.channel.IDChannel)
					{
						worksheet.Cells[count, 0] = new Cell(channel.channel.ChannelName);
						worksheet.Cells[count, 1] = new Cell(channel.AnaliticsPhraseAllChatId);
						worksheet.Cells[count, 2] = new Cell(channel.Count);
						count++;
					}
				}
			}
			workbook.Worksheets.Add(worksheet);
			workbook.Save(files);
			SendMessage(botClient, _message, user, "Аналитика по всем чатам");
		}

		/// <summary>
		/// Аналитика только в одном чате - фразы
		/// </summary>
		/// <param name="analiticsPhrases"></param>
		/// <param name="channel"></param>
		public static void GetAnaliticsInOneChatPharase(AnaliticsPhrase[] analiticsPhrases, Channel channel, TelegramBotClient botClient, Message _message, User user)
		{
			//создание файла
			String file = AppDomain.CurrentDomain.BaseDirectory + "\\newdoc.xls";
			Workbook workbook = new Workbook();
			Worksheet worksheet = new Worksheet("Аналитика по чатам");
			worksheet.Cells[0, 0] = new Cell("Название чата");
			worksheet.Cells[0, 1] = new Cell("Текст");
			worksheet.Cells[0, 2] = new Cell("Количество");

			Int32 count = 1;

			IQueryable<AnaliticsPhrase> temp = analiticsPhrases.OrderByDescending(p => p.Count).AsQueryable();


			foreach (AnaliticsPhrase text in temp)
			{
				if (text.channel.IDChannel == channel.IDChannel)
				{
					worksheet.Cells[count, 0] = new Cell(text.channel.ChannelName);
					worksheet.Cells[count, 1] = new Cell(text.AnaliticsPhraseAllChatId);
					worksheet.Cells[count, 2] = new Cell(text.Count);
					count++;
				}
			}
			workbook.Worksheets.Add(worksheet);
			workbook.Save(file);

			SendMessage(botClient, _message, user, "Аналитика по чату");
		}

		/// <summary>
		/// Аналитика всех фраз
		/// </summary>
		/// <param name="analiticsPhraseAllChats"></param>
		public static void GetAnaliticsPharase(AnaliticsPhraseAllChat[] analiticsPhraseAllChats, TelegramBotClient botClient, CallbackQuery _message, User user)
		{
			//создание файла
			String file = AppDomain.CurrentDomain.BaseDirectory + "\\newdoc.xls";
			Workbook workbook = new Workbook();
			Worksheet worksheet = new Worksheet("Аналитика по фразам");
			worksheet.Cells[0, 0] = new Cell("Текст");
			worksheet.Cells[0, 1] = new Cell("Количество");

			Int32 count = 1;

			IQueryable<AnaliticsPhraseAllChat> temp = analiticsPhraseAllChats.OrderByDescending(p => p.Count).AsQueryable();

			foreach (AnaliticsPhraseAllChat text in temp)
			{
				worksheet.Cells[count, 0] = new Cell(text.NameId);
				worksheet.Cells[count, 1] = new Cell(text.Count);
				count++;
			}
			workbook.Worksheets.Add(worksheet);
			workbook.Save(file);

			SendMessage(botClient, _message, user, "Аналитика по фразам");
		}
		/// <summary>
		/// Аналитика слова по всем чатам.
		/// </summary>
		/// <param name="analiticsTextAllChats"></param>
		public static void GetAnaliticsWord(AnaliticsTextAllChat[] analiticsTextAllChats, TelegramBotClient botClient, CallbackQuery _message, User user)
		{
			//создание файла
			String file = AppDomain.CurrentDomain.BaseDirectory + "\\newdoc.xls";
			Workbook workbook = new Workbook();
			Worksheet worksheet = new Worksheet("Аналитика по словам");
			worksheet.Cells[0, 0] = new Cell("Текст");
			worksheet.Cells[0, 1] = new Cell("Количество");

			Int32 count = 1;

			IQueryable<AnaliticsTextAllChat> temp = analiticsTextAllChats.OrderByDescending(p => p.Count).AsQueryable();

			foreach (AnaliticsTextAllChat text in temp)
			{
				worksheet.Cells[count, 0] = new Cell(text.NameId);
				worksheet.Cells[count, 1] = new Cell(text.Count);
				count++;
			}
			workbook.Worksheets.Add(worksheet);
			workbook.Save(file);

			SendMessage(botClient, _message, user, "Аналитика по словам");
		}

		/// <summary>
		/// Аналитика слова, во всех чатах.
		/// </summary>
		/// <param name="analyticsTexts"></param>
		public static void GetAnaliticsInAllChatWord(AnalyticsText[] analyticsTexts, TelegramBotClient botClient, CallbackQuery _message, User user)
		{
			DataBase db = Singleton.GetInstance().Context;
			//создание файла
			String file = AppDomain.CurrentDomain.BaseDirectory + "\\newdoc.xls";
			Workbook workbook = new Workbook();
			Worksheet worksheet = new Worksheet("Аналитика по словам, во всех чатах");
			worksheet.Cells[0, 0] = new Cell("Название чата");
			worksheet.Cells[0, 1] = new Cell("Текст");
			worksheet.Cells[0, 2] = new Cell("Количество");

			Int32 count = 1;

			Channel[] channel = db.GetChannels();

			IQueryable<AnalyticsText> temp = analyticsTexts.OrderByDescending(p => p.Count).AsQueryable();

			foreach (Channel item in channel)
			{
				worksheet.Cells[count, 1] = new Cell(item.ChannelName, "Text");
				count++;
				foreach (AnalyticsText text in temp)
				{
					if (item.IDChannel == text.channel.IDChannel)
					{
						worksheet.Cells[count, 0] = new Cell(text.channel.ChannelName);
						worksheet.Cells[count, 1] = new Cell(text.AnaliticsTextAllChatId);
						worksheet.Cells[count, 2] = new Cell(text.Count);
						count++;
					}
				}
			}
			workbook.Worksheets.Add(worksheet);
			workbook.Save(file);

			SendMessage(botClient, _message, user, "Аналитика по словам в чатах");
		}

		/// <summary>
		/// Аналитика слова по одному чату.
		/// </summary>
		/// <param name="analyticsTexts"></param>
		/// <param name="channel"></param>
		public static void GetAnaliticsInOneChatWord(AnalyticsText[] analyticsTexts, Channel channel, TelegramBotClient botClient, Message _message, User user)
		{
			//создание файла
			String file = AppDomain.CurrentDomain.BaseDirectory + "\\newdoc.xls";
			Workbook workbook = new Workbook();
			Worksheet worksheet = new Worksheet("Аналитика по словам в одном чате");
			worksheet.Cells[0, 0] = new Cell("Название чата");
			worksheet.Cells[0, 1] = new Cell("Текст");
			worksheet.Cells[0, 2] = new Cell("Количество");

			Int32 count = 1;

			IQueryable<AnalyticsText> temp = analyticsTexts.OrderByDescending(p => p.Count).AsQueryable();

			foreach (AnalyticsText text in temp)
			{
				if (text.channel.IDChannel == channel.IDChannel)
				{
					worksheet.Cells[count, 0] = new Cell(text.channel.ChannelName);
					worksheet.Cells[count, 1] = new Cell(text.AnaliticsTextAllChatId);
					worksheet.Cells[count, 2] = new Cell(text.Count);
					count++;
				}
			}
			workbook.Worksheets.Add(worksheet);
			workbook.Save(file);

			SendMessage(botClient, _message, user, "Аналитика по словам в одном чате");
		}
	}
}
