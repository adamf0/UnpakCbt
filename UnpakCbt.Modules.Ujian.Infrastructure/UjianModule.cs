using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using UnpakCbt.Common.Application.Data;
using UnpakCbt.Common.Infrastructure.Data;
using UnpakCbt.Modules.Ujian.Domain.Ujian;
//using UnpakCbt.Modules.Ujian.PublicApi;
using UnpakCbt.Modules.Ujian.Presentation.Ujian;
using UnpakCbt.Modules.Ujian.Application.Abstractions.Data;
using UnpakCbt.Modules.Ujian.Infrastructure.Ujian;
using UnpakCbt.Modules.Ujian.Infrastructure.Database;

namespace UnpakCbt.Modules.Ujian.Infrastructure
{
    public static class UjianModule
    {
        public static void MapEndpoints(IEndpointRouteBuilder app)
        {
            UjianEndpoints.MapEndpoints(app);
        }

        public static IServiceCollection AddUjianModule(
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

            services.AddDbContext<UjianDbContext>(optionsBuilder => optionsBuilder.UseMySQL(databaseConnectionString));

            services.AddScoped<IUjianRepository, UjianRepository>();
            services.AddScoped<ICounterRepository, CounterRepository>();

            services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<UjianDbContext>());
        }
    }
}
