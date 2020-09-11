using System.Threading.Tasks;

using Quartz;

namespace BotCore
{
	internal class UpdateCountReviews : IJob
	{
		public async Task Execute(IJobExecutionContext context)
		{
			DataBase db = Singleton.GetInstance().Context;
			User[] users = db.GetUsers();
			foreach (User user in users)
			{
				if (user.DateRegister.Month + 1 == System.DateTime.Today.Month)
				{
					user.CountReviews = 3;
					user.DateRegister = System.DateTime.Today;
				}
			}
			db.Save();
		}
	}
}