using System;

namespace BotCore.Blockchain
{
	internal static class CommandTextBlockchain
	{
		#region GuarantorMeinMenu

		public static String TransactionCreationGuarantor = "/TransactionCreationGuarantor!";
		public static String ShowMyTransaction = "/ShowMyTransaction!";
		public static String ReputationUser = "/ReputationUser!";
		public static String ConfirmGuarantorMeinMenu = "/ConfirmGuarantorMeinMenu!";
		public static String BackToMeinMenu = "/BackToMeinMenu!";

		#endregion GuarantorMeinMenu
		#region Settings 
		public static String SettingsCommisionCripta = "/SettingsCommisionCripta!";
		public static String SettingsCommisionAdmin = "/SettingsCommisionAdmin!";
		#region Comission
		public static String LeftComissionAdmin = "/LeftComissionAdmin!";
		public static String RightComissionAdmin = "/RightComissionAdmin!";

		public static String LeftComissionCripta = "/LeftComissionCripta!";
		public static String RightComissionCripta = "/RightComissionCripta!";
		#endregion
		#endregion

		#region AdminCall

		public static String AdminCallBlockchain = "/AdminCallBlockchain!";

		#endregion AdminCall

		#region TransactionMenu

		#region TransactionCreationGuarantor

		public static String AddUserInTransaction = "/AddUserInTransaction!";
		public static String ChoosingPaymentMethod = "/ChoosingPaymentMethod!";
		public static String CommissionPayment = "/CommissionPayment!";
		public static String BackToGuarantorMeinMenu = "/BackToGuarantorMeinMenu!";

		#endregion TransactionCreationGuarantor

		#region AddUserInTransactionToBack

		public static String BackToTransactionCreationGuarantor = "/BackToTransactionCreationGuarantor!";

		#endregion AddUserInTransactionToBack

		#region SendTheConfirmationToUser

		public static String ConfirmSendTheConfirmationToUser = "/ConfirmSendTheConfirmationToUser!";
		public static String CancelSendTheConfirmationToUser = "/CancelSendTheConfirmationToUser!";

		#endregion SendTheConfirmationToUser

		#region ChoosingPaymentMethod

		public static String BTCPaymentMethod = "/BTCPaymentMethod!";
		public static String USDTPaymentMethod = "/USDTPaymentMethod!";
		public static String EthereumPaymentMethod = "/EthereumPaymentMethod!";
		public static String RipplePaymentMethod = "/RipplePaymentMethod!";
		// -- public static String BackToTransactionCreationGuarantor = "/BackToTransactionCreationGuarantor!";

		#endregion ChoosingPaymentMethod

		#region ChoosingPaymentMethodToBack

		public static String BackToChoosingPaymentMethod = "/BackToChoosingPaymentMethod!";

		#endregion ChoosingPaymentMethodToBack

		#region CommissionPayment

		public static String CommissionPaymentRecipient = "/CommissionPaymentRecipient!";
		public static String CommissionPaymentSender = "/CommissionPaymentSender!";
		// -- public static String BackToTransactionCreationGuarantor = "/BackToTransactionCreationGuarantor!";

		#endregion CommissionPayment

		#region BackToSetMoneyCount

		public static String BackToSetMoneyCount = "/BackToSetMoneyCount!";

		#endregion BackToSetMoneyCount

		#endregion TransactionMenu

		#region MyTransactionMenu

		#region ShowMyTransaction

		public static String NameTransactionConfirm = "/NameTransactionConfirm!";
		public static String NameTransactionCancel = "/NameTransactionCancel!";
		public static String NameTransaction = "/NameTransaction!";
		// -- public static String BackToGuarantorMeinMenu = "/BackToGuarantorMeinMenu!";

		#endregion ShowMyTransaction

		#region ChouseMyTransaction

		public static String ConfirmMyTransaction = "/ConfirmMyTransaction!";
		public static String CancelMyTransaction = "/CancelMyTransaction!";
		public static String BackToShowMyTransaction = "/BackToShowMyTransaction!";

		#endregion ChouseMyTransaction

		#region ConfirmThisTransaction

		public static String ConfirmThisTransaction = "/ConfirmThisTransaction!";
		public static String BackToSelectConfirmOrCancelThisTransaction = "/BackToSelectConfirmOrCancelThisTransaction!";

		#endregion ConfirmThisTransaction

		#region ConfirmOrCancelThisTransactionUserTwo

		public static String ConfirmThisTransactionUserTwo = "/ConfirmThisTransactionUserTwo!";
		public static String CancelThisTransactionUserTwo = "/CancelThisTransactionUserTwo!";

		#endregion ConfirmOrCancelThisTransactionUserTwo

		#region CancelThisTransactionUserTwo

		// -- public static String BackToSelectConfirmOrCancelThisTransaction = "/BackToSelectConfirmOrCancelThisTransaction!";

		#endregion CancelThisTransactionUserTwo

		#region BackToConfirmOrCancelThisTransactionUserTwo

		public static String BackToConfirmOrCancelThisTransactionUserTwo = "/BackToConfirmOrCancelThisTransactionUserTwo!";

		#endregion BackToConfirmOrCancelThisTransactionUserTwo

		#endregion MyTransactionMenu

		#region Reputation

		#region ReputationUser

		public static String ConfirmTransactionReputationUser = "/ConfirmTransactionReputationUser!";
		public static String CancelTransactionReputationUser = "/CancelTransactionReputationUser!";
		// -- public static String BackToGuarantorMeinMenu = "/BackToGuarantorMeinMenu!";

		#endregion ReputationUser

		#region ShowReputationConfirmTransaction

		public static String NameReputationConfirmTransaction = "/NameReputationConfirmTransaction!";
		public static String BackToReputationUser = "/ReputationUser!";

		#endregion ShowReputationConfirmTransaction

		#region ShowReputationCancelTransaction

		public static String NameReputationCancelTransaction = "/NameReputationCancelTransaction!";
		// -- public static String BackToReputationUser = "/ReputationUser!";

		#endregion ShowReputationCancelTransaction

		#region ShowOneReputationConfirmTransaction

		public static String BackToShowOneReputationConfirmTransaction = "/ShowOneReputationConfirmTransaction!";

		#endregion ShowOneReputationConfirmTransaction

		#region ShowOneReputationCancelTransaction

		public static String BackToShowOneReputationCancelTransaction = "/BackToShowOneReputationCancelTransaction!";

		#endregion ShowOneReputationCancelTransaction

		#endregion Reputation

		#region AdminBlackChain

		public static String GetAdminInBlockChain = "/GetAdminInBlockChain!";
		public static String GetAdminInMyTransaction = "/GetAdminInMyTransaction!";

		#region SetAdminBlackChain

		public static String SetConfirmAdminInBlockChain = "/SetConfirmAdminInBlockChain!";
		public static String SetCancelAdminInMyTransaction = "/SetCancelAdminInMyTransaction!";

		#endregion SetAdminBlackChain

		#region GetMoneyAdminBlackChain

		public static String GetMoneySenderAdminInBlockChain = "/GetMoneySenderAdminInBlockChain!";
		public static String GetMoneyRecipientAdminInMyTransaction = "/GetMoneyRecipientAdminInMyTransaction!";

		#endregion GetMoneyAdminBlackChain

		#region SettingsAdminInBlockChain

		public static String SettingsAdminInBlockChain = "/SettingsAdminInBlockChain!";

		#endregion SettingsAdminInBlockChain

		#endregion AdminBlackChain
	}
}