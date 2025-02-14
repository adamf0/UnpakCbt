using Dapper;
using System.Data.Common;
using UnpakCbt.Common.Application.Data;
using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Common.Domain;
using UnpakCbt.Modules.TemplatePertanyaan.Application.TemplatePertanyaan.GetTemplatePertanyaan;
using UnpakCbt.Modules.TemplatePertanyaan.Domain.TemplatePertanyaan;

namespace UnpakCbt.Modules.TemplatePertanyaan.Application.TemplatePertanyaan.GetAllTemplatePertanyaan
{
    internal sealed class GetAllTemplatePertanyaanDefaultByBankSoalQueryHandler(IDbConnectionFactory _dbConnectionFactory) : IQueryHandler<GetAllTemplatePertanyaanDefaultByBankSoalQuery, List<TemplatePertanyaanDefaultResponse>>
    {
        public async Task<Result<List<TemplatePertanyaanDefaultResponse>>> Handle(GetAllTemplatePertanyaanDefaultByBankSoalQuery request, CancellationToken cancellationToken)
        {
            await using DbConnection connection = await _dbConnectionFactory.OpenConnectionAsync();

            const string sql =
            """
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
            WHERE id_bank_soal = @IdBankSoal
            """;

            DefaultTypeMap.MatchNamesWithUnderscores = true;

            var result = await connection.QueryAsync<TemplatePertanyaanDefaultResponse>(sql, new { IdBankSoal = request.IdBankSoal });

            if (result == null || !result.Any())
            {
                return Result.Failure<List<TemplatePertanyaanDefaultResponse>>(TemplatePertanyaanErrors.EmptyData());
            }

            return Result.Success(result.ToList());
        }
    }
}
