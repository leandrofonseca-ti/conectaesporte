using ConectaEsporte.Core.Database;
using ConectaEsporte.Core.Services;
using ConectaEsporte.Core.Services.Repositories;
using ConectaEsporte.Uol.Repository;
using ConectaEsporte.Uol.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace ConectaEsporte.API.Setup
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
            services.AddScoped<IServiceRepository, GeralService>();
            services.AddScoped<IPaymentRepository, PaymentService>(); 
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
