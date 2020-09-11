using Telegram.Bot;

namespace BotCore
{
	internal interface ICommandSlash
	{
		System.String Name { get; } //имя команды например /start, Привет, Пока

		void Execute(TelegramBotClient botClient, System.Object message);//выполнение кода

		System.Boolean Equals(System.String CommandName);
	}
}