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
            DefaultTypeMap.MatchNamesWithUnderscores = true;

            int enableTrial = 0;
            string enableTrialValue = Environment.GetEnvironmentVariable("EnableTrial");
            if (int.TryParse(enableTrialValue, out enableTrial) == false)
            {
                enableTrial = 0; // Default value
            }

            JadwalUjianResponse? result = null;
            if (enableTrial == 1)
            {
                string idBankSoalTrialValue = Environment.GetEnvironmentVariable("IdBankSoalTrial");

                const string sql =
                $"""
                 SELECT 
                     CAST(NULLIF(ju.uuid, '') AS VARCHAR(36)) AS Uuid,
                     ju.deskripsi as Deskripsi,
                     ju.kuota AS Kouta,
                     ju.tanggal AS Tanggal,
                     ju.jam_mulai_ujian AS JamMulai,
                     ju.jam_akhir_ujian AS JamAkhir,
                     bs.uuid AS UuidBankSoal,
                     (SELECT bank_soal.uuid FROM bank_soal WHERE bank_soal.id = @idBankSoalTrial) AS UuidBankSoalTrial
                 FROM jadwal_ujian ju 
                 LEFT JOIN bank_soal bs ON ju.id_bank_soal = bs.id
                 WHERE ju.uuid = @Uuid
                 """;

                result = await connection.QuerySingleOrDefaultAsync<JadwalUjianResponse?>(sql, new { Uuid = request.JadwalUjianUuid, idBankSoalTrial = idBankSoalTrialValue });
            }
            else {
                const string sql =
                    $"""
                 SELECT 
                     CAST(NULLIF(ju.uuid, '') AS VARCHAR(36)) AS Uuid,
                     ju.deskripsi as Deskripsi,
                     ju.kuota AS Kouta,
                     ju.tanggal AS Tanggal,
                     ju.jam_mulai_ujian AS JamMulai,
                     ju.jam_akhir_ujian AS JamAkhir,
                     bs.uuid AS UuidBankSoal,
                     null AS UuidBankSoalTrial
                 FROM jadwal_ujian ju 
                 LEFT JOIN bank_soal bs ON ju.id_bank_soal = bs.id
                 WHERE ju.uuid = @Uuid
                 """;

                result = await connection.QuerySingleOrDefaultAsync<JadwalUjianResponse?>(sql, new { Uuid = request.JadwalUjianUuid });
            }
            
            if (result == null)
            {
                return Result.Failure<JadwalUjianResponse>(JadwalUjianErrors.NotFound(request.JadwalUjianUuid));
            }

            return result;
        }
    }
}
