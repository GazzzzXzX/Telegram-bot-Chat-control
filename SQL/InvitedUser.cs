using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BotCore
{
	internal class InvitedUser
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public System.Int32 UserAddedId { get; set; }

		public System.Int32 UserWhoAddedId { get; set; }
		
		public System.Int64 ChannelId { get; set; }
	}
}