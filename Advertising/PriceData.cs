using System;

namespace BotCore.Advertising
{
	[Serializable]
	public class PriceData
	{
		public Single postSymbolPrice = 0.0f;
		public Single postImagePrice = 0.0f;
		public Single postTypeDefault = 0.5f;
		public Single postTypePinnedPrice = 0.5f;
		public Single postTypePinnedNotificationPrice = 0.5f;
		public Single postTypeTime = 0.0f;
	}
}