using Dapper;
using Microsoft.Extensions.Logging;
using System.Data.Common;
using System.Text;
using UnpakCbt.Common.Application.Data;
using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Common.Application.Pagingnation;
using UnpakCbt.Common.Application.SortAndFilter;
using UnpakCbt.Common.Domain;
using UnpakCbt.Modules.BankSoal.Application.BankSoal.GetBankSoal;
using UnpakCbt.Modules.BankSoal.Domain.BankSoal;

namespace UnpakCbt.Modules.BankSoal.Application.BankSoal.GetAllBankSoal
{
    internal sealed class GetAllBankSoalWithPagingQueryHandler(IDbConnectionFactory _dbConnectionFactory, ILogger<GetAllBankSoalWithPagingQueryHandler> logger)
        : IQueryHandler<GetAllBankSoalWithPagingQuery, PagedList<BankSoalResponse>>
    {
        public async Task<Result<PagedList<BankSoalResponse>>> Handle(GetAllBankSoalWithPagingQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Received command with parameters: {@Request}", request);

            await using DbConnection connection = await _dbConnectionFactory.OpenConnectionAsync();

            StringBuilder sql = new StringBuilder(
            """
            SELECT 
                uuid AS Uuid,
                judul AS Judul,
                rule AS Rule,
                status AS Status,
                (SELECT COUNT(*) FROM jadwal_ujian WHERE jadwal_ujian.id_bank_soal = bank_soal.id) AS JadwalTerhubung,
                disabled AS Disabled 
            FROM bank_soal
            """);

            var queryBuilder = new DynamicQueryBuilder();
            DynamicParameters parameters = new();

            try
            {
                List<SearchColumn> allowSearch = new()
                {
                    new("judul", "bank_soal.judul", ""),
                    new("rule", "bank_soal.rule", ""),
                    new("status", "bank_soal.status", "") 
                };
                string[]? allowSearchKeys = allowSearch.Select(a => a.Key).ToArray();

                string[] searchKeys = request.SearchColumn?.Select(sc => sc.Key).ToArray() ?? Array.Empty<string>();
                string[] sortKeys = request.SortColumn?.Select(sc => sc.Key).ToArray() ?? Array.Empty<string>();
                
                if (request.Page <1) {
                    return Result.Failure<PagedList<BankSoalResponse>>(BankSoalErrors.InvalidPage());
                }
                if (request.PageSize <1)
                {
                    return Result.Failure<PagedList<BankSoalResponse>>(BankSoalErrors.InvalidPageSize());
                }

                List<string> invalidKeys = searchKeys.Except(allowSearchKeys).ToList();
                if (invalidKeys.Any())
                {
                    return Result.Failure<PagedList<BankSoalResponse>>(BankSoalErrors.InvalidSearchRegistry(string.Join(",", invalidKeys)));
                }
                invalidKeys = sortKeys.Except(allowSearchKeys).ToList();
                if (invalidKeys.Any())
                {
                    return Result.Failure<PagedList<BankSoalResponse>>(BankSoalErrors.InvalidSortRegistry(string.Join(",", invalidKeys)));
                }

                queryBuilder.ApplySearchFilters(request, allowSearch);
                sql.Append(queryBuilder.BuildWhereClause());
                parameters = queryBuilder.GetParameters();

                queryBuilder.ApplySorting(sql, request);

                logger.LogInformation("Executing query: {@Query}; \nparameters: {@parameters}; \nPage: {@Page}; \nPageSize: {@PageSize}", sql.ToString(), parameters.ParameterNames.ToDictionary(p => p, p => parameters.Get<object>(p)), request.Page, request.PageSize);

                var pagedResult = await PagedList<BankSoalResponse>.CreateAsync(sql.ToString(), parameters, request.Page, request.PageSize, connection, logger);

                if (!pagedResult.Items.Any())
                {
                    return Result.Failure<PagedList<BankSoalResponse>>(BankSoalErrors.EmptyData());
                }

                return Result.Success(pagedResult);
            }
            catch (ArgumentException ex)
            {
                return Result.Failure<PagedList<BankSoalResponse>>(BankSoalErrors.InvalidArgs(ex.Message));
            }
        }
    }
}
