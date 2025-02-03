using Dapper;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using UnpakCbt.Common.Application.Data;
using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Common.Domain;
using System.Text.Json;
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
                CAST(NULLIF(uuid, '') AS VARCHAR(36)) AS Uuid,
                deskripsi as Deskripsi,
                kuota AS Kouta,
                tanggal AS Tanggal,
                jam_mulai_ujian AS JamMulai,
                jam_akhir_ujian AS JamAkhir,
                id_bank_soal AS IdBankSoal
            FROM jadwal_ujian 
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
