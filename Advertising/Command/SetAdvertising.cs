using System.Threading.Tasks;
using BotCore.SQL;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace BotCore.Advertising.Command
{
	internal class SetAdvertising : AbsCommand
	{
		public override System.String Name { set; get; } = CommandsText.SetAdvertising;

		private async void DeleteMessage(TelegramBotClient botClient, CallbackQuery _message)
		{
			for (System.Int32 i = 1; i <= 10; i++)
			{
				try
				{
					await botClient.DeleteMessageAsync(_message.From.Id, _message.Message.MessageId - i);
				}
				catch { }
			}
		}

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase      db       = Singleton.GetInstance().Context;
			await Task.Run(() => DeleteMessage(botClient, _message));
			try
			{
				if (IsBan.Ban(botClient, message))
				{
					User user = db.GetUser(_message.From.Id);
					if (IsNullDataBase.IsNull(botClient, _message, user)) return;
					AdUser adUser = db.GetAdUser(user.ID);
					adUser.EditingPostTemplateId = -1;
					db.Save();
					InlineKeyboardMarkup answer = InlineButton.GetAdvertising(user);
					await botClient.EditMessageTextAsync(_message.From.Id, _message.Message.MessageId,
					                                     "📞Ваша реклама📞", Telegram.Bot.Types.Enums.ParseMode.Html,
					                                     replyMarkup: answer);
				}
			}
			catch (System.Exception ex)
			{
				Log.Logging(ex);
			}
		}
	}
}