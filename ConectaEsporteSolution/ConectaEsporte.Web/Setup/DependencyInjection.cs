using ConectaEsporte.Core.Database;
using ConectaEsporte.Core.Services.Repositories;
using ConectaEsporte.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace ConectaEsporte.Web.Setup
{
	public static class DependencyInjection
	{

 

		public static IServiceCollection ResolveDependenciesForCore(this IServiceCollection services,
			IConfiguration configuration)
		{
			//services.AddScoped<DbContext, AppDbContext>();
			//services.AddScoped<IContextTransactionScope<DbContext>, ContextTransactionScope>();

			services
		   .AddDbContext<AppDbContext>()
		   .AddTransient<IAppDbContext, AppDbContext>()
		   .AddTransient<IAppDbContextFactory, AppDbContextFactory>(
				   sp => new AppDbContextFactory(() => sp.GetService<IAppDbContext>())
			   );

			// Repositories.
			services.AddScoped<IUserRepository, UserService>();
			//services.AddLazySingleton<ISomething, Something>();
			// Helpers

			// Services.

			return services;

		}

		public static IServiceCollection AddInfrastruture(this IServiceCollection services,
			IConfiguration configuration)
		{
			var connectionString = configuration.GetConnectionString("MySqlConn");

			services.AddDbContext<AppDbContext>(options =>
			{
				options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
			});

			
			return services;
		}
	}
}
