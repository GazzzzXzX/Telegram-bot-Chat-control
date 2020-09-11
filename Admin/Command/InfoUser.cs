using BotCore.SQL;

namespace BotCore
{
	internal static class InfoUser
	{
		public static System.String Info(User user, User Admin = null)
		{
			System.String temp;
			DataBase db = Singleton.GetInstance().Context;
			AdUser adUser = db.GetAdUser(user.ID);
			if (Admin == null)
			{
				temp = "🔍<b>Ваша карточка в системе UBC</b>🔍\n\n" + "🆔Id: " + user.ID;
				temp += user.Username != "Нет!" ? "\n🧸Юзернейм: @" + user.Username : "\n ";
				temp += "\n🖌ФИО: " + user.FIO + "\n📱Номер: " + user.Number + "\n⭐️Рейтинг: " + user.SumRating + "/5";
				temp += "\n💰Баланс: " + adUser.Balance + " грн";
				return temp;
			}
			else if (Admin.IsAdmin > 0)
			{
				temp = "🔍Пользователь найден🔍\n\n" + "🆔Id: " + user.ID;
				temp += user.Username != "Нет!" ? "\n🧸Юзернейм: @" + user.Username : " ";
				temp += "\n🖌ФИО: " + user.FIO + "\n📱Номер: " + user.Number + "\n⭐️Рейтинг: " + user.SumRating + "/5" + "\n⏬Добавил(а) людей: " + user.AddMembers;
				return temp;
			}
			return null;
		}

		public static System.String Search(User user, User admin)
		{
			System.String temp = "";
			if (admin.IsAdmin == 0)
			{
				temp = "🔍Пользователь найден🔍\n\n" + "🆔Id: " + user.ID;
				temp += user.Username != "Нет!" ? "\n🧸Юзернейм: @" + user.Username : " ";
				temp += "\n🖌ФИО: " + user.FIO + "\n📱Номер: " + user.Number + "\n⭐️Рейтинг: " + user.SumRating + "/5";
				return temp;
			}
			else
			{
				temp = "🔍Пользователь найден🔍\n\n" + "🆔Id: " + user.ID;
				temp += user.Username != "Нет!" ? "\n🧸Юзернейм: @" + user.Username : " ";
				temp += "\n🖌ФИО: " + user.FIO + "\n📱Номер: " + user.Number + "\n⭐️Рейтинг: " + user.SumRating + "/5";
				temp += "\n⏬Добавил(а) людей: " + user.AddMembers;
				return temp;
			}
		}

		public static System.String Star(User user, User userTwo, Reviews reviews)
		{
			System.String temp = "Вы оставили отзыв о "
								+ userTwo.FIO + "\n"
								+ reviews.Description
								+ "\nРейтинг: "
								+ System.Math.Round(userTwo.SumRating, 2) + "/5"
								+ "\n🚪Личный кабинет🚪\n\n🆔Id:: "
								+ user.ID + "\n🖌ФИО: " + user.FIO;
			temp += user.Username != "Нет!" ? "\n🧸Юзернейм: @" + user.Username : " ";
			temp += "\n📱Номер: "
								+ user.Number + "\n⭐️Рейтинг: "
								+ user.SumRating + "/5";
			return temp;
		}

		public static System.String BanAccaunt(User user)
		{
			DataBase db = Singleton.GetInstance().Context;
			Settings settings = db.GetSettings();
			System.String temp = "🚪Личный кабинет🚪\n🆔: "
								+ user.ID + "\n🖌ФИО: " + user.FIO;
			temp += user.Username != "Нет!" ? "\n🧸Username: @" + user.Username : " ";
			temp += "\n📱Номер: "
					+ user.Number + "\n⭐️Рейтинг: "
					+ user.SumRating + "\n✖️Бан: "
					+ user.BanDate + " дней\nПричина: "
					+ user.BanDescript
					+ " \n\nВы можете подать апелляцию, либо оплатить бан, после чего вы сразу будете разблокированы!\n\nСтоимость разбана: 100 грн\n\nТак же вы можете добавить в чат " + settings.AddUser + " человек и вы так же будете разбанены!";
			return temp;
		}
	}
}