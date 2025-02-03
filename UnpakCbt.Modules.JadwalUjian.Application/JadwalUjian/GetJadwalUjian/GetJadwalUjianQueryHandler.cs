﻿using Dapper;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
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
                     CAST(NULLIF(uuid, '') AS VARCHAR(36)) AS Uuid,
                     deskripsi as Deskripsi,
                     kuota AS Kouta,
                     tanggal AS Tanggal,
                     jam_mulai_ujian AS JamMulai,
                     jam_akhir_ujian AS JamAkhir,
                     id_bank_soal AS IdBankSoal
                 FROM jadwal_ujian 
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
