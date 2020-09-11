using System;
using System.Collections.Generic;

using BotCore.SQL;

using Telegram.Bot.Types.ReplyMarkups;

namespace BotCore.Blockchain
{
	internal static class InlineButtonBlockchain
	{
		public static InlineKeyboardMarkup BackToGuarantorMeinMenu = new InlineKeyboardButton[][]
		{
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton { Text = "🔙Назад🔙", CallbackData = CommandTextBlockchain.BackToGuarantorMeinMenu }
			}
		};


		/// <summary>
		/// Главное меню Гаранта, кнопка "✅Подтверждения запроса✅" отоброжается автоматически
		/// если есть хотя бы один запрос на создание танзакции
		/// </summary>
		/// <returns></returns>
		public static InlineKeyboardMarkup GuarantorMeinMenu(User user)
		{
			List<List<InlineKeyboardButton>> list = new List<List<InlineKeyboardButton>>
			{
				new List<InlineKeyboardButton>()
			};

			list[list.Count - 1].Add(new InlineKeyboardButton() { Text = "⚖Создать транзакцию⚖", CallbackData = CommandTextBlockchain.TransactionCreationGuarantor });
			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton() { Text = "💼Мои транзакции💼", CallbackData = CommandTextBlockchain.ShowMyTransaction });
			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton() { Text = "🏆Репутация🏆", CallbackData = CommandTextBlockchain.ReputationUser });
			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton() { Text = "⭐️Связь с администратором⭐️", CallbackData = CommandTextBlockchain.AdminCallBlockchain });
			if (user.IsAdmin >= 2)
			{
				list.Add(new List<InlineKeyboardButton>());
				list[list.Count - 1].Add(new InlineKeyboardButton() { Text = "⚙Настройки⚙", CallbackData = CommandTextBlockchain.SettingsAdminInBlockChain });
			}
			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton() { Text = "🔙Назад🔙", CallbackData = CommandTextBlockchain.BackToMeinMenu });

			return new InlineKeyboardMarkup(list);
		}

		/// <summary>
		/// Нострайки процента комисси гаранта
		/// </summary>
		/// <returns></returns>
		public static InlineKeyboardMarkup SettingsAdminInBlockChain()
		{
			List<List<InlineKeyboardButton>> list = new List<List<InlineKeyboardButton>>
			{
				new List<InlineKeyboardButton>()
			};
			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton() { Text = "Комиссия за криптовалюту", CallbackData = CommandTextBlockchain.SettingsCommisionCripta });
			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton() { Text = "Комиссия за вызов админа", CallbackData = CommandTextBlockchain.SettingsCommisionAdmin });
			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton() { Text = "🔙Назад🔙", CallbackData = CommandTextBlockchain.BackToGuarantorMeinMenu });

			return new InlineKeyboardMarkup(list);
		}//SettingsCommisionAdmin
		public static InlineKeyboardMarkup SettingsCommisionCripta(Decimal value)
		{
			List<List<InlineKeyboardButton>> list = new List<List<InlineKeyboardButton>>
			{
				new List<InlineKeyboardButton>()
			};
			list[list.Count - 1].Add(new InlineKeyboardButton() { Text = "<", CallbackData = CommandTextBlockchain.LeftComissionCripta + " " + +value });
			list[list.Count - 1].Add(new InlineKeyboardButton() { Text = value + "%", CallbackData = CommandTextBlockchain.SettingsAdminInBlockChain });
			list[list.Count - 1].Add(new InlineKeyboardButton() { Text = ">", CallbackData = CommandTextBlockchain.RightComissionCripta + " " + +value });
			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton() { Text = "🔙Назад🔙", CallbackData = CommandTextBlockchain.SettingsAdminInBlockChain });

			return new InlineKeyboardMarkup(list);
		}
		public static InlineKeyboardMarkup SettingsCommisionAdmin(Decimal value)
		{
			List<List<InlineKeyboardButton>> list = new List<List<InlineKeyboardButton>>
			{
				new List<InlineKeyboardButton>()
			};
			list[list.Count - 1].Add(new InlineKeyboardButton() { Text = "<", CallbackData = CommandTextBlockchain.LeftComissionAdmin + " " + +value });
			list[list.Count - 1].Add(new InlineKeyboardButton() { Text = value + "%", CallbackData = CommandTextBlockchain.SettingsAdminInBlockChain });
			list[list.Count - 1].Add(new InlineKeyboardButton() { Text = ">", CallbackData = CommandTextBlockchain.RightComissionAdmin + " " + +value });
			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton() { Text = "🔙Назад🔙", CallbackData = CommandTextBlockchain.SettingsAdminInBlockChain });

			return new InlineKeyboardMarkup(list);
		}

		#region TransactionMenu

		/// <summary>
		/// Меню создание транзкции, кнопка "🛅Добавить участника🛅" создается автоматически когда была добавленна сумма транзакции
		/// и выбран плательщик комисии.
		/// </summary>
		/// <returns></returns>
		public static InlineKeyboardMarkup TransactionCreationGuarantor(Transaction transaction)
		{
			List<List<InlineKeyboardButton>> list = new List<List<InlineKeyboardButton>>();

			if (transaction.PaymentId != 0 && transaction.SumPayNew != 0)
			{
				list.Add(new List<InlineKeyboardButton>());
				list[list.Count - 1].Add(new InlineKeyboardButton() { Text = "🛅Добавить участника🛅", CallbackData = CommandTextBlockchain.AddUserInTransaction + " " + transaction.Id });
			}
			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton() { Text = "💰Выбор способа оплаты💰", CallbackData = CommandTextBlockchain.ChoosingPaymentMethod + " " + transaction.Id });
			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton() { Text = "💸Оплата комиссии💸", CallbackData = CommandTextBlockchain.CommissionPayment + " " + transaction.Id });
			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton() { Text = "🔙Назад🔙", CallbackData = CommandTextBlockchain.BackToGuarantorMeinMenu });

			return new InlineKeyboardMarkup(list);
		}

		/// <summary>
		/// Вывод одной кнопки "🔙Назад🔙", которая приводит в меню TransactionCreationGuarantor
		/// </summary>
		public static InlineKeyboardMarkup AddUserInTransactionToBack(Transaction transaction)
		{
			List<List<InlineKeyboardButton>> list = new List<List<InlineKeyboardButton>>
			{
				new List<InlineKeyboardButton>()
			};
			list[list.Count - 1].Add(new InlineKeyboardButton() { Text = "🔙Назад🔙", CallbackData = CommandTextBlockchain.BackToTransactionCreationGuarantor + " " + transaction.Id });

			return new InlineKeyboardMarkup(list);
		}

		/// <summary>
		/// Отправка подтверждения второму пользователю, он должен это либо подтвердить либо отменить
		/// </summary>
		/// <param name="IdTransaction">Передается ID транзакции которая была создана, после подтверждения User
		/// должен добавится в ДБ</param>
		/// <returns></returns>
		public static InlineKeyboardMarkup SendTheConfirmationToUser(Int32 IdTransaction)
		{
			List<List<InlineKeyboardButton>> list = new List<List<InlineKeyboardButton>>
			{
				new List<InlineKeyboardButton>()
			};
			list[list.Count - 1].Add(new InlineKeyboardButton() { Text = "✅Подтвердить✅", CallbackData = CommandTextBlockchain.ConfirmSendTheConfirmationToUser + " " + IdTransaction });
			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton() { Text = "❌Отменить❌", CallbackData = CommandTextBlockchain.CancelSendTheConfirmationToUser + " " + IdTransaction });

			return new InlineKeyboardMarkup(list);
		}

		/// <summary>
		/// Меню выбора способа оплаты, различные методы, которые имею каждый свою реализацию, кнопка "Назад"
		/// ведет в TransactionCreationGuarantor
		/// </summary>
		public static InlineKeyboardMarkup ChoosingPaymentMethod(Transaction transaction)
		{
			List<List<InlineKeyboardButton>> list = new List<List<InlineKeyboardButton>>
			{
				new List<InlineKeyboardButton>()
			};
			list[list.Count - 1].Add(new InlineKeyboardButton() { Text = "BTC", CallbackData = CommandTextBlockchain.BTCPaymentMethod + " " + transaction.Id });
			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton() { Text = "Ethereum", CallbackData = CommandTextBlockchain.EthereumPaymentMethod + " " + transaction.Id });
			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton() { Text = "Ripple", CallbackData = CommandTextBlockchain.RipplePaymentMethod + " " + transaction.Id });
			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton() { Text = "🔙Назад🔙", CallbackData = CommandTextBlockchain.BackToTransactionCreationGuarantor + " " + transaction.Id });

			return new InlineKeyboardMarkup(list);
		}

		/// <summary>
		/// Вывод одной кнопки "🔙Назад🔙", которая приводит в меню ChoosingPaymentMethod
		/// Данная кнопка будет появляться когда человек нажал на какой либо конкретный способ оплаты!
		/// Когда человек ввел сумму, она должна записаться в БД
		/// Выводимый текст должен быть
		/// Текст: "Сумма транзакции:(в выбранной валюте)
		/// или [ГРН]/[USD] [сумма]:(данная сумма автоматически сконвертируется в выбранный вами формат платежа) "
		/// </summary>
		public static InlineKeyboardMarkup ChoosingPaymentMethodToBack(Transaction transaction)
		{
			List<List<InlineKeyboardButton>> list = new List<List<InlineKeyboardButton>>
			{
				new List<InlineKeyboardButton>()
			};
			list[list.Count - 1].Add(new InlineKeyboardButton() { Text = "🔙Назад🔙", CallbackData = CommandTextBlockchain.BackToChoosingPaymentMethod + " " + transaction.Id });

			return new InlineKeyboardMarkup(list);
		}

		/// <summary>
		/// Выбор кто оплачивает ккомисию, выбор записывается в БД = True - получатель, False - Отправитель
		/// Кнопка наза введет в TransactionCreationGuarantor
		/// </summary>
		public static InlineKeyboardMarkup CommissionPayment(Transaction transaction)
		{
			List<List<InlineKeyboardButton>> list = new List<List<InlineKeyboardButton>>
			{
				new List<InlineKeyboardButton>()
			};
			list[list.Count - 1].Add(new InlineKeyboardButton() { Text = "🧮Получатель🧮", CallbackData = CommandTextBlockchain.CommissionPaymentRecipient + " " + transaction.Id });
			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton() { Text = "🧮Отправитель🧮", CallbackData = CommandTextBlockchain.CommissionPaymentSender + " " + transaction.Id });
			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton() { Text = "🔙Назад🔙", CallbackData = CommandTextBlockchain.BackToTransactionCreationGuarantor + " " + transaction.Id });

			return new InlineKeyboardMarkup(list);
		}

		/// <summary>
		/// Добавление ID транзакции в базу данных для подтверждение личности.
		/// </summary>
		/// <param name="transaction"></param>
		/// <returns></returns>
		public static InlineKeyboardMarkup SetTransactionMenu(Transaction transaction)
		{
			List<List<InlineKeyboardButton>> list = new List<List<InlineKeyboardButton>>
			{
				new List<InlineKeyboardButton>()
			};

			list[list.Count - 1].Add(new InlineKeyboardButton() { Text = "🔙Назад🔙", CallbackData = CommandTextBlockchain.BackToSetMoneyCount + " " + transaction.Id });

			return new InlineKeyboardMarkup(list);
		}

		#endregion TransactionMenu

		#region MyTransactionMenu

		/// <summary>
		/// Меню "Мои транзакции" выводит все транзакции:
		/// "Имя транзакции✅" - транзакция которая должна подтвердится как успешная
		/// "Имя транзакции❌" - транзакция котораяя должна отменится
		/// "Имя транзакции" - транзакция которая находится в работе
		/// </summary>
		/// <returns></returns>
		public static InlineKeyboardMarkup ShowMyTransaction(List<Transaction> transactions, User user)
		{
			List<List<InlineKeyboardButton>> list = new List<List<InlineKeyboardButton>>();

			foreach (Transaction transaction in transactions)
			{
				if (transaction.UserSenderId == user.ID)
				{
					if (transaction.TransactionSuspension == false)
					{
						if ((transaction.IsConfirmOrCancelUserRecipient == 2 ||
						     transaction.IsConfirmOrCancelUserSender    == 2) &&
						    transaction.IsPaySenderOrRecipiend == false)
						{
							list.Add(new List<InlineKeyboardButton>());
							list[list.Count - 1].Add(new InlineKeyboardButton()
							{
								Text = transaction.Name == null
									       ? "гарант✅"
									       : transaction.Name + "✅",
								CallbackData =
									CommandTextBlockchain.NameTransactionConfirm + " " +
									transaction.Id
							});
						}
						else if ((transaction.IsConfirmOrCancelUserRecipient == 1 ||
						          transaction.IsConfirmOrCancelUserSender    == 1) &&
						         transaction.IsPaySenderOrRecipiend == false)
						{
							list.Add(new List<InlineKeyboardButton>());
							list[list.Count - 1].Add(new InlineKeyboardButton()
							{
								Text = transaction.Name == null
									       ? "гарант❌"
									       : transaction.Name + "❌",
								CallbackData =
									CommandTextBlockchain.NameTransactionCancel + " " +
									transaction.Id
							});
						}
						else if ((transaction.IsConfirmOrCancelUserRecipient == 0 ||
						          transaction.IsConfirmOrCancelUserSender    == 0) &&
						         transaction.IsPaySenderOrRecipiend == false)
						{
							list.Add(new List<InlineKeyboardButton>());
							list[list.Count - 1].Add(new InlineKeyboardButton()
							{
								Text = transaction.Name == null
									       ? "гарант✅"
									       : transaction.Name,
								CallbackData =
									CommandTextBlockchain.NameTransaction + " " +
									transaction.Id
							});
						}
					}
				}
			}

			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton() { Text = "🔙Назад🔙", CallbackData = CommandTextBlockchain.BackToGuarantorMeinMenu });

			return new InlineKeyboardMarkup(list);
		}

		/// <summary>
		/// Меню выбранной транзакции.
		/// </summary>
		public static InlineKeyboardMarkup SelectConfirmOrCancelThisTransaction(Transaction transaction)
		{
			List<List<InlineKeyboardButton>> list = new List<List<InlineKeyboardButton>>
			{
				new List<InlineKeyboardButton>()
			};
			if (transaction.TransactionSuspension == false)
			{
				list[list.Count - 1].Add(new InlineKeyboardButton() { Text = "✅Подтвердить✅", CallbackData = CommandTextBlockchain.ConfirmMyTransaction + " " + transaction.Id });
				list.Add(new List<InlineKeyboardButton>());
				list[list.Count - 1].Add(new InlineKeyboardButton() { Text = "❌Отменить❌", CallbackData = CommandTextBlockchain.CancelMyTransaction + " " + transaction.Id });
				list.Add(new List<InlineKeyboardButton>());
				list[list.Count - 1].Add(new InlineKeyboardButton() { Text = "🛎Вызов администратора🛎", CallbackData = CommandTextBlockchain.GetAdminInBlockChain + " " + transaction.Id });
				list.Add(new List<InlineKeyboardButton>());
			}
			list[list.Count - 1].Add(new InlineKeyboardButton() { Text = "🔙Назад🔙", CallbackData = CommandTextBlockchain.BackToShowMyTransaction + " " + transaction.Id });

			return new InlineKeyboardMarkup(list);
		}

		/// <summary>
		/// Подтверждения выхова администратора!
		/// </summary>
		/// <param name="transaction"></param>
		/// <returns></returns>
		public static InlineKeyboardMarkup GetAdminInMyTransaction(Transaction transaction)
		{
			List<List<InlineKeyboardButton>> list = new List<List<InlineKeyboardButton>>
			{
				new List<InlineKeyboardButton>()
			};
			if (transaction.TransactionSuspension == false)
			{
				list[list.Count - 1].Add(new InlineKeyboardButton() { Text = "✅Подтвердить✅", CallbackData = CommandTextBlockchain.GetAdminInMyTransaction + " " + transaction.Id });
				list.Add(new List<InlineKeyboardButton>());
			}
			list[list.Count - 1].Add(new InlineKeyboardButton() { Text = "🔙Назад🔙", CallbackData = CommandTextBlockchain.BackToSelectConfirmOrCancelThisTransaction + " " + transaction.Id });

			return new InlineKeyboardMarkup(list);
		}

		/// <summary>
		/// Отмена или принятие добавления админа в транзакцию.
		/// </summary>
		/// <param name="transaction"></param>
		/// <returns></returns>
		public static InlineKeyboardMarkup SetAdminInTransaction(Transaction transaction)
		{
			List<List<InlineKeyboardButton>> list = new List<List<InlineKeyboardButton>>
			{
				new List<InlineKeyboardButton>()
			};
			list[list.Count - 1].Add(new InlineKeyboardButton() { Text = "✅Подтвердить✅", CallbackData = CommandTextBlockchain.SetConfirmAdminInBlockChain + " " + transaction.Id });
			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton() { Text = "❌Отменить❌", CallbackData = CommandTextBlockchain.SetCancelAdminInMyTransaction + " " + transaction.Id });

			return new InlineKeyboardMarkup(list);
		}

		/// <summary>
		/// Выбор кому отправить деньги!
		/// </summary>
		/// <param name="transaction"></param>
		/// <param name="user"></param>
		/// <returns></returns>
		public static InlineKeyboardMarkup SetMoneyInTransaction(Transaction transaction, User user)
		{
			List<List<InlineKeyboardButton>> list = new List<List<InlineKeyboardButton>>
			{
				new List<InlineKeyboardButton>()
			};
			list[list.Count - 1].Add(new InlineKeyboardButton() { Text = "👐Отправитель👐", CallbackData = CommandTextBlockchain.GetMoneySenderAdminInBlockChain + " " + transaction.Id + " " + user.ID });
			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton() { Text = "🤝Получатель🤝", CallbackData = CommandTextBlockchain.GetMoneyRecipientAdminInMyTransaction + " " + transaction.Id + " " + user.ID });

			return new InlineKeyboardMarkup(list);
		}

		/// <summary>
		/// Подтверждение конкретной транзакции, как только человек нажал на кнопку подтвержить
		/// должно прийти уведомление другому участнику с подтверждением ConfirmThisTransactionUserTwo
		/// </summary>
		public static InlineKeyboardMarkup ConfirmThisTransaction(Transaction transaction)
		{
			List<List<InlineKeyboardButton>> list = new List<List<InlineKeyboardButton>>
			{
				new List<InlineKeyboardButton>()
			};
			if (transaction.TransactionSuspension == false)
			{
				list[list.Count - 1].Add(new InlineKeyboardButton() { Text = "✅Подтвердить✅", CallbackData = CommandTextBlockchain.ConfirmThisTransaction + " " + transaction.Id });
				list.Add(new List<InlineKeyboardButton>());
			}
			list[list.Count - 1].Add(new InlineKeyboardButton() { Text = "🔙Назад🔙", CallbackData = CommandTextBlockchain.BackToSelectConfirmOrCancelThisTransaction + " " + transaction.Id });

			return new InlineKeyboardMarkup(list);
		}

		/// <summary>
		/// Подтвердление или отмена транзакции другим участником
		/// Подтверждение - выводить что данная транзакция была подтверждена.
		/// Отмена - спросить почему он хочет отменить данную транзакцию
		/// </summary>
		public static InlineKeyboardMarkup ConfirmOrCancelThisTransactionUserTwo(Transaction transaction)
		{
			List<List<InlineKeyboardButton>> list = new List<List<InlineKeyboardButton>>
			{
				new List<InlineKeyboardButton>()
			};
			if (transaction.TransactionSuspension == false)
			{
				list[list.Count - 1].Add(new InlineKeyboardButton() { Text = "✅Подтвердить✅", CallbackData = CommandTextBlockchain.ConfirmThisTransactionUserTwo + " " + transaction.Id });
				list.Add(new List<InlineKeyboardButton>());
			}
			list[list.Count - 1].Add(new InlineKeyboardButton() { Text = "❌Отменить❌", CallbackData = CommandTextBlockchain.CancelThisTransactionUserTwo + " " + transaction.Id });

			return new InlineKeyboardMarkup(list);
		}

		/// <summary>
		/// При нажатиее кнопки подтвердить в меню ConfirmOrCancelThisTransactionUserTwo выскакивает данная меню, которая вернет назад!
		/// Если вывелось данная меню, нужно выводить public key кошеляка!
		/// </summary>
		/// <param name="transaction"></param>
		/// <returns></returns>
		public static InlineKeyboardMarkup BackToConfirmOrCancelThisTransactionUserTwo(Transaction transaction)
		{
			List<List<InlineKeyboardButton>> list = new List<List<InlineKeyboardButton>>
			{
				new List<InlineKeyboardButton>()
			};
			list[list.Count - 1].Add(new InlineKeyboardButton() { Text = "🔙Назад🔙", CallbackData = CommandTextBlockchain.BackToConfirmOrCancelThisTransactionUserTwo + " " + transaction.Id });

			return new InlineKeyboardMarkup(list);
		}

		/// <summary>
		/// Отмена транзакции, после того как пользователь отменил транзакцию, должно приходить
		/// уведомление админам, и они будут решать что не так.
		/// А так же уведомление прийдет и второму пользователю с подтверждением или отминением
		/// </summary>
		public static InlineKeyboardMarkup CancelThisTransactionUserTwo(Transaction transaction)
		{
			List<List<InlineKeyboardButton>> list = new List<List<InlineKeyboardButton>>
			{
				new List<InlineKeyboardButton>()
			};
			list[list.Count - 1].Add(new InlineKeyboardButton() { Text = "🔙Назад🔙", CallbackData = CommandTextBlockchain.BackToSelectConfirmOrCancelThisTransaction + " " + transaction.Id });

			return new InlineKeyboardMarkup(list);
		}

		#endregion MyTransactionMenu

		#region Reputation

		/// <summary>
		/// Вывод меню с где будет вся инфопмация о пользователе, а именно:
		/// Сумма всех транзакций
		/// Количество успешных транзакций
		/// Количество отмененных транзакций
		/// </summary>
		public static InlineKeyboardMarkup ReputationUser = new InlineKeyboardButton[][]
		{
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton { Text = "✅Успешные транзакции✅", CallbackData = CommandTextBlockchain.ConfirmTransactionReputationUser }
			},
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton { Text = "❌Отмененные транзакции❌", CallbackData = CommandTextBlockchain.CancelTransactionReputationUser }
			},
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton { Text = "🔙Назад🔙", CallbackData = CommandTextBlockchain.BackToGuarantorMeinMenu }
			},
		};

		/// <summary>
		/// Успешные транзакции
		/// </summary>
		/// <returns></returns>
		public static InlineKeyboardMarkup ShowReputationConfirmTransaction(List<Transaction> transactions)
		{
			List<List<InlineKeyboardButton>> list = new List<List<InlineKeyboardButton>>();

			foreach (Transaction transaction in transactions)
			{
				if (transaction.IsPayGarantor == true && transaction.PaymentId != 0 && transaction.SumPayNew != 0 && transaction.IsConfirmOrCancelUserRecipient == 2 && transaction.IsConfirmOrCancelUserSender == 2 && transaction.IsPaySenderOrRecipiend == true)
				{
					list.Add(new List<InlineKeyboardButton>());
					list[list.Count - 1].Add(new InlineKeyboardButton() { Text = transaction.Name, CallbackData = CommandTextBlockchain.NameReputationConfirmTransaction + " " + transaction.Id });
				}
			}
			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton() { Text = "🔙Назад🔙", CallbackData = CommandTextBlockchain.BackToReputationUser });

			return new InlineKeyboardMarkup(list);
		}

		/// <summary>
		/// Отмененные транзакции
		/// </summary>
		/// <returns></returns>
		public static InlineKeyboardMarkup ShowReputationCancelTransaction(List<Transaction> transactions)
		{
			List<List<InlineKeyboardButton>> list = new List<List<InlineKeyboardButton>>();

			foreach (Transaction transaction in transactions)
			{
				if (transaction.IsPayGarantor == true && transaction.PaymentId != 0 && transaction.SumPayNew != 0 && transaction.IsConfirmOrCancelUserRecipient == 1 && transaction.IsConfirmOrCancelUserSender == 1 && transaction.IsPaySenderOrRecipiend == true)
				{
					list.Add(new List<InlineKeyboardButton>());
					list[list.Count - 1].Add(new InlineKeyboardButton() { Text = transaction.Name, CallbackData = CommandTextBlockchain.NameReputationCancelTransaction + " " + transaction.Id });
				}
			}
			list.Add(new List<InlineKeyboardButton>());
			list[list.Count - 1].Add(new InlineKeyboardButton() { Text = "🔙Назад🔙", CallbackData = CommandTextBlockchain.BackToReputationUser });

			return new InlineKeyboardMarkup(list);
		}

		/// <summary>
		/// Успешная транзакция:
		/// Текст:
		/// 1. Пользователь №1
		/// 2. Пользователь №2
		/// 3. Сумма;
		/// </summary>
		public static InlineKeyboardMarkup ShowOneReputationConfirmTransaction = new InlineKeyboardButton[][]
		{
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton { Text = "🔙Назад🔙", CallbackData = CommandTextBlockchain.BackToShowOneReputationConfirmTransaction }
			}
		};

		/// <summary>
		/// Отмененная транзакция:
		/// 1. Пользователь №1
		/// 2. Пользователь №2
		/// 3. Сумма
		/// 4. Причина;
		/// </summary>
		public static InlineKeyboardMarkup ShowOneReputationCancelTransaction = new InlineKeyboardButton[][]
		{
			new InlineKeyboardButton[]
			{
				new InlineKeyboardButton { Text = "🔙Назад🔙", CallbackData = CommandTextBlockchain.BackToShowOneReputationCancelTransaction }
			}
		};

		#endregion Reputation
	}
}