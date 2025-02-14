using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using UnpakCbt.Common.Domain;
using UnpakCbt.Common.Presentation.ApiResults;
using UnpakCbt.Modules.BankSoal.Application.BankSoal.GetBankSoal;

namespace UnpakCbt.Modules.BankSoal.Presentation.BankSoal
{
    internal static class GetBankSoal
    {
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("BankSoal/{id}", async (Guid id, ISender sender) =>
            {
                Result<BankSoalResponse> result = await sender.Send(new GetBankSoalQuery(id));

                return result.Match(Results.Ok, ApiResults.Problem);
            }).WithTags(Tags.BankSoal);
        }
    }
}
