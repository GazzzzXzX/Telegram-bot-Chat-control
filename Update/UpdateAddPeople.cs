using System.Threading.Tasks;

using Quartz;

namespace BotCore.Update
{
	internal class UpdateAddPeople : IJob
	{
		public async Task Execute(IJobExecutionContext context)
		{
			DataBase db = Singleton.GetInstance().Context;
			User[] users = db.GetUsers();

			foreach (User user in users)
			{
				user.AddMembers = 0;
			}
			db.Save();
		}
	}
}