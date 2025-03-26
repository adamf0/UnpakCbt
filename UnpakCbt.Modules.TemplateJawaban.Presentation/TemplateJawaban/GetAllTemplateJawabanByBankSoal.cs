using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using UnpakCbt.Common.Application.Security;
using UnpakCbt.Common.Domain;
using UnpakCbt.Common.Presentation.ApiResults;
using UnpakCbt.Common.Presentation.Security;
using UnpakCbt.Modules.TemplateJawaban.Application.TemplateJawaban.GetAllTemplateJawaban;
using UnpakCbt.Modules.TemplateJawaban.Application.TemplateJawaban.GetTemplateJawaban;
using static UnpakCbt.Common.Application.Security.Xss;

namespace UnpakCbt.Modules.TemplateJawaban.Presentation.TemplateJawaban
{
    internal class GetAllTemplateJawabanByBankSoal
    {
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("TemplateJawaban/BankSoal/{uuidBankSoal}", async (string uuidBankSoal, string? type, ISender sender) => //HttpContext context, TokenValidator tokenValidator
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
                if (Check(type ?? "") != SanitizerType.CLEAR)
                {
                    return ApiResults.Problem(Result.Failure(Error.Problem("Request.Invalid", "Type mengandung xss")));
                }

                Result<List<TemplateJawabanResponse>> result = await sender.Send(new GetAllTemplateJawabanByBankSoalQuery(
                    Guid.Parse(uuidBankSoal),
                    Sanitize(type ?? "")
                ));

                return result.Match(Results.Ok, ApiResults.Problem);
            }).WithTags(Tags.TemplateJawaban);
        }
    }
}
