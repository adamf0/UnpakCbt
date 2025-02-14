using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using UnpakCbt.Common.Domain;
using UnpakCbt.Common.Presentation.ApiResults;
using UnpakCbt.Modules.BankSoal.Application.BankSoal.UpdateBankSoal;

namespace UnpakCbt.Modules.BankSoal.Presentation.BankSoal
{
    internal static class UpdateBankSoal
    {
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("BankSoal", async (UpdateBankSoalRequest request, ISender sender) =>
            {
                Result result = await sender.Send(new UpdateBankSoalCommand(
                    request.Id,
                    request.Judul,
                    request.Rule
                    )
                );

                return result.Match(() => Results.Ok(), ApiResults.Problem);
            }).WithTags(Tags.BankSoal);
        }

        internal sealed class UpdateBankSoalRequest
        {
            public Guid Id { get; set; }
            public string Judul { get; set; }

            public string Rule { get; set; }
        }
    }
}
