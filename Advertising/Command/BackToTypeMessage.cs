using System;
using BotCore.Blockchain;
using BotCore.SQL;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCore.Advertising.Command
{
	internal class BackToTypeMessage : AbsCommand, ISplitName, IStandartCommand
	{
		public override System.String Name { set; get; } = CommandsText.BackToTypeMessage;

		private DataBase db = null;
		private User user = null;
		private CallbackQuery _message = null;
		private PostTemplate postTemplate = null;

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			IStandartCommand standartCommand = new CreateAdKeyboardTypeMessage();
			ISplitName splitName = new CreateAdKeyboardTypeMessage();

			if (standartCommand.SetCallbackQuery(message, out _message)) return;

			if (standartCommand.SetDataBase(out db)) return;

			if (standartCommand.SetUserAndCheckIsNull(botClient, _message, out user, db)) return;

			Int32 idPostTemplate = splitName.GetNameSplit(Name);
			Name = CommandsText.BackToTypeMessage;
			if (IsBan.Ban(botClient, message))
			{
				if (GetPostTemplate(idPostTemplate)) return;

				try
				{
					AdUser adUser = db.GetAdUser(user.ID);
					PostTemplate postTemplate = db.GetTempalte(adUser.User.ID, adUser.EditingPostTemplateId);

					SendMessage(botClient);
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}

		private Boolean GetPostTemplate(Int32 idPostTemplate)
		{
			postTemplate = db.GetTempalte(user.ID, idPostTemplate);
			return postTemplate == null;
		}


		public void SendMessage(TelegramBotClient botClient) => botClient.EditMessage(user.ID, user.MessageID, "🧾Тип сообщения🧾", "58 - BackToTypeMessage", user, InlineButton.CreateAdKeyboardTypeMessage(postTemplate));
	}
}
