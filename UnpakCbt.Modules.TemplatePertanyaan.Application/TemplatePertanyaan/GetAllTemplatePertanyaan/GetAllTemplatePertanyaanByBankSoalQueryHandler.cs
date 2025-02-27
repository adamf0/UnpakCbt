using Dapper;
using System.Data.Common;
using UnpakCbt.Common.Application.Data;
using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Common.Domain;
using UnpakCbt.Modules.TemplatePertanyaan.Application.TemplatePertanyaan.GetTemplatePertanyaan;
using UnpakCbt.Modules.TemplatePertanyaan.Domain.TemplatePertanyaan;

namespace UnpakCbt.Modules.TemplatePertanyaan.Application.TemplatePertanyaan.GetAllTemplatePertanyaan
{
    internal sealed class GetAllTemplatePertanyaanByBankSoalQueryHandler(IDbConnectionFactory _dbConnectionFactory) : IQueryHandler<GetAllTemplatePertanyaanByBankSoalQuery, List<TemplatePertanyaanResponse>>
    {
        public async Task<Result<List<TemplatePertanyaanResponse>>> Handle(GetAllTemplatePertanyaanByBankSoalQuery request, CancellationToken cancellationToken)
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
                CAST(NULLIF(tp.uuid, '') as VARCHAR(36)) as UuidJawabanBenar,
                ts.bobot as Bobot,
                ts.state as State 
            FROM template_soal ts 
            LEFT JOIN bank_soal bs ON ts.id_bank_soal = bs.id 
            LEFT JOIN template_pilihan tp ON ts.jawaban_benar = tp.id 
            where bs.uuid = @BankSoalUuid
            """;

            DefaultTypeMap.MatchNamesWithUnderscores = true;

            var result = await connection.QueryAsync<TemplatePertanyaanResponse>(sql, new { BankSoalUuid = request.BakSoalUuid });

            if (result == null || !result.Any())
            {
                return Result.Failure<List<TemplatePertanyaanResponse>>(TemplatePertanyaanErrors.EmptyData());
            }

            return Result.Success(result.ToList());
        }
    }
}
