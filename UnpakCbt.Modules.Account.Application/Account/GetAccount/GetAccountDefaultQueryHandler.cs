using Dapper;
using System.Data.Common;
using UnpakCbt.Common.Application.Data;
using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Common.Domain;
using UnpakCbt.Modules.Account.Domain.Account;

namespace UnpakCbt.Modules.Account.Application.Account.GetAccount
{
    internal sealed class GetAccountDefaultQueryHandler(IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetAccountDefaultQuery, AccountDefaultResponse>
    {
        public async Task<Result<AccountDefaultResponse>> Handle(GetAccountDefaultQuery request, CancellationToken cancellationToken)
        {
            await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

            //CAST(NULLIF(id_group, '') AS CHAR(36)) -> guid
            const string sql =
                $"""
                 SELECT 
                     id as Id,
                     CAST(NULLIF(uuid, '') AS VARCHAR(36)) AS Uuid,
                     username as Username,
                     password AS Password,
                     level AS Level,
                     status AS Status
                 FROM user 
                 WHERE uuid = @Uuid
                 """;

            DefaultTypeMap.MatchNamesWithUnderscores = true;

            var result = await connection.QuerySingleOrDefaultAsync<AccountDefaultResponse?>(sql, new { Uuid = request.AccountUuid });
            if (result == null)
            {
                return Result.Failure<AccountDefaultResponse>(AccountErrors.NotFound(request.AccountUuid));
            }

            return result;
        }
    }
}
