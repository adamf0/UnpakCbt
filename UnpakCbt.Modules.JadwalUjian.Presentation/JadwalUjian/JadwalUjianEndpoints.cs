using Microsoft.AspNetCore.Routing;

namespace UnpakCbt.Modules.JadwalUjian.Presentation.JadwalUjian
{
    public static class JadwalUjianEndpoints
    {
        public static void MapEndpoints(IEndpointRouteBuilder app)
        {
            CreateJadwalUjian.MapEndpoint(app);
            UpdateJadwalUjian.MapEndpoint(app);
            DeleteJadwalUjian.MapEndpoint(app);
            GetJadwalUjian.MapEndpoint(app);
            GetAllJadwalUjian.MapEndpoint(app);
            GetActiveJadwalUjian.MapEndpoint(app);
        }
    }
}
