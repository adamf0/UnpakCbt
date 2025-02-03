using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using UnpakCbt.Common.Application.Data;
using UnpakCbt.Common.Infrastructure.Data;
using UnpakCbt.Modules.TemplatePertanyaan.Application.Abstractions.Data;
using UnpakCbt.Modules.TemplatePertanyaan.Domain.TemplatePertanyaan;
using UnpakCbt.Modules.TemplatePertanyaan.Infrastructure.Database;
using UnpakCbt.Modules.TemplatePertanyaan.Infrastructure.PublicApi;
using UnpakCbt.Modules.TemplatePertanyaan.Infrastructure.TemplatePertanyaan;
using UnpakCbt.Modules.TemplatePertanyaan.Presentation.TemplatePertanyaan;
using UnpakCbt.Modules.TemplatePertanyaan.PublicApi;

namespace UnpakCbt.Modules.TemplatePertanyaan.Infrastructure
{
    public static class TemplatePertanyaanModule
    {
        public static void MapEndpoints(IEndpointRouteBuilder app)
        {
            TemplatePertanyaanEndpoints.MapEndpoints(app);
        }

        public static IServiceCollection AddTemplatePertanyaanModule(
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

            services.AddDbContext<TemplatePertanyaanDbContext>(optionsBuilder => optionsBuilder.UseMySQL(databaseConnectionString));

            services.AddScoped<ITemplatePertanyaanRepository, TemplatePertanyaanRepository>();

            services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<TemplatePertanyaanDbContext>());

            services.AddScoped<ITemplatePertanyaanApi, TemplatePertanyaanApi>();
        }
    }
}
