using System;
using System.Collections.Generic;
using System.Linq;
using BotCore.Advertising;
using BotCore.Blockchain;
using BotCore.SQL;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCore
{
	internal class BackToAddAdversting : Advertising.Command.AbsCommand, IStandartCommand, ISplitName
	{
		public override System.String Name { set; get; } = CommandsText.BackToAddAdversting;

		private DataBase db = null;
		private User user = null;
		private CallbackQuery _message = null;
		private PostTemplate postTemplate = null;
		private AdUser adUser = null;

		public override void Execute(TelegramBotClient botClient, System.Object message)
		{
			IStandartCommand standartCommand = new BackToAddAdversting();
			ISplitName splitName = new BackToAddAdversting();

			if (standartCommand.SetCallbackQuery(message, out _message)) return;

			if (standartCommand.SetDataBase(out db)) return;

			if (standartCommand.SetUserAndCheckIsNull(botClient, _message, out user, db)) return;

			Int32 idPostTemplate = splitName.GetNameSplit(Name);
			Name = CommandsText.BackToAddAdversting;
			if (IsBan.Ban(botClient, message))
			{
				if (GetPostTemplate(idPostTemplate)) return;

				ChangeUser();

				SendMessage(botClient);
			}
		}


		private Boolean GetPostTemplate(Int32 idPostTemplate)
		{
			adUser = db.GetAdUser(user.ID);
			postTemplate = db.GetTempalte(adUser.User.ID, idPostTemplate);
			return postTemplate == null;
		}

		public async void SendMessage(TelegramBotClient botClient)
		{
			//botClient.EditMessage(_message.From.Id, user.MessageID, "Реклама", "18 - ConfirmMyTransaction", user, InlineBatton.GetTemplateMenu(adUser));
			//botClient.SendText(transaction.UserSenderId, $"Транзакция была подтверждена {user.FIO}");

			if (postTemplate.PostContent.Count != 0)
			{
				IOrderedEnumerable<PostContent> orderedList = postTemplate.PostContent.OrderBy(c => c.Order);

				Message textMessage = null;
				List<InputMediaBase> inputMedias = new List<InputMediaBase>();
				foreach (PostContent content in orderedList)
				{
					Message forward = await botClient.ForwardMessageAsync(CommandText.bufferChannelId, CommandText.bufferChannelId, content.MessageId);

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
					await botClient.ForwardMessageAsync(_message.From.Id, CommandText.bufferChannelId, textMessage.MessageId);
				}
				catch { }
				if (postTemplate.IsOnValidation == false && postTemplate.IsValidated == false)
				{
					botClient.DeleteMessage(_message.From.Id, user.MessageID, "275");
					botClient.SendText(_message.From.Id, $"Управление шаблоном\n1. Заполните контент\n2. Введите время постинга.\n3. Выберете чат(ы)\n4. Отправите на проверку администрации.", user, replyMarkup: Advertising.InlineButton.GetTemplateMenu(adUser));
				}
				else if (postTemplate.IsValidated == true && postTemplate.IsPaid == false)
				{
					botClient.DeleteMessage(_message.From.Id, user.MessageID, "275");
					botClient.SendText(_message.From.Id, $"<b>Название поста: {textMessage.Text}</b>\n\nДанный пост одобрен администрацией, вы моежете его оплатить после чего он пойдет в работу!", user, replyMarkup: Advertising.InlineButton.GetTemplateMenu(adUser));
				}
				else if (postTemplate.IsValidated == true && postTemplate.IsPaid == true)
				{
					botClient.DeleteMessage(_message.From.Id, user.MessageID, "275");
					botClient.SendText(_message.From.Id, $"<b>Название поста: {textMessage.Text}</b>\n\nДанный пост находится в работе!", user, replyMarkup: Advertising.InlineButton.GetTemplateMenu(adUser));
				}
				else
				{
					botClient.DeleteMessage(_message.From.Id, user.MessageID, "275");
					botClient.SendText(_message.From.Id, $"<b>Название поста: {textMessage.Text}</b>\n\nДанный пост находится в работе!", user, replyMarkup: Advertising.InlineButton.GetTemplateMenu(adUser));
				}
			}
			else
			{
				if (postTemplate.IsPaid == false)
				{
					botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "Управление шаблоном\n1. Заполните контент\n2. Введите время постинга.\n3. Выберете чат(ы)\n4. Отправите на проверку администрации.", "276", user, replyMarkup: Advertising.InlineButton.GetTemplateMenu(adUser));
				}
				else
				{
					botClient.EditMessage(_message.From.Id, _message.Message.MessageId, "Данный пост находится в работе!", "280", user, replyMarkup: Advertising.InlineButton.GetTemplateMenu(adUser));
				}
			}
		}

		private void ChangeUser()
		{
			user.Chain = (Int32)SetChain.SetPublicKeyUserTwo;
			db.Save();
		}
	}
}