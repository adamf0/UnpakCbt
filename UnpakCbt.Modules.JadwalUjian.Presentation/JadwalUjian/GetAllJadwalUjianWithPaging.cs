using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Text.Json.Serialization;
using UnpakCbt.Common.Application.Pagingnation;
using UnpakCbt.Common.Application.SortAndFilter;
using UnpakCbt.Common.Domain;
using UnpakCbt.Common.Presentation.ApiResults;
using UnpakCbt.Common.Presentation.Security;
using UnpakCbt.Modules.JadwalUjian.Application.JadwalUjian.GetAllJadwalUjian;
using UnpakCbt.Modules.JadwalUjian.Application.JadwalUjian.GetJadwalUjian;

namespace UnpakCbt.Modules.JadwalUjian.Presentation.JadwalUjian
{
    internal class GetAllJadwalUjianWithPaging
    {
        [Authorize]
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("JadwalUjians", async (GetAllJadwalUjianWithPagingRequest request, ISender sender, HttpContext context, TokenValidator tokenValidator) =>
            {
                var (isValid, error) = tokenValidator.ValidateToken(context);
                if (!isValid)
                {
                    return error;
                }

                Result<PagedList<JadwalUjianResponse>> result = await sender.Send(new GetAllJadwalUjianWithPagingQuery(
                    request.SearchTerm,
                    request.SearchColumns,
                    request.SortColumn,
                    request.Page,
                    request.PageSize
                ));

                return result.Match(Results.Ok, ApiResults.Problem);
            }).WithTags(Tags.JadwalUjian).RequireAuthorization();
        }

        internal sealed class GetAllJadwalUjianWithPagingRequest
        {
            [JsonPropertyName("SearchTerm")]
            public string? SearchTerm { get; set; } = null;
            [JsonPropertyName("SearchColumns")]
            public List<SearchColumn>? SearchColumns { get; set; }

            [JsonPropertyName("SortColumn")]
            public List<SortColumn>? SortColumn { get; set; }
            public int Page { get; set; } = 1;
            public int PageSize { get; set; } = 1;
        }
    }
}
