using Dapper;
using System.Data.Common;
using UnpakCbt.Common.Application.Data;
using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Common.Domain;
using UnpakCbt.Modules.Account.Application.Account.GetAccount;
using UnpakCbt.Modules.Account.Domain.Account;


namespace UnpakCbt.Modules.Account.Application.Account.GetAllAccount
{
    internal sealed class GetAllAccountQueryHandler(IDbConnectionFactory _dbConnectionFactory)
        : IQueryHandler<GetAllAccountQuery, List<AccountResponse>>
    {
        public async Task<Result<List<AccountResponse>>> Handle(GetAllAccountQuery request, CancellationToken cancellationToken)
        {
            await using DbConnection connection = await _dbConnectionFactory.OpenConnectionAsync();

            const string sql =
            """
            SELECT 
                CAST(NULLIF(uuid, '') AS VARCHAR(36)) AS Uuid,
                username as Username,
                level AS Level,
                status AS Status
            FROM user
            """;

            var queryResult = await connection.QueryAsync<AccountResponse>(sql);

            if (!queryResult.Any())
            {
                return Result.Failure<List<AccountResponse>>(AccountErrors.EmptyData());
            }

            return Result.Success(queryResult.ToList());
        }
    }
}
