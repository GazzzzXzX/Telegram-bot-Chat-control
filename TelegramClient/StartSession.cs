using System;
using System.Collections.Generic;
using BotCore.Advertising;
using BotCore.TelegramClient;
using Telegram.Bot;
using Telegram.Bot.Types;


namespace BotCore
{
	internal static class StartSession
	{
		private static SessionTelegram session = new SessionTelegram();

		/// <summary>
		/// Метод отвечающий за чекер людей в группе.
		/// Возвращаемое значение -1 означает ошибку.
		/// </summary>
		/// <param name="ChannelLink">Ссылка на группу передавать без "@"</param>
		/// <returns></returns>
		public static Int32 NumberOfParticipants(String ChannelLink)
		{
			Console.WriteLine(session.Start(, "").Result);

			return WorkingTelegramClient.GetCountParticipantsChannelandSuperGroup(ChannelLink, session.client).Result;
		}

		public static void AddNumber(String number)
		{
			Console.WriteLine(session.Start(, "").Result);
			//вводишь api_id и api_hash (Лучше используй мои) , обязательный метод тут мы подключаемся к серверу 
			//так же Start можно вызвать для изменения данных текущей сесиии

			session.SendRequstOnServer(number).Wait();
		}

		public static Boolean SetCode(TelegramBotClient botClient, Message _message, String code, User user, DataBase db)
		{
			try
			{
				session.CreateSession(code).Wait();
				return false;
			}
			catch
			{
				InlineButton inlineButton = new InlineButton();
				user.Chain = (Int32)SetChain.AddAccauntUser;
				db.Save();
				botClient.DeleteMessage(_message.From.Id, _message.MessageId, "50 - StartSession");
				botClient.EditMessage(_message.From.Id, user.MessageID, "Этот номер уже используется, введите пожалуйста другой!", "45 - StartSession", replyMarkup: inlineButton.BackToSetting);
				return true;
			}
		}

		public static List<ChannelInfo> Test(List<Channel> channels, Int32 id_user)
		{
			var client = new TLSharp.Core.TelegramClient(833968, "a6c65fd9460c44e42c5268b5601c849f");
			var isAuth = client.IsUserAuthorized();
			Console.WriteLine($"Файл авторизации существует: {isAuth}");
			client.ConnectAsync().Wait();

			var res_2 = StartInfoChannel.GetInfoGroup(channels, client).Result;

			List<ChannelInfo> ListFoundChannels = StartInfoChannel.GetChannelByUser(res_2, id_user);

			foreach (var item in ListFoundChannels)
			{
				foreach (var item2 in item.Users)
				{
					
					Console.WriteLine("User: " + item2.FirstName);
				}
				foreach (var item2 in item.Admins)
				{
					Console.WriteLine("Admin: " + item2.FirstName);
				}
			}
			return ListFoundChannels;
		}
	}
}
