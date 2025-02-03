using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnpakCbt.Modules.TemplateJawaban.Presentation.TemplateJawaban
{
    public static class TemplateJawabanEndpoints
    {
        public static void MapEndpoints(IEndpointRouteBuilder app)
        {
            CreateTemplateJawaban.MapEndpoint(app);
            UpdateTemplateJawaban.MapEndpoint(app);
            DeleteTemplateJawaban.MapEndpoint(app);
            GetTemplateJawaban.MapEndpoint(app);
            GetAllTemplateJawaban.MapEndpoint(app);
        }
    }
}
