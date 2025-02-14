using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using UnpakCbt.Common.Domain;
using UnpakCbt.Common.Presentation.ApiResults;
using UnpakCbt.Modules.BankSoal.Application.BankSoal.GetAllBankSoal;
using UnpakCbt.Modules.BankSoal.Application.BankSoal.GetBankSoal;

namespace UnpakCbt.Modules.BankSoal.Presentation.BankSoal
{
    internal class GetAllBankSoal
    {
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("BankSoal", async (ISender sender) =>
            {
                Result<List<BankSoalResponse>> result = await sender.Send(new GetAllBankSoalQuery());

                return result.Match(Results.Ok, ApiResults.Problem);
            }).WithTags(Tags.BankSoal);
        }
    }
}
