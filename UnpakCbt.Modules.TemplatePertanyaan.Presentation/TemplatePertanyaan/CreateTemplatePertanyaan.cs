using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Text.Encodings.Web;
using UnpakCbt.Common.Domain;
using UnpakCbt.Common.Presentation.ApiResults;
using UnpakCbt.Common.Presentation.FileManager;
using UnpakCbt.Modules.TemplatePertanyaan.Application.TemplatePertanyaan.CreateTemplatePertanyaan;
using UnpakCbt.Modules.TemplatePertanyaan.Presentation;

namespace UnpakCbt.Modules.TemplatePertanyaan.Presentation.TemplatePertanyaan
{
    internal static class CreateTemplatePertanyaan
    {
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("TemplatePertanyaan", [IgnoreAntiforgeryToken(Order = 1001)] async ([FromForm] CreateTemplatePertanyaanRequest request, ISender sender, IFileProvider fileProvider) =>
            {
                string? jawabanImgPath = null;
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads/pertanyaan_img");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                if (request.Gambar != null && request.Gambar.Length > 0)
                {
                    string safeFileName = fileProvider.GenerateFileName(request.Gambar);
                    string extension = fileProvider.GetSafeExtension(request.Gambar);

                    var filePath = Path.Combine(uploadsFolder, safeFileName);

                    // Optional file size and extension validation
                    if (request.Gambar.Length > 5 * 1024 * 1024) // 5 MB limit
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
                        await request.Gambar.CopyToAsync(stream);
                    }

                    jawabanImgPath = "pertanyaan_img/" + safeFileName; // Relative path
                }

                Result<Guid> result = await sender.Send(new CreateTemplatePertanyaanCommand(
                    request.IdBankSoal,
                    request.Tipe,
                    request.Pertanyaan,
                    jawabanImgPath,
                    null,
                    null,
                    "init"
                    )
                );

                return result.Match(Results.Ok, ApiResults.Problem);

            }).WithTags(Tags.TemplatePertanyaan)
              .Accepts<CreateTemplatePertanyaanRequest>("multipart/form-data")
              .DisableAntiforgery();
        }

        internal sealed class CreateTemplatePertanyaanRequest
        {
            [FromForm] public Guid IdBankSoal { get; set; }
            [FromForm] public string Tipe { get; set; }
            [FromForm] public string? Pertanyaan { get; set; } = null;
            [FromForm] public IFormFile? Gambar { get; set; } = null;
        }
    }
}
