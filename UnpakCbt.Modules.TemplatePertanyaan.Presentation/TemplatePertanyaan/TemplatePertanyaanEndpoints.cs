using Microsoft.AspNetCore.Routing;

namespace UnpakCbt.Modules.TemplatePertanyaan.Presentation.TemplatePertanyaan
{
    public static class TemplatePertanyaanEndpoints
    {
        public static void MapEndpoints(IEndpointRouteBuilder app)
        {
            CreateTemplatePertanyaan.MapEndpoint(app);
            UpdateTemplatePertanyaan.MapEndpoint(app);
            DeleteTemplatePertanyaan.MapEndpoint(app);
            RemoveImageTemplatePertanyaan.MapEndpoint(app);
            GetTemplatePertanyaan.MapEndpoint(app);
            GetAllTemplatePertanyaan.MapEndpoint(app);
            GetAllTemplatePertanyaanWithPaging.MapEndpoint(app);
            GetAllTemplatePertanyaanByBankSoal.MapEndpoint(app);
            GetAllTemplatePertanyaanByBankSoalV2.MapEndpoint(app);
        }
    }
}
