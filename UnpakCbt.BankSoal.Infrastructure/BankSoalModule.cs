using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UnpakCbt.Common.Application.Data;
using UnpakCbt.Common.Infrastructure.Data;
using UnpakCbt.Modules.BankSoal.Application.Abstractions.Data;
using UnpakCbt.Modules.BankSoal.Domain.BankSoal;
using UnpakCbt.Modules.BankSoal.Infrastructure.BankSoal;
using UnpakCbt.Modules.BankSoal.Infrastructure.Database;
using UnpakCbt.Modules.BankSoal.Infrastructure.PublicApi;
using UnpakCbt.Modules.BankSoal.Presentation.BankSoal;
using UnpakCbt.Modules.BankSoal.PublicApi;

namespace UnpakCbt.Modules.BankSoal.Infrastructure
{
    public static class BankSoalModule
    {
        public static void MapEndpoints(IEndpointRouteBuilder app)
        {
            BankSoalEndpoints.MapEndpoints(app);
        }

        public static IServiceCollection AddBankSoalModule(
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

            services.AddDbContext<BankSoalDbContext>(optionsBuilder => optionsBuilder.UseMySQL(databaseConnectionString));

            services.AddScoped<IBankSoalRepository, BankSoalRepository>();

            services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<BankSoalDbContext>());

            services.AddScoped<IBankSoalApi, BankSoalApi>();
        }
    }
}
