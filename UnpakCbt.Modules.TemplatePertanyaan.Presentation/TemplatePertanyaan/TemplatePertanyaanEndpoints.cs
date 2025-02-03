using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnpakCbt.Modules.TemplatePertanyaan.Presentation.TemplatePertanyaan
{
    public static class TemplatePertanyaanEndpoints
    {
        public static void MapEndpoints(IEndpointRouteBuilder app)
        {
            CreateTemplatePertanyaan.MapEndpoint(app);
            UpdateTemplatePertanyaan.MapEndpoint(app);
            DeleteTemplatePertanyaan.MapEndpoint(app);
            GetTemplatePertanyaan.MapEndpoint(app);
            GetAllTemplatePertanyaan.MapEndpoint(app);
        }
    }
}
