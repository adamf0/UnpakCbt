using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using UnpakCbt.Common.Domain;
using UnpakCbt.Common.Presentation.ApiResults;
using UnpakCbt.Modules.BankSoal.Application.BankSoal.StatusBankSoal;

namespace UnpakCbt.Modules.BankSoal.Presentation.BankSoal
{
    internal static class StatusBankSoal
    {
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("BankSoal/status", async (StatusBankSoalRequest request, ISender sender) =>
            {
                Result result = await sender.Send(new StatusBankSoalCommand(
                    request.Id,
                    request.Status
                    )
                );

                return result.Match(() => Results.Ok(), ApiResults.Problem);
            }).WithTags(Tags.BankSoal);
        }

        internal sealed class StatusBankSoalRequest
        {
            public Guid Id { get; set; }
            public string Status { get; set; }
        }
    }
}
