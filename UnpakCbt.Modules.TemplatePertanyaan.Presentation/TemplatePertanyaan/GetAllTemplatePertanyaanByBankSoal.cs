using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using UnpakCbt.Common.Application.Security;
using UnpakCbt.Common.Domain;
using UnpakCbt.Common.Presentation.ApiResults;
using UnpakCbt.Common.Presentation.Security;
using UnpakCbt.Modules.TemplatePertanyaan.Application.TemplatePertanyaan.GetAllTemplatePertanyaan;
using UnpakCbt.Modules.TemplatePertanyaan.Application.TemplatePertanyaan.GetTemplatePertanyaan;
using static UnpakCbt.Common.Application.Security.Xss;

namespace UnpakCbt.Modules.TemplatePertanyaan.Presentation.TemplatePertanyaan
{
    internal class GetAllTemplatePertanyaanByBankSoal
    {
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("TemplatePertanyaan/BankSoal/{uuidBankSoal}", async (string uuidBankSoal, string? type, ISender sender) => //HttpContext context, TokenValidator tokenValidator
            {
                /*var (isValid, error) = tokenValidator.ValidateToken(context);
                if (!isValid)
                {
                    return error;
                }*/

                if (!SecurityCheck.NotContainInvalidCharacters(uuidBankSoal))
                {
                    return ApiResults.Problem(Result.Failure(Error.Problem("Request.Invalid", "uuidBankSoal mengandung karakter berbahaya")));
                }
                if (!SecurityCheck.isValidGuid(uuidBankSoal))
                {
                    return ApiResults.Problem(Result.Failure(Error.Problem("Request.Invalid", "uuidBankSoal harus Guid format")));
                }
                if (Check(type??"") != SanitizerType.CLEAR) {
                    return ApiResults.Problem(Result.Failure(Error.Problem("Request.Invalid", "Type mengandung xss")));
                }

                Result<List<TemplatePertanyaanResponse>> result = await sender.Send(new GetAllTemplatePertanyaanByBankSoalQuery(
                    Guid.Parse(uuidBankSoal),
                    Sanitize(type ?? "")
                ));

                return result.Match(Results.Ok, ApiResults.Problem);
            }).WithTags(Tags.TemplatePertanyaan);
        }
    }
}
