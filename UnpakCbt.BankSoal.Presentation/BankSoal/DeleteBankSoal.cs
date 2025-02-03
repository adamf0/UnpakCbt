using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Text.Encodings.Web;
using UnpakCbt.Common.Domain;
using UnpakCbt.Common.Presentation.ApiResults;
using UnpakCbt.Modules.BankSoal.Application.BankSoal.DeleteBankSoal;

namespace UnpakCbt.Modules.BankSoal.Presentation.BankSoal
{
    internal class DeleteBankSoal
    {
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("BankSoal/{id}", async (Guid id, ISender sender) =>
            {
                Result result = await sender.Send(
                    new DeleteBankSoalCommand(id)
                );

                return result.Match(() => Results.Ok(), ApiResults.Problem);
            }).WithTags(Tags.BankSoal);
        }
    }
}
