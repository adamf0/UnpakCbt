using Dapper;
using System.Data.Common;
using UnpakCbt.Common.Application.Data;
using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Common.Domain;
using UnpakCbt.Modules.JadwalUjian.Application.JadwalUjian.GetJadwalUjian;
using UnpakCbt.Modules.JadwalUjian.Domain.JadwalUjian;

namespace UnpakCbt.Modules.JadwalUjian.Application.JadwalUjian.GetActiveJadwalUjian
{
    internal sealed class GetActiveJadwalUjianQueryHandler(IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetActiveJadwalUjianQuery, JadwalUjianActiveResponse>
    {
        public async Task<Result<JadwalUjianActiveResponse>> Handle(GetActiveJadwalUjianQuery request, CancellationToken cancellationToken)
        {
            await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

            //CAST(NULLIF(id_group, '') AS CHAR(36)) -> guid
            const string sql =
                $"""
                 SELECT 
                     ju.deskripsi AS Deskripsi,
                     ju.kuota AS Kuota,
                     (SELECT COUNT(*) FROM ujian u WHERE u.id_jadwal_ujian = ju.id AND u.status != 'cancel') AS TotalJoin,
                     ju.tanggal AS Tanggal,
                     ju.jam_mulai_ujian AS JamMulai,
                     ju.jam_akhir_ujian AS JamAkhir,
                     (
                         CASE 
                             WHEN NOW() BETWEEN TIMESTAMP(ju.tanggal, ju.jam_mulai_ujian) AND TIMESTAMP(ju.tanggal, ju.jam_akhir_ujian) THEN "ongoing"
                             WHEN NOW() < TIMESTAMP(ju.tanggal, ju.jam_mulai_ujian) THEN "not started"
                             WHEN NOW() > TIMESTAMP(ju.tanggal, ju.jam_akhir_ujian) THEN "expired"
                             ELSE "unknown"
                         END 
                     ) AS StatusUjian
                 FROM jadwal_ujian ju 
                 LEFT JOIN bank_soal bs ON ju.id_bank_soal = bs.id 
                 WHERE ju.tanggal = CURDATE();
                 """;

            DefaultTypeMap.MatchNamesWithUnderscores = true;

            var result = await connection.QuerySingleOrDefaultAsync<JadwalUjianActiveResponse?>(sql);
            if (result == null)
            {
                return Result.Failure<JadwalUjianActiveResponse>(JadwalUjianErrors.NotFoundActive());
            }

            return result;
        }
    }
}
