using Dapper;
using Microsoft.Extensions.Logging;
using System.Data.Common;
using System.Text;
using UnpakCbt.Common.Application.Data;
using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Common.Application.Pagingnation;
using UnpakCbt.Common.Application.SortAndFilter;
using UnpakCbt.Common.Domain;
using UnpakCbt.Modules.Account.Application.Account.GetAccount;
using UnpakCbt.Modules.Account.Domain.Account;

namespace UnpakCbt.Modules.Account.Application.Account.GetAllAccount
{
    internal sealed class GetAllAccountWithPagingQueryHandler(IDbConnectionFactory _dbConnectionFactory, ILogger<GetAllAccountWithPagingQueryHandler> logger)
        : IQueryHandler<GetAllAccountWithPagingQuery, PagedList<AccountResponse>>
    {
        public async Task<Result<PagedList<AccountResponse>>> Handle(GetAllAccountWithPagingQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Received command with parameters: {@Request}", request);

            await using DbConnection connection = await _dbConnectionFactory.OpenConnectionAsync();

            StringBuilder sql = new(
            """
            SELECT 
                CAST(NULLIF(uuid, '') AS VARCHAR(36)) AS Uuid,
                username as Username,
                level AS Level,
                status AS Status
            FROM user
            """);

            var queryBuilder = new DynamicQueryBuilder();
            DynamicParameters parameters = new();

            try
            {
                List<SearchColumn> allowSearch = new()
                {
                    new("username", "user.username", ""),
                    new("level", "user.level", ""),
                    new("status", "user.status", "")
                };
                string[]? allowSearchKeys = allowSearch.Select(a => a.Key).ToArray();

                string[] searchKeys = request.SearchColumn?.Select(sc => sc.Key).ToArray() ?? Array.Empty<string>();
                string[] sortKeys = request.SortColumn?.Select(sc => sc.Key).ToArray() ?? Array.Empty<string>();

                if (request.Page < 1)
                {
                    return Result.Failure<PagedList<AccountResponse>>(AccountErrors.InvalidPage());
                }
                if (request.PageSize < 1)
                {
                    return Result.Failure<PagedList<AccountResponse>>(AccountErrors.InvalidPageSize());
                }

                List<string> invalidKeys = searchKeys.Except(allowSearchKeys).ToList();
                if (invalidKeys.Any())
                {
                    return Result.Failure<PagedList<AccountResponse>>(AccountErrors.InvalidSearchRegistry(string.Join(",", invalidKeys)));
                }
                invalidKeys = sortKeys.Except(allowSearchKeys).ToList();
                if (invalidKeys.Any())
                {
                    return Result.Failure<PagedList<AccountResponse>>(AccountErrors.InvalidSortRegistry(string.Join(",", invalidKeys)));
                }

                queryBuilder.ApplySearchFilters(request, allowSearch);
                sql.Append(queryBuilder.BuildWhereClause());
                parameters = queryBuilder.GetParameters();

                queryBuilder.ApplySorting(sql, request);

                logger.LogInformation("Executing query: {@Query}; \nparameters: {@parameters}; \nPage: {@Page}; \nPageSize: {@PageSize}", sql.ToString(), parameters.ParameterNames.ToDictionary(p => p, p => parameters.Get<object>(p)), request.Page, request.PageSize);

                var pagedResult = await PagedList<AccountResponse>.CreateAsync(sql.ToString(), parameters, request.Page, request.PageSize, connection, logger);

                if (!pagedResult.Items.Any())
                {
                    return Result.Failure<PagedList<AccountResponse>>(AccountErrors.EmptyData());
                }

                return Result.Success(pagedResult);
            }
            catch (ArgumentException ex)
            {
                return Result.Failure<PagedList<AccountResponse>>(AccountErrors.InvalidArgs(ex.Message));
            }
        }
    }
}
