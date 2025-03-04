using Microsoft.AspNetCore.Routing;
using UnpakCbt.Modules.Laporan.Presentation.Laporan;

namespace UnpakCbt.Modules.Laporan.Presentation.JadwalUjian
{
    public static class LaporanEndpoints
    {
        public static void MapEndpoints(IEndpointRouteBuilder app)
        {
            GetAllLaporanLulus.MapEndpoint(app);
        }
    }
}
