using System;

using BotCore.SQL;

using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Payments;

namespace BotCore.Advertising
{
	internal class Payments : AbstractHandlerAdvertising
	{
		internal async void SendMessage(TelegramBotClient botClient, Message _message)
		{
			DataBase db = Singleton.GetInstance().Context;
			User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return; // - заменить на другую таблицу
			try
			{
				await botClient.DeleteMessageAsync(_message.From.Id, _message.MessageId);
				await botClient.DeleteMessageAsync(_message.From.Id, user.MessageID);

				System.Int32 payment = 0;
				System.Int32.TryParse(_message.Text, out payment);

				if (payment == 0)
				{
					await botClient.SendTextMessageAsync(_message.From.Id, "Вы ввели не верный формат! \nПожалуйста введите еще раз сумму оплаты: (грн)", replyMarkup: InlineButton.BackToAdvertisingMenu);
				}
				else
				{
					await botClient.SendInvoiceAsync(
					chatId: _message.From.Id,
					title: "Пополнение баланса на: " + _message.Text + " грн",
					description: "Пополнение баланса на: " + _message.Text + " грн",
					payload: "Balance is correct",
					providerToken: "635983722:LIVE:i29496402528",
					startParameter: "HEllo",
					currency: "UAH",
					prices: new[] { new LabeledPrice("price", payment * 100), },
					replyMarkup: InlineButton.Payment
					);
					user.Chain = (System.Int32)SetChain.MessageUserInBot;
				}//
				db.Save();
			}
			catch { }
			db.Save();
		}

		public override System.Object Handle(System.Int32 request, TelegramBotClient botClient, System.Object message)
		{
			if (request == (System.Int32)SetChain.GetPayments)
			{
				Message _message = message as Message;

				SendMessage(botClient, _message);

				return null;
			}
			else
			{
				return base.Handle(request, botClient, message);
			}
		}
	}

	internal class AddPhotoAdvertising : AbstractHandlerAdvertising
	{
		internal async void SendMessage(TelegramBotClient botClient, Message _message)
		{
			DataBase db = Singleton.GetInstance().Context;
			User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return; // - заменить на другую таблицу
			AdUser adUser = db.GetAdUser(user.ID);
			PostTemplate postTemplate = db.GetTempalte(adUser.User.ID, adUser.EditingPostTemplateId);
			try
			{
				AdController.SetContent(botClient, postTemplate, _message, adUser.Order);
				botClient.DeleteMessage(_message.From.Id, _message.MessageId, "78 - AddPhoto ChainAnaliz");
				botClient.DeleteMessage(_message.From.Id, _message.MessageId - 1, "79 - AddPhoto ChainAnaliz");
				botClient.SendText(_message.From.Id, "Управление шаблоном\n1. Заполните контент\n2. Введите время постинга.\n3. Выберете чат(ы)\n4. Отправьте на проверку администрации.", user, replyMarkup: InlineButton.ContentKeyboard(postTemplate, true));
				user.Chain = (System.Int32)SetChain.MessageUserInBot;
				db.Save();
			}
			catch (System.Exception ex)
			{
				Log.Logging(ex);
			}
			db.Save();
		}

		public override System.Object Handle(System.Int32 request, TelegramBotClient botClient, System.Object message)
		{
			if (request == (System.Int32)SetChain.AddPhoto)
			{
				Message _message = message as Message;

				SendMessage(botClient, _message);

				return null;
			}
			else
			{
				return base.Handle(request, botClient, message);
			}
		}
	}

	internal class ContentEditorText : AbstractHandlerAdvertising
	{
		internal async void SendMessage(TelegramBotClient botClient, Message _message)
		{
			DataBase db = Singleton.GetInstance().Context;
			User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return; // - заменить на другую таблицу
			AdUser adUser = db.GetAdUser(user.ID);
			PostTemplate postTemplate = db.GetTempalte(adUser.User.ID, adUser.EditingPostTemplateId);
			try
			{
				AdController.SetContent(botClient, postTemplate, _message, 0);

				Int32 len = _message.Text.Length;

				if (len > 20)
				{
					len = 20;
				}

				String str = new String(_message.Text.ToCharArray(), 0 , len - 1);

				postTemplate.Name = _message.Text;
				await botClient.DeleteMessageAsync(_message.From.Id, _message.MessageId);
				await botClient.EditMessageTextAsync(_message.From.Id, user.MessageID, "Управление шаблоном\n1. Заполните контент\n2. Введите время постинга.\n3. Выберете чат(ы)\n4. Отправите на проверку администрации.", replyMarkup: InlineButton.ContentKeyboard(postTemplate, true));
				user.Chain = (System.Int32)SetChain.MessageUserInBot;
				db.Save();
			}
			catch (System.Exception ex)
			{
				Log.Logging(ex);
			}
			db.Save();
		}

		public override System.Object Handle(System.Int32 request, TelegramBotClient botClient, System.Object message)
		{
			if (request == (System.Int32)SetChain.SetEditorText)
			{
				Message _message = message as Message;

				SendMessage(botClient, _message);

				return null;
			}
			else
			{
				return base.Handle(request, botClient, message);
			}
		}
	}

	internal class ThisRegulationsUBC : AbstractHandlerAdvertising
	{
		internal async void SendMessage(TelegramBotClient botClient, Message _message)
		{
			DataBase db = Singleton.GetInstance().Context;
			User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return; // - заменить на другую таблицу

			if (user.IsAdmin >= 2)
			{
				Settings settings = db.GetSettings();
				settings.Regulations = _message.Text;
				db.Save();
				BotCore.InlineButton inlineButton = new BotCore.InlineButton();
				botClient.DeleteMessage(_message.From.Id, _message.MessageId, "172 - ThisRegulationsUBC");
				botClient.EditMessage(_message.From.Id, user.MessageID, "Ссылка на правила была изменена!", "173 - ThisRegulationsUBC", user, replyMarkup: inlineButton.SettingBotLvl2(user));
			}

		}

		public override System.Object Handle(System.Int32 request, TelegramBotClient botClient, System.Object message)
		{
			if (request == (System.Int32)SetChain.RegulationsUBC)
			{
				Message _message = message as Message;

				SendMessage(botClient, _message);

				return null;
			}
			else
			{
				return base.Handle(request, botClient, message);
			}
		}
	}

	internal class ThisDeleteChat : AbstractHandlerAdvertising
	{
		private void Delete(DataBase db, Channel channel)
		{
			AnaliticsPhrase[] analiticsPhrase = db.GetAnaliticsPharse();

			foreach (AnaliticsPhrase item in analiticsPhrase)
			{
				if (item.ChannelId == channel.IDChannel)
				{
					db._analiticsPhrases.Remove(item);
				}
			}

			AnalyticsText[] analyticsTexts = db.GetAnalitics();

			foreach (AnalyticsText item in analyticsTexts)
			{
				if (item.ChannelId == channel.IDChannel)
				{
					db._analyticsTexts.Remove(item);
				}
			}

			AnaliticsPhraseMonth[] analiticsPhraseMonths = db.GetAnaliticsPhraseMonth();

			foreach (AnaliticsPhraseMonth item in analiticsPhraseMonths)
			{
				if (item.ChannelId == channel.IDChannel)
				{
					db._analiticsPhraseMonths.Remove(item);
				}
			}

			db.Save();
		}

		internal async void SendMessage(TelegramBotClient botClient, Message _message)
		{
			DataBase db = Singleton.GetInstance().Context;
			User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return;

			if (user.IsAdmin >= 2)
			{
				Channel channel = db.GetChannel(_message.Text);
				if (channel == null)
					return;

				TMessage[] messages = db.GetTMessages();

				foreach (TMessage item in messages)
				{
					if (item.channel != null)
					{
						if (item.channel.IDChannel == channel.IDChannel)
						{
							db._tmessage.Remove(item);
						}
					}
				}
				Delete(db, channel);
				db._channels.Remove(channel);
				db.Save();

				BotCore.InlineButton inlineButton = new BotCore.InlineButton();
				botClient.DeleteMessage(_message.From.Id, _message.MessageId, "208 - ThisDeleteChat");
				botClient.EditMessage(_message.From.Id, user.MessageID, "Чат был успешно удаляен! Не забудьте удалить бота их чата!", "209 - ThisDeleteChat", user, replyMarkup: inlineButton.SettingBotLvl2(user));
			}

		}

		public override System.Object Handle(System.Int32 request, TelegramBotClient botClient, System.Object message)
		{
			if (request == (System.Int32)SetChain.DeleteChat)
			{
				Message _message = message as Message;

				SendMessage(botClient, _message);

				return null;
			}
			else
			{
				return base.Handle(request, botClient, message);
			}
		}
	}

	internal class ThisDeleteCategory : AbstractHandlerAdvertising
	{
		internal async void SendMessage(TelegramBotClient botClient, Message _message)
		{
			DataBase db = Singleton.GetInstance().Context;
			User user = db.GetUser(_message.From.Id); if (IsNullDataBase.IsNull(botClient, _message, user)) return; // - заменить на другую таблицу

			if (user.IsAdmin >= 2)
			{
				Category category = db.GetCategory(_message.Text);
				if (category == null) return;

				db._categories.Remove(category);
				db.Save();


				BotCore.InlineButton inlineButton = new BotCore.InlineButton();
				botClient.DeleteMessage(_message.From.Id, _message.MessageId, "295 - ThisDeleteCategory");
				botClient.EditMessage(_message.From.Id, user.MessageID, "Категория была удалена!", "295 - ThisDeleteCategory", user, replyMarkup: inlineButton.SettingBotLvl2(user));
			}

		}

		public override System.Object Handle(System.Int32 request, TelegramBotClient botClient, System.Object message)
		{
			if (request == (System.Int32)SetChain.DeleteCategory)
			{
				Message _message = message as Message;

				SendMessage(botClient, _message);

				return null;
			}
			else
			{
				return base.Handle(request, botClient, message);
			}
		}
	}
}