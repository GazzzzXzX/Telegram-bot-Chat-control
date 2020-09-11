using System;
using System.Collections.Generic;
using System.Text;
using TeleSharp.TL;

namespace BotCore.TelegramClient
{
	class ChannelInfo
	{
		public TLChannel Channel { get; internal set; }
		public TeleSharp.TL.Messages.TLChatFull ChatFull { get; internal set; }
		public List<TLUser> Users { get; set; } = new List<TLUser>();
		public List<TLUser> Admins { get; set; } = new List<TLUser>();

	}
}
