using System;
using BotCore.Blockchain;
using BotCore.SQL;
using Telegram.Bot;
using Telegram.Bot.Types;
namespace BotCore.Advertising.Command
{
	internal class BackToContentKeyboard : AbsCommand, ISplitName, IStandartCommand
	{
		public override System.String Name { set; get; } = CommandsText.BackToContentKeyboard;

		private DataBase db = null;
		private User user = null;
		private CallbackQuery _message = null;
		private PostTemplate postTemplate = null;

		public override async void Execute(TelegramBotClient botClient, System.Object message)
		{
			IStandartCommand standartCommand = new BackToContentKeyboard();
			ISplitName splitName = new BackToContentKeyboard();

			if (standartCommand.SetCallbackQuery(message, out _message)) return;

			if (standartCommand.SetDataBase(out db)) return;

			if (standartCommand.SetUserAndCheckIsNull(botClient, _message, out user, db)) return;

			Int32 idPostTemplate = splitName.GetNameSplit(Name);
			Name = CommandsText.BackToContentKeyboard;
			if (IsBan.Ban(botClient, message))
			{
				if (GetPostTemplate(idPostTemplate)) return;

				ChangeUser();

				try
				{
					AdUser adUser = db.GetAdUser(user.ID);
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

		private void ChangeUser()
		{
			user.Chain = (System.Int32)SetChain.MessageUserInBot;
			db.Save();
		}

		public async void SendMessage(TelegramBotClient botClient)
		{
			System.Boolean showContent = db.GetPostContentCount(postTemplate);
			try
			{
				await botClient.EditMessageTextAsync(_message.From.Id, _message.Message.MessageId, "Добавьте картинки и текст\nПункт 1-10 предназначен для картинок, заполнять их все не обязательно!", Telegram.Bot.Types.Enums.ParseMode.Html, replyMarkup: InlineButton.ContentKeyboard(postTemplate, showContent));
			}
			catch
			{
				await botClient.DeleteMessageAsync(_message.From.Id, _message.Message.MessageId);
				await botClient.SendTextMessageAsync(_message.From.Id, "Добавьте картинки и текст\nПункт 1-10 предназначен для картинок, заполнять их все не обязательно!", Telegram.Bot.Types.Enums.ParseMode.Html, replyMarkup: InlineButton.ContentKeyboard(postTemplate, showContent));
			}
		}
	}
}
