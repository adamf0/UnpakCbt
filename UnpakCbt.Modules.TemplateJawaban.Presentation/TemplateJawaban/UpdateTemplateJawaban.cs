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
using UnpakCbt.Modules.TemplateJawaban.Application.TemplateJawaban.UpdateTemplateJawaban;

namespace UnpakCbt.Modules.TemplateJawaban.Presentation.TemplateJawaban
{
    internal static class UpdateTemplateJawaban
    {
        [Authorize]
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("TemplateJawaban", [IgnoreAntiforgeryToken(Order = 1001)] async ([FromForm] UpdateTemplateJawabanRequest request, ISender sender, IFileProvider fileProvider, HttpContext context, TokenValidator tokenValidator) =>
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
                if (!SecurityCheck.NotContainInvalidCharacters(request.IdTemplateSoal))
                {
                    return ApiResults.Problem(Result.Failure(Error.Problem("Request.Invalid", "IdTemplateSoal mengandung karakter berbahaya")));
                }

                if (!SecurityCheck.isValidGuid(request.Id))
                {
                    return ApiResults.Problem(Result.Failure(Error.Problem("Request.Invalid", "Id harus Guid format")));
                }
                if (!SecurityCheck.isValidGuid(request.IdTemplateSoal))
                {
                    return ApiResults.Problem(Result.Failure(Error.Problem("Request.Invalid", "IdTemplateSoal harus Guid format")));
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
                        return ApiResults.Problem(Result.Failure(Error.Problem("Request.Invalid", "File size is too large")));
                    }

                    var allowedExtensions = new[] { "png", "jpg", "jpeg" };
                    if (!allowedExtensions.Contains(extension.ToLower()))
                    {
                        return ApiResults.Problem(Result.Failure(Error.Problem("Request.Invalid", "Invalid file type")));
                    }
                    if (!fileProvider.IsSafeMimeType(request.JawabanImg)) {
                        return ApiResults.Problem(Result.Failure(Error.Problem("Request.Invalid", "Invalid mime type")));
                    }
                    if (!fileProvider.IsValidMimeTypeAllowedExtension(request.JawabanImg.ContentType, extension.ToLower()))
                    {
                        return ApiResults.Problem(Result.Failure(Error.Problem("Request.Invalid", "Invalid allowed extension in mime type")));
                    }

                    using (var stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
                    {
                        await request.JawabanImg.CopyToAsync(stream);
                        stream.Close();
                        File.SetAttributes(filePath, FileAttributes.Normal);
                        File.SetAttributes(filePath, (FileAttributes)Convert.ToInt32("600", 8));
                    }

                    jawabanImgPath = "jawaban_img/" + safeFileName; // Relative path
                }

                Result result = await sender.Send(new UpdateTemplateJawabanCommand(
                     Guid.Parse(request.Id),
                     Guid.Parse(request.IdTemplateSoal),
                     request.JawabanText,
                     jawabanImgPath
                 ));

                return result.Match(() => Results.Ok(), ApiResults.Problem);
            }).WithTags(Tags.TemplateJawaban)
              .Accepts<UpdateTemplateJawabanRequest>("multipart/form-data")
              .DisableAntiforgery()
              .RequireAuthorization();
        }

        internal sealed class UpdateTemplateJawabanRequest
        {
            [FromForm] public string Id { get; set; }
            [FromForm] public string IdTemplateSoal { get; set; }
            [FromForm] public string? JawabanText { get; set; }
            [FromForm] public IFormFile? JawabanImg { get; set; }
        }

        /*public class UpdateTemplateJawabanRequestValidator : AbstractValidator<UpdateTemplateJawabanRequest>
        {
            public UpdateTemplateJawabanRequestValidator(IFileProvider fileProvider)
            {
                // Validate Id - must be a valid Guid
                RuleFor(x => x.Id)
                    .NotEmpty().WithMessage("Id is required.")
                    .Must(x => Guid.TryParse(x.ToString(), out _)).WithMessage("Invalid Id format.");

                // Validate IdTemplateSoal - must be a valid Guid
                RuleFor(x => x.IdTemplateSoal)
                    .NotEmpty().WithMessage("Template Soal Id is required.")
                    .Must(x => Guid.TryParse(x.ToString(), out _)).WithMessage("Invalid Template Soal Id format.");

                // Validate JawabanText - Optional but must not be too long (if provided)
                RuleFor(x => x.JawabanText)
                    .MaximumLength(1000).WithMessage("JawabanText cannot exceed 1000 characters.");

                // Validate JawabanImg - Optional, but must meet size and type restrictions
                RuleFor(x => x.JawabanImg)
                    .Must(file => ValidateSafeFileExtension(file, fileProvider)).WithMessage("upload file extension contains ASCII null which is vulnerable to exploit")
                    .Must(ValidateFileSize).WithMessage("File size is too large. Max 5MB.")
                    .Must(ValidateFileExtension).WithMessage("Invalid file type. Allowed types: .png, .jpg, .jpeg.");
            }

            private bool ValidateFileSize(IFormFile? file)
            {
                // Ensure file size is less than 5 MB
                return file == null || file.Length <= 5 * 1024 * 1024; // 5 MB limit
            }

            private bool ValidateFileExtension(IFormFile? file)
            {
                if (file == null)
                    return true;

                var allowedExtensions = new[] { ".png", ".jpg", ".jpeg" };
                var fileExtension = Path.GetExtension(file.FileName)?.ToLower();
                return allowedExtensions.Contains(fileExtension);
            }

            private bool ValidateSafeFileExtension(IFormFile? file, IFileProvider fileProvider)
            {
                if (file == null)
                    return true;

                return fileProvider.IsSafeFileExtension(file.FileName);
            }
        }*/
    }
}
