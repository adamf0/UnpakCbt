using Dapper;
using System.Data.Common;
using UnpakCbt.Common.Application.Data;
using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Common.Domain;
using UnpakCbt.Modules.Ujian.Domain.Ujian;

namespace UnpakCbt.Modules.Ujian.Application.Ujian.GetUjian
{
    internal sealed class GetUjianQueryHandler(IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetUjianQuery, UjianResponse>
    {
        public async Task<Result<UjianResponse>> Handle(GetUjianQuery request, CancellationToken cancellationToken)
        {
            await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

            //CAST(NULLIF(id_group, '') AS CHAR(36)) -> guid
            const string sql =
                $"""
                 SELECT 
                     CAST(NULLIF(ujian.uuid, '') AS VARCHAR(36)) AS Uuid,
                     ujian.no_reg as NoReg,
                     CAST(NULLIF(jadwal_ujian.uuid, '') AS VARCHAR(36)) AS UuidJadwalUjian,
                     ujian.status AS Status
                 FROM ujian 
                 JOIN jadwal_ujian ON ujian.id_jadwal_ujian = jadwal_ujian.id
                 WHERE ujian.uuid = @Uuid
                 """;

            DefaultTypeMap.MatchNamesWithUnderscores = true;

            var result = await connection.QuerySingleOrDefaultAsync<UjianResponse?>(sql, new { Uuid = request.UjianUuid });
            if (result == null)
            {
                return Result.Failure<UjianResponse>(UjianErrors.NotFound(request.UjianUuid));
            }

            return result;
        }
    }
}
