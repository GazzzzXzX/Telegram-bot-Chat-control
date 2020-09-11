using Telegram.Bot;

namespace BotCore.Advertising
{
	public interface IHandleAdvertising
	{
		IHandleAdvertising SetNext(IHandleAdvertising handler);

		System.Object Handle(System.Int32 request, TelegramBotClient botClient, System.Object message);
	}

	public abstract class AbstractHandlerAdvertising : IHandleAdvertising
	{
		private IHandleAdvertising _nextHandler;

		public IHandleAdvertising SetNext(IHandleAdvertising handler)
		{
			_nextHandler = handler;

			// Возврат обработчика отсюда позволит связать обработчики простым
			// способом, вот так:
			// monkey.SetNext(squirrel).SetNext(dog);
			return handler;
		}

		public virtual System.Object Handle(System.Int32 request, TelegramBotClient botClient, System.Object message)
		{
			if (_nextHandler != null)
			{
				return _nextHandler.Handle(request, botClient, message);
			}
			else
			{
				return null;
			}
		}
	}

	internal interface IHandleAdvertisingAnaliz
	{
		IHandleAdvertisingAnaliz SetNext(IHandleAdvertisingAnaliz handler);

		System.Object Handle(TelegramBotClient botClient, System.Object message, User user, Settings settings, TMessage tmessage, Channel channel);
	}

	internal abstract class AbstractHandlerAdvertisingAnaliz : IHandleAdvertisingAnaliz
	{
		private IHandleAdvertisingAnaliz _nextHandler;

		public virtual System.Object Handle(TelegramBotClient botClient, System.Object message, User user, Settings settings, TMessage tmessage, Channel channel)
		{
			if (_nextHandler != null)
			{
				return _nextHandler.Handle(botClient, message, user, settings, tmessage, channel);
			}
			else
			{
				return null;
			}
		}

		public IHandleAdvertisingAnaliz SetNext(IHandleAdvertisingAnaliz handler)
		{
			_nextHandler = handler;

			// Возврат обработчика отсюда позволит связать обработчики простым
			// способом, вот так:
			// monkey.SetNext(squirrel).SetNext(dog);
			return handler;
		}
	}
}