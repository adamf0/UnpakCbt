using Dapper;
using System.Data.Common;
using UnpakCbt.Common.Application.Data;
using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Common.Domain;
using UnpakCbt.Modules.JadwalUjian.Domain.JadwalUjian;

namespace UnpakCbt.Modules.JadwalUjian.Application.JadwalUjian.GetJadwalUjian
{
    internal sealed class GetJadwalUjianQueryHandler(IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetJadwalUjianQuery, JadwalUjianResponse>
    {
        public async Task<Result<JadwalUjianResponse>> Handle(GetJadwalUjianQuery request, CancellationToken cancellationToken)
        {
            await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

            //CAST(NULLIF(id_group, '') AS CHAR(36)) -> guid
            const string sql =
                $"""
                 SELECT 
                     CAST(NULLIF(ju.uuid, '') AS VARCHAR(36)) AS Uuid,
                     ju.deskripsi as Deskripsi,
                     ju.kuota AS Kouta,
                     ju.tanggal AS Tanggal,
                     ju.jam_mulai_ujian AS JamMulai,
                     ju.jam_akhir_ujian AS JamAkhir,
                     bs.uuid AS UuidBankSoal
                 FROM jadwal_ujian ju 
                 LEFT JOIN bank_soal bs ON ju.id_bank_soal = bs.id
                 WHERE uuid = @Uuid
                 """;

            DefaultTypeMap.MatchNamesWithUnderscores = true;

            var result = await connection.QuerySingleOrDefaultAsync<JadwalUjianResponse?>(sql, new { Uuid = request.JadwalUjianUuid });
            if (result == null)
            {
                return Result.Failure<JadwalUjianResponse>(JadwalUjianErrors.NotFound(request.JadwalUjianUuid));
            }

            return result;
        }
    }
}
