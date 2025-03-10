using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Common.Application.Pagingnation;
using UnpakCbt.Common.Application.SortAndFilter;
using UnpakCbt.Modules.Account.Application.Account.GetAccount;

namespace UnpakCbt.Modules.Account.Application.Account.GetAllAccount
{
    public sealed class GetAllAccountWithPagingQuery : IQuery<PagedList<AccountResponse>>, ISearchable
    {
        public string? SearchTerm { get; set; }
        public List<SearchColumn>? SearchColumn { get; set; } = new();
        public List<SortColumn>? SortColumn { get; set; } = new();
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public GetAllAccountWithPagingQuery(
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
