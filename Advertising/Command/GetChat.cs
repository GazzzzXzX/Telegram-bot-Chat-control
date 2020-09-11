using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace BotCore.Advertising.Command
{
	internal class GetChat : AbsCommand
	{
		public override System.String Name { set; get; } = CommandsText.GetChat;

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			try
			{
				if (IsBan.Ban(botClient, message))
				{
					InlineKeyboardMarkup answer = InlineButton.GetChat();
					await botClient.EditMessageTextAsync(_message.From.Id, _message.Message.MessageId, "Чаты: ",
					                                     Telegram.Bot.Types.Enums.ParseMode.Html, replyMarkup: answer);
				}
			}
			catch (System.Exception ex)
			{
				Log.Logging(ex);
			}
		}
	}
}