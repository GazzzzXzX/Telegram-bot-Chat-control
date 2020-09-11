using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BotCore.SQL
{
	internal class Transaction
	{
		/// <summary>
		/// Id уникальной транзакции.
		/// </summary>
		[Key]
		public Int32 Id { get; set; }

		public String Name { get; set; }

		/// <summary>
		/// ID отправителя
		/// </summary>
		[ForeignKey("User")]
		public Int32 UserSenderId { get; set; }

		/// <summary>
		/// User отправитель
		/// </summary>
		public User UserSender { get; set; }

		/// <summary>
		/// ID получателя
		/// </summary>
		[ForeignKey("User")]
		public Int32? UserRecipientId { get; set; } = null;

		/// <summary>
		/// User получатель
		/// </summary>
		public User? UserRecipient { get; set; } = null;

		/// <summary>
		/// Id администратора который зашел в данную транзакцию.
		/// </summary>
		public Int32? UserAdminId { get; set; } = null;

		public Int32? AddresBTCId { get; set; } = null;

		/// <summary>
		/// Приостановление транзакции на время, что бы администратор мог решить проблему.
		/// </summary>
		public Boolean TransactionSuspension { get; set; } = false;

		/// <summary>
		/// Система оплаты:
		/// 1 - BTC;
		/// 2 - USDT;
		/// 3 - Ethereum;
		/// 4 - Ripple;
		/// </summary>
		public Int32 PaymentId { get; set; }

		/// <summary>
		/// Оплата комиссии:
		/// True - отправитель;
		/// False -  получатель;
		/// </summary>
		public Boolean WhoCommissionPay { get; set; } = true;

		/// <summary>
		/// Поступления средств на счёт гаранта:
		/// False - не поступили;
		/// True - поступили;
		/// </summary>
		public Boolean IsPayGarantor { get; set; } = false;

		/// <summary>
		/// Поступления средств на счёт получателя или отправителя:
		/// False - не поступили;
		/// True - поступили;
		/// </summary>
		public Boolean IsPaySenderOrRecipiend { get; set; } = false;

		/// <summary>
		/// Сумма зачисления на счёт.
		/// </summary>
		public Single SumPay { get; set; } = 0.0f;
		public Decimal SumPayNew { get; set; } = 0;

		/// <summary>
		/// Система подтверждения и отмены отправителя:
		/// -1 - не подтвержденно;
		/// 0 - в работе;
		/// 1 - отмена;
		/// 2 - подтвержденно;
		/// </summary>
		public Int32 IsConfirmOrCancelUserSender { get; set; } = 0;

		/// <summary>
		/// Система подтверждения и отмены получателя:
		/// -1 - не подтвержденно;
		/// 0 - в работе;
		/// 1 - отмена;
		/// 2 - подтвержденно;
		/// </summary>
		public Int32 IsConfirmOrCancelUserRecipient { get; set; } = -1;

		/// <summary>
		/// Описание отмены отправитель.
		/// </summary>
		public String DescriptionCancelSender { get; set; }

		/// <summary>
		/// Описание отмены получатель.
		/// </summary>
		public String DescriptionCancelRecipient { get; set; }

		/// <summary>
		/// Для проверке выбраная ли данная транзакция или нет.
		/// </summary>
		public Boolean AddUser { get; set; }

		/// <summary>
		/// ID транзакции для того что бы понимать что деньги пришли и сколько пришло.
		/// </summary>
		/// <value></value>
		public String IdTransaction { get; set; }

		/// <summary>
		/// Проверка на валидность транзакции, если она не прошла валидность сказать об этом пользователь
		/// если False ничего не совершать с данной транзакцией, она считается заблокированной то того момента когда не станет True.
		/// </summary>
		/// <value></value>
		public Boolean CheckSendTransaction { get; set; }

		/// <summary>
		/// публичный ключ кошелька получателя!
		/// </summary>
		public String PublicKeyWallet { get; set; }

		/// <summary>
		/// публичный ключ кошелька отправителя!
		/// </summary>
		public String PublicKeyWalletSender { get; set; }


	}
}