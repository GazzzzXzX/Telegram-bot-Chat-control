using System;
using System.Collections.Generic;

using BotCore.SQL;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCore.Blockchain
{
	internal interface IStandartCommand
	{
		public Boolean SetUserAndCheckIsNull(TelegramBotClient botClient, CallbackQuery _message, out User user, DataBase db)
		{
			user = db.GetUser(_message.From.Id);
			if (IsNullDataBase.IsNull(botClient, _message, user)) return true;
			else
			{
				if (user.Username == "Нет!" && _message.From.Username != null)
				{
					user.Username = _message.From.Username;
					db.Save();
				}
			}
			return false;
		}

		public Boolean SetUserAndCheckIsNullMessage(TelegramBotClient botClient, Message _message, out User user, DataBase db)
		{
			user = db.GetUser(_message.From.Id);
			if (IsNullDataBase.IsNull(botClient, _message, user)) return true;
			else
			{
				if (user.Username == "Нет!" && _message.From.Username != null)
				{
					user.Username = _message.From.Username;
					db.Save();
				}
			}
			return false;
		}

		public Boolean SetDataBase(out DataBase db)
		{
			db = Singleton.GetInstance().Context;
			return db == null;
		}

		public Boolean SetCallbackQuery(Object message, out CallbackQuery _message)
		{
			_message = message as CallbackQuery;
			return _message == null;
		}

		public Boolean SetMessage(Object message, out Message _message)
		{
			_message = message as Message;
			return _message == null;
		}

		public void SendMessage(TelegramBotClient botClient);
	}

	internal interface ITransactions
	{
		public Boolean SetTransaction(out List<BotCore.SQL.Transaction> transactions, DataBase db)
		{
			transactions = db.GetTransactions();
			return transactions == null;
		}
	}

	internal interface ISplitName
	{
		public Int32 GetNameSplit(String Name) => System.Convert.ToInt32(Name.Split(" ")[1]);
		public Int64 GetNameSplit64(String Name) => System.Convert.ToInt64(Name.Split(" ")[1]);
	}

	internal interface ITransaction
	{
		public Boolean GetTransaction(out Transaction transaction, Int32 IdTransaction, DataBase db)
		{
			transaction = db.GetTransaction(IdTransaction);
			return transaction == null;
		}
	}

	internal interface IChannel
	{
		public Boolean GetChannel(out Channel channel, Int64 idChannel, DataBase db)
		{
			channel = db.GetChannel(idChannel);
			return channel == null;
		}
	}

	internal interface ISplitNameInt64
	{
		public Int64 GetNameSplit(String Name) => System.Convert.ToInt64(Name.Split(" ")[1]);
	}
}