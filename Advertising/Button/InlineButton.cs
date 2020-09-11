using System;
using System.Collections.Generic;
using System.Linq;
using BotCore.SQL;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace BotCore.Advertising
{
	internal static class InlineButton
	{
		public static InlineKeyboardMarkup CategoryChat(Int32 size)
		{
			DataBase db = Singleton.GetInstance().Context;

			List<List<InlineKeyboardButton>> button = new List<List<InlineKeyboardButton>>();

			Int32 i = 0;

			button.Add(new List<InlineKeyboardButton>());
			button[button.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "🔎Поиск по всем категориям", SwitchInlineQueryCurrentChat = ""
			});

			button.Add(new List<InlineKeyboardButton>());

			Int32 Count = size;
			if (db._categories.Count() != 0)
			{
				Category[] categories = db._categories.ToArray();

				for (Int32 j = size; j < categories.Length; j++)
				{
					if (size + 8 > j)
					{
						button[button.Count - 1].Add(item: new InlineKeyboardButton()
						{
							Text                         = categories[j].Name,
							SwitchInlineQueryCurrentChat = categories[j].Name
						});
					}
					else
					{
						break;
					}

					i++;

					if (i == 2)
					{
						button.Add(new List<InlineKeyboardButton>());
						i = 0;
					}
				}


				button.Add(new List<InlineKeyboardButton>());
				if (size - 8 >= 0)
				{
					button[button.Count - 1].Add(new InlineKeyboardButton()
					{
						Text         = "⬅",
						CallbackData = "/CategoryChat" + " " + (size - 8).ToString()
					});
				}

				if (size + 8 <= categories.Length)
				{
					button[button.Count - 1].Add(new InlineKeyboardButton()
					{
						Text         = "➡",
						CallbackData = "/CategoryChat" + " " + (size + 8).ToString()
					});
				}
			}

			button.Add(new List<InlineKeyboardButton>());
			button[button.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "🔙Назад🔙", CallbackData = CommandText.BackToAccauntMenu
			});

			return new InlineKeyboardMarkup(button);
		}

		public static InlineKeyboardMarkup ButtonOk()
		{
			List<List<InlineKeyboardButton>> list = new List<List<InlineKeyboardButton>>
			{
				new List<InlineKeyboardButton>()
			};
			//list[list.Count - 1].Add(new InlineKeyboardButton()
			//{
			//	Text = "Купить абонимент на месяц",
			//	CallbackData = CommandText.PayConfirm
			//});
			//list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton() {Text = "Ок", CallbackData = CommandsText.ButtonOk});
			return new InlineKeyboardMarkup(list);
		}

		public static InlineKeyboardMarkup AdminPanenLvl3Users()
		{
			DataBase db    = Singleton.GetInstance().Context;
			User[]   users = db.GetUsers();
			List<List<InlineKeyboardButton>> list = new List<List<InlineKeyboardButton>>
			{
				new List<InlineKeyboardButton>()
			};
			foreach (var user in users)
			{
				if (user.IsAdmin != 0)
				{
					list.Add(new List<InlineKeyboardButton>());
					list[list.Count - 1].Add(new InlineKeyboardButton()
					{
						Text         = $"{user.FIO}, {user.IsAdmin} лвл",
						CallbackData = CommandText.ChuseAdmin + " " + user.ID
					});
				}
			}


			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "🔙Назад🔙", CallbackData = CommandText.BackToSetting
			});
			return new InlineKeyboardMarkup(list);
		}

		public static InlineKeyboardMarkup AdminPanelLvl3(User user)
		{
			List<List<InlineKeyboardButton>> list = new List<List<InlineKeyboardButton>>
			{
				new List<InlineKeyboardButton>()
			};
			if (user.IsAdmin == 0)
			{
				list[list.Count - 1].Add(new InlineKeyboardButton()
				{
					Text         = "🔼Повысить до 1 уровня🔼",
					CallbackData = CommandText.UpInAdmin1lvl + " " + user.ID
				});
				list.Add(new List<InlineKeyboardButton>());
				list[list.Count - 1].Add(new InlineKeyboardButton()
				{
					Text         = "⏫Повысить до 2 уровня⏫",
					CallbackData = CommandText.UpInAdmin2lvl + " " + user.ID
				});
			}
			else if (user.IsAdmin == 1)
			{
				list.Add(new List<InlineKeyboardButton>());
				list[list.Count - 1].Add(new InlineKeyboardButton()
				{
					Text         = "🔽Понизить до 0 уровня🔽",
					CallbackData = CommandText.DownInAdmin0lvl + " " + user.ID
				});
				list.Add(new List<InlineKeyboardButton>());
				list[list.Count - 1].Add(new InlineKeyboardButton()
				{
					Text         = "🔼Повысить до 2 уровня🔼",
					CallbackData = CommandText.UpInAdmin2lvl + " " + user.ID
				});
			}
			else if (user.IsAdmin == 2)
			{
				list.Add(new List<InlineKeyboardButton>());
				list[list.Count - 1].Add(new InlineKeyboardButton()
				{
					Text         = "⏬Понизить до 0 уровня⏬",
					CallbackData = CommandText.DownInAdmin0lvl + " " + user.ID
				});
				list.Add(new List<InlineKeyboardButton>());
				list[list.Count - 1].Add(new InlineKeyboardButton()
				{
					Text         = "🔽Понизить до 1 уровня🔽",
					CallbackData = CommandText.UpInAdmin1lvl + " " + user.ID
				});
			}

			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "🔙Назад🔙", CallbackData = CommandText.BackToSetting
			});
			return new InlineKeyboardMarkup(list);
		}

		/// <summary>
		/// Главное меню рекламы
		/// </summary>
		public static InlineKeyboardMarkup AdvertisingShow = new InlineKeyboardButton[][]
		{
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton {Text = "💶Баланс💶", CallbackData = CommandsText.Balance,}
			},
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton {Text = "📞Реклама📞", CallbackData = CommandsText.SetAdvertising}
			},
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton {Text = "📇💬Чаты💬📇", CallbackData = CommandsText.GetChat,}
			},
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton {Text = "🔙Назад🔙", CallbackData = CommandsText.BackToMenu}
			}
		};

		public static InlineKeyboardMarkup AdvertisingShowAdmin = new InlineKeyboardButton[][]
		{
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton {Text = "💶Баланс💶", CallbackData = CommandsText.Balance,}
			},
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton {Text = "📞Реклама📞", CallbackData = CommandsText.SetAdvertising}
			},
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton {Text = "📇Чаты📇", CallbackData = CommandsText.GetChat}
			},
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton
				{
					Text = "⚙️Настройка рекламы⚙️", CallbackData = Advertising.CommandsText.AdminPanelPrice
				}
			},
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton {Text = "🔙Назад🔙", CallbackData = CommandsText.BackToMenu}
			}
		};

		public static InlineKeyboardMarkup Balance = new InlineKeyboardButton[][]
		{
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton {Text = "💶Пополнить счёт💶", CallbackData = CommandsText.PaymentCount}
			},
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton {Text = "🔙Назад🔙", CallbackData = CommandsText.BackToAdvertisingMenu}
			}
		};

		public static InlineKeyboardMarkup Payment = new InlineKeyboardButton[][]
		{
			new InlineKeyboardButton[] {InlineKeyboardButton.WithPayment("💵Оплатить!💵")},
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton {Text = "🔙Назад🔙", CallbackData = CommandsText.BackToAdvertisingMenu}
			}
		};

		public static InlineKeyboardMarkup BackToAdvertisingMenu = new InlineKeyboardButton[][]
		{
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton {Text = "🔙Назад🔙", CallbackData = CommandsText.BackToAdvertisingMenu}
			}
		};

		public static InlineKeyboardMarkup back = new InlineKeyboardButton[][]
		{
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton {Text = "🔙Назад🔙", CallbackData = CommandsText.BackToAdvertisingMenu}
			}
		};

		public static InlineKeyboardMarkup GetTemplateMenu(AdUser user)
		{
			DataBase db = Singleton.GetInstance().Context;

			List<InlineKeyboardButton[]> list = new List<InlineKeyboardButton[]>();

			PostTemplate template = db.GetTempalte(user.User.ID, user.EditingPostTemplateId);

			if (!template.IsOnValidation)
			{
				if (!template.IsValidated)
				{
					list.Add(new InlineKeyboardButton[]
					{
						new InlineKeyboardButton
						{
							Text         = "📝Контент📝",
							CallbackData = CommandsText.MenuAdContent + " " + template.Id
						}
					});

					list.Add(new InlineKeyboardButton[]
					{
						new InlineKeyboardButton
						{
							Text         = "🧾Тип сообщения🧾",
							CallbackData = CommandsText.CreateAdKeyboardTypeMessage + " " + template.Id
						}
					});

					list.Add(new InlineKeyboardButton[]
					{
						new InlineKeyboardButton
						{
							Text = "💬Чаты💬", CallbackData = CommandsText.SetChatInPostTemplate
						}
					});

					list.Add(new InlineKeyboardButton[]
					{
						new InlineKeyboardButton
						{
							Text         = "🆗Отправить на проверку🆗",
							CallbackData = CommandsText.MenuAdModeration
						}
					});
				}
				else if (template.IsPaid == false)
				{
					list.Add(new InlineKeyboardButton[]
					{
						new InlineKeyboardButton
						{
							Text         = "💵Оплатить!💵",
							Pay          = true,
							CallbackData = CommandsText.PayPostTemplate
						}
					});
				}
			}

			list.Add(new InlineKeyboardButton[]
			{
				new InlineKeyboardButton
				{
					Text = "🔙Назад🔙", CallbackData = CommandsText.BackSetAdvertising
				}
			});

			return new InlineKeyboardMarkup(list);
		}

		internal static void ButtonOk(Object p) => throw new NotImplementedException();

		public static InlineKeyboardMarkup GetAdvertising(User user)
		{
			DataBase                         db   = Singleton.GetInstance().Context;
			List<List<InlineKeyboardButton>> list = new List<List<InlineKeyboardButton>>();

			PostTemplate[] post  = db.GetPostTemplates();
			System.Int32   count = 1;
			foreach (PostTemplate post1 in post)
			{
				if (post1.AdUser.UserId == user.ID)
				{
					list.Add(new List<InlineKeyboardButton>());
					list[list.Count - 1].Add(new InlineKeyboardButton()
					{
						Text = post1.Name == "Шаблон"
							       ? post1.Name + " " + count.ToString()
							       : post1.Name,
						CallbackData = CommandsText.ShowPostTemplateData + " " + post1.Id
					});
					count++;
				}
			}

			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "🆕Добавить🆕", CallbackData = CommandsText.AddAdvertising
			});
			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "🔙Назад🔙", CallbackData = CommandsText.BackToAdvertisingMenu
			});

			return new InlineKeyboardMarkup(list);
		}

		public static InlineKeyboardMarkup GetChat()
		{
			DataBase                         db   = Singleton.GetInstance().Context;
			List<List<InlineKeyboardButton>> list = new List<List<InlineKeyboardButton>>();

			Channel[] channel  = db.GetChannels();
			Settings  settings = db.GetSettings();

			foreach (Channel channel1 in channel)
			{
				if (settings.ChannelAdmin != channel1.IDChannel)
				{
					list.Add(new List<InlineKeyboardButton>());
					list[list.Count - 1].Add(new InlineKeyboardButton()
					{
						Text         = channel1.InviteLink,
						CallbackData = channel1.IDChannel + " " + channel1.InviteLink
					});
				}
			}

			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "🔙Назад🔙", CallbackData = CommandsText.BackToAdvertisingMenu
			});

			return new InlineKeyboardMarkup(list);
		}

		public static InlineKeyboardMarkup GetChatInPosting(PostTemplate postTemplate)
		{
			DataBase                         db   = Singleton.GetInstance().Context;
			List<List<InlineKeyboardButton>> list = new List<List<InlineKeyboardButton>>();

			Channel[] channel  = db.GetChannels();
			Settings  settings = db.GetSettings();

			foreach (Channel channel1 in channel)
			{
				if (channel1.IDChannel != settings.ChannelAdmin)
				{
					System.String name = channel1.InviteLink;
					list.Add(new List<InlineKeyboardButton>());

					if (postTemplate?.PostChannel.Where(p => p.Channel.IDChannel == channel1.IDChannel).Count() > 0)
					{
						name += "✅";
					}

					list[list.Count - 1].Add(new InlineKeyboardButton()
					{
						Text = name,
						CallbackData =
							CommandsText.SelectChatInPostTemplate + " " +
							channel1.IDChannel
					});
				}
			}

			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text         = "🔙Назад🔙",
				CallbackData = CommandsText.BackToAddAdversting + " " + postTemplate.Id
			});

			return new InlineKeyboardMarkup(list);
		}

		public static InlineKeyboardMarkup ThisChannel(System.Int64 id)
		{
			DataBase db = Singleton.GetInstance().Context;
			List<List<InlineKeyboardButton>> list = new List<List<InlineKeyboardButton>>
			{
				new List<InlineKeyboardButton>()
			};

			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text         = "📌Заказать рекламу📌",
				CallbackData = CommandsText.SetAdvertisingOneChannel + " " + id
			});
			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "🔙Назад🔙", CallbackData = CommandsText.BackToChannelMenu
			});
			return new InlineKeyboardMarkup(list);
		}

		public static InlineKeyboardMarkup CreateAdKeyboard = new InlineKeyboardButton[][]
		{
			new[] {new InlineKeyboardButton {Text = "📝Контент📝", CallbackData = CommandsText.MenuAdContent}},
			new[]
			{
				new InlineKeyboardButton
				{
					Text = "🧾Тип сообщения🧾", CallbackData = CommandsText.CreateAdKeyboardTypeMessage
				}
			},
			new[] {new InlineKeyboardButton {Text = "💬Чаты💬", CallbackData  = CommandsText.SetChatInPostTemplate}},
			new[] {new InlineKeyboardButton {Text = "🔙Назад🔙", CallbackData = CommandsText.BackSetAdvertising}}
		};

		public static InlineKeyboardMarkup CreateAdKeyboardTypeMessage(PostTemplate postTemplate)
		{
			List<List<InlineKeyboardButton>> list = new List<List<InlineKeyboardButton>>
			{
				new List<InlineKeyboardButton>()
			};
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "🔊Стандартные сообщения🔊", CallbackData = CommandsText.Calendar
			});
			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "🔕Закрепленные сообщения🔕",
				CallbackData =
					CommandsText.CalendarPinned + " " +
					(System.Int32)TypePostTime.PinnedMessage
			});
			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "🔔Закрепленные сообщения с уведомлением🔔",
				CallbackData =
					CommandsText.CalendarPinned + " " +
					(System.Int32)TypePostTime.PinnedMessageNotification
			});
			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text         = "🔙Назад🔙",
				CallbackData = CommandsText.BackToAddAdversting + " " + postTemplate.Id
			});
			return new InlineKeyboardMarkup(list);
		}

		public static InlineKeyboardMarkup AdValidationKeyboard = new InlineKeyboardButton[][]
		{
			new[]
			{
				new InlineKeyboardButton
				{
					Text = "✅Подтвердить✅", CallbackData = CommandsText.SendToValidationConfirmed
				}
			},
			new[] {new InlineKeyboardButton {Text = "🔙Назад🔙", CallbackData = CommandsText.BackToAdTemplate}}
		};

		public static InlineKeyboardMarkup ContentKeyboard(PostTemplate postTemplate,
			System.Boolean                                              showContent = false)
		{
			List<List<InlineKeyboardButton>> list = new List<List<InlineKeyboardButton>>();
			DataBase                         db   = Singleton.GetInstance().Context;

			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "K1",
				CallbackData =
					CommandsText.ContentEditorImg + " " + 1 + " " + postTemplate.Id
			});
			list.Add(new List<InlineKeyboardButton>());
			for (System.Int32 i = 2; i <= 4; i++)
			{
				list[list.Count - 1].Add(new InlineKeyboardButton()
				{
					Text = "K" + i.ToString(),
					CallbackData =
						CommandsText.ContentEditorImg + " " + i.ToString() + " " +
						postTemplate.Id
				});
			}

			list.Add(new List<InlineKeyboardButton>());
			for (System.Int32 i = 5; i <= 7; i++)
			{
				list[list.Count - 1].Add(new InlineKeyboardButton()
				{
					Text = "K" + i.ToString(),
					CallbackData =
						CommandsText.ContentEditorImg + " " + i.ToString() + " " +
						postTemplate.Id
				});
			}

			list.Add(new List<InlineKeyboardButton>());
			for (System.Int32 i = 8; i <= 10; i++)
			{
				list[list.Count - 1].Add(new InlineKeyboardButton()
				{
					Text = "K" + i.ToString(),
					CallbackData =
						CommandsText.ContentEditorImg + " " + i.ToString() + " " +
						postTemplate.Id
				});
			}

			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text         = "📝Текст📝",
				CallbackData = CommandsText.ContentEditorText + " " + postTemplate.Id
			});

			if (showContent == true)
			{
				list.Add(new List<InlineKeyboardButton>());
				list[list.Count - 1].Add(new InlineKeyboardButton()
				{
					Text = "👁‍🗨Посмотреть👁‍🗨", CallbackData = CommandsText.ShowContent
				});
			}

			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "❌Удалить❌", CallbackData = CommandsText.DeletePost
			});
			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text         = "🔙Назад🔙",
				CallbackData = CommandsText.BackToAddAdversting + " " + postTemplate.Id
			});

			return new InlineKeyboardMarkup(list);
		}

		public static InlineKeyboardMarkup BackToContentKeyboard(PostTemplate postTemplate)
		{
			List<List<InlineKeyboardButton>> list = new List<List<InlineKeyboardButton>>
			{
				new List<InlineKeyboardButton>()
			};
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text         = "🔙Назад🔙",
				CallbackData = CommandsText.BackToContentKeyboard + " " + postTemplate.Id
			});

			return new InlineKeyboardMarkup(list);
		}

		public static InlineKeyboardMarkup AdminPostValidation(System.Object message, System.Int32 AdUserId,
			System.Int32                                                     postTemplateId)
		{
			Message _message = message as Message;
			List<InlineKeyboardButton> list = new List<InlineKeyboardButton>
			{
				new InlineKeyboardButton()
				{
					Text         = "✅Одобрить✅",
					CallbackData = CommandsText.AdminAccept + " " + AdUserId + " " + postTemplateId
				},
				new InlineKeyboardButton()
				{
					Text         = "❌Отказать❌",
					CallbackData = CommandsText.AdminCancel + " " + AdUserId + " " + postTemplateId
				}
			};

			return new InlineKeyboardMarkup(list);
		}

		public static InlineKeyboardMarkup AddPhoto(PostTemplate postTemplate)
		{
			List<List<InlineKeyboardButton>> list = new List<List<InlineKeyboardButton>>
			{
				new List<InlineKeyboardButton>()
			};
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "❌Удалить❌", CallbackData = CommandsText.DeletePhoto
			});
			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "⭕️Изменить⭕️", CallbackData = CommandsText.ChangePhoto
			});
			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text         = "🔙Назад🔙",
				CallbackData = CommandsText.BackToContentKeyboard + " " + postTemplate.Id
			});

			return new InlineKeyboardMarkup(list);
		}

		#region WorkTime

		public static List<List<InlineKeyboardButton>> AddBack(List<List<InlineKeyboardButton>> list,
			System.String                                                                       command)
		{
			list[list.Count - 1].Add(new InlineKeyboardButton() {Text = "<", CallbackData = command});
			return list;
		}

		public static List<List<InlineKeyboardButton>> AddNext(List<List<InlineKeyboardButton>> list,
			System.String                                                                       command)
		{
			list[list.Count - 1].Add(new InlineKeyboardButton() {Text = ">", CallbackData = command});
			return list;
		}

		#endregion WorkTime

		#region Price

		public static InlineKeyboardMarkup OptionAdvertising = new InlineKeyboardButton[][]
		{
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton {Text = "💬Чаты💬", CallbackData = CommandsText.ShowPriceChat}
			},
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton {Text = "💵Общая цена💵", CallbackData = CommandsText.ShowStaticPrices}
			},
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton {Text = "⏱Цена за время⏱", CallbackData = CommandsText.StaticPriceTime}
			},
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton {Text = "🔙Назад🔙", CallbackData = CommandsText.BackToAdvertisingMenu}
			}
		};

		public static InlineKeyboardMarkup OptionAdvertisingShowStaticChats = new InlineKeyboardButton[][]
		{
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton
				{
					Text = "💰Общая цена за все чаты💰", CallbackData = CommandsText.StaticPriceChat
				}
			},
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton
				{
					Text         = "💸Общая цена за стандартные сообщения💸",
					CallbackData = CommandsText.StaticPriceStandartMessage
				}
			},
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton
				{
					Text         = "💵Общая цена за закрепленные сообщения💵",
					CallbackData = CommandsText.StaticPricePinned
				}
			},
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton
				{
					Text         = "💶Общая цена за закрепленные сообщения с уведомлением💶",
					CallbackData = CommandsText.StaticPricePinnedNotification
				}
			},
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton {Text = "🔙Назад🔙", CallbackData = CommandsText.BackInShowMenuPrice}
			}
		};

		//LeftChangeStaticPinned_Price
		public static InlineKeyboardMarkup OptionAdvertisingShowChats()
		{
			DataBase                         db   = Singleton.GetInstance().Context;
			List<List<InlineKeyboardButton>> list = new List<List<InlineKeyboardButton>>();

			Channel[] channel = db.GetChannels();
			list.Add(new List<InlineKeyboardButton>());
			foreach (Channel channel1 in channel)
			{
				list.Add(new List<InlineKeyboardButton>());
				list[list.Count - 1].Add(new InlineKeyboardButton()
				{
					Text         = channel1.InviteLink          + $"\nЦена: {channel1.Price}",
					CallbackData = CommandsText.PriceChat + " " + channel1.IDChannel
				});
			}

			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "🔙Назад🔙", CallbackData = CommandsText.BackInShowMenuPrice
			});

			return new InlineKeyboardMarkup(list);
		}

		public static InlineKeyboardMarkup ChangePriceStaticChat(System.Single price)
		{
			List<List<InlineKeyboardButton>> list = new List<List<InlineKeyboardButton>>
			{
				new List<InlineKeyboardButton>()
			};
			list = InlineButton.AddBack(list, CommandsText.LeftStaticChange_PriceChat + " " + price);
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text         = price.ToString(),
				CallbackData = CommandsText.BackInShowStaticPriceChat
			});
			list = InlineButton.AddNext(list, CommandsText.RightStaticChange_PriceChat + " " + price);
			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text         = "🔙Назад🔙",
				CallbackData = CommandsText.BackOfChangePriceChatInMenu + " " + price
			});
			return new InlineKeyboardMarkup(list);
		}

		public static InlineKeyboardMarkup ChangePriceChat(System.Int64 chatId, System.Single price)
		{
			List<List<InlineKeyboardButton>> list = new List<List<InlineKeyboardButton>>
			{
				new List<InlineKeyboardButton>()
			};
			list = InlineButton.AddBack(list, CommandsText.LeftChange_PriceChate + " " + price + " " + chatId);
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = price.ToString(), CallbackData = CommandsText.BackInShowChatPrice
			});
			list = InlineButton.AddNext(list, CommandsText.RightChange_PriceChat + " " + price + " " + chatId);
			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "🔙Назад🔙",
				CallbackData =
					CommandsText.BackOfChangePriceChatInShowChatPrice + " " + price
			});
			return new InlineKeyboardMarkup(list);
		}

		public static InlineKeyboardMarkup ChangePriceStandartMessage(System.Single price)
		{
			List<List<InlineKeyboardButton>> list = new List<List<InlineKeyboardButton>>
			{
				new List<InlineKeyboardButton>()
			};
			list = InlineButton.AddBack(list, CommandsText.LeftStaticChangeStandartMessage + " " + price);
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = price.ToString(), CallbackData = CommandsText.BackInShowChatPrice
			});
			list = InlineButton.AddNext(list, CommandsText.RightStaticChangeStandartMessage + " " + price);
			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text         = "🔙Назад🔙",
				CallbackData = CommandsText.BackOfChangePriceStaticTimeInMenu + " " + price
			});
			return new InlineKeyboardMarkup(list);
		}

		public static InlineKeyboardMarkup ChangePriceTime(System.Single price)
		{
			List<List<InlineKeyboardButton>> list = new List<List<InlineKeyboardButton>>
			{
				new List<InlineKeyboardButton>()
			};
			list = InlineButton.AddBack(list, CommandsText.LeftStaticChange_PriceTime + " " + price);
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = price.ToString(), CallbackData = CommandsText.BackInShowChatPrice
			});
			list = InlineButton.AddNext(list, CommandsText.RightStaticChange_PriceTime + " " + price);
			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text         = "🔙Назад🔙",
				CallbackData = CommandsText.BackOfChangePriceStaticTimeInMenu + " " + price
			});
			return new InlineKeyboardMarkup(list);
		}

		public static InlineKeyboardMarkup ChangePricePinned(System.Single price)
		{
			List<List<InlineKeyboardButton>> list = new List<List<InlineKeyboardButton>>
			{
				new List<InlineKeyboardButton>()
			};
			list = InlineButton.AddBack(list, CommandsText.LeftChangeStaticPinned_Price + " " + price);
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = price.ToString(), CallbackData = CommandsText.BackInShowChatPrice
			});
			list = InlineButton.AddNext(list, CommandsText.RightChangeStaticPinned_Price + " " + price);
			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text         = "🔙Назад🔙",
				CallbackData = CommandsText.BackOfChangePricePinnedInMenu + " " + price
			});
			return new InlineKeyboardMarkup(list);
		}

		public static InlineKeyboardMarkup ChangePricePinnedNotification(System.Single price)
		{
			List<List<InlineKeyboardButton>> list = new List<List<InlineKeyboardButton>>
			{
				new List<InlineKeyboardButton>()
			};
			list = InlineButton.AddBack(list, CommandsText.LeftChangeStaticPinnedNotification_Price + " " + price);
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = price.ToString(), CallbackData = CommandsText.BackInShowChatPrice
			});
			list = InlineButton.AddNext(list, CommandsText.RightChangeStaticPinnedNotification_Price + " " + price);
			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "🔙Назад🔙",
				CallbackData =
					CommandsText.BackOfChangePricePinnedNotificationInMenu + " " + price
			});
			return new InlineKeyboardMarkup(list);
		}

		#endregion Price

		#region AddTime

		public static InlineKeyboardMarkup TimeShow(System.Int32 idUser, System.String postName, System.Object posttime,
			PostTemplate                                         postTemplate)
		{
			//  DataBase db = Singleton.GetInstance().Context;

			List<List<InlineKeyboardButton>> list      = new List<List<InlineKeyboardButton>>();
			List<PostTime>                   postTime  = posttime as List<PostTime>;
			List<PostTime>                   temp_time = new List<PostTime>();

			for (System.Int32 i = 0; i < postTime.Count; i++)
			{
				if (postTime.Count != 0)
				{
					if (postTime[i].UseTime != false)
					{
						temp_time.Add(postTime[i]);
					}
				}
			}

			if (temp_time.Count > 0)
			{
				for (System.Int32 i = 0; i < temp_time.Count; i++)
				{
					list.Add(new List<InlineKeyboardButton>());
					list[list.Count - 1].Add(new InlineKeyboardButton()
					{
						Text = temp_time[i].Time.TimeOfDay.ToString(),
						CallbackData =
							CommandsText.ChangeTimeButton          + " " +
							temp_time[i].Time.TimeOfDay.ToString() + " " +
							temp_time[i].IdDateTime
					});
				}
			}

			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1]
				.Add(new InlineKeyboardButton() {Text = "🕒Добавить🕒", CallbackData = CommandsText.AddTimeButton});
			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text         = "📟Управление шаблоном📟",
				CallbackData = CommandsText.BackToAddAdversting + " " + postTemplate.Id
			});
			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1]
				.Add(new InlineKeyboardButton() {Text = "🔙Назад🔙", CallbackData = CommandsText.ChangeAllTime});
			return new InlineKeyboardMarkup(list);
		}

		public static InlineKeyboardMarkup TimeShowChange(System.Int32 idUser, System.String postName,
			System.Object                                              posttime)
		{
			//  DataBase db = Singleton.GetInstance().Context;

			List<List<InlineKeyboardButton>> list     = new List<List<InlineKeyboardButton>>();
			List<PostTime>                   postTime = posttime as List<PostTime>;
			// = db.GetPostTimesCollection(db.GetAdUser(idUser), postName);
			if (postTime.Count > 0)
			{
				for (System.Int32 i = 0; i < postTime.Count; i++)
				{
					list.Add(new List<InlineKeyboardButton>());
					list[list.Count - 1].Add(new InlineKeyboardButton()
					{
						Text = postTime[i].Time.TimeOfDay.ToString(),
						CallbackData =
							CommandsText.ChangeTimeButton         + " " +
							postTime[i].Time.TimeOfDay.ToString() + " " +
							postTime[i].IdDateTime
					});
				}

				list.Add(new List<InlineKeyboardButton>());
				list[list.Count - 1]
					.Add(new InlineKeyboardButton() {Text = "‼️Сохранить‼️", CallbackData = CommandsText.ChangeTime});
			}

			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1]
				.Add(new InlineKeyboardButton() {Text = "🆕Добавить🆕", CallbackData = CommandsText.AddTimeButton});
			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1]
				.Add(new InlineKeyboardButton() {Text = "🔙Назад🔙", CallbackData = CommandsText.ChangeAllTime});
			return new InlineKeyboardMarkup(list);
		}

		public static InlineKeyboardMarkup AddTime(System.Int32 idUser, System.String postName, PostTime time)
		{
			List<List<InlineKeyboardButton>> list = new List<List<InlineKeyboardButton>>
			{
				//PostTime time =null;
				// time = BotCore.WorkingTime.WorkingTimeBot.users_time[idUser].timeModels.Where(p => p.Use == true).FirstOrDefault().TempTime;
				new List<InlineKeyboardButton>()
			};
			list = InlineButton.AddBack(list,
			                            CommandsText.LeftHourButton + " " + time.Time.TimeOfDay.ToString() + " " +
			                            time.IdDateTime);
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = time.Time.TimeOfDay.Hours.ToString(),
				CallbackData =
					CommandsText.Back_AddTime + " " + time.Time.TimeOfDay.ToString() +
					" "                       + time.IdDateTime
			});
			list = InlineButton.AddNext(list,
			                            CommandsText.RightHourButton + " " + time.Time.TimeOfDay.ToString() + " " +
			                            time.IdDateTime);

			list.Add(new List<InlineKeyboardButton>());
			list = InlineButton.AddBack(list,
			                            CommandsText.LeftMinuteButton + " " + time.Time.TimeOfDay.ToString() + " " +
			                            time.IdDateTime);
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = time.Time.TimeOfDay.Minutes.ToString(),
				CallbackData =
					CommandsText.ChangeTimeButton + " " + time.Time.TimeOfDay.ToString()
			});
			list = InlineButton.AddNext(list,
			                            CommandsText.RightMinuteButton + " " + time.Time.TimeOfDay.ToString() + " " +
			                            time.IdDateTime);
			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "🔙Назад🔙",
				CallbackData =
					CommandsText.Back_AddTime + " " + time.Time.TimeOfDay.ToString() +
					" "                       + time.IdDateTime
			});

			return new InlineKeyboardMarkup(list);
		}

		#endregion AddTime

		#region ChangeTime

		public static InlineKeyboardMarkup ChangeTimeShow(System.Int32 idUser, System.String postName, PostTime time)
		{
			List<List<InlineKeyboardButton>> list = new List<List<InlineKeyboardButton>>
			{
				new List<InlineKeyboardButton>()
			};
			list = InlineButton.AddBack(list,
			                            CommandsText.LeftHourButton_Change + " " + time.Time.TimeOfDay.ToString() +
			                            " "                                + time.IdDateTime);
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = time.Time.TimeOfDay.Hours.ToString(),
				CallbackData =
					CommandsText.Back_AddTime + " " + time.Time.TimeOfDay.ToString() +
					" "                       + time.IdDateTime
			});
			list = InlineButton.AddNext(list,
			                            CommandsText.RightHourButton_Change + " " + time.Time.TimeOfDay.ToString() +
			                            " "                                 + time.IdDateTime);

			list.Add(new List<InlineKeyboardButton>());
			list = InlineButton.AddBack(list,
			                            CommandsText.LeftMinuteButton_Change + " " + time.Time.TimeOfDay.ToString() +
			                            " "                                  + time.IdDateTime);
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = time.Time.TimeOfDay.Minutes.ToString(),
				CallbackData =
					CommandsText.Back_AddTime + " " + time.Time.TimeOfDay.ToString() +
					" "                       + time.IdDateTime
			});
			list = InlineButton.AddNext(list,
			                            CommandsText.RightMinuteButton_Change + " " + time.Time.TimeOfDay.ToString() +
			                            " "                                   + time.IdDateTime);
			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "❌Удалить❌",
				CallbackData =
					CommandsText.DeleteTime + " " + time.Time.TimeOfDay.ToString() + " " +
					time.IdDateTime
			});
			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "🔙Назад🔙",
				CallbackData =
					CommandsText.Back_AddTime + " " + time.Time.TimeOfDay.ToString() +
					" "                       + time.IdDateTime
			});

			return new InlineKeyboardMarkup(list);
		}

		#endregion ChangeTime

		#region Calendar

		private static List<List<InlineKeyboardButton>> AddNameDay(List<List<InlineKeyboardButton>> list)
		{
			list[list.Count - 1]
				.Add(new InlineKeyboardButton() {Text = "Пн", CallbackData = CommandText.BackToAccauntMenu});
			list[list.Count - 1]
				.Add(new InlineKeyboardButton() {Text = "Вт", CallbackData = CommandText.BackToAccauntMenu});
			list[list.Count - 1]
				.Add(new InlineKeyboardButton() {Text = "Ср", CallbackData = CommandText.BackToAccauntMenu});
			list[list.Count - 1]
				.Add(new InlineKeyboardButton() {Text = "Чт", CallbackData = CommandText.BackToAccauntMenu});
			list[list.Count - 1]
				.Add(new InlineKeyboardButton() {Text = "Пт", CallbackData = CommandText.BackToAccauntMenu});
			list[list.Count - 1]
				.Add(new InlineKeyboardButton() {Text = "Сб", CallbackData = CommandText.BackToAccauntMenu});
			list[list.Count - 1]
				.Add(new InlineKeyboardButton() {Text = "Вс", CallbackData = CommandText.BackToAccauntMenu});

			return list;
		}

		private static List<List<InlineKeyboardButton>> AddNameMouth(List<List<InlineKeyboardButton>> list,
			Calendar.Calendar                                                                         calendar)
		{
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text         = calendar.NameCalendar(calendar.date),
				CallbackData = CommandText.BackToAccauntMenu
			});
			return list;
		}

		private static List<List<InlineKeyboardButton>> AddBack(List<List<InlineKeyboardButton>> list)
		{
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = "<", CallbackData = CommandsText.Back_Calendar
			});
			return list;
		}

		private static List<List<InlineKeyboardButton>> AddNext(List<List<InlineKeyboardButton>> list)
		{
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text = ">", CallbackData = CommandsText.Calendar_Next
			});
			return list;
		}
		
		//выделение динамических кнопок
		public static InlineKeyboardMarkup CalendarShow(List<PostTime> postTime, System.Int32 id, Calendar.Calendar cal, System.Int32 postTemplateID) 
		{
			List<List<InlineKeyboardButton>> list = new List<List<InlineKeyboardButton>>();

			//   CommandText.MyData.Clear();
			Calendar.Calendar calendar = new Calendar.Calendar();

			calendar.date = new System.DateTime(calendar.year, calendar.month, 1);
			//   System.Console.WriteLine(calendar.date);

			list.Add(new List<InlineKeyboardButton>());
			list = AddNameMouth(list, calendar);
			list.Add(new List<InlineKeyboardButton>());
			list = AddNameDay(list);

			calendar.FillCalendar();

			list.Add(new List<InlineKeyboardButton>());
			System.DateTime date = System.DateTime.Today;
			for (System.Int32 i = 0; i < calendar.calendar.GetLength(0); i++)
			{
				for (System.Int32 j = 0; j < calendar.calendar.GetLength(1); j++)
				{
					if (calendar.calendar[i, j] < date.Day && calendar.month == date.Month &&
					    calendar.year           == date.Year)
					{
						calendar.calendar[i, j] = 0;
					}
				}
			}

			for (System.Int32 i = 0; i < calendar.calendar.GetLength(0); i++)
			{
				for (System.Int32 j = 0; j < calendar.calendar.GetLength(1); j++)
				{
					if (calendar.calendar[i, j] > 0)
					{
						if (calendar.month          == System.DateTime.Today.Month &&
						    calendar.calendar[i, j] == System.DateTime.Today.Day   &&
						    calendar.year           == System.DateTime.Today.Year)
						{
							list[list.Count - 1].Add(new InlineKeyboardButton()
							{
								Text = "[ " + calendar.calendar[i, j].ToString() + " ]",
								CallbackData =
									(calendar.calendar[i, j] + "." + calendar.month + "." +
									 calendar.year)
							});
						}
						else
						{
							list[list.Count - 1].Add(new InlineKeyboardButton()
							{
								Text = calendar.calendar[i, j].ToString(),
								CallbackData =
									(calendar.calendar[i, j] + "." + calendar.month + "." +
									 calendar.year)
							});
						}
					}
					else
					{
						list[list.Count - 1]
							.Add(new InlineKeyboardButton() {Text = " ", CallbackData = CommandText.BackToAccauntMenu});
					}
				}

				list.Add(new List<InlineKeyboardButton>());
			}

			if (postTime.Count > 0)
			{
				list.Add(new List<InlineKeyboardButton>());
				list[list.Count - 1].Add(new InlineKeyboardButton()
				{
					Text = "❇Далее❇", CallbackData = CommandsText.NextToTimeMenu_Calendar
				});
			}

			list.Add(new List<InlineKeyboardButton>());
			list = AddBack(list);
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text         = "🔙Назад🔙",
				CallbackData = CommandsText.BackToTypeMessage + " " + postTemplateID
			});
			list = AddNext(list);
			// list.Add(new List<InlineKeyboardButton>());

			Bot.users_calendar[id] = calendar;
			return new InlineKeyboardMarkup(list);
		}

		public static InlineKeyboardMarkup CalendarShowPinnedMessage(List<PostTime> postTime,
			System.Int32                                                            dateChange,
			System.Int32                                                            id, Calendar.Calendar cal,
			System.Int32                                                            postTemplateID) //выделение динамических кнопок
		{
			List<List<InlineKeyboardButton>> list     = new List<List<InlineKeyboardButton>>();
			Calendar.Calendar                calendar = cal;
			//   CommandText.MyData.Clear();
			DataBase db = Singleton.GetInstance().Context;
			calendar.date = new System.DateTime(calendar.year, calendar.month, 1);
			//   System.Console.WriteLine(calendar.date);

			list.Add(new List<InlineKeyboardButton>());
			list = AddNameMouth(list, calendar);
			list.Add(new List<InlineKeyboardButton>());
			list = AddNameDay(list);

			calendar.FillCalendar();
			System.DateTime date = System.DateTime.Today;
			list.Add(new List<InlineKeyboardButton>());
			System.Int32 day = System.DateTime.Today.Day;
			for (System.Int32 i = 0; i < calendar.calendar.GetLength(0); i++)
			{
				for (System.Int32 j = 0; j < calendar.calendar.GetLength(1); j++)
				{
					if (calendar.calendar[i, j] < date.Day && calendar.month == date.Month &&
					    calendar.year           == date.Year)
					{
						calendar.calendar[i, j] = 0;
					}

					if (db._postTemplates.Any(p => p.IsPaid == true))
					{
						List<PostTemplate> template = db._postTemplates.Where(p => p.IsPaid == true).ToList();

						for (System.Int32 a = 0; a < template.Count; a++)
						{
							if (template[a]
							    .PostTime.Any(p => p.Time.Date.Day == calendar.calendar[i, j] &&
							                       calendar.month  == date.Month              &&
							                       calendar.year   == date.Year))
							{
								calendar.calendar[i, j] = 0;
							}
						}
					}
				}
			}

			for (System.Int32 i = 0; i < calendar.calendar.GetLength(0); i++)
			{
				for (System.Int32 j = 0; j < calendar.calendar.GetLength(1); j++)
				{
					if (calendar.calendar[i, j] > 0)
					{
						if (postTime.Any(p => p.Time.Day   == calendar.calendar[i, j] &&
						                      p.Time.Month == calendar.month          &&
						                      p.Time.Year  == calendar.year))
						{
							list[list.Count - 1].Add(new InlineKeyboardButton()
							{
								Text = calendar.calendar[i, j].ToString() + "✅",
								CallbackData =
									CommandsText.ChooseDatePinned + " " +
									(calendar.calendar[i, j] + "." + calendar.month + "." +
									 calendar.year           + "+")
							});
						}
						else
						{
							list[list.Count - 1].Add(new InlineKeyboardButton()
							{
								Text = calendar.calendar[i, j].ToString(),
								CallbackData =
									CommandsText.ChooseDatePinned + " " +
									(calendar.calendar[i, j] + "." + calendar.month + "." +
									 calendar.year)
							});
						}
					}
					else
					{
						list[list.Count - 1]
							.Add(new InlineKeyboardButton() {Text = " ", CallbackData = CommandsText.BackToCalendar});
					}
				}

				list.Add(new List<InlineKeyboardButton>());
			}

			list.Add(new List<InlineKeyboardButton>());
			list = AddBack(list);
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text         = "🔙Назад🔙",
				CallbackData = CommandsText.BackToTypeMessage + " " + postTemplateID
			});
			list = AddNext(list);

			Bot.users_calendar[id] = calendar;
			return new InlineKeyboardMarkup(list);
		}

		public static InlineKeyboardMarkup CalendarShow(List<PostTime> postTime, System.Int32      dateChange,
			System.Int32                                               id,       Calendar.Calendar cal,
			System.Int32                                               postTemplateID) //выделение динамических кнопок
		{
			List<List<InlineKeyboardButton>> list     = new List<List<InlineKeyboardButton>>();
			Calendar.Calendar                calendar = cal;
			//   CommandText.MyData.Clear();

			calendar.date = new System.DateTime(calendar.year, calendar.month, 1);
			//   System.Console.WriteLine(calendar.date);

			list.Add(new List<InlineKeyboardButton>());
			list = AddNameMouth(list, calendar);
			list.Add(new List<InlineKeyboardButton>());
			list = AddNameDay(list);

			calendar.FillCalendar();
			System.DateTime date = System.DateTime.Today;
			list.Add(new List<InlineKeyboardButton>());
			System.Int32 day = System.DateTime.Today.Day;
			for (System.Int32 i = 0; i < calendar.calendar.GetLength(0); i++)
			{
				for (System.Int32 j = 0; j < calendar.calendar.GetLength(1); j++)
				{
					if (calendar.calendar[i, j] < date.Day && calendar.month == date.Month &&
					    calendar.year           == date.Year)
					{
						calendar.calendar[i, j] = 0;
					}
				}
			}

			for (System.Int32 i = 0; i < calendar.calendar.GetLength(0); i++)
			{
				for (System.Int32 j = 0; j < calendar.calendar.GetLength(1); j++)
				{
					if (calendar.calendar[i, j] > 0)
					{
						if (postTime.Any(p => p.Time.Day   == calendar.calendar[i, j] &&
						                      p.Time.Month == calendar.month          &&
						                      p.Time.Year  == calendar.year))
						{
							list[list.Count - 1].Add(new InlineKeyboardButton()
							{
								Text = calendar.calendar[i, j].ToString() + "✅",
								CallbackData =
									(calendar.calendar[i, j] + "." + calendar.month + "." +
									 calendar.year           + "+")
							});
						}
						else
						{
							list[list.Count - 1].Add(new InlineKeyboardButton()
							{
								Text = calendar.calendar[i, j].ToString(),
								CallbackData =
									(calendar.calendar[i, j] + "." + calendar.month + "." +
									 calendar.year)
							});
						}
					}
					else
					{
						list[list.Count - 1]
							.Add(new InlineKeyboardButton() {Text = " ", CallbackData = CommandsText.BackToCalendar});
					}
				}

				list.Add(new List<InlineKeyboardButton>());
			}

			if (postTime.Count > 0)
			{
				list.Add(new List<InlineKeyboardButton>());
				list[list.Count - 1].Add(new InlineKeyboardButton()
				{
					Text = "❇️Далее❇️", CallbackData = CommandsText.NextToTimeMenu_Calendar
				});
			}

			list.Add(new List<InlineKeyboardButton>());
			list = AddBack(list);
			list[list.Count - 1].Add(new InlineKeyboardButton()
			{
				Text         = "🔙Назад🔙",
				CallbackData = CommandsText.BackToTypeMessage + " " + postTemplateID
			});
			list = AddNext(list);

			Bot.users_calendar[id] = calendar;
			return new InlineKeyboardMarkup(list);
		}

		#endregion Calendar
	}
}