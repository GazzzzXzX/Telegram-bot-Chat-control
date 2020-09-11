using System;
using BotCore.Advertising;
using BotCore.Blockchain;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCore
{
	internal class PostCountChannel : AbstractHandlerAdvertising, IStandartCommand
	{
		private User user = null;
		private Channel channel = null;
		private DataBase db = null;
		private Message _message = null;

		public override System.Object Handle(System.Int32 request, TelegramBotClient botClient, System.Object message)
		{
			if (request == (System.Int32)SetChain.PostCountChannel)
			{
				IStandartCommand standartCommand = new PostCountChannel();

				if (standartCommand.SetMessage(message, out _message)) return null;

				if (standartCommand.SetDataBase(out db)) return null;

				if (standartCommand.SetUserAndCheckIsNullMessage(botClient, _message, out user, db)) return null;

				if (SetChannel()) return null;

				ChangeChannel();

				ChangeTMessage();
				
				ChangeUser();

				SendMessage(botClient);

				return null;
			}
			else
			{
				return base.Handle(request, botClient, message);
			}
		}

		public void SendMessage(TelegramBotClient botClient)
		{
			InlineButton inlineButton = new InlineButton();
			botClient.DeleteMessage(_message.From.Id, _message.MessageId, "47 - PostCountChannel");
			botClient.EditMessage(_message.From.Id, user.MessageID, $"Количество постов в день для чата {channel.ChannelName} изменено на {channel.PostCount}", "48 - PostCountChannel", user, replyMarkup: inlineButton.SettingBotLvl2(user));
		}

		private void ChangeUser()
		{
			user.Chain = (Int32)SetChain.MessageUserInBot;
			db.Save();
		}

		private void ChangeChannel()
		{
			System.Int32 temp = 3;
			System.Int32.TryParse(_message.Text, out temp);
			channel.PostCount = temp;
			channel.isPostCount = false;
			db.Save();
		}

		private Boolean SetChannel()
		{
			channel = db.GetChannel();
			return channel == null;
		}

		private void ChangeTMessage()
		{
			var mes = db.GetTMessages();

			foreach (TMessage message in mes)
			{
				if (channel.IDChannel == message.channel.IDChannel)
				{
					message.Post = channel.PostCount;
				}
			}
			db.Save();
		}
	}
}
