namespace Calendar
{
	internal interface ICalendar
	{
		System.String Name { get; } //имя команды например /start, Привет, Пока

		//  void Execute(TelegramBotClient botClient, System.Object message);//выполнение кода

		System.Boolean Equals(System.String CommandName);
	}
}