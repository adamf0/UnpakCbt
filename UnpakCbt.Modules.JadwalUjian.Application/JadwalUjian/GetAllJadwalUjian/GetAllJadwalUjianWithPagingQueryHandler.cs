using Dapper;
using Microsoft.Extensions.Logging;
using System.Data.Common;
using System.Text;
using UnpakCbt.Common.Application.Data;
using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Common.Application.Pagingnation;
using UnpakCbt.Common.Application.SortAndFilter;
using UnpakCbt.Common.Domain;
using UnpakCbt.Modules.JadwalUjian.Application.JadwalUjian.GetJadwalUjian;
using UnpakCbt.Modules.JadwalUjian.Domain.JadwalUjian;

namespace UnpakCbt.Modules.JadwalUjian.Application.JadwalUjian.GetAllJadwalUjian
{
    internal sealed class GetAllJadwalUjianWithPagingQueryHandler(IDbConnectionFactory _dbConnectionFactory, ILogger<GetAllJadwalUjianWithPagingQueryHandler> logger)
        : IQueryHandler<GetAllJadwalUjianWithPagingQuery, PagedList<JadwalUjianResponse>>
    {
        public async Task<Result<PagedList<JadwalUjianResponse>>> Handle(GetAllJadwalUjianWithPagingQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Received command with parameters: {@Request}", request);

            await using DbConnection connection = await _dbConnectionFactory.OpenConnectionAsync();

            StringBuilder sql = new(
            """
            SELECT 
                CAST(NULLIF(ju.uuid, '') AS VARCHAR(36)) AS Uuid,
                ju.deskripsi as Deskripsi,
                ju.kuota AS Kouta,
                ju.tanggal AS Tanggal,
                ju.jam_mulai_ujian AS JamMulai,
                ju.jam_akhir_ujian AS JamAkhir,
                bs.uuid AS UuidBankSoal
            FROM jadwal_ujian ju 
            LEFT JOIN bank_soal bs ON ju.id_bank_soal = bs.id
            """);

            var queryBuilder = new DynamicQueryBuilder();
            DynamicParameters parameters = new();

            try
            {
                List<SearchColumn> allowSearch = new()
                {
                    new("tanggal", "jadwal_ujian.judul", ""),
                    new("rule", "jadwal_ujian.rule", ""),
                    new("deskripsi", "jadwal_ujian.deskripsi", "")
                };
                string[]? allowSearchKeys = allowSearch.Select(a => a.Key).ToArray();

                string[] searchKeys = request.SearchColumn?.Select(sc => sc.Key).ToArray() ?? Array.Empty<string>();
                string[] sortKeys = request.SortColumn?.Select(sc => sc.Key).ToArray() ?? Array.Empty<string>();

                if (request.Page < 1)
                {
                    return Result.Failure<PagedList<JadwalUjianResponse>>(JadwalUjianErrors.InvalidPage());
                }
                if (request.PageSize < 1)
                {
                    return Result.Failure<PagedList<JadwalUjianResponse>>(JadwalUjianErrors.InvalidPageSize());
                }

                List<string> invalidKeys = searchKeys.Except(allowSearchKeys).ToList();
                if (invalidKeys.Any())
                {
                    return Result.Failure<PagedList<JadwalUjianResponse>>(JadwalUjianErrors.InvalidSearchRegistry(string.Join(",", invalidKeys)));
                }
                invalidKeys = sortKeys.Except(allowSearchKeys).ToList();
                if (invalidKeys.Any())
                {
                    return Result.Failure<PagedList<JadwalUjianResponse>>(JadwalUjianErrors.InvalidSortRegistry(string.Join(",", invalidKeys)));
                }

                queryBuilder.ApplySearchFilters(request, allowSearch);
                sql.Append(queryBuilder.BuildWhereClause());
                parameters = queryBuilder.GetParameters();

                queryBuilder.ApplySorting(sql, request);

                logger.LogInformation("Executing query: {@Query}; \nparameters: {@parameters}; \nPage: {@Page}; \nPageSize: {@PageSize}", sql.ToString(), parameters.ParameterNames.ToDictionary(p => p, p => parameters.Get<object>(p)), request.Page, request.PageSize);

                var pagedResult = await PagedList<JadwalUjianResponse>.CreateAsync(sql.ToString(), parameters, request.Page, request.PageSize, connection, logger);

                if (!pagedResult.Items.Any())
                {
                    return Result.Failure<PagedList<JadwalUjianResponse>>(JadwalUjianErrors.EmptyData());
                }

                return Result.Success(pagedResult);
            }
            catch (ArgumentException ex)
            {
                return Result.Failure<PagedList<JadwalUjianResponse>>(JadwalUjianErrors.InvalidArgs(ex.Message));
            }
        }
    }
}
