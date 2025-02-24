using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using UnpakCbt.Common.Domain;
using UnpakCbt.Common.Presentation.ApiResults;
using UnpakCbt.Common.Presentation.FileManager;
using UnpakCbt.Common.Presentation.Security;
using UnpakCbt.Modules.TemplateJawaban.Application.TemplateJawaban.CreateTemplateJawaban;
using static UnpakCbt.Modules.TemplateJawaban.Presentation.TemplateJawaban.UpdateTemplateJawaban;

namespace UnpakCbt.Modules.TemplateJawaban.Presentation.TemplateJawaban
{
    internal static class CreateTemplateJawaban
    {
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("TemplateJawaban", [IgnoreAntiforgeryToken(Order = 1001)] async ([FromForm] CreateTemplateJawabanRequest request, ISender sender, IFileProvider fileProvider) =>
            {
                if (!SecurityCheck.NotContainInvalidCharacters(request.IdTemplateSoal))
                {
                    return Results.BadRequest(ApiResults.Problem(Result.Failure(Error.Problem("Request.Invalid", "IdTemplateSoal mengandung karakter berbahaya"))));
                }
                if (!SecurityCheck.isValidGuid(request.IdTemplateSoal))
                {
                    return Results.BadRequest(ApiResults.Problem(Result.Failure(Error.Problem("Request.Invalid", "IdTemplateSoal harus Guid format"))));
                }

                string? jawabanImgPath = null;
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads/jawaban_img");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                if (request.JawabanImg != null && request.JawabanImg.Length > 0)
                {
                    string safeFileName = fileProvider.GenerateFileName(request.JawabanImg);
                    string extension = fileProvider.GetSafeExtension(request.JawabanImg);

                    var filePath = Path.Combine(uploadsFolder, safeFileName);

                    // Optional file size and extension validation
                    if (request.JawabanImg.Length > 5 * 1024 * 1024) // 5 MB limit
                    {
                        return Results.BadRequest("File size is too large.");
                    }

                    var allowedExtensions = new[] { "png", "jpg", "jpeg" };
                    if (!allowedExtensions.Contains(extension.ToLower()))
                    {
                        return Results.BadRequest("Invalid file type.");
                    }

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await request.JawabanImg.CopyToAsync(stream);
                    }

                    jawabanImgPath = "jawaban_img/" + safeFileName; // Relative path
                }

                Result<Guid> result = await sender.Send(new CreateTemplateJawabanCommand(
                    Guid.Parse(request.IdTemplateSoal),
                    request.JawabanText,
                    jawabanImgPath
                    )
                );

                return result.Match(Results.Ok, ApiResults.Problem);
            }).WithTags(Tags.TemplateJawaban)
              .Accepts<UpdateTemplateJawabanRequest>("multipart/form-data")
              .DisableAntiforgery();
        }

        internal sealed class CreateTemplateJawabanRequest
        {
            [FromForm] public string IdTemplateSoal { get; set; }
            [FromForm] public string? JawabanText { get; set; }
            [FromForm] public IFormFile? JawabanImg { get; set; }
        }
    }
}
