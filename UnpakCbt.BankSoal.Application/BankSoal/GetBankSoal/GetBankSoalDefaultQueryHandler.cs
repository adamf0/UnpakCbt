using Dapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using UnpakCbt.Common.Application.Data;
using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Common.Domain;
using UnpakCbt.Modules.BankSoal.Domain.BankSoal;

namespace UnpakCbt.Modules.BankSoal.Application.BankSoal.GetBankSoal
{
    internal sealed class GetBankSoalDefaultQueryHandler(IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetBankSoalDefaultQuery, BankSoalDefaultResponse>
    {
        public async Task<Result<BankSoalDefaultResponse>> Handle(GetBankSoalDefaultQuery request, CancellationToken cancellationToken)
        {
            await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

            //CAST(NULLIF(id_group, '') AS CHAR(36)) -> guid
            const string sql =
                $"""
                 SELECT 
                     id as Id,
                     CAST(NULLIF(uuid, '') AS VARCHAR(36)) AS Uuid,
                     judul as Judul,
                     rule AS Rule 
                 FROM bank_soal 
                 WHERE uuid = @Uuid
                 """;

            DefaultTypeMap.MatchNamesWithUnderscores = true;

            var result = await connection.QuerySingleOrDefaultAsync<BankSoalDefaultResponse?>(sql, new { Uuid = request.BankSoalUuid });
            if (result == null)
            {
                return Result.Failure<BankSoalDefaultResponse>(BankSoalErrors.NotFound(request.BankSoalUuid));
            }

            return result;
        }
    }
}
