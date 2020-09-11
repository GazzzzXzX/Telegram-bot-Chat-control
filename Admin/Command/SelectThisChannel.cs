using System;
using BotCore.Advertising;
using BotCore.Blockchain;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCore
{
	internal class SelectThisChannel : AbsCommand, IStandartCommand, ISplitName
	{
		public override System.String Name { set; get; } = CommandText.SelectThisChannel;

		private DataBase db = null;
		private User user = null;
		private Channel channel = null;
		private CallbackQuery _message = null;

		public override void Execute(TelegramBotClient botClient, System.Object message)
		{
			IStandartCommand standartCommand = new SelectThisChannel();
			ISplitName splitName = new SelectThisChannel();

			if (standartCommand.SetCallbackQuery(message, out _message)) return;

			if (standartCommand.SetDataBase(out db)) return;

			if (standartCommand.SetUserAndCheckIsNull(botClient, _message, out user, db)) return;

			Int64 idUser = splitName.GetNameSplit64(Name);
			Name = CommandText.SelectThisChannel;

			if (SelectedUser(idUser)) return;

			ChangeChannel();

			SendMessage(botClient);
		}

		private Boolean SelectedUser(Int64 idUser)
		{
			channel = db.GetChannel(idUser);
			return channel == null;
		}

		private void ChangeChannel()
		{
			user.Chain = (Int32)SetChain.PostCountChannel;
			channel.isPostCount = true;
			db.Save();
		}

		public void SendMessage(TelegramBotClient botClient)
		{
			InlineButton inlineButton = new InlineButton();
			Settings settings = db.GetSettings();
			botClient.EditMessage(_message.From.Id, user.MessageID, "Количество постов в день: " + channel.PostCount + "\nВведите общее значение для этого чата или выберете без ограничения: ", "36 - LimitedUser", replyMarkup: inlineButton.LimitedChannel(channel));
		}
	}
}
