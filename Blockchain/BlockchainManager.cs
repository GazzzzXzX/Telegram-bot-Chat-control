using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using BotCore.Advertising;
using BotCore.SQL;
using NBitcoin;
using Nethereum.RPC.Eth.Transactions;
using Nethereum.Util;
using Nethereum.Web3;
using Newtonsoft.Json;
using QBitNinja.Client;
using QBitNinja.Client.Models;
using File = System.IO.File;
using Transaction = Nethereum.RPC.Eth.DTOs.Transaction;

namespace BotCore.Blockchain
{
	public class BlockchainManager
	{
		public static readonly BlockchainManager Instance = new BlockchainManager();

		public BlockchainSettings Settings
		{
			get => settings;
			set
			{
				settings = value;

				ReCreate();
			}
		}

		private QBitNinjaClient client = new QBitNinjaClient(Network.Main);
		private IEthGetTransactionByHash transactionGetter;
		public BlockchainSettings settings;
		private Web3 ethWeb3;

		private BlockchainManager()
		{
			LoadSettings("settings.json");

			Web3 web3 = new Web3("https://mainnet.infura.io/v3/1023b095206d49f3a66fa113566fb2ea");
			transactionGetter = web3.Eth.Transactions.GetTransactionByHash;

			if (settings == null)
			{
				settings = new BlockchainSettings
				{
					btcWalletPrivateKey = "",
					ethWalletPrivateKey = "",
					xrpWalletPrivateKey = "",
					btcWalletAddress = "",
					ethWalletAddress = "",
					xrpWalletAddress = "",
					transactionPercentFee = 1,
					transactionPercentFeeAdmin = 1
				};
			}

			ReCreate();
		}

		~BlockchainManager()
		{
			//SaveSettings("settings.json");
		}

		private void LoadSettings(String filename)
		{
			if (File.Exists(filename))
			{
				String json = File.ReadAllText(filename);

				settings = (BlockchainSettings)JsonConvert.DeserializeObject(json, typeof(BlockchainSettings));
			}
		}

		public void SaveSettings(String filename)
		{
			using (FileStream fs = File.Create(filename))
			{
				String json = JsonConvert.SerializeObject(settings);

				Byte[] bytes = Encoding.UTF8.GetBytes(json);

				fs.Write(bytes);
			}
		}
		private void ReCreate()
		{
			Nethereum.Web3.Accounts.Account account = new Nethereum.Web3.Accounts.Account(settings.ethWalletPrivateKey);
			ethWeb3 = new Web3(account, "https://mainnet.infura.io/v3/1023b095206d49f3a66fa113566fb2ea");
		}

		/// <summary>
		/// Возвращение денег отправителю если стороны не пришли к согласиию или при оплате денег гаранту не была пройдена проверка.
		/// </summary>
		/// <param name="transactionHash">ID транзакции</param>
		/// <returns></returns>
		public async Task SendBTCBackByHash(String transactionHash, Decimal feePercent = Decimal.Zero)
		{
			uint256 transactionId = uint256.Parse(transactionHash);

			NBitcoin.Transaction transaction = null;

			try
			{
				GetTransactionResponse transactionResponse = await client.GetTransaction(transactionId);
				transaction = transactionResponse.Transaction;
			}
			catch (Exception ex)
			{
				Log.Logging(ex);
			}

			TransactionCheckResult transactionStatus = transaction.Check();

			BitcoinSecret secret = new BitcoinSecret(Key.Parse(settings.btcWalletPrivateKey, Network.Main), Network.Main);
			KeyId d = secret.PubKey.Hash;

			BitcoinAddress destionation = BitcoinAddress.Create(settings.btcWalletAddress, Network.Main);

			foreach (TxOut output in transaction.Outputs)
			{
				if (output.IsTo(destionation))
				{
					Decimal send = output.Value.ToDecimal(MoneyUnit.BTC);

					if (feePercent != Decimal.Zero)
					{
						Decimal totalFee = send * (feePercent / 100.0m);
						send -= totalFee;
					}

					//await SendBTC(output.ScriptPubKey.Hash.GetAddress(Network.Main).ToString(), send);
				}
			}
		}

		/// <summary>
		/// Возвращение денег отправителю если стороны не пришли к согласиию или при оплате денег гаранту не была пройдена проверка.
		/// </summary>
		/// <param name="transactionHash">ID транзакции</param>
		/// <returns></returns>
		public async Task SendETHBackByHash(String transactionHash, Decimal feePercent = Decimal.Zero)
		{
			Transaction transaction = await transactionGetter.SendRequestAsync(transactionHash);

			Decimal ether = Nethereum.Util.UnitConversion.Convert.FromWei(transaction.Value, UnitConversion.EthUnit.Ether);

			if (feePercent != Decimal.Zero)
			{
				Decimal totalFee = ether * (feePercent / 100.0m);
				ether -= totalFee;
			}
			
			await SendETH(transaction.From, ether);
		}

		/// <summary>
		/// Возвращение денег отправителю если стороны не пришли к согласиию или при оплате денег гаранту не была пройдена проверка.
		/// </summary>
		/// <param name="transactionHash">ID транзакции</param>
		/// <returns></returns>
		public async Task SendXRPBackByHash(String transactionHash, Decimal feePercent = Decimal.Zero)
		{
			RippleTransactionUBC rippleWallet_UBC = new RippleTransactionUBC(transactionHash);
			if (rippleWallet_UBC.CheckTransaction() == false)
			{
				return;
			}

			Decimal ripple = rippleWallet_UBC.GetTransactionResponse().Amount.ValueAsXrp.Value;

			if (feePercent != Decimal.Zero)
			{
				Decimal totalFee = ripple * (feePercent / 100.0m);
				ripple -= totalFee;
			}

			String destination = rippleWallet_UBC.GetTransactionResponse().Account;
			await SendRipple(destination, ripple);
		}

		/// <summary>
		/// Отправляет определенное количество BTC к получателю
		/// </summary>
		/// <param name="receiverWalletKey">Публичный ключ одного из сторон</param>
		/// <param name="amount">Сумма</param>
		public async Task SendBTC(String receiverWalletKey, Decimal amount, Decimal commission, AddresBTC addres)
		{
			Money money = new Money(amount, MoneyUnit.BTC);
			Money free  = new Money(0.00000339m, MoneyUnit.BTC);
			CryptoCourse.ExchangeInfo cours = CryptoCourse.GetExchangeInfo("USD", "BTC");
			Decimal perOne = (Decimal) (1 / cours.Last);

			Decimal value_free = Convert.ToDecimal(cours.Last);

			Money min_value  = new Money(perOne, MoneyUnit.BTC);


			BitcoinSecret secret       = new BitcoinSecret(addres.PrivateKey);//Сюда приватный ключ , для каждой сделки свой
			Network network            = secret.Network;
			BitcoinAddress bitAddress  = secret.PubKey.GetAddress(ScriptPubKeyType.Legacy, network);
			Key privateKey             = secret.PrivateKey;

			BalanceSummary balancetemp = client.GetBalanceSummary(bitAddress).Result;
			Money balance  = new Money(commission, MoneyUnit.BTC);

			BitcoinAddress sendAddress = BitcoinAddress.Create(receiverWalletKey, network);
			BitcoinAddress sendAddress2 = BitcoinAddress.Create(settings.btcWalletAddress, network);

			NBitcoin.Transaction txGettingCoin = NBitcoin.Transaction.Create(network);
			txGettingCoin.Outputs.Add(new TxOut(money + free, secret.GetAddress(ScriptPubKeyType.Legacy)));

			Coin[] coins = GetCoinsByAddress(bitAddress,network,0).ToArray();
			money = money - free;
			if (money < Money.Zero || free < Money.Zero || (money + free + balance) < min_value)
			{
				Console.WriteLine("Проверьте введенные данные либо слишком малое значение (Больше 1 долора)");
				return;
			}
			if (coins[0].Amount < money + balance + free)
			{
				Console.WriteLine("Введена не верная сумма либо слишком малое значение");
				return;
			}


			TransactionBuilder transactionBuilder = network.CreateTransactionBuilder();
			NBitcoin.Transaction transaction = NBitcoin.Transaction.Create(network);
			if (balance == Money.Zero)
			{
				transaction = transactionBuilder
						  .AddCoins(coins)
						  .AddKeys(privateKey)
						  .Send(sendAddress, money)
						  .SendFees(free)
						  .SetChange(bitAddress)
						  .BuildTransaction(true);
			}
			else
			{
				transaction = transactionBuilder
						  .AddCoins(coins)
						  .AddKeys(privateKey)
						  .Send(sendAddress, money)
						  .Send(sendAddress2, balance)
						  .SendFees(free)
						  .SetChange(bitAddress)
						  .BuildTransaction(true);
			}

			transaction = transactionBuilder.SignTransaction(transaction);

			Boolean isVerified = transactionBuilder.Verify(transaction);
			if (isVerified)
			{
				BroadcastResponse broadcastResponse = await client.Broadcast(transaction);

				if (!broadcastResponse.Success)
				{
					Console.WriteLine($"BTC Transaction Failed -> ErrorCode: {broadcastResponse.Error.ErrorCode}, ErrMessage: {broadcastResponse.Error.Reason}");
				}
				else
				{
					Console.WriteLine($"BTC Transaction Success, Hash: {transaction.GetHash()}");
				}
			}
		}
		public BitcoinSecret GenerateBTCKey()
		{
			Key key_temp = new Key();
			BitcoinSecret secret_temp = key_temp.GetWif(Network.Main);
			Console.WriteLine(secret_temp.ToString() + " " + secret_temp.GetAddress() + " created BTC wallet");
			return secret_temp;

			//TODO запись в бд для каждой сделки свой
		}

		public List<Coin> GetCoinsByAddress(BitcoinAddress Address, Network net = null, Int32 confirmations = 6)
		{
			QBitNinja.Client.QBitNinjaClient cl = new QBitNinja.Client.QBitNinjaClient(net);
			QBitNinja.Client.Models.BalanceModel bm = cl.GetBalance(new BitcoinPubKeyAddress(Address.ToString())).Result;

			List<Coin> txs = new List<Coin>();

			foreach (BalanceOperation operation in bm.Operations)
			{
				foreach (ICoin cn in operation.ReceivedCoins)
				{
					if (operation.Confirmations >= confirmations)
					{
						Coin C = (Coin)cn;
						txs.Add(C);
					}
				}
			}

			return txs;
		}

		/// <summary>
		///  Отправляет определенное количество ETH к получателю
		/// </summary>
		/// <param name="receiver"></param>
		/// <param name="costETH"></param>
		public async Task SendETH(String receiver, Decimal amount)
		{
			Nethereum.RPC.Eth.DTOs.TransactionReceipt transaction = await ethWeb3.Eth.GetEtherTransferService()
				.TransferEtherAndWaitForReceiptAsync(receiver, amount);

			if (transaction != null)
			{
				Console.WriteLine($"ETH Transaction Success, Hash: {transaction.TransactionHash}");
			}
			else
			{
				Console.WriteLine($"ETH Transaction Error");
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="destination">Адрес кошелка получателя</param>
		/// <param name="amount">Сумма</param>
		/// <returns></returns>
		public async Task SendRipple(String destination, Decimal amount)
		{

			RippleWalletUBC wallet = new RippleWalletUBC(Settings.xrpWalletAddress, Settings.xrpWalletPrivateKey);
			await wallet.SendMoney(amount, destination);
		}

		/// <summary>
		/// Проверяет успешно проведена конкретная транзация или нет (проверка статуса транзации, получателя, кол-ва BTC) 
		/// </summary>
		/// <param name="transactionHash">Хэш транзации для проверки</param>
		/// <param name="receiverWalletHash">BTC кошелек получателя</param>
		/// <param name="costBTC">Количество BTC которое нужно для транзакции</param>
		/// <returns>True если все условия соблюдены</returns>
		public async Task<Money> GetBTCTransactionMoney(String transactionHash, AddresBTC addresBTC)
		{
			uint256 transactionId = uint256.Parse(transactionHash);

			NBitcoin.Transaction transaction = null;

			try
			{
				GetTransactionResponse transactionResponse = await client.GetTransaction(transactionId);
				transaction = transactionResponse.Transaction;
			}
			catch (Exception ex)
			{
				Log.Logging(ex);
			}

			TransactionCheckResult transactionStatus = transaction.Check();

			BitcoinSecret secret = new BitcoinSecret(Key.Parse(addresBTC.PrivateKey, Network.Main), Network.Main);
			BitcoinAddress addressTo = BitcoinAddress.Create(addresBTC.PublickKey, Network.Main);

			Decimal totalReceived = 0;
			foreach (TxOut output in transaction.Outputs)
			{
				if (output.IsTo(addressTo))
				{
					totalReceived += output.Value.ToDecimal(MoneyUnit.BTC);
				}
			}

			return new Money(totalReceived, MoneyUnit.BTC);
		}

		/// <summary>
		///  Проверяет успешно проведена конкретная транзация или нет (проверка статуса транзации, получателя, кол-ва ETH) 
		/// </summary>
		/// <param name="transactionHash">Хэш транзации для проверки</param>
		/// <param name="receiverWalletHash">ETH кошелек получателя</param>
		/// <param name="costETH">Количество ETH которое нужно для транзакции</param>
		/// <returns>True если все условия соблюдены</returns>
		public async Task<Money> GetETHTransactionMoney(String transactionHash)
		{
			Transaction transaction = await transactionGetter.SendRequestAsync(transactionHash);

			if (transaction.To.ToLower() != settings.ethWalletAddress.ToLower())
			{
				return new Money(0, MoneyUnit.BTC);
			}

			Decimal ether = Nethereum.Util.UnitConversion.Convert.FromWei(transaction.Value, UnitConversion.EthUnit.Ether);

			return new Money(ether, MoneyUnit.BTC);
		}

		public async Task<Money> GetXRPTransactionMoney(String transactionHash)
		{
			RippleTransactionUBC rippleWallet_UBC = new RippleTransactionUBC(transactionHash);

			if (rippleWallet_UBC.CheckTransaction() == false)
			{
				return null;
			}
			if (rippleWallet_UBC.CheckTransactionAsync(settings.xrpWalletAddress) == false)
			{
				return null;
			}
			return rippleWallet_UBC.GetMoney();
		}
	}
}
