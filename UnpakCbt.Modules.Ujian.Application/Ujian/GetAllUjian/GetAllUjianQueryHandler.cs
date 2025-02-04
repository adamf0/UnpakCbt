using Dapper;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using UnpakCbt.Common.Application.Data;
using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Common.Domain;
using System.Text.Json;
using UnpakCbt.Modules.Ujian.Domain.Ujian;
using UnpakCbt.Modules.Ujian.Application.Ujian.GetUjian;

namespace UnpakCbt.Modules.Ujian.Application.Ujian.GetAllUjian
{
    internal sealed class GetAllUjianQueryHandler(IDbConnectionFactory _dbConnectionFactory)
        : IQueryHandler<GetAllUjianQuery, List<UjianResponse>>
    {
        public async Task<Result<List<UjianResponse>>> Handle(GetAllUjianQuery request, CancellationToken cancellationToken)
        {
            await using DbConnection connection = await _dbConnectionFactory.OpenConnectionAsync();

            const string sql =
            """
            SELECT 
                CAST(NULLIF(uuid, '') AS VARCHAR(36)) AS Uuid,
                no_reg as NoReg,
                id_jadwal_ujian AS JadwalUjian,
                status AS Status
            FROM ujian
            """;

            var queryResult = await connection.QueryAsync<UjianResponse>(sql);

            if (!queryResult.Any())
            {
                return Result.Failure<List<UjianResponse>>(UjianErrors.EmptyData());
            }

            return Result.Success(queryResult.ToList());
        }
    }
}
