using Dapper;
using System.Data.Common;
using Microsoft.Extensions.Logging;

namespace UnpakCbt.Common.Application.Pagingnation
{
    public class PagedList<T>
    {
        public PagedList(List<T> items, int page, int pageSize, int totalCount)
        {
            Items = items;
            Page = page;
            PageSize = pageSize;
            TotalCount = totalCount;
        }

        public List<T> Items { get; }

        public int Page { get; }

        public int PageSize { get; }

        public int TotalCount { get; }

        public bool HasNextPage => Page * PageSize < TotalCount;

        public bool HasPreviousPage => Page > 1;

        public static async Task<PagedList<T>> CreateAsync(string sql, DynamicParameters parameters, int page, int pageSize, DbConnection connection, ILogger logger)
        {
            // Hitung total data terlebih dahulu
            string countSql = $"SELECT COUNT(*) FROM ({sql}) AS countQuery";
            int totalCount = await connection.ExecuteScalarAsync<int>(countSql, parameters);

            // Tambahkan pagination ke query utama
            string paginatedSql = $"{sql} LIMIT @PageSize OFFSET @Offset";
            parameters.Add("PageSize", pageSize);
            parameters.Add("Offset", (page - 1) * pageSize);

            // Ambil data berdasarkan pagination
            Console.WriteLine($"paginatedSql: {paginatedSql}; pageSize: {pageSize}; Offset: {(page - 1) * pageSize}");
            logger.LogInformation(
                    "paginatedSql: {@paginatedSql}; \n" +
                    "pageSize: {@pageSize}; \n" +
                    "Offset: {@Offset};",
                    paginatedSql,
                    pageSize,
                    (page - 1) * pageSize
            );
            var items = (await connection.QueryAsync<T>(paginatedSql, parameters)).ToList();

            return new PagedList<T>(items, page, pageSize, totalCount);
        }
    }
}
