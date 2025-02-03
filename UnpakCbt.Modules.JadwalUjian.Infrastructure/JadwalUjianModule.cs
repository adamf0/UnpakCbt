using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using UnpakCbt.Common.Application.Data;
using UnpakCbt.Common.Infrastructure.Data;
using UnpakCbt.Modules.JadwalUjian.Domain.JadwalUjian;
//using UnpakCbt.Modules.JadwalUjian.PublicApi;
using UnpakCbt.Modules.JadwalUjian.Infrastructure.Database;
using UnpakCbt.Modules.JadwalUjian.Presentation.JadwalUjian;
using UnpakCbt.Modules.JadwalUjian.Infrastructure.JadwalUjian;
using UnpakCbt.Modules.JadwalUjian.Application.Abstractions.Data;

namespace UnpakCbt.Modules.JadwalUjian.Infrastructure
{
    public static class JadwalUjianModule
    {
        public static void MapEndpoints(IEndpointRouteBuilder app)
        {
            JadwalUjianEndpoints.MapEndpoints(app);
        }

        public static IServiceCollection AddJadwalUjianModule(
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

            services.AddDbContext<JadwalUjianDbContext>(optionsBuilder => optionsBuilder.UseMySQL(databaseConnectionString));

            services.AddScoped<IJadwalUjianRepository, JadwalUjianRepository>();

            services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<JadwalUjianDbContext>());

            //services.AddScoped<IJadwalUjianApi, JadwalUjianApi>();
        }
    }
}
