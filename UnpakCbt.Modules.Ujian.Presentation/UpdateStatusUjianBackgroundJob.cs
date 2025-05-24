using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using UnpakCbt.Modules.Ujian.Application.Ujian.AllDoneUjian;

namespace UnpakCbt.Modules.Ujian.Presentation
{
    public class UpdateStatusUjianBackgroundJob(
        ILogger<UpdateStatusUjianBackgroundJob> logger,
        IServiceScopeFactory _scopeFactory
    ) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("Background job started at: {time}", DateTimeOffset.Now);
            using var scope = _scopeFactory.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await mediator.Send(new AllDoneUjianCommand(), stoppingToken);
                }
                catch (Exception ex)
                { 
                    logger.LogError(ex, "Error executing AllDoneUjianCommand");
                    throw ex;
                }

                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }
    }
}
