using Dapper;
using System.Data.Common;
using UnpakCbt.Common.Application.Data;
using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Common.Domain;
using UnpakCbt.Modules.BankSoal.Domain.BankSoal;

namespace UnpakCbt.Modules.BankSoal.Application.BankSoal.GetBankSoal
{
    internal sealed class GetBankSoalQueryHandler(IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetBankSoalQuery, BankSoalResponse>
    {
        public async Task<Result<BankSoalResponse>> Handle(GetBankSoalQuery request, CancellationToken cancellationToken)
        {
            await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

            //CAST(NULLIF(id_group, '') AS CHAR(36)) -> guid
            const string sql =
                $"""
                 SELECT 
                     CAST(NULLIF(uuid, '') AS VARCHAR(36)) AS Uuid,
                     judul as Judul,
                     rule AS Rule 
                 FROM bank_soal 
                 WHERE uuid = @Uuid
                 """;

            DefaultTypeMap.MatchNamesWithUnderscores = true;

            var result = await connection.QuerySingleOrDefaultAsync<BankSoalResponse?>(sql, new { Uuid = request.BankSoalUuid });
            if (result == null)
            {
                return Result.Failure<BankSoalResponse>(BankSoalErrors.NotFound(request.BankSoalUuid));
            }

            return result;
        }
    }
}
