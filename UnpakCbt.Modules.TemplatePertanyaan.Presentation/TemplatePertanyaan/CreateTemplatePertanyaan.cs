using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using UnpakCbt.Common.Domain;
using UnpakCbt.Common.Presentation.ApiResults;
using UnpakCbt.Common.Presentation.FileManager;
using UnpakCbt.Common.Presentation.Security;
using UnpakCbt.Modules.TemplatePertanyaan.Application.TemplatePertanyaan.CreateTemplatePertanyaan;

namespace UnpakCbt.Modules.TemplatePertanyaan.Presentation.TemplatePertanyaan
{
    internal static class CreateTemplatePertanyaan
    {
        [Authorize]
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("TemplatePertanyaan", [IgnoreAntiforgeryToken(Order = 1001)] async ([FromForm] CreateTemplatePertanyaanRequest request, ISender sender, IFileProvider fileProvider, HttpContext context, TokenValidator tokenValidator) =>
            {
                var (isValid, error) = tokenValidator.ValidateToken(context);
                if (!isValid)
                {
                    return error;
                }

                if (!SecurityCheck.NotContainInvalidCharacters(request.IdBankSoal))
                {
                    return ApiResults.Problem(Result.Failure(Error.Problem("Request.Invalid", "IdBankSoal mengandung karakter berbahaya")));
                }
                if (!SecurityCheck.isValidGuid(request.IdBankSoal))
                {
                    return ApiResults.Problem(Result.Failure(Error.Problem("Request.Invalid", "IdBankSoal harus Guid format")));
                }

                Result<Guid> result = await sender.Send(new CreateTemplatePertanyaanCommand(
                    Guid.Parse(request.IdBankSoal),
                    request.Tipe,
                    null, //request.Pertanyaan,
                    null, //jawabanImgPath,
                    null,
                    null,
                    "init"
                    )
                );

                return result.Match(Results.Ok, ApiResults.Problem);

            }).WithTags(Tags.TemplatePertanyaan)
              .Accepts<CreateTemplatePertanyaanRequest>("multipart/form-data")
              .DisableAntiforgery()
              .RequireAuthorization();
        }

        internal sealed class CreateTemplatePertanyaanRequest
        {
            [FromForm] public string IdBankSoal { get; set; }
            [FromForm] public string Tipe { get; set; }
            /*[FromForm] public string? Pertanyaan { get; set; } = null;
            [FromForm] public IFormFile? Gambar { get; set; } = null;*/
        }
    }
}
