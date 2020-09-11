using System;
using BotCore.Advertising;
using BotCore.Blockchain;

using Telegram.Bot;
using Telegram.Bot.Types;


namespace BotCore
{
	internal class AnaliticsShowUser : Advertising.Command.AbsCommand, IStandartCommand
	{
		public override System.String Name { set; get; } = CommandText.AnaliticsShowUser;

		private DataBase db = null;
		private User user = null;
		private CallbackQuery _message = null;

		public override void Execute(TelegramBotClient botClient, System.Object message)
		{
			IStandartCommand standartCommand = new AnaliticsShowUser();

			if (standartCommand.SetCallbackQuery(message, out _message)) return;

			if (standartCommand.SetDataBase(out db)) return;

			if (standartCommand.SetUserAndCheckIsNull(botClient, _message, out user, db)) return;
			if (IsBan.Ban(botClient, message))
			{
				ChangeUser();

				SendMessage(botClient);
			}
		}

		private void ChangeUser()
		{
			user.Chain = (Int32)SetChain.PhraseUser;
			db.Save();
		}

		public void SendMessage(TelegramBotClient botClient)
		{
			InlineButton inlineButton = new InlineButton();

			String text = "💹Аналитика💹";
			text += "\nВаша фраза будет проанализирована нашим алгоритмом, после чего мы выдадим вам группы, где эта фраза встречается больше всего. С помощью данной функции вы сможете более детально настраивать рекламу.";
			text += "\n\nВведите фразу:";

			botClient.EditMessage(user.ID, user.MessageID, text, "38 - AnaliticsShow", user, inlineButton.BackToAccauntMenu);
		}
	}
}
