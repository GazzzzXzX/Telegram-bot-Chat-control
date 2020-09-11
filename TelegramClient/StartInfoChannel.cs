using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeleSharp.TL;
using TeleSharp.TL.Channels;
using TeleSharp.TL.Contacts;
using TeleSharp.TL.Messages;
using TLSharp.Core;
using TLChatFull = TeleSharp.TL.TLChatFull;

namespace BotCore.TelegramClient
{
	internal static class StartInfoChannel
	{
		private static Boolean ThereIsUserInChannels(ChannelInfo channel, Int32 id_user) => channel.Users.Any(p => p.Id == id_user);

		private static Boolean ThereIsUserInChannels(List<ChannelInfo> channel, Int32 id_user) => channel.Any(p => p.Users.Any(p => p.Id == id_user) == true);
		
		public static List<ChannelInfo> GetChannelByUser(List<ChannelInfo> channels, Int32 id_user)
		{
			if (ThereIsUserInChannels(channels, id_user) == false)
			{
				return null;
			}
			return channels.Where(p => p.Users.Any(p => p.Id == id_user) == true).ToList();
		}

		public static async Task<ChannelInfo> GetInfoGroup(String channelName, TLSharp.Core.TelegramClient client)
		{
			ChannelInfo result = new ChannelInfo();
			TLChannel channelInfo;
			if (client == null)
			{
				return null;
			}
			var dialogs_temp = await client.GetUserDialogsAsync();
			TLDialogsSlice dialogs = dialogs_temp as TLDialogsSlice;
			TeleSharp.TL.Messages.TLChatFull res = new TeleSharp.TL.Messages.TLChatFull();
			try
			{
				channelInfo = (await client.SendRequestAsync<TeleSharp.TL.Contacts.TLResolvedPeer>(
			   new TeleSharp.TL.Contacts.TLRequestResolveUsername
			   {
				   Username = channelName
			   }).ConfigureAwait(false)).Chats.Last() as TeleSharp.TL.TLChannel;

				TLRequestGetFullChannel req = new TLRequestGetFullChannel()
				{
					Channel = new TLInputChannel() { AccessHash = channelInfo.AccessHash.Value, ChannelId = channelInfo.Id }
				};

				res = await client.SendRequestAsync<TeleSharp.TL.Messages.TLChatFull>(req);

			}
			catch
			{
				return null;
			}
			Int32 offset = 0;
			result.Channel = channelInfo;
			result.ChatFull = res;
			while (offset < (res.FullChat as TLChannelFull).ParticipantsCount)
			{

				var pReq = new TLRequestGetParticipants()
				{
					Channel = new TLInputChannel() { AccessHash = channelInfo.AccessHash.Value, ChannelId = channelInfo.Id },
					Filter = new TLChannelParticipantsRecent() { },
					Limit = 200,
					Offset = offset
				};

				var pRes = await client.SendRequestAsync<TLChannelParticipants>(pReq);
				//TLChatParticipantAdmin

				for (Int32 i = 0; i < pRes.Participants.Count; i++)
				{
					if (pRes.Participants[i] is TLChannelParticipantEditor || pRes.Participants[i] is TLChannelParticipantCreator
						|| pRes.Participants[i] is TLChannelParticipantModerator)
					{
						result.Admins.Add(pRes.Users[i] as TLUser);
					}
				}
				result.Users.AddRange(pRes.Users.Cast<TLUser>());
				offset += 200;
				await Task.Delay(500);
			}

			return result;
		}

		public static async Task<List<ChannelInfo>> GetInfoGroup(List<Channel> channels, TLSharp.Core.TelegramClient client)
		{
			List<ChannelInfo> channelInfos = new List<ChannelInfo>();
			for (Int32 i = 0; i < channels.Count; i++)
			{
				channelInfos.Add(await GetInfoGroup(channels[i].InviteLink.Replace("@", String.Empty), client));
			}
			return channelInfos;
		}
	}
}
