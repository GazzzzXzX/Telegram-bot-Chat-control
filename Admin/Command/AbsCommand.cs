using Telegram.Bot;

namespace BotCore
{
	internal abstract class AbsCommand
	{
		public abstract System.String Name { get; set; } //имя команды например /start, Привет, Пока

		public abstract void Execute(TelegramBotClient botClient, System.Object message);//выполнение кода

		public System.Boolean Equals(System.String CommandName)
		{
			if (CommandName != null)
			{
				if (Name == CommandName.Split(" ")[0])
				{
					System.String oldName = Name;
					Name = CommandName;
					return oldName.Equals(CommandName.Split(" ")[0]);
				}
			}
			return Name.Equals(CommandName);
		}
	}
}