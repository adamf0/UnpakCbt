using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using UnpakCbt.Common.Domain;
using UnpakCbt.Common.Presentation.ApiResults;
using UnpakCbt.Modules.Account.Application.Account.GetAllAccount;
using UnpakCbt.Modules.Account.Application.Account.GetAccount;
using Microsoft.AspNetCore.Authorization;

namespace UnpakCbt.Modules.Account.Presentation.Account
{
    internal class GetAllAccount
    {
        [Authorize]
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("Account", async (ISender sender) =>
            {
                Result<List<AccountResponse>> result = await sender.Send(new GetAllAccountQuery());

                return result.Match(Results.Ok, ApiResults.Problem);
            }).WithTags(Tags.Account).RequireAuthorization();
        }
    }
}
