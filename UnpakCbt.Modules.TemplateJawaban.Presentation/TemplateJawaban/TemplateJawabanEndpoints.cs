using Microsoft.AspNetCore.Routing;

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
            GetAllTemplateJawabanByBankSoal.MapEndpoint(app);
            GetAllTemplateJawabanByBankSoalV2.MapEndpoint(app);
            GetAllTemplateJawabanWithPaging.MapEndpoint(app);
        }
    }
}
