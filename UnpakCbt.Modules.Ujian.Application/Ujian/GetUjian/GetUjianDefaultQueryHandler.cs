using Dapper;
using System.Data.Common;
using UnpakCbt.Common.Application.Data;
using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Common.Domain;
using UnpakCbt.Modules.Ujian.Domain.Ujian;

namespace UnpakCbt.Modules.Ujian.Application.Ujian.GetUjian
{
    internal sealed class GetUjianDefaultQueryHandler(IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetUjianDefaultQuery, UjianDefaultResponse>
    {
        public async Task<Result<UjianDefaultResponse>> Handle(GetUjianDefaultQuery request, CancellationToken cancellationToken)
        {
            await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

            //CAST(NULLIF(id_group, '') AS CHAR(36)) -> guid
            const string sql =
                $"""
                 SELECT 
                     id as Id,
                     CAST(NULLIF(uuid, '') AS VARCHAR(36)) AS Uuid,
                     no_reg as NoReg,
                     id_jadwal_ujian AS JadwalUjian,
                     status AS Status
                 FROM ujian  
                 WHERE uuid = @Uuid
                 """;

            DefaultTypeMap.MatchNamesWithUnderscores = true;

            var result = await connection.QuerySingleOrDefaultAsync<UjianDefaultResponse?>(sql, new { Uuid = request.UjianUuid });
            if (result == null)
            {
                return Result.Failure<UjianDefaultResponse>(UjianErrors.NotFound(request.UjianUuid));
            }

            return result;
        }
    }
}
