using System;
using System.Net;
using Newtonsoft.Json;

namespace BotCore.Advertising
{
	internal class CryptoCourse
	{
		public static ExchangeInfo GetExchangeInfo(String from, String to)
		{
			using (WebClient client = new WebClient())
			{
				String json = client.DownloadString(String.Format(@"https://spectrocoin.com/scapi/ticker/{0}/{1}/", from, to));
				return JsonConvert.DeserializeObject<ExchangeInfo>(json);
			}
		}
		public class ExchangeInfo
		{
			[JsonProperty("currencyFrom")]
			public String CurrencyFrom { get; set; }
			[JsonProperty("currencyFromScale")]
			public Int32 CurrencyFromScale { get; set; }
			[JsonProperty("currencyTo")]
			public String CurrencyTo { get; set; }
			[JsonProperty("currencyToScale")]
			public Int32 CurrencyToScale { get; set; }
			[JsonProperty("last")]
			public Double Last { get; set; }
			[JsonProperty("timestamp")]
			public Int64 Timestamp { get; set; }
			[JsonProperty("friendlyLast")]
			public String FriendlyLast { get; set; }
		}
	}
}
