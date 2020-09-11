using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BotCore.Advertising;
using BotCore.SQL;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCore
{
	internal static class CheckCountBan
	{
		public static System.Boolean CheckBan(TelegramBotClient botClient, Message _message, User user, DataBase db, System.String text, System.String banText)
		{
			Settings settings = db.GetSettings();
			if (user.Test == 0)
			{
				user.BanDate = System.DateTime.Now.AddMinutes(10);
				user.Test++;
				user.PayConfirm = false;
				user.PayDate = System.DateTime.Today;
				user.BanDescript = text;
				db.Save();
				IsMute.ThisMut(botClient, user);
				IsBan.Ban(botClient, _message);

				System.String textAdmin = "<b>Бан системой UBC!</b>\n" + "ФИО: " + user.FIO;
				text += user.Username != "Нет!" ? "\n🧸Юзернейм: @" + user.Username : "";
				text += "\nНомер: " + user.Number + "\nПричина бана: " + user.BanDescript + $"\nТекст сообщения: {banText}";
				botClient.SendText(settings.ChannelAdmin, text, user, true);

				return true;
			}
			else if (user.Test == 1)
			{
				user.BanDate = System.DateTime.Now.AddHours(1);
				user.Test++;
				user.PayConfirm = false;
				user.PayDate = System.DateTime.Today;
				user.BanDescript = text;
				db.Save();
				IsMute.ThisMut(botClient, user);
				IsBan.Ban(botClient, _message);

				System.String textAdmin = "<b>Бан системой UBC!</b>\n" + "ФИО: " + user.FIO;
				text += user.Username != "Нет!" ? "\n🧸Юзернейм: @" + user.Username : "";
				text += "\nНомер: " + user.Number + "\nПричина бана: " + user.BanDescript + $"\nТекст сообщения: {banText}";
				botClient.SendText(settings.ChannelAdmin, text, user, true);

				return true;
			}
			return false;
		}
	}

	internal static class IsBanUser
	{
		public static async void ThisBan(TelegramBotClient botClient, Message _message, User user, Settings settings)
		{
			if (user.IsAdmin == 0)
			{
				DataBase db = Singleton.GetInstance().Context;
				Channel[] channels = db.GetChannels();
				foreach (Channel channel in channels)
				{
					try
					{
						await botClient.RestrictChatMemberAsync(channel.IDChannel, user.ID, new ChatPermissions { CanAddWebPagePreviews = false, CanChangeInfo = false, CanInviteUsers = true, CanPinMessages = false, CanSendMediaMessages = false, CanSendMessages = false, CanSendOtherMessages = false, CanSendPolls = false }, user.BanDate);
					}
					catch
					{
						try
						{
							botClient.Kick(channel.IDChannel, user.ID, "29 - IsBanUser");
						}
						catch { }
					}
				}
			}
		}

		public static async void ThisBan(TelegramBotClient botClient, CallbackQuery _message, User user, Settings settings)
		{
			if (user.IsAdmin == 0)
			{
				DataBase db = Singleton.GetInstance().Context;
				Channel[] channels = db.GetChannels();
				foreach (Channel channel in channels)
				{
					try
					{
						await botClient.RestrictChatMemberAsync(channel.IDChannel, user.ID, new ChatPermissions { CanAddWebPagePreviews = false, CanChangeInfo = false, CanInviteUsers = true, CanPinMessages = false, CanSendMediaMessages = false, CanSendMessages = false, CanSendOtherMessages = false, CanSendPolls = false }, user.BanDate);
					}
					catch
					{
						botClient.Kick(channel.IDChannel, user.ID, "29 - IsBanUser");
					}
				}
			}
		}
	}

	internal static class IsUnBan
	{
		public static async void ThisUnBan(TelegramBotClient botClient, User user)
		{
			DataBase db = Singleton.GetInstance().Context;
			Channel[] channels = db.GetChannels();
			foreach (Channel channel in channels)
			{
				try
				{
					Chat chat = await botClient.GetChatAsync(channel.IDChannel);
					await botClient.RestrictChatMemberAsync(channel.IDChannel, user.ID, new ChatPermissions
					{
						CanChangeInfo = chat.Permissions.CanChangeInfo,
						CanInviteUsers = chat.Permissions.CanInviteUsers,
						CanPinMessages = chat.Permissions.CanPinMessages,
						CanAddWebPagePreviews = chat.Permissions.CanAddWebPagePreviews,
						CanSendMessages = chat.Permissions.CanSendMessages,
						CanSendMediaMessages = chat.Permissions.CanSendMediaMessages,
						CanSendOtherMessages = chat.Permissions.CanSendOtherMessages,
						CanSendPolls = chat.Permissions.CanSendPolls
					});
				}
				catch
				{ }
			}
			user.CountBanAddPeople = 0;
			user.BanDate = System.DateTime.Today;
			db.Save();
		}
	}

	internal static class IsKick
	{
		public static async void ThisKick(TelegramBotClient botClient, User user)
		{
			try
			{
				await Singleton.GetInstance().Context._channels.ForEachAsync(p =>
																						   {
																							   botClient.KickChatMemberAsync(p.IDChannel, user.ID, user.BanDate);
																						   });
			}
			catch { }
		}
	}

	internal static class IsMute
	{
		public static async void ThisMut(TelegramBotClient botClient, User user)
		{
			DataBase db = Singleton.GetInstance().Context;
			Channel[] channels = db.GetChannels();
			foreach (Channel item in channels)
			{
				try
				{
					await botClient.RestrictChatMemberAsync(item.IDChannel, user.ID, new ChatPermissions { CanSendMediaMessages = false, CanSendMessages = false, CanSendOtherMessages = false, CanSendPolls = false, CanInviteUsers = true }, user.BanDate);
				}
				catch { }
			}
		}
	}

	internal static class DeletePost
	{
		public static async void ThisDelete(TelegramBotClient botClient, Message _message, System.String text)
		{
			try
			{
				await botClient.DeleteMessageAsync(_message.Chat.Id, _message.MessageId);

				botClient.SendText(_message.From.Id, text, replyMarkup: Advertising.InlineButton.ButtonOk());
			}
			catch (System.Exception ex)
			{
				Log.Logging("DeletePost" + ex);
			}
		}
	}

	internal static class CheckChannel
	{
		public static void ThisChannel(TelegramBotClient botClient, Message _message, User user, Channel channel, TMessage tmessage)
		{
			DataBase db = Singleton.GetInstance().Context;
			if (_message.ForwardFromChat != null)
			{
				if (channel == null)
				{
					channel.IDChannel = _message.Chat.Id;
					channel.ChannelName = _message.Chat.Title;
					channel.InviteLink = "@" + _message.Chat.Username;
					db.Save();
				}
				if (db.GetChannel(_message.ForwardFromChat.Id) == null)
				{
					System.String banText = _message.Text + $"\nБан был выданан в группе: {_message.Chat.FirstName}";
					botClient.DeleteMessage(_message.Chat.Id, _message.MessageId, "99");
					botClient.SendText(_message.From.Id, "Ваше сообщение было удалено, по причине:\nПересылка с канала!", replyMarkup: Advertising.InlineButton.ButtonOk());
					Settings settings = db.GetSettings();

					if (CheckCountBan.CheckBan(botClient, _message, user, db, "На вас наложено молчание за пересылку с другого чата или канала не привязанного к UBC! Бан выдан системой UBC!", banText)) return;
					else
					{
						user.BanDate = System.DateTime.Now.AddDays(settings.CountLink);
						user.Test++;
						db.Save();
						IsBan.Ban(botClient, _message);
					}

					if (settings.IsBanOrKickOrMutLink == 0 && user.IsAdmin == 0)
					{
						user.BanDescript = "Вы были забанены за пересылку с другого чата или канала не привязанного к UBC! Бан выдан системой UBC!";
						IsBanUser.ThisBan(botClient, _message, user, settings);
					}
					else if (settings.IsBanOrKickOrMutLink == 1 && user.IsAdmin == 0)
					{
						user.BanDescript = "Вы были кикнуты за пересылку с другого чата или канала не привязанного к UBC! Бан выдан системой UBC!";
						IsKick.ThisKick(botClient, user);
					}
					else if (settings.IsBanOrKickOrMutLink == 2 && user.IsAdmin == 0)
					{
						user.BanDescript = "На вас наложено молчание за пересылку с другого чата или канала не привязанного к UBC! Бан выдан системой UBC!";
						IsMute.ThisMut(botClient, user);
					}
					user.PayConfirm = false;
					user.PayDate = System.DateTime.Today;
					db.Save();
					System.String text = "<b>Бан системой UBC!</b>\n" + "ФИО: " + user.FIO;
					text += user.Username != "Нет!" ? "\n🧸Юзернейм: @" + user.Username : "";
					text += "\nНомер: " + user.Number + "\nПричина бана: " + user.BanDescript + $"\nТекст сообщения: {banText}";
					botClient.SendText(settings.ChannelAdmin, text, user, true);
					return;
				}
				else if (_message.Chat.Id != user.ID)
				{
					if (tmessage.user.IsAdmin == 0)
					{
						tmessage.Post--;
					}
					db.Save();
				}
			}
			else if (_message.Chat.Id != user.ID)
			{
				if (tmessage.user.IsAdmin == 0)
				{
					tmessage.Post--;
				}
				db.Save();
			}
		}
	}

	internal class CheckWord
	{
		//private async void ChekText(TelegramBotClient botClient, Message _message, User user, Settings settings, TMessage tmessage) => await Task.Run(() => ChekTextAsync(botClient, _message, user, settings, tmessage));

		private void CheckUserMessage(TelegramBotClient botClient, Message _message, User user, Settings settings, TMessage tmessage, System.String temp)
		{
			DataBase db = Singleton.GetInstance().Context;
			System.Int32 Count = 0;

			UserMessage[] userMessage = db.GetUserMessages();

			foreach (UserMessage item in userMessage)
			{
				if (user.ID == item.UserID)
				{
					if (CalculateFuzzyEqualValue(temp, item.SendMessage, settings) >= settings.ProcentMessage)
					{
						if (Count < 3)
						{
							Count++;
						}
						else
						{
							System.String banText = temp + $"\nБан был выданан в группе: {_message.Chat.FirstName}";
							botClient.DeleteMessage(_message.Chat.Id, _message.MessageId, "269 - CheckWord");

							if (CheckCountBan.CheckBan(botClient, _message, user, db, "На вас наложено молчание за флуд! Бан выдан системой UBC!", banText)) return;
							else
							{
								user.BanDate = System.DateTime.Now.AddDays(settings.CountBanFlud);
								user.Test++;
								db.Save();
								IsBan.Ban(botClient, _message);
							}

							if (settings.IsBanOrKickOrMutFlud == 0 && user.IsAdmin == 0)
							{
								user.BanDescript = "Вы были забанены за флуд! Бан выдан системой UBC!";
								db.Save();

								IsBanUser.ThisBan(botClient, _message, user, settings);
								IsBan.Ban(botClient, _message);
							}
							else if (settings.IsBanOrKickOrMutFlud == 1 && user.IsAdmin == 0)
							{
								user.BanDescript = "Вы были кикнуты за флуд! Бан выдан системой UBC!";
								db.Save();

								IsKick.ThisKick(botClient, user);
								IsBan.Ban(botClient, _message);
							}
							else if (settings.IsBanOrKickOrMutFlud == 2 && user.IsAdmin == 0)
							{
								user.BanDescript = "На вас наложено молчание за флуд! Бан выдан системой UBC!";
								db.Save();

								IsMute.ThisMut(botClient, user);
								IsBan.Ban(botClient, _message);
							}
							user.PayConfirm = false;
							user.PayDate = System.DateTime.Today;
							db.Save();

							System.String text = "<b>Бан системой UBC!</b>\n" + "ФИО: " + user.FIO;
							text += user.Username != "Нет!" ? "\n🧸Юзернейм: @" + user.Username : "";
							text += "\nНомер: " + user.Number + "\nПричина бана: " + user.BanDescript + $"\n{banText}";
							botClient.SendText(settings.ChannelAdmin, text, user, true);
							return;
						}
					}
				}
			}
		}

		private async void ChekTextAsync(TelegramBotClient botClient, Message _message, User user, Settings settings, TMessage tmessage)
		{
			DataBase db = Singleton.GetInstance().Context;
			try
			{
				System.String temp;
				if (_message.Text != null)
				{
					temp = _message.Text.ToLower();
				}
				else if (_message.Sticker != null)
				{
					temp = _message.Sticker.SetName.ToLower();
				}
				else if (_message.Photo != null)
				{
					temp = null;
				}
				else
				{
					temp = null;
				}

				if (_message.Caption != null)
				{
					temp = _message.Caption;
				}
				
				if (temp != null)
				{
					await Task.Run(() => CheckUserMessage(botClient, _message, user, settings, tmessage, temp));
				}


				if (temp != null)
				{
					db.SetValue<UserMessage>(new UserMessage() { UserID = user.ID, SendMessage = temp, dateTime = System.DateTime.Today });
				}
				
				tmessage.Post--;
				db.Save();
				if (temp != null)
				{
					try
					{
						AddAnalitics(_message);
					}
					catch (Exception e)
					{
						Log.Logging("AddAnalitics(_message)) - " + e);
					}

					try
					{
						AddAnaliticsPharase(_message, settings);
					}
					catch (Exception e)
					{
						Log.Logging("AddAnaliticsPharase(_message, settings));" + e);
					}

					try
					{
						AddAnaliticsPharaseMonth(botClient, _message, settings);
					}
					catch (Exception e)
					{
						Log.Logging("AddAnaliticsPharaseMonth(botClient, _message, settings));" + e);
					}
				}
			}
			catch (System.Exception ex)
			{
				Log.Logging("ChekTextAsync - ChekRegister: " + ex);
			}
		}

		private void AddAnalitics(Message _message)
		{
			if(_message == null) return;

			DataBase db = Singleton.GetInstance().Context;
			Channel channel = db.GetChannel(_message);
			String text;
			if (_message.Text != null)
			{
				text = _message.Text;
				text = Regex.Replace(text, @"[^a-z,A-Z,а-я,А-Я,0-9]", " ");
			}
			else
			{
				text = _message.Caption;
				text = Regex.Replace(text, @"[^a-z,A-Z,а-я,А-Я,0-9]", " ");
			}

			var text2 = text.Split(' ');
			
			var a = text2.GroupBy(x => x.ToLower()).ToList();
			
			String result = String.Empty;
			a.ForEach(x => result += (x.Key +" ") );
			
			foreach (System.String word in result.Split(" "))
			{
				System.String clean = word.Trim(new System.Char[] { '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '-', '_', '=', '+', '|', '\\', ']', '}', '[', '{', '\'', '\"', ';', ':', '/', '?', '.', '>', '<', ',', ' ' });
				AnaliticsTextAllChat allChat = db.GetAnaliticsAllChat(clean.ToLower());
				AnalyticsText analyticsText = db.GetAnalitics(channel, clean.ToLower());
				if (allChat == null)
				{
					if (clean.Length > 3)
					{
						db.SetValue<AnaliticsTextAllChat>(new AnaliticsTextAllChat() { NameId = clean.ToLower(), Count = 1 });
					}
				}
				else
				{
					allChat.Count++;
				}
				if (analyticsText == null)
				{
					if (clean.Length > 3)
					{
						db.SetValue<AnalyticsText>(new AnalyticsText()
						{
							channel                = channel,
							ChannelId              = channel.IDChannel,
							Count                  = 1,
							AnaliticsTextAllChatId = clean.ToLower()
						});
					}
				}
				else
				{
					analyticsText.Count++;
				}
			}
			db.Save();
		}

		private void AddAnaliticsPharase(Message _message, Settings settings)
		{
			DataBase db = Singleton.GetInstance().Context;
			Channel channel = db.GetChannel(_message);
			
			String ThisText;
			if (_message.Text != null)
			{
				ThisText = _message.Text;
			}
			else
			{
				ThisText = _message.Caption;
			}
			
			if(ThisText == null) return;

			System.String clean = "";

			foreach (System.String word in ThisText.Split(" "))
			{
				clean += word.Trim(new System.Char[] { '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '-', '_', '=', '+', '|', '\\', ']', '}', '[', '{', '\'', '\"', ';', ':', '/', '?', '.', '>', '<', ',', ' ' }) + " ";
			}

			Boolean coincidence = false;
			AnaliticsPhraseAllChat[] analiticsPhraseAllChats = db.GetAnaliticsPhraseAllChats();
			if (_message.Chat.Type == Telegram.Bot.Types.Enums.ChatType.Supergroup || _message.Chat.Type == Telegram.Bot.Types.Enums.ChatType.Group)
			{
				if (analiticsPhraseAllChats.Length != 0)
				{
					if (clean.Length > 5)
					{
						foreach (AnaliticsPhraseAllChat text in analiticsPhraseAllChats)
						{
							if (CalculateFuzzyEqualValue(clean.ToLower(), text.NameId, settings) >= settings.ProcentMessage)
							{
								coincidence = true;
								text.Count++;
								AnaliticsPhrase analyticsText = db.GetAnaliticsPharse(channel, text.NameId);
								if (analyticsText != null)
								{
									analyticsText.Count++;
								}
								else
								{
									db.SetValue<AnaliticsPhrase>(new AnaliticsPhrase() { channel = channel, ChannelId = channel.IDChannel, Count = 1, AnaliticsPhraseAllChatId = clean.ToLower() });
								}
							}
						}

						if (coincidence == false)
						{
							db.SetValue<AnaliticsPhraseAllChat>(new AnaliticsPhraseAllChat() { NameId = clean.ToLower(), Count = 1 });
							db.SetValue<AnaliticsPhrase>(new AnaliticsPhrase() { channel              = channel, ChannelId     = channel.IDChannel, Count = 1, AnaliticsPhraseAllChatId = clean.ToLower() });
							db.Save();
						}

						return;
					}
				}
			
				if (clean.Length > 5)
				{
					db.SetValue<AnaliticsPhraseAllChat>(new AnaliticsPhraseAllChat() { NameId = clean.ToLower(), Count = 1 });
					db.SetValue<AnaliticsPhrase>(new AnaliticsPhrase() { channel = channel, ChannelId = channel.IDChannel, Count = 1, AnaliticsPhraseAllChatId = clean.ToLower() });
					db.Save();
				}
			}
		}

		private void AddAnaliticsPharaseMonth(TelegramBotClient botClient, Message _message, Settings settings)
		{
			DataBase db = Singleton.GetInstance().Context;
			Channel channel = db.GetChannel(_message);

			String ThisText;
			if (_message.Text != null)
			{
				ThisText = _message.Text;
			}
			else
			{
				ThisText = _message.Caption;
			}
			
			System.String clean = "";

			foreach (System.String word in ThisText.Split(" "))
			{
				clean += word.Trim(new System.Char[] { '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '-', '_', '=', '+', '|', '\\', ']', '}', '[', '{', '\'', '\"', ';', ':', '/', '?', '.', '>', '<', ',', ' ' }) + " ";
			}
			
			AnaliticsPhraseMonth[] analiticsPhraseMonth = db.GetAnaliticsPhraseMonth();
			if (_message.Chat.Type == Telegram.Bot.Types.Enums.ChatType.Supergroup || _message.Chat.Type == Telegram.Bot.Types.Enums.ChatType.Group)
			{
				if (clean.Length > 5)
				{
					if (analiticsPhraseMonth.Length != 0)
					{
						foreach (AnaliticsPhraseMonth text in analiticsPhraseMonth)
						{
							if (text.channel.IDChannel == channel.IDChannel && text.DateTime.Month == System.DateTime.Today.Month)
							{
								if (CalculateFuzzyEqualValue(clean.ToLower(), text.Phrase, settings) >= settings.ProcentMessage)
								{
									text.Count++;
									AnaliticsPhraseMonth phrase = analiticsPhraseMonth.FirstOrDefault(p => p.Phrase == text.Phrase 
									                                                                       && p.DateTime.Month == System.DateTime.Today.AddMonths(-1).Month 
									                                                                       && p.channel.IDChannel == channel.IDChannel);
									if (phrase != null)
									{
										if (phrase.Count < text.Count && phrase.isVerified == false)
										{
											phrase.isVerified = true;
											System.String temp = $"‼Увеличено число использование фразы‼\n\nФраза:\n💹 {text.Phrase} 💹\n\nГруппа: 👨‍👩‍👧‍👦 {text.channel.ChannelName} 👨‍👩‍👧‍👦\n\nКол-во использования: 🔝 {text.Count} 🔝";
											botClient.SendText(settings.ChannelAdmin, temp);
										}
									}

									return;
								}
							}
						}
					}
					
					db.SetValue<AnaliticsPhraseMonth>(new AnaliticsPhraseMonth() { Phrase = clean, channel = channel, ChannelId = channel.IDChannel, Count = 1, DateTime = System.DateTime.Today, isVerified = false });
					db.Save();
				}
			}
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

		public async void ThisWord(TelegramBotClient botClient, Message _message, User user, Settings settings, TMessage tmessage)
		{
			DataBase db = Singleton.GetInstance().Context;
			System.String temp;
			if (_message.Text != null)
			{
				temp = _message.Text.ToLower();
			}
			else if (_message.Sticker != null)
			{
				temp = _message.Sticker.SetName.ToLower();
			}
			else if (_message.Photo != null)
			{
				temp = "";
			}
			else
			{
				temp = "";
			}

			if (_message.Caption != null)
			{
				Console.WriteLine("Caption");
				temp = _message.Caption;
			}

			Word word =  null;
			System.Char[] seperator = { '\n', ' ', ',', '.', '/', '\\', '|', ':', ';', '!', '(', ')', '?', '-', '~', '=', '_', '<', '>', '\t' };
			for (System.Int32 i = 0; i < temp.Split(seperator).Length; i++)
			{
				word = db.GetWords(temp.Split(seperator)[i]);
				if (word != null)
					break;
			}

			if (word != null && user.IsAdmin == 0)
			{
				System.String banText = _message.Text + $"\nБан был выданан в группе: {_message.Chat.FirstName}";
				await botClient.DeleteMessageAsync(_message.Chat.Id, _message.MessageId);
				if (settings.IsBanOrKicOrMutkMat == 0 && user.IsAdmin == 0)
				{
					if (CheckCountBan.CheckBan(botClient, _message, user, db, "На вас наложено молчание за запрещенные слова! Бан выдан системой UBC!", banText)) return;
					else
					{
						user.BanDate = System.DateTime.Now.AddDays(settings.CountBanMat);
						user.Test++;
						user.BanDescript = "Вы были забанены за запрещенные слова! Бан выдан системой UBC!";

						db.Save();

						IsBanUser.ThisBan(botClient, _message, user, settings);
					}


					IsBan.Ban(botClient, _message);
				}
				else if (settings.IsBanOrKicOrMutkMat == 1 && user.IsAdmin == 0)
				{
					if (CheckCountBan.CheckBan(botClient, _message, user, db, "На вас наложено молчание за запрещенные слова! Бан выдан системой UBC!", banText)) return;
					else
					{
						user.BanDate = System.DateTime.Now.AddDays(settings.CountBanMat);
						user.Test++;
						user.BanDescript = "Вы были кикнуты за запрещенные слова! Бан выдан системой UBC!";
						db.Save();
						IsKick.ThisKick(botClient, user);
					}


					IsBan.Ban(botClient, _message);
				}
				else if (settings.IsBanOrKicOrMutkMat == 2 && user.IsAdmin == 0)
				{
					if (CheckCountBan.CheckBan(botClient, _message, user, db, "На вас наложено молчание за запрещенные слова! Бан выдан системой UBC!", banText)) return;
					else
					{
						user.BanDate = System.DateTime.Now.AddDays(settings.CountBanMat);
						user.Test++;
						user.BanDescript = "Вы были забанены за запрещенные слова! Бан выдан системой UBC!";
						db.Save();

						IsMute.ThisMut(botClient, user);
					}


					IsBan.Ban(botClient, _message);
				}
				user.PayConfirm = false;
				user.PayDate = System.DateTime.Today;
				db.Save();
				System.String text = "<b>Бан системой UBC!</b>\n" + "ФИО: " + user.FIO;
				text += user.Username != "Нет!" ? "\n🧸Юзернейм: @" + user.Username : "";
				text += "\nНомер: " + user.Number + "\nПричина бана: " + user.BanDescript + $"\n{banText}";
				botClient.SendText(settings.ChannelAdmin, text, user, true);
				return;
			}
			else if (_message.Chat.Id != user.ID)
			{
				UserMessage userMessage =  db.GetUserMessage(_message.Text, user);

				if (tmessage == null)
				{
					await Task.Run(() => AddNewChannel.SetTmessage());
				}

				if (user.IsAdmin == 0 && _message.Audio == null && _message.Type != Telegram.Bot.Types.Enums.MessageType.Voice 
				    && _message.Type != Telegram.Bot.Types.Enums.MessageType.Video || _message.Type != Telegram.Bot.Types.Enums.MessageType.VideoNote 
				                                                                   || _message.Type != Telegram.Bot.Types.Enums.MessageType.Document 
				                                                                   || _message.Type != Telegram.Bot.Types.Enums.MessageType.Game 
				                                                                   || _message.Type != Telegram.Bot.Types.Enums.MessageType.Location 
				                                                                   || _message.Type != Telegram.Bot.Types.Enums.MessageType.Contact 
				                                                                   || _message.Type != Telegram.Bot.Types.Enums.MessageType.MessagePinned)
				{
					ChekTextAsync(botClient, _message, user, settings, tmessage);
				}
				else
				{
					if (tmessage.Post != 0)
					{
						tmessage.Post--;
						db.Save();
					}
				}
			}
		}
	}

	internal static class CheckLink
	{
		public static async void ThisLink(TelegramBotClient botClient, Message _message, User user, Settings settings)
		{
			DataBase db = Singleton.GetInstance().Context;

			var channel = db.GetChannels();
			
			if (_message.EntityValues != null)
			{
				foreach (System.String item in _message.EntityValues)
				{
					//System.Int32 temp = StartSession.NumberOfParticipants(item.Split('@', ' ')[0]);
					//if (temp != -1)
					//{

					String TempText = item.Split("https://")[1];
						if (channel.Any(p => p.LinkChannel == TempText)) continue;

						try
						{
							System.String banText = $"\nБан был выданан в группе: @{_message.Chat.InviteLink}" + _message.Text;
							await botClient.DeleteMessageAsync(_message.Chat.Id, _message.MessageId);

							if (settings.IsBanOrKickOrMutLink == 0 && user.IsAdmin == 0)
							{
								if (CheckCountBan.CheckBan(botClient, _message, user, db, "На вас наложено молчание за распространение ссылок! Бан выдан системой UBC!", banText)) return;
								else
								{
									user.BanDate = System.DateTime.Now.AddDays(settings.CountLink);
									user.Test++;
									user.BanDescript = "Вы были забанены за распространение ссылок! Бан выдан системой UBC!";
									db.Save();
									IsBanUser.ThisBan(botClient, _message, user, settings);
								}

								System.String text = "<b>Бан системой UBC!</b>\n" + "ФИО: " + user.FIO;
								text += user.Username != "Нет!" ? "\n🧸Юзернейм: @" + user.Username : "";
								text += "\nНомер: " + user.Number + "\nПричина бана: " + user.BanDescript;
								botClient.SendText(settings.ChannelAdmin, text, user, true);
								IsBan.Ban(botClient, _message);


							}
							else if (settings.IsBanOrKickOrMutLink == 1 && user.IsAdmin == 0)
							{
								if (CheckCountBan.CheckBan(botClient, _message, user, db, "На вас наложено молчание за распространение ссылок! Бан выдан системой UBC!", banText)) return;
								else
								{
									user.BanDate = System.DateTime.Now.AddDays(settings.CountLink);
									user.Test++;
									user.BanDescript = "Вы были кикнуты за распространение ссылок! Бан выдан системой UBC!";
									db.Save();
									IsKick.ThisKick(botClient, user);
								}

								System.String text = "<b>Бан системой UBC!</b>\n" + "ФИО: " + user.FIO;
								text += user.Username != "Нет!" ? "\n🧸Юзернейм: @" + user.Username : "";
								text += "\nНомер: " + user.Number + "\nПричина бана: " + user.BanDescript;
								botClient.SendText(settings.ChannelAdmin, text, user, true);
								IsBan.Ban(botClient, _message);

							}
							else if (settings.IsBanOrKickOrMutLink == 2 && user.IsAdmin == 0)
							{
								if (CheckCountBan.CheckBan(botClient, _message, user, db, "На вас наложено молчание за распространение ссылок! Бан выдан системой UBC!", banText)) return;
								else
								{
									user.BanDate = System.DateTime.Now.AddDays(settings.CountLink);
									user.Test++;
									user.BanDescript = "На вас наложено молчание за распространение ссылок! Бан выдан системой UBC!";
									db.Save();
									IsMute.ThisMut(botClient, user);
								}

								System.String text = "<b>Бан системой UBC!</b>\n" + "ФИО: " + user.FIO;
								text += user.Username != "Нет!" ? "\n🧸Юзернейм: @" + user.Username : "";
								text += "\nНомер: " + user.Number + "\nПричина бана: " + user.BanDescript + $"\n{banText}";
								botClient.SendText(settings.ChannelAdmin, text, user, true);
								IsBan.Ban(botClient, _message);

							}
							user.PayConfirm = false;
							user.PayDate = System.DateTime.Today;
							db.Save();
						}
						catch (System.Exception ex)
						{
							Log.Logging("Analize - ChekLink: " + ex);
						}
						return;
					}
				//}
			}
		}
	}

	internal static class CheckInvite
	{
		public static async void ThisInvite(TelegramBotClient botClient, Message _message, User user, Channel channel)
		{
			DataBase db = Singleton.GetInstance().Context;
			try
			{
				if (channel == null)
				{
					Chat chat = await botClient.GetChatAsync(_message.Chat.Id);
					db.SetChannel(chat);
					db.Save();
					await Task.Run(() => AddNewChannel.SetTmessage());
				}
				else
				{
					Settings settings = db.GetSettings();
					InvitedUser[] invitedUsers = db.GetInvitedUsers();
					AdUser addUser = db.GetAdUser(user.ID);
					System.Int32 temp = _message.NewChatMembers.Count();
					System.Int32 Coin = 0;
					if (invitedUsers != null)
					{
						foreach (Telegram.Bot.Types.User users in _message.NewChatMembers)
						{
							if (invitedUsers.Any(p => p.UserAddedId == users.Id))
							{
								temp--;
							}
							else
							{
								if (user.BanDate > System.DateTime.Now)
								{
									user.CountBanAddPeople++;
									if (user.CountBanAddPeople >= settings.AddUser)
									{
										user.BanDate = System.DateTime.Now;
										IsUnBan.ThisUnBan(botClient, user);
									}
								}
								db.SetInvitedUser(users.Id, _message.From.Id, _message.Chat.Id);
								db.Save();
								Coin += settings.Coin;
							}
						}
						user.AddMembers += temp;
						addUser.Balance += Coin;
						db.Save();
						botClient.SendText(user.ID, $"Вы добавили {temp} человек в наши группы, вам начислено бонусы в размере {Coin}💰.\nОбщее количество баланса можно посмотреть в \"Реклама\" -> \"Баланс\"\nМы очень рады что помогаете нам расширить нашу аудиторию, продолжайете в том же духе 👍", replyMarkup: Advertising.InlineButton.ButtonOk());
					}
					else
					{
						user.AddMembers += temp;
						addUser.Balance += settings.Coin;
						db.SetInvitedUser(_message);
						db.Save();
						botClient.SendText(user.ID, $"Вы добавили {temp} человек в наши группы, вам начислено бонусы в размере {settings.Coin}💰.\nОбщее количество баланса можно посмотреть в \"Реклама\" -> \"Баланс\"\nМы очень рады что помогаете нам расширить нашу аудиторию, продолжайете в том же духе 👍", replyMarkup: Advertising.InlineButton.ButtonOk());
					}
				}
				
			}
			catch (System.Exception ex)
			{
				Log.Logging("SettingBot : AbsCommand: " + ex);
			}
		}
	}

	internal static class CheckPhoto
	{
		private static System.Object _Lock = new System.Object();

		public static void ThisPhoto(TelegramBotClient botClient, Message _message, User user, Channel channel, TMessage tmessage)
		{
			DataBase db = Singleton.GetInstance().Context;
			try
			{
				lock (_Lock)
				{
					List<PhotoDate> photo = Singleton.GetInstance().Context._photoData.Where(p => p.IDChannel == channel.IDChannel && p.IDUser == user.ID && p.timeMessage >= System.DateTime.Now.Subtract(System.TimeSpan.FromSeconds(30))).ToList();

					if (photo.Count >= 2)
					{
						foreach (PhotoDate i in photo)
						{
							try
							{
								botClient.DeleteMessageAsync(_message.Chat.Id, i.MessageID);
							}
							catch (System.Exception Ex) { Log.Logging(Ex); }
						}
						try
						{
							botClient.DeleteMessageAsync(_message.Chat.Id, _message.MessageId);
						}
						catch (System.Exception Ex) { Log.Logging(Ex); }

						db.RemoveRangePhotoList(photo);
						db.Save();

						botClient.SendTextMessageAsync(_message.From.Id, "Ваше сообщение было удалено, по причине:\nИспользуйте альбом вместо одиночного сообщения!", replyMarkup: Advertising.InlineButton.ButtonOk());
						return;
					}

					tmessage.Post--;

					db.SetPhoto(new PhotoDate() { IDChannel = channel.IDChannel, IDUser = user.ID, timeMessage = System.DateTime.Now, MessageID = _message.MessageId });
					db.Save();
				}
			}
			catch (System.Exception Ex) { Log.Logging(Ex); }
		}
	}

	internal class FludChain : AbstractHandlerAnaliz
	{
		public override System.Object Handle(TelegramBotClient botClient, System.Object message, User user, Settings settings, TMessage tmessage, Channel channel)
		{
			Message _message = message as Message;

			if (settings.Timer > System.DateTime.Now && _message.Chat.Id != user.ID)
			{
				return null;
			}
			else
			{
				return base.Handle(botClient, message, user, settings, tmessage, channel);
			}
		}
	}

	internal class AddPeopleChain : AbstractHandlerAnaliz
	{
		public override System.Object Handle(TelegramBotClient botClient, System.Object message, User user, Settings settings, TMessage tmessage, Channel channel)
		{
			Message _message = message as Message;

			if ((user.AddMembers < settings.AddUser && user.IsAdmin == 0 && _message.Chat.Id != user.ID && _message.NewChatMembers == null) || (user.AddMembers < settings.AddUser && user.IsAdmin == 0 && _message.Chat.Id != user.ID && _message.Sticker != null && _message.NewChatMembers == null))
			{
				if (user.PayConfirm == false)
				{
					DeletePost.ThisDelete(botClient, _message, "Ваше сообщение было удалено, по причине:\nВам нужно добавить " + settings.AddUser + " человек в любую нашу группу, что бы начать общаться!\nC нашими сообществами можете ознакомиться здесь @allUBC или в главном меню нажать на кнопку \"💭Чаты💭\"/*\nТакже вы можете купить абонимент в наши чаты и общаться без ограничения!*/");
				}
				return null;
			}
			else
			{
				return base.Handle(botClient, message, user, settings, tmessage, channel);
			}
		}
	}

	internal class PostNullChain : AbstractHandlerAnaliz
	{
		public override System.Object Handle(TelegramBotClient botClient, System.Object message, User user, Settings settings, TMessage tmessage, Channel channel)
		{
			Message _message = message as Message;

			if (_message.Chat.Id != user.ID && tmessage.Post <= 0 && _message.Type != Telegram.Bot.Types.Enums.MessageType.ChatMembersAdded && user.IsAdmin == 0 && user.PayConfirm == false)
			{
				DeletePost.ThisDelete(botClient, _message, "Ваше сообщение было удалено, по причине:\nВы исчерпали дневной лимит сообщения для данного чата: " 
				                                           + _message.Chat.Title
				                                           + "!\nНе огорчайтесь так как вы можете общаться в других чатах!\nC нашими сообществами можете ознакомиться здесь @allUBC или в главном меню нажать на кнопку \"💭Чаты💭\"");
				return null;
			}
			else
			{
				return base.Handle(botClient, message, user, settings, tmessage, channel);
			}
		}
	}

	internal class CheckChannelChain : AbstractHandlerAnaliz
	{
		public override System.Object Handle(TelegramBotClient botClient, System.Object message, User user, Settings settings, TMessage tmessage, Channel channel)
		{
			Message _message = message as Message;

			if (_message.ForwardFromChat != null && _message.Type != Telegram.Bot.Types.Enums.MessageType.ChatMembersAdded && user.IsAdmin == 0)
			{
				CheckChannel.ThisChannel(botClient, _message, user, channel, tmessage);
				return null;
			}
			else
			{
				return base.Handle(botClient, message, user, settings, tmessage, channel);
			}
		}
	}

	internal class CheckWordAdnLinkChain : AbstractHandlerAnaliz
	{
		public override System.Object Handle(TelegramBotClient botClient, System.Object message, User user, Settings settings, TMessage tmessage, Channel channel)
		{
			Message _message = message as Message;

			if ((_message.Text != null && _message.Chat.Id != _message.From.Id && _message.Type != Telegram.Bot.Types.Enums.MessageType.ChatMembersAdded) 
			    || (_message.Text == null && _message.Chat.Id != _message.From.Id && _message.Sticker != null && _message.Type != Telegram.Bot.Types.Enums.MessageType.ChatMembersAdded) 
			    || (_message.Text == null && _message.Chat.Id != _message.From.Id && _message.Photo == null && _message.Type != Telegram.Bot.Types.Enums.MessageType.ChatMembersAdded) 
			    || (_message.Text != null && _message.Chat.Id != _message.From.Id && _message.Type != Telegram.Bot.Types.Enums.MessageType.ChatMembersAdded && _message.Audio != null) 
			    || (_message.Type == Telegram.Bot.Types.Enums.MessageType.Voice)
			    ||(_message.Caption != null && _message.Text == null && _message.Chat.Id != _message.From.Id && _message.Type  != Telegram.Bot.Types.Enums.MessageType.ChatMembersAdded) 
			    || (_message.Caption == null && _message.Text == null && _message.Chat.Id != _message.From.Id && _message.Sticker != null && _message.Type  != Telegram.Bot.Types.Enums.MessageType.ChatMembersAdded) 
			    || (_message.Caption == null && _message.Text == null && _message.Chat.Id != _message.From.Id && _message.Photo == null && _message.Type  != Telegram.Bot.Types.Enums.MessageType.ChatMembersAdded) 
			    || (_message.Caption != null && _message.Text == null && _message.Chat.Id != _message.From.Id && _message.Type != Telegram.Bot.Types.Enums.MessageType.ChatMembersAdded && _message.Audio != null) 
			    || (_message.Type == Telegram.Bot.Types.Enums.MessageType.Voice))
			{
				if (user.IsAdmin == 0)
				{
					CheckWord checkWord = new CheckWord();
					checkWord.ThisWord(botClient, _message, user, settings, tmessage);
					CheckLink.ThisLink(botClient, _message, user, settings);
				}

				return null;
			}
			else
			{
				return base.Handle(botClient, message, user, settings, tmessage, channel);
			}
		}
	}

	internal class CheckInviteChain : AbstractHandlerAnaliz
	{
		public override System.Object Handle(TelegramBotClient botClient, System.Object message, User user, Settings settings, TMessage tmessage, Channel channel)
		{
			Message _message = message as Message;

			if (_message.NewChatMembers != null)
			{
				CheckInvite.ThisInvite(botClient, _message, user, channel);
				return null;
			}
			else
			{
				return base.Handle(botClient, message, user, settings, tmessage, channel);
			}
		}
	}

	internal class CheckPhotoChain : AbstractHandlerAnaliz
	{
		public override System.Object Handle(TelegramBotClient botClient, System.Object message, User user, Settings settings, TMessage tmessage, Channel channel)
		{
			Message _message = message as Message;

			if (_message.Photo != null && _message.Chat.Type == Telegram.Bot.Types.Enums.ChatType.Supergroup && user.IsAdmin == 0)
			{
				CheckPhoto.ThisPhoto(botClient, _message, user, channel, tmessage);
				return null;
			}
			else
			{
				return base.Handle(botClient, message, user, settings, tmessage, channel);
			}
		}
	}
}