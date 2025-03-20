using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Text.RegularExpressions;
using UnpakCbt.Common.Domain;
using UnpakCbt.Common.Presentation.ApiResults;
using UnpakCbt.Common.Presentation.Security;
using UnpakCbt.Modules.BankSoal.Application.BankSoal.StatusBankSoal;

namespace UnpakCbt.Modules.BankSoal.Presentation.BankSoal
{
    internal static class StatusBankSoal
    {
        [Authorize]
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("BankSoal/status", async (StatusBankSoalRequest request, ISender sender, HttpContext context, TokenValidator tokenValidator) =>
            {
                var (isValid, error) = tokenValidator.ValidateToken(context);
                if (!isValid)
                {
                    return error;
                }

                if (!SecurityCheck.NotContainInvalidCharacters(request.Id))
                {
                    return ApiResults.Problem(Result.Failure(Error.Problem("Request.Invalid", "Id mengandung karakter berbahaya")));
                }
                if (!SecurityCheck.isValidGuid(request.Id))
                {
                    return ApiResults.Problem(Result.Failure(Error.Problem("Request.Invalid", "Id harus Guid format")));
                }

                Result result = await sender.Send(new StatusBankSoalCommand(
                    Guid.Parse(request.Id),
                    Sanitizer.Sanitize(request.Status)
                    )
                );

                return result.Match(() => Results.Ok(), ApiResults.Problem);
            }).WithTags(Tags.BankSoal).RequireAuthorization();
        }

        internal sealed class StatusBankSoalRequest
        {
            public string Id { get; set; }
            public string Status { get; set; }
        }
    }
}
