using System;
using System.Threading.Tasks;

namespace BotCore.TelegramClient
{
	internal class SessionTelegram
	{
		public TLSharp.Core.TelegramClient client { get; private set; }

		private RegistrationSession _registrationInfo = new RegistrationSession();

		public async Task<Boolean> Start(Int32 id, String hash)
		{
			//if (client == null || client.IsUserAuthorized() == false)
			//{
			//	return false;
			//}
			client = new TLSharp.Core.TelegramClient(id, hash);
			await client.ConnectAsync();
			return client.IsConnected;
		}
		public async Task SendRequstOnServer(String phoneNumber)
		{
			_registrationInfo.PhoneNumber = phoneNumber;
			_registrationInfo.Hash = await client.SendCodeRequestAsync(phoneNumber);
		}
		public async Task CreateSession(String code)
		{
			if (_registrationInfo.PhoneNumber == null) return;
			_registrationInfo.Code = code;
			await client.MakeAuthAsync(_registrationInfo.PhoneNumber, _registrationInfo.Hash, _registrationInfo.Code);
		}
	}
}
