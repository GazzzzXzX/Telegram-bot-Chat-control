using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BotCore.Advertising;
using BotCore.Blockchain;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCore
{
	internal class PublishChatNotification : AbsCommand, IStandartCommand
	{
		public override System.String Name { set; get; } = CommandText.PublishChatNotification;
	
		private DataBase db = null;
		private User user = null;
		private CallbackQuery _message = null;
		private ButtonAndTextNotication TextAndButton = null;
		private List<CollectionButtonNotification> CollectionNotification = new List<CollectionButtonNotification>();
	
		public override void Execute(TelegramBotClient botClient, System.Object message)
		{
			IStandartCommand standartCommand = new CoinAdd();
	
			if (standartCommand.SetCallbackQuery(message, out _message)) return;
	
			if (standartCommand.SetDataBase(out db)) return;
	
			if (standartCommand.SetUserAndCheckIsNull(botClient, _message, out user, db)) return;

			SendMessage(botClient);
		}

		private Boolean CheckIsReady()
		{
			TextAndButton = db.GetButtonAndTextNotication(user);
			if (TextAndButton == null || TextAndButton.Text == null)
				return false;
			
			CollectionNotification = db.GetListCollectionButtonNotification(TextAndButton);

			if (CollectionNotification.Count == 0)
				return false;

			foreach (CollectionButtonNotification collectionButtonNotification in CollectionNotification)
			{
				if (collectionButtonNotification.buttonNotification        == null ||
				    collectionButtonNotification.buttonAndTextNotification == null)
					return false;
			}
			
			return true;
		}

		private async Task<Boolean> SendNotificationAllUsers(TelegramBotClient botClient)
		{
			Channel[] channels = db.GetChannels();
			InlineButton inlineButton = new InlineButton();
			var temp = inlineButton.PublishNotification(CollectionNotification, true);
			var picture = db.GetListCollectionPictureNotification(TextAndButton);
			NotificationChat[] notificationChats = db.GetNotificationChats();
			Int32 Count = 0;
			foreach (var ThisUser in channels)
			{
				if (notificationChats == null)
				{
					if (notificationChats.Any(p => p.IdChannel == ThisUser && p.IdNotification == TextAndButton.Id))
					{
						if (picture == null)
						{
							try
							{
								await botClient.SendTextMessageAsync(ThisUser.IDChannel, TextAndButton.Text.Text,
								                                     replyMarkup: temp);
							}
							catch (Exception e)
							{

							}
						}
						else
						{
							try
							{
								await botClient.SendPhotoAsync(ThisUser.IDChannel, picture[0].Picture,
								                               TextAndButton.Text.Text, replyMarkup: temp);
							}
							catch (Exception e)
							{
							}
						}
					}
				}
				else
				{
					if (picture == null)
					{
						try
						{
							await botClient.SendTextMessageAsync(ThisUser.IDChannel, TextAndButton.Text.Text,
							                                     replyMarkup: temp);
						}
						catch (Exception e)
						{

						}
					}
					else
					{
						try
						{
							await botClient.SendPhotoAsync(ThisUser.IDChannel, picture[0].Picture,
							                               TextAndButton.Text.Text, replyMarkup: temp);
						}
						catch (Exception e)
						{
						}
					}
				}
			}

			var tempList = db.GetListCollectionButtonNotification();
			foreach (CollectionButtonNotification collectionButtonNotification in tempList)
			{
				db.Remove(collectionButtonNotification);
			}

			foreach (CollectionPictureNotification collectionPictureNotification in picture)
			{
				db.Remove(collectionPictureNotification);
			}

			foreach (NotificationChat notificationChat in notificationChats)
			{
				db.Remove(notificationChat);
			}
			
			var TextTemp = db.GetTextNotification(TextAndButton.Text);
			
			db.Remove(TextAndButton);
			db.Remove(TextTemp);
			db.Save();
			
			return true;
		}

		private async void SendMessageAll(TelegramBotClient botClient)
		{
			await SendNotificationAllUsers(botClient);
		}

		private void ChangeNotification()
		{
			TextAndButton.isWork = false;
			db.Save();
		}
		
		
		public void SendMessage(TelegramBotClient botClient)
		{
			InlineButton inlineButton = new InlineButton();

			if (CheckIsReady())
			{
				ChangeNotification();
				String Text = "🔔Рассылка отправлена🔔";

				SendMessageAll(botClient);
				
				botClient.EditMessage(user.ID, user.MessageID, Text, TextAndButton.Text.Text, user,
				                      inlineButton.SettingBotLvl2(user));
			}
			else
			{
				String Text = "🔔Рассылка не может быть отправлена, так как вы не заполнили все поля🔔";
				botClient.EditMessage(user.ID, user.MessageID, Text, "", user,
				                      inlineButton.NotificationBot());
			}
			
		}
	}
}