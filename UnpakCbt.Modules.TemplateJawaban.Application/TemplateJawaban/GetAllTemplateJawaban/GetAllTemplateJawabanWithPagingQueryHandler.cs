using Dapper;
using Microsoft.Extensions.Logging;
using System.Data.Common;
using System.Text;
using UnpakCbt.Common.Application.Data;
using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Common.Application.Pagingnation;
using UnpakCbt.Common.Application.SortAndFilter;
using UnpakCbt.Common.Domain;
using UnpakCbt.Modules.TemplateJawaban.Application.TemplateJawaban.GetTemplateJawaban;
using UnpakCbt.Modules.TemplateJawaban.Domain.TemplateJawaban;

namespace UnpakCbt.Modules.TemplateJawaban.Application.TemplateJawaban.GetAllTemplateJawaban
{
    internal sealed class GetAllTemplateJawabanWithPagingQueryHandler(IDbConnectionFactory _dbConnectionFactory, ILogger<GetAllTemplateJawabanWithPagingQueryHandler> logger)
        : IQueryHandler<GetAllTemplateJawabanWithPagingQuery, PagedList<TemplateJawabanResponse>>
    {
        public async Task<Result<PagedList<TemplateJawabanResponse>>> Handle(GetAllTemplateJawabanWithPagingQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Received command with parameters: {@Request}", request);

            await using DbConnection connection = await _dbConnectionFactory.OpenConnectionAsync();

            StringBuilder sql = new(
            """
            SELECT 
                ts.uuid as UuidTemplateSoal,
                CAST(NULLIF(tp.uuid, '') AS VARCHAR(36)) AS Uuid,
                tp.jawaban_text as JawabanText,
                tp.jawaban_img AS JawabanImg 
            FROM template_pilihan tp 
            LEFT JOIN template_soal ts ON tp.id_template_soal = ts.id
            """);

            var queryBuilder = new DynamicQueryBuilder();
            DynamicParameters parameters = new();

            try
            {
                List<SearchColumn> allowSearch = new()
                {
                    new("jawaban_text", "template_soal.jawaban_text", ""),
                    new("jawaban_img", "template_soal.jawaban_img", "")
                };
                string[]? allowSearchKeys = allowSearch.Select(a => a.Key).ToArray();

                string[] searchKeys = request.SearchColumn?.Select(sc => sc.Key).ToArray() ?? Array.Empty<string>();
                string[] sortKeys = request.SortColumn?.Select(sc => sc.Key).ToArray() ?? Array.Empty<string>();

                if (request.Page < 1)
                {
                    return Result.Failure<PagedList<TemplateJawabanResponse>>(TemplateJawabanErrors.InvalidPage());
                }
                if (request.PageSize < 1)
                {
                    return Result.Failure<PagedList<TemplateJawabanResponse>>(TemplateJawabanErrors.InvalidPageSize());
                }

                List<string> invalidKeys = searchKeys.Except(allowSearchKeys).ToList();
                if (invalidKeys.Any())
                {
                    return Result.Failure<PagedList<TemplateJawabanResponse>>(TemplateJawabanErrors.InvalidSearchRegistry(string.Join(",", invalidKeys)));
                }
                invalidKeys = sortKeys.Except(allowSearchKeys).ToList();
                if (invalidKeys.Any())
                {
                    return Result.Failure<PagedList<TemplateJawabanResponse>>(TemplateJawabanErrors.InvalidSortRegistry(string.Join(",", invalidKeys)));
                }

                queryBuilder.ApplySearchFilters(request, allowSearch);
                sql.Append(queryBuilder.BuildWhereClause());
                parameters = queryBuilder.GetParameters();

                queryBuilder.ApplySorting(sql, request);

                logger.LogInformation("Executing query: {@Query}; \nparameters: {@parameters}; \nPage: {@Page}; \nPageSize: {@PageSize}", sql.ToString(), parameters.ParameterNames.ToDictionary(p => p, p => parameters.Get<object>(p)), request.Page, request.PageSize);

                var pagedResult = await PagedList<TemplateJawabanResponse>.CreateAsync(sql.ToString(), parameters, request.Page, request.PageSize, connection, logger);

                if (!pagedResult.Items.Any())
                {
                    return Result.Failure<PagedList<TemplateJawabanResponse>>(TemplateJawabanErrors.EmptyData());
                }

                return Result.Success(pagedResult);
            }
            catch (ArgumentException ex)
            {
                return Result.Failure<PagedList<TemplateJawabanResponse>>(TemplateJawabanErrors.InvalidArgs(ex.Message));
            }
        }
    }
}
