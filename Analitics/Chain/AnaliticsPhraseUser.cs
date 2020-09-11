using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BotCore.Advertising;
using Telegram.Bot;
using Telegram.Bot.Types;


namespace BotCore
{
	internal class AnaliticsPhraseUser : AbstractHandlerAdvertising
	{
		private User user = null;
		private String analitics = "Аналитика фразы по чатам: ";

		private async void ChekText(TelegramBotClient botClient, Message _message, User user, Settings settings) => await Task.Run(() => ChekTextAsync(botClient, _message, user, settings));

		private async void ChekTextAsync(TelegramBotClient botClient, Message _message, User user, Settings settings)
		{
			DataBase db = Singleton.GetInstance().Context;
			try
			{
				await Task.Run(() => AddAnaliticsPharase(botClient, _message, settings));

				db.Save();
			}
			catch (System.Exception ex)
			{
				Log.Logging(ex);
			}
		}

		private void AddAnaliticsPharase(TelegramBotClient botClient, Message _message, Settings settings)
		{
			DataBase db = Singleton.GetInstance().Context;
			Channel channel = db.GetChannel(_message);

			String clean = "";

			String analiticsText = "";

			foreach (System.String word in _message.Text.Split(" "))
			{
				clean += word.Trim(new System.Char[] { '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '-', '_', '=', '+', '|', '\\', ']', '}', '[', '{', '\'', '\"', ';', ':', '/', '?', '.', '>', '<', ',', ' ' }) + " ";
			}
			AnaliticsPhraseAllChat[] analiticsPhraseAllChats = db.GetAnaliticsPhraseAllChats();
			if (analiticsPhraseAllChats.Length != 0)
			{
				if (_message.Chat.Type == Telegram.Bot.Types.Enums.ChatType.Private)
				{
					foreach (AnaliticsPhraseAllChat text in analiticsPhraseAllChats)
					{
						if (CalculateFuzzyEqualValue(clean.ToLower(), text.NameId, settings) >= settings.ProcentMessage)
						{
							AnaliticsPhrase[] analyticsText = db.GetAnaliticsPharse(text.NameId);
							IQueryable<AnaliticsPhrase> temp = analyticsText.OrderByDescending(p => p.Count).AsQueryable();
							foreach (AnaliticsPhrase item in temp)
							{
								analiticsText += "\nГруппа: " + item.channel.ChannelName + ";\nФраза: " + text.NameId + ";\nКоличество использования: " + item.Count + "\n";
							}
							break;
						}
					}
				}
			}

			if (analiticsText.Length != 0)
			{
				analitics += analiticsText;
			}
			else
			{
				analitics += "\nПо Вашему запросу нет совподений.";
			}
			SendMessage(botClient, _message);
		}

		private System.Double CalculateFuzzyEqualValue(System.String first, System.String second, Settings settings)
		{
			if (System.String.IsNullOrWhiteSpace(first) && System.String.IsNullOrWhiteSpace(second))
			{
				return 1.0;
			}

			if (System.String.IsNullOrWhiteSpace(first) || System.String.IsNullOrWhiteSpace(second))
			{
				return 0.0;
			}

			System.String normalizedFirst = NormalizeSentence(first);
			System.String normalizedSecond = NormalizeSentence(second);

			System.String[] tokensFirst = GetTokens(normalizedFirst);
			System.String[] tokensSecond = GetTokens(normalizedSecond);

			System.String[] fuzzyEqualsTokens = GetFuzzyEqualsTokens(tokensFirst, tokensSecond, settings);

			System.Int32 equalsCount = fuzzyEqualsTokens.Length;
			System.Int32 firstCount = tokensFirst.Length;
			System.Int32 secondCount = tokensSecond.Length;

			System.Double resultValue = (1.0 * equalsCount) / (firstCount + secondCount - equalsCount);

			return resultValue;
		}

		private System.String NormalizeSentence(System.String sentence)
		{
			StringBuilder resultContainer = new StringBuilder(100);
			System.String lowerSentece = sentence.ToLower();
			foreach (System.Char c in lowerSentece)
			{
				if (IsNormalChar(c))
				{
					resultContainer.Append(c);
				}
			}

			return resultContainer.ToString();
		}

		private System.Boolean IsNormalChar(System.Char c) => System.Char.IsLetterOrDigit(c) || c == ' ';

		private System.String[] GetTokens(System.String sentence)
		{
			List<System.String> tokens = new List<System.String>();
			System.String[] words = sentence.Split(' ');
			foreach (System.String word in words)
			{
				if (word.Length >= 2)
				{
					tokens.Add(word);
				}
			}

			return tokens.ToArray();
		}

		private System.Boolean IsTokensFuzzyEqual(System.String firstToken, System.String secondToken, Settings settings)
		{
			System.Int32 equalSubtokensCount = 0;
			System.Int32 SubtokenLength = 2;
			System.Boolean[] usedTokens = new System.Boolean[secondToken.Length - SubtokenLength + 1];
			for (System.Int32 i = 0; i < firstToken.Length - SubtokenLength + 1; ++i)
			{
				System.String subtokenFirst = firstToken.Substring(i, SubtokenLength);
				for (System.Int32 j = 0; j < secondToken.Length - SubtokenLength + 1; ++j)
				{
					if (!usedTokens[j])
					{
						System.String subtokenSecond = secondToken.Substring(j, SubtokenLength);
						if (subtokenFirst.Equals(subtokenSecond))
						{
							equalSubtokensCount++;
							usedTokens[j] = true;
							break;
						}
					}
				}
			}

			System.Int32 subtokenFirstCount = firstToken.Length - SubtokenLength + 1;
			System.Int32 subtokenSecondCount = secondToken.Length - SubtokenLength + 1;

			System.Double tanimoto = (1.0 * equalSubtokensCount) / (subtokenFirstCount + subtokenSecondCount - equalSubtokensCount);
			return settings.ProcentMessage <= tanimoto;
		}

		private System.String[] GetFuzzyEqualsTokens(System.String[] tokensFirst, System.String[] tokensSecond, Settings settings)
		{
			List<System.String> equalsTokens = new List<System.String>();
			System.Boolean[] usedToken = new System.Boolean[tokensSecond.Length];
			for (System.Int32 i = 0; i < tokensFirst.Length; ++i)
			{
				for (System.Int32 j = 0; j < tokensSecond.Length; ++j)
				{
					if (!usedToken[j])
					{
						if (IsTokensFuzzyEqual(tokensFirst[i], tokensSecond[j], settings))
						{
							equalsTokens.Add(tokensFirst[i]);
							usedToken[j] = true;
							break;
						}
					}
				}
			}

			return equalsTokens.ToArray();
		}


		private void GetUser(Message _message)
		{
			DataBase db = Singleton.GetInstance().Context;
			user = db.GetUser(_message.From.Id);
		}


		internal void SendMessage(TelegramBotClient botClient, Message _message)
		{
			DataBase db = Singleton.GetInstance().Context;

			botClient.DeleteMessage(_message.From.Id, _message.MessageId, "33 - AddUserInTransaction");

			InlineButton inlineBatton = new InlineButton();

			botClient.EditMessage(user.ID, user.MessageID, analitics, "49 - AddPhoto", user, replyMarkup: inlineBatton.BackToAccauntMenu);

		}

		public override System.Object Handle(System.Int32 request, TelegramBotClient botClient, System.Object message)
		{
			if (request == (System.Int32)SetChain.PhraseUser)
			{
				Message _message = message as Message;
				DataBase db = Singleton.GetInstance().Context;

				GetUser(_message);

				Settings settings = db.GetSettings();

				ChekText(botClient, _message, user, settings);

				return null;
			}
			else
			{
				return base.Handle(request, botClient, message);
			}
		}
	}
}
