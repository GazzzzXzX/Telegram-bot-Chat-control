using System;
using BotCore.Blockchain;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCore
{
	internal class Info : AbsCommand, IStandartCommand
	{
		public override System.String Name { set; get; } = "/info";

		private Message _message = null;

		public override void Execute(TelegramBotClient botClient, System.Object message)
		{
			IStandartCommand standartCommand = new Info();

			if (standartCommand.SetMessage(message, out _message)) return;

			SendMessage(botClient);
		}

		public void SendMessage(TelegramBotClient botClient)
		{
			botClient.DeleteMessage(_message.From.Id, _message.MessageId, "25 - Info");
			DataBase db = Singleton.GetInstance().Context;
			Settings settings = db.GetSettings();
			InlineButton inlineButton = new InlineButton();
			String Text = $"Здравсвуйте {_message.From.FirstName}, мы подготовили для вас более детальную информацию \"О нас\"\n" + "\n\n/start - запуск бота!\n" + settings.Regulations;
			botClient.SendText(_message.From.Id, Text, replyMarkup: inlineButton.Closeinfo);
		}
	}

	internal class CloseInfo : AbsCommand, IStandartCommand
	{
		public override System.String Name { set; get; } = "/closeInfo";

		private CallbackQuery _message = null;

		public override void Execute(TelegramBotClient botClient, System.Object message)
		{
			IStandartCommand standartCommand = new CloseInfo();

			if (standartCommand.SetCallbackQuery(message, out _message)) return;

			SendMessage(botClient);
		}

		public void SendMessage(TelegramBotClient botClient) => botClient.DeleteMessage(_message.From.Id, _message.Message.MessageId, "25 - Info");
	}
}
