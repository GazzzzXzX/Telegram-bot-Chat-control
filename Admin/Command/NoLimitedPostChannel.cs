using System;
using BotCore.Blockchain;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCore
{
	internal class NoLimitedPostChannel : AbsCommand, IStandartCommand, ISplitName
	{
		public override System.String Name { set; get; } = CommandText.NoLimitedPostChannel;

		private DataBase db = null;
		private User user = null;
		private Channel channel = null;
		private CallbackQuery _message = null;

		public override void Execute(TelegramBotClient botClient, System.Object message)
		{
			IStandartCommand standartCommand = new NoLimitedPostChannel();
			ISplitName splitName = new NoLimitedPostChannel();

			if (standartCommand.SetCallbackQuery(message, out _message)) return;

			if (standartCommand.SetDataBase(out db)) return;

			if (standartCommand.SetUserAndCheckIsNull(botClient, _message, out user, db)) return;

			Int64 idUser = splitName.GetNameSplit64(Name);
			Name = CommandText.NoLimitedPostChannel;

			if (SelectedUser(idUser)) return;

			SetLimited();

			SendMessage(botClient);
		}

		private Boolean SelectedUser(Int64 idUser)
		{
			channel = db.GetChannel(idUser);
			return channel == null;
		}

		private void SetLimited()
		{
			TMessage[] message = db.GetTMessages();
			channel.PostCount = 999999;

			foreach (TMessage item in message)
			{
				if (item.channel.IDChannel == channel.IDChannel)
				{
					item.Post = channel.PostCount;
				}
			}
			db.Save();
		}

		public void SendMessage(TelegramBotClient botClient)
		{
			InlineButton inlineButton = new InlineButton();
			botClient.EditMessage(_message.From.Id, user.MessageID, $"На группу {channel.ChannelName} выставлено \"Нет ограничения\"", "36 - LimitedUser", replyMarkup: inlineButton.SettingBotLvl2(user));
		}
	}
}
