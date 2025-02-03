using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using UnpakCbt.Common.Application.Data;
using UnpakCbt.Common.Infrastructure.Data;
using UnpakCbt.Modules.TemplateJawaban.Application.Abstractions.Data;
using UnpakCbt.Modules.TemplateJawaban.Domain.TemplateJawaban;
using UnpakCbt.Modules.TemplateJawaban.Infrastructure.Database;
using UnpakCbt.Modules.TemplateJawaban.Infrastructure.PublicApi;
using UnpakCbt.Modules.TemplateJawaban.Infrastructure.TemplateJawaban;
using UnpakCbt.Modules.TemplateJawaban.Presentation.TemplateJawaban;
using UnpakCbt.Modules.TemplateJawaban.PublicApi;

namespace UnpakCbt.Modules.TemplateJawaban.Infrastructure
{
    public static class TemplateJawabanModule
    {
        public static void MapEndpoints(IEndpointRouteBuilder app)
        {
            TemplateJawabanEndpoints.MapEndpoints(app);
        }

        public static IServiceCollection AddTemplateJawabanModule(
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

            services.AddDbContext<TemplateJawabanDbContext>(optionsBuilder => optionsBuilder.UseMySQL(databaseConnectionString));

            services.AddScoped<ITemplateJawabanRepository, TemplateJawabanRepository>();

            services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<TemplateJawabanDbContext>());

            services.AddScoped<ITemplateJawabanApi, TemplateJawabanApi>();
        }
    }
}
