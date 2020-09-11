using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;

using BotCore.SQL;

using Microsoft.EntityFrameworkCore;

using Telegram.Bot.Types;

namespace BotCore
{
	internal static class Extentions
	{
		public static void SetValue<T>(this DataBase dataBase, T value) where T : class
		{
			lock (Singleton.BDLock)
			{
				dataBase.Add<T>(value);
			}
		}

		public static NotificationChat[] GetNotificationChats(this DataBase dataBase)
		{
			lock (Singleton.BDLock)
			{
				return dataBase._notificationChats.ToArray();
			}
		}
		
		public static ButtonAndTextNotication GetButtonAndTextNotication(this DataBase dataBase, User value)
		{
			lock (Singleton.BDLock)
			{
				return dataBase._ButtonAndTextNotications.FirstOrDefault(p => p.User == value && p.isWork == true);
			}
		}
		
		public static ButtonAndTextNotication GetButtonAndTextNotication(this DataBase dataBase, String value)
		{
			lock (Singleton.BDLock)
			{
				return dataBase._ButtonAndTextNotications.FirstOrDefault(p => p.Text.Text == value);
			}
		}

		public static List<CollectionButtonNotification> GetListCollectionButtonNotification(this DataBase dataBase, ButtonAndTextNotication delete)
		{
			lock (Singleton.BDLock)
			{
				return dataBase._CollectionButtonNotifications.Where(p => p.buttonAndTextNotification == delete).ToList();
			}
		}
		
		public static List<CollectionButtonNotification> GetListCollectionButtonNotification(this DataBase dataBase)
		{
			lock (Singleton.BDLock)
			{
				return dataBase._CollectionButtonNotifications.ToList();
			}
		}
		
		public static List<CollectionPictureNotification> GetListCollectionPictureNotification(this DataBase dataBase, ButtonAndTextNotication value)
		{
			lock (Singleton.BDLock)
			{
				return dataBase._CollectionPictureNotifications.Where(p => p.buttonAndTextNotification == value)
				               .ToList();
			}
		}

		public static ButtonNotification GetButtonNotification(this DataBase dataBase, ButtonNotification button)
		{
			lock (Singleton.BDLock)
			{
				return dataBase._ButtonNotifications.FirstOrDefault(p => p.Text == button.Text);
			}
		}
		
		public static TextNotification GetTextNotification(this DataBase dataBase, TextNotification text)
		{
			TextNotification temp;
			lock (Singleton.BDLock)
			{
				temp = dataBase._TextNotifications.FirstOrDefault(p => p.Text == text.Text);
			}

			return temp;
		}
		
		public static ChannelMessage[] GetChannelMessages(this DataBase dataBase)
		{
			lock (Singleton.BDLock)
			{
				return dataBase._channelMessages.ToArray();
			}
		}
		public static IncomeChannel[] GetIncomeChannels(this DataBase dataBase)
		{
			lock (Singleton.BDLock)
			{
				return dataBase._incomeChannels.ToArray();
			}
		}

		public static IncomeChannel GetIncomeChannels(this DataBase dataBase, Int64 id)
		{
			lock (Singleton.BDLock)
			{
				return dataBase._incomeChannels.FirstOrDefault(p => p.ChannelId == id && p.DateTime.Year == System.DateTime.Today.Year && p.DateTime.Month == System.DateTime.Today.Month);
			}
		}

		public static IncomeChannelAdmin[] GetIncomeChannelsAdmin(this DataBase dataBase)
		{
			lock (Singleton.BDLock)
			{
				return dataBase._incomeChannelAdmins.ToArray();
			}
		}

		public static IncomeChannelAdmin GetIncomeChannelsAdmin(this DataBase dataBase, Int32 idUser, Int64 IdChannel)
		{
			lock (Singleton.BDLock)
			{
				return dataBase._incomeChannelAdmins.FirstOrDefault(p => p.ChannelId == IdChannel && p.UserId == idUser && p.DateTime.Year == System.DateTime.Today.Year && p.DateTime.Month == System.DateTime.Today.Month);
			}
		}

		public static Income[] GetIncome(this DataBase dataBase)
		{
			lock (Singleton.BDLock)
			{
				return dataBase._incomes.ToArray();
			}
		}

		public static Income GetIncome(this DataBase dataBase, Int32 idUser, Int64 IdChannel)
		{
			lock (Singleton.BDLock)
			{
				return dataBase._incomes.FirstOrDefault(p => p.ChannelId == IdChannel && p.UserId == idUser && p.dateTime.Year == System.DateTime.Today.Year && p.dateTime.Month == System.DateTime.Today.Month);
			}
		}

		public static FeaturedUserNew GetFeaturedUsers(this DataBase dataBase, Int32 id)
		{
			lock (Singleton.BDLock)
			{
				return dataBase._featuredUserNews.FirstOrDefault(p => p.ID == id);
			}
		}

		public static FeaturedUserNew GetFeaturedUsers(this DataBase dataBase, User user, User userTwo)
		{
			lock (Singleton.BDLock)
			{
				return dataBase._featuredUserNews.FirstOrDefault(p => p.UserId == userTwo.ID && p.UserWhoAddedId == user.ID);
			}
		}

		public static FeaturedUserNew[] GetFeaturedUsers(this DataBase dataBase)
		{
			lock (Singleton.BDLock)
			{
				return dataBase._featuredUserNews.ToArray();
			}
		}

		public static AnaliticsTextAllChat GetAnaliticsAllChat(this DataBase dataBase, String text)
		{
			lock (Singleton.BDLock)
			{
				return dataBase._analiticsTextAllChats.FirstOrDefault(p => p.NameId == text);
			}
		}

		public static AnaliticsTextAllChat[] GetAnaliticsAllChat(this DataBase dataBase)
		{
			lock (Singleton.BDLock)
			{
				return dataBase._analiticsTextAllChats.ToArray();
			}
		}

		public static AnaliticsPhraseAllChat GetAnaliticsPhraseAllChat(this DataBase dataBase, String text)
		{
			lock (Singleton.BDLock)
			{
				return dataBase._analiticsPhraseAllChats.FirstOrDefault(p => p.NameId == text);
			}
		}

		public static AnaliticsPhraseMonth GetAnaliticsPhraseMonth(this DataBase dataBase, String text, System.DateTime dateTime, Channel channel)
		{
			lock (Singleton.BDLock)
			{
				return dataBase._analiticsPhraseMonths.FirstOrDefault(p => p.Phrase == text && p.DateTime == dateTime && p.channel.IDChannel == channel.IDChannel);
			}
		}

		public static AnaliticsPhraseMonth[] GetAnaliticsPhraseMonth(this DataBase dataBase)
		{
			lock (Singleton.BDLock)
			{
				return dataBase._analiticsPhraseMonths.ToArray();
			}
		}

		public static AnaliticsPhraseAllChat[] GetAnaliticsPhraseAllChats(this DataBase dataBase)
		{
			lock (Singleton.BDLock)
			{
				return dataBase._analiticsPhraseAllChats.ToArray();
			}
		}

		public static AnalyticsText GetAnalitics(this DataBase dataBase, Channel channelId, String word)
		{
			lock (Singleton.BDLock)
			{
				return dataBase._analyticsTexts.FirstOrDefault(p => p.ChannelId == channelId.IDChannel && p.AnaliticsTextAllChatId == word);
			}
		}

		public static AnalyticsText[] GetAnalitics(this DataBase dataBase)
		{
			lock (Singleton.BDLock)
			{
				return dataBase._analyticsTexts.ToArray();
			}
		}

		public static AnaliticsPhrase GetAnaliticsPharse(this DataBase dataBase, Channel channelId, String word)
		{
			lock (Singleton.BDLock)
			{
				return dataBase._analiticsPhrases.FirstOrDefault(p => p.ChannelId == channelId.IDChannel && p.AnaliticsPhraseAllChatId == word);
			}
		}

		public static AnaliticsPhrase[] GetAnaliticsPharse(this DataBase dataBase, String word)
		{
			lock (Singleton.BDLock)
			{
				return dataBase._analiticsPhrases.Include(p => p.channel).Where(p => p.AnaliticsPhraseAllChatId == word).ToArray();
			}
		}

		public static AnaliticsPhrase[] GetAnaliticsPharse(this DataBase dataBase)
		{
			lock (Singleton.BDLock)
			{
				return dataBase._analiticsPhrases.ToArray();
			}
		}

		public static AddresBTC GetAddresBTC(this DataBase dataBase, String privatKey)
		{
			lock (Singleton.BDLock)
			{
				return dataBase._addresBTCs.FirstOrDefault(p => p.PrivateKey == privatKey);
			}
		}

		public static AddresBTC GetAddresBTCInt(this DataBase dataBase, Int32? privatKey)
		{
			lock (Singleton.BDLock)
			{
				return dataBase._addresBTCs.FirstOrDefault(p => p.Id == privatKey);
			}
		}

		public static void AddFailedTransaction(this DataBase database, TransactionId tran)
		{
			lock (Singleton.BDLock)
			{
				database._transactionIds.Add(tran);
			}
		}

		public static void SetTransaction(this DataBase dataBase, Transaction transaction)
		{
			lock (Singleton.BDLock)
			{
				dataBase._transactions.Add(transaction);
			}
		}

		public static List<Transaction> GetTransactions(this DataBase dataBase)
		{
			List<Transaction> transactions = null;
			lock (Singleton.BDLock)
			{
				transactions = dataBase._transactions.ToList();
			}
			return transactions;
		}

		public static Transaction GetTransaction(this DataBase dataBase, Int32 IdTransaction)
		{
			Transaction transaction = null;
			lock (Singleton.BDLock)
			{
				transaction = dataBase._transactions.Include(p => p.UserRecipient).Include(p => p.UserSender).FirstOrDefault(p => p.Id == IdTransaction);
			}
			return transaction;
		}

		public static void DeleteTransaction(this DataBase dataBase, Transaction transaction)
		{
			lock (Singleton.BDLock)
			{
				dataBase._transactions.Remove(transaction);
			}
		}

		public static Transaction GetTransaction(this DataBase dataBase, User user)
		{
			Transaction transaction = null;
			lock (Singleton.BDLock)
			{
				transaction = dataBase._transactions.Include(p => p.UserRecipient).Include(p => p.UserSender).Where(p => p.UserSenderId == user.ID).FirstOrDefault(p => p.AddUser == true);
			}
			return transaction;
		}

		public static Transaction GetTransactionUserRecipient(this DataBase dataBase, User user)
		{
			Transaction transaction = null;
			lock (Singleton.BDLock)
			{
				transaction = dataBase._transactions.Include(p => p.UserRecipient).Include(p => p.UserSender).Where(p => p.UserRecipientId == user.ID).FirstOrDefault(p => p.AddUser == true);
			}
			return transaction;
		}

		public static Transaction GetTransactionUserSender(this DataBase dataBase, User user)
		{
			Transaction transaction = null;
			lock (Singleton.BDLock)
			{
				transaction = dataBase._transactions.Include(p => p.UserSender).Where(p => p.UserSenderId == user.ID).FirstOrDefault(p => p.AddUser == true);
			}
			return transaction;
		}

		public static void SetPostChannel(this DataBase dataBase, PostTemplate postTemplate, PostChannel post)
		{
			lock (Singleton.BDLock)
			{
				postTemplate.PostChannel.Add(post);
			}
		}

		public static Post GetPostInChannel(this DataBase dataBase, Int64 channelId, Int32 messageId)
		{
			lock (Singleton.BDLock)
			{
				return dataBase._posts.Where(p => p.ChannelId == channelId).FirstOrDefault(p => p.MessageId == messageId);
			}
		}

		public static PostTemplate GetPostTemplate(this DataBase database, Int32 id)
		{
			lock (Singleton.BDLock)
			{
				return database._postTemplates.FirstOrDefault(template => template.Id == id);
			}
		}

		public static Transaction GetTransactionByHash(this DataBase database, String hash)
		{
			lock (Singleton.BDLock)
			{
				return database._transactions.FirstOrDefault(t => t.IdTransaction == hash);
			}
		}

		public static TransactionId GetFailedTransactionByHash(this DataBase database, String hash)
		{
			lock (Singleton.BDLock)
			{
				return database._transactionIds.FirstOrDefault(t => t.NameHashOne == hash);
			}
		}

		public static Reviews[] GetReviews(this DataBase dataBase)
		{
			Reviews[] reviews = null;
			lock (Singleton.BDLock)
			{
				reviews = dataBase._reviews.ToArray();
			}
			return reviews;
		}

		public static Channel[] GetChannels(this DataBase dataBase)
		{
			Channel[] channels = null;
			lock (Singleton.BDLock)
			{
				channels = dataBase._channels.ToArray();
			}
			return channels;
		}

		public static List<Channel> GetChannelsList(this DataBase dataBase)
		{
			List<Channel> channels = null;
			lock (Singleton.BDLock)
			{
				channels = dataBase._channels.ToList();
			}
			return channels;
		}

		public static Category[] GetCategories(this DataBase dataBase)
		{
			Category[] category = null;
			lock (Singleton.BDLock)
			{
				category = dataBase._categories.ToArray();
			}
			return category;
		}

		public static Category GetCategory(this DataBase dataBase, String name)
		{
			Category category = null;
			lock (Singleton.BDLock)
			{
				category = dataBase._categories.FirstOrDefault(p => p.Name == name);
			}
			return category;
		}

		public static List<PostChannel> GetPostChannels(this DataBase dataBase)
		{
			List<PostChannel> channels = null;
			lock (Singleton.BDLock)
			{
				channels = dataBase._postChannel.ToList();
			}
			return channels;
		}

		public static PostTime getPostTime(this DataBase dataBase, AdUser user, DateTime date, PostTemplate postTemplate)
		{
			PostTime time = null;
			lock (Singleton.BDLock)
			{
				time = dataBase._postTemplates.Include(p => p.PostTime).FirstOrDefault(p => p.Id == postTemplate.Id && p.AdUserId == user.UserId).PostTime
				.Where(p => p.Time.Minute == date.Minute && p.Time.Hour == date.Hour && p.IsDate == false).FirstOrDefault();
			}
			return time;
		}

		public static PostTime GetPostTime(this DataBase dataBase, AdUser user, DateTime date, PostTemplate postTemplate, Int32 IdDateTime)
		{
			PostTime time = null;
			lock (Singleton.BDLock)
			{
				time = dataBase._postTemplates.Include(p => p.PostTime).FirstOrDefault(p => p.Id == postTemplate.Id && p.AdUserId == user.UserId).PostTime
				.Where(p => p.Time.Minute == date.Minute && p.Time.Hour == date.Hour && p.IdDateTime == IdDateTime).FirstOrDefault();
			}
			return time;
		}

		public static PostTime getPostDate(this DataBase dataBase, DateTime date, AdUser user, PostTemplate postTemplate)
		{
			PostTime time = null;
			lock (Singleton.BDLock)
			{
				time = dataBase._adUsers.Where(p => p.UserId == user.UserId).FirstOrDefault().PostTemplates.Where(p => p.Id == postTemplate.Id).FirstOrDefault()
					.PostTime.Where(p => p.Time.Date == date.Date).FirstOrDefault();
			}
			return time;
		}

		public static void AddPostTime(this DataBase dataBase, AdUser user, PostTime time)
		{
			lock (Singleton.BDLock)
			{
				dataBase._postTime.Add(time);
			}
		}

		public static void DeleteDatePostTime(this DataBase dataBase, AdUser user, PostTemplate template, DateTime time)
		{
			lock (Singleton.BDLock)
			{
				if (dataBase._adUsers.Where(p => p.UserId == user.UserId).FirstOrDefault().PostTemplates
					.Where(p => p.Id == template.Id).FirstOrDefault().PostTime.Count == 0)
				{
					dataBase._adUsers.Where(p => p.UserId == user.UserId).FirstOrDefault().PostTemplates
						.Where(p => p.Id == template.Id).FirstOrDefault().PostTime.Clear();
				}
				else
				{
					System.Int32 Count = dataBase._adUsers.Where(p => p.UserId == user.UserId).FirstOrDefault().PostTemplates
						.Where(p => p.Id == template.Id).FirstOrDefault().PostTime.
						Where(p => p.Time.Year == time.Year && p.Time.Day == time.Day && p.Time.Month == time.Month).Count();
					for (System.Int32 i = 0; i < Count; i++)
					{
						PostTime delete_time = dataBase._adUsers.Where(p => p.UserId == user.UserId).FirstOrDefault()
							.PostTemplates.Where(p => p.Id == template.Id).FirstOrDefault().PostTime.
							FirstOrDefault(p => p.Time.Year == time.Year && p.Time.Day == time.Day && p.Time.Month == time.Month);
						if (delete_time != null)
						{
							dataBase._adUsers.Where(p => p.UserId == user.UserId).FirstOrDefault().PostTemplates.Where(p => p.Id == template.Id).FirstOrDefault().PostTime.
								Remove(delete_time);
						}
					}
				}
			}
		}

		public static void DeleteTimePostTime(this DataBase dataBase, AdUser user, PostTemplate template, PostTime time)
		{
			lock (Singleton.BDLock)
			{
				if (dataBase._adUsers.Where(p => p.UserId == user.UserId).FirstOrDefault().PostTemplates
					.Where(p => p.Id == template.Id).FirstOrDefault().PostTime.Count == 0)
				{
					dataBase._adUsers.Where(p => p.UserId == user.UserId).FirstOrDefault().PostTemplates
						.Where(p => p.Id == template.Id).FirstOrDefault().PostTime.Clear();
				}
				else
				{
					System.Int32 Count = dataBase._adUsers.Where(p => p.UserId == user.UserId).FirstOrDefault().PostTemplates
						.Where(p => p.Id == template.Id).FirstOrDefault().PostTime.
					   Where(p => p.IdDateTime == time.IdDateTime ).Count();
					for (System.Int32 i = 0; i < Count; i++)
					{
						PostTime delete_time = dataBase._adUsers.Where(p => p.UserId == user.UserId).FirstOrDefault().PostTemplates
							.Where(p => p.Id == template.Id).FirstOrDefault().PostTime.
							FirstOrDefault(p => p.IdDateTime == time.IdDateTime);
						if (delete_time != null)
						{
							dataBase._adUsers.Where(p => p.UserId == user.UserId).FirstOrDefault().PostTemplates
								.Where(p => p.Id == template.Id).FirstOrDefault().PostTime.
								Remove(delete_time);
						}
					}
				}
			}
		}

		public static void DeleteReplayTimePostTime(this DataBase dataBase, AdUser user, PostTemplate template, PostTime time)
		{
			lock (Singleton.BDLock)
			{
				if (dataBase._adUsers.Where(p => p.UserId == user.UserId).FirstOrDefault().PostTemplates
					 .Where(p => p.Id == template.Id).FirstOrDefault().PostTime.Count == 0)
				{
					dataBase._adUsers.Where(p => p.UserId == user.UserId).FirstOrDefault().PostTemplates
						.Where(p => p.Id == template.Id).FirstOrDefault().PostTime.Clear();
				}
				else
				{
					System.Int32 Count = dataBase._adUsers.Where(p => p.UserId == user.UserId).FirstOrDefault().PostTemplates
						.Where(p => p.Id == template.Id).FirstOrDefault().PostTime.
					   Where(p => p.Time.Minute == time.Time.Minute && p.Time.Hour == time.Time.Hour && p.IdDateTime == -1).Count();
					for (System.Int32 i = 0; i < Count; i++)
					{
						PostTime delete_time = dataBase._adUsers.Where(p => p.UserId == user.UserId).FirstOrDefault().PostTemplates
							.Where(p => p.Id == template.Id).FirstOrDefault().PostTime.
							FirstOrDefault(p => p.Time.Minute == time.Time.Minute && p.Time.Hour == time.Time.Hour && p.IdDateTime == -1);
						if (delete_time != null)
						{
							dataBase._adUsers.Where(p => p.UserId == user.UserId).FirstOrDefault().PostTemplates
								.Where(p => p.Id == template.Id).FirstOrDefault().PostTime.
								Remove(delete_time);
						}
					}
				}
			}
		}

		public static void DeleteAllDatePostTime(this DataBase dataBase, AdUser user, PostTemplate template)
		{
			lock (Singleton.BDLock)
			{
				dataBase._adUsers.Where(p => p.UserId == user.UserId).FirstOrDefault().PostTemplates
					.Where(p => p.Id == template.Id).FirstOrDefault().PostTime.ToList().RemoveAll(p => p != null);
			}
		}

		public static void DeleteDatePostTime(this DataBase dataBase, AdUser user, PostTemplate template, PostTime postTime)
		{
			lock (Singleton.BDLock)
			{
				List<PostTime> times_delete = dataBase._adUsers.Where(p => p.UserId == user.UserId).FirstOrDefault().PostTemplates
					.Where(p => p.Id == template.Id).FirstOrDefault().PostTime.Where(p => p.Time.Date == postTime.Time.Date).ToList();

				for (System.Int32 i = 0; i < times_delete.Count; i++)
				{
					dataBase._adUsers.Where(p => p.UserId == user.UserId).FirstOrDefault().PostTemplates
					.Where(p => p.Id == template.Id).FirstOrDefault().PostTime.Remove(times_delete[i]);
				}

				for (System.Int32 i = 0; i < times_delete.Count; i++)
				{
					if (dataBase._adUsers.Where(p => p.UserId == user.UserId).FirstOrDefault().PostTemplates
				   .Where(p => p.Id == template.Id).FirstOrDefault().PostTime.Where(p => p.Time.Minute == times_delete[i].Time.Minute && p.Time.Hour == times_delete[i].Time.Hour &&
				   times_delete[i].UseTime == true).FirstOrDefault() != null)
					{
						dataBase._adUsers.Where(p => p.UserId == user.UserId).FirstOrDefault().PostTemplates
					.Where(p => p.Id == template.Id).FirstOrDefault().PostTime.Where(p => p.Time.Minute == times_delete[i].Time.Minute && p.Time.Hour == times_delete[i].Time.Hour &&
					times_delete[i].UseTime == true).FirstOrDefault().UseTime = true;
					}
				}
			}
		}

		public static void DeleteDatePinned(this DataBase dataBase, AdUser user, PostTemplate template, PostTime postTime)
		{
			lock (Singleton.BDLock)
			{
				dataBase._adUsers.Where(p => p.UserId == user.UserId).FirstOrDefault().PostTemplates
				   .Where(p => p.Id == template.Id).FirstOrDefault().PostTime.Remove
				   (
					dataBase._adUsers.Where(p => p.UserId == user.UserId).FirstOrDefault().PostTemplates
					.Where(p => p.Id == template.Id).FirstOrDefault().PostTime.Where(p => p.Time.Date == postTime.Time.Date).FirstOrDefault()
					);
			}
		}

		public static void ChangePostTimeForPostTemplate(this DataBase dataBase, AdUser user, PostTemplate template, DateTime time, DateTime old)
		{
			lock (Singleton.BDLock)
			{
				PostTime temp = dataBase._adUsers.Where(p => p.UserId == user.UserId).FirstOrDefault().PostTemplates
					.Where(p => p.Id == template.Id).FirstOrDefault().PostTime.ToList().Find(p => p.Time.Minute == old.Minute&&p.Time.Hour == old.Hour);
				temp.Time = time;
			}
		}

		public static void AddDatePostTimeForPostTemplate(this DataBase dataBase, PostTemplate template, AdUser user, PostTime time)
		{
			lock (Singleton.BDLock)
			{
				PostTime temp_time = time;
				List<PostTime> times_date = new List<PostTime>();
				List<PostTime> temps_date = dataBase._adUsers.Where(p => p.UserId == user.UserId).FirstOrDefault().PostTemplates.
				 Where(p => p.Id == template.Id).FirstOrDefault().PostTime.ToList();
				if (temps_date.Count == 0 || temps_date == null)
				{
					temp_time = new PostTime()
					{
						Time = new System.DateTime(time.Time.Year, time.Time.Month, time.Time.Day,
					   0, 0, 0),
						PostTemplateId = template.Id,
						UseTime = false,
						IsDate = true
					};
					dataBase._adUsers.Where(p => p.UserId == user.UserId).FirstOrDefault().PostTemplates
				   .Where(p => p.Id == template.Id).FirstOrDefault().PostTime.Add(temp_time);
					return;
				}
				if (times_date.Count == 0)
				{
					times_date.Add(temps_date[0]);
				}
				for (System.Int32 i = 0; i < temps_date.Count; i++)
				{
					if (times_date.All(p => p.Time.Minute != temps_date[i].Time.Minute || p.Time.Hour != temps_date[i].Time.Hour))
					{
						times_date.Add(temps_date[i]);
					}
				}
				for (System.Int32 i = 0; i < times_date.Count; i++)
				{
					temp_time = new PostTime()
					{
						Time = new System.DateTime(time.Time.Year, time.Time.Month, time.Time.Day,
					   times_date[i].Time.Hour, times_date[i].Time.Minute, 0),
						PostTemplateId = template.Id,
						UseTime = false,
						IsDate = true
					};
					dataBase._adUsers.Where(p => p.UserId == user.UserId).FirstOrDefault().PostTemplates
					.Where(p => p.Id == template.Id).FirstOrDefault().PostTime.Add(temp_time);
				}
			}
		}

		public static void AddDatePinnedForPostTemplate(this DataBase dataBase, PostTemplate template, AdUser user, PostTime time)
		{
			lock (Singleton.BDLock)
			{
				time.IsDate = false;
				//time.isPinnedMessage = true;
				time.Time = time.Time.AddHours(12);
				if (CheckIdPinnedDateTimeInPostTime(dataBase, user, template, time))
				{
					return;
				}
				dataBase._adUsers.Where(p => p.UserId == user.UserId).FirstOrDefault().PostTemplates.
				Where(p => p.Id == template.Id).FirstOrDefault().PostTime.Add(time);
			}
		}

		public static void AddPostTimeForPostTemplate(this DataBase dataBase, PostTemplate template, AdUser user, PostTime time)
		{
			lock (Singleton.BDLock)
			{
				PostTime temp_time = time;
				List<PostTime> times_date =new List<PostTime>();
				List<PostTime> temps_date = dataBase._adUsers.Where(p => p.UserId == user.UserId).FirstOrDefault().PostTemplates.
					Where(p => p.Id == template.Id).FirstOrDefault().PostTime.ToList();

				for (System.Int32 i = 0; i < temps_date.Count; i++)
				{
					if (times_date.Count == 0)
					{
						if (temps_date != null && temps_date.Count == 0)
						{
							times_date.Add(temps_date[0]);
						}
					}
					if (times_date.All(p => p.Time.Date != temps_date[i].Time.Date))
					{
						times_date.Add(temps_date[i]);
					}
				}

				for (System.Int32 i = 0; i < times_date.Count; i++)
				{
					temp_time = new PostTime()
					{
						Time = new System.DateTime(times_date[i].Time.Year, times_date[i].Time.Month, times_date[i].Time.Day,
						time.Time.Hour, time.Time.Minute, 0),
						PostTemplateId = template.Id,
						IdDateTime = -1
					};

					dataBase._adUsers.Where(p => p.UserId == user.UserId).FirstOrDefault().PostTemplates.
							Where(p => p.Id == template.Id).FirstOrDefault().PostTime.Add(temp_time);
				}
				dataBase._adUsers.Where(p => p.UserId == user.UserId).FirstOrDefault().PostTemplates.
							Where(p => p.Id == template.Id).FirstOrDefault().PostTime.Where(p => p.IdDateTime == -1).FirstOrDefault().UseTime = true;
			}
		}

		public static void UpdateIdDateTimeInPostTime(this DataBase dataBase, AdUser user, PostTemplate namePost)
		{
			lock (Singleton.BDLock)
			{
				List<PostTime> temps_date = dataBase._adUsers.Where(p => p.UserId == user.UserId).FirstOrDefault().PostTemplates.
				  Where(p => p.Id == namePost.Id).FirstOrDefault().PostTime.Where(p => p.IdDateTime == -1).ToList();
				if (temps_date.Count == 0)
				{
					return;
				}
				System.Int32 idDateTime = dataBase._postTime.OrderByDescending(u => u.ID).FirstOrDefault().ID + 1;
				for (Int32 i = 0; i < temps_date.Count; i++)
				{
					dataBase._adUsers.Where(p => p.UserId == user.UserId).FirstOrDefault().PostTemplates.
					  Where(p => p.Id == namePost.Id).FirstOrDefault().PostTime.Where(p => p == temps_date[i]).FirstOrDefault().IdDateTime = idDateTime;
				}
			}
		}

		public static Boolean CheckIdPinnedDateTimeInPostTime(this DataBase dataBase, AdUser user, PostTemplate namePost, PostTime time)
		{
			lock (Singleton.BDLock)
			{
				List<PostChannel> channel = new List<PostChannel>();
				if (dataBase._postTemplates.Include(p => p.PostChannel).FirstOrDefault(p => p.Id == namePost.Id && p.IsPaid == true) == null)
				{
					return false;
				}
				if (dataBase._postTemplates.Where(p => p.Id == namePost.Id && p.IsPaid == true).FirstOrDefault().PostChannel.Count == 0)
				{
					return false;
				}
				else
				{
					channel = dataBase._postTemplates.Where(p => p.Id == namePost.Id && p.IsPaid == true).FirstOrDefault().PostChannel.ToList();
				}
				for (Int32 i = 0; i < channel.Count; i++)
				{
					if (namePost.PostChannel.All(p => p.ChannelId != channel[i].ChannelId))
					{
						return false;
					}
				}
				if (dataBase._postTime.Count() == 0)
				{
					return false;
				}
				if (dataBase._postTime.Where(p => p.Time == time.Time && p.UseTime == true && p.isWorked == false).Count() < 2)
				{
					return false;
				}

				return true;
			}
		}

		public static void CheckIdDateTimeInPostTime(this DataBase dataBase, AdUser user, PostTemplate namePost, PostTime time)
		{
			lock (Singleton.BDLock)
			{
				List<PostChannel> channel = new List<PostChannel>();
				List<PostTime> times = new List<PostTime>();
				if (dataBase._postTemplates.Include(p => p.PostChannel).FirstOrDefault(p => p.Id == namePost.Id) == null)
				{
					return;
				}
				if (dataBase._postTime.Count() == 0)
				{
					return;
				}
				times = dataBase.GetPostTimesCollection(user, namePost).Where(p => p.UseTime == true && p.ID != time.ID).ToList();
				if (times.Count == 0)
				{
					return;
				}
				if (times.All(p => p.Time.Hour != time.Time.AddHours(1).Hour && p.Time.Hour != time.Time.AddHours(-1).Hour && p.Time.Hour != time.Time.Hour))
				{
					return;
				}
				List<DateTime> freeDates = new List<DateTime>();
				DateTime tempDate = new DateTime();
				tempDate = tempDate.AddDays(10);
				for (Int32 i = 0; i < 23; i++)
				{
					if (times.All(p => p.Time.Hour != tempDate.AddHours(1).Hour && p.Time.Hour != tempDate.AddHours(-1).Hour && p.Time.Hour != tempDate.Hour))
					{
						freeDates.Add(tempDate);
					}
					tempDate = tempDate.AddHours(1);
				}
				if (freeDates.Count == 0)
				{
					return;
				}
				List<PostTime> find_times =  dataBase._adUsers.Where(p => p.UserId == user.UserId).FirstOrDefault().PostTemplates.
					  Where(p => p.Id == namePost.Id).FirstOrDefault().PostTime.Where(p => p.IdDateTime == time.IdDateTime).ToList();
				tempDate = freeDates[freeDates.IndexOf(freeDates.OrderBy(d => Math.Abs(d.Hour - time.Time.Hour)).ElementAt(0))];
				DateTime search_time = new DateTime(time.Time.Year,time.Time.Month,time.Time.Day,tempDate.Hour,tempDate.Minute,tempDate.Second);

				for (Int32 i = 0; i < find_times.Count; i++)
				{

					dataBase._adUsers.Where(p => p.UserId == user.UserId).FirstOrDefault().PostTemplates.
					 Where(p => p.Id == namePost.Id).FirstOrDefault().PostTime.Where(p => p.ID == find_times[i].ID).FirstOrDefault().Time
					 =
					search_time;
				}
			}
		}

		public static List<PostTime> GetPostTimesCollection(this DataBase dataBase, AdUser user, PostTemplate namePost)
		{
			List<PostTime> post =null;
			lock (Singleton.BDLock)
			{
				post = namePost.PostTime.ToList();
			}
			return post;
		}

		public static List<User> GetCalendarUsers(this DataBase dataBase)
		{
			Dictionary<System.Int32, Calendar.Calendar> users_calendar =
				new Dictionary<System.Int32, Calendar.Calendar>();
			lock (Singleton.BDLock)
			{
				if (dataBase._users.Any())
				{
					return dataBase._users.ToList();

				}
			}

			return null;
		}

		//dataBase._users
		public static User GetUser(this DataBase dataBase, Int32 id)
		{
			User user;
			lock (Singleton.BDLock)
			{
				user = dataBase._users.FirstOrDefault(p => p.ID == id);
			}
			return user;
		}

		public static User GetUser(this DataBase dataBase, String id)
		{
			User user;
			lock (Singleton.BDLock)
			{
				user = dataBase._users.FirstOrDefault(p => p.Username == id);
			}
			return user;
		}

		public static User GetUser(this DataBase dataBase, Int64 id)
		{
			User user;
			lock (Singleton.BDLock)
			{
				user = dataBase._users.FirstOrDefault(p => p.ID == id);
			}

			return user;
		}

		public static User[] GetUsers(this DataBase dataBase)
		{
			User[] user;
			lock (Singleton.BDLock)
			{
				user = dataBase._users.ToArray();
			}
			return user;
		}

		public static UserMessage[] GetUserMessages(this DataBase dataBase)
		{
			UserMessage[] userMessages;
			lock (Singleton.BDLock)
			{
				userMessages = dataBase._userMessages.ToArray();
			}
			return userMessages;
		}

		public static UserMessage GetUserMessage(this DataBase dataBase, System.String message, User user)
		{
			UserMessage userMessage;
			lock (Singleton.BDLock)
			{
				userMessage = dataBase._userMessages.FirstOrDefault(p => p.SendMessage == message && p.UserID == user.ID);
			}
			return userMessage;
		}

		public static AdUser GetAdUser(this DataBase dataBase, System.Int32 id)
		{
			AdUser adUser;
			lock (Singleton.BDLock)
			{
				adUser = dataBase._adUsers.FirstOrDefault(p => p.User.ID == id);
			}
			return adUser;
		}

		public static Channel GetChannel(this DataBase dataBase, Int64 id)
		{
			Channel channel;
			lock (Singleton.BDLock)
			{
				channel = dataBase._channels.FirstOrDefault(p => p.IDChannel == id);
			}
			return channel;
		}

		public static Channel GetChannel(this DataBase dataBase)
		{
			Channel channel;
			lock (Singleton.BDLock)
			{
				channel = dataBase._channels.FirstOrDefault(p => p.isPostCount == true);
			}
			return channel;
		}

		public static Channel GetChannel(this DataBase dataBase, System.String link)
		{
			Channel channel;
			lock (Singleton.BDLock)
			{
				channel = dataBase._channels.FirstOrDefault(p => p.InviteLink == link);
			}
			return channel;
		}

		public static TMessage GetTMessage_ForChannel(this DataBase dataBase, Channel channels)
		{
			TMessage mes;
			lock (Singleton.BDLock)
			{
				mes = dataBase._tmessage.FirstOrDefault(p => p.channel == channels);
			}
			return mes;
		}

		public static TMessage[] GetTMessages(this DataBase dataBase)
		{
			TMessage[] messages;
			lock (Singleton.BDLock)
			{
				messages = dataBase._tmessage.ToArray();
			}
			return messages;
		}

		public static void Add_TMessage(this DataBase dataBase, TMessage message)
		{
			lock (Singleton.BDLock)
			{
				dataBase.Add(message);
				dataBase.SaveChanges();
			}
		}

		public static void Add_TempBase(this DataBase dataBase, TempBase message)
		{
			lock (Singleton.BDLock)
			{
				dataBase._tempBase.Add(message);
				dataBase.SaveChanges();
			}
		}

		public static void Add_Settings(this DataBase dataBase, Settings ch)
		{
			lock (Singleton.BDLock)
			{
				dataBase._settings.Add(ch);
				dataBase.SaveChanges();
			}
		}

		public static void Add_Reviews(this DataBase dataBase, Reviews ch)
		{
			lock (Singleton.BDLock)
			{
				dataBase._reviews.Add(ch);
				dataBase.SaveChanges();
			}
		}

		public static void Add_Complaint(this DataBase dataBase, Complaint ch)
		{
			lock (Singleton.BDLock)
			{
				dataBase._complaints.Add(ch);
				dataBase.SaveChanges();
			}
		}

		public static void Add_Ap(this DataBase dataBase, Appeal ch)
		{
			lock (Singleton.BDLock)
			{
				dataBase._appeals.Add(ch);
				dataBase.SaveChanges();
			}
		}

		public static void Add_Words(this DataBase dataBase, Word ch)
		{
			lock (Singleton.BDLock)
			{
				dataBase._words.Add(ch);
				dataBase.SaveChanges();
			}
		}

		public static void Add_Channel(this DataBase dataBase, Channel ch)
		{
			lock (Singleton.BDLock)
			{
				dataBase._channels.Add(ch);
				dataBase.SaveChanges();
			}
		}

		public static User GetUserForNumber(this DataBase dataBase, System.String number)
		{
			User mes;
			lock (Singleton.BDLock)
			{
				mes = dataBase._users.FirstOrDefault(p => p.Number == number);
				//dataBase.SaveChanges();
			}
			return mes;
		}

		public static void RemoveWord(this DataBase dataBase, Word tb)
		{
			lock (Singleton.BDLock)
			{
				dataBase._words.Remove(tb);
				dataBase.SaveChanges();
			}
		}

		public static Word GetWords(this DataBase dataBase, System.String word)
		{
			Word mes;
			lock (Singleton.BDLock)
			{
				mes = dataBase._words.FirstOrDefault(p => p.word == word);
			}
			return mes;
		}

		public static TempBase GetTempBase(this DataBase dataBase, Int64 id)
		{
			TempBase mes;
			lock (Singleton.BDLock)
			{
				mes = dataBase._tempBase.FirstOrDefault(p => p.ID == id);
			}
			return mes;
		}

		public static Reviews GetReviews(this DataBase dataBase, Int64 id)
		{
			Reviews mes;
			lock (Singleton.BDLock)
			{
				mes = dataBase._reviews.Where(p => p.IDSender == id).FirstOrDefault(p => p.TheEnd == false);
			}
			return mes;
		}

		public static User GetUser(this DataBase dataBase, Reviews reviews)
		{
			User user;
			lock (Singleton.BDLock)
			{
				user = dataBase._users.FirstOrDefault(p => p.ID == reviews.IDRecipient);
			}
			return user;
		}

		/// <summary>
		/// Сохранение!
		/// </summary>
		/// <param name="dataBase"></param>
		public static void Save(this DataBase dataBase)
		{
			lock (Singleton.BDLock)
			{
				dataBase.SaveChanges();
			}
		}

		/// <summary>
		/// Добавление нового пользователя!
		/// </summary>
		/// <param name="dataBase"></param>
		/// <param name="_message"></param>
		/// <param name="mes"></param>
		public static void AddNewUser(this DataBase dataBase, CallbackQuery _message)
		{
			lock (Singleton.BDLock)
			{
				dataBase._users.Add(new User()
				{
					ID = _message.From.Id,
					Username = _message.From.Username != null ? _message.From.Username : "Нет!",
					Number = "0",
					AddMembers = 0,
					Chain = 1,
					BanDate = System.DateTime.Now,
					DateRegister = System.DateTime.Today,
					CountReviews = 0
				});
			}
		}


		public static void AddAdUser(this DataBase dataBase, User user)
		{
			lock (Singleton.BDLock)
			{
				dataBase._adUsers.Add(new AdUser() { Balance = 0, User = user, EditingPostTemplateId = -1 });
			}
		}

		/// <summary>
		/// Возвращение настроек!
		/// </summary>
		/// <param name="dataBase"></param>
		/// <returns></returns>
		public static Settings GetSettings(this DataBase dataBase)
		{
			Settings settings;
			lock (Singleton.BDLock)
			{
				settings = dataBase._settings.FirstOrDefault();
			}
			return settings;
		}

		/// <summary>
		/// Вернуть настройки!
		/// </summary>
		/// <param name="dataBase"></param>
		/// <param name="_message"></param>
		/// <returns></returns>
		public static Reviews GetReviews(this DataBase dataBase, CallbackQuery _message)
		{
			Reviews reviews;
			lock (Singleton.BDLock)
			{
				reviews = dataBase._reviews.Where(p => p.IDSender == _message.From.Id).FirstOrDefault(p => p.TheEnd == false);
			}
			return reviews;
		}

		/// <summary>
		/// Возврат TempBase
		/// </summary>
		/// <param name="dataBase"></param>
		/// <param name="user"></param>
		/// <returns></returns>
		public static TempBase GetTempBase(this DataBase dataBase, User user)
		{
			TempBase tempBase;
			lock (Singleton.BDLock)
			{
				tempBase = dataBase._tempBase.FirstOrDefault(p => p.ID == user.ID);
			}
			return tempBase;
		}

		/// <summary>
		/// Удаление строки в бд
		/// </summary>
		/// <param name="dataBase"></param>
		/// <param name="tempBase"></param>
		public static void RemoveTempBase(this DataBase dataBase, TempBase tempBase)
		{
			lock (Singleton.BDLock)
			{
				dataBase._tempBase.Remove(tempBase);
			}
		}

		/// <summary>
		/// Удаление строки в отзывах!
		/// </summary>
		/// <param name="dataBase"></param>
		/// <param name="reviews"></param>
		public static void RemoveReviews(this DataBase dataBase, Reviews reviews)
		{
			lock (Singleton.BDLock)
			{
				dataBase._reviews.Remove(reviews);
			}
		}

		public static Reviews GetReviews(this DataBase dataBase, System.Int32 id)
		{
			Reviews reviews;
			lock (Singleton.BDLock)
			{
				reviews = dataBase._reviews.Where(p => p.ID == id).FirstOrDefault();
			}
			return reviews;
		}

		public static Reviews GetReviewsIDSender(this DataBase dataBase, User user)
		{
			Reviews reviews;
			lock (Singleton.BDLock)
			{
				reviews = dataBase._reviews.Where(p => p.IDSender == user.ID).FirstOrDefault(p => p.TheEnd == false);
			}
			return reviews;
		}

		/// <summary>
		/// Добавить рейтинг!
		/// </summary>
		/// <param name="dataBase"></param>
		/// <param name="user"></param>
		/// <param name="star"></param>
		public static void SetStar(this DataBase dataBase, User user, Int32 star)
		{
			lock (Singleton.BDLock)
			{
				dataBase._ratings.Add(new Rating() { IDUser = user.ID, Star = star });
			}
		}

		public static Double SetSumReting(this DataBase dataBase, User user)
		{
			Double temp;
			lock (Singleton.BDLock)
			{
				temp = dataBase._ratings.Where(p => p.IDUser == user.ID).Average(p => p.Star);
			}
			return temp;
		}

		public static void SetTempDate(this DataBase dataBase, User Admin, User user)
		{
			lock (Singleton.BDLock)
			{
				dataBase._tempBase.Add(new TempBase() { ID = Admin.ID, IDTwo = user.ID });
			}
		}

		public static Channel GetChannel(this DataBase dataBase, Message _message)
		{
			Channel channel;
			lock (Singleton.BDLock)
			{
				channel = dataBase._channels.FirstOrDefault(p => p.IDChannel == _message.Chat.Id);
			}
			return channel;
		}

		public static void SetChannel(this DataBase dataBase, Chat _message)
		{
			lock (Singleton.BDLock)
			{
				if (dataBase.GetChannel(_message.Id) == null)
				{
					dataBase._channels.Add(new Channel() { IDChannel = _message.Id, ChannelName = _message.Title, Description = _message.Description, LinkChannel = "t.me/" + _message.Username, InviteLink = "@" + _message.Username, PhotoLink = "https://bipbap.ru/wp-content/uploads/2017/10/0_8eb56_842bba74_XL-640x400.jpg" });
				}
			}
		}

		public static TMessage GetMessage(this DataBase dataBase, Message _message)
		{
			TMessage tmessage;
			lock (Singleton.BDLock)
			{
				tmessage = dataBase._tmessage.Where(p => p.user.ID == _message.From.Id).FirstOrDefault(p => p.channel.IDChannel == _message.Chat.Id);
			}
			return tmessage;
		}

		/// <summary>
		/// Удалить все с photoData
		/// </summary>
		/// <param name="dataBase"></param>
		public static void RemoveRangePhotoData(this DataBase dataBase)
		{
			lock (Singleton.BDLock)
			{
				dataBase._photoData.RemoveRange(dataBase._photoData);
			}
		}

		public static void AddTemplate(this DataBase dataBase, PostTemplate template)
		{
			lock (Singleton.BDLock)
			{
				dataBase._postTemplates.Add(template);
			}
		}

		public static PostTemplate GetTempalte(this DataBase dataBase, Int32 id, Int32 postID)
		{
			PostTemplate result = null;
			//ICollection<PostChannel> postChannel = null;

			lock (Singleton.BDLock)
			{
				result = dataBase._postTemplates.Include(p => p.PostContent).Include(p => p.AdUser).Include(p => p.PostChannel).FirstOrDefault(p => p.AdUser.User.ID == id && p.Id == postID);
				//postChannel = dataBase._postChannel.Include(p => p.Channel).Where(p => p.PostTemplateId == result.Id).ToArray();
				//result.PostChannel = postChannel;
			}

			return result;
		}

		public static PostTemplate GetTempalteOne(this DataBase dataBase, Int32 id, Int32 postID)
		{
			PostTemplate result = null;

			lock (Singleton.BDLock)
			{
				result = dataBase._postTemplates.Include(p => p.PostContent).Include(p => p.AdUser).Include(p => p.PostChannel).FirstOrDefault(p => p.AdUser.User.ID == id && p.Id == postID);
			}

			return result;
		}

		public static void DeleteAllPostTimeOfPostTemplate(this DataBase dataBase, AdUser user, PostTemplate postTemplate)
		{
			lock (Singleton.BDLock)
			{
				dataBase._adUsers.FirstOrDefault(p => p.UserId == user.UserId)
				        ?.PostTemplates.
				        Where(p => p.Id == postTemplate.Id).FirstOrDefault()
				        ?.PostTime.Clear();
			}
		}

		public static void ChangeTempalte(this DataBase dataBase, Int32 id, Int32 postID, Int32 message)
		{
			lock (Singleton.BDLock)
			{
				dataBase._postTemplates.Include(p => p.PostContent).Include(p => p.AdUser).FirstOrDefault(p => p.AdUser.User.ID == id && p.Id == postID).isPinnedMessage = message;
			}
		}

		public static void RemoveTemplate(this DataBase dataBase, Int32 id)
		{
			PostTemplate result = null;

			lock (Singleton.BDLock)
			{
				result = dataBase._postTemplates.FirstOrDefault(template => template.Id == id);

				dataBase._postTemplates.Remove(result);
			}
		}

		public static Post[] GetPosts(this DataBase dataBase, PostTemplate template)
		{
			lock (Singleton.BDLock)
			{
				Post[] posts;
				{
					posts = dataBase._posts.Where(post => post.Template.Id == template.Id).ToArray();
				}
				return posts;
			}
		}

		public static void SetPhoto(this DataBase dataBase, PhotoDate photoDate)
		{
			lock (Singleton.BDLock)
			{
				dataBase._photoData.Add(photoDate);
			}
		}

		public static void RemoveRangePhotoList(this DataBase dataBase, List<PhotoDate> photo)
		{
			lock (Singleton.BDLock)
			{
				dataBase._photoData.RemoveRange(photo);
			}
		}

		public static void SetInvitedUser(this DataBase dataBase, Message message)
		{
			foreach (Telegram.Bot.Types.User mes in message.NewChatMembers)
			{
				lock (Singleton.BDLock)
				{
					dataBase._invitedUsers.Add(new InvitedUser()
					{
						UserAddedId = mes.Id, UserWhoAddedId = message.From.Id, ChannelId = message.Chat.Id
					});
				}
			}
		}

		public static void SetInvitedUser(this DataBase dataBase, System.Int32 Added, System.Int32 WhoAdded, System.Int64 ChannelId)
		{
			lock (Singleton.BDLock)
			{
				dataBase._invitedUsers.Add(new InvitedUser() { UserAddedId = Added, UserWhoAddedId = WhoAdded, ChannelId = ChannelId});
			}
		}

		public static InvitedUser[] GetInvitedUsers(this DataBase dataBase)
		{
			InvitedUser[] invitedUser;
			lock (Singleton.BDLock)
			{
				invitedUser = dataBase._invitedUsers.ToArray();
			}
			return invitedUser;
		}

		public static void SetPostContent(this DataBase dataBase, PostContent postContent)
		{
			lock (Singleton.BDLock)
			{
				dataBase._postContent.Add(postContent);
			}
		}

		public static PostContent GetPostContent(this DataBase dataBase, PostTemplate postTemplate, System.Int32 order)
		{
			PostContent postContent;
			lock (Singleton.BDLock)
			{
				postContent = dataBase._postContent.FirstOrDefault(p => p.PostTemplateId == postTemplate.Id && p.Order == order);
			}
			return postContent;
		}

		public static System.Boolean GetPostContentCount(this DataBase dataBase, PostTemplate postTemplate)
		{
			System.Boolean count = false;
			lock (Singleton.BDLock)
			{
				count = dataBase._postContent.Any(p => p.PostTemplateId == postTemplate.Id) ? true : false;
			}
			return count;
		}

		public static PostTemplate[] GetPostTemplates(this DataBase dataBase)
		{
			PostTemplate[] postTemplates;
			lock (Singleton.BDLock)
			{
				postTemplates = dataBase._postTemplates.Include(c => c.AdUser).Include(c => c.PostChannel).Include(c => c.PostContent).Include(c => c.PostTime).ToArray();
			}
			return postTemplates;
		}

		public static List<PostTemplate> GetPostTemplatesList(this DataBase dataBase)
		{
			List<PostTemplate> postTemplates;
			lock (Singleton.BDLock)
			{
				postTemplates = dataBase._postTemplates.Include(c => c.AdUser).Include(c => c.PostChannel).Include(c => c.PostContent).Include(c => c.PostTime).ToList();
			}
			return postTemplates;
		}

		public static List<PostTime> GetPostTimesToDate(this DataBase dataBase, AdUser user, PostTemplate postTemplate)
		{
			List<PostTime> postTime;
			lock (Singleton.BDLock)
			{
				postTime = dataBase._postTemplates.Include(p => p.PostTime).FirstOrDefault(p => p.Id == postTemplate.Id && p.AdUserId == user.UserId).PostTime
			   .ToList();
			}
			return postTime;
		}

		public static PostTime GetPostTime(this DataBase dataBase, AdUser user, PostTemplate postTemplate, Int32 IdTime)
		{
			PostTime postTime;
			lock (Singleton.BDLock)
			{
				postTime = dataBase._postTemplates.Include(p => p.PostTime).FirstOrDefault(p => p.Id == postTemplate.Id && p.AdUserId == user.UserId).PostTime
			   .Where(p => p.ID == IdTime).FirstOrDefault();
			}
			return postTime;
		}

		public static List<PostTime> GetPinnedToDate(this DataBase dataBase, AdUser user, PostTemplate postTemplate)
		{
			List<PostTime> postTime;
			lock (Singleton.BDLock)
			{
				postTime = dataBase._postTemplates.Include(p => p.PostTime).FirstOrDefault(p => p.Id == postTemplate.Id && p.AdUserId == user.UserId)
				                   ?.PostTime
			   .ToList();
			}
			return postTime;
		}

		public static List<PostTime> GetPostTimesToDatePinnedMessage(this DataBase dataBase, AdUser user, PostTemplate postTemplate)
		{
			List<PostTime> postTime;
			lock (Singleton.BDLock)
			{
				postTime = dataBase._postTemplates.Include(p => p.PostTime).FirstOrDefault(p => p.Id == postTemplate.Id && p.AdUserId == user.UserId)
				                   ?.PostTime
			   .ToList();
			}
			return postTime;
		}

		public static void DeletePostTemplate(this DataBase dataBase, AdUser user, PostTemplate postTemplate)
		{
			lock (Singleton.BDLock)
			{
				dataBase._adUsers.FirstOrDefault(p => p.UserId == user.UserId)?.PostTemplates.Remove(postTemplate);
			}
		}

		public static void AddHoursPostTime(this DataBase dataBase, PostTime posttime, AdUser user, PostTemplate postTemplate)
		{
			lock (Singleton.BDLock)
			{
				List<PostTime> times = (dataBase._adUsers.FirstOrDefault(p => p.UserId == user.UserId)
				                                ?.PostTemplates).FirstOrDefault(p => p.Id == postTemplate.Id)
				                                                ?.PostTime.Where(p => p.IdDateTime == posttime.IdDateTime).ToList();
				for (System.Int32 i = 0; i < times.Count; i++)
				{
					if (posttime.Time.Hour == 23)
					{
						dataBase._postTime.FirstOrDefault(p => p.ID == times[i].ID).Time =
							dataBase._postTime.FirstOrDefault(p => p.ID == times[i].ID).Time.AddDays(-1);
					}
					dataBase._postTime.FirstOrDefault(p => p.ID == times[i].ID).Time =
						dataBase._postTime.FirstOrDefault(p => p.ID == times[i].ID).Time.AddHours(1);
				}
			}
		}

		public static void SubHoursPostTime(this DataBase dataBase, PostTime posttime, AdUser user, PostTemplate postTemplate)
		{
			lock (Singleton.BDLock)
			{
				List<PostTime> times = (dataBase._adUsers.FirstOrDefault(p => p.UserId == user.UserId)
				                                ?.PostTemplates).FirstOrDefault(p => p.Id == postTemplate.Id)
				                                                ?.PostTime.Where(p => p.IdDateTime == posttime.IdDateTime).ToList();
				for (System.Int32 i = 0; i < times.Count; i++)
				{
					if (posttime.Time.Hour == 0)
					{
						dataBase._postTime.FirstOrDefault(p => p.ID == times[i].ID).Time =
							dataBase._postTime.FirstOrDefault(p => p.ID == times[i].ID).Time.AddDays(-1);
					}
					dataBase._postTime.FirstOrDefault(p => p.ID == times[i].ID).Time =
						dataBase._postTime.FirstOrDefault(p => p.ID == times[i].ID).Time.AddHours(-1);
				}
			}
		}

		public static void SubMinutePostTime(this DataBase dataBase, PostTime posttime, AdUser user, PostTemplate postTemplate)
		{
			lock (Singleton.BDLock)
			{
				List<PostTime> times = (dataBase._adUsers.FirstOrDefault(p => p.UserId == user.UserId)
				                                ?.PostTemplates).FirstOrDefault(p => p.Id == postTemplate.Id)
				                                                ?.PostTime.Where(p => p.IdDateTime == posttime.IdDateTime).ToList();

				for (System.Int32 i = 0; i < times.Count; i++)
				{
					if (posttime.Time.Minute == 0 && posttime.Time.Hour == 0)
					{
						dataBase._postTime.FirstOrDefault(p => p.ID == times[i].ID).Time =
							dataBase._postTime.FirstOrDefault(p => p.ID == times[i].ID).Time.AddDays(-1);
					}
					dataBase._postTime.FirstOrDefault(p => p.ID == times[i].ID).Time =
						dataBase._postTime.FirstOrDefault(p => p.ID == times[i].ID).Time.AddMinutes(-5);
				}
			}
		}

		public static void AddMinutePostTime(this DataBase dataBase, PostTime posttime, AdUser user, PostTemplate postTemplate)
		{
			lock (Singleton.BDLock)
			{
				List<PostTime> times = (dataBase._adUsers.FirstOrDefault(p => p.UserId == user.UserId)
				                                ?.PostTemplates).FirstOrDefault(p => p.Id == postTemplate.Id)
				                                                ?.PostTime.Where(p => p.IdDateTime == posttime.IdDateTime).ToList();
				for (System.Int32 i = 0; i < times.Count; i++)
				{
					if (posttime.Time.Minute == 55 && posttime.Time.Hour == 23)
					{
						dataBase._postTime.FirstOrDefault(p => p.ID == times[i].ID).Time =
							dataBase._postTime.FirstOrDefault(p => p.ID == times[i].ID).Time.AddDays(-1);
					}
					dataBase._postTime.FirstOrDefault(p => p.ID == times[i].ID).Time =
						dataBase._postTime.FirstOrDefault(p => p.ID == times[i].ID).Time.AddMinutes(5);
				}
			}
		}

		public static void DeletePostTime(this DataBase dataBase, PostTime postTime)
		{
			lock (Singleton.BDLock)
			{
				dataBase._postTime.Remove(postTime);
			}
		}
	}
}