using Telegram.Bot.Types.ReplyMarkups;

namespace BotCore
{
	internal class ReplyButton
	{
		public ReplyKeyboardMarkup Register()
		{
			ReplyKeyboardMarkup rep = new ReplyKeyboardMarkup(new[]
			{
				new[]
				{
					new KeyboardButton("❗Регистрация❗") { Text = "❗Регистрация❗"}
				}
			})
			{
				ResizeKeyboard = true,
				OneTimeKeyboard = true
			};

			return rep;
		}

		public ReplyKeyboardMarkup Menu()
		{
			ReplyKeyboardMarkup rep = new ReplyKeyboardMarkup(new[]
			{
				new[]
				{
					new KeyboardButton("🚪Личный кабинет🚪"),
				},
				new[]
				{
					new KeyboardButton("⭐Связь с администратором⭐️"),
				}
			})
			{
				ResizeKeyboard = true
			};
			return rep;
		}

		public ReplyKeyboardMarkup BanMenu()
		{
			ReplyKeyboardMarkup rep = new ReplyKeyboardMarkup(new[]
			{
				new[]
				{
					new KeyboardButton("🚪Личный кабинет🚪"),
				}
			})
			{
				ResizeKeyboard = true
			};
			return rep;
		}

		public ReplyKeyboardMarkup SetNumber()
		{
			ReplyKeyboardMarkup rep = new ReplyKeyboardMarkup(new[]
			{
				new[]
				{
					new KeyboardButton("Поделиться номером телефона") { RequestContact = true},
				}
			})
			{
				ResizeKeyboard = true
			};
			return rep;
		}
	}
}