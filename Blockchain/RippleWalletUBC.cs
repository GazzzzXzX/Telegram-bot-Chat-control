using System;
using System.Threading.Tasks;
using BotCore.Blockchain;
using Newtonsoft.Json.Linq;
using Ripple.TxSigning;
using RippleDotNet.Model;
using RippleDotNet.Model.Account;
using RippleDotNet.Model.Transaction.Interfaces;
using RippleDotNet.Model.Transaction.TransactionTypes;
using RippleDotNet.Requests.Transaction;
using Submit = RippleDotNet.Model.Transaction.Submit;

internal class RippleWalletUBC
{
	private RippleModelWalletUBC wallet = new RippleModelWalletUBC();
	private AccountInfo accountInfo;

	public RippleWalletUBC(String addr, String pv) => ChangeWalletUBC(addr, pv).Wait();

	public async Task ChangeWalletUBC(String addr, String pv)
	{
		accountInfo = await RippleClientUBC.GetClient().client.AccountInfo(addr);
		wallet.Address = addr;
		wallet.PrivateKey = pv;
		Console.WriteLine("Address = " + addr + " \n PrivateKey = " + pv);
	}

	public async Task SendMoney(Decimal coin, String destination)
	{
		IPaymentTransaction paymentTransaction = new PaymentTransaction
		{
			Account = wallet.Address,
			Destination = destination
		};
		Decimal dec = coin;
		paymentTransaction.Amount = new Currency { ValueAsXrp = dec };
		paymentTransaction.Sequence = accountInfo.AccountData.Sequence;

		TxSigner signer = TxSigner.FromSecret(wallet.PrivateKey);
		SignedTx signedTx = signer.SignJson(JObject.Parse(paymentTransaction.ToJson()));

		SubmitBlobRequest request = new SubmitBlobRequest
		{
			TransactionBlob = signedTx.TxBlob
		};

		Submit result = await RippleClientUBC.GetClient().client.SubmitTransactionBlob(request);

		AccountInfo accoun = await RippleClientUBC.GetClient().client.AccountInfo(wallet.Address);
		AccountInfo accountIfo = await RippleClientUBC.GetClient().client.AccountInfo(destination);
	}

}