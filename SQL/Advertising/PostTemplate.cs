using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BotCore.SQL
{
	internal class PostTemplate
	{
		public PostTemplate() => PostTime = new HashSet<PostTime>();

		[Key]
		public Int32 Id { get; set; }

		[Required]
		public String Name { get; set; }

		public Boolean IsOnValidation { get; set; }
		public Boolean IsValidated { get; set; }
		public Boolean IsPaid { get; set; }

		public Int32 isPinnedMessage { get; set; }

		[ForeignKey("AdUser")]
		public System.Int32 AdUserId { get; set; }

		public AdUser AdUser { get; set; }

		public virtual ICollection<PostContent> PostContent { get; set; }

		public virtual ICollection<PostTime> PostTime { get; set; }

		public virtual ICollection<PostChannel> PostChannel { get; set; }
	}
}