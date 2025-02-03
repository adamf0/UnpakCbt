using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Text.Encodings.Web;
using UnpakCbt.Common.Domain;
using UnpakCbt.Common.Presentation.ApiResults;
using UnpakCbt.Modules.BankSoal.Application.BankSoal.CreateBankSoal;

namespace UnpakCbt.Modules.BankSoal.Presentation.BankSoal
{
    internal static class CreateBankSoal
    {
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("BankSoal", async (CreateBankSoalRequest request, ISender sender) =>
            {
                Result<Guid> result = await sender.Send(new CreateBankSoalCommand(
                    request.Judul,
                    request.Rule
                    )
                );

                return result.Match(Results.Ok, ApiResults.Problem);
            }).WithTags(Tags.BankSoal);
        }

        internal sealed class CreateBankSoalRequest
        {
            public string Judul { get; set; }

            public string Rule { get; set; }
        }
    }
}
