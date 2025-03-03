using Dapper;
using System.Data.Common;
using UnpakCbt.Common.Application.Data;
using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Common.Domain;
using UnpakCbt.Modules.Account.Domain.Account;

namespace UnpakCbt.Modules.Account.Application.Account.GetAccount
{
    internal sealed class GetAccountQueryHandler(IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetAccountQuery, AccountResponse>
    {
        public async Task<Result<AccountResponse>> Handle(GetAccountQuery request, CancellationToken cancellationToken)
        {
            await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

            //CAST(NULLIF(id_group, '') AS CHAR(36)) -> guid
            const string sql =
                $"""
                 SELECT 
                     CAST(NULLIF(uuid, '') AS VARCHAR(36)) AS Uuid,
                     username as Username,
                     level AS Level,
                     status AS Status
                 FROM user 
                 WHERE uuid = @Uuid
                 """;

            DefaultTypeMap.MatchNamesWithUnderscores = true;

            var result = await connection.QuerySingleOrDefaultAsync<AccountResponse?>(sql, new { Uuid = request.AccountUuid });
            if (result == null)
            {
                return Result.Failure<AccountResponse>(AccountErrors.NotFound(request.AccountUuid));
            }

            return result;
        }
    }
}
