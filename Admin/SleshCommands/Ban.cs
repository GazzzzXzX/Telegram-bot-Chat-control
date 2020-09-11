using BotCore.Advertising;
using BotCore.SQL;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCore.SleshCommands
{
	internal class Ban : AbsCommand
	{
		public override System.String Name { get; set; } = "/ban";

		public override void Execute(TelegramBotClient botClient, System.Object message)
		{
			DataBase db = Singleton.GetInstance().Context;
			Message _Message = message as Message;

			System.Int32 FromId = _Message.ReplyToMessage.From.Id;
			System.Int32 ban = 10;
			System.Int32.TryParse(_Message.Text.ToLower().Replace(Name, "").Replace(" ", "").Replace(".", ","), out ban);
			Settings settings = db.GetSettings();

			botClient.DeleteMessage(_Message.Chat.Id, _Message.MessageId, "20");

			User Admin = db.GetUser(_Message.From.Id);
			User user = db.GetUser(_Message.ReplyToMessage.From.Id);

			try
			{
				if (Admin.IsAdmin > 0)
				{
					if (user.IsAdmin != 2 && user.IsAdmin != 3)
					{
						user.BanDate = System.DateTime.Now.AddDays(ban);
						user.BanDescript = "Вы были забанены администратором группы!";
						db.Save();

						System.String text = "Пользователь " + user.FIO + "\nID: " + user.ID + "\nНомер: " + user.Number + "\nБыл забанен администратором " + Admin.IsAdmin + " урованя " + Admin.FIO;
						botClient.SendText(settings.ChannelAdmin, text, user, true);
						
						IsBanUser.ThisBan(botClient, _Message, user, settings);
						
						
					}
					else
					{
						Admin.BanDate = System.DateTime.Now.AddDays(ban);
						Admin.IsAdmin = 0;
						db.Save();
						IsBanUser.ThisBan(botClient, _Message, Admin, settings);
						System.String temp = "Администратор " + Admin.IsAdmin + " уровня: @" + Admin.Username + " пытался забанить 2 уровня администратора на " + ban + " дней!" + "\nФИО: " + user.FIO + "\nС данного администратора снята админка, так же он был забанен во всех чатах! Если бан был выдан случайно пропишите /UbBan " + user.ID;
						InlineButton inlineButton = new InlineButton();
						Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup answer = inlineButton.AdminPanelAppeal(message, user.ID, _Message.Text);
						botClient.SendText(settings.ChannelAdmin, temp, Admin, true, replyMarkup: answer);
					}
				}
			}
			catch (System.Exception Ex) { Log.Logging(Ex); }
			return;
		}
	}

	internal class DeletePostMessage : AbsCommand
	{
		public override System.String Name { get; set; } = "/deletepost";

		public override void Execute(TelegramBotClient botClient, System.Object message)
		{
			DataBase db = Singleton.GetInstance().Context;
			Message _Message = message as Message;

			System.Int32 FromId = _Message.ReplyToMessage.From.Id;

			botClient.DeleteMessage(_Message.Chat.Id, _Message.MessageId, "205");

			User Admin = db.GetUser(_Message.From.Id);

			try
			{
				if (Admin.IsAdmin > 1)
				{
					Message _message = _Message.ReplyToMessage;

					User user = db.GetUser(_message.From.Id);

					Post post = db.GetPostInChannel(_message.Chat.Id, _message.MessageId);

					PostTemplate template = db.GetPostTemplate(post.PostTemplateId);

					AdController.DeleteAllPosts(botClient, template);

					db.RemoveTemplate(template.Id);
				}
			}
			catch (System.Exception Ex)
			{
				Log.Logging(Ex);
			}

			return;
		}
	}
}