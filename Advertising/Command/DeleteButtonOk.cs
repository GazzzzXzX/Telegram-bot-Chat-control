using BotCore.Advertising;
using BotCore.Blockchain;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCore
{
	internal class DeleteButtonOk : Advertising.Command.AbsCommand, IStandartCommand, ISplitName
	{
		public override System.String Name { set; get; } = CommandsText.ButtonOk;

		private DataBase db = null;
		private User user = null;
		private CallbackQuery _message = null;

		public override void Execute(TelegramBotClient botClient, System.Object message)
		{
			IStandartCommand standartCommand = new DeleteButtonOk();
			ISplitName splitName = new DeleteButtonOk();

			if (standartCommand.SetCallbackQuery(message, out _message)) return;

			if (standartCommand.SetDataBase(out db)) return;

			if (standartCommand.SetUserAndCheckIsNull(botClient, _message, out user, db)) return;

			SendMessage(botClient);
		}

		public void SendMessage(TelegramBotClient botClient) => botClient.DeleteMessage(_message.From.Id, _message.Message.MessageId, "40 - DeleteButtonOk");
	}
}
