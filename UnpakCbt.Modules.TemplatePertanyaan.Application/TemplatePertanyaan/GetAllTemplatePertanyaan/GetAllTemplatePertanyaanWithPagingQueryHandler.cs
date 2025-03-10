using Dapper;
using Microsoft.Extensions.Logging;
using System.Data.Common;
using System.Text;
using UnpakCbt.Common.Application.Data;
using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Common.Application.Pagingnation;
using UnpakCbt.Common.Application.SortAndFilter;
using UnpakCbt.Common.Domain;
using UnpakCbt.Modules.TemplatePertanyaan.Application.TemplatePertanyaan.GetTemplatePertanyaan;
using UnpakCbt.Modules.TemplatePertanyaan.Domain.TemplatePertanyaan;

namespace UnpakCbt.Modules.TemplatePertanyaan.Application.TemplatePertanyaan.GetAllTemplatePertanyaan
{
    internal sealed class GetAllTemplatePertanyaanWithPagingQueryHandler(IDbConnectionFactory _dbConnectionFactory, ILogger<GetAllTemplatePertanyaanWithPagingQueryHandler> logger)
        : IQueryHandler<GetAllTemplatePertanyaanWithPagingQuery, PagedList<TemplatePertanyaanResponse>>
    {
        public async Task<Result<PagedList<TemplatePertanyaanResponse>>> Handle(GetAllTemplatePertanyaanWithPagingQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Received command with parameters: {@Request}", request);

            await using DbConnection connection = await _dbConnectionFactory.OpenConnectionAsync();

            StringBuilder sql = new(
            """
            SELECT 
                CAST(NULLIF(ts.uuid, '') as VARCHAR(36)) AS Uuid,
                CAST(NULLIF(bs.uuid, '') as VARCHAR(36)) as UuidBankSoal,
                ts.tipe as Tipe,
                ts.pertanyaan_text as Pertanyaan,
                ts.pertanyaan_img as Gambar,
                CAST(NULLIF(tp.uuid, '') as VARCHAR(36)) as UuidJawabanBenar,
                ts.bobot as Bobot,
                ts.state as State 
            FROM template_soal ts 
            LEFT JOIN bank_soal bs ON ts.id_bank_soal = bs.id 
            LEFT JOIN template_pilihan tp ON ts.jawaban_benar = tp.id
            """);

            var queryBuilder = new DynamicQueryBuilder();
            DynamicParameters parameters = new();

            try
            {
                List<SearchColumn> allowSearch = new()
                {
                    new("pertanyaan", "template_soal.pertanyaan", ""),
                    new("tipe", "template_soal.tipe", ""),
                    new("state", "template_soal.state", "")
                };
                string[]? allowSearchKeys = allowSearch.Select(a => a.Key).ToArray();

                string[] searchKeys = request.SearchColumn?.Select(sc => sc.Key).ToArray() ?? Array.Empty<string>();
                string[] sortKeys = request.SortColumn?.Select(sc => sc.Key).ToArray() ?? Array.Empty<string>();

                if (request.Page < 1)
                {
                    return Result.Failure<PagedList<TemplatePertanyaanResponse>>(TemplatePertanyaanErrors.InvalidPage());
                }
                if (request.PageSize < 1)
                {
                    return Result.Failure<PagedList<TemplatePertanyaanResponse>>(TemplatePertanyaanErrors.InvalidPageSize());
                }

                List<string> invalidKeys = searchKeys.Except(allowSearchKeys).ToList();
                if (invalidKeys.Any())
                {
                    return Result.Failure<PagedList<TemplatePertanyaanResponse>>(TemplatePertanyaanErrors.InvalidSearchRegistry(string.Join(",", invalidKeys)));
                }
                invalidKeys = sortKeys.Except(allowSearchKeys).ToList();
                if (invalidKeys.Any())
                {
                    return Result.Failure<PagedList<TemplatePertanyaanResponse>>(TemplatePertanyaanErrors.InvalidSortRegistry(string.Join(",", invalidKeys)));
                }

                queryBuilder.ApplySearchFilters(request, allowSearch);
                sql.Append(queryBuilder.BuildWhereClause());
                parameters = queryBuilder.GetParameters();

                queryBuilder.ApplySorting(sql, request);

                logger.LogInformation("Executing query: {@Query}; \nparameters: {@parameters}; \nPage: {@Page}; \nPageSize: {@PageSize}", sql.ToString(), parameters.ParameterNames.ToDictionary(p => p, p => parameters.Get<object>(p)), request.Page, request.PageSize);

                var pagedResult = await PagedList<TemplatePertanyaanResponse>.CreateAsync(sql.ToString(), parameters, request.Page, request.PageSize, connection, logger);

                if (!pagedResult.Items.Any())
                {
                    return Result.Failure<PagedList<TemplatePertanyaanResponse>>(TemplatePertanyaanErrors.EmptyData());
                }

                return Result.Success(pagedResult);
            }
            catch (ArgumentException ex)
            {
                return Result.Failure<PagedList<TemplatePertanyaanResponse>>(TemplatePertanyaanErrors.InvalidArgs(ex.Message));
            }
        }
    }
}
