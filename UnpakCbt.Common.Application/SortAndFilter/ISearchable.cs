namespace UnpakCbt.Common.Application.SortAndFilter
{
    public interface ISearchable
    {
        List<SearchColumn>? SearchColumn { get; set; }
        List<SortColumn>? SortColumn { get; set; }
        string? SearchTerm { get; set; }
    }
}
