using System;
using System.Threading.Tasks;
using NBitcoin;
using RippleDotNet.Model;
using RippleDotNet.Responses.Transaction.TransactionTypes;
namespace BotCore.Blockchain
{
	internal class RippleTransactionUBC
	{
		private PaymentTransactionResponse transaction;

		public async Task ChangeAddress(String trans) => transaction = await RippleClientUBC.GetClient().client.Transaction(trans) as PaymentTransactionResponse;

		public RippleTransactionUBC(String trans) => ChangeAddress(trans).Wait();

		public Decimal? CheckTransactionAmountAsync(Decimal? coin)//trans - Транзакция | distination - куда | payment - тот кто платит | coin - сколько должен оплатить (UXR)
		{

			if (transaction.Amount.ValueAsXrp < coin && transaction.Amount.ValueAsXrp != coin)
			{
				return (coin - transaction.Amount.ValueAsXrp);
			}
			return 0;
		}//Проверка на отправленную сумму

		public Boolean CheckTransactionAsync(String distination)//trans - Транзакция | distination - куда | payment - тот кто платит | coin - сколько должен оплатить (UXR)
		{

			if (transaction == null)
			{
				return false;
			}
			if (transaction.Destination != distination)
			{
				return false;
			}
			if (transaction.Validated == false)
			{
				return false;
			}
			if (transaction.TransactionType != TransactionType.Payment)
			{
				return false;
			}
			return true;
		}// Валлидная ли транзакция
		public Boolean CheckTransaction()
		{
			if (transaction != null)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		public Money GetMoney() => new Money(Convert.ToUInt64(transaction.Amount.ValueAsXrp.Value), MoneyUnit.BTC);
		public PaymentTransactionResponse GetTransactionResponse() => transaction;
		public RippleModelUBCTransaction ShowTransactionAsync()//trans - Транзакция
		{

			if (transaction == null)
			{
				return null;
			}
			if (transaction.TransactionType != TransactionType.Payment)
			{
				return null;
			}
			RippleModelUBCTransaction show = new RippleModelUBCTransaction()
			{
				Hash = transaction.Hash,
				Payment = transaction.Account,
				Distination = transaction.Destination,
				Coin = transaction.Amount.ValueAsXrp,
				Id = transaction.LedgerIndex,
				Date = transaction.Date
			};

			return show;
		}

	}
}
