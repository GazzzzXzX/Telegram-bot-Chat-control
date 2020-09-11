using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BotCore
{
	internal class Channel
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public System.Int64 IDChannel { get; set; }

		public System.Int32 IDMessage { get; set; }

		public System.Boolean MessageDelete { get; set; } = false;

		public System.DateTime DateMessageDelete { get; set; }

		public System.String ChannelName { get; set; }

		public System.String InviteLink { get; set; }

		public System.String Description { get; set; }

		public System.String PhotoLink { get; set; }

		public System.String LinkChannel { get; set; }

		public Single Price { get; set; } = 0.5f;

		public Int32 CategoryId { get; set; }

		public Int32 PostCount { get; set; }

		public Boolean isPostCount { get; set; }
	}
}