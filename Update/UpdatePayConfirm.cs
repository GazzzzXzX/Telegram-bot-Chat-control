using System.Threading.Tasks;

using Quartz;

namespace BotCore.Update
{
	internal class UpdatePay : IJob
	{
		public async Task Execute(IJobExecutionContext context)
		{
			DataBase db = Singleton.GetInstance().Context;
			User[] users = db.GetUsers();

			foreach (User user in users)
			{
				if (System.DateTime.Today >= user.PayDate && user.PayConfirm == true)
				{
					user.PayConfirm = false;
				}
			}
			db.Save();
		}
	}
}
