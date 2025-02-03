using Dapper;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnpakCbt.Common.Application.Data;
using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Common.Domain;
using UnpakCbt.Modules.TemplatePertanyaan.Domain.TemplatePertanyaan;
using UnpakCbt.Modules.TemplatePertanyaan.Application.TemplatePertanyaan.GetTemplatePertanyaan;

namespace UnpakCbt.Modules.TemplatePertanyaan.Application.TemplatePertanyaan.GetAllTemplatePertanyaan
{
    internal sealed class GetAllTemplatePertanyaanQueryHandler(IDbConnectionFactory _dbConnectionFactory) : IQueryHandler<GetAllTemplatePertanyaanQuery, List<TemplatePertanyaanResponse>>
    {
        public async Task<Result<List<TemplatePertanyaanResponse>>> Handle(GetAllTemplatePertanyaanQuery request, CancellationToken cancellationToken)
        {
            await using DbConnection connection = await _dbConnectionFactory.OpenConnectionAsync();

            const string sql =
            """
            SELECT 
                CAST(NULLIF(uuid, '') as VARCHAR(36)) AS Uuid,
                id_bank_soal as IdBankSoal,
                tipe as Tipe,
                pertanyaan_text as Pertanyaan,
                pertanyaan_img as Gambar,
                jawaban_benar as JawabanBenar,
                bobot as Bobot,
                state as State 
            FROM template_soal  
            """;

            DefaultTypeMap.MatchNamesWithUnderscores = true;

            var result = await connection.QueryAsync<TemplatePertanyaanResponse>(sql);

            if (result == null || !result.Any())
            {
                return Result.Failure<List<TemplatePertanyaanResponse>>(TemplatePertanyaanErrors.EmptyData());
            }

            return Result.Success(result.ToList());
        }
    }
}
