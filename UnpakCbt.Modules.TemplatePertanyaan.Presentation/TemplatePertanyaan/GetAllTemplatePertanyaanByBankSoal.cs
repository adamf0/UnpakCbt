using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using UnpakCbt.Common.Domain;
using UnpakCbt.Common.Presentation.ApiResults;
using UnpakCbt.Modules.TemplatePertanyaan.Application.TemplatePertanyaan.GetAllTemplatePertanyaan;
using UnpakCbt.Modules.TemplatePertanyaan.Application.TemplatePertanyaan.GetTemplatePertanyaan;

namespace UnpakCbt.Modules.TemplatePertanyaan.Presentation.TemplatePertanyaan
{
    internal class GetAllTemplatePertanyaanByBankSoal
    {
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("TemplatePertanyaan/BankSoal/{uuidBankSoal}", async (Guid uuidBankSoal, ISender sender) =>
            {
                Result<List<TemplatePertanyaanResponse>> result = await sender.Send(new GetAllTemplatePertanyaanByBankSoalQuery(uuidBankSoal));

                return result.Match(Results.Ok, ApiResults.Problem);
            }).WithTags(Tags.TemplatePertanyaan);
        }
    }
}
