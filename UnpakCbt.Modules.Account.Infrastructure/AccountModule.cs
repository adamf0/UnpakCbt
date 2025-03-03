using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UnpakCbt.Common.Application.Data;
using UnpakCbt.Common.Infrastructure.Data;
using UnpakCbt.Modules.Account.Infrastructure.Database;
using UnpakCbt.Modules.Account.Application.Abstractions.Data;
using UnpakCbt.Modules.Account.Domain.Account;
using UnpakCbt.Modules.Account.Infrastructure.Account;
using UnpakCbt.Modules.Account.Presentation.Account;

namespace UnpakCbt.Modules.Account.Infrastructure
{
    public static class AccountModule
    {
        public static void MapEndpoints(IEndpointRouteBuilder app)
        {
            AccountEndpoints.MapEndpoints(app);
        }

        public static IServiceCollection AddAccountModule(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.AddInfrastructure(configuration);

            return services;
        }

        private static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            string databaseConnectionString = configuration.GetConnectionString("Database")!;

            services.AddScoped<IDbConnectionFactory>(_ => new DbConnectionFactory(databaseConnectionString));

            services.AddDbContext<AccountDbContext>(optionsBuilder => optionsBuilder.UseMySQL(databaseConnectionString));

            services.AddScoped<IAccountRepository, AccountRepository>();

            services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<AccountDbContext>());
        }
    }
}
