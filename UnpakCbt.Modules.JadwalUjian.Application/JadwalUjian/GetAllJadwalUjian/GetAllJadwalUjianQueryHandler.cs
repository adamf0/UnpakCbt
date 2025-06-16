using Dapper;
using System.Data.Common;
using UnpakCbt.Common.Application.Data;
using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Common.Domain;
using UnpakCbt.Modules.JadwalUjian.Application.JadwalUjian.GetJadwalUjian;
using UnpakCbt.Modules.JadwalUjian.Domain.JadwalUjian;

namespace UnpakCbt.Modules.JadwalUjian.Application.JadwalUjian.GetAllJadwalUjian
{
    internal sealed class GetAllJadwalUjianQueryHandler(IDbConnectionFactory _dbConnectionFactory)
        : IQueryHandler<GetAllJadwalUjianQuery, List<JadwalUjianResponse>>
    {
        public async Task<Result<List<JadwalUjianResponse>>> Handle(GetAllJadwalUjianQuery request, CancellationToken cancellationToken)
        {
            await using DbConnection connection = await _dbConnectionFactory.OpenConnectionAsync();

            const string sql =
            """
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
            ORDER BY ju.tanggal ASC, ju.jam_mulai_ujian ASC
            """;

            var queryResult = await connection.QueryAsync<JadwalUjianResponse>(sql);

            if (!queryResult.Any())
            {
                return Result.Failure<List<JadwalUjianResponse>>(JadwalUjianErrors.EmptyData());
            }

            return Result.Success(queryResult.ToList());
        }
    }
}
