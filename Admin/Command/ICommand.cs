using Telegram.Bot;

namespace BotCore
{
	internal abstract class ICommand
	{
		public abstract System.String Name { get; } //имя команды например /start, Привет, Пока

		public abstract void Execute(TelegramBotClient botClient, System.Object message);//выполнение кода

		public System.Boolean Equals(System.String CommandName) => Name.Equals(CommandName);
	}
}