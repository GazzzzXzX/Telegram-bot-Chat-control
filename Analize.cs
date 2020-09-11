using System;
using System.Threading;
using System.Threading.Tasks;

using BotCore.Advertising;
using Quartz;
using Quartz.Impl;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace BotCore
{
	internal class Analize
	{
		private InlineButton _inlineButton = new InlineButton();
		private DataBase db = null;
		private TelegramBotClient botClient = null;
		private Message _message = null;

		public async void ChekRegister(TelegramBotClient bot, System.Object message)
		{
			_message = message as Message;
			db = Singleton.GetInstance().Context;
			botClient = bot;

			try
			{
				Channel channel = db.GetChannel(_message);
				if (channel == null && _message.Chat.Type == Telegram.Bot.Types.Enums.ChatType.Supergroup)
				{
					Channel[] channels = db.GetChannels();

					Chat chat = await botClient.GetChatAsync(_message.Chat.Id);

					System.String NewChannel = $"Здравствуйте у нас появился новая группа @{_message.Chat.Username}!\n{chat.Description}";

					foreach (Channel ch in channels)
					{
						botClient.SendText(ch.IDChannel, NewChannel);
					}

					db.SetChannel(chat);
					db.Save();
					
					channel = db.GetChannel(_message);
					
					await Task.Run(() => AddNewChannel.SetTmessage());
				}

				User user = db.GetUser(_message.From.Id);

				TMessage tmessage =  db.GetMessage(_message);
				Settings settings = db.GetSettings();

				if ((user == null || user.FIO == null || user.Number == "0") && _message.Chat.Type != ChatType.Private)
				{
					botClient.DeleteMessage(_message.Chat.Id, _message.MessageId, "Невозможно удалить сообщение так как оно не было отправленное!");

					System.String textChannel = $"❗Здравствуйте, что бы писать в чатах UBC - пройдите регистрацию в боте, это занимает меньше 1 минуты.\nСсылка на бота/регистрацию: @{botClient.GetMeAsync().Result.Username}\n\n📜Полный список чатов UBC @allUBC";


					try
					{
						if ((_message.Type != MessageType.Photo) || (_message.Caption != null))
						{
							await botClient.SendTextMessageAsync(_message.From.Id, textChannel,
							                                     replyMarkup: _inlineButton.Register);
						}
					}
					catch
					{
						Message mes = null;

						if ((channel.IDMessage == 0    && _message.Type     != MessageType.Photo) ||
						    (_message.Caption  != null && channel.IDMessage == 0))
						{
							mes = await botClient.SendTextMessageAsync(_message.Chat.Id, textChannel);

							channel.IDMessage = mes.MessageId;
							db.SetValue(new ChannelMessage()
							{
								ChannelID         = channel.IDChannel,
								MessageId         = mes.MessageId,
								DateMessageDelete = DateTime.Now.AddSeconds(20)
							});

							db.Save();
						}
					}
				}
				else if (user.Chain > 0 && user.Chain < (System.Int32)Advertising.SetChain.GetPayments && _message.Chat.Type == Telegram.Bot.Types.Enums.ChatType.Private)
				{
					FIO fIO = new FIO();
					fIO.SetNext(
						new Number()).SetNext(
						new ChaingeFIO()).SetNext(
						new ChaingeNumber()).SetNext(
						new SetReviewsName()).SetNext(
						new SearchIDUser()).SetNext(
						new SearchNumberUser()).SetNext(
						new ChangeReviewsName()).SetNext(
						new SetSendComplaint()).SetNext(
						new SetSendAppeal()).SetNext(
						new Admin()).SetNext(
						new SetAdminPassword()).SetNext(
						new Ban()).SetNext(
						new KickUser()).SetNext(
						new AdminAdd()).SetNext(
						new AdminDelete()).SetNext(
						new PostCount()).SetNext(
						new WordAdd()).SetNext(
						new AddUsers()).SetNext(
						new FludUser()).SetNext(
						new ChannelAdd()).SetNext(
						new NewAdmin2Lvl()).SetNext(
						new WordDelete());

					ChainOfResposnsibility(botClient, fIO, message, user);
				}
				else if (user.Chain >= (System.Int32)Advertising.SetChain.GetPayments && _message.Chat.Type == Telegram.Bot.Types.Enums.ChatType.Private)
				{
					Advertising.Payments payments = new Advertising.Payments();
					payments.SetNext(
						new AddPhotoAdvertising()).SetNext(
						new ContentEditorText()).SetNext(
						new SetUserInTransaction()).SetNext(
						new SetBlockChain()).SetNext(
						new SetDiscriptionInTransaction()).SetNext(
						new SetIdTransaction()).SetNext(
						new SetPublicKeyUserTwo()).SetNext(
						new AddPhotoChannel()).SetNext(
						new AddCategotyInDataBase()).SetNext(
						new AddChannelInCategoryInDataBase()).SetNext(
						new LinkChat()).SetNext(
						new LinkChatPharase()).SetNext(
						new AnaliticsPhraseUser()).SetNext(
						new AdminCallBlockchainUser()).SetNext(
						new MessageUserBot()).SetNext(
						new AddAccauntBotUser()).SetNext(
						new AddAccauntCode()).SetNext(
						new PostCountChannel()).SetNext(
						new ThisRegulationsUBC()).SetNext(
						new ThisDeleteChat()).SetNext(
						new ThisDeleteCategory()).SetNext(
						new SetPriceTime()).SetNext(
						new SetPriceStandartMessage()).SetNext(
						new SetStaticTimePinnedPrice()).SetNext(
						new SetPriceStatic()).SetNext(
						new SetStaticTimePinnedNotificationPrice()).SetNext(
						new NotificationTextChain()).SetNext(
						new NotificationButtonChain()).SetNext(
						new PictureNotificationChain());

					payments.Handle(user.Chain, botClient, message);
				}
				else
				{
					FludChain fludChain = new FludChain();
					fludChain.SetNext(
						new AddPeopleChain()).SetNext(
						new PostNullChain()).SetNext(
						new CheckChannelChain()).SetNext(
						new CheckWordAdnLinkChain()).SetNext(
						new CheckInviteChain()).SetNext(
						new CheckPhotoChain());
					
					fludChain.Handle(botClient, message, user, settings, tmessage, channel);
				}
			}
			catch (System.Exception ex)
			{
				Log.Logging("Класс Analize - " + ex);
			}
		}

		public void ChainOfResposnsibility(TelegramBotClient botClient, AbstractHandler handler, System.Object _message, User user)
		{
			System.Object resul = handler.Handle(user.Chain, botClient, _message);
		}
	}
}