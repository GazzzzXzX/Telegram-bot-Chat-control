using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using BotCore.Blockchain;
using Nethereum.ABI.Util;
using RestSharp.Extensions;

namespace BotCore
{
	internal class Program
	{
		private static void Main(System.String[] args)
		{
			try
			{
				UpdateSystem updateSystem = new UpdateSystem();
				updateSystem.Start();

				//var a = Convert.ToString(Console.ReadLine());
				//session.CreateSession(a).Wait();

				// 1046277477:AAHxIx5MkmYSEH9qIiGYlLMTEUcz5Nage6E

				//676537196:AAG4Y1D9JwQAtwXpPJYqPKAnPqA8CBkb2qU
				//965810994:AAE2fkzML2sZFS0BjqlaeQv97Elhi8y823U
				
				//741970595:AAHSTsVYg_tGhelF_2M7BDKvxf2v9vfTq_E
				RippleClientUBC.GetClient().client = new RippleDotNet.RippleClient("wss://s1.ripple.com:443");
				Bot bot = new Bot("1046277477:AAHxIx5MkmYSEH9qIiGYlLMTEUcz5Nage6E");
				bot._exitEvent.WaitOne();
			}
			catch (Exception e)
			{
				using (FileStream fs = File.Open("log", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
				{
					fs.Write(Encoding.UTF8.GetBytes(e.Message));
				}
			}
		}
	}
}