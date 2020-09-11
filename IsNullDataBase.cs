using System.Threading.Tasks;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCore
{
	internal static class IsNullDataBase
	{
		private static void DeleteMessage(TelegramBotClient botClient, CallbackQuery _message)
		{
			for (System.Int32 i = 0; i <= 100; i++)
			{
				botClient.DeleteMessage(_message.From.Id, _message.Message.MessageId - i, "");
			}
		}
		
		private static void DeleteMessageUser(TelegramBotClient botClient, CallbackQuery _message)
		{
			DataBase db = Singleton.GetInstance().Context;
			User user = db.GetUser(_message.From.Id);
			for (System.Int32 i = 0; i <= 100; i++)
			{
				botClient.DeleteMessage(_message.From.Id, user.MessageID - i, "");
			}
		}

		private static async void DeleteMessage(TelegramBotClient botClient, Message _message)
		{
			for (System.Int32 i = 0; i <= 100; i++)
			{
				try
				{
					await botClient.DeleteMessageAsync(_message.From.Id, _message.MessageId - i);
				}
				catch { }
			}
		}
		
		private static async void DeleteMessageUser(TelegramBotClient botClient, Message _message)
		{
			DataBase db   = Singleton.GetInstance().Context;
			User     user = db.GetUser(_message.From.Id);
			for (System.Int32 i = 0; i <= 100; i++)
			{
				try
				{
					await botClient.DeleteMessageAsync(_message.From.Id, user.MessageID - i);
				}
				catch { }
			}
		}

		private static void DeleteMessagePlus(TelegramBotClient botClient, CallbackQuery _message)
		{
			for (System.Int32 i = 0; i <= 100; i++)
			{
				botClient.DeleteMessage(_message.From.Id, _message.Message.MessageId + i, "");
			}
		}
		
		private static void DeleteMessagePlusUser(TelegramBotClient botClient, CallbackQuery _message)
		{
			DataBase db   = Singleton.GetInstance().Context;
			User     user = db.GetUser(_message.From.Id);
			for (System.Int32 i = 0; i <= 100; i++)
			{
				botClient.DeleteMessage(_message.From.Id, user.MessageID + i, "");
			}
		}

		private static async void DeleteMessagePlus(TelegramBotClient botClient, Message _message)
		{
			for (System.Int32 i = 0; i <= 100; i++)
			{
				try
				{
					await botClient.DeleteMessageAsync(_message.From.Id, _message.MessageId + i);
				}
				catch { }
			}
		}
		
		private static async void DeleteMessagePlusUser(TelegramBotClient botClient, Message _message)
		{
			DataBase db   = Singleton.GetInstance().Context;
			User     user = db.GetUser(_message.From.Id);
			for (System.Int32 i = 0; i <= 100; i++)
			{
				try
				{
					await botClient.DeleteMessageAsync(_message.From.Id, user.MessageID + i);
				}
				catch { }
			}
		}

		public static System.Boolean IsNull<T>(TelegramBotClient botClient, System.Object message, T value) where T : class
		{
			CallbackQuery _message = message as CallbackQuery;

			if (value == null)
			{
				if (_message != null)
				{
					Task.Run(() => DeleteMessage(botClient, _message));

					Task.Run(() => DeleteMessagePlus(botClient, _message));
				}
				else
				{
					Message _Message = message as Message;

					Task.Run(() => DeleteMessage(botClient, _Message));

					//Task.Run(() => DeleteMessagePlus(botClient, _Message));
				}
				return true;
			}
			return false;
		}

		public static void DeleteAllMessage(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;

			if (_message != null)
			{
				Task.Run(() => DeleteMessage(botClient, _message));
				//Task.Run(() => DeleteMessagePlus(botClient, _message));
			}
			else
			{
				Message _Message = message as Message;

				Task.Run(() => DeleteMessage(botClient, _Message));
				//Task.Run(() => DeleteMessagePlus(botClient, _Message));
			}
		}

		public static void DeleteMessageUser(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;

			if (_message != null)
			{
				Task.Run(() => DeleteMessageUser(botClient, _message));
				//Task.Run(() => DeleteMessagePlusUser(botClient, _message));
			}
			else
			{
				Message _Message = message as Message;

				Task.Run(() => DeleteMessageUser(botClient, _Message));
				//Task.Run(() => DeleteMessagePlus(botClient, _Message));
			}
		}
		
	}
}