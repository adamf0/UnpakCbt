using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using UnpakCbt.Common.Domain;
using UnpakCbt.Common.Presentation.ApiResults;
using UnpakCbt.Common.Presentation.Security;
using UnpakCbt.Modules.Account.Application.Account.StatusAccount;

namespace UnpakCbt.Modules.Account.Presentation.Account
{
    internal static class StatusAccount
    {
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("Account/status", async (StatusAccountRequest request, ISender sender, HttpContext context, TokenValidator tokenValidator) =>
            {
                var (isValid, error) = tokenValidator.ValidateToken(context);
                if (!isValid)
                {
                    return error;
                }

                if (!SecurityCheck.NotContainInvalidCharacters(request.Id))
                {
                    return Results.BadRequest(ApiResults.Problem(Result.Failure(Error.Problem("Request.Invalid", "Id mengandung karakter berbahaya"))));
                }
                if (!SecurityCheck.isValidGuid(request.Id))
                {
                    return Results.BadRequest(ApiResults.Problem(Result.Failure(Error.Problem("Request.Invalid", "Id harus Guid format"))));
                }

                Result result = await sender.Send(new StatusAccountCommand(
                    Guid.Parse(request.Id),
                    request.Status
                    )
                );

                return result.Match(() => Results.Ok(), ApiResults.Problem);
            }).WithTags(Tags.Account).RequireAuthorization();
        }

        internal sealed class StatusAccountRequest
        {
            public string Id { get; set; }
            public string Status { get; set; }
        }
    }
}
