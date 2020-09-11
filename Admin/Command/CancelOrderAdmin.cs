using System;
using System.Linq;
using BotCore.Advertising;
using BotCore.Blockchain;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCore
{
	internal class CancelOrderAdmin: Advertising.Command.AbsCommand, IStandartCommand, ISplitName
	{
		public override System.String Name { set; get; } = CommandText.CancelOrderAdmin;

		private DataBase db = null;
		private User user = null;
		private User userTwo = null;
		private CallbackQuery _message = null;

		public override void Execute(TelegramBotClient botClient, System.Object message)
		{
			IStandartCommand standartCommand = new AcceptOrderAdmin();
			ISplitName       splitName       = new AcceptOrderAdmin();

			if (standartCommand.SetCallbackQuery(message, out _message)) return;

			if (standartCommand.SetDataBase(out db)) return;

			if (standartCommand.SetUserAndCheckIsNull(botClient, _message, out user, db)) return;

			Int32 idUser = splitName.GetNameSplit(Name);
			Name = CommandText.CancelOrderAdmin;

			if (user.IsAdmin == 3)
			{
				SetUser(idUser);
				if (userTwo == null) return;
				
				ChangeAdmin();
				
				SendMessage(botClient);
			}

		}

		private void ChangeAdmin()
		{
			userTwo.IsAdmin = 0;
			db.Save();
		}

		private void SetUser(Int32 id)
		{
			userTwo = db.GetUser(id);
		}

		public void SendMessage(TelegramBotClient botClient)
		{
			botClient.EditMessage(user.ID, _message.Message.MessageId, $"Заявка по назначению администратора {userTwo.FIO} была отклонена!", "57 - CancelOrderAdmin");
		}
	}
}