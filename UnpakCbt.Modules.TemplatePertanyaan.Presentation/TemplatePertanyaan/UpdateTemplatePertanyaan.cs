﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using UnpakCbt.Common.Domain;
using UnpakCbt.Common.Presentation.ApiResults;
using UnpakCbt.Common.Presentation.FileManager;
using UnpakCbt.Common.Presentation.Security;
using UnpakCbt.Modules.TemplatePertanyaan.Application.TemplatePertanyaan.UpdateTemplatePertanyaan;
using static UnpakCbt.Modules.TemplatePertanyaan.Presentation.TemplatePertanyaan.CreateTemplatePertanyaan;

namespace UnpakCbt.Modules.TemplatePertanyaan.Presentation.TemplatePertanyaan
{
    internal static class UpdateTemplatePertanyaan
    {
        [Authorize]
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("TemplatePertanyaan", [IgnoreAntiforgeryToken(Order = 1001)] async ([FromForm] UpdateTemplatePertanyaanRequest request, ISender sender, IFileProvider fileProvider, HttpContext context, TokenValidator tokenValidator) =>
            {
                var (isValid, error) = tokenValidator.ValidateToken(context);
                if (!isValid)
                {
                    return error;
                }

                if (!SecurityCheck.NotContainInvalidCharacters(request.Id))
                {
                    return ApiResults.Problem(Result.Failure(Error.Problem("Request.Invalid", "Id mengandung karakter berbahaya")));
                }
                if (!SecurityCheck.NotContainInvalidCharacters(request.IdBankSoal))
                {
                    return ApiResults.Problem(Result.Failure(Error.Problem("Request.Invalid", "IdBankSoal mengandung karakter berbahaya")));
                }
                if (!SecurityCheck.NotContainInvalidCharacters(request.Jawaban))
                {
                    return ApiResults.Problem(Result.Failure(Error.Problem("Request.Invalid", "Jawaban mengandung karakter berbahaya")));
                }

                if (!SecurityCheck.isValidGuid(request.Id))
                {
                    return ApiResults.Problem(Result.Failure(Error.Problem("Request.Invalid", "Id harus Guid format")));
                }
                if (!SecurityCheck.isValidGuid(request.IdBankSoal))
                {
                    return ApiResults.Problem(Result.Failure(Error.Problem("Request.Invalid", "IdBankSoal harus Guid format")));
                }
                if (!SecurityCheck.isValidGuid(request.Jawaban))
                {
                    return ApiResults.Problem(Result.Failure(Error.Problem("Request.Invalid", "Jawaban harus Guid format")));
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
                        return ApiResults.Problem(Result.Failure(Error.Problem("Request.Invalid", "File size is too large")));
                    }

                    var allowedExtensions = new[] { "png", "jpg", "jpeg" };
                    if (!allowedExtensions.Contains(extension.ToLower()))
                    {
                        return ApiResults.Problem(Result.Failure(Error.Problem("Request.Invalid", "Invalid file type")));
                    }
                    if (!fileProvider.IsSafeMimeType(request.Gambar))
                    {
                        return ApiResults.Problem(Result.Failure(Error.Problem("Request.Invalid", "Invalid mime type")));
                    }
                    if (!fileProvider.IsValidMimeTypeAllowedExtension(request.Gambar.ContentType, extension.ToLower()))
                    {
                        return ApiResults.Problem(Result.Failure(Error.Problem("Request.Invalid", "Invalid allowed extension in mime type")));
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
                    Sanitizer.Sanitize(request.Tipe),
                    request.Pertanyaan, //[PR][Skipped] review kena xss tidak
                    jawabanImgPath,
                    Guid.Parse(request.Jawaban),
                    int.Parse(request.Bobot),
                    Sanitizer.Sanitize(request?.State ?? "")
                    )
                );

                return result.Match(() => Results.Ok(), ApiResults.Problem);
            }).WithTags(Tags.TemplatePertanyaan)
              .Accepts<CreateTemplatePertanyaanRequest>("multipart/form-data")
              .DisableAntiforgery()
              .RequireAuthorization();
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
