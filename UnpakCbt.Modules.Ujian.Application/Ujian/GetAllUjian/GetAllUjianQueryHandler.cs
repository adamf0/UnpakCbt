using Dapper;
using System.Data.Common;
using UnpakCbt.Common.Application.Data;
using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Common.Domain;
using UnpakCbt.Modules.Ujian.Application.Cbt.GetCbt;
using UnpakCbt.Modules.Ujian.Application.Ujian.GetUjian;
using UnpakCbt.Modules.Ujian.Domain.Ujian;

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
                CAST(NULLIF(ujian.uuid, '') AS VARCHAR(36)) AS Uuid,
                ujian.no_reg as NoReg,
                CAST(NULLIF(jadwal_ujian.uuid, '') AS VARCHAR(36)) AS UuidJadwalUjian,
                ujian.status AS Status
            FROM ujian 
            JOIN jadwal_ujian ON ujian.id_jadwal_ujian = jadwal_ujian.id
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
