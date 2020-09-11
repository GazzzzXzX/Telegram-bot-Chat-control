using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BotCore.SQL
{
	internal class AdUser
	{
		[Key, ForeignKey("User")]
		public Int32 UserId { get; set; }

		public User User { get; set; }

		public Single Balance { get; set; }

		[ForeignKey("PostTemplate")]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public Int32 EditingPostTemplateId { get; set; }

		public Int32 Order { get; set; }

		public virtual ICollection<PostTemplate> PostTemplates { get; set; }
	}
}