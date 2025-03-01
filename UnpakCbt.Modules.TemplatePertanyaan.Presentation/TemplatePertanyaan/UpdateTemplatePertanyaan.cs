using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Runtime.InteropServices;
using UnpakCbt.Common.Domain;
using UnpakCbt.Common.Presentation.ApiResults;
using UnpakCbt.Common.Presentation.FileManager;
using UnpakCbt.Common.Presentation.Security;
using UnpakCbt.Modules.BankSoal.Domain.BankSoal;
using UnpakCbt.Modules.TemplatePertanyaan.Application.TemplatePertanyaan.UpdateTemplatePertanyaan;
using static UnpakCbt.Modules.TemplatePertanyaan.Presentation.TemplatePertanyaan.CreateTemplatePertanyaan;

namespace UnpakCbt.Modules.TemplatePertanyaan.Presentation.TemplatePertanyaan
{
    internal static class UpdateTemplatePertanyaan
    {
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("TemplatePertanyaan", [IgnoreAntiforgeryToken(Order = 1001)] async ([FromForm] UpdateTemplatePertanyaanRequest request, ISender sender, IFileProvider fileProvider) =>
            {
                if (!SecurityCheck.NotContainInvalidCharacters(request.Id))
                {
                    return Results.BadRequest(ApiResults.Problem(Result.Failure(Error.Problem("Request.Invalid", "Id mengandung karakter berbahaya"))));
                }
                if (!SecurityCheck.NotContainInvalidCharacters(request.IdBankSoal))
                {
                    return Results.BadRequest(ApiResults.Problem(Result.Failure(Error.Problem("Request.Invalid", "IdBankSoal mengandung karakter berbahaya"))));
                }
                if (!SecurityCheck.NotContainInvalidCharacters(request.Jawaban))
                {
                    return Results.BadRequest(ApiResults.Problem(Result.Failure(Error.Problem("Request.Invalid", "Jawaban mengandung karakter berbahaya"))));
                }

                if (!SecurityCheck.isValidGuid(request.Id))
                {
                    return Results.BadRequest(ApiResults.Problem(Result.Failure(Error.Problem("Request.Invalid", "Id harus Guid format"))));
                }
                if (!SecurityCheck.isValidGuid(request.IdBankSoal))
                {
                    return Results.BadRequest(ApiResults.Problem(Result.Failure(Error.Problem("Request.Invalid", "IdBankSoal harus Guid format"))));
                }
                if (!SecurityCheck.isValidGuid(request.Jawaban))
                {
                    return Results.BadRequest(ApiResults.Problem(Result.Failure(Error.Problem("Request.Invalid", "Jawaban harus Guid format"))));
                }

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
                    /*if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                    {
                        File.SetUnixFileMode(filePath, UnixFileMode.UserRead | UnixFileMode.UserWrite);
                    }*/

                    using (var stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
                    {
                        await request.Gambar.CopyToAsync(stream);
                        stream.Close();
                        File.SetAttributes(filePath, FileAttributes.Normal);
                        File.SetAttributes(filePath, (FileAttributes)Convert.ToInt32("600", 8));
                    }

                    jawabanImgPath = "pertanyaan_img/" + safeFileName; // Relative path
                }

                Result result = await sender.Send(new UpdateTemplatePertanyaanCommand(
                    Guid.Parse(request.Id),
                    Guid.Parse(request.IdBankSoal),
                    request.Tipe,
                    request.Pertanyaan,
                    jawabanImgPath,
                    Guid.Parse(request.Jawaban),
                    int.Parse(request.Bobot),
                    request.State
                    )
                );

                return result.Match(() => Results.Ok(), ApiResults.Problem);
            }).WithTags(Tags.TemplatePertanyaan)
              .Accepts<CreateTemplatePertanyaanRequest>("multipart/form-data")
              .DisableAntiforgery();
        }

        internal sealed class UpdateTemplatePertanyaanRequest
        {
            [FromForm] public string Id { get; set; }
            [FromForm] public string IdBankSoal { get; set; }
            [FromForm] public string Tipe { get; set; }
            [FromForm] public string? Pertanyaan { get; set; } = null;
            [FromForm] public IFormFile? Gambar { get; set; } = null;
            [FromForm] public string Jawaban { get; set; }
            [FromForm] public string Bobot { get; set; }
            [FromForm] public string? State { get; set; } = null;
        }
    }
}
