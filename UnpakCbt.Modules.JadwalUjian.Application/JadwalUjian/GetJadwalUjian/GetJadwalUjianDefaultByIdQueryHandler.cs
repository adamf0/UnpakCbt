using Dapper;
using System.Data.Common;
using UnpakCbt.Common.Application.Data;
using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Common.Domain;
using UnpakCbt.Modules.JadwalUjian.Domain.JadwalUjian;

namespace UnpakCbt.Modules.JadwalUjian.Application.JadwalUjian.GetJadwalUjian
{
    internal sealed class GetJadwalUjianDefaultByIdQueryHandler(IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetJadwalUjianDefaultByIdQuery, JadwalUjianDefaultResponse>
    {
        public async Task<Result<JadwalUjianDefaultResponse>> Handle(GetJadwalUjianDefaultByIdQuery request, CancellationToken cancellationToken)
        {
            await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

            //CAST(NULLIF(id_group, '') AS CHAR(36)) -> guid
            const string sql =
                $"""
                 SELECT 
                     id as Id,
                     CAST(NULLIF(uuid, '') AS VARCHAR(36)) AS Uuid,
                     deskripsi as Deskripsi,
                     kuota AS Kouta,
                     tanggal AS Tanggal,
                     jam_mulai_ujian AS JamMulai,
                     jam_akhir_ujian AS JamAkhir,
                     id_bank_soal AS IdBankSoal
                 FROM jadwal_ujian  
                 WHERE id = @id
                 """;

            DefaultTypeMap.MatchNamesWithUnderscores = true;

            var result = await connection.QuerySingleOrDefaultAsync<JadwalUjianDefaultResponse?>(sql, new { id = request.id });
            if (result == null)
            {
                return Result.Failure<JadwalUjianDefaultResponse>(JadwalUjianErrors.IdNotFound(request.id??0));
            }

            return result;
        }
    }
}
