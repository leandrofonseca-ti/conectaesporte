

using ConectaEsporte.Core.Models;
using Microsoft.Extensions.Configuration;
using System.Reflection.Metadata;

namespace ConectaEsporte.Core.Database
{
	public interface IAppDbContext : IDisposable
	{
		  DbSet<User> user { get; set; }
		  DbSet<Profile> profile { get; set; }
		  DbSet<UserProfile> userprofile { get; set; }
	}

	public class AppDbContext : DbContext, IAppDbContext
	{
		private string connectionString = string.Empty;
		public DbSet<User> user { get; set; }
		public DbSet<Profile> profile { get; set; }
		public DbSet<UserProfile> userprofile { get; set; }
		public AppDbContext(DbContextOptions options, IConfiguration configuration) : base(options)
		{
			connectionString = configuration.GetConnectionString("MySqlConn");
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{

			optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
			base.OnConfiguring(optionsBuilder);
		}
	}

	public interface IAppDbContextFactory
	{
		IAppDbContext CreateContext();
	}

	public class AppDbContextFactory : IAppDbContextFactory
	{
		private Func<IAppDbContext> _contextCreator;
		public AppDbContextFactory(Func<IAppDbContext> contextCreator)// This is fine with .net and unity, this is treated as factory function, but creating problem in .netcore service provider
		{
			_contextCreator = contextCreator;
		}

		public IAppDbContext CreateContext()
		{
			return _contextCreator();
		}
	}
}
