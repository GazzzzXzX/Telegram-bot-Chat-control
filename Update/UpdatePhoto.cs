using System.Threading.Tasks;

using Quartz;

namespace BotCore
{
	internal class UpdatePhoto : IJob
	{
		public async Task Execute(IJobExecutionContext context)
		{
			DataBase db = Singleton.GetInstance().Context;
			db.RemoveRangePhotoData();
			db.Save();
		}
	}
}