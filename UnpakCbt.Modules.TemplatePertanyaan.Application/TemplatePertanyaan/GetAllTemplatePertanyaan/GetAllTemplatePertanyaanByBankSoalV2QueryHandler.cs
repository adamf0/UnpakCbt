using Dapper;
using System.Data.Common;
using System.Data.SqlTypes;
using UnpakCbt.Common.Application.Data;
using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Common.Domain;
using UnpakCbt.Modules.TemplatePertanyaan.Application.TemplatePertanyaan.GetTemplatePertanyaan;
using UnpakCbt.Modules.TemplatePertanyaan.Domain.TemplatePertanyaan;

namespace UnpakCbt.Modules.TemplatePertanyaan.Application.TemplatePertanyaan.GetAllTemplatePertanyaan
{
    internal sealed class GetAllTemplatePertanyaanByBankSoalV2QueryHandler(IDbConnectionFactory _dbConnectionFactory) : IQueryHandler<GetAllTemplatePertanyaanByBankSoalV2Query, List<TemplatePertanyaanResponseV2>>
    {
        //[PR] harus dipisah antara entry pertanyaan dan soal pertanyaan
        public async Task<Result<List<TemplatePertanyaanResponseV2>>> Handle(GetAllTemplatePertanyaanByBankSoalV2Query request, CancellationToken cancellationToken)
        {
            await using DbConnection connection = await _dbConnectionFactory.OpenConnectionAsync();

            const string sql =
            """
            SELECT 
                CAST(NULLIF(ts.uuid, '') as VARCHAR(36)) AS Uuid,
                CAST(NULLIF(bs.uuid, '') as VARCHAR(36)) as UuidBankSoal,
                ts.tipe as Tipe,
                ts.pertanyaan_text as Pertanyaan,
                ts.pertanyaan_img as Gambar,
                ts.bobot as Bobot,
                ts.state as State 
            FROM template_soal ts 
            LEFT JOIN bank_soal bs ON ts.id_bank_soal = bs.id 
            LEFT JOIN template_pilihan tp ON ts.jawaban_benar = tp.id 
            WHERE bs.uuid = @BankSoalUuid 
            AND ts.state != 'init' 
            AND (
                (ts.pertanyaan_text IS NOT NULL OR TRIM(IFNULL(ts.pertanyaan_text, '')) <> '') OR 
                (ts.pertanyaan_img IS NOT NULL OR TRIM(IFNULL(ts.pertanyaan_img, '')) <> '')
            ) 
            ORDER BY RAND()
            """;

            DefaultTypeMap.MatchNamesWithUnderscores = true;

            var result = await connection.QueryAsync<TemplatePertanyaanResponseV2>(sql, new { BankSoalUuid = request.BakSoalUuid });

            if (result == null || !result.Any())
            {
                return Result.Failure<List<TemplatePertanyaanResponseV2>>(TemplatePertanyaanErrors.EmptyData());
            }

            return Result.Success(result.ToList());
        }
    }
}
