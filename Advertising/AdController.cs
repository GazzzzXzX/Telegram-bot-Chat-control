using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BotCore.SQL;

using Newtonsoft.Json;

using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Payments;

using File = System.IO.File;

namespace BotCore.Advertising
{
	internal static class AdController
	{
		public static PriceData PriceData { get; set; }

		static AdController()
		{
			if (File.Exists("pricelist.json"))
			{
				String json = File.ReadAllText("pricelist.json");

				PriceData = JsonConvert.DeserializeObject<PriceData>(json);
			}
			else
			{
				PriceData = new PriceData
				{
					postTypeDefault = 0.5f,
					postSymbolPrice = 0.0f,
					postImagePrice = 0.0f
				};

				SavePriceData();
			}
		}

		public static void SavePriceData()
		{
			if (PriceData == null)
				throw new Exception("Can't save price data. Price data is null.");

			using (FileStream fs = File.Create("pricelist.json"))
			{
				fs.Write(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(PriceData)));
			}
		}

		public static PostTemplate CreateTemplate(AdUser user)
		{
			DataBase db = Singleton.GetInstance().Context;
			PostTemplate template = new PostTemplate
			{
				AdUser = user,
				Name = "Шаблон"
			};
			db.AddTemplate(template);
			db.Save();

			return template;
		}

		/// <summary>
		/// Sets a content for message
		/// </summary>
		/// <param name="template">Template to set content</param>
		/// <param name="source">Source message where content stores</param>
		/// <param name="order">Order of sending, where caption always is 0</param>
		public static void SetContent(TelegramBotClient client, PostTemplate template, Message source, System.Int32 order)
		{
			DataBase db = Singleton.GetInstance().Context;
			PostContent content = null;

			if (template.PostContent != null)
			{
				content = template.PostContent.FirstOrDefault(postContent => postContent.Order == order);
			}

			if (content == null)
			{
				db.SetPostContent(new PostContent() { Order = order, PostTemplate = template, PostTemplateId = template.Id });
				db.Save();
				content = db.GetPostContent(template, order);
			}
			if (source.Text != null)
			{
				Task<Message> mes = client.SendTextMessageAsync(CommandText.bufferChannelId, source.Text);
				content.MessageId = mes.Result.MessageId;
			}
			else
			{
				Task<Message> mes = client.ForwardMessageAsync(CommandText.bufferChannelId, source.From.Id, source.MessageId);
				content.MessageId = mes.Result.MessageId;
			}

			db.Save();
		}

		public static void SetContent2(TelegramBotClient client, PostTemplate template, Message source, System.Int32 order)
		{
			DataBase db = Singleton.GetInstance().Context;
			PostContent content = null;

			if (template.PostContent != null)
			{
				content = template.PostContent.FirstOrDefault(postContent => postContent.Order == order);
			}

			if (content == null)
			{
				db.SetPostContent(new PostContent() { Order = order, PostTemplate = template, PostTemplateId = template.Id });
				db.Save();
				content = db.GetPostContent(template, order);
			}

			Task<Message> mes = client.ForwardMessageAsync(CommandText.bufferChannelId, source.From.Id, source.MessageId);
			content.MessageId = mes.Result.MessageId;

			db.Save();
		}

		public static PostContent GetContent(TelegramBotClient client, PostTemplate template, System.Int32 order)
		{
			DataBase db = Singleton.GetInstance().Context;

			if (template.PostContent == null)
			{
				return null;
			}

			PostContent content = db.GetPostContent(template, order);
			return content;
		}

		/// <summary>
		/// Removes content in specific position in order
		/// </summary>
		/// <param name="template">Template </param>
		/// <param name="order">Order position</param>
		public static void RemoveContentAt(TelegramBotClient client, PostTemplate template, System.Int32 order)
		{
			DataBase db = Singleton.GetInstance().Context;
			PostContent content = template.PostContent.FirstOrDefault(postContent => postContent.Order == order);

			template.PostContent.Remove(content);

			db.Save();
		}

		public static async Task<System.String> GetImage(TelegramBotClient client, PostContent content)
		{
			Message mes = await client.ForwardMessageAsync(CommandText.bufferChannelId, CommandText.bufferChannelId, content.MessageId);
			return mes.Photo[0].FileId;
		}

		/// <summary>
		/// Assemblies post content into ready to send files
		/// </summary>
		/// <param name="template">Template where content stores</param>
		/// <returns>Message if no any photos added or List<InputMediaBase> if there is 1 or more photos</returns>
		public static async Task<Object> AssemblyTemplate(TelegramBotClient client, PostTemplate template)
		{
			IOrderedEnumerable<PostContent> orderedList = template.PostContent.OrderBy(c => c.Order);

			Message textMessage = null;
			List<InputMediaBase> inputMedias = new List<InputMediaBase>();
			foreach (PostContent content in orderedList)
			{
				Message forward = await client.ForwardMessageAsync(CommandText.bufferChannelId, CommandText.bufferChannelId, content.MessageId);

				if (forward.Type != Telegram.Bot.Types.Enums.MessageType.Text)
				{
					InputMediaPhoto photo = new InputMediaPhoto(new InputMedia(forward.Photo[0].FileId));
					inputMedias.Add(photo);
				}
				else
				{
					textMessage = forward;
				}
			}

			if (inputMedias.Count != 0)
			{
				if (textMessage != null && !String.IsNullOrEmpty(textMessage.Text))
				{
					inputMedias[0].Caption = textMessage.Text;
				}

				return inputMedias;
			}
			return textMessage;
		}

		public static void DeleteAllPosts(TelegramBotClient client, PostTemplate template)
		{
			DataBase db = Singleton.GetInstance().Context;
			Post[] posts = db.GetPosts(template);

			foreach (Post post in posts)
			{
				client.DeleteMessageAsync(post.ChannelId, post.MessageId);

				db.Remove(post);
			}

			db.Save();
		}

		public static Single GetPostTypePrice(TypePostTime type)
		{
			switch (type)
			{
				case TypePostTime.StandartMessage:
					return PriceData.postTypeDefault;

				case TypePostTime.PinnedMessage:
					return PriceData.postTypePinnedPrice;

				case TypePostTime.PinnedMessageNotification:
					return PriceData.postTypePinnedNotificationPrice;
			}

			return -1;
		}

		public static String GetPostTypeName(TypePostTime type)
		{
			switch (type)
			{
				case TypePostTime.StandartMessage:
					return "Стандартное";

				case TypePostTime.PinnedMessage:
					return "Закрепленное";

				case TypePostTime.PinnedMessageNotification:
					return "Закрпленное с уведомлением";
			}

			return "Неизвестно";
		}

		public static String GetPriceString(PostTemplate template)
		{
			StringBuilder builder = new StringBuilder();
			DataBase db = Singleton.GetInstance().Context;

			Single totalPrice = 0.0f;

			builder.Append("Чек:\n");

			builder.Append($"Тип поста ({GetPostTypeName((TypePostTime)template.isPinnedMessage)}): {GetPostTypePrice((TypePostTime)template.isPinnedMessage)} грн\n");
			if (template.isPinnedMessage == 0)
			{
				builder.Append("Старндартное сообщение постится в указаное время!\n");
			}
			else
			{
				builder.Append("Сообщение с закрепом постится каждый день в 12:00, удалятся каждый день в 11:00!\n");
			}

			//totalPrice += GetPostTypePrice((TypePostTime)template.isPinnedMessage);

			foreach (PostChannel item in template.PostChannel)
			{
				if (item.Channel != null)
				{
					totalPrice += StartSession.NumberOfParticipants(item.Channel.InviteLink.Split("@")[1]) * (item.Channel.Price + GetPostTypePrice((TypePostTime)template.isPinnedMessage));
				}
				else
				{
					Channel channel = db.GetChannel(item.ChannelId);
					totalPrice += StartSession.NumberOfParticipants(channel.InviteLink.Split("@")[1]) * (item.Channel.Price + GetPostTypePrice((TypePostTime)template.isPinnedMessage));
				}
				builder.Append($"{item.Channel.ChannelName}: {item.Channel.Price} грн\n");
			}

			builder.Append($"\n");



			foreach (PostTime item in template.PostTime)
			{
				if (item.Time != System.DateTime.Today)
				{
					totalPrice += PriceData.postTypeTime;

					builder.Append($"\nПост на {item.Time.ToString("hh:mm:ss, dd.MM.yyyy")}: {PriceData.postTypeTime} грн");
				}
			}

			builder.Append($"\n\nВсего: {(Int32)totalPrice} грн");

			return builder.ToString();
		}

		public static LabeledPrice[] GetLabelPrices(PostTemplate template)
		{
			List<LabeledPrice> prices = new List<LabeledPrice>();
			DataBase db = Singleton.GetInstance().Context;

			Single totalPrice = 0.0f;

			//totalPrice += GetPostTypePrice((TypePostTime)template.isPinnedMessage);
			String temp = "";
			if (template.isPinnedMessage == 0)
			{
				temp = "Старндартное сообщение постится в указаное время!\n";
			}
			else
			{
				temp = "Сообщение с закрепом постится каждый день в 12:00, удалятся каждый день в 11:00!\n";
			}

			prices.Add(new LabeledPrice($"Тип поста: {GetPostTypeName((TypePostTime)template.isPinnedMessage)} " + temp, (Int32)GetPostTypePrice((TypePostTime)template.isPinnedMessage) * 100));


			foreach (PostChannel item in template.PostChannel)
			{
				if (item.Channel != null)
				{
					totalPrice = StartSession.NumberOfParticipants(item.Channel.InviteLink.Split("@")[1]) * (item.Channel.Price + GetPostTypePrice((TypePostTime)template.isPinnedMessage));
				}
				else
				{
					Channel channel = db.GetChannel(item.ChannelId);
					totalPrice = StartSession.NumberOfParticipants(channel.InviteLink.Split("@")[1]) * (item.Channel.Price + GetPostTypePrice((TypePostTime)template.isPinnedMessage));
				}
				if (totalPrice < 0)
				{
					totalPrice = 1;
				}
				prices.Add(new LabeledPrice($"Чат {item.Channel.ChannelName}", ((Int32)item.Channel.Price * 100) + (Convert.ToInt32(totalPrice) * 100)));
			}

			foreach (PostTime item in template.PostTime)
			{
				if (item.Time != System.DateTime.Today)
				{
					prices.Add(new LabeledPrice($"Пост на {item.Time.ToString("hh:mm:ss, dd.MM.yyyy:")} ", (Int32)PriceData.postTypeTime * 100));
				}
			}

			return prices.ToArray();
		}

		public static Single GetTotalPrice(PostTemplate template)
		{
			DataBase db = Singleton.GetInstance().Context;
			Single totalPrice = 0.0f;

			//totalPrice += GetPostTypePrice((TypePostTime)template.isPinnedMessage);

			foreach (PostChannel item in template.PostChannel)
			{
				if (item.Channel != null)
				{
					totalPrice += StartSession.NumberOfParticipants(item.Channel.InviteLink.Split("@")[1]) * (item.Channel.Price + GetPostTypePrice((TypePostTime)template.isPinnedMessage));
				}
				else
				{
					Channel channel = db.GetChannel(item.ChannelId);
					totalPrice += StartSession.NumberOfParticipants(channel.InviteLink.Split("@")[1]) * (item.Channel.Price + GetPostTypePrice((TypePostTime)template.isPinnedMessage));
				}
			}

			foreach (PostTime item in template.PostTime)
			{
				if (item.Time != System.DateTime.Today)
				{
					totalPrice += PriceData.postTypeTime;
				}
			}
			if ((totalPrice * 100) > 0)
			{
				return totalPrice * 100;
			}
			else
			{
				return 100;
			}
		}
	}
}