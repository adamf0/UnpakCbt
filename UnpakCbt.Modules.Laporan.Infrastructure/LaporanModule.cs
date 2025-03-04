using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UnpakCbt.Common.Application.Data;
using UnpakCbt.Common.Infrastructure.Data;
using UnpakCbt.Modules.Laporan.Presentation.JadwalUjian;

namespace UnpakCbt.Modules.Laporan.Infrastructure
{
    public static class LaporanModule
    {
        public static void MapEndpoints(IEndpointRouteBuilder app)
        {
            LaporanEndpoints.MapEndpoints(app);
        }

        public static IServiceCollection AddLaporanModule(
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
            string redisConnectionString = configuration.GetValue<string>("Redis:ConnectionString") ?? "localhost:6379";

            services.AddScoped<IDbConnectionFactory>(_ => new DbConnectionFactory(databaseConnectionString));
        }
    }
}
