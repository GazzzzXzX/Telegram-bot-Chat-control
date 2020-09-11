using System;

namespace BotCore.Blockchain
{
	[Serializable]
	public class BlockchainSettings
	{
		public String btcWalletPrivateKey;
		public String ethWalletPrivateKey;
		public String xrpWalletPrivateKey;

		public String btcWalletAddress;
		public String ethWalletAddress;
		public String xrpWalletAddress;

		public Decimal transactionPercentFee;
		public Decimal transactionPercentFeeAdmin;

		public BlockchainSettings()
		{
		}
	}
}
