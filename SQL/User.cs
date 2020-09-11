using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BotCore
{
	internal class User
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public System.Int32 ID { get; set; }

		public System.String Username { get; set; }
		public System.String FIO { get; set; }
		public System.String Number { get; set; }

		public System.Int32 AddMembers { get; set; }
		public System.Boolean PayConfirm { get; set; }
		public System.DateTime PayDate { get; set; }

		public System.Int32 Chain { get; set; }

		public System.Double SumRating { get; set; }

		public System.DateTime BanDate { get; set; }
		public System.DateTime DateRegister { get; set; }
		public System.Int32 CountReviews { get; set; }

		public System.Int32 MessageID { get; set; }

		public System.Int32 IDRecipient { get; set; }

		public System.Int32 IsAdmin { get; set; }
		public System.Int64 ChatID { get; set; }

		public System.String BanDescript { get; set; }
		public System.Int32 MessageBan { get; set; }
		public System.Int32 CountBanAddPeople { get; set; }

		/// <summary>
		/// Количество выданных банов!
		/// </summary>
		public System.Int32 Test { get; set; }

		public System.String MessageComplaint { get; set; }
	}
}