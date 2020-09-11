using Telegram.Bot;

namespace BotCore.Advertising.Command
{
	internal abstract class AbsCommand
	{
		public abstract System.String Name { set; get; } //имя команды например /start, Привет, Пока

		public abstract void Execute(TelegramBotClient botClient, System.Object message);//выполнение кода

		public System.Boolean Equals(System.String CommandName)
		{
			if (Name == CommandName.Split(" ")[0])
			{
				System.String oldName = Name;
				Name = CommandName;
				return oldName.Equals(CommandName.Split(" ")[0]);
			}
			return Name.Equals(CommandName);
		}
	}
}