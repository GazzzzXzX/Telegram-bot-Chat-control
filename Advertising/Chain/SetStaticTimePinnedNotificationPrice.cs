using System;
using BotCore.Blockchain;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCore.Advertising
{
	public class SetStaticTimePinnedNotificationPrice: AbstractHandlerAdvertising, IStandartCommand
	{
		private User user = null;
		private DataBase db = null;
		private Message _message = null;

		public override System.Object Handle(System.Int32 request, TelegramBotClient botClient, System.Object message)
		{
			if (request == (System.Int32)SetChain.SetStaticTimePinnedNotificationPrice)
			{
				IStandartCommand standartCommand = new SetPriceTime();

				if (standartCommand.SetDataBase(out db)) return null;

				if (standartCommand.SetMessage(message, out _message)) return null;

				if (standartCommand.SetUserAndCheckIsNullMessage(botClient, _message, out user, db)) return null;

				ChangeUser();

				ChangePrice();

				SendMessage(botClient);				
				return null;
			}
			else
			{
				return base.Handle(request, botClient, message);
			}
		}

		private void ChangeUser()
		{
			user.Chain = (Int32)SetChain.MessageUserInBot;
		}

		private void ChangePrice()
		{
			Int32 price = 10;
			Int32.TryParse(_message.Text, out price);
			
			AdController.PriceData.postTypePinnedNotificationPrice = price;

			AdController.SavePriceData();
		}

		public void SendMessage(TelegramBotClient botClient)
		{
			botClient.DeleteMessage(user.ID, _message.MessageId,"");
			botClient.EditMessage(user.ID, user.MessageID, "Настройка рекламы", "56 SetPriceTime", user,  replyMarkup: InlineButton.OptionAdvertising);
		}
	}
}