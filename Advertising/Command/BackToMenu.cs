using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCore.Advertising.Command
{
	internal class BackToMenu : AbsCommand
	{
		public override System.String Name { set; get; } = CommandsText.BackToMenu;

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery        _message     = message as CallbackQuery;
			DataBase             db           = Singleton.GetInstance().Context;
			BotCore.InlineButton inlineButton = new BotCore.InlineButton();
			try
			{
				if (IsBan.Ban(botClient, message))
				{
					User user = db.GetUser(_message.From.Id);
					if (IsNullDataBase.IsNull(botClient, _message, user)) return;
					System.String temp = InfoUser.Info(user);
					if (user.IsAdmin == 0)
					{
						await botClient.EditMessageTextAsync(_message.From.Id, _message.Message.MessageId, temp,
						                                     Telegram.Bot.Types.Enums.ParseMode.Html,
						                                     replyMarkup: inlineButton.Accaunt);
					}
					else
					{
						await botClient.EditMessageTextAsync(_message.From.Id, _message.Message.MessageId, temp,
						                                     Telegram.Bot.Types.Enums.ParseMode.Html,
						                                     replyMarkup: inlineButton.AdminAccaunt);
					}
				}
			}
			catch (System.Exception ex)
			{
				Log.Logging(ex);
			}
		}
	}
}