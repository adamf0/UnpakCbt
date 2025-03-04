using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using UnpakCbt.Common.Domain;
using UnpakCbt.Common.Presentation.ApiResults;
using UnpakCbt.Common.Presentation.Security;
using UnpakCbt.Modules.Account.Application.Account.CreateAccount;

namespace UnpakCbt.Modules.Account.Presentation.Account
{
    internal static class CreateAccount
    {
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("Account", async (CreateAccountRequest request, ISender sender, HttpContext context, TokenValidator tokenValidator) =>
            {
                var (isValid, error) = tokenValidator.ValidateToken(context);
                if (!isValid)
                {
                    return error;
                }

                Result<Guid> result = await sender.Send(new authenticationCommand(
                    request.Username,
                    request.Password,
                    request.Level
                    )
                );

                return result.Match(Results.Ok, ApiResults.Problem);
            }).WithTags(Tags.Account).RequireAuthorization();
        }

        internal sealed class CreateAccountRequest
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public string Level { get; set; }
        }
    }
}
