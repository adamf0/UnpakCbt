using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using UnpakCbt.Common.Domain;
using UnpakCbt.Common.Presentation.ApiResults;
using UnpakCbt.Common.Presentation.Security;
using UnpakCbt.Modules.TemplatePertanyaan.Application.TemplatePertanyaan.GetTemplatePertanyaan;

namespace UnpakCbt.Modules.TemplatePertanyaan.Presentation.TemplatePertanyaan
{
    internal static class GetTemplatePertanyaan
    {
        [Authorize]
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("TemplatePertanyaan/{id}", async (string id, ISender sender, HttpContext context, TokenValidator tokenValidator) =>
            {
                var (isValid, error) = tokenValidator.ValidateToken(context);
                if (!isValid)
                {
                    return error;
                }

                if (!SecurityCheck.NotContainInvalidCharacters(id))
                {
                    return ApiResults.Problem(Result.Failure(Error.Problem("Request.Invalid", "IdBankSoal mengandung karakter berbahaya")));
                }
                if (!SecurityCheck.isValidGuid(id))
                {
                    return ApiResults.Problem(Result.Failure(Error.Problem("Request.Invalid", "Id harus Guid format")));
                }

                Result<TemplatePertanyaanResponse> result = await sender.Send(new GetTemplatePertanyaanQuery(Guid.Parse(id)));

                return result.Match(Results.Ok, ApiResults.Problem);
            }).WithTags(Tags.TemplatePertanyaan).RequireAuthorization();
        }
    }
}
