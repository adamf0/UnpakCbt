using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            CancelUjian.MapEndpoint(app);
            GetUjian.MapEndpoint(app);
            GetAllUjian.MapEndpoint(app);
        }
    }
}
