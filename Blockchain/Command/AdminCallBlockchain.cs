using BotCore.Advertising;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCore.Blockchain
{
	internal class AdminCallBlockchain : Advertising.Command.AbsCommand, IStandartCommand
	{
		public override System.String Name { set; get; } = CommandTextBlockchain.AdminCallBlockchain;

		private DataBase db = null;
		private User user = null;
		private CallbackQuery _message = null;

		public override void Execute(TelegramBotClient botClient, System.Object message)
		{
			IStandartCommand standartCommand = new AdminCallBlockchain();

			if (standartCommand.SetCallbackQuery(message, out _message)) return;

			if (standartCommand.SetDataBase(out db)) return;

			if (standartCommand.SetUserAndCheckIsNull(botClient, _message, out user, db)) return;

			//ChangeUser();

			SendMessage(botClient);
		}

		private void ChangeUser()
		{
			user.Chain = (System.Int32)SetChain.AdminCallBlockchain;
			db.Save();
		}

		public void SendMessage(TelegramBotClient botClient)
		{
			if (user.Username != "Нет!")
			{
				botClient.EditMessage(_message.From.Id, user.MessageID, "Вы можете связаться с администратором через данного бота @UBCSupport_Bot", "20 - BackToGuarantorMeinMenu", user, replyMarkup: InlineButtonBlockchain.BackToGuarantorMeinMenu);
			}
			else
			{
				botClient.SendText(_message.From.Id, "Мы не можем отправить заявку администрации, у вас не установлен \"имя пользователя\" на аккаунте.\nЕго можно установить в настройках -> \"Имя пользователя\"", user, replyMarkup: InlineButtonBlockchain.BackToGuarantorMeinMenu);
			}
		}
	}
}
