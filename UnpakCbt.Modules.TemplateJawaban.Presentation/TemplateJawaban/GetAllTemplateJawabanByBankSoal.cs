using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using UnpakCbt.Common.Domain;
using UnpakCbt.Common.Presentation.ApiResults;
using UnpakCbt.Common.Presentation.Security;
using UnpakCbt.Modules.TemplateJawaban.Application.TemplateJawaban.GetAllTemplateJawaban;
using UnpakCbt.Modules.TemplateJawaban.Application.TemplateJawaban.GetTemplateJawaban;

namespace UnpakCbt.Modules.TemplateJawaban.Presentation.TemplateJawaban
{
    internal class GetAllTemplateJawabanByBankSoal
    {
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("TemplateJawaban/BankSoal/{IdBankSoal}", async (string IdBankSoal, ISender sender) => //HttpContext context, TokenValidator tokenValidator
            {
                /*var (isValid, error) = tokenValidator.ValidateToken(context);
                if (!isValid)
                {
                    return error;
                }*/

                if (!SecurityCheck.NotContainInvalidCharacters(IdBankSoal))
                {
                    return ApiResults.Problem(Result.Failure(Error.Problem("Request.Invalid", "IdBankSoal mengandung karakter berbahaya")));
                }
                if (!SecurityCheck.isValidGuid(IdBankSoal))
                {
                    return ApiResults.Problem(Result.Failure(Error.Problem("Request.Invalid", "IdBankSoal harus Guid format")));
                }

                Result<List<TemplateJawabanResponse>> result = await sender.Send(new GetAllTemplateJawabanByBankSoalQuery(Guid.Parse(IdBankSoal)));

                return result.Match(Results.Ok, ApiResults.Problem);
            }).WithTags(Tags.TemplateJawaban);
        }
    }
}
