using Dapper;
using System.Data.Common;
using UnpakCbt.Common.Application.Data;
using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Common.Domain;
using UnpakCbt.Modules.Ujian.Application.Ujian.GetUjian;
using UnpakCbt.Modules.Ujian.Domain.Ujian;

namespace UnpakCbt.Modules.Ujian.Application.Ujian.GetAllUjian
{
    internal sealed class GetAllUjianByJadwalUjianQueryHandler(IDbConnectionFactory _dbConnectionFactory)
        : IQueryHandler<GetAllUjianByJadwalUjianQuery, List<UjianDetailResponse>>
    {
        public async Task<Result<List<UjianDetailResponse>>> Handle(GetAllUjianByJadwalUjianQuery request, CancellationToken cancellationToken)
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
            WHERE jadwal_ujian.uuid = @UuidJadwalUjian
            """;

            var queryResult = await connection.QueryAsync<UjianDetailResponse>(sql, new { UuidJadwalUjian = request.uuidJadwalUjian });

            if (!queryResult.Any())
            {
                return Result.Failure<List<UjianDetailResponse>>(UjianErrors.EmptyData());
            }

            return Result.Success(queryResult.ToList());
        }
    }
}
