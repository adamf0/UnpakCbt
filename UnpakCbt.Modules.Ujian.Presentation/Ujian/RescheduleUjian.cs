using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using UnpakCbt.Common.Domain;
using UnpakCbt.Common.Presentation.ApiResults;
using UnpakCbt.Modules.Ujian.Application.Ujian.RescheduleUjian;

namespace UnpakCbt.Modules.Ujian.Presentation.Ujian
{
    internal static class RescheduleUjian
    {
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("Ujian/Reschedule", async (RescheduleUjianRequest request, ISender sender) =>
            {
                Result<Guid> result = await sender.Send(new RescheduleUjianCommand(
                    request.NoReg,
                    request.PrevIdJadwalUjian,
                    request.NewIdJadwalUjian
                    )
                );

                return result.Match(Results.Ok, ApiResults.Problem);

            }).WithTags(Tags.Ujian);
        }

        internal sealed class RescheduleUjianRequest
        {            
            public string NoReg { get; set; }
            public Guid PrevIdJadwalUjian { get; set; }
            public Guid NewIdJadwalUjian { get; set; }
        }
    }
}
