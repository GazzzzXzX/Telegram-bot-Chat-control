using System.Collections.Generic;
using Telegram.Bot.Types.ReplyMarkups;

namespace BotCore.PhotoChannel
{
	internal static class InlineButtonPhotoChannel
	{
		public static InlineKeyboardMarkup AddPhotoAdmin(Channel channel)
		{
			List<List<InlineKeyboardButton>> list = new List<List<InlineKeyboardButton>>
			{
				new List<InlineKeyboardButton>()
			};

			list[list.Count - 1].Add(new InlineKeyboardButton() { Text = "✅Добавить изображение✅", CallbackData = PhotoCommandName.AddPhotoInDataBase + " " + channel.IDMessage });
			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton() { Text = "❌Отмена❌", CallbackData = PhotoCommandName.CancelPhoto + " " + channel.IDMessage });
			return new InlineKeyboardMarkup(list);
		}

		public static InlineKeyboardMarkup ShowAnalitics = new InlineKeyboardButton[][]
		{
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton{Text = "Аналитика по тексту", CallbackData = CommandText.AnaliticsShowUser }
			},
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton{ Text = "Доходность", CallbackData = CommandText.ThisIncome }
			},
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton{Text = "Аналитика по пользователям", CallbackData = CommandText.AddUsersAnalitics}
			}, 
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton { Text = "Количество добавленых людей", CallbackData = CommandText.AddUserInChannel }
			},
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton{ Text = "Аналитика по всем чатам(слова)", CallbackData = PhotoCommandName.GetAnaliticsInAllChatWord }
			},
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton{ Text = "Аналитика по чату(слова)",  CallbackData = PhotoCommandName.GetAnaliticsInOneChatWord }
			},
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton{ Text = "Аналитика по словам",  CallbackData = PhotoCommandName.GetAnaliticsWord }
			},
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton{ Text = "Аналитинка по всем чатам(фраза)",  CallbackData = PhotoCommandName.GetAnaliticsInAllChatPharase }
			},
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton{ Text = "Аналитика по чату(фраза)",  CallbackData = PhotoCommandName.GetAnaliticsInOneChatPharase }
			},
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton { Text = "Аналитика по фразам",  CallbackData = PhotoCommandName.GetAnaliticsPharase }
			},
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton{Text = "🔙Назад🔙", CallbackData = CommandText.BackToAccauntMenu}
			}
		};

		public static InlineKeyboardMarkup ThisIncome = new InlineKeyboardButton[][]
		{
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton{ Text = "Доходность групп", CallbackData =  CommandText.ThisIncomeChannel }
			},
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton { Text = "Доходность от пользователей", CallbackData = CommandText.ThisIncomeUser }
			},
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton { Text = "Зарплата администрации", CallbackData = CommandText.ThisIncomeAdmin }
			},
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton{Text = "🔙Назад🔙", CallbackData = CommandText.BackToAccauntMenu}
			}
		};
	}
}
