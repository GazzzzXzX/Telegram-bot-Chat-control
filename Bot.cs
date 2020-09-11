using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using BotCore.Advertising;
using BotCore.Advertising.Command;
using BotCore.Blockchain;
using BotCore.Blockchain.Command.Settings;
using BotCore.PhotoChannel;
using BotCore.SQL;
using BotCore.TelegramClient;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;

namespace BotCore
{
	internal class BotEssence
	{
		protected System.String ApiKey { get; set; }
		protected DataBase dataBase { get; set; } = Singleton.GetInstance().Context;

		protected TelegramBotClient BotClient { get; set; }
		protected List<AbsCommand> commands = new List<AbsCommand>();
		protected List<AbsCommand> SleshComands = new List<AbsCommand>();
		protected List<ICommandSlash> commandSlashes = new List<ICommandSlash>();
		protected List<Advertising.Command.AbsCommand> AdvertisingCommands = new List<Advertising.Command.AbsCommand>();


		public BotEssence(System.String Api)
		{
			ApiKey = Api;
			BotClient = new TelegramBotClient(ApiKey);

			SetCommands();
			SetCommandsSlash();
			NewSleshCommands();
			SetAdvertisingCommands();
		}

		private void NewSleshCommands() => SleshComands = new List<AbsCommand>
			{
				new BotCore.SleshCommands.Ban(),
				new BotCore.SleshCommands.DeletePostMessage()
			};

		private void SetCommandsSlash() => commandSlashes = new List<ICommandSlash>()
		{
			new Kick(),                                         new UserBan(),
			new UnUserBan(),                                    new AddChannel(),
			new AddGroup(),                                     new GetAdmin()
		};

		private void SetCommands() => commands = new List<AbsCommand>()
		{
			new Start(),                                        new Register(),
			new MeinMenu(),                                     new Accaunt(),
			new Close(),                                        new ChouseFIO(),
			new ChouseNumber(),                                 new BackToAccauntMenu(),
			new ShowMyReviews(),                                new ShowOtherReviews(),
			new MyReviews(),                                    new BackToReviewsMenu(),
			new DeleteMyReviews(),                              new BackToReviewsMenuAndDelete(),
			new ChangeMyReviews(),                              new OneStar(),
			new TwoStar(),                                      new ThreeStar(),
			new FourStar(),                                     new FiveStar(),
			new SearchUsers(),                                  new SearchID(),
			new SearchNumber(),                                 new LeaveFeedback(),
			new CheckFeedback(),                                new BackToAccauntMenuAndDelete(),
			new SendComplaint(),                                new BackToReviewsMenuBan(),
			new SendAppeal(),                                   new ThisAdminChannel(),
			new SettingBot(),                                   new BackToSetting(),
			new BanUser(),                                      new UnBanUser(),
			new Flud(),                                         new AddAdmin(),
			new DeleteAdmin(),                                  new CountPost(),
			new AddWord(),                                      new AddUser(),
			new StopFlud(),                                     new PayBan(),
			new CountBanFludMinus(),                            new CountBanFludPlus(),
			new MathBan(),                                      new CountBanMatMinus(),
			new CountBanMatPlus(),                              new CallAdminYes(),
			new CallAdmin(),                                    new ProcentBan(),
			new CountBanProcentMinus(),                         new CountBanProcentPlus(),
			new LinkBan(),                                      new CountBanPLinkMinus(),
			new CountBanLinkPlus(),                             new IsBanFlud(),
			new IsKickFlud(),                                   new IsBanMat(),
			new IsKickMat(),                                    new IsBanLink(),
			new IsKickLink(),                                   new IsMutFlud(),
			new IsMutLink(),                                    new IsMutMat(),
			new DeleteWord(),                                   new Info(),
			new SearchUsername(),                               new CoinAdd(),
			new CoinMinus(),                                    new CoinPlus(),
			new Featured(),                                     new FeaturedSelected(),
			new AddFeatured(),                                  new DeleteFeatured(),
			new PayConfirm(),                                   new AddAccauntBot(),
			new LimitUser(),                                    new BanOneDay(),
			new BanTwoDay(),                                    new BanEver(),
			new MuteOneDay(),                                   new MuteTwoDay(),
			new MuteEver(),                                     new GetChatPostCount(),
			new SelectThisChannel(),                            new NoLimitedPostChannel(),
			new CloseInfo(),
			
			new NotificationBot(),								new NotificationBotText(),
			new BackToNotifacation(), 							new ButtounNotification(),
			new AddNotificationBotText(), 						new AddButtounNotification(),
			new PublishNotification(), 							new AddPictureNotification(),
			new PublishChatNotification(), 						new NotificationChannelId(),
			new NotificationSetCaht(), 							new PublishChatNotificationYes(),
			new PublishChatNotificationNo()
		};

		private void SetAdvertisingCommands() => AdvertisingCommands = new List<Advertising.Command.AbsCommand>()
		{
			new AdvertisingShow(),                              new BackToMenu(),
			new BackToAdvertisingMenu(),                        new Balance(),
			new PaymentCount(),                                 new BackToAdvertisingMenu(),
			new GetChat(),                                      new BackToChannelMenu(),
			new SetAdvertising(),                               new AddAdvertising(),
			new BackSetAdvertising(),                           new MenuAdContent(),
			new Advertising.Command.AddPhoto(),                 new ShowPostTemplateData(),
			new Advertising.Command.ContentEditorText(),        new DeletePhoto(),
			new ChangePhoto(),                                  new ShowContent(),
			new Advertising.Command.DeletePost(),               new SetChatInPostTemplate(),
			new SelectChatInPostTemplate(),                     new SetAdvertisingOneChannel(),
			new Advertising.Command.DeletePostTemplate(),

			new CalendarBot(),                                  new Back_Calendar(),
			new Next_Calendar(),                                new ChouseData(),
			new ChangeDate(),                                   new BackToCalendar(),

			new TimeShow(),                                     new AddTime(),
			new AddTimeButton(),                                new  BackTimeShowForAddTime(),
			new  LeftHour(),                                    new LeftMinute(),
			new  RightHour(),                                   new  RightMinute(),

			new ChangeTime(),                                   new BackToTime(),
			new LeftMinute_Change(),                            new RightMinute_Change(),
			new DeleteTime(),                                   new RightHour_Change(),
			new LeftHour_Change(),                              new ClickDateTime_Result(),
			new NextToTimeMenu_Calendar(),

			new Advertising.Command.DeletePost(),               new SendToModerationConfirmation(),
			new BackToAdTemplateFromValidation(),               new SendToValidationConfirmed(),
			new AdminAccept(),                                  new PayPostTemplate(),
			new AdminCancel(),

			new AdminPanelPrice(),                              new BackInAdminPanelPrice(),
			new ShowStaticPrice(),                              new BackInShowStaticChatPrice(),
			new EditChatPrice(),                                new EditStaticChatPrice(),
			new EditStaticTimePrice(),                          new ShowChatPrice(),
			new RightChangeChat_Price(),                        new LeftChangeChat_Price(),
			new BackOfChangePriceChatInShowChatPrice(),         new LeftChangeStaticTime_Price(),
			new RightChangeStaticTime_Price(),                  new BackOfChangePriceTimeInMenu(),
			new LeftChangeStaticChat_Price(),                   new RightChangeStaticChat_Price(),
			new BackOfChangePriceChatInMenu(),                  new CreateAdKeyboardTypeMessage(),
			new ChooseDatePinned(),                             new CalendarPinnedBot(),
			new EditStaticTimePinnedPrice(),                    new LeftChangeStaticPinned_Price(),
			new RightChangeStaticPinned_Price(),                new BackOfChangePricePinnedInMenu(),
			new EditStaticTimePinnedNotificationPrice(),        new LeftChangeStaticPinnedNotification_Price(),
			new RightChangeStaticPinnedNotification_Price(),    new BackOfChangePricePinnedNotificationInMenu(),
			new BackToAddAdversting(),                          new BackToTypeMessage(),
			new BackToContentKeyboard(),                        new EditStaticTimeStandartMessage(),
			new LeftChangeStaticStandartMessage(),              new RightChangeStaticStandartMessage(),

			//--------------Blockchain---------------//
			new GuarantorMeinMenu(),                             new BackToMeinMenu(),
			new TransactionCreationGuarantor(),                  new BackToGuarantorMeinMenu(),
			new ChoosingPaymentMethod(),                         new BackToTransactionCreationGuarantor(),
			new AddUserInTransaction(),                          new CommissionPayment(),
			new BackToChoosingPaymentMethod(),                   new BTCPaymentMethod(),
			new EthereumPaymentMethod(),                         new USDTPaymentMethod(),
			new RipplePaymentMethod(),                           new CommissionPaymentRecipient(),
			new CommissionPaymentSender(),
			//------------MyTransaction------------//
			new ShowMyTransaction(),                             new NameTransactionConfirm(),
			new NameTransactionCancel(),                         new NameTransaction(),
			new ConfirmMyTransaction(),                          new ConfirmThisTransaction(),
			new BackToSelectConfirmOrCancelThisTransaction(),    new CancelMyTransaction(),
			new ConfirmThisTransactionUserTwo(),                 new CancelThisTransactionUserTwo(),
			new BackToShowMyTransaction(),                       new AdminCallBlockchain(),
			//-------------Reputation--------------//
			new BackToReputationUser(),                          new BackToShowOneReputationCancelTransaction(),
			new BackToShowOneReputationConfirmTransaction(),     new CancelTransactionReputationUser(),
			new ConfirmTransactionReputationUser(),              new NameReputationCancelTransaction(),
			new NameReputationConfirmTransaction(),              new ReputationUser(),
			//--------------Admin----------------//
			new GetAdminInBlockChain(),                          new GetAdminInMyTransaction(),
			new SetConfirmAdminInBlockChain(),                   new GetMoneySenderAdminInBlockChain(),
			new SetCancelAdminInMyTransaction(),                 new GetMoneyRecipientAdminInMyTransaction(),
			new BackToSetMoneyCount(),                           new SettingsAdminInBlockChain(),
			new LeftComissionAdmin(),                            new RightComissionAdmin(),
			new LeftComissionCripta(),                           new RightComissionCripta(),
			new SettingsComissionAdmin(),                        new SettingsComissionCripta(),
			new BackToConfirmOrCancelThisTransactionUserTwo(),

			//------------Photo-----------------//
			new AddPhotoInDataBase(),                           new AddCategoty(),
			new AddChannelInCategoty(),                         new GetAnaliticsInAllChatWord(),
			new AnaliticsShow(),                                new GetAnaliticsInOneChatWord(),
			new GetAnaliticsWord(),                             new GetAnaliticsInAllChatPharase(),
			new GetAnaliticsPharase(),                          new GetAnaliticsInOneChatPharase(),
			new AddUserInChannel(),								new AddUsersAnalitics(),


			//-----------Info------------------//
			new InfoCenter(),                                   new AnaliticsShowUser(),
			new DeleteButtonOk(),                               new RegulationsUBC(),
			new DeleteChat(),                                   new DeleteCategory(),

			new ThisIncome(),									new ThisIncomeChannel(),
			new ThisIncomeUser(),								new ThisIncomeAdmin(),


			new UpInAdmin1lvl(),								new UpInAdmin2lvl(),
			new DownInAdmin0lvl(),								new ChuseAdmin(),
			
			new CategoryChat(),									new AcceptOrderAdmin(),
			new CancelOrderAdmin()
		};
	}

	internal class Bot : BotEssence
	{
		public static Dictionary<System.Int32,Calendar.Calendar> users_calendar = new Dictionary<System.Int32, Calendar.Calendar>();

		public AutoResetEvent _exitEvent;

		[Obsolete]
		public Bot(System.String api) : base(api)
		{
			DataBase db = Singleton.GetInstance().Context;

			_exitEvent = new AutoResetEvent(false);

			BotClient.OnMessage += BotAnaliz;
			BotClient.OnCallbackQuery += CallBack;
			BotClient.OnUpdate += BotClient_OnUpdate;
			BotClient.OnInlineQuery += BotClient_OnInlineQuery;
			BotClient.OnInlineResultChosen += BotClient_OnInlineResultChosen;
			BotClient.OnReceiveError += BotClient_OnReceiveError;


			BotClient.StartReceiving();
			Console.WriteLine(BotClient.GetMeAsync().Result);

			var tempUsers = db.GetCalendarUsers();
			if (tempUsers != null)
			{
				users_calendar = GetCalendarUsers(tempUsers);
			}

			UpdateSystem.DeletePinnedMessages(BotClient);
		}
		
		public Dictionary<System.Int32, Calendar.Calendar> GetCalendarUsers(List<User> users)
		{
			Dictionary<System.Int32, Calendar.Calendar> users_calendar = new Dictionary<System.Int32, Calendar.Calendar>();

			foreach (var user in users)
			{
				users_calendar.Add(user.ID, new Calendar.Calendar());
			}
			
			return users_calendar;
		}

		~Bot()
		{
			_exitEvent.Set();

			Console.WriteLine("Destructing bot....");
		}

		private void BotClient_OnReceiveError(Object sender, Telegram.Bot.Args.ReceiveErrorEventArgs receiveErrorEventArgs) => Console.WriteLine("Received error: {0} — {1}",
			  receiveErrorEventArgs.ApiRequestException.ErrorCode,
			  receiveErrorEventArgs.ApiRequestException.Message);

		private void BotClient_OnInlineResultChosen(Object sender, Telegram.Bot.Args.ChosenInlineResultEventArgs e) => Console.WriteLine($"Received inline result: {e.ChosenInlineResult.ResultId}");

		private async void BotClient_OnInlineQuery(Object sender, Telegram.Bot.Args.InlineQueryEventArgs e)
		{
			List<InlineQueryResultBase> list = new List<InlineQueryResultBase>();

			DataBase   db         = Singleton.GetInstance().Context;
			Channel[]  channels   = db.GetChannels();
			Category[] categories = db.GetCategories();

			if (e.InlineQuery.Query == "")
			{
				list.Clear();

				foreach (Channel channel in channels)
				{
					Settings settings = db._settings.FirstOrDefault();
					if (channel.IDChannel != settings.ChannelAdmin)
					{
						String text = "";
						if (channel.Description != null)
						{
							List<String> description = channel.Description.Split("\n").ToList();
							for (Int32 i = 0; i < description.Count; i++)
							{
								if (description[i].StartsWith("ПРАВИЛА"))
								{
									continue;
								}

								if (description[i].Length == 0)
								{
									continue;
								}

								text += description[i] + "\n" + "\n";
							}
						}

						List<List<InlineKeyboardButton>> button = new List<List<InlineKeyboardButton>>
						{
							new List<InlineKeyboardButton>()
						};
						button[button.Count - 1]
							.Add(new InlineKeyboardButton() {Text = "Перейти в канал", Url = channel.LinkChannel});
						button.Add(new List<InlineKeyboardButton>());
						button[button.Count - 1]
							.Add(new InlineKeyboardButton() {Text = "Категории", SwitchInlineQueryCurrentChat = ""});
						button.Add(new List<InlineKeyboardButton>());
						button[button.Count - 1]
							.Add(new InlineKeyboardButton() {Text = "Закрыть", CallbackData = CommandText.Close});

						list.Add(new InlineQueryResultArticle(id: channel.IDChannel.ToString(),
						                                      title: channel.ChannelName,
						                                      inputMessageContent:
						                                      new
							                                      InputTextMessageContent($"{text}\nЗаходите - " +
							                                                              channel.InviteLink))
						{
							Url         = "https://" + channel.LinkChannel,
							Title       = channel.ChannelName,
							Description = text,
							HideUrl     = true,
							ThumbUrl    = channel.PhotoLink,
							ThumbHeight = 48,
							ThumbWidth  = 48,
							ReplyMarkup = button.ToArray()
						});
					}
				}
			}

			else if (categories.Any(p => p.Name == e.InlineQuery.Query))
			{
				list.Clear();
				Category category = categories.FirstOrDefault(p => p.Name == e.InlineQuery.Query);
				foreach (Channel channel in channels)
				{
					if (category.Id != channel.CategoryId)
					{
						continue;
					}

					String text = "";
					if (channel.Description != null)
					{
						List<String> description = channel.Description.Split("\n").ToList();
						for (Int32 i = 0; i < description.Count; i++)
						{
							if (description[i].StartsWith("ПРАВИЛА"))
							{
								continue;
							}
	
							if (description[i].Length == 0)
							{
								continue;
							}
	
							text += description[i] + "\n" + "\n";
						}
					}
	
					List<List<InlineKeyboardButton>> button = new List<List<InlineKeyboardButton>>
					{
						new List<InlineKeyboardButton>()
					};
					button[button.Count - 1]
						.Add(new InlineKeyboardButton() {Text = "Перейти в канал", Url = channel.LinkChannel});
					button.Add(new List<InlineKeyboardButton>());
					button[button.Count - 1]
						.Add(new InlineKeyboardButton() {Text = "Категории", SwitchInlineQueryCurrentChat = ""});
					button.Add(new List<InlineKeyboardButton>());
					button[button.Count - 1]
						.Add(new InlineKeyboardButton() {Text = "Закрыть", CallbackData = CommandText.Close});
					
					list.Add(new InlineQueryResultArticle(id: channel.IDChannel.ToString(),
					                                      title: channel.ChannelName,
					                                      inputMessageContent:
					                                      new
						                                      InputTextMessageContent($"{text}\nЗаходите - " +
						                                                              channel.InviteLink))
					{
						Url         = "https://" + channel.LinkChannel,
						Title       = channel.ChannelName,
						Description = text,
						HideUrl     = true,
						ThumbUrl    = channel.PhotoLink != "" ? channel.PhotoLink : "https://bipbap.ru/wp-content/uploads/2017/10/0_8eb56_842bba74_XL-640x400.jpg",
						ThumbHeight = 48,
						ThumbWidth  = 48,
						ReplyMarkup = button.ToArray()
					});
				}
			}
			try
			{
				await BotClient.AnswerInlineQueryAsync(e.InlineQuery.Id, list, isPersonal: true, cacheTime: 0);
			}
			catch (Exception b) { Log.Logging(b); }
		}


		private System.Boolean IsUserAdmin(Telegram.Bot.Types.User user, Chat chat = null)
		{
			if (Singleton.GetInstance().Context._users.Any(c => c.ID == user.Id && c.IsAdmin != 0))
			{
				return true;
			}
			if (chat != null)
			{
				List<ChatMember> Ads = BotClient.GetChatAdministratorsAsync(chat).Result.ToList();
				if (Ads.Any(c => c.User.Id == user.Id))
				{
					return true;
				}
			}
			return false;
		}

		private System.Boolean IsSystemEvent(Message message)
		{
			switch (message.Type)
			{
				case Telegram.Bot.Types.Enums.MessageType.ChatMemberLeft:
				case Telegram.Bot.Types.Enums.MessageType.ChatTitleChanged:
				case Telegram.Bot.Types.Enums.MessageType.ChatPhotoChanged:
				case Telegram.Bot.Types.Enums.MessageType.ChatPhotoDeleted:
				case Telegram.Bot.Types.Enums.MessageType.GroupCreated:
				case Telegram.Bot.Types.Enums.MessageType.SupergroupCreated:
				case Telegram.Bot.Types.Enums.MessageType.MigratedToSupergroup:
				case Telegram.Bot.Types.Enums.MessageType.MigratedFromGroup:
				case Telegram.Bot.Types.Enums.MessageType.ChannelCreated:
					return true;
			}
			return false;
		}

		[Obsolete]
		private async void BotClient_OnUpdate(System.Object sender, Telegram.Bot.Args.UpdateEventArgs e)
		{
			DataBase db = Singleton.GetInstance().Context;

			if (e.Update.PreCheckoutQuery != null)
			{
				await BotClient.AnswerPreCheckoutQueryAsync(e.Update.PreCheckoutQuery.Id);
				if (e.Update.PreCheckoutQuery.InvoicePayload == "Pay is correct")
				{
					User user = db.GetUser(e.Update.PreCheckoutQuery.From.Id);
					user.BanDate = System.DateTime.Now;
					db.Save();
					IsUnBan.ThisUnBan(BotClient, user);
					await Task.Run(() => SetIncomeChannel(e.Update.PreCheckoutQuery.From.Id, e.Update.PreCheckoutQuery.TotalAmount / 100));
				}
				else if (e.Update.PreCheckoutQuery.InvoicePayload == "Balance is correct")
				{
					AdUser adUser = db.GetAdUser(e.Update.PreCheckoutQuery.From.Id);
					adUser.Balance += e.Update.PreCheckoutQuery.TotalAmount / 100;
					db.Save();
					await Task.Run(() => SetIncomeChannel(e.Update.PreCheckoutQuery.From.Id, e.Update.PreCheckoutQuery.TotalAmount / 100));
				}
				else if (e.Update.PreCheckoutQuery.InvoicePayload == "PostTemplate is correct")
				{
					AdUser adUser = db.GetAdUser(e.Update.PreCheckoutQuery.From.Id);
					PostTemplate postTemplate = db.GetTempalte(adUser.User.ID, adUser.EditingPostTemplateId);
					postTemplate.IsPaid = true;
					System.Object ob = await AdController.AssemblyTemplate(BotClient, postTemplate);
					if (ob is Message)
					{
						Message mes = (Message)ob;
						System.String text = mes.Text;
						await BotClient.SendTextMessageAsync(CommandText.bufferChannelId, text);
					}
					else
					{
						List<InputMediaBase> inputMedias = (List<InputMediaBase>)ob;

						await BotClient.SendMediaGroupAsync(CommandText.bufferChannelId, inputMedias);
					}
					IsDataTaken.IsCheck(BotClient, e.Update.PreCheckoutQuery.From.Id, postTemplate.PostTime.ToList());
					db.Save();

					await Task.Run(() => SetIncomeChannel(e.Update.PreCheckoutQuery.From.Id, e.Update.PreCheckoutQuery.TotalAmount / 100, postTemplate));
				}
				else if (e.Update.PreCheckoutQuery.InvoicePayload == "Pay Confirm User")
				{
					User user = db.GetUser(e.Update.PreCheckoutQuery.From.Id);
					user.PayConfirm = true;
					user.PayDate = System.DateTime.Today.AddMonths(1);
					db.Save();

					await Task.Run(() => SetIncomeChannel(e.Update.PreCheckoutQuery.From.Id, e.Update.PreCheckoutQuery.TotalAmount / 100));
				}
			}
		}

		private void SetIncome(List<ChannelInfo> channel, Int32 id, Single total)
		{
			DataBase db = Singleton.GetInstance().Context;
			Int32 temp = channel.Count;
			foreach (var item in channel)
			{
				Income income = db.GetIncome(id, ((item.Channel.Id + 1000000000000) * -1));
				if(income == null)
				{
					db.SetValue<Income>(new Income() { ChannelId = (item.Channel.Id + 1000000000000) * -1, UserId = id, dateTime = System.DateTime.Today, SumIncome = total / temp });
				}
				else
				{
					income.SumIncome = (total / temp) + income.SumIncome;
				}
			}
			db.Save();
		}

		private void SetIncomeChannel(Int32 id, Single total, PostTemplate postTemplate = null)
		{
			DataBase db = Singleton.GetInstance().Context;
			List<ChannelInfo> channel = null;
			if (postTemplate == null)
			{
				channel = StartSession.Test(db.GetChannelsList(), id);
			}
			else
			{
				List<Channel> channels = new List<Channel>();
				foreach (var item in postTemplate.PostChannel)
				{
					Channel Thischannel = db.GetChannel(item.ChannelId);
					channels.Add(Thischannel);
				}
				channel = StartSession.Test(channels, id);
			}

			Int32 temp = channel.Count;

			foreach (var item in channel)
			{
				IncomeChannel incomeChannel = db.GetIncomeChannels((item.Channel.Id + 1000000000000) * -1);
				if (incomeChannel == null)
				{
					db.SetValue<IncomeChannel>(new IncomeChannel() { ChannelId = (item.Channel.Id + 1000000000000) * -1, DateTime = System.DateTime.Today, SumIncome = total / temp });
				}
				else
				{
					incomeChannel.SumIncome = (total / temp) + incomeChannel.SumIncome;
				}
			}
			db.Save();
			SetIncome(channel, id, total);
			SetIncomeChannelAdmin(channel, total);
		}

		private void SetIncomeChannelAdmin(List<ChannelInfo> channel, Single total)
		{
			if (channel == null)
			{
				throw new ArgumentNullException(nameof(channel));
			}

			DataBase db = Singleton.GetInstance().Context;
			total = (total * 20) / 100;
			foreach (var item in channel)
			{
				Int32 temp = item.Admins.Count;
				foreach (var admin in item.Admins)
				{
					IncomeChannelAdmin income = db.GetIncomeChannelsAdmin(admin.Id, ((item.Channel.Id + 1000000000000) * -1));
					if (admin.Id != BotClient.BotId)
					{
						if (income == null)
						{
							db.SetValue<IncomeChannelAdmin>(new IncomeChannelAdmin() { ChannelId = (item.Channel.Id + 1000000000000) * -1, UserId = admin.Id, DateTime = System.DateTime.Today, SumIncome = total / temp });
						}
						else
						{
							income.SumIncome = (total / temp) + income.SumIncome;
						}
					}
				}
			}
			db.Save();
		}

		private void SetUserName(CallbackQuery message, DataBase db)
		{
			
			User user = db.GetUser(message.From.Id);
			try
			{
				if (user != null)
				{
					if (user.Username == null || message.From.Username != user.Username)
					{
						user.Username = message.From.Username;
						db.Save();
					}
				}
			}
			catch (Exception e)
			{
				Log.Logging(e);
			}
		}

		private void SetUserName(Message message, DataBase db)
		{
			User user = db.GetUser(message.From.Id);
			try
			{
				if (user != null)
				{
					if (user.Username == null || message.From.Username != user.Username)
					{
						user.Username = message.From.Username;
						db.Save();
					}
				}
			}
			catch (Exception e)
			{
				Log.Logging(e);
			}
		}
		

		private void AsycnCallBack(System.Object sender, Telegram.Bot.Args.CallbackQueryEventArgs e)
		{
			DateTime d;
			DataBase db = Singleton.GetInstance().Context;

			if (e.CallbackQuery.From.Username != null)
			{
				SetUserName(e.CallbackQuery, db);
			}

			if (DateTime.TryParseExact(e.CallbackQuery.Data.Replace("+", "").Replace('.', '/'), "d/M/yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out d))
			{
				try
				{
					Advertising.Command.AbsCommand Command = AdvertisingCommands.FirstOrDefault(c => c.Equals(e.CallbackQuery.Data.Replace("+", "")));
					if (Command == null && DateTime.ParseExact(e.CallbackQuery.Data.Replace("+", "").Replace('.', '/'), "d/M/yyyy", null) != null)
					{
						Command = AdvertisingCommands.FirstOrDefault(c => c.Equals(Advertising.CommandsText.ChoseDate));
					}
					Command.Execute(BotClient, e.CallbackQuery);
				}
				catch (Exception ex)
				{
					Log.Logging(ex);
				}
			}
			else if (commands.Any(c => c.Equals(e.CallbackQuery.Data)))
			{
				try
				{
					AbsCommand Command = commands.FirstOrDefault(c => c.Equals(e.CallbackQuery.Data)); // вытягиваем класс
					Command.Execute(BotClient, e.CallbackQuery);
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
			else if (commandSlashes.Any(c => c.Equals(e.CallbackQuery.Data)))
			{
				try
				{
					ICommandSlash Command = commandSlashes.FirstOrDefault(c => c.Equals(e.CallbackQuery.Data)); // вытягиваем класс
					Command.Execute(BotClient, e.CallbackQuery);
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
			else if (AdvertisingCommands.Any(c => c.Equals(e.CallbackQuery.Data)))
			{
				try
				{
					Advertising.Command.AbsCommand command = AdvertisingCommands.FirstOrDefault(c => c.Equals(e.CallbackQuery.Data));
					command.Execute(BotClient, e.CallbackQuery);
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
			else if (db.GetChannels().Any(p => p.IDChannel == System.Convert.ToInt64(e.CallbackQuery.Data.Split(" ")[0])))
			{
				ThisChannel channel = new ThisChannel();
				channel.Execute(BotClient, e.CallbackQuery, System.Convert.ToInt64(e.CallbackQuery.Data.Split(" ")[0]));
			}
			else
			{
				User user = db.GetUser(e.CallbackQuery.From.Id);
				if (IsNullDataBase.IsNull(BotClient, e.CallbackQuery, user)) return;
				if (user.Chain == 53)
				{
					SelectMyReviews selectReviews = new SelectMyReviews();
					selectReviews.Execute(BotClient, e.CallbackQuery);
				}
				else if (user.Chain == 54)
				{
					SelectOtherReviews selectReviews = new SelectOtherReviews();
					selectReviews.Execute(BotClient, e.CallbackQuery);
				}
				else if (user.IsAdmin > 0)
				{
					System.String[] words = e.CallbackQuery.Data.Split(new System.Char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
					if (words[1] == "1")
					{
						SetBan setBan = new SetBan();
						setBan.Execute(BotClient, e.CallbackQuery);
					}
					else if (words[1] == "2")
					{
						SetCancel setCancel = new SetCancel();
						setCancel.Execute(BotClient, e.CallbackQuery);
					}
					else if (words[1] == "3")
					{
						SetAppeal setAppeal = new SetAppeal();
						setAppeal.Execute(BotClient, e.CallbackQuery);
					}
					else if (words[1] == "4")
					{
						SetCancelAppeal setAppeal = new SetCancelAppeal();
						setAppeal.Execute(BotClient, e.CallbackQuery);
					}
				}
			}
		}

		private async void AsyncBotAnalize(System.Object sender, Telegram.Bot.Args.MessageEventArgs e)
		{
			DataBase db = Singleton.GetInstance().Context;

			//BotClient.SendPhotoAsync(e.Message.From.Id, e.Message.Photo[2].FileId);

			if (e.Message.From.Username != null)
			{
				SetUserName(e.Message, db);
			}


			if (IsSystemEvent(e.Message))
			{
				try
				{
					await BotClient.DeleteMessageAsync(e.Message.Chat.Id, e.Message.MessageId);
					return;
				}
				catch { }
			}
			if (e.Message.Chat.Type == Telegram.Bot.Types.Enums.ChatType.Supergroup)
			{
				if (e.Message.ReplyToMessage != null)
				{
					if (e.Message.ReplyToMessage.MessageId != 0 && e.Message.Text != null)
					{
						if (IsUserAdmin(e.Message.From, e.Message.Chat))
						{
							if (SleshComands.Any(c => e.Message.Text.ToLower().Contains(c.Name)))
							{
								SleshComands.First(c => e.Message.Text.ToLower().Contains(c.Name)).Execute(BotClient, e.Message);
								return;
							}
						}
					}
				}
			}

			if (e.Message.Text != null && e.Message.Chat.Type == ChatType.Private)
			{
				User user = db.GetUser(e.Message.From.Id);

				if ((user == null || user.FIO == null || user.Number == "0") && e.Message.Text != "/start")
				{
					try
					{
						new Start().Execute(BotClient, e.Message);
						BotClient.DeleteMessage(user.ID, e.Message.MessageId, "Bot");
					}
					catch (Exception exception)
					{
						Log.Logging(exception);
					}
				}
					
			}

			if (e.Message.Text != null || e.Message.Contact != null || e.Message.ForwardFromChat != null
				|| e.Message.NewChatMembers != null || e.Message.Sticker != null || e.Message.Photo != null || e.Message.MediaGroupId != null 
				|| e.Message.Type == Telegram.Bot.Types.Enums.MessageType.Voice || e.Message.Type == Telegram.Bot.Types.Enums.MessageType.Video 
				|| e.Message.Type == Telegram.Bot.Types.Enums.MessageType.VideoNote || e.Message.Type == Telegram.Bot.Types.Enums.MessageType.Document 
				|| e.Message.Type == Telegram.Bot.Types.Enums.MessageType.Game || e.Message.Type == Telegram.Bot.Types.Enums.MessageType.Location 
				|| e.Message.Type == Telegram.Bot.Types.Enums.MessageType.Contact || e.Message.Type == Telegram.Bot.Types.Enums.MessageType.MessagePinned)
			{
				if (commands.Any(c => c.Equals(e.Message.Text))) // проверка есть ли команда в списке
				{
					try
					{
						commands.FirstOrDefault(c => c.Equals(e.Message.Text)).Execute(BotClient, e.Message); // вытягиваем класс
					}
					catch (System.Exception ex)
					{
						Log.Logging(ex);
					}
				}
				else if (commandSlashes.Any(c => c.Equals(e.Message.Text)))
				{
					try
					{
						commandSlashes.FirstOrDefault(c => c.Equals(e.Message.Text)).Execute(BotClient, e.Message); // вытягиваем класс
					}
					catch (System.Exception ex)
					{
						Log.Logging(ex);
					}
				}
				else if (e.Message.Text == null || e.Message.Sticker != null || e.Message.Photo != null || e.Message.MediaGroupId != null)
				{
					Analize t = new Analize();
					t.ChekRegister(BotClient, e.Message);
				}
				else if (!e.Message.Text.Equals("/start"))
				{
					Analize t = new Analize();
					t.ChekRegister(BotClient, e.Message);
				}
			}
		}

		private async void CallBack(System.Object sender, Telegram.Bot.Args.CallbackQueryEventArgs e) => await Task.Run(() => AsycnCallBack(sender, e));

		private async void BotAnaliz(System.Object sender, Telegram.Bot.Args.MessageEventArgs e) => await Task.Run(() => AsyncBotAnalize(sender, e));
	}
}