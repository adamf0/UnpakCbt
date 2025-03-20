using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using UnpakCbt.Modules.Ujian.Application.StreamHub;

namespace UnpakCbt.Modules.Ujian.Presentation.Ujian
{
    public static class UjianEndpoints
    {
        public static void MapEndpoints(IEndpointRouteBuilder app)
        {
            CreateUjian.MapEndpoint(app);
            UpdateUjian.MapEndpoint(app);
            RescheduleUjian.MapEndpoint(app);
            DeleteUjian.MapEndpoint(app);
            StartUjian.MapEndpoint(app);
            DoneUjian.MapEndpoint(app);
            CancelUjian.MapEndpoint(app);
            GetUjian.MapEndpoint(app);
            GetAllUjian.MapEndpoint(app);
            GetAllUjianByJadwalUjian.MapEndpoint(app);

            UpdateUjianCbt.MapEndpoint(app);
            GetAllCbt.MapEndpoint(app);

            app.MapHub<JadwalUjianHub>("/JadwalUjianHub");
        }
    }
}
