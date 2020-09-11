using System;
using System.IO;
using System.Text;

namespace BotCore
{
	internal class Log
	{
		private static System.Object Locker = new System.Object();

		internal static void Logging(Exception Ex)
		{
			lock (Locker)
			{
				using (StreamWriter wr = new StreamWriter("Log.log", true, Encoding.Default))
				{
					wr.WriteLine($"{DateTime.Now.ToShortDateString()} ---- {DateTime.Now.ToShortTimeString()}  - - -  {Ex.Message}");
					wr.Close();
				}
			}
		}

		public static void Logging(String name)
		{
			lock (Locker)
			{
				using (StreamWriter wr = new StreamWriter("Log.log", true, Encoding.Default))
				{
					wr.WriteLine($"{DateTime.Now.ToShortDateString()} ---- {DateTime.Now.ToShortTimeString()}  - - -  {name}");
                	wr.Close();
                }
			}
		}
	}
}