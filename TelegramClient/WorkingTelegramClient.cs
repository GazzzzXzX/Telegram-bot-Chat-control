using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeleSharp.TL;
using TeleSharp.TL.Channels;
using TeleSharp.TL.Messages;

namespace BotCore.TelegramClient
{
	class ModelParserInfo
	{
		public ModelParserInfo()
		{
			ChannelInfo            = new ChannelInfo();
			ChannelParserMediaInfo = new ParserMediaInfo();
		}
		public String          About                  { get; set; }
		public TLPhoto         Avatar                 { get; set; }
		public Int32             CountPeople            { get; set; }
		public Int32             CountMessage           { get; set; }
		public Int32             CountMediaFiles        { get; set; }
		public Int32             AverageViews           { get; set; }
		public Int32             CountPosts             { get; set; }
		public ChannelInfo     ChannelInfo            { get; set; }
		public ParserMediaInfo ChannelParserMediaInfo { get; set; }
	}
	
	class ParserMediaInfo
	{
		public Int32 CountPhoto   { get; set; }
		public Int32 CountVideo   { get; set; }
		public Int32 CountContact { get; set; }
		public Int32 CountWebPag  { get; set; }
	}
	
	/// <summary>
	/// Для работы с TelegramClient
	/// </summary>
	internal class WorkingTelegramClientTest
	{
		private TLSharp.Core.TelegramClient client;
		/// <summary>
		/// Изменении сессии
		/// </summary>
		/// <param name="session"></param>
		public void Start(SessionTelegram session) => client = session.client;
		/// <summary>
		/// Возвращает код-во людей
		/// </summary>
		/// <param name="channelName"></param>
		/// <returns>Получение кол-во людей (int) в канале  , если что-то пошло не так вернет -1</returns>
		public async Task<Int32> GetCountParticipantsChannelandSuperGroup(String channelName)
		{
			if (client == null)
			{
				return -1;
			}
			TLAbsDialogs dialogs_temp = await client.GetUserDialogsAsync();
			TLDialogsSlice dialogs = dialogs_temp as TLDialogsSlice;
			TeleSharp.TL.Messages.TLChatFull res = new TeleSharp.TL.Messages.TLChatFull();
			try
			{
				TLChannel channelInfo = (await client.SendRequestAsync<TeleSharp.TL.Contacts.TLResolvedPeer>(
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
				return -1;
			}
			return (res.FullChat as TLChannelFull).ParticipantsCount.Value;
		}

	}

	internal class WorkingTelegramClient
	{
		public static async Task<Int32> GetCountParticipantsChannelandSuperGroup(String channelName, TLSharp.Core.TelegramClient client)
		{
			if (client == null)
			{
				return -1;
			}
			TLAbsDialogs dialogs_temp = await client.GetUserDialogsAsync();
			TLDialogsSlice dialogs = dialogs_temp as TLDialogsSlice;
			TeleSharp.TL.Messages.TLChatFull res = new TeleSharp.TL.Messages.TLChatFull();
			try
			{
				TLChannel channelInfo = (await client.SendRequestAsync<TeleSharp.TL.Contacts.TLResolvedPeer>(
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
				return -1;
			}
			return (res.FullChat as TLChannelFull).ParticipantsCount.Value; 
		}

		public static async Task<ModelParserInfo> ParserInfoAboutChannelAndSuperGroup(String channelName, TLSharp.Core.TelegramClient client, Int32 countMessageParse = 200)
        {
            ModelParserInfo info = new ModelParserInfo();
            ChannelInfo result = new ChannelInfo();

            List<Int32> AverageViews = new List<Int32>();
            TLChannel channelInfo;
            if (client == null)
            {
                return null;
            }
            var dialogs_temp = client.GetUserDialogsAsync().GetAwaiter().GetResult();
            TLDialogsSlice dialogs = dialogs_temp as TLDialogsSlice;
            TeleSharp.TL.Messages.TLChatFull res = new TeleSharp.TL.Messages.TLChatFull();

            channelInfo = (client.SendRequestAsync<TeleSharp.TL.Contacts.TLResolvedPeer>(
              new TeleSharp.TL.Contacts.TLRequestResolveUsername
              {
                  Username = channelName
              }).ConfigureAwait(false)).GetAwaiter().GetResult().Chats.Last() as TeleSharp.TL.TLChannel;

            TLRequestGetFullChannel req = new TLRequestGetFullChannel()
            {
                Channel = new TLInputChannel() { AccessHash = channelInfo.AccessHash.Value, ChannelId = channelInfo.Id }
            };

            res = client.SendRequestAsync<TeleSharp.TL.Messages.TLChatFull>(req).GetAwaiter().GetResult();
            Int32 offset = 0;

            result.Channel = channelInfo;
            result.ChatFull = res;
            info.ChannelInfo = result;
            info.CountPeople = (res.FullChat as TLChannelFull).ParticipantsCount.Value;
            info.Avatar = (res.FullChat as TLChannelFull).ChatPhoto as TLPhoto;
            info.About = (res.FullChat as TLChannelFull).About;
            TLChannelMessages temp = new TLChannelMessages();
            TLVector<TLAbsMessage> msgs = new TLVector<TLAbsMessage>();
            do
            {

                try
                {
                    temp = client.SendRequestAsync<TLChannelMessages>
                   (new TLRequestGetHistory()
                   {
                       Peer = new TLInputPeerChannel() { ChannelId = result.Channel.Id, AccessHash = (long)result.Channel.AccessHash },
                       Limit = 500,
                       AddOffset = offset,
                       OffsetId = 0
                   }).GetAwaiter().GetResult();
                    msgs = temp.Messages;
                }
                catch
                {
                    await Task.Delay(3000);
                }
                if (countMessageParse != 0)
                {
                    offset = temp.Count - countMessageParse;
                    countMessageParse = 0;
                }
                if (temp.Count > offset || temp.Count == 0)
                {

                    offset += msgs.Count;
                    foreach (var msg in msgs)
                    {
                        if (msg is TLMessage)
                        {

                            TLMessage sms = msg as TLMessage;
                            if (sms.Views != null)
                                AverageViews.Add(sms.Views.Value);
                            info.CountMessage++;
                            if (sms.Media != null)
                            {
                                info.CountMediaFiles++;

                                if (sms.Media is TLMessageMediaDocument)
                                {
                                    info.ChannelParserMediaInfo.CountVideo++;
                                }
                                else if (sms.Media is TLMessageMediaPhoto)
                                {
                                    info.ChannelParserMediaInfo.CountPhoto++;
                                }
                                else if (sms.Media is TLMessageMediaWebPage)
                                {
                                    info.ChannelParserMediaInfo.CountWebPag++;
                                }
                                else if (sms.Media is TLMessageMediaContact)
                                {
                                    info.ChannelParserMediaInfo.CountContact++;
                                }
                            }
                        }
                        if (msg is TLMessageService)
                        {
                            continue;
                        }
                    }

                }
                else
                    break;
            } while (temp.Count > offset || temp.Count == 0);

            if (AverageViews.Count != 0)
            {
	            info.AverageViews = AverageViews.Sum() / AverageViews.Count;
            }

            return info;
        }

	}
}
