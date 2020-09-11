using System;
using System.Collections.Generic;
using System.Linq;
using BotCore.Advertising;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace BotCore
{
	internal class InlineButton
	{
		public InlineKeyboardMarkup OrderAdmin(User user)
		{
			List<List<InlineKeyboardButton>> list = new List<List<InlineKeyboardButton>>
			{
				new List<InlineKeyboardButton>()
			};
			list[^1].Add(new InlineKeyboardButton()
			{
				Text = "Принять", CallbackData = CommandText.AceptOrderAdmin + " " + user.ID
			});
			list.Add(new List<InlineKeyboardButton>());
			list[^1].Add(new InlineKeyboardButton()
			{
				Text = "Отменить", CallbackData = CommandText.CancelOrderAdmin + " " + user.ID
			});
			return new InlineKeyboardMarkup(list);
		}
		
		public InlineKeyboardMarkup Closeinfo = new InlineKeyboardButton[][]
		{
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton { Text = "✖️Закрыть✖️", CallbackData = "/closeInfo" }
			}

		};

		public InlineKeyboardMarkup Register = new InlineKeyboardButton[][]
		{
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton{ Text = "❗️Регистрация❗️",
					CallbackData = CommandText.Register}
			}
		};

		public InlineKeyboardMarkup Payment = new InlineKeyboardButton[][]
		{
			new InlineKeyboardButton[]
			{
				InlineKeyboardButton.WithPayment("Оплатить!")
			},
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton{ Text = "🔙Назад🔙", CallbackData = CommandText.BackToReviewsMenuBan}
			}
		};

		public InlineKeyboardMarkup PaymentUser = new InlineKeyboardButton[][]
		{
			new InlineKeyboardButton[]
			{
				InlineKeyboardButton.WithPayment("Оплатить!")
			},
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton{ Text = "🔙Назад🔙", CallbackData = CommandText.BackToAccauntMenu}
			}
		};

		public InlineKeyboardMarkup Accaunt = new InlineKeyboardButton[][]
		{
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton{ Text = "🖌Редактировать ФИО🖌",
					CallbackData = CommandText.ChouseFIO}
			},
			//new InlineKeyboardButton[]
			//{
			//	new InlineKeyboardButton { Text = "🔑Купить абонимент на месяц🔑", CallbackData = CommandText.PayConfirm }
			//},
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton{ Text = "💌Отзывы💌", CallbackData = CommandText.Reviews}
			},
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton{ Text = "🔍Поиск участника🔍", CallbackData = CommandText.SearchUsers}
			},
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton{ Text = "🔐Гарант🔐", CallbackData = CommandText.GuarantorMeinMenu}
			},
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton{Text = "🎫Реклама🎫", CallbackData = Advertising.CommandsText.Advertising }
			},
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton{Text = "💹Аналитика💹", CallbackData = CommandText.AnaliticsShowUser }
			},
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton { Text = "💭Чаты💭", CallbackData = "/CategoryChat" + " " + 0.ToString()}
			},
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton { Text = "❔О нас и правила чатов❔", CallbackData = CommandText.InfoCenter }
			},
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton{ Text = "⭐️Связь с администратором⭐️", CallbackData = CommandText.CallAdmin }
			},
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton { Text = "✖️Закрыть✖️",
					CallbackData = CommandText.Close}
			}
		};

		public InlineKeyboardMarkup CallAdmin = new InlineKeyboardButton[][]
		{
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton{ Text = "👍Да👍",
					CallbackData = CommandText.CallAdminYes}
			},
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton{ Text = "👎Нет👎", CallbackData = CommandText.BackToAccauntMenu}
			}
		};

		public InlineKeyboardMarkup AdminAccaunt = new InlineKeyboardButton[][]
		{
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton{ Text = "🖌Редактировать ФИО🖌",
					CallbackData = CommandText.ChouseFIO}
			},
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton{ Text = "💌Отзывы💌", CallbackData = CommandText.Reviews}
			},
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton{ Text = "🔍Поиск участника🔍", CallbackData = CommandText.SearchUsers}
			},
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton{Text = "🎫Реклама🎫", CallbackData = Advertising.CommandsText.Advertising}
			},
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton{ Text = "🔐Гарант🔐", CallbackData = CommandText.GuarantorMeinMenu}
			},
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton{Text = "💹Аналитика💹", CallbackData = CommandText.AnaliticsShow}
			},
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton { Text = "💭Чаты💭", CallbackData = "/CategoryChat" + " " + 0.ToString() }
			},
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton { Text = "❔О нас и правила чатов❔", CallbackData = CommandText.InfoCenter }
			},
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton{ Text = "⚙️Настройки бота⚙️", CallbackData = CommandText.SettingBot}
			},
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton { Text = "✖️Закрыть✖️",
					CallbackData = CommandText.Close}
			}
		};

		public InlineKeyboardMarkup SettingBot = new InlineKeyboardButton[][]
		{
			/*new InlineKeyboardButton[]
			{
				new InlineKeyboardButton{ Text = "⚰️Забанить участника (/ban @...)⚰️",
					CallbackData = CommandText.BanUser}
			},
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton{ Text = "💉Разбанить участника(/unban @...)💉",
					CallbackData = CommandText.UnBanUser}
			},*/

			//new InlineKeyboardButton[]
			//{
			//	new InlineKeyboardButton{ Text = "⏳Настройки⏳",
			//		CallbackData = CommandText.Flud}
			//},
			//new InlineKeyboardButton[]
			//{
			//	new InlineKeyboardButton{ Text = "🤬Добавить запретное слово🤬",
			//		CallbackData = CommandText.AddWord}
			//},
			//new InlineKeyboardButton[]
			//{
			//	new InlineKeyboardButton{ Text = "🤬Удалить запретное слово🤬",
			//		CallbackData = CommandText.DeleteWord}
			//},
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton{ Text = "🔙Назад🔙", CallbackData = CommandText.BackToAccauntMenu}
			}
		};

		public InlineKeyboardMarkup NotificationBot()
		{
			List<List<InlineKeyboardButton>> list = new List<List<InlineKeyboardButton>>
			{
				new List<InlineKeyboardButton>()
			};
			list[^1].Add(new InlineKeyboardButton()
			{
				Text		 = "💬Текст💬", 
				CallbackData = CommandText.AddNotificationBotText
			});

			list.Add(new List<InlineKeyboardButton>());
			list[^1].Add(new InlineKeyboardButton()
			{
				Text		 = "💬Добавить кнопку💬", 
				CallbackData = CommandText.AddButtounNotification
			});
			list.Add(new List<InlineKeyboardButton>());			
			list[^1].Add(new InlineKeyboardButton()
			{
				Text         = "❇Добавить картинку❇",
				CallbackData = CommandText.AddPictureNotification
			});
			list.Add(new List<InlineKeyboardButton>());
			list[^1].Add(new InlineKeyboardButton()
			{
				Text         = "✔Опубликовать в приват✔", 
				CallbackData = CommandText.PublishNotification
			});
			list.Add(new List<InlineKeyboardButton>());
			list[^1].Add(new InlineKeyboardButton()
			{
				Text         = "✔Опубликовать в чаты✔", 
				CallbackData = CommandText.NotificationSetCaht
			});
			
			list.Add(new List<InlineKeyboardButton>());
			list[^1].Add(new InlineKeyboardButton()
			{
				Text         = "🔙Назад🔙",
				CallbackData = CommandText.BackToSetting
			});
			return new InlineKeyboardMarkup(list);
		}

		public InlineKeyboardMarkup PublishNotificationAllOrOneChat()
		{
			var db = Singleton.GetInstance().Context;
			Channel[] channels = db.GetChannels();
			NotificationChat[] notificationChats = db.GetNotificationChats();
			List<List<InlineKeyboardButton>> list = new List<List<InlineKeyboardButton>>
			{
				new List<InlineKeyboardButton>()
			};
			list[^1].Add(new InlineKeyboardButton()
			{
				Text = "✔Опубликовать в чаты✔", CallbackData = CommandText.PublishChatNotification
			});
			
			list.Add(new List<InlineKeyboardButton>());
			list[^1].Add(new InlineKeyboardButton()
			{
				Text = "✔Опубликовать c закрепом без уведомления✔", CallbackData = CommandText.PublishChatNotificationYes
			});
			list.Add(new List<InlineKeyboardButton>());
			list[^1].Add(new InlineKeyboardButton()
			{
				Text = "✔Опубликовать c закрепом с уведомление✔", CallbackData = CommandText.PublishChatNotificationNo
			});

			foreach (Channel channel in channels)
			{
				if (notificationChats.Any(p => p.IdChannel.IDChannel == channel.IDChannel))
				{
					list.Add(new List<InlineKeyboardButton>());
					list[^1].Add(new InlineKeyboardButton()
					{
						Text         = channel.ChannelName                     + "✔",
						CallbackData = CommandText.NotificationChannelId + " " + channel.IDChannel
					});
				}
				else
				{
					list.Add(new List<InlineKeyboardButton>());
					list[^1].Add(new InlineKeyboardButton()
					{
						Text         = channel.ChannelName,
						CallbackData = CommandText.NotificationChannelId + " " + channel.IDChannel
					});
				}
			}

			list.Add(new List<InlineKeyboardButton>());
			list[^1].Add(new InlineKeyboardButton()
			{
				Text         = "🔙Назад🔙",
				CallbackData = CommandText.BackToNotifacation
			});
			return new InlineKeyboardMarkup(list);
		}
		
		public InlineKeyboardMarkup NotificationBotText()
		{
			List<List<InlineKeyboardButton>> list = new List<List<InlineKeyboardButton>>
			{
				new List<InlineKeyboardButton>()
			};
			/*list[^1].Add(new InlineKeyboardButton()
			{
				Text		 = "Название",
				CallbackData = CommandText.NotificationBot
			});

			list.Add(new List<InlineKeyboardButton>());*/
			list[^1].Add(new InlineKeyboardButton()
			{
				Text         = "🔙Добавить текст🔙",
				CallbackData = CommandText.AddNotificationBotText
			});
			
			list.Add(new List<InlineKeyboardButton>());
			list[^1].Add(new InlineKeyboardButton()
			{
				Text         = "🔙Назад🔙",
				CallbackData = CommandText.BackToNotifacation
			});
			return new InlineKeyboardMarkup(list);
		}

		public InlineKeyboardMarkup BackToNotificationBotText = new InlineKeyboardButton[][]
		{
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton{ Text = "🔙Назад🔙",
					CallbackData               = CommandText.BackToNotifacation }
			}
		};
		
		public InlineKeyboardMarkup BackToNotificationBotButton = new InlineKeyboardButton[][]
		{
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton{ Text = "🔙Назад🔙",
					CallbackData               = CommandText.BackToNotifacation }
			}
		};

		public InlineKeyboardMarkup PublishNotification(List<CollectionButtonNotification> CollectionNotification, Boolean isChat = false)
		{
			List<List<InlineKeyboardButton>> list = new List<List<InlineKeyboardButton>>
			{
				new List<InlineKeyboardButton>()
			};

			foreach (CollectionButtonNotification collectionButtonNotification in CollectionNotification)
			{
				list[^1].Add(new InlineKeyboardButton()
				{
					Text         = "Ссылка", Url = collectionButtonNotification.buttonNotification.Text
				});
				list.Add(new List<InlineKeyboardButton>());
			}

			if (isChat == false)
			{
				list[^1].Add(new InlineKeyboardButton() {Text = "❌Закрыть❌", CallbackData = CommandsText.ButtonOk});
			}

			return new InlineKeyboardMarkup(list);
		}

		public InlineKeyboardMarkup BackToNotification()
		{
			List<List<InlineKeyboardButton>> list = new List<List<InlineKeyboardButton>>
			{
				new List<InlineKeyboardButton>()
			};
			list[^1].Add(new InlineKeyboardButton()
			{
				Text         = "🔙Назад🔙",
				CallbackData = CommandText.BackToNotifacation
			});
			return new InlineKeyboardMarkup(list);
		}
		
		public InlineKeyboardMarkup NotificationBotButton()
		{
			List<List<InlineKeyboardButton>> list = new List<List<InlineKeyboardButton>>
			{
				new List<InlineKeyboardButton>()
			};
			list[^1].Add(new InlineKeyboardButton()
			{
				Text         = "🔙Добавить кнопку🔙",
				CallbackData = CommandText.AddButtounNotification
			});

			list.Add(new List<InlineKeyboardButton>());
			list[^1].Add(new InlineKeyboardButton()
			{
				Text         = "🔙Назад🔙",
				CallbackData = CommandText.BackToNotifacation
			});
			return new InlineKeyboardMarkup(list);
		}
		
		public InlineKeyboardMarkup SettingBotLvl2(User user)
		{
			/*new InlineKeyboardButton[]
			{
				new InlineKeyboardButton{ Text = "⚰️Забанить участника (/Ban @...)⚰️",
					CallbackData = CommandText.BanUser}
			},
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton{ Text = "💉Разбанить участника(/UnBan @...)💉",
					CallbackData = CommandText.UnBanUser}
			},*/
			
			List<List<InlineKeyboardButton>> list = new List<List<InlineKeyboardButton>>
			{
				new List<InlineKeyboardButton>()
			};
			
			list[^1].Add(new InlineKeyboardButton()
			{
				Text = "⏳Настройки⏳",
				CallbackData = CommandText.Flud
			});

			if (user.IsAdmin == 3)
			{
				list.Add(new List<InlineKeyboardButton>());
				list[^1].Add(new InlineKeyboardButton()
				{
					Text = "🔔Уведомление🔔", CallbackData = CommandText.NotificationBot
				});
			

				list.Add(new List<InlineKeyboardButton>());
				list[^1].Add(new InlineKeyboardButton()
				{
					Text = "💼Назначить администратора💼",
					CallbackData = CommandText.AddAdmin
				});
				list.Add(new List<InlineKeyboardButton>());
				list[^1].Add(new InlineKeyboardButton()
				{
					Text = "❌Снять администратора❌",
					CallbackData = CommandText.DeleteAdmin
				});
			}
			list.Add(new List<InlineKeyboardButton>());
			list[^1].Add(new InlineKeyboardButton()
			{
				Text         = "🌓Количество постов в день🌓",
				CallbackData = CommandText.CountPost
			});
			list.Add(new List<InlineKeyboardButton>());
			list[^1].Add(new InlineKeyboardButton()
			{
				Text         = "🤬Добавить запретное слово🤬",
				CallbackData = CommandText.AddWord
			});
			list.Add(new List<InlineKeyboardButton>());
			list[^1].Add(new InlineKeyboardButton()
			{
				Text         = "🤬Удалить запретное слово🤬",
				CallbackData = CommandText.DeleteWord
			});
			list.Add(new List<InlineKeyboardButton>());
			list[^1].Add(new InlineKeyboardButton()
			{
				Text         = "🤬Кол-во человек добавление в группу🤬",
				CallbackData = CommandText.AddUser
			});
			list.Add(new List<InlineKeyboardButton>());
			list[^1].Add(new InlineKeyboardButton()
			{
				Text         = "💰Кол-во бонусов за добавление💰",
				CallbackData = CommandText.CoinAdd
			});
			list.Add(new List<InlineKeyboardButton>());
			list[^1].Add(new InlineKeyboardButton()
			{
				Text         = "✳Добавить картинку✳",
				CallbackData = CommandText.AddPicture
			});
			list.Add(new List<InlineKeyboardButton>());
			list[^1].Add(new InlineKeyboardButton()
			{
				Text         = "🆕Добавить категорию🆕",
				CallbackData = CommandText.AddCategoty
			});
			list.Add(new List<InlineKeyboardButton>());
			list[^1].Add(new InlineKeyboardButton()
			{
				Text         = "🆕Добавить чат в категорию🆕",
				CallbackData = CommandText.AddChannelInCategoty
			});
			list.Add(new List<InlineKeyboardButton>());
			list[^1].Add(new InlineKeyboardButton()
			{
				Text         = "✖Удалить чат✖",
				CallbackData = CommandText.DeleteChat
			});
			list.Add(new List<InlineKeyboardButton>());
			list[^1].Add(new InlineKeyboardButton()
			{
				Text         = "✖Удалить категорию✖",
				CallbackData = CommandText.DeleteCategory
			});
			list.Add(new List<InlineKeyboardButton>());
			list[^1].Add(new InlineKeyboardButton()
			{
				Text         = "🛡Правила UBC🛡",
				CallbackData = CommandText.RegulationsUBC
			});
			list.Add(new List<InlineKeyboardButton>());
			list[^1].Add(new InlineKeyboardButton()
			{
				Text         = "➕Добавить аккаунт➕",
				CallbackData = CommandText.AddAccaunt
			});
			list.Add(new List<InlineKeyboardButton>());
			list[^1].Add(new InlineKeyboardButton()
			{
				Text         = "🔙Назад🔙",
				CallbackData = CommandText.BackToAccauntMenu
			});

			return new InlineKeyboardMarkup(list);
		}

		public InlineKeyboardMarkup BackToSettingAdmin = new InlineKeyboardButton[][]
		{
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton{ Text = "🔙Назад🔙",
					CallbackData = CommandText.BackToSetting }
			}
		};

		public InlineKeyboardMarkup FludAdmin = new InlineKeyboardButton[][]
		{
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton{ Text = "🛑Остановить флуд🛑",
					CallbackData = CommandText.StopFlud }
			},
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton{ Text = "🔙Назад🔙",
					CallbackData = CommandText.BackToSetting }
			}
		};

		public InlineKeyboardMarkup CoinAdd()
		{
			DataBase db = Singleton.GetInstance().Context;
			Settings settings = db.GetSettings();
			List<List<InlineKeyboardButton>> list = new List<List<InlineKeyboardButton>>
			{
				new List<InlineKeyboardButton>()
			};
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "<",
				CallbackData = CommandText.CoinMinus
			});
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = settings.Coin.ToString(),
				CallbackData = CommandText.BackToSetting
			});
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = ">",
				CallbackData = CommandText.CoinPlus
			});
			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "Назад",
				CallbackData = CommandText.BackToSetting
			});
			return new InlineKeyboardMarkup(list);
		}

		public InlineKeyboardMarkup CountFludBanIsBan(System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase db = Singleton.GetInstance().Context;
			List<List<InlineKeyboardButton>> list = new List<List<InlineKeyboardButton>>
			{
				new List<InlineKeyboardButton>()
			};
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "<",
				CallbackData = CommandText.LickBan
			});
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "Флуд",
				CallbackData = CommandText.BackToSetting
			});
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = ">",
				CallbackData = CommandText.MathBan
			});

			Settings settings = db.GetSettings();
			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "<",
				CallbackData = CommandText.CountBanFludMinus
			});
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = settings.CountBanFlud.ToString() + " дн",
				CallbackData = CommandText.BackToSetting
			});
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = ">",
				CallbackData = CommandText.CountBanFludPlus
			});

			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "<",
				CallbackData = CommandText.IsKickFlud
			});
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "Бан",
				CallbackData = CommandText.BackToSetting
			});
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = ">",
				CallbackData = CommandText.IsKickFlud
			});

			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "Назад",
				CallbackData = CommandText.BackToSetting
			});

			return new InlineKeyboardMarkup(list);
		}

		public InlineKeyboardMarkup CountFludBanIsKick(System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase db = Singleton.GetInstance().Context;
			List<List<InlineKeyboardButton>> list = new List<List<InlineKeyboardButton>>
			{
				new List<InlineKeyboardButton>()
			};
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "<",
				CallbackData = CommandText.LickBan
			});
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "Флуд",
				CallbackData = CommandText.BackToSetting
			});
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = ">",
				CallbackData = CommandText.MathBan
			});

			Settings settings = db.GetSettings();
			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "<",
				CallbackData = CommandText.CountBanFludMinus
			});
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = settings.CountBanFlud.ToString() + " дн",
				CallbackData = CommandText.BackToSetting
			});
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = ">",
				CallbackData = CommandText.CountBanFludPlus
			});

			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "<",
				CallbackData = CommandText.IsBanFlud
			});
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "Кик",
				CallbackData = CommandText.BackToSetting
			});
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = ">",
				CallbackData = CommandText.IsMutFlud
			});

			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "Назад",
				CallbackData = CommandText.BackToSetting
			});

			return new InlineKeyboardMarkup(list);
		}

		public InlineKeyboardMarkup CountFludBanIsMut(System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			List<List<InlineKeyboardButton>> list = new List<List<InlineKeyboardButton>>
			{
				new List<InlineKeyboardButton>()
			};
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "<",
				CallbackData = CommandText.LickBan
			});
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "Флуд",
				CallbackData = CommandText.BackToSetting
			});
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = ">",
				CallbackData = CommandText.MathBan
			});

			DataBase db = Singleton.GetInstance().Context;
			Settings settings = db.GetSettings();

			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "<",
				CallbackData = CommandText.CountBanFludMinus
			});
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = settings.CountBanFlud.ToString() + " дн",
				CallbackData = CommandText.BackToSetting
			});
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = ">",
				CallbackData = CommandText.CountBanFludPlus
			});

			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "<",
				CallbackData = CommandText.IsKickFlud
			});
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "Мут",
				CallbackData = CommandText.BackToSetting
			});
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = ">",
				CallbackData = CommandText.IsBanFlud
			});

			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "Назад",
				CallbackData = CommandText.BackToSetting
			});

			return new InlineKeyboardMarkup(list);
		}

		public InlineKeyboardMarkup CountMatBanIsBan(System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			List<List<InlineKeyboardButton>> list = new List<List<InlineKeyboardButton>>
			{
				new List<InlineKeyboardButton>()
			};
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "<",
				CallbackData = CommandText.Flud
			});
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "Мат",
				CallbackData = CommandText.BackToSetting
			});
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = ">",
				CallbackData = CommandText.ProcentBan
			});

			DataBase db = Singleton.GetInstance().Context;
			Settings settings = db.GetSettings();
			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "<",
				CallbackData = CommandText.CountBanMatMinus
			});
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = settings.CountBanMat.ToString() + " дн",
				CallbackData = CommandText.BackToSetting
			});
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = ">",
				CallbackData = CommandText.CountBanMatPlus
			});

			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "<",
				CallbackData = CommandText.IsKickMat
			});
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "Бан",
				CallbackData = CommandText.BackToSetting
			});
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = ">",
				CallbackData = CommandText.IsKickMat
			});

			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "Назад",
				CallbackData = CommandText.BackToSetting
			});

			return new InlineKeyboardMarkup(list);
		}

		public InlineKeyboardMarkup CountMatBanIsKick(System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			List<List<InlineKeyboardButton>> list = new List<List<InlineKeyboardButton>>
			{
				new List<InlineKeyboardButton>()
			};
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "<",
				CallbackData = CommandText.Flud
			});
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "Мат",
				CallbackData = CommandText.BackToSetting
			});
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = ">",
				CallbackData = CommandText.ProcentBan
			});

			DataBase db = Singleton.GetInstance().Context;
			Settings settings = db.GetSettings();
			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "<",
				CallbackData = CommandText.CountBanMatMinus
			});
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = settings.CountBanMat.ToString() + " дн",
				CallbackData = CommandText.BackToSetting
			});
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = ">",
				CallbackData = CommandText.CountBanMatPlus
			});

			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "<",
				CallbackData = CommandText.IsBanMat
			});
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "Кик",
				CallbackData = CommandText.BackToSetting
			});
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = ">",
				CallbackData = CommandText.IsMutMat
			});

			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "Назад",
				CallbackData = CommandText.BackToSetting
			});

			return new InlineKeyboardMarkup(list);
		}

		public InlineKeyboardMarkup CountMatBanIsMut(System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			List<List<InlineKeyboardButton>> list = new List<List<InlineKeyboardButton>>
			{
				new List<InlineKeyboardButton>()
			};
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "<",
				CallbackData = CommandText.Flud
			});
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "Мат",
				CallbackData = CommandText.BackToSetting
			});
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = ">",
				CallbackData = CommandText.ProcentBan
			});

			DataBase db = Singleton.GetInstance().Context;
			Settings settings = db.GetSettings();

			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "<",
				CallbackData = CommandText.CountBanMatMinus
			});
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = settings.CountBanMat.ToString() + " дн",
				CallbackData = CommandText.BackToSetting
			});
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = ">",
				CallbackData = CommandText.CountBanMatPlus
			});

			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "<",
				CallbackData = CommandText.IsKickMat
			});
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "Мут",
				CallbackData = CommandText.BackToSetting
			});
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = ">",
				CallbackData = CommandText.IsBanMat
			});

			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "Назад",
				CallbackData = CommandText.BackToSetting
			});

			return new InlineKeyboardMarkup(list);
		}

		public InlineKeyboardMarkup CountProcentBanIsBan(System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			List<List<InlineKeyboardButton>> list = new List<List<InlineKeyboardButton>>
			{
				new List<InlineKeyboardButton>()
			};
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "<",
				CallbackData = CommandText.MathBan
			});
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "Соответсвия",
				CallbackData = CommandText.BackToSetting
			});
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = ">",
				CallbackData = CommandText.LickBan
			});

			DataBase db = Singleton.GetInstance().Context;
			Settings settings = db.GetSettings();

			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "<",
				CallbackData = CommandText.CountBanProcentMinus
			});
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = (settings.ProcentMessage * 100).ToString() + " %",
				CallbackData = CommandText.BackToSetting
			});
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = ">",
				CallbackData = CommandText.CountBanProcentPlus
			});

			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "Назад",
				CallbackData = CommandText.BackToSetting
			});

			return new InlineKeyboardMarkup(list);
		}

		public InlineKeyboardMarkup CountLinkBanIsBan(System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			List<List<InlineKeyboardButton>> list = new List<List<InlineKeyboardButton>>
			{
				new List<InlineKeyboardButton>()
			};
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "<",
				CallbackData = CommandText.ProcentBan
			});
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "Ссылка",
				CallbackData = CommandText.BackToSetting
			});
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = ">",
				CallbackData = CommandText.Flud
			});

			DataBase db = Singleton.GetInstance().Context;
			Settings settings = db.GetSettings();

			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "<",
				CallbackData = CommandText.CountBanLinkMinus
			});
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = settings.CountLink.ToString() + " дн",
				CallbackData = CommandText.BackToSetting
			});
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = ">",
				CallbackData = CommandText.CountBanLinkPlus
			});

			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "<",
				CallbackData = CommandText.IsKickLink
			});
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "Бан",
				CallbackData = CommandText.BackToSetting
			});
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = ">",
				CallbackData = CommandText.IsKickLink
			});

			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "Назад",
				CallbackData = CommandText.BackToSetting
			});

			return new InlineKeyboardMarkup(list);
		}

		public InlineKeyboardMarkup CountLinkBanIsKick(System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			List<List<InlineKeyboardButton>> list = new List<List<InlineKeyboardButton>>
			{
				new List<InlineKeyboardButton>()
			};
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "<",
				CallbackData = CommandText.ProcentBan
			});
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "Ссылка",
				CallbackData = CommandText.BackToSetting
			});
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = ">",
				CallbackData = CommandText.Flud
			});

			DataBase db = Singleton.GetInstance().Context;
			Settings settings = db.GetSettings();
			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "<",
				CallbackData = CommandText.CountBanLinkMinus
			});
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = settings.CountLink.ToString() + " дн",
				CallbackData = CommandText.BackToSetting
			});
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = ">",
				CallbackData = CommandText.CountBanLinkPlus
			});

			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "<",
				CallbackData = CommandText.IsBanLink
			});
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "Кик",
				CallbackData = CommandText.BackToSetting
			});
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = ">",
				CallbackData = CommandText.IsMutLink
			});

			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "Назад",
				CallbackData = CommandText.BackToSetting
			});

			return new InlineKeyboardMarkup(list);
		}

		public InlineKeyboardMarkup CountLinkBanIsMut(System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			List<List<InlineKeyboardButton>> list = new List<List<InlineKeyboardButton>>
			{
				new List<InlineKeyboardButton>()
			};
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "<",
				CallbackData = CommandText.ProcentBan
			});
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "Ссылка",
				CallbackData = CommandText.BackToSetting
			});
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = ">",
				CallbackData = CommandText.Flud
			});

			DataBase db = Singleton.GetInstance().Context;
			Settings settings = db.GetSettings();

			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "<",
				CallbackData = CommandText.CountBanLinkMinus
			});
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = settings.CountLink.ToString() + " дн",
				CallbackData = CommandText.BackToSetting
			});
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = ">",
				CallbackData = CommandText.CountBanLinkPlus
			});

			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "<",
				CallbackData = CommandText.IsKickLink
			});
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "Мут",
				CallbackData = CommandText.BackToSetting
			});
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = ">",
				CallbackData = CommandText.IsBanLink
			});

			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "Назад",
				CallbackData = CommandText.BackToSetting
			});

			return new InlineKeyboardMarkup(list);
		}

		public InlineKeyboardMarkup SearchAdminPanel = new InlineKeyboardButton[][]
		{
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton{ Text = "Поиск по ID",
					CallbackData = CommandText.SearchID }
			},
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton{ Text = "Поиск по номеру телефона",
					CallbackData = CommandText.SearchNumber }
			},
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton{ Text = "🔙Назад🔙",
					CallbackData = CommandText.BackToSetting }
			}
		};

		public InlineKeyboardMarkup BanAccaunt = new InlineKeyboardButton[][]
		{
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton { Text = "🗂Подать апелляцию🗂", CallbackData = CommandText.SendAppeal}
			},
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton { Text = "💵Отплатить бан💵", CallbackData = CommandText.PayBan}
			},
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton { Text = "❔О нас и правила чатов❔", CallbackData = CommandText.InfoCenter}
			},
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton { Text = "✖️Закрыть✖️",
					CallbackData = CommandText.Close}
			}
		};

		public InlineKeyboardMarkup MyReviews = new InlineKeyboardButton[][]
		{
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton{ Text = "💬Отправленные отзывы💬",
					CallbackData = CommandText.ShowMyReviews}
			},
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton{ Text = "💬Полученные отзывы💬",
					CallbackData = CommandText.ShowOtherReviews}
			},
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton{ Text = "🔙Назад🔙", CallbackData = CommandText.BackToAccauntMenu}
			}
		};

		public InlineKeyboardMarkup BackToSetting = new InlineKeyboardButton[][]
		{
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton{ Text = "🔙Назад🔙",
					CallbackData = CommandText.BackToSetting }
			}
		};

		public InlineKeyboardMarkup BackToAccauntMenu = new InlineKeyboardButton[][]
		{
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton{ Text = "🔙Назад🔙",
					CallbackData = CommandText.BackToAccauntMenu }
			}
		};

		public InlineKeyboardMarkup BackToReviewsMenuBan = new InlineKeyboardButton[][]
		{
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton{ Text = "🔙Назад🔙",
					CallbackData = CommandText.BackToReviewsMenuBan }
			}
		};

		public InlineKeyboardMarkup BackToAccauntMenuAndDelete = new InlineKeyboardButton[][]
		{
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton{ Text = "🔙Назад🔙",
					CallbackData = CommandText.BackToAccauntMenuAndDelete }
			}
		};

		public InlineKeyboardMarkup BackToReviewsMenu = new InlineKeyboardButton[][]
		{
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton{ Text = "🔙Назад🔙",
					CallbackData = CommandText.BackToReviewsMenu }
			}
		};

		public InlineKeyboardMarkup BackToReviewsMenuAndDelete = new InlineKeyboardButton[][]
		{
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton{ Text = "🔙Назад🔙",
					CallbackData = CommandText.BackToReviewsMenuAndDelete }
			}
		};

		public InlineKeyboardMarkup SearchUsers = new InlineKeyboardButton[][]
		{
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton{ Text = "Избранные",
					CallbackData = CommandText.Featured }
			},
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton{ Text = "Поиск по ID",
					CallbackData = CommandText.SearchID }
			},
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton{ Text = "Поиск по номеру телефона",
					CallbackData = CommandText.SearchNumber }
			},
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton{ Text = "Поиск по @username", CallbackData = CommandText.SearchUsername}
			},
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton{ Text = "🔙Назад🔙",
					CallbackData = CommandText.BackToAccauntMenu }
			}
		};

		public InlineKeyboardMarkup Featured(User user)
		{
			DataBase db = Singleton.GetInstance().Context;
			FeaturedUserNew[] featuredUsers = db.GetFeaturedUsers();
			List<List<InlineKeyboardButton>> list = new List<List<InlineKeyboardButton>>();

			foreach (FeaturedUserNew item in featuredUsers)
			{
				if (item.UserWhoAddedId == user.ID)
				{
					User userTwo = db.GetUser(item.UserId);
					list.Add(new List<InlineKeyboardButton>());
					list[list.Count - 1].Add(new InlineKeyboardButton()
					{
						Text = userTwo.FIO,
						CallbackData = CommandText.FeaturedSelected + " " + userTwo.ID
					});
				}
			}

			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "🔙Назад🔙",
				CallbackData = CommandText.BackToAccauntMenu
			});

			return new InlineKeyboardMarkup(list);
		}

		public InlineKeyboardMarkup InteractionUsers(User user, System.Boolean delete = false, FeaturedUserNew featuredUserNew = null, System.String mes = null, System.Boolean isAdmin = false)
		{
			List<List<InlineKeyboardButton>> list = new List<List<InlineKeyboardButton>>
			{
				new List<InlineKeyboardButton>()
			};
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "🧧Оставить отзыв🧧",
				CallbackData = CommandText.LeaveFeedback
			});
			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "📖Посмотреть отзывы📖",
				CallbackData = CommandText.CheckFeedback
			});
			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "✉️Отправить жалобу✉️",
				CallbackData = CommandText.SendComplaint + " " + mes
			});
			if (delete == false)
			{
				list.Add(new List<InlineKeyboardButton>());
				list[list.Count - 1].Add(new InlineKeyboardButton()
				{
					Text = "✨Добавить в избранное✨",
					CallbackData = CommandText.AddFeatured + " " + user.ID
				});
			}
			else
			{
				list.Add(new List<InlineKeyboardButton>());
				list[list.Count - 1].Add(new InlineKeyboardButton()
				{
					Text = "✨Удалить из избранного✨",
					CallbackData = CommandText.DeleteFeatured + " " + featuredUserNew.ID
				});
			}
			if (isAdmin == true)
			{
				list.Add(new List<InlineKeyboardButton>());
				list[list.Count - 1].Add(new InlineKeyboardButton()
				{
					Text = "💥Ограничить пользователя💥",
					CallbackData = CommandText.LimitUser + " " + user.ID
				});
			}
			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "🔙Назад🔙",
				CallbackData = CommandText.BackToAccauntMenuAndDelete
			});

			return new InlineKeyboardMarkup(list);
		}

		public InlineKeyboardMarkup AdminPanelInMessage(User user)
		{
			List<List<InlineKeyboardButton>> list = new List<List<InlineKeyboardButton>>
			{
				new List<InlineKeyboardButton>()
			};
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "Забанить на 1 день",
				CallbackData = CommandText.BanOneDay + " " + user.ID
			});
			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "Забанить на 2 день",
				CallbackData = CommandText.BanTwoDay + " " + user.ID
			});
			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "Забанить на навсегда",
				CallbackData = CommandText.BanEver + " " + user.ID
			});
			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "Замутить на 1 день",
				CallbackData = CommandText.MuteOneDay + " " + user.ID
			});
			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "Замутить на 2 деня",
				CallbackData = CommandText.MuteTwoDay + " " + user.ID
			});
			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "Замутить на навсегда",
				CallbackData = CommandText.MuteEver + " " + user.ID
			});
			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "🔙Назад🔙",
				CallbackData = CommandText.BackToAccauntMenu
			});
			return new InlineKeyboardMarkup(list);
		}

		public InlineKeyboardMarkup InteractionUsersNoReviews = new InlineKeyboardButton[][]
		{
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton { Text = "📖Посмотреть отзывы📖", CallbackData = CommandText.CheckFeedback}
			},
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton { Text = "✉️Отправить жалобу✉️", CallbackData = CommandText.SendComplaint}
			},
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton { Text = "🔙Назад🔙", CallbackData = CommandText.BackToAccauntMenuAndDelete}
			}
		};

		public InlineKeyboardMarkup SetRating = new InlineKeyboardButton[][]
		{
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton { Text = "⭐️", CallbackData = CommandText.OneStar }
			},
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton { Text = "⭐️⭐️", CallbackData = CommandText.TwoStar }
			},
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton { Text = "⭐️⭐️⭐️", CallbackData = CommandText.ThreeStar }
			},
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton { Text = "⭐️⭐️⭐️⭐️", CallbackData = CommandText.FourStar }
			},
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton { Text = "⭐️⭐️⭐️⭐️⭐️", CallbackData = CommandText.FiveStar }
			}
		};

		public InlineKeyboardMarkup ChangeOrDeleteMyReviews = new InlineKeyboardButton[][]
		{
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton{ Text = "Удалить", CallbackData = CommandText.DeleteMyReviews},
				new InlineKeyboardButton{ Text = "Редактировать", CallbackData = CommandText.ChangeMyReviews}
			},
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton{ Text = "🔙Назад🔙", CallbackData = CommandText.BackToReviewsMenu }
			}
		};

		public InlineKeyboardMarkup ShowMyReviews(System.Object message)
		{
			CallbackQuery mes = message as CallbackQuery;
			List<List<InlineKeyboardButton>> list = new List<List<InlineKeyboardButton>>();
			System.Int32 temp = 0;
			System.Boolean temp2 = true;

			CommandText.ShowReviews.Clear();

			DataBase db = Singleton.GetInstance().Context;

			Reviews[] reviews = db.GetReviews();
			foreach (Reviews review in reviews)
			{
				User user = db.GetUser(review.IDRecipient);
				if (review.IDSender == mes.From.Id)
				{
					temp++;
					if (temp < 2)
					{
						if (temp2 == true)
						{
							list.Add(new List<InlineKeyboardButton>());
							temp2 = false;
						}
					}
					else
					{
						temp = 0;
						temp2 = true;
					}

					CommandText.ShowReviews.Add(review.ID, review.Description);
					list[list.Count - 1].Add(new InlineKeyboardButton()
					{
						Text = user.FIO,
						CallbackData = review.ID + " " + CommandText.ShowReviews[review.ID]
					});
				}
			}

			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "Назад",
				CallbackData = CommandText.BackToReviewsMenu
			});

			return new InlineKeyboardMarkup(list);
		}

		public InlineKeyboardMarkup ShowMyReviewsByUsers(System.Object message)
		{
			CallbackQuery mes = message as CallbackQuery;
			List<List<InlineKeyboardButton>> list = new List<List<InlineKeyboardButton>>();
			System.Int32 temp = 0;
			System.Boolean temp2 = true;

			CommandText.ShowReviews.Clear();

			DataBase db = Singleton.GetInstance().Context;

			Reviews[] reviews = db.GetReviews();
			foreach (Reviews review in reviews)
			{
				User user = db.GetUser(review.IDSender);
				if (review.IDRecipient == mes.From.Id)
				{
					User UserTwo = db.GetUser(review.IDSender);
					temp++;
					if (temp < 2)
					{
						if (temp2 == true)
						{
							list.Add(new List<InlineKeyboardButton>());
							temp2 = false;
						}
					}
					else
					{
						temp = 0;
						temp2 = true;
					}

					CommandText.ShowReviews.Add(review.ID, review.Description);
					if (UserTwo.IsAdmin == 0)
					{
						list[list.Count - 1].Add(new InlineKeyboardButton()
						{
							Text = review.Name,
							CallbackData = review.ID + " " + CommandText.ShowReviews[review.ID]
						});
					}
					else if (UserTwo.IsAdmin > 0)
					{
						list[list.Count - 1].Insert(0, new InlineKeyboardButton()
						{
							Text = review.Name + "(🛡Админ!)",
							CallbackData = review.ID + " " + CommandText.ShowReviews[review.ID]
						});
					}
				}
			}

			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "Назад",
				CallbackData = CommandText.BackToReviewsMenu
			});

			return new InlineKeyboardMarkup(list);
		}

		public InlineKeyboardMarkup ShowOtherReviews(System.Object message)
		{
			CallbackQuery mes = message as CallbackQuery;
			List<List<InlineKeyboardButton>> list = new List<List<InlineKeyboardButton>>();
			System.Int32 temp = 0;
			System.Boolean temp2 = true;

			CommandText.ShowReviews.Clear();

			DataBase db = Singleton.GetInstance().Context;

			User user = db.GetUser(mes.From.Id);
			Reviews[] reviews = db.GetReviews();

			foreach (Reviews review in reviews)
			{
				if (review.IDRecipient == user.IDRecipient && review.Name != null)
				{
					User UserTwo = db.GetUser(review.IDSender);
					temp++;
					if (temp < 2)
					{
						if (temp2 == true)
						{
							list.Add(new List<InlineKeyboardButton>());
							temp2 = false;
						}
					}
					else
					{
						temp = 0;
						temp2 = true;
					}

					CommandText.ShowReviews.Add(review.ID, review.Description);
					if (UserTwo.IsAdmin == 0)
					{
						list[list.Count - 1].Add(new InlineKeyboardButton()
						{
							Text = review.Name,
							CallbackData = review.ID + " " + CommandText.ShowReviews[review.ID]
						});
					}
					else if (UserTwo.IsAdmin > 0)
					{
						list[list.Count - 1].Insert(0, new InlineKeyboardButton()
						{
							Text = review.Name + "(🛡Админ!)",
							CallbackData = review.ID + " " + CommandText.ShowReviews[review.ID]
						});
					}
				}
			}

			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "Назад",
				CallbackData = CommandText.BackToAccauntMenu
			});

			return new InlineKeyboardMarkup(list);
		}

		public InlineKeyboardMarkup AdminPanel(System.Object message, System.Int32 IDBan, System.String text)
		{
			Message _message = message as Message;
			List<InlineKeyboardButton> list = new List<InlineKeyboardButton>
			{
				new InlineKeyboardButton()
				{
					Text = "Забанить",
					CallbackData = IDBan + " " + 1 + " " + text
				},
				new InlineKeyboardButton()
				{
					Text = "Отменить",
					CallbackData = IDBan + " " + 2 + " " + text
				}
			};

			return new InlineKeyboardMarkup(list);
		}

		public InlineKeyboardMarkup AdminPanelAppeal(System.Object message, System.Int32 IDUser, System.String text)
		{
			Message _message = message as Message;
			List<InlineKeyboardButton> list = new List<InlineKeyboardButton>
			{
				new InlineKeyboardButton()
				{
					Text = "Разбанить",
					CallbackData = IDUser + " " + 3 + " " + text
				},
				new InlineKeyboardButton()
				{
					Text = "Отменить",
					CallbackData = IDUser + " " + 4 + " " + text
				}
			};

			return new InlineKeyboardMarkup(list);
		}

		public InlineKeyboardMarkup MessageUserInBot(System.Object message, System.String mes)
		{
			Message _message = message as Message;
			List<InlineKeyboardButton> list = new List<InlineKeyboardButton>
			{
				new InlineKeyboardButton()
				{
					Text = "Разбанить"

				},
				new InlineKeyboardButton()
				{
					Text = "Отменить"
				}
			};
			return new InlineKeyboardMarkup(list);
		}

		/// <summary>
		/// При заходе в данное меню можно сразу ввести ограничение на все чаты либо выбрать чат!
		/// </summary>
		/// <returns></returns>
		public InlineKeyboardMarkup LimitedChannelMenu()
		{
			List<List<InlineKeyboardButton>> list = new List<List<InlineKeyboardButton>>
			{
				new List<InlineKeyboardButton>()
			};
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "Выбрать чат",
				CallbackData = CommandText.GetChatPostCount
			});
			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "🔙Назад🔙",
				CallbackData = CommandText.BackToSetting
			});
			return new InlineKeyboardMarkup(list);
		}

		/// <summary>
		/// При заходе в данное меню можно сразу ввести ограничение на один чат либо поставить ограничение вручную!
		/// </summary>
		/// <param name="channel"></param>
		/// <returns></returns>
		public InlineKeyboardMarkup LimitedChannel(Channel channel)
		{
			List<List<InlineKeyboardButton>> list = new List<List<InlineKeyboardButton>>
			{
				new List<InlineKeyboardButton>()
			};
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "Нет ограничения",
				CallbackData = CommandText.NoLimitedPostChannel + " " + channel.IDChannel
			});
			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "🔙Назад🔙",
				CallbackData = CommandText.BackToSetting
			});
			return new InlineKeyboardMarkup(list);
		}

		public InlineKeyboardMarkup GetChat()
		{
			DataBase db = Singleton.GetInstance().Context;
			List<List<InlineKeyboardButton>> list = new List<List<InlineKeyboardButton>>();

			Channel[] channel = db.GetChannels();
			Settings settings = db.GetSettings();

			foreach (Channel channel1 in channel)
			{
				if (settings.ChannelAdmin != channel1.IDChannel)
				{
					list.Add(new List<InlineKeyboardButton>());
					list[list.Count - 1].Add(new InlineKeyboardButton()
					{
						Text = channel1.InviteLink,
						CallbackData = CommandText.SelectThisChannel + " " + channel1.IDChannel
					});
				}
			}
			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "🔙Назад🔙",
				CallbackData = CommandText.BackToSetting
			});

			return new InlineKeyboardMarkup(list);
		}
	}
}