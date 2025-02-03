using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        }
    }
}
