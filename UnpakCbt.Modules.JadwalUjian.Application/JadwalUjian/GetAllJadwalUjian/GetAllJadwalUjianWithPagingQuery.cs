using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Common.Application.Pagingnation;
using UnpakCbt.Common.Application.SortAndFilter;
using UnpakCbt.Modules.JadwalUjian.Application.JadwalUjian.GetJadwalUjian;

namespace UnpakCbt.Modules.JadwalUjian.Application.JadwalUjian.GetAllJadwalUjian
{
    public sealed class GetAllJadwalUjianWithPagingQuery : IQuery<PagedList<JadwalUjianResponse>>, ISearchable
    {
        public string? SearchTerm { get; set; }
        public List<SearchColumn>? SearchColumn { get; set; } = new();
        public List<SortColumn>? SortColumn { get; set; } = new();
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public GetAllJadwalUjianWithPagingQuery(
            string? searchTerm = null,
            List<SearchColumn>? searchColumn = null,
            List<SortColumn>? sortColumn = null,
            int page = 1,
            int pageSize = 10
        )
        {
            SearchTerm = searchTerm;
            SearchColumn = searchColumn ?? new();
            SortColumn = sortColumn ?? new();
            Page = page;
            PageSize = pageSize;
        }
    }
}
