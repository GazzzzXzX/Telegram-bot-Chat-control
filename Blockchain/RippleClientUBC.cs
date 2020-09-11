using RippleDotNet;

namespace BotCore.Blockchain
{
	//private static string serverUrl = "wss://s1.ripple.com:443";
	//private static string serverUrl = "wss://s2.ripple.com:443";
	//"wss://s.altnet.rippletest.net:51233"

	//"0F94DBC1C5C872A653934F33B84D6D3ED09B6A3156AFA000F09BE04E76F3FDA5" transaction

	// "wss://s.altnet.rippletest.net:51233"

	//rpQ3crDGDnUjTxv3qU53XmudsmZMBZshWJ she84PEfrDjRowosdGfPaMbSCTjTH
	//raJtjEhwkS56hR1XAkazwBysVbcMmcgKy6 ssNR7qwKzr5sNEdBtfWfQuBMA65i6
	internal class RippleClientUBC
	{
		private RippleClientUBC() { }
		private static RippleClientUBC rippleClientUBC { get; set; }
		private RippleClient _client { get; set; }

		public RippleClient client
		{
			get => _client;
			set
			{
				if (_client != null)
				{
					value.Disconnect();
				}
				_client = value;
				value.Connect();
			}
		}

		public static RippleClientUBC GetClient()
		{
			if (rippleClientUBC == null)
			{
				rippleClientUBC = new RippleClientUBC();
			}
			return rippleClientUBC;
		}
	}

}
