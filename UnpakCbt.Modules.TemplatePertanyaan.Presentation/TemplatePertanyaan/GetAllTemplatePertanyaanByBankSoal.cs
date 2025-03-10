using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using UnpakCbt.Common.Domain;
using UnpakCbt.Common.Presentation.ApiResults;
using UnpakCbt.Common.Presentation.Security;
using UnpakCbt.Modules.TemplatePertanyaan.Application.TemplatePertanyaan.GetAllTemplatePertanyaan;
using UnpakCbt.Modules.TemplatePertanyaan.Application.TemplatePertanyaan.GetTemplatePertanyaan;

namespace UnpakCbt.Modules.TemplatePertanyaan.Presentation.TemplatePertanyaan
{
    internal class GetAllTemplatePertanyaanByBankSoal
    {
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("TemplatePertanyaan/BankSoal/{uuidBankSoal}", async (string IdBankSoal, ISender sender) => //HttpContext context, TokenValidator tokenValidator
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

                Result<List<TemplatePertanyaanResponse>> result = await sender.Send(new GetAllTemplatePertanyaanByBankSoalQuery(Guid.Parse(IdBankSoal)));

                return result.Match(Results.Ok, ApiResults.Problem);
            }).WithTags(Tags.TemplatePertanyaan);
        }
    }
}
