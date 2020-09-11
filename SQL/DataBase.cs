using System;
using System.Data.SQLite;
using BotCore.SQL;
using LiqPay.SDK;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Nethereum.ABI.Util;
using RestSharp.Extensions;

namespace BotCore
{
	internal class DataBase : DbContext
	{
		public DataBase() => Database.SetCommandTimeout(150);//Database.Migrate();

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlite($"Data Source={AppDomain.CurrentDomain.BaseDirectory}\\BotSteam.db");
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<PostChannel>().HasKey(c => new { c.PostTemplateId, c.ChannelId });
			

			modelBuilder.Entity<PostTemplate>().HasMany<PostContent>(p => p.PostContent).WithOne(p => p.PostTemplate).HasForeignKey(p => p.PostTemplateId);

		}

		public DbSet<User> _users { get; set; }
		public DbSet<Word> _words { get; set; }
		public DbSet<Reviews> _reviews { get; set; }
		public DbSet<Rating> _ratings { get; set; }
		public DbSet<Complaint> _complaints { get; set; }
		public DbSet<Appeal> _appeals { get; set; }
		public DbSet<TMessage> _tmessage { get; set; }
		public DbSet<UserMessage> _userMessages { get; set; }
		public DbSet<ChannelMessage> _channelMessages { get; set; }
		public DbSet<Settings> _settings { get; set; }
		public DbSet<TempBase> _tempBase { get; set; }
		public DbSet<Channel> _channels { get; set; }
		public DbSet<PhotoDate> _photoData { get; set; }
		public DbSet<InvitedUser> _invitedUsers { get; set; }
		public DbSet<FeaturedUser> _featuredUsers { get; set; }
		public DbSet<FeaturedUserNew> _featuredUserNews { get; set; }

		// Advertising
		public DbSet<AdUser> _adUsers { get; set; }

		public DbSet<PostTemplate> _postTemplates { get; set; }
		public DbSet<PostChannel> _postChannel { get; set; }
		public DbSet<PostTime> _postTime { get; set; }
		public DbSet<PostContent> _postContent { get; set; }
		public DbSet<Post> _posts { get; set; }

		// Transaction
		public DbSet<Transaction> _transactions { get; set; }
		public DbSet<TransactionId> _transactionIds { get; set; }
		public DbSet<AddresBTC> _addresBTCs { get; set; }
		public DbSet<Category> _categories { get; set; }

		public DbSet<AnalyticsText> _analyticsTexts { get; set; }
		public DbSet<AnaliticsTextAllChat> _analiticsTextAllChats { get; set; }
		public DbSet<AnaliticsPhraseAllChat> _analiticsPhraseAllChats { get; set; }
		public DbSet<AnaliticsPhrase> _analiticsPhrases { get; set; }

		public DbSet<AnaliticsPhraseMonth> _analiticsPhraseMonths { get; set; }

		//Income
		public DbSet<Income> _incomes { get; set; }
		public DbSet<IncomeChannel> _incomeChannels { get; set; }
		public DbSet<IncomeChannelAdmin> _incomeChannelAdmins { get; set; }
		public DbSet<ChannelAdmin> channelAdmins { get; set; }
		
		public DbSet<TextNotification> _TextNotifications { get; set; }
		
		public DbSet<ButtonNotification> _ButtonNotifications { get; set; }
		
		public DbSet<ButtonAndTextNotication> _ButtonAndTextNotications { get; set; }
		public DbSet<CollectionButtonNotification> _CollectionButtonNotifications { get; set; }
		
		public DbSet<CollectionPictureNotification> _CollectionPictureNotifications { get; set; }
		
		public DbSet<NotificationChat> _notificationChats { get; set; }
		
	}
}