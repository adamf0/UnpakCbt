﻿using Dapper;
using System.Data.Common;
using UnpakCbt.Common.Application.Data;
using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Common.Domain;
using UnpakCbt.Modules.TemplatePertanyaan.Domain.TemplatePertanyaan;

namespace UnpakCbt.Modules.TemplatePertanyaan.Application.TemplatePertanyaan.GetTemplatePertanyaan
{
    internal sealed class GetTemplatePertanyaanDefaultQueryHandler(IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetTemplatePertanyaanDefaultQuery, TemplatePertanyaanDefaultResponse>
    {
        public async Task<Result<TemplatePertanyaanDefaultResponse>> Handle(GetTemplatePertanyaanDefaultQuery request, CancellationToken cancellationToken)
        {
            await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

            //CAST(NULLIF(id_group, '') AS CHAR(36)) -> guid
            const string sql =
                $"""
                 SELECT 
                     id as Id,
                     CAST(NULLIF(uuid, '') as VARCHAR(36)) AS Uuid,
                     id_bank_soal as IdBankSoal,
                     tipe as Tipe,
                     pertanyaan_text as Pertanyaan,
                     pertanyaan_img as Gambar,
                     jawaban_benar as JawabanBenar,
                     bobot as Bobot,
                     state as State 
                 FROM template_soal 
                 WHERE uuid = @Uuid
                 """;

            DefaultTypeMap.MatchNamesWithUnderscores = true;

            var result = await connection.QuerySingleOrDefaultAsync<TemplatePertanyaanDefaultResponse?>(sql, new { Uuid = request.TemplatePertanyaanUuid });
            if (result == null)
            {
                return Result.Failure<TemplatePertanyaanDefaultResponse>(TemplatePertanyaanErrors.NotFound(request.TemplatePertanyaanUuid));
            }

            return result;
        }
    }
}
