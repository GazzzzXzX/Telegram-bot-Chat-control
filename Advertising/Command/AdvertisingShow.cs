using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCore.Advertising.Command
{
	internal class AdvertisingShow : AbsCommand
	{
		public override System.String Name { set; get; } = CommandsText.Advertising;

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase      db       = Singleton.GetInstance().Context;

			User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return;
			if (IsBan.Ban(botClient, message))
			{
				if (user.IsAdmin >= 2)
				{
					try
					{
						await botClient.EditMessageTextAsync(_message.From.Id, _message.Message.MessageId, "Реклама", replyMarkup: InlineButton.AdvertisingShowAdmin);
					}
					catch (System.Exception ex)
					{
						Log.Logging(ex);
					}
				}
				else
				{
					try
					{
						await botClient.EditMessageTextAsync(_message.From.Id, _message.Message.MessageId, "Реклама", replyMarkup: InlineButton.AdvertisingShow);
					}
					catch (System.Exception ex)
					{
						Log.Logging(ex);
					}
				}
			}
		}
	}
}