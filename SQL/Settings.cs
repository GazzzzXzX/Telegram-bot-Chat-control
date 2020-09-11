using System.ComponentModel.DataAnnotations;

namespace BotCore
{
	internal class Settings
	{
		[Key]
		public System.Int32 ID { get; set; }

		public System.Int64 ChannelAdmin { get; set; }

		public System.Int32 BanOfComplaint { get; set; }

		public System.Int32 CountPost { get; set; }

		public System.Int32 AddUser { get; set; }

		public System.String PasswordAdmin { get; set; }

		public System.Boolean Flud { get; set; }

		public System.DateTime Timer { get; set; }

		public System.Int32 CountBanMat { get; set; }
		public System.Int32 IsBanOrKicOrMutkMat { get; set; }

		public System.Int32 CountBanFlud { get; set; }
		public System.Int32 IsBanOrKickOrMutFlud { get; set; }

		public System.Double ProcentMessage { get; set; }

		public System.Int32 CountLink { get; set; }
		public System.Int32 IsBanOrKickOrMutLink { get; set; }

		public System.Int32 Coin { get; set; }

		public System.String Regulations { get; set; }
	}
}