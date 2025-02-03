using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Text.Encodings.Web;
using UnpakCbt.Common.Domain;
using UnpakCbt.Common.Presentation.ApiResults;
using UnpakCbt.Modules.TemplatePertanyaan.Application.TemplatePertanyaan.CreateTemplatePertanyaan;
using UnpakCbt.Modules.TemplatePertanyaan.Presentation;

namespace UnpakCbt.Modules.TemplatePertanyaan.Presentation.TemplatePertanyaan
{
    internal static class CreateTemplatePertanyaan
    {
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("TemplatePertanyaan", async (CreateTemplatePertanyaanRequest request, ISender sender) =>
            {
                Result<Guid> result = await sender.Send(new CreateTemplatePertanyaanCommand(
                    request.IdBankSoal,
                    request.Tipe,
                    request.Pertanyaan,
                    request.Gambar,
                    request.Jawaban,
                    request.State
                    )
                );

                return result.Match(Results.Ok, ApiResults.Problem);

            }).WithTags(Tags.TemplatePertanyaan);
        }

        internal sealed class CreateTemplatePertanyaanRequest
        {
            public Guid IdBankSoal { get; set; }

            public string Tipe { get; set; }
            public string Pertanyaan { get; set; }
            public string Gambar { get; set; }
            public Guid? Jawaban { get; set; }
            public string State { get; set; }
        }
    }
}
