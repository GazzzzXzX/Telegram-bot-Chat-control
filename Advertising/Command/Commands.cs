using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using BotCore.Blockchain;
using BotCore.SQL;
using BotCore.WorkingTime;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace BotCore.Advertising.Command
{
	internal class AddAdvertising : AbsCommand
	{
		public override System.String Name { set; get; } = CommandsText.AddAdvertising;

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase      db       = Singleton.GetInstance().Context;
			try
			{
				if (IsBan.Ban(botClient, message))
				{
					User user = db.GetUser(_message.From.Id);
					if (IsNullDataBase.IsNull(botClient, _message, user)) return;
					AdUser       AdUser = db.GetAdUser(user.ID);
					PostTemplate template;
					if (AdUser.EditingPostTemplateId == -1)
					{
						template = AdController.CreateTemplate(AdUser);
					}
					else
					{
						template = db.GetTempalte(AdUser.User.ID, AdUser.EditingPostTemplateId);
					}

					AdUser.EditingPostTemplateId = template.Id;

					db.Save();
					await botClient.EditMessageTextAsync(_message.From.Id, _message.Message.MessageId,
					                                     "Управление шаблоном\n1. Заполните контент\n2. Введите время постинга.\n3. Выберете чат(ы)\n4. Отправите на проверку администрации.",
					                                     Telegram.Bot.Types.Enums.ParseMode.Html,
					                                     replyMarkup: InlineButton.GetTemplateMenu(AdUser));
				}
			}
			catch (System.Exception ex)
			{
				Log.Logging(ex);
			}
		}
	}

	internal class SetAdvertisingOneChannel : AbsCommand
	{
		public override System.String Name { set; get; } = CommandsText.SetAdvertisingOneChannel;

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase      db       = Singleton.GetInstance().Context;
			try
			{
				if (IsBan.Ban(botClient, message))
				{
					User user = db.GetUser(_message.From.Id);
					if (IsNullDataBase.IsNull(botClient, _message, user)) return;
					AdUser       AdUser  = db.GetAdUser(user.ID);
					System.Int64 temp    = System.Convert.ToInt64(Name.Split(" ")[1]);
					Channel      channel = db.GetChannel(temp);
					PostTemplate template;
					if (AdUser.EditingPostTemplateId == -1)
					{
						template = AdController.CreateTemplate(AdUser);
						db.SetValue<PostChannel>(new PostChannel()
						{
							Channel        = channel,
							ChannelId      = channel.IDChannel,
							PostTemplateId = template.Id,
							Template       = template
						});
					}
					else
					{
						template = db.GetTempalte(AdUser.User.ID, AdUser.EditingPostTemplateId);
					}

					AdUser.EditingPostTemplateId = template.Id;

					Name = CommandsText.SetAdvertisingOneChannel;

					db.Save();
					await botClient.EditMessageTextAsync(_message.From.Id, _message.Message.MessageId,
					                                     "Управление шаблоном\n1. Заполните контент\n2. Введите время постинга.\n3. Выберете чат(ы)\n4. Отправите на проверку администрации.",
					                                     Telegram.Bot.Types.Enums.ParseMode.Html,
					                                     replyMarkup: InlineButton.GetTemplateMenu(AdUser));
				}
			}
			catch (System.Exception ex)
			{
				Log.Logging(ex);
			}
		}
	}

	internal class MenuAdContent : AbsCommand, ISplitName, IStandartCommand
	{
		public override System.String Name { set; get; } = CommandsText.MenuAdContent;

		private async void DeleteMessage(TelegramBotClient botClient, CallbackQuery _message)
		{
			for (System.Int32 i = 1; i <= 10; i++)
			{
				try
				{
					await botClient.DeleteMessageAsync(_message.From.Id, _message.Message.MessageId - i);
				}
				catch { }
			}
		}

		private DataBase db = null;
		private User user = null;
		private CallbackQuery _message = null;
		private PostTemplate postTemplate = null;

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			IStandartCommand standartCommand = new CreateAdKeyboardTypeMessage();
			ISplitName       splitName       = new CreateAdKeyboardTypeMessage();

			if (standartCommand.SetCallbackQuery(message, out _message)) return;

			if (standartCommand.SetDataBase(out db)) return;

			if (standartCommand.SetUserAndCheckIsNull(botClient, _message, out user, db)) return;

			Int32 idPostTemplate = splitName.GetNameSplit(Name);
			Name = CommandsText.MenuAdContent;
			if (IsBan.Ban(botClient, message))
			{
				if (GetPostTemplate(idPostTemplate)) return;

				await Task.Run(() => DeleteMessage(botClient, _message));

				try
				{
					AdUser       adUser       = db.GetAdUser(user.ID);
					PostTemplate postTemplate = db.GetTempalte(adUser.User.ID, adUser.EditingPostTemplateId);

					SendMessage(botClient);
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}

		private Boolean GetPostTemplate(Int32 idPostTemplate)
		{
			postTemplate = db.GetTempalte(user.ID, idPostTemplate);
			return postTemplate == null;
		}

		public async void SendMessage(TelegramBotClient botClient)
		{
			System.Boolean showContent = db.GetPostContentCount(postTemplate);
			try
			{
				await botClient.EditMessageTextAsync(_message.From.Id, _message.Message.MessageId,
				                                     "Добавьте картинки и текст\nПункт 1-10 предназначен для картинок, заполнять их все не обязательно!",
				                                     Telegram.Bot.Types.Enums.ParseMode.Html,
				                                     replyMarkup: InlineButton.ContentKeyboard(postTemplate,
				                                                                               showContent));
			}
			catch
			{
				await botClient.DeleteMessageAsync(_message.From.Id, _message.Message.MessageId);
				await botClient.SendTextMessageAsync(_message.From.Id,
				                                     "Добавьте картинки и текст\nПункт 1-10 предназначен для картинок, заполнять их все не обязательно!",
				                                     Telegram.Bot.Types.Enums.ParseMode.Html,
				                                     replyMarkup: InlineButton.ContentKeyboard(postTemplate,
				                                                                               showContent));
			}
		}
	}

	internal class ShowPostTemplateData : AbsCommand
	{
		public override System.String Name { set; get; } = CommandsText.ShowPostTemplateData;

		private async void DeleteMessage(TelegramBotClient botClient, CallbackQuery _message)
		{
			for (System.Int32 i = 1; i <= 11; i++)
			{
				try
				{
					await botClient.DeleteMessageAsync(_message.From.Id, _message.Message.MessageId - i);
				}
				catch { }
			}
		}

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase      db       = Singleton.GetInstance().Context;
			await Task.Run(() => DeleteMessage(botClient, _message));
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					User user = db.GetUser(_message.From.Id);
					if (IsNullDataBase.IsNull(botClient, _message, user)) return;
					AdUser       adUser = db.GetAdUser(user.ID);
					System.Int32 temp   = System.Convert.ToInt32(Name.Split(" ")[1]);
					adUser.EditingPostTemplateId = temp;
					db.Save();
					PostTemplate postTemplate = db.GetTempalte(adUser.User.ID, temp);
					Name = CommandsText.ShowPostTemplateData;

					if (postTemplate.PostContent.Count != 0)
					{
						IOrderedEnumerable<PostContent> orderedList = postTemplate.PostContent.OrderBy(c => c.Order);

						Message              textMessage = null;
						List<InputMediaBase> inputMedias = new List<InputMediaBase>();
						foreach (PostContent content in orderedList)
						{
							Message forward =
								await botClient.ForwardMessageAsync(CommandText.bufferChannelId,
								                                    CommandText.bufferChannelId, content.MessageId);

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

						try
						{
							await botClient.SendMediaGroupAsync(_message.From.Id, inputMedias);
						}
						catch { }

						try
						{
							await botClient.ForwardMessageAsync(_message.From.Id, CommandText.bufferChannelId,
							                                    textMessage.MessageId);
						}
						catch { }

						if (postTemplate.IsOnValidation == false && postTemplate.IsValidated == false)
						{
							botClient.DeleteMessage(_message.From.Id, user.MessageID, "275");
							botClient.SendText(_message.From.Id,
							                   $"Управление шаблоном\n1. Заполните контент\n2. Введите время постинга.\n3. Выберете чат(ы)\n4. Отправите на проверку администрации.",
							                   user, replyMarkup: InlineButton.GetTemplateMenu(adUser));
						}
						else if (postTemplate.IsValidated == true && postTemplate.IsPaid == false)
						{
							botClient.DeleteMessage(_message.From.Id, user.MessageID, "275");
							botClient.SendText(_message.From.Id,
							                   $"<b>Название поста: {textMessage.Text}</b>\n\nДанный пост одобрен администрацией, вы моежете его оплатить после чего он пойдет в работу!",
							                   user, replyMarkup: InlineButton.GetTemplateMenu(adUser));
						}
						else if (postTemplate.IsValidated == true && postTemplate.IsPaid == true)
						{
							botClient.DeleteMessage(_message.From.Id, user.MessageID, "275");
							botClient.SendText(_message.From.Id,
							                   $"<b>Название поста: {textMessage.Text}</b>\n\nДанный пост находится в работе!",
							                   user, replyMarkup: InlineButton.GetTemplateMenu(adUser));
						}
						else
						{
							botClient.DeleteMessage(_message.From.Id, user.MessageID, "275");
							botClient.SendText(_message.From.Id,
							                   $"<b>Название поста: {textMessage.Text}</b>\n\nДанный пост находится в работе!",
							                   user, replyMarkup: InlineButton.GetTemplateMenu(adUser));
						}
					}
					else
					{
						if (postTemplate.IsPaid == false)
						{
							botClient.EditMessage(_message.From.Id, _message.Message.MessageId,
							                      "Управление шаблоном\n1. Заполните контент\n2. Введите время постинга.\n3. Выберете чат(ы)\n4. Отправите на проверку администрации.",
							                      "276", user, replyMarkup: InlineButton.GetTemplateMenu(adUser));
						}
						else
						{
							botClient.EditMessage(_message.From.Id, _message.Message.MessageId,
							                      "Данный пост находится в работе!", "280", user,
							                      replyMarkup: InlineButton.GetTemplateMenu(adUser));
						}
					}
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class SetChatInPostTemplate : AbsCommand
	{
		public override System.String Name { set; get; } = CommandsText.SetChatInPostTemplate;

		private async void DeleteMessage(TelegramBotClient botClient, CallbackQuery _message)
		{
			for (System.Int32 i = 1; i <= 10; i++)
			{
				try
				{
					await botClient.DeleteMessageAsync(_message.From.Id, _message.Message.MessageId - i);
				}
				catch { }
			}
		}

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase      db       = Singleton.GetInstance().Context;
			await Task.Run(() => DeleteMessage(botClient, _message));
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					User user = db.GetUser(_message.From.Id);
					if (IsNullDataBase.IsNull(botClient, _message, user)) return;
					AdUser       adUser       = db.GetAdUser(user.ID);
					PostTemplate postTemplate = db.GetTempalteOne(adUser.User.ID, adUser.EditingPostTemplateId);

					InlineKeyboardMarkup answer = InlineButton.GetChatInPosting(postTemplate);
					await botClient.EditMessageTextAsync(_message.From.Id, _message.Message.MessageId,
					                                     "Выбере чаты в которых будет поститься реклама:",
					                                     Telegram.Bot.Types.Enums.ParseMode.Html, replyMarkup: answer);
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class SelectChatInPostTemplate : AbsCommand
	{
		public override System.String Name { set; get; } = CommandsText.SelectChatInPostTemplate;

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase      db       = Singleton.GetInstance().Context;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					User user = db.GetUser(_message.From.Id);
					if (IsNullDataBase.IsNull(botClient, _message, user)) return;
					AdUser       adUser       = db.GetAdUser(user.ID);
					PostTemplate postTemplate = db.GetTempalteOne(adUser.User.ID, adUser.EditingPostTemplateId);
					System.Int64 temp         = System.Convert.ToInt64(Name.Split(" ")[1]);
					Name = CommandsText.SelectChatInPostTemplate;
					Channel channel = db.GetChannel(temp);

					if (postTemplate.PostChannel.Any(p => p.ChannelId == channel.IDChannel))
					{
						PostChannel postChannel =
							postTemplate.PostChannel.FirstOrDefault(p => p.ChannelId == channel.IDChannel);
						postTemplate.PostChannel.Remove(postChannel);
					}
					else
					{
						PostChannel postChannel = new PostChannel
						{
							ChannelId = channel.IDChannel, PostTemplateId = postTemplate.Id
						};
						db.SetValue<PostChannel>(postChannel);
					}

					db.Save();
					InlineKeyboardMarkup answer = InlineButton.GetChatInPosting(postTemplate);
					await botClient.EditMessageTextAsync(_message.From.Id, _message.Message.MessageId,
					                                     "Выбере чаты в которых будет постится реклама:",
					                                     Telegram.Bot.Types.Enums.ParseMode.Html, replyMarkup: answer);
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class BackToAdTemplateFromValidation : AbsCommand
	{
		public override String Name { get; set; } = CommandsText.BackToAdTemplate;

		public override async void Execute(TelegramBotClient botClient, Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase      db       = Singleton.GetInstance().Context;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					User user = db.GetUser(_message.From.Id);
					if (IsNullDataBase.IsNull(botClient, _message, user)) return;
					AdUser adUser = db.GetAdUser(user.ID);

					await botClient.EditMessageTextAsync(_message.From.Id, _message.Message.MessageId,
					                                     "Управление шаблоном\n1. Заполните контент\n2. Введите время постинга.\n3. Выберете чат(ы)\n4. Отправите на проверку администрации.",
					                                     Telegram.Bot.Types.Enums.ParseMode.Html,
					                                     replyMarkup: InlineButton.GetTemplateMenu(adUser));
				}
				catch (Exception ex)
				{
					Log.Logging("Failed returning to template menu from validation." + ex);
				}
			}
		}
	}

	internal class AddPhoto : AbsCommand
	{
		public override System.String Name { set; get; } = CommandsText.ContentEditorImg;

		private async void DeleteMessage(TelegramBotClient botClient, CallbackQuery _message)
		{
			for (System.Int32 i = 1; i <= 10; i++)
			{
				try
				{
					await botClient.DeleteMessageAsync(_message.From.Id, _message.Message.MessageId - i);
				}
				catch { }
			}
		}

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase      db       = Singleton.GetInstance().Context;
			await Task.Run(() => DeleteMessage(botClient, _message));
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					User user = db.GetUser(_message.From.Id);
					if (IsNullDataBase.IsNull(botClient, _message, user)) return;

					AdUser adUser = db.GetAdUser(user.ID);
					adUser.Order = System.Convert.ToInt32(Name.Split(" ")[1]);

					PostTemplate postTemplate = db.GetTempalte(adUser.User.ID, adUser.EditingPostTemplateId);

					Name = CommandsText.ContentEditorImg;
					db.Save();

					PostContent postContent = AdController.GetContent(botClient, postTemplate, adUser.Order);

					if (postContent == null)
					{
						Message mes = await botClient.EditMessageTextAsync(_message.From.Id, _message.Message.MessageId,
						                                                   "Картинка: ",
						                                                   Telegram.Bot.Types.Enums.ParseMode.Html,
						                                                   replyMarkup: InlineButton
							                                                   .BackToContentKeyboard(postTemplate));
						user.Chain     = (System.Int32)SetChain.AddPhoto;
						user.MessageID = mes.MessageId;
						db.Save();
					}
					else
					{
						await botClient.DeleteMessageAsync(_message.From.Id, _message.Message.MessageId);
						await botClient.SendPhotoAsync(_message.From.Id,
						                               await AdController.GetImage(botClient, postContent),
						                               replyMarkup: InlineButton.AddPhoto(postTemplate));
					}
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class ContentEditorText : AbsCommand
	{
		public override System.String Name { set; get; } = CommandsText.ContentEditorText;

		private async void DeleteMessage(TelegramBotClient botClient, CallbackQuery _message)
		{
			for (System.Int32 i = 1; i <= 10; i++)
			{
				try
				{
					await botClient.DeleteMessageAsync(_message.From.Id, _message.Message.MessageId - i);
				}
				catch { }
			}
		}

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase      db       = Singleton.GetInstance().Context;
			await Task.Run(() => DeleteMessage(botClient, _message));
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					User user = db.GetUser(_message.From.Id);
					if (IsNullDataBase.IsNull(botClient, _message, user)) return;

					AdUser adUser = db.GetAdUser(user.ID);
					adUser.Order = 0;

					PostTemplate postTemplate = db.GetTempalte(adUser.User.ID, adUser.EditingPostTemplateId);

					db.Save();

					PostContent postContent = AdController.GetContent(botClient, postTemplate, adUser.Order);

					if (postContent == null)
					{
						Message mes = await botClient.EditMessageTextAsync(_message.From.Id, _message.Message.MessageId,
						                                                   "Введите текст: ",
						                                                   Telegram.Bot.Types.Enums.ParseMode.Html,
						                                                   replyMarkup: InlineButton
							                                                   .BackToContentKeyboard(postTemplate));
						user.Chain     = (System.Int32)SetChain.SetEditorText;
						user.MessageID = mes.MessageId;
						db.Save();
					}
					else
					{
						Message mes = await botClient.ForwardMessageAsync(CommandText.bufferChannelId,
						                                                  CommandText.bufferChannelId,
						                                                  postContent.MessageId);
						Message mes2 = await botClient.EditMessageTextAsync(_message.From.Id,
						                                                    _message.Message.MessageId,
						                                                    "Ваш старый текст: " + mes.Text +
						                                                    "\nВведите текст: ",
						                                                    Telegram.Bot.Types.Enums.ParseMode.Html,
						                                                    replyMarkup: InlineButton
							                                                    .BackToContentKeyboard(postTemplate));
						user.Chain     = (System.Int32)SetChain.SetEditorText;
						user.MessageID = mes2.MessageId;
						db.Save();
					}
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class BackSetAdvertising : AbsCommand
	{
		public override System.String Name { set; get; } = CommandsText.BackSetAdvertising;

		private async void DeleteMessage(TelegramBotClient botClient, CallbackQuery _message)
		{
			for (System.Int32 i = 1; i <= 10; i++)
			{
				try
				{
					await botClient.DeleteMessageAsync(_message.From.Id, _message.Message.MessageId - i);
				}
				catch { }
			}
		}

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase      db       = Singleton.GetInstance().Context;
			await Task.Run(() => DeleteMessage(botClient, _message));
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					User user = db.GetUser(_message.From.Id);
					if (IsNullDataBase.IsNull(botClient, _message, user)) return;

					AdUser adUser = db.GetAdUser(user.ID);

					adUser.EditingPostTemplateId = -1;
					db.Save();

					await botClient.EditMessageTextAsync(_message.From.Id, _message.Message.MessageId,
					                                     "📞Ваша реклама📞", Telegram.Bot.Types.Enums.ParseMode.Html,
					                                     replyMarkup: InlineButton.GetAdvertising(user));
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class DeletePhoto : AbsCommand
	{
		public override System.String Name { set; get; } = CommandsText.DeletePhoto;

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase      db       = Singleton.GetInstance().Context;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					User user = db.GetUser(_message.From.Id);
					if (IsNullDataBase.IsNull(botClient, _message, user)) return;

					AdUser adUser = db.GetAdUser(user.ID);

					PostTemplate   postTemplate = db.GetTempalte(adUser.User.ID, adUser.EditingPostTemplateId);
					System.Boolean showContent  = db.GetPostContentCount(postTemplate);
					db.Save();

					AdController.RemoveContentAt(botClient, postTemplate, adUser.Order);

					await botClient.DeleteMessageAsync(_message.From.Id, _message.Message.MessageId);
					await botClient.SendTextMessageAsync(_message.From.Id, "Добавьте картинки и текст",
					                                     Telegram.Bot.Types.Enums.ParseMode.Html,
					                                     replyMarkup: InlineButton.ContentKeyboard(postTemplate,
					                                                                               showContent));
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class ChangePhoto : AbsCommand
	{
		public override System.String Name { set; get; } = CommandsText.ChangePhoto;

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase      db       = Singleton.GetInstance().Context;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					User user = db.GetUser(_message.From.Id);
					if (IsNullDataBase.IsNull(botClient, _message, user)) return;

					AdUser adUser = db.GetAdUser(user.ID);

					PostTemplate postTemplate = db.GetTempalte(adUser.User.ID, adUser.EditingPostTemplateId);
					db.Save();

					PostContent postContent = AdController.GetContent(botClient, postTemplate, adUser.Order);
					await botClient.DeleteMessageAsync(_message.From.Id, _message.Message.MessageId);
					Message mes =
						await botClient.SendTextMessageAsync(_message.From.Id, "Картинка: ",
						                                     Telegram.Bot.Types.Enums.ParseMode.Html);
					user.Chain     = (System.Int32)SetChain.AddPhoto;
					user.MessageID = mes.MessageId;
					db.Save();
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class ShowContent : AbsCommand
	{
		public override System.String Name { set; get; } = CommandsText.ShowContent;

		private async void DeleteMessage(TelegramBotClient botClient, CallbackQuery _message)
		{
			for (System.Int32 i = 1; i <= 10; i++)
			{
				try
				{
					await botClient.DeleteMessageAsync(_message.From.Id, _message.Message.MessageId - i);
				}
				catch { }
			}
		}

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase      db       = Singleton.GetInstance().Context;
			await Task.Run(() => DeleteMessage(botClient, _message));
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					User user = db.GetUser(_message.From.Id);
					if (IsNullDataBase.IsNull(botClient, _message, user)) return;

					AdUser adUser = db.GetAdUser(user.ID);

					PostTemplate postTemplate = db.GetTempalte(adUser.User.ID, adUser.EditingPostTemplateId);

					if (postTemplate.PostContent.Count != 0)
					{
						IOrderedEnumerable<PostContent> orderedList = postTemplate.PostContent.OrderBy(c => c.Order);

						Message              textMessage = null;
						List<InputMediaBase> inputMedias = new List<InputMediaBase>();
						foreach (PostContent content in orderedList)
						{
							Message forward =
								await botClient.ForwardMessageAsync(CommandText.bufferChannelId,
								                                    CommandText.bufferChannelId, content.MessageId);

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

						await botClient.SendMediaGroupAsync(_message.From.Id, inputMedias);
						await botClient.ForwardMessageAsync(_message.From.Id, CommandText.bufferChannelId,
						                                    textMessage.MessageId);

						await botClient.SendTextMessageAsync(_message.From.Id, "Картинки и текст: ",
						                                     replyMarkup: InlineButton.ContentKeyboard(postTemplate,
						                                                                               true));
					}
					else
					{
						await botClient.EditMessageTextAsync(_message.From.Id, _message.Message.MessageId,
						                                     "Картинки и текст: ",
						                                     replyMarkup: InlineButton.ContentKeyboard(postTemplate,
						                                                                               true));
					}
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class DeletePost : AbsCommand
	{
		public override System.String Name { set; get; } = CommandsText.DeletePost;

		private async void DeleteMessage(TelegramBotClient botClient, CallbackQuery _message)
		{
			for (System.Int32 i = 1; i <= 10; i++)
			{
				try
				{
					await botClient.DeleteMessageAsync(_message.From.Id, _message.Message.MessageId - i);
				}
				catch { }
			}
		}

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase      db       = Singleton.GetInstance().Context;
			await Task.Run(() => DeleteMessage(botClient, _message));
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					User user = db.GetUser(_message.From.Id);
					if (IsNullDataBase.IsNull(botClient, _message, user)) return;

					AdUser adUser = db.GetAdUser(user.ID);

					PostTemplate postTemplate = db.GetTempalte(adUser.User.ID, adUser.EditingPostTemplateId);
					adUser.EditingPostTemplateId = -1;

					db.DeletePostTemplate(adUser, postTemplate);
					db.Save();
					await botClient.EditMessageTextAsync(_message.From.Id, _message.Message.MessageId,
					                                     "📞Ваша реклама📞", Telegram.Bot.Types.Enums.ParseMode.Html,
					                                     replyMarkup: InlineButton.GetAdvertising(user));
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class SendToModerationConfirmation : AbsCommand
	{
		public override String Name { get; set; } = CommandsText.MenuAdModeration;

		public override async void Execute(TelegramBotClient botClient, Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase      db       = Singleton.GetInstance().Context;

			try
			{
				User user = db.GetUser(_message.From.Id);
				if (IsNullDataBase.IsNull(botClient, _message, user)) return;
				AdUser       adUser       = db.GetAdUser(user.ID);
				PostTemplate postTemplate = db.GetTempalte(adUser.User.ID, adUser.EditingPostTemplateId);
				if (IsBan.Ban(botClient, message))
				{
					if (postTemplate.Name == "Шаблон")
					{
						botClient.EditMessage(_message.From.Id, user.MessageID,
						                      "Невозможно отправить пост на проверку.\nВы не ввели текст поста.",
						                      "749 - Commands", replyMarkup: InlineButton.GetTemplateMenu(adUser));
						return;
					}

					if (postTemplate.PostChannel.Count < 1)
					{
						botClient.EditMessage(_message.From.Id, user.MessageID,
						                      "Невозможно отправить пост на проверку.\nВы не выбрали ни одного чата для постинга.",
						                      "748 - Commands", replyMarkup: InlineButton.GetTemplateMenu(adUser));
						return;
					}

					if (postTemplate.PostTime.Count < 1)
					{
						botClient.EditMessage(_message.From.Id, user.MessageID,
						                      "Невозможно отправить пост на проверку.\nВы не выбрали время и дату для поста",
						                      "754 - Commands", replyMarkup: InlineButton.GetTemplateMenu(adUser));
						return;
					}

					botClient.EditMessage(_message.From.Id, user.MessageID,
					                      AdController.GetPriceString(postTemplate) +
					                      "\n\nВы действително хотите отправить пост на проверку? (После отправки редактирование будет недоступно)",
					                      "832 - Message on moderation - Commands", user,
					                      replyMarkup: InlineButton.AdValidationKeyboard);
				}
			}
			catch (Exception ex)
			{
				Log.Logging("Failed to sending post to moderation. " + ex);
			}
		}
	}

	internal class SendToValidationConfirmed : AbsCommand
	{
		public override String Name { get; set; } = CommandsText.SendToValidationConfirmed;

		public override async void Execute(TelegramBotClient botClient, Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase      db       = Singleton.GetInstance().Context;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					User user = db.GetUser(_message.From.Id);
					if (IsNullDataBase.IsNull(botClient, _message, user)) return;
					AdUser       adUser       = db.GetAdUser(user.ID);
					PostTemplate postTemplate = db.GetTempalte(adUser.User.ID, adUser.EditingPostTemplateId);

					postTemplate.IsOnValidation = true;
					db.Save();

					Settings setting = db.GetSettings();
					try
					{
						String temp = "";

						foreach (PostChannel channel in postTemplate.PostChannel)
						{
							temp += channel.Channel.ChannelName + "\n";
						}

						botClient.SendText(setting.ChannelAdmin,
						                   $"Запрос на модерацию поста в группе:\n {temp}\nID: {user.ID}\nОтправитель: {user.FIO}",
						                   replyMarkup: InlineButton.AdminPostValidation(_message, adUser.UserId,
						                                                                 postTemplate.Id));
					}
					catch (Exception ex)
					{
						Log.Logging("It seems like admin channel does not exists. " + ex);
					}

					if (postTemplate.PostContent.Count != 0)
					{
						IOrderedEnumerable<PostContent> orderedList = postTemplate.PostContent.OrderBy(c => c.Order);

						Message              textMessage = null;
						List<InputMediaBase> inputMedias = new List<InputMediaBase>();
						foreach (PostContent content in orderedList)
						{
							Message forward =
								await botClient.ForwardMessageAsync(CommandText.bufferChannelId,
								                                    CommandText.bufferChannelId, content.MessageId);

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

						await botClient.SendMediaGroupAsync(setting.ChannelAdmin, inputMedias);
						await botClient.ForwardMessageAsync(setting.ChannelAdmin, CommandText.bufferChannelId,
						                                    textMessage.MessageId);
					}

					await botClient.EditMessageTextAsync(_message.From.Id, _message.Message.MessageId,
					                                     "Пост был отправлен на модерацию администрации.",
					                                     Telegram.Bot.Types.Enums.ParseMode.Html,
					                                     replyMarkup: InlineButton.GetTemplateMenu(adUser));
				}
				catch (Exception ex)
				{
					Log.Logging("Failed to sending post to moderation. " + ex);
				}
			}
		}
	}

	internal class AdminAccept : AbsCommand
	{
		public override String Name { get; set; } = CommandsText.AdminAccept;

		public override async void Execute(TelegramBotClient botClient, Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase      db       = Singleton.GetInstance().Context;

			try
			{
				User user = db.GetUser(_message.From.Id);
				if (IsNullDataBase.IsNull(botClient, _message, user)) return;
				AdUser       adUser        = db.GetAdUser(System.Convert.ToInt32(Name.Split(" ")[1]));
				System.Int32 postTempateId = System.Convert.ToInt32(Name.Split(" ")[2]);
				Name = CommandsText.AdminAccept;
				if (user.IsAdmin > 1)
				{
					PostTemplate postTemplate = db.GetTempalte(adUser.User.ID, postTempateId);
					postTemplate.IsOnValidation = false;
					postTemplate.IsValidated    = true;

					db.Save();

					Settings setting = db.GetSettings();
					try
					{
						Int32 price = (Int32)AdController.GetTotalPrice(postTemplate);
						botClient.DeleteMessage(setting.ChannelAdmin, _message.Message.MessageId, "1327 - Command");
						botClient.SendText(setting.ChannelAdmin,
						                   $"Администратор @{user.Username} одобрил рекламный пост:\n {postTemplate.Name}");
						botClient.SendText(adUser.User.ID,
						                   $"Ваш запрос был одобрен!\nИмя шаблона:\n {postTemplate.Name}");
						await botClient.SendInvoiceAsync(chatId: adUser.UserId,
						                                 title:
						                                 $"Оплата поста: {postTemplate.Name}\nСтоимость поста: {price} грн!",
						                                 description: AdController.GetPriceString(postTemplate),
						                                 payload: "PostTemplate is correct",
						                                 providerToken: "635983722:LIVE:i29496402528",
						                                 startParameter: "Hello", currency: "UAH",
						                                 prices: AdController.GetLabelPrices(postTemplate),
						                                 replyMarkup: InlineButton.Payment);
					}
					catch (Exception ex)
					{
						Log.Logging("It seems like admin channel does not exists. " + ex);
					}
				}
				else
				{
					botClient.SendText(user.ID, $"Одобрить рекламный пост могут только администраторы 2-го уровня!");
				}
			}
			catch (Exception ex)
			{
				Log.Logging("Failed to sending post to moderation. " + ex);
			}
		}
	}

	internal class AdminCancel : AbsCommand
	{
		public override String Name { get; set; } = CommandsText.AdminCancel;

		public override async void Execute(TelegramBotClient botClient, Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase      db       = Singleton.GetInstance().Context;

			try
			{
				User user = db.GetUser(_message.From.Id);
				if (IsNullDataBase.IsNull(botClient, _message, user)) return;
				AdUser       adUser        = db.GetAdUser(System.Convert.ToInt32(Name.Split(" ")[1]));
				System.Int32 postTempateId = System.Convert.ToInt32(Name.Split(" ")[2]);
				Name = CommandsText.AdminCancel;
				if (user.IsAdmin > 1)
				{
					PostTemplate postTemplate = db.GetTempalte(adUser.User.ID, postTempateId);
					postTemplate.IsOnValidation = false;
					postTemplate.IsValidated    = false;

					db.Save();

					Settings setting = db.GetSettings();
					try
					{
						botClient.DeleteMessage(setting.ChannelAdmin, _message.Message.MessageId, "1327 - Command");

						botClient.SendText(setting.ChannelAdmin,
						                   $"Администратор @{user.Username} отменил рекламный пост {postTemplate.Name}");
						botClient.SendText(adUser.User.ID,
						                   $"Ваш запрос был отклонен!\nИмя шаблона: {postTemplate.Name}");
					}
					catch (Exception ex)
					{
						Log.Logging("It seems like admin channel does not exists. " + ex);
					}
				}
				else
				{
					botClient.SendText(user.ID, $"Отменить рекламный пост могут только администраторы 2-го уровня!");
				}
			}
			catch (Exception ex)
			{
				Log.Logging("Failed to sending post to moderation. " + ex);
			}
		}
	}

	internal class PayPostTemplate : AbsCommand
	{
		public override String Name { get; set; } = CommandsText.PayPostTemplate;

		public override async void Execute(TelegramBotClient botClient, Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase      db       = Singleton.GetInstance().Context;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					User user = db.GetUser(_message.From.Id);
					if (IsNullDataBase.IsNull(botClient, _message, user)) return;
					AdUser       adUser       = db.GetAdUser(user.ID);
					PostTemplate postTemplate = db.GetTempalte(adUser.User.ID, adUser.EditingPostTemplateId);

					Single                                     price = AdController.GetTotalPrice(postTemplate);
					Telegram.Bot.Types.Payments.LabeledPrice[] test  = AdController.GetLabelPrices(postTemplate);
					foreach (Telegram.Bot.Types.Payments.LabeledPrice item in test)
					{
						System.Console.WriteLine(item.Amount);
					}

					if (adUser.Balance >= (price / 100))
					{
						postTemplate.IsPaid =  true;
						adUser.Balance      -= (price / 100);
						db.Save();

						try
						{
							botClient.EditMessage(_message.From.Id, _message.Message.MessageId,
							                      $"Данный шаблон был оплачен!", "687", user,
							                      replyMarkup: InlineButton.BackToAdvertisingMenu);
						}
						catch (Exception ex)
						{
							Log.Logging("It seems like admin channel does not exists. " + ex);
						}
					}
					else
					{
						botClient.DeleteMessage(_message.From.Id, _message.Message.MessageId, "697 - Commands");
						await botClient.SendInvoiceAsync(chatId: _message.From.Id,
						                                 title:
						                                 $"Оплата поста: {postTemplate.Name}\nСтоимость поста: {price} грн!",
						                                 description: AdController.GetPriceString(postTemplate),
						                                 payload: "PostTemplate is correct",
						                                 providerToken: "635983722:LIVE:i29496402528",
						                                 startParameter: "Hello", currency: "UAH",
						                                 prices: AdController.GetLabelPrices(postTemplate),
						                                 replyMarkup: InlineButton.Payment);
					}
				}

				catch (Exception ex)
				{
					Log.Logging("Failed to sending post to moderation. " + ex);
				}
			}
		}
	}

	internal class BackToAdvertisingMenu : AbsCommand
	{
		public override System.String Name { set; get; } = CommandsText.BackToAdvertisingMenu;

		private async void DeleteMessage(TelegramBotClient botClient, CallbackQuery _message)
		{
			for (System.Int32 i = 0; i <= 20; i++)
			{
				try
				{
					await botClient.DeleteMessageAsync(_message.From.Id, _message.Message.MessageId - i);
				}
				catch { }
			}
		}

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase      db       = Singleton.GetInstance().Context;
			User          user;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					user       = db.GetUser(_message.From.Id);
					user.Chain = (System.Int32)SetChain.MessageUserInBot;
					db.Save();
				}
				catch
				{
					await Task.Run(() => DeleteMessage(botClient, _message));
					return;
				}

				InlineKeyboardMarkup mes;
				if (user.IsAdmin >= 2)
				{
					mes = InlineButton.AdvertisingShowAdmin;
				}
				else
				{
					mes = InlineButton.AdvertisingShow;
				}

				try
				{
					await botClient.EditMessageTextAsync(_message.From.Id, _message.Message.MessageId, "Реклама",
					                                     replyMarkup: mes);
				}
				catch
				{
					await botClient.DeleteMessageAsync(_message.From.Id, _message.Message.MessageId);
					await botClient.SendTextMessageAsync(_message.From.Id, "Реклама", replyMarkup: mes);
				}
			}
		}
	}

	internal class ThisChannel
	{
		public async void Execute(TelegramBotClient botClient, System.Object message, System.Int64 channelId)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase      db       = Singleton.GetInstance().Context;
			Channel       channel  = db.GetChannel(channelId);
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					await botClient.EditMessageTextAsync(_message.From.Id, _message.Message.MessageId,
					                                     "Чат: " + channel.ChannelName + "\nСсылка: " +
					                                     channel.InviteLink,
					                                     replyMarkup: InlineButton.ThisChannel(channelId));
				}
				catch
				{
				}
			}
		}
	}

	internal class BackToChannelMenu : AbsCommand
	{
		public override System.String Name { set; get; } = CommandsText.BackToChannelMenu;

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase      db       = Singleton.GetInstance().Context;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					InlineKeyboardMarkup answer = InlineButton.GetChat();
					await botClient.EditMessageTextAsync(_message.From.Id, _message.Message.MessageId, "Чаты: ",
					                                     Telegram.Bot.Types.Enums.ParseMode.Html, replyMarkup: answer);
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class PaymentCount : AbsCommand
	{
		public override System.String Name { set; get; } = CommandsText.PaymentCount;

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase      db       = Singleton.GetInstance().Context;
			User          user     = db.GetUser(_message.From.Id);
			if (IsNullDataBase.IsNull(botClient, _message, user)) return;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					Message mes = await botClient.EditMessageTextAsync(_message.From.Id, _message.Message.MessageId,
					                                                   "Введите сумму пополнения: (грн)",
					                                                   replyMarkup: InlineButton.BackToAdvertisingMenu);
					user.MessageID = mes.MessageId;
					user.Chain     = (System.Int32)SetChain.GetPayments;
					db.Save();
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class ContentKeyboard : AbsCommand
	{
		public override System.String Name { set; get; } = CommandsText.ContentEditorImg;

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase      db       = Singleton.GetInstance().Context;
			User          user     = db.GetUser(_message.From.Id);
			if (IsNullDataBase.IsNull(botClient, _message, user)) return;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					Message mes = await botClient.EditMessageTextAsync(_message.From.Id, _message.Message.MessageId,
					                                                   "Введите сумму пополнения: (грн)",
					                                                   replyMarkup: InlineButton.BackToAdvertisingMenu);
					user.MessageID = mes.MessageId;
					user.Chain     = (System.Int32)SetChain.GetPayments;
					db.Save();
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	// CreateAdKeyboardTypeMessage

	internal class ClickDateTime_Result : AbsCommand
	{
		public override System.String Name { get; set; } = CommandsText.ClickDateTime_Result;

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase      db       = Singleton.GetInstance().Context;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					WorkingTimeAndDate.Clear(_message.From.Id);
					db.Save();
					await botClient.EditMessageTextAsync(_message.From.Id, _message.Message.MessageId,
					                                     "Дата и время сохранены!!!",
					                                     replyMarkup: InlineButton.BackToAdvertisingMenu);
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	#region Working Time

	internal class CreateAdKeyboardTypeMessage : AbsCommand, ISplitName, IStandartCommand
	{
		public override System.String Name { get; set; } = CommandsText.CreateAdKeyboardTypeMessage;

		private DataBase db = null;
		private User user = null;
		private CallbackQuery _message = null;
		private PostTemplate postTemplate = null;

		public override void Execute(TelegramBotClient botClient, System.Object message)
		{
			IStandartCommand standartCommand = new CreateAdKeyboardTypeMessage();
			ISplitName       splitName       = new CreateAdKeyboardTypeMessage();

			if (standartCommand.SetCallbackQuery(message, out _message)) return;

			if (standartCommand.SetDataBase(out db)) return;

			if (standartCommand.SetUserAndCheckIsNull(botClient, _message, out user, db)) return;

			Int32 idPostTemplate = splitName.GetNameSplit(Name);
			Name = CommandsText.CreateAdKeyboardTypeMessage;
			if (IsBan.Ban(botClient, message))
			{
				if (GetPostTemplate(idPostTemplate)) return;

				try
				{
					SendMessage(botClient);
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}

		private Boolean GetPostTemplate(Int32 idPostTemplate)
		{
			postTemplate = db.GetTempalte(user.ID, idPostTemplate);
			return postTemplate == null;
		}

		public void SendMessage(TelegramBotClient botClient) =>
			botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "Тип сообщения",
			                      "1259 - CreateAdKeyboardTypeMessage",
			                      replyMarkup: InlineButton.CreateAdKeyboardTypeMessage(postTemplate));
	}

	#endregion Working Time

	#region AddTimeBot

	internal class AddTimeButton : AbsCommand
	{
		public override System.String Name { get; set; } = CommandsText.AddTimeButton;

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase      db       = Singleton.GetInstance().Context;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					//WorkingTimeBot.users_time[_message.From.Id].Add_DB(System.DateTime.Today);
					User user = db.GetUser(_message.From.Id);
					if (IsNullDataBase.IsNull(botClient, _message, user)) return;

					AdUser          adUser       = db.GetAdUser(user.ID);
					PostTemplate    postTemplate = db.GetTempalte(adUser.User.ID, adUser.EditingPostTemplateId);
					System.DateTime time         = new System.DateTime(1, 1, 1, System.DateTime.Now.Hour, 30, 0);
					db.AddPostTimeForPostTemplate(postTemplate, adUser,
					                              new PostTime() {Time = time, PostTemplateId = postTemplate.Id});
					db.Save();
					await botClient.EditMessageTextAsync(_message.From.Id, _message.Message.MessageId,
					                                     "Добавление времени",
					                                     replyMarkup: InlineButton.AddTime(_message.From.Id, " ",
					                                                                       db.GetPostTime(adUser, time,
					                                                                                      postTemplate,
					                                                                                      -1)));
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	} // WorkingTimeBot.users_time[_message.From.Id].timeModels

	internal class AddTime : AbsCommand
	{
		public override System.String Name { get; set; } = CommandsText.AddTime;

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			// WorkingTimeBot.users_time.Add(_message.From.Id);
			System.String postName = " ";

			CallbackQuery _message = message as CallbackQuery;
			//CallbackQuery.Data

			DataBase db = Singleton.GetInstance().Context;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					User user = db.GetUser(_message.From.Id);
					if (IsNullDataBase.IsNull(botClient, _message, user)) return;

					AdUser       adUser       = db.GetAdUser(user.ID);
					PostTemplate postTemplate = db.GetTempalte(adUser.User.ID, adUser.EditingPostTemplateId);
					db.Save();
					await botClient.EditMessageTextAsync(_message.From.Id, _message.Message.MessageId, "Время:",
					                                     replyMarkup: InlineButton.TimeShow(_message.From.Id, postName,
					                                                                        db
						                                                                        .GetPostTimesToDate(adUser,
						                                                                                            postTemplate),
					                                                                        postTemplate));
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class TimeShow : AbsCommand
	{
		public override System.String Name { get; set; } = CommandsText.ShowTime;

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			System.String postName = " ";

			CallbackQuery _message = message as CallbackQuery;
			if (IsBan.Ban(botClient, message))
			{
				DataBase db   = Singleton.GetInstance().Context;
				User     user = db.GetUser(_message.From.Id);
				if (IsNullDataBase.IsNull(botClient, _message, user)) return;

				AdUser       adUser       = db.GetAdUser(user.ID);
				PostTemplate postTemplate = db.GetTempalte(adUser.User.ID, adUser.EditingPostTemplateId);

				Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup answer =
					InlineButton.TimeShow(_message.From.Id, postName, db.GetPostTimesToDate(adUser, postTemplate),
					                      postTemplate);
				try
				{
					db.Save();
					await botClient.EditMessageTextAsync(_message.From.Id, _message.Message.MessageId, "Даты:",
					                                     replyMarkup: answer);
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class BackTimeShowForAddTime : AbsCommand
	{
		public override String Name { get; set; } = CommandsText.Back_AddTime;

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			DataBase      db       = Singleton.GetInstance().Context;
			CallbackQuery _message = message as CallbackQuery;
			if (IsBan.Ban(botClient, message))
			{
				System.String temp_string = Name.Split(" ")[1];
				User          user        = db.GetUser(_message.From.Id);
				if (IsNullDataBase.IsNull(botClient, _message, user)) return;

				AdUser       adUser       = db.GetAdUser(user.ID);
				PostTemplate postTemplate = db.GetTempalte(adUser.User.ID, adUser.EditingPostTemplateId);
				System.DateTime time = DateTime.ParseExact("05/01/2009 " + temp_string, "MM/dd/yyyy HH:mm:ss",
				                                           new CultureInfo("en-US"), DateTimeStyles.None);
				PostTime       find_time = db.GetPostTime(adUser, time, postTemplate, -1);
				List<PostTime> times     = db.GetPostTimesToDate(adUser, postTemplate);
				try
				{
					if (find_time != null)
					{
						if (times?.Where(p => p.UseTime == true).Count() > 1 &&
						    times.Any(p => p.Time.Minute == find_time.Time.Minute &&
						                   p.Time.Hour   == find_time.Time.Hour   && p.IdDateTime != -1 &&
						                   p.IsDate      == false))
						{
							db.DeleteReplayTimePostTime(adUser, postTemplate, find_time);
							db.Save();
						}
						else
						{
							db.UpdateIdDateTimeInPostTime(adUser, postTemplate);
							db.Save();
						}
					}

					db.CheckIdDateTimeInPostTime(adUser, postTemplate, db.getPostTime(adUser, time, postTemplate));

					Name = CommandsText.Back_AddTime;

					db.Save();
					await botClient.EditMessageTextAsync(_message.From.Id, _message.Message.MessageId, "Даты",
					                                     replyMarkup: InlineButton.TimeShow(_message.From.Id, " ",
					                                                                        db
						                                                                        .GetPostTimesToDate(adUser,
						                                                                                            postTemplate),
					                                                                        postTemplate));
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class LeftMinute : AbsCommand
	{
		public override System.String Name { get; set; } = CommandsText.LeftMinuteButton;

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message    = message as CallbackQuery;
			System.String temp_string = Name.Split(" ")[1];
			DataBase      db          = Singleton.GetInstance().Context;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					User user = db.GetUser(_message.From.Id);
					if (IsNullDataBase.IsNull(botClient, _message, user)) return;

					AdUser       adUser       = db.GetAdUser(user.ID);
					PostTemplate postTemplate = db.GetTempalte(adUser.User.ID, adUser.EditingPostTemplateId);
					PostTime date =
						db.GetPostTime(adUser,
						               DateTime.ParseExact("05/01/2009 " + temp_string, "MM/dd/yyyy HH:mm:ss",
						                                   new CultureInfo("en-US"), DateTimeStyles.None), postTemplate,
						               Convert.ToInt32(Name.Split(" ")[2]));
					Name = CommandsText.LeftMinuteButton;
					db.SubMinutePostTime(date, adUser, postTemplate);
					db.Save();
					// this.Equals(WorkingTime.CommandText.LeftMinuteButton + " " + temp.Time.TimeOfDay.ToString());
					await botClient.EditMessageTextAsync(_message.From.Id, _message.Message.MessageId,
					                                     "Добавление времени",
					                                     replyMarkup: InlineButton.AddTime(_message.From.Id, " ",
					                                                                       date));
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class LeftHour : AbsCommand
	{
		public override System.String Name { get; set; } = CommandsText.LeftHourButton;

		//System.Int32 hour = 1;
		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			System.String temp_string = Name.Split(" ")[1];

			CallbackQuery _message = message as CallbackQuery;
			DataBase      db       = Singleton.GetInstance().Context;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					User user = db.GetUser(_message.From.Id);
					if (IsNullDataBase.IsNull(botClient, _message, user)) return;

					AdUser       adUser       = db.GetAdUser(user.ID);
					PostTemplate postTemplate = db.GetTempalte(adUser.User.ID, adUser.EditingPostTemplateId);
					PostTime temp =
						db.GetPostTime(adUser,
						               DateTime.ParseExact("05/01/2009 " + temp_string, "MM/dd/yyyy HH:mm:ss",
						                                   new CultureInfo("en-US"), DateTimeStyles.None), postTemplate,
						               Convert.ToInt32(Name.Split(" ")[2]));
					Name = CommandsText.LeftHourButton;
					db.SubHoursPostTime(temp, adUser, postTemplate);
					db.Save();
					await botClient.EditMessageTextAsync(_message.From.Id, _message.Message.MessageId,
					                                     "Добавление времени",
					                                     replyMarkup: InlineButton.AddTime(_message.From.Id, " ",
					                                                                       temp));
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class RightMinute : AbsCommand
	{
		public override System.String Name { get; set; } = CommandsText.RightMinuteButton;

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			System.String temp_string = Name.Split(" ")[1];

			CallbackQuery _message = message as CallbackQuery;
			DataBase      db       = Singleton.GetInstance().Context;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					User user = db.GetUser(_message.From.Id);
					if (IsNullDataBase.IsNull(botClient, _message, user)) return;

					AdUser       adUser       = db.GetAdUser(user.ID);
					PostTemplate postTemplate = db.GetTempalte(adUser.User.ID, adUser.EditingPostTemplateId);
					PostTime date =
						db.GetPostTime(adUser,
						               DateTime.ParseExact("05/01/2009 " + temp_string, "MM/dd/yyyy HH:mm:ss",
						                                   new CultureInfo("en-US"), DateTimeStyles.None), postTemplate,
						               Convert.ToInt32(Name.Split(" ")[2]));
					Name = CommandsText.RightMinuteButton;
					db.AddMinutePostTime(date, adUser, postTemplate);
					db.Save();
					await botClient.EditMessageTextAsync(_message.From.Id, _message.Message.MessageId,
					                                     "Добавление времени",
					                                     replyMarkup: InlineButton.AddTime(_message.From.Id, " ",
					                                                                       date));
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class RightHour : AbsCommand
	{
		public override System.String Name { get; set; } = CommandsText.RightHourButton;

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			DataBase      db       = Singleton.GetInstance().Context;
			if (IsBan.Ban(botClient, message))
			{
				System.String temp_string = Name.Split(" ")[1];

				db.Save();
				try
				{
					User user = db.GetUser(_message.From.Id);
					if (IsNullDataBase.IsNull(botClient, _message, user)) return;

					AdUser       adUser       = db.GetAdUser(user.ID);
					PostTemplate postTemplate = db.GetTempalte(adUser.User.ID, adUser.EditingPostTemplateId);
					PostTime date =
						db.GetPostTime(adUser,
						               DateTime.ParseExact("05/01/2009 " + temp_string, "MM/dd/yyyy HH:mm:ss",
						                                   new CultureInfo("en-US"), DateTimeStyles.None), postTemplate,
						               Convert.ToInt32(Name.Split(" ")[2]));
					Name = CommandsText.RightHourButton;
					db.AddHoursPostTime(date, adUser, postTemplate);

					db.Save();
					await botClient.EditMessageTextAsync(_message.From.Id, _message.Message.MessageId,
					                                     "Добавление времени",
					                                     replyMarkup: InlineButton.AddTime(_message.From.Id, " ",
					                                                                       date));
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	#endregion AddTimeBot

	#region ChangeTime

	internal class ChangeTime : AbsCommand
	{
		public override System.String Name { get; set; } = CommandsText.ChangeTimeButton;

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			System.String temp_string = Name.Split(" ")[1];
			CallbackQuery _message    = message as CallbackQuery;
			if (IsBan.Ban(botClient, message))
			{
				DataBase db   = Singleton.GetInstance().Context;
				User     user = db.GetUser(_message.From.Id);
				if (IsNullDataBase.IsNull(botClient, _message, user)) return;

				AdUser adUser = db.GetAdUser(user.ID);

				PostTemplate postTemplate = db.GetTempalte(adUser.User.ID, adUser.EditingPostTemplateId);
				PostTime time =
					db.getPostTime(adUser,
					               DateTime.ParseExact("05/01/2009 " + temp_string, "MM/dd/yyyy HH:mm:ss",
					                                   new CultureInfo("en-US"), DateTimeStyles.None), postTemplate);

				Name = CommandsText.ChangeTimeButton;
				try
				{
					await botClient.EditMessageTextAsync(_message.From.Id, _message.Message.MessageId,
					                                     "Изменение даты:",
					                                     replyMarkup: InlineButton.ChangeTimeShow(_message.From.Id, " ",
					                                                                              time));
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class DeleteTime : AbsCommand
	{
		public override System.String Name { get; set; } = CommandsText.DeleteTime;

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message    = message as CallbackQuery;
			System.String temp_string = Name.Split(" ")[1];

			DataBase db = Singleton.GetInstance().Context;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					User user = db.GetUser(_message.From.Id);
					if (IsNullDataBase.IsNull(botClient, _message, user)) return;

					AdUser       adUser       = db.GetAdUser(user.ID);
					PostTemplate postTemplate = db.GetTempalte(adUser.User.ID, adUser.EditingPostTemplateId);
					db.DeleteTimePostTime(adUser, postTemplate,
					                      db.GetPostTime(adUser,
					                                     DateTime.ParseExact("05/01/2009 " + temp_string,
					                                                         "MM/dd/yyyy HH:mm:ss",
					                                                         new CultureInfo("en-US"),
					                                                         DateTimeStyles.None), postTemplate,
					                                     Convert.ToInt32(Name.Split(" ")[2])));
					Name = CommandsText.DeleteTime;
					db.Save();
					postTemplate = db.GetTempalte(adUser.User.ID, adUser.EditingPostTemplateId);
					await botClient.EditMessageTextAsync(_message.From.Id, _message.Message.MessageId, "Дата удалена",
					                                     replyMarkup: InlineButton.TimeShow(_message.From.Id, " ",
					                                                                        db
						                                                                        .GetPostTimesToDate(adUser,
						                                                                                            postTemplate),
					                                                                        postTemplate));
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class BackToTime : AbsCommand
	{
		public override System.String Name { get; set; } = CommandsText.BackToTime;

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			Name = CommandsText.BackToTime;
			DataBase      db       = Singleton.GetInstance().Context;
			CallbackQuery _message = message as CallbackQuery;
			db.Save();
			//	WorkingTime.WorkingTimeBot.users_time[_message.From.Id].Change_DB(new PostTime() { Time = System.Convert.ToDateTime(temp_string) }, temp_id);
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					User user = db.GetUser(_message.From.Id);
					if (IsNullDataBase.IsNull(botClient, _message, user)) return;

					AdUser       adUser       = db.GetAdUser(user.ID);
					PostTemplate postTemplate = db.GetTempalte(adUser.User.ID, adUser.EditingPostTemplateId);
					await botClient.EditMessageTextAsync(_message.From.Id, _message.Message.MessageId, "Даты",
					                                     replyMarkup: InlineButton.TimeShow(_message.From.Id, " ",
					                                                                        db
						                                                                        .GetPostTimesToDate(adUser,
						                                                                                            postTemplate),
					                                                                        postTemplate));
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class LeftMinute_Change : AbsCommand
	{
		public override System.String Name { get; set; } = CommandsText.LeftMinuteButton_Change;

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			System.String temp_string = (Name.Split(" ")[1]);

			CallbackQuery _message = message as CallbackQuery;
			//WorkingTime.WorkingTimeBot.users_time[_message.From.Id].Change_DB(new PostTime() { Time = System.Convert.ToDateTime(temp_string) }, temp_id);

			DataBase db = Singleton.GetInstance().Context;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					User user = db.GetUser(_message.From.Id);
					if (IsNullDataBase.IsNull(botClient, _message, user)) return;

					AdUser       adUser       = db.GetAdUser(user.ID);
					PostTemplate postTemplate = db.GetTempalte(adUser.User.ID, adUser.EditingPostTemplateId);
					PostTime date =
						db.GetPostTime(adUser,
						               DateTime.ParseExact("05/01/2009 " + temp_string, "MM/dd/yyyy HH:mm:ss",
						                                   new CultureInfo("en-US"), DateTimeStyles.None), postTemplate,
						               Convert.ToInt32(Name.Split(" ")[2]));
					Name = CommandsText.LeftMinuteButton_Change;

					db.SubMinutePostTime(date, adUser, postTemplate);
					db.Save();

					await botClient.EditMessageTextAsync(_message.From.Id, _message.Message.MessageId, "Изменение даты",
					                                     replyMarkup: InlineButton.ChangeTimeShow(_message.From.Id, " ",
					                                                                              date));
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class LeftHour_Change : AbsCommand
	{
		public override System.String Name { get; set; } = CommandsText.LeftHourButton_Change;

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			System.String temp_string = Name.Split(" ")[1];

			CallbackQuery _message = message as CallbackQuery;
			DataBase      db       = Singleton.GetInstance().Context;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					User user = db.GetUser(_message.From.Id);
					if (IsNullDataBase.IsNull(botClient, _message, user)) return;

					AdUser       adUser       = db.GetAdUser(user.ID);
					PostTemplate postTemplate = db.GetTempalte(adUser.User.ID, adUser.EditingPostTemplateId);
					PostTime date =
						db.GetPostTime(adUser,
						               DateTime.ParseExact("05/01/2009 " + temp_string, "MM/dd/yyyy HH:mm:ss",
						                                   new CultureInfo("en-US"), DateTimeStyles.None), postTemplate,
						               Convert.ToInt32(Name.Split(" ")[2]));
					Name = CommandsText.LeftHourButton_Change;
					db.SubHoursPostTime(date, adUser, postTemplate);
					db.Save();

					await botClient.EditMessageTextAsync(_message.From.Id, _message.Message.MessageId, "Изменение даты",
					                                     replyMarkup: InlineButton.ChangeTimeShow(_message.From.Id, " ",
					                                                                              date));
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class RightMinute_Change : AbsCommand
	{
		public override System.String Name { get; set; } = CommandsText.RightMinuteButton_Change;

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			System.String temp_string = Name.Split(" ")[1];

			CallbackQuery _message = message as CallbackQuery;
			DataBase      db       = Singleton.GetInstance().Context;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					User user = db.GetUser(_message.From.Id);
					if (IsNullDataBase.IsNull(botClient, _message, user)) return;

					AdUser       adUser       = db.GetAdUser(user.ID);
					PostTemplate postTemplate = db.GetTempalte(adUser.User.ID, adUser.EditingPostTemplateId);
					PostTime date =
						db.GetPostTime(adUser,
						               DateTime.ParseExact("05/01/2009 " + temp_string, "MM/dd/yyyy HH:mm:ss",
						                                   new CultureInfo("en-US"), DateTimeStyles.None), postTemplate,
						               Convert.ToInt32(Name.Split(" ")[2]));
					Name = CommandsText.RightMinuteButton_Change;
					db.AddMinutePostTime(date, adUser, postTemplate);
					db.Save();
					await botClient.EditMessageTextAsync(_message.From.Id, _message.Message.MessageId, "Изменение даты",
					                                     replyMarkup: InlineButton.ChangeTimeShow(_message.From.Id, " ",
					                                                                              date));
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class RightHour_Change : AbsCommand
	{
		public override System.String Name { get; set; } = CommandsText.RightHourButton_Change;

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			DataBase      db       = Singleton.GetInstance().Context;
			CallbackQuery _message = message as CallbackQuery;
			if (IsBan.Ban(botClient, message))
			{
				System.String temp_string = Name.Split(" ")[1];

				try
				{
					User user = db.GetUser(_message.From.Id);
					if (IsNullDataBase.IsNull(botClient, _message, user)) return;

					AdUser       adUser       = db.GetAdUser(user.ID);
					PostTemplate postTemplate = db.GetTempalte(adUser.User.ID, adUser.EditingPostTemplateId);
					PostTime date =
						db.GetPostTime(adUser,
						               DateTime.ParseExact("05/01/2009 " + temp_string, "MM/dd/yyyy HH:mm:ss",
						                                   new CultureInfo("en-US"), DateTimeStyles.None), postTemplate,
						               Convert.ToInt32(Name.Split(" ")[2]));
					Name = CommandsText.RightHourButton_Change;
					db.AddHoursPostTime(date, adUser, postTemplate);
					db.Save();

					await botClient.EditMessageTextAsync(_message.From.Id, _message.Message.MessageId, "Изменение даты",
					                                     replyMarkup: InlineButton.ChangeTimeShow(_message.From.Id, " ",
					                                                                              date));
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	#endregion ChangeTime

	#region Calendar

	internal class CalendarBot : AbsCommand
	{
		public override System.String Name { get; set; } = CommandsText.Calendar;

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			if (IsBan.Ban(botClient, message))
			{
				DataBase db = Singleton.GetInstance().Context;
				if (Bot.users_calendar.All(p => p.Key != _message.From.Id))
				{
					Bot.users_calendar.Add(_message.From.Id, new Calendar.Calendar());
				}

				User user = db.GetUser(_message.From.Id);
				if (IsNullDataBase.IsNull(botClient, _message, user)) return;

				AdUser adUser = db.GetAdUser(user.ID);

				PostTemplate postTemplate = db.GetTempalte(adUser.User.ID, adUser.EditingPostTemplateId);
				if (postTemplate.isPinnedMessage != (Int32)BotCore.TypePostTime.StandartMessage)
				{
					db.DeleteAllPostTimeOfPostTemplate(adUser, postTemplate);
					postTemplate = db.GetTempalte(adUser.User.ID, adUser.EditingPostTemplateId);
				}

				db.Save();
				db.ChangeTempalte(adUser.User.ID, adUser.EditingPostTemplateId, (Int32)TypePostTime.StandartMessage);
				//  WorkingTimeAndDate.Start(_message.From.Id, postTemplate);

				List<PostTime> times = db.GetPostTimesToDate(adUser, postTemplate);

				Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup answer =
					InlineButton.CalendarShow(times, _message.From.Id, _message.From.Id,
					                          Bot.users_calendar[_message.From.Id], postTemplate.Id);
				try
				{
					await botClient.EditMessageTextAsync(_message.From.Id, _message.Message.MessageId, "Выберете дату:",
					                                     replyMarkup: answer);
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class Next_Calendar : AbsCommand, Calendar.ICalendar
	{
		public override System.String Name { get; set; } = CommandsText.Calendar_Next;

		public System.Boolean Equals(String CommandName) => Name.Equals(CommandName);

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			Dictionary<System.Int32, Calendar.Calendar> keyValues2 = Bot.users_calendar;
			CallbackQuery                               _message   = message as CallbackQuery;
			if (IsBan.Ban(botClient, message))
			{
				DataBase db   = Singleton.GetInstance().Context;
				User     user = db.GetUser(_message.From.Id);
				if (IsNullDataBase.IsNull(botClient, _message, user)) return;

				AdUser adUser = db.GetAdUser(user.ID);

				PostTemplate postTemplate = db.GetTempalte(adUser.User.ID, adUser.EditingPostTemplateId);
				keyValues2[_message.From.Id].Next();
				Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup answer =
					InlineButton.CalendarShow(db.GetPostTimesToDate(adUser, postTemplate), _message.From.Id, -1,
					                          Bot.users_calendar[_message.From.Id], postTemplate.Id);
				try
				{
					await botClient.EditMessageTextAsync(_message.From.Id, _message.Message.MessageId, "Выберете дату:",
					                                     replyMarkup: answer);
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class ChangeDate : AbsCommand, Calendar.ICalendar
	{
		public override System.String Name { get; set; } = CommandsText.ChangeAllTime;

		public System.Boolean Equals(System.String CommandName) => Name.Equals(CommandName);

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			Dictionary<System.Int32, Calendar.Calendar> keyValues2 = Bot.users_calendar;
			CallbackQuery                               _message   = message as CallbackQuery;
			if (IsBan.Ban(botClient, message))
			{
				DataBase db   = Singleton.GetInstance().Context;
				User     user = db.GetUser(_message.From.Id);
				if (IsNullDataBase.IsNull(botClient, _message, user)) return;

				AdUser adUser = db.GetAdUser(user.ID);

				PostTemplate postTemplate = db.GetTempalte(adUser.User.ID, adUser.EditingPostTemplateId);
				//keyValues2[_message.From.Id].Next();
				Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup answer =
					InlineButton.CalendarShow(db.GetPostTimesToDate(adUser, postTemplate), _message.From.Id, -1,
					                          Bot.users_calendar[_message.From.Id], postTemplate.Id);
				try
				{
					await botClient.EditMessageTextAsync(_message.From.Id, _message.Message.MessageId, "Выберете дату:",
					                                     replyMarkup: answer);
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class NextToTimeMenu_Calendar : AbsCommand, Calendar.ICalendar
	{
		public override System.String Name { get; set; } = CommandsText.NextToTimeMenu_Calendar;

		public System.Boolean Equals(System.String CommandName) => Name.Equals(CommandName);

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			if (IsBan.Ban(botClient, message))
			{
				Dictionary<System.Int32, Calendar.Calendar> keyValues2 = Bot.users_calendar;
				DataBase                                    db         = Singleton.GetInstance().Context;
				User                                        user       = db.GetUser(_message.From.Id);
				if (IsNullDataBase.IsNull(botClient, _message, user)) return;

				AdUser adUser = db.GetAdUser(user.ID);

				PostTemplate   postTemplate = db.GetTempalte(adUser.User.ID, adUser.EditingPostTemplateId);
				List<PostTime> times        = db.GetPostTimesToDate(adUser, postTemplate);
				PostTime find_time =
					times.Where(p => p.IdDateTime == -1 && p.UseTime == true).FirstOrDefault();
				if (find_time != null)
				{
					if (times?.Where(p => p.UseTime == true).Count() > 1 &&
					    times.Any(p => p.IdDateTime == -1 && p.UseTime == true))
					{
						db.DeleteReplayTimePostTime(adUser, postTemplate, find_time);
						db.Save();
					}
				}

				Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup answer =
					InlineButton.TimeShow(_message.From.Id, " ", db.GetPostTimesToDate(adUser, postTemplate),
					                      postTemplate);
				try
				{
					await botClient.EditMessageTextAsync(_message.From.Id, _message.Message.MessageId, "Время:",
					                                     replyMarkup: answer);
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class BackToCalendar : AbsCommand, Calendar.ICalendar, ISplitName, IStandartCommand
	{
		public override System.String Name { get; set; } = CommandsText.BackToCalendar;

		public System.Boolean Equals(System.String CommandName) => Name.Equals(CommandName);

		private DataBase db = null;
		private User user = null;
		private CallbackQuery _message = null;
		private PostTemplate postTemplate = null;

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			IStandartCommand standartCommand = new BackToCalendar();
			ISplitName       splitName       = new BackToCalendar();

			if (standartCommand.SetCallbackQuery(message, out _message)) return;

			if (standartCommand.SetDataBase(out db)) return;

			if (standartCommand.SetUserAndCheckIsNull(botClient, _message, out user, db)) return;

			Int32 idPostTemplate = splitName.GetNameSplit(Name);
			Name = CommandsText.BackToCalendar;
			if (IsBan.Ban(botClient, message))
			{
				if (GetPostTemplate(idPostTemplate)) return;
				try
				{
					SendMessage(botClient);
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}

		private Boolean GetPostTemplate(Int32 idPostTemplate)
		{
			postTemplate = db.GetTempalte(user.ID, idPostTemplate);
			return postTemplate == null;
		}

		public void SendMessage(TelegramBotClient botClient) =>
			botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "Реклама:", "1948 - BackToCalendar",
			                      replyMarkup: InlineButton.CreateAdKeyboardTypeMessage(postTemplate));
	}

	internal class Back_Calendar : AbsCommand, Calendar.ICalendar
	{
		public override System.String Name { get; set; } = CommandsText.Back_Calendar;

		public System.Boolean Equals(System.String CommandName) => Name.Equals(CommandName);

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			if (IsBan.Ban(botClient, message))
			{
				Dictionary<System.Int32, Calendar.Calendar> keyValues2 = Bot.users_calendar;
				DataBase                                    db         = Singleton.GetInstance().Context;
				User                                        user       = db.GetUser(_message.From.Id);
				if (IsNullDataBase.IsNull(botClient, _message, user)) return;

				AdUser adUser = db.GetAdUser(user.ID);

				PostTemplate postTemplate = db.GetTempalte(adUser.User.ID, adUser.EditingPostTemplateId);
				keyValues2[_message.From.Id].Back();

				Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup answer =
					InlineButton.CalendarShow(db.GetPostTimesToDate(adUser, postTemplate), _message.From.Id, -1,
					                          Bot.users_calendar[_message.From.Id], postTemplate.Id);
				try
				{
					await botClient.EditMessageTextAsync(_message.From.Id, _message.Message.MessageId, "Выберете дату:",
					                                     replyMarkup: answer);
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class ChouseData : AbsCommand, Calendar.ICalendar
	{
		public override System.String Name { get; set; } = CommandsText.ChoseDate;

		public System.Boolean Equals(String CommandName) => Name.Equals(CommandName);

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			if (IsBan.Ban(botClient, message))
			{
				Dictionary<System.Int32, Calendar.Calendar> keyValues2 = Bot.users_calendar;
				DataBase                                    db         = Singleton.GetInstance().Context;
				User                                        user       = db.GetUser(_message.From.Id);
				if (IsNullDataBase.IsNull(botClient, _message, user)) return;

				AdUser adUser = db.GetAdUser(user.ID);

				PostTemplate   postTemplate = db.GetTempalte(adUser.User.ID, adUser.EditingPostTemplateId);
				List<PostTime> times        = db.GetPostTimesToDate(adUser, postTemplate);
				if (_message.Data.IndexOf('+') > -1)
				{
					System.DateTime time =
						DateTime.ParseExact(_message.Data.Replace("+", "").Replace('.', '/'), "d/M/yyyy", null);
					db.DeleteDatePostTime(adUser, postTemplate, db.getPostDate(time, adUser, postTemplate));
					db.Save();
				}
				else
				{
					System.DateTime time =
						DateTime.ParseExact(_message.Data.Replace("+", "").Replace(".", "/"), "d/M/yyyy", null);
					db.AddDatePostTimeForPostTemplate(postTemplate, adUser,
					                                  new PostTime() {Time = time, PostTemplateId = postTemplate.Id});
					db.Save();
				}

				Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup answer;

				postTemplate = db.GetTempalte(adUser.User.ID, adUser.EditingPostTemplateId);
				times        = db.GetPostTimesToDate(adUser, postTemplate);
				if (times.Count == 0)
				{
					answer = InlineButton.CalendarShow(db.GetPostTimesToDate(adUser, postTemplate), _message.From.Id,
					                                   Bot.users_calendar[_message.From.Id], postTemplate.Id);
				}
				else
				{
					answer = InlineButton.CalendarShow(db.GetPostTimesToDate(adUser, postTemplate), 0, _message.From.Id,
					                                   Bot.users_calendar[_message.From.Id], postTemplate.Id);
				}

				try
				{
					await botClient.EditMessageTextAsync(_message.From.Id, _message.Message.MessageId, "Выберете дату:",
					                                     replyMarkup: answer);
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	#endregion Calendar

	#region CalendarPinned

	internal class CalendarPinnedBot : AbsCommand
	{
		public override System.String Name { get; set; } = CommandsText.CalendarPinned;

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			if (IsBan.Ban(botClient, message))
			{
				DataBase db = Singleton.GetInstance().Context;
				try
				{
					if (Bot.users_calendar.All(p => p.Key != _message.From.Id))
					{
						Bot.users_calendar.Add(_message.From.Id, new Calendar.Calendar());
					}

					User user = db.GetUser(_message.From.Id);
					if (IsNullDataBase.IsNull(botClient, _message, user)) return;

					AdUser adUser = db.GetAdUser(user.ID);

					PostTemplate postTemplate = db.GetTempalte(adUser.User.ID, adUser.EditingPostTemplateId);

					if (postTemplate.isPinnedMessage == (System.Int32)TypePostTime.StandartMessage)
					{
						db.DeleteAllPostTimeOfPostTemplate(adUser, postTemplate);
						postTemplate = db.GetTempalte(adUser.User.ID, adUser.EditingPostTemplateId);
					}

					db.ChangeTempalte(adUser.User.ID, adUser.EditingPostTemplateId,
					                  Convert.ToInt32(Name.Split(" ")[1]));
					db.Save();
					List<PostTime> times = db.GetPostTimesToDatePinnedMessage(adUser, postTemplate);

					Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup answer =
						InlineButton.CalendarShowPinnedMessage(times, _message.From.Id, _message.From.Id,
						                                       Bot.users_calendar[_message.From.Id], postTemplate.Id);

					Name = CommandsText.CalendarPinned;
					await botClient.EditMessageTextAsync(_message.From.Id, _message.Message.MessageId, "Выберете дату:",
					                                     replyMarkup: answer);
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class ChooseDatePinned : AbsCommand, Calendar.ICalendar
	{
		public override System.String Name { get; set; } = CommandsText.ChooseDatePinned;

		public System.Boolean Equals(String CommandName) => Name.Equals(CommandName);

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			if (IsBan.Ban(botClient, message))
			{
				Dictionary<System.Int32, Calendar.Calendar> keyValues2 = Bot.users_calendar;
				DataBase                                    db         = Singleton.GetInstance().Context;
				User                                        user       = db.GetUser(_message.From.Id);
				if (IsNullDataBase.IsNull(botClient, _message, user)) return;

				AdUser adUser = db.GetAdUser(user.ID);

				PostTemplate   postTemplate = db.GetTempalte(adUser.User.ID, adUser.EditingPostTemplateId);
				List<PostTime> times        = db.GetPinnedToDate(adUser, postTemplate);
				try
				{
					if (_message.Data.IndexOf('+') > -1)
					{
						System.DateTime time =
							DateTime.ParseExact(Name.Split(" ")[1].Replace("+", "").Replace(".", "/"), "d/M/yyyy",
							                    null);
						db.DeleteDatePinned(adUser, postTemplate, db.getPostDate(time, adUser, postTemplate));
						db.Save();
					}
					else
					{
						DateTime dateTime =
							DateTime.ParseExact(Name.Split(" ")[1].Replace("+", "").Replace(".", "/"), "d/M/yyyy",
							                    null);

						db.AddDatePinnedForPostTemplate(postTemplate, adUser,
						                                new PostTime()
						                                {
							                                Time = dateTime, PostTemplateId = postTemplate.Id
						                                });
						db.Save();
					}

					Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup answer;

					postTemplate = db.GetTempalte(adUser.User.ID, adUser.EditingPostTemplateId);
					times        = db.GetPinnedToDate(adUser, postTemplate);

					answer = InlineButton.CalendarShowPinnedMessage(db.GetPinnedToDate(adUser, postTemplate), 0,
					                                                _message.From.Id,
					                                                Bot.users_calendar[_message.From.Id],
					                                                postTemplate.Id);

					Name = CommandsText.ChooseDatePinned;
					try
					{
						await botClient.EditMessageTextAsync(_message.From.Id, _message.Message.MessageId,
						                                     "Вы можете выбрать еще дату:", replyMarkup: answer);
					}
					catch
					{
						botClient.SendText(_message.From.Id, "Вы можете выбрать еще дату:", user, replyMarkup: answer);
					}
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	#endregion CalendarPinned

	#region Advertising Panel Price

	//Sanya

	#region Change Chat

	internal class EditChatPrice : AbsCommand
	{
		public override System.String Name { get; set; } = CommandsText.PriceChat;
		private DataBase db = Singleton.GetInstance().Context;
		private Single price = 0.1f;

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message  = message as CallbackQuery;
			Int64         channelId = Int64.Parse(Name.Split(" ")[1]);
			if (IsBan.Ban(botClient, message))
			{
				Name = CommandsText.PriceChat;
				try
				{
					await botClient.EditMessageTextAsync(_message.From.Id, _message.Message.MessageId, "Изменение чата",
					                                     replyMarkup: InlineButton.ChangePriceChat(channelId, price));
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	} //Start Price
	internal class LeftChangeChat_Price : AbsCommand
	{
		public override System.String Name { get; set; } = CommandsText.LeftChange_PriceChate;
		private Single sub_price = 0.1f;

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message  = message as CallbackQuery;
			DataBase      db        = Singleton.GetInstance().Context;
			Int64         channelId = System.Convert.ToInt64(Name.Split(" ")[2]);
			Single        old_price = System.Convert.ToSingle(Name.Split(" ")[1]);
			Name = CommandsText.LeftChange_PriceChate;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					Single newPrice = old_price - sub_price;
					if (old_price - sub_price < 0)
					{
						newPrice = 0;
					}

					Channel channel = db.GetChannel(channelId);
					channel.Price = newPrice;

					await botClient.EditMessageTextAsync(_message.From.Id, _message.Message.MessageId,
					                                     "Настройка рекламы",
					                                     replyMarkup: InlineButton
						                                     .ChangePriceChat(channelId, newPrice));
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class RightChangeChat_Price : AbsCommand
	{
		public override System.String Name { get; set; } = CommandsText.RightChange_PriceChat;
		private Single add_price = 0.1f;

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message  = message as CallbackQuery;
			DataBase      db        = Singleton.GetInstance().Context;
			Int64         channelId = System.Convert.ToInt64(Name.Split(" ")[2]);
			Single        old_price = System.Convert.ToSingle(Name.Split(" ")[1]);
			Name = CommandsText.RightChange_PriceChat;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					Single newPrice = old_price + add_price;

					Channel channel = db.GetChannel(channelId);
					channel.Price = newPrice;

					await botClient.EditMessageTextAsync(_message.From.Id, _message.Message.MessageId,
					                                     "Настройка рекламы",
					                                     replyMarkup: InlineButton
						                                     .ChangePriceChat(channelId, newPrice));
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class BackOfChangePriceChatInShowChatPrice : AbsCommand
	{
		public override System.String Name { get; set; } = CommandsText.BackOfChangePriceChatInShowChatPrice;
		private Single Result = 0;

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			try
			{
				Result = Convert.ToSingle(Name.Split(" ")[1]);
				Name   = CommandsText.BackOfChangePriceChatInShowChatPrice;
				await botClient.EditMessageTextAsync(_message.From.Id, _message.Message.MessageId, "Настройка рекламы",
				                                     replyMarkup: InlineButton.OptionAdvertisingShowChats());
			}
			catch (System.Exception ex)
			{
				Log.Logging(ex);
			}
		}
	} // Result Change Chat

	#endregion Change Chat

	#region StaticPriceStandartMessage

	internal class EditStaticTimeStandartMessage : AbsCommand
	{
		public override System.String Name { get; set; } = CommandsText.StaticPriceStandartMessage;
		private DataBase db = null;
		private User user = null;
		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			Single        price    = AdController.PriceData.postTypeDefault;
			CallbackQuery _message = message as CallbackQuery;
			db = Singleton.GetInstance().Context;
			user = db.GetUser(_message.From.Id);
			
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					changeUser();
					await botClient.EditMessageTextAsync(_message.From.Id, _message.Message.MessageId,
					                                     "Общая цена за стандартное сообщение\nВы так же можете ввести цену вручную [10]!",
					                                     replyMarkup: InlineButton.ChangePriceStandartMessage(price));
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
		
		private void changeUser()
		{
			user.Chain = (Int32)SetChain.SetPriceStandartMessage;
			db.Save();
		}
	} // Start Price

	internal class LeftChangeStaticStandartMessage : AbsCommand
	{
		public override System.String Name { get; set; } = CommandsText.LeftStaticChangeStandartMessage;
		private Single sub_price = 0.1f;

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message  = message as CallbackQuery;
			Single        old_price = System.Convert.ToSingle(Name.Split(" ")[1]);
			Name = CommandsText.LeftStaticChangeStandartMessage;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					Single newPrice = old_price - sub_price;

					if (newPrice < 0)
					{
						newPrice = 0.1f;
					}

					newPrice                               = (Single)Math.Round(newPrice, 1);
					AdController.PriceData.postTypeDefault = newPrice;

					AdController.SavePriceData();

					await botClient.EditMessageTextAsync(_message.From.Id, _message.Message.MessageId,
					                                     "Общая цена за стандартное сообщение:",
					                                     replyMarkup: InlineButton
						                                     .ChangePriceStandartMessage(newPrice));
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class RightChangeStaticStandartMessage : AbsCommand
	{
		public override System.String Name { get; set; } = CommandsText.RightStaticChangeStandartMessage;
		private Single add_price = 0.1f;

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message  = message as CallbackQuery;
			Single        old_price = System.Convert.ToSingle(Name.Split(" ")[1]);
			Name = CommandsText.RightStaticChangeStandartMessage;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					Single newPrice = old_price + add_price;
					newPrice                               = (Single)Math.Round(newPrice, 1);
					AdController.PriceData.postTypeDefault = newPrice;

					AdController.SavePriceData();

					await botClient.EditMessageTextAsync(_message.From.Id, _message.Message.MessageId,
					                                     "Общая цена за стандартное сообщение:",
					                                     replyMarkup: InlineButton
						                                     .ChangePriceStandartMessage(newPrice));
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	#endregion StaticPriceStandartMessage

	#region Change Genegal Time

	internal class EditStaticTimePrice : AbsCommand
	{
		public override System.String Name { get; set; } = CommandsText.StaticPriceTime;

		private User user = null;
		private DataBase db = null;
		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			Single        price    = AdController.PriceData.postTypeTime;
			CallbackQuery _message = message as CallbackQuery;
			db = Singleton.GetInstance().Context;
			user = db.GetUser(_message.From.Id);
			
			if(user == null) return;
			
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					changeUser();
					botClient.EditMessage(_message.From.Id, user.MessageID, 
					                                     "Общая цена за стандартное сообщение: \nВы так же можете ввести цену вручную [10]!",
					                                     "",
					                                     user,
					                                     replyMarkup: InlineButton.ChangePriceTime(price));
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}

		private void changeUser()
		{
			user.Chain = (Int32)SetChain.SetPriceTime;
			db.Save();
		}
	} // Start Price

	internal class LeftChangeStaticTime_Price : AbsCommand
	{
		public override System.String Name { get; set; } = CommandsText.LeftStaticChange_PriceTime;
		private Single sub_price = 10;

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message  = message as CallbackQuery;
			Single        old_price = System.Convert.ToSingle(Name.Split(" ")[1]);
			Name = CommandsText.LeftStaticChange_PriceTime;

			var db = Singleton.GetInstance().Context;
			User user = db.GetUser(_message.From.Id);

			if(user == null) return;
			
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					Single newPrice = old_price - sub_price;

					if (newPrice < 0)
					{
						newPrice = 0;
					}

					newPrice                            = (Single)Math.Round(newPrice, 1);
					AdController.PriceData.postTypeTime = newPrice;

					AdController.SavePriceData();

					botClient.EditMessage(_message.From.Id, user.MessageID, 
					                      "Общая цена за стандартное сообщение:",
					                      "",
					                      user,
					                      replyMarkup: InlineButton.ChangePriceTime(newPrice));
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class RightChangeStaticTime_Price : AbsCommand
	{
		public override System.String Name { get; set; } = CommandsText.RightStaticChange_PriceTime;
		private Single add_price = 10;

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message  = message as CallbackQuery;
			Single        old_price = System.Convert.ToSingle(Name.Split(" ")[1]);
			Name = CommandsText.RightStaticChange_PriceTime;
			var  db   = Singleton.GetInstance().Context;
			User user = db.GetUser(_message.From.Id);
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					Single newPrice = old_price + add_price;

					newPrice                            = (Single)Math.Round(newPrice, 1);
					AdController.PriceData.postTypeTime = newPrice;

					AdController.SavePriceData();

					botClient.EditMessage(_message.From.Id, user.MessageID, 
					                      "Общая цена за стандартное сообщение:",
					                      "",
					                      user,
					                      replyMarkup: InlineButton.ChangePriceTime(newPrice));
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class BackOfChangePriceTimeInMenu : AbsCommand
	{
		public override System.String Name { get; set; } = CommandsText.BackOfChangePriceStaticTimeInMenu;
		private Single Result = 0;

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			var db = Singleton.GetInstance().Context;
			User user = db.GetUser(_message.From.Id);
			
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					Result = Convert.ToSingle(Name.Split(" ")[1]);
					Name   = CommandsText.BackOfChangePriceStaticTimeInMenu;
					
					user.Chain = (Int32)SetChain.MessageUserInBot;
					db.Save();
					
					await botClient.EditMessageTextAsync(_message.From.Id, _message.Message.MessageId,
					                                     "Настройка рекламы",
					                                     replyMarkup: InlineButton.OptionAdvertising);
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	} // Result Change Genegal Time

	#endregion Change Genegal Time

	#region Change General Chats

	internal class EditStaticChatPrice : AbsCommand
	{
		public override System.String Name { get; set; } = CommandsText.StaticPriceChat;
		private DataBase db = Singleton.GetInstance().Context;
		private Single price = 0;
		private User user = null;

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			
			user = db.GetUser(_message.From.Id);
			if(user == null) return;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					changeUser();
					await botClient.EditMessageTextAsync(_message.From.Id, _message.Message.MessageId,
					                                     "Изменение цены чата\nВы так же можете ввести цену вручную [10]!",
					                                     replyMarkup: InlineButton.ChangePriceStaticChat(price));
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
		
		private void changeUser()
		{
			user.Chain = (Int32)SetChain.SetPriceStatic;
			db.Save();
		}
	} // Start Price

	internal class LeftChangeStaticChat_Price : AbsCommand
	{
		public override System.String Name { get; set; } = CommandsText.LeftStaticChange_PriceChat;
		private Single sub_price = 0.1f;

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message  = message as CallbackQuery;
			DataBase      db        = Singleton.GetInstance().Context;
			Single        old_price = System.Convert.ToSingle(Name.Split(" ")[1]);
			Name = CommandsText.LeftStaticChange_PriceChat;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					Single newPrice = old_price - sub_price;
					if (old_price - sub_price < 0)
					{
						newPrice = 0;
					}

					Channel[] channels = db.GetChannels();

					newPrice = (Single)Math.Round(newPrice, 1);
					foreach (Channel channel in channels)
					{
						channel.Price = newPrice;
					}

					db.Save();

					await botClient.EditMessageTextAsync(_message.From.Id, _message.Message.MessageId,
					                                     "Настройка рекламы",
					                                     replyMarkup: InlineButton.ChangePriceStaticChat(newPrice));
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class RightChangeStaticChat_Price : AbsCommand
	{
		public override System.String Name { get; set; } = CommandsText.RightStaticChange_PriceChat;
		private Single add_price = 0.1f;

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message  = message as CallbackQuery;
			DataBase      db        = Singleton.GetInstance().Context;
			Single        old_price = System.Convert.ToSingle(Name.Split(" ")[1]);
			Name = CommandsText.RightStaticChange_PriceChat;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					Single newPrice = old_price + add_price;

					Channel[] channels = db.GetChannels();

					newPrice = (Single)Math.Round(newPrice, 1);
					foreach (Channel channel in channels)
					{
						channel.Price = newPrice;
					}

					db.Save();

					await botClient.EditMessageTextAsync(_message.From.Id, _message.Message.MessageId,
					                                     "Настройка рекламы",
					                                     replyMarkup: InlineButton.ChangePriceStaticChat(newPrice));
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class BackOfChangePriceChatInMenu : AbsCommand
	{
		public override System.String Name { get; set; } = CommandsText.BackOfChangePriceChatInMenu;
		private Single Result = 0;

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			
			var  db   = Singleton.GetInstance().Context;
			User user = db.GetUser(_message.From.Id);
			
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					Result = Convert.ToSingle(Name.Split(" ")[1]);
					Name   = CommandsText.BackOfChangePriceChatInMenu;
					
					user.Chain = (Int32)SetChain.MessageUserInBot;
					db.Save();
					
					await botClient.EditMessageTextAsync(_message.From.Id, _message.Message.MessageId,
					                                     "Настройка рекламы",
					                                     replyMarkup: InlineButton.OptionAdvertisingShowStaticChats);
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	} // Result Change Genegal Time

	#endregion Change General Chats

	#region Change General Pinned Message
	internal class EditStaticTimePinnedPrice : AbsCommand
	{
		public override System.String Name { get; set; } = CommandsText.StaticPricePinned;
		private DataBase db = Singleton.GetInstance().Context;
		private User user = null;

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			Single        price    = AdController.PriceData.postTypePinnedPrice;
			CallbackQuery _message = message as CallbackQuery;
			
			user = db.GetUser(_message.From.Id);
			if(user == null) return;
			
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					changeUser();
					await botClient.EditMessageTextAsync(_message.From.Id, _message.Message.MessageId,
					                                     "Цена за закрепленное сообщение \nВы так же можете ввести цену вручную [10]!",
					                                     replyMarkup: InlineButton.ChangePricePinned(price));
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
		
		private void changeUser()
		{
			user.Chain = (Int32)SetChain.SetStaticTimePinnedPrice;
			db.Save();
		}
	} // Start Price

	internal class LeftChangeStaticPinned_Price : AbsCommand
	{
		public override System.String Name { get; set; } = CommandsText.LeftChangeStaticPinned_Price;
		private Single sub_price = 0.1f;

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message  = message as CallbackQuery;
			Single        old_price = System.Convert.ToSingle(Name.Split(" ")[1]);
			Name = CommandsText.LeftChangeStaticPinned_Price;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					Single newPrice = old_price - sub_price;

					if (newPrice < 0)
					{
						newPrice = 0.1f;
					}

					newPrice                                   = (Single)Math.Round(newPrice, 1);
					AdController.PriceData.postTypePinnedPrice = newPrice;

					AdController.SavePriceData();

					await botClient.EditMessageTextAsync(_message.From.Id, _message.Message.MessageId,
					                                     "Цена за закрепленное сообщение:",
					                                     replyMarkup: InlineButton.ChangePricePinned(newPrice));
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class RightChangeStaticPinned_Price : AbsCommand
	{
		public override System.String Name { get; set; } = CommandsText.RightChangeStaticPinned_Price;
		private Single add_price = 0.1f;

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message  = message as CallbackQuery;
			Single        old_price = System.Convert.ToSingle(Name.Split(" ")[1]);
			Name = CommandsText.RightChangeStaticPinned_Price;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					Single newPrice = old_price + add_price;

					if (newPrice < 0)
					{
						newPrice = 0.1f;
					}

					newPrice                                   = (Single)Math.Round(newPrice, 1);
					AdController.PriceData.postTypePinnedPrice = newPrice;

					AdController.SavePriceData();

					await botClient.EditMessageTextAsync(_message.From.Id, _message.Message.MessageId,
					                                     "Цена за закрепленное сообщение:",
					                                     replyMarkup: InlineButton.ChangePricePinned(newPrice));
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class BackOfChangePricePinnedInMenu : AbsCommand
	{
		public override System.String Name { get; set; } = CommandsText.BackOfChangePricePinnedInMenu;
		private DataBase db = Singleton.GetInstance().Context;
		private User user = null;

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			
			user = db.GetUser(_message.From.Id);
			if(user == null) return;
			
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					user.Chain = (Int32)SetChain.MessageUserInBot;
					db.Save();
					Name = CommandsText.BackOfChangePricePinnedInMenu;
					await botClient.EditMessageTextAsync(_message.From.Id, _message.Message.MessageId,
					                                     "Настройка рекламы",
					                                     replyMarkup: InlineButton.OptionAdvertising);
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	} // Result Change Genegal Time

	#endregion Change General Pinned Message

	#region Change General Pinned Message Notification

	internal class EditStaticTimePinnedNotificationPrice : AbsCommand
	{
		public override System.String Name { get; set; } = CommandsText.StaticPricePinnedNotification;
		private DataBase db = Singleton.GetInstance().Context;
		private User user = null;
		
		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			Single        price    = AdController.PriceData.postTypePinnedNotificationPrice;
			CallbackQuery _message = message as CallbackQuery;
			user = db.GetUser(_message.From.Id);
			if(user == null) return;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					changeUser();
					await botClient.EditMessageTextAsync(_message.From.Id, _message.Message.MessageId,
					                                     "Цена за закрепленное сообщение с уведомлением\nВы так же можете ввести цену вручную [10]!",
					                                     replyMarkup: InlineButton
						                                     .ChangePricePinnedNotification(price));
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
		
		private void changeUser()
		{
			user.Chain = (Int32)SetChain.SetStaticTimePinnedNotificationPrice;
			db.Save();
		}
	} // Start Price

	internal class LeftChangeStaticPinnedNotification_Price : AbsCommand
	{
		public override System.String Name { get; set; } = CommandsText.LeftChangeStaticPinnedNotification_Price;
		private Single sub_price = 0.1f;

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message  = message as CallbackQuery;
			Single        old_price = System.Convert.ToSingle(Name.Split(" ")[1]);
			Name = CommandsText.LeftChangeStaticPinnedNotification_Price;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					Single newPrice = old_price - sub_price;

					if (newPrice < 0)
					{
						newPrice = 0.1f;
					}

					newPrice                                               = (Single)Math.Round(newPrice, 1);
					AdController.PriceData.postTypePinnedNotificationPrice = newPrice;

					AdController.SavePriceData();

					await botClient.EditMessageTextAsync(_message.From.Id, _message.Message.MessageId,
					                                     "Цена за закрепленное сообщение с уведомлением:",
					                                     replyMarkup: InlineButton
						                                     .ChangePricePinnedNotification(newPrice));
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class RightChangeStaticPinnedNotification_Price : AbsCommand
	{
		public override System.String Name { get; set; } = CommandsText.RightChangeStaticPinnedNotification_Price;
		private Single add_price = 0.1f;

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message  = message as CallbackQuery;
			Single        old_price = System.Convert.ToSingle(Name.Split(" ")[1]);
			Name = CommandsText.RightChangeStaticPinnedNotification_Price;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					Single newPrice = old_price + add_price;
					if (newPrice < 0)
					{
						newPrice = 0.1f;
					}

					newPrice                                               = (Single)Math.Round(newPrice, 1);
					AdController.PriceData.postTypePinnedNotificationPrice = newPrice;

					AdController.SavePriceData();

					await botClient.EditMessageTextAsync(_message.From.Id, _message.Message.MessageId,
					                                     "Цена за закрепленное сообщение с уведомлением:",
					                                     replyMarkup: InlineButton
						                                     .ChangePricePinnedNotification(newPrice));
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class BackOfChangePricePinnedNotificationInMenu : AbsCommand
	{
		public override System.String Name { get; set; } = CommandsText.BackOfChangePricePinnedNotificationInMenu;
		private Single Result = 0;
		private DataBase db = Singleton.GetInstance().Context;
		private User user = null;

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
				
			user = db.GetUser(_message.From.Id);
			if(user == null) return;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					Result = Convert.ToSingle(Name.Split(" ")[1]);
					Name   = CommandsText.BackOfChangePricePinnedNotificationInMenu;
					
					user.Chain = (Int32)SetChain.MessageUserInBot;
					db.Save();
					
					await botClient.EditMessageTextAsync(_message.From.Id, _message.Message.MessageId,
					                                     "Настройка рекламы",
					                                     replyMarkup: InlineButton.OptionAdvertising);
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	#endregion Change General Pinned Message Notification

	//Sanya
	internal class ShowChatPrice : AbsCommand
	{
		public override System.String Name { get; set; } = CommandsText.ShowPriceChat;

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			Name = CommandsText.ShowPriceChat;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					await botClient.EditMessageTextAsync(_message.From.Id, _message.Message.MessageId, "Чаты",
					                                     replyMarkup: InlineButton.OptionAdvertisingShowChats());
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class AdminPanelPrice : AbsCommand
	{
		public override System.String Name { get; set; } = CommandsText.AdminPanelPrice;

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					await botClient.EditMessageTextAsync(_message.From.Id, _message.Message.MessageId,
					                                     "Настройка рекламы",
					                                     replyMarkup: InlineButton.OptionAdvertising);
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class BackInAdminPanelPrice : AbsCommand
	{
		public override System.String Name { get; set; } = CommandsText.BackInShowMenuPrice;

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					await botClient.EditMessageTextAsync(_message.From.Id, _message.Message.MessageId,
					                                     "Настройка рекламы",
					                                     replyMarkup: InlineButton.OptionAdvertising);
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class BackInShowStaticChatPrice : AbsCommand
	{
		public override System.String Name { get; set; } = CommandsText.BackInShowStaticPriceChat;

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					await botClient.EditMessageTextAsync(_message.From.Id, _message.Message.MessageId, "Общие ценны",
					                                     replyMarkup: InlineButton.OptionAdvertisingShowStaticChats);
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class BackInShowChatPrice : AbsCommand
	{
		public override System.String Name { get; set; } = CommandsText.BackInShowChatPrice;

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					await botClient.EditMessageTextAsync(_message.From.Id, _message.Message.MessageId,
					                                     "Настройка рекламы",
					                                     replyMarkup: InlineButton.OptionAdvertising);
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class ShowStaticPrice : AbsCommand
	{
		public override System.String Name { get; set; } = CommandsText.ShowStaticPrices;

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			if (IsBan.Ban(botClient, message))
			{
				try
				{
					await botClient.EditMessageTextAsync(_message.From.Id, _message.Message.MessageId, "Чаты",
					                                     replyMarkup: InlineButton.OptionAdvertisingShowStaticChats);
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	internal class DeletePostTemplate : AbsCommand
	{
		public override System.String Name { get; set; } = CommandsText.DeletePostTemplate;

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			CallbackQuery _message = message as CallbackQuery;
			if (IsBan.Ban(botClient, message))
			{
				DataBase db   = Singleton.GetInstance().Context;
				User     user = db.GetUser(_message.From.Id);
				user.Chain = (Int32)SetChain.DeletePostTemplate;
				db.Save();

				try
				{
					await botClient.EditMessageTextAsync(_message.From.Id, _message.Message.MessageId,
					                                     "Перешлите сообщение рекламы: ");
				}
				catch (System.Exception ex)
				{
					Log.Logging(ex);
				}
			}
		}
	}

	#endregion Advertising Panel Price
}