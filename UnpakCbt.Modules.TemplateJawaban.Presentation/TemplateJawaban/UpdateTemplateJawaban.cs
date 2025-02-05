using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using UnpakCbt.Common.Domain;
using UnpakCbt.Common.Presentation.ApiResults;
using UnpakCbt.Common.Presentation.FileManager;
using UnpakCbt.Modules.TemplateJawaban.Application.TemplateJawaban.UpdateTemplateJawaban;

namespace UnpakCbt.Modules.TemplateJawaban.Presentation.TemplateJawaban
{
    internal static class UpdateTemplateJawaban
    {
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("TemplateJawaban", [IgnoreAntiforgeryToken(Order = 1001)] async ([FromForm] UpdateTemplateJawabanRequest request, ISender sender, IFileProvider fileProvider) =>
            {
                string? jawabanImgPath = null;
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "jawaban_img");

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

                Result result = await sender.Send(new UpdateTemplateJawabanCommand(
                     request.Id,
                     request.IdTemplateSoal,
                     request.JawabanText,
                     jawabanImgPath
                 ));

                return result.Match(() => Results.Ok(), ApiResults.Problem);
            }).WithTags(Tags.TemplateJawaban)
              .Accepts<UpdateTemplateJawabanRequest>("multipart/form-data")
              .DisableAntiforgery();
        }

        internal sealed class UpdateTemplateJawabanRequest
        {
            [FromForm] public Guid Id { get; set; }
            [FromForm] public Guid IdTemplateSoal { get; set; }
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
