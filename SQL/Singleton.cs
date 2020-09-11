using Microsoft.EntityFrameworkCore;

namespace BotCore
{
	internal class Singleton
	{
		private Singleton()
		{
		}

		private static Singleton _instance;

		public static System.Object BDLock = new System.Object();

		private static readonly System.Object _lock = new System.Object ();

		public static Singleton GetInstance()
		{
			if (_instance != null)
			{
				return _instance;
			}
			lock (_lock)
			{
				_instance = new Singleton
				{
					Context = new DataBase()
				};
				_instance.Context.Database.Migrate();
			}
			return _instance;
		}

		public DataBase Context { get; set; }
	}
}