using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Text.Encodings.Web;
using UnpakCbt.Common.Domain;
using UnpakCbt.Common.Presentation.ApiResults;
using UnpakCbt.Modules.TemplatePertanyaan.Application.TemplatePertanyaan.UpdateTemplatePertanyaan;
using UnpakCbt.Modules.TemplatePertanyaan.Presentation;

namespace UnpakCbt.Modules.TemplatePertanyaan.Presentation.TemplatePertanyaan
{
    internal static class UpdateTemplatePertanyaan
    {
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("TemplatePertanyaan", async (UpdateTemplatePertanyaanRequest request, ISender sender) =>
            {
                Result result = await sender.Send(new UpdateTemplatePertanyaanCommand(
                    request.Id,
                    request.IdBankSoal,
                    request.Tipe,
                    request.Pertanyaan,
                    request.Gambar,
                    request.Jawaban,
                    int.Parse(request.Bobot),
                    request.State
                    )
                );

                return result.Match(() => Results.Ok(), ApiResults.Problem);
            }).WithTags(Tags.TemplatePertanyaan);
        }

        internal sealed class UpdateTemplatePertanyaanRequest
        {
            public Guid Id { get; set; }
            public Guid IdBankSoal { get; set; }

            public string Tipe { get; set; }
            public string? Pertanyaan { get; set; } = null;
            public string? Gambar { get; set; } = null;
            public Guid Jawaban { get; set; }
            public string Bobot { get; set; }
            public string? State { get; set; } = null;
        }
    }
}
