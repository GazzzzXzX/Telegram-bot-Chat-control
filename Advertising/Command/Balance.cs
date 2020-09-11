using BotCore.SQL;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCore.Advertising.Command
{
	internal class Balance : AbsCommand
	{
		public override System.String Name { set; get; } = CommandsText.Balance;

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase      db       = Singleton.GetInstance().Context;

			try
			{
				if (IsBan.Ban(botClient, message))
				{
					AdUser adUser = db.GetAdUser(_message.From.Id);
					if (IsNullDataBase.IsNull(botClient, _message, adUser)) return;
					await botClient.EditMessageTextAsync(_message.From.Id, _message.Message.MessageId, "Ваш баланс: " + adUser.Balance + " грн", replyMarkup: InlineButton.Balance);
				}
			}
			catch (System.Exception ex)
			{
				Log.Logging(ex);
			}
		}
	}
}