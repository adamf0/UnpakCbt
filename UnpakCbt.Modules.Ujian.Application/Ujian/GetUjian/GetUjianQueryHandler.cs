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
            DefaultTypeMap.MatchNamesWithUnderscores = true;

            int enableTrial = 0;
            string enableTrialValue = Environment.GetEnvironmentVariable("EnableTrial");
            if (int.TryParse(enableTrialValue, out enableTrial) == false)
            {
                enableTrial = 0; // Default value
            }


            string sql;
            if (enableTrial==1) {
                sql =
                $"""
                 SELECT 
                     CAST(NULLIF(ujian.uuid, '') AS VARCHAR(36)) AS Uuid,
                     ujian.no_reg as NoReg,
                     CAST(NULLIF(jadwal_ujian.uuid, '') AS VARCHAR(36)) AS UuidJadwalUjian,
                     ujian.status AS Status,
                     ujian.coba_ujian AS FreeTrial
                 FROM ujian 
                 JOIN jadwal_ujian ON ujian.id_jadwal_ujian = jadwal_ujian.id
                 WHERE ujian.uuid = @Uuid AND ujian.no_reg = @NoReg
                 """;
            }
            else {
                sql =
                $"""
                 SELECT 
                     CAST(NULLIF(ujian.uuid, '') AS VARCHAR(36)) AS Uuid,
                     ujian.no_reg as NoReg,
                     CAST(NULLIF(jadwal_ujian.uuid, '') AS VARCHAR(36)) AS UuidJadwalUjian,
                     ujian.status AS Status,
                     0 AS FreeTrial
                 FROM ujian 
                 JOIN jadwal_ujian ON ujian.id_jadwal_ujian = jadwal_ujian.id
                 WHERE ujian.uuid = @Uuid AND ujian.no_reg = @NoReg
                 """;
            }

            UjianResponse? result = await connection.QuerySingleOrDefaultAsync<UjianResponse?>(sql, new { Uuid = request.UjianUuid, NoReg = request.NoReg });

            if (result == null)
            {
                return Result.Failure<UjianResponse>(UjianErrors.NotFound(request.UjianUuid));
            }

            return result;
        }
    }
}
