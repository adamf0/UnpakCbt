using Dapper;
using System.Data.Common;
using UnpakCbt.Common.Application.Data;
using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Common.Domain;
using UnpakCbt.Modules.TemplateJawaban.Domain.TemplateJawaban;
using UnpakCbt.Modules.TemplateJawaban.Application.TemplateJawaban.GetTemplateJawaban;
using System.Data.SqlTypes;

namespace UnpakCbt.Modules.TemplateJawaban.Application.TemplateJawaban.GetAllTemplateJawaban
{
    internal sealed class GetAllTemplateJawabanByBankSoalV2QueryHandler(IDbConnectionFactory _dbConnectionFactory) : IQueryHandler<GetAllTemplateJawabanByBankSoalV2Query, List<TemplateJawabanResponse>>
    {
        public async Task<Result<List<TemplateJawabanResponse>>> Handle(GetAllTemplateJawabanByBankSoalV2Query request, CancellationToken cancellationToken)
        {
            await using DbConnection connection = await _dbConnectionFactory.OpenConnectionAsync();

            const string sql =
            """
            SELECT 
                ts.uuid as UuidTemplateSoal,
                CAST(NULLIF(tp.uuid, '') AS VARCHAR(36)) AS Uuid,
                tp.jawaban_text as JawabanText,
                tp.jawaban_img AS JawabanImg 
            FROM template_pilihan tp 
            LEFT JOIN template_soal ts ON tp.id_template_soal = ts.id 
            LEFT JOIN bank_soal bs ON ts.id_bank_soal = bs.id 
            WHERE bs.uuid = @BankSoalUuid 
            AND ts.state != 'init' 
            AND (
                (ts.pertanyaan_text IS NOT NULL OR TRIM(IFNULL(ts.pertanyaan_text, '')) <> '') OR 
                (ts.pertanyaan_img IS NOT NULL OR TRIM(IFNULL(ts.pertanyaan_img, '')) <> '')
            ) 
            ORDER BY RAND()
            """;


            /*AND 
            ts.state != "init" AND
            (
                (ts.pertanyaan_text is not null OR trim(IFNULL(ts.pertanyaan_text, '')) <> '') OR
                (ts.pertanyaan_img is not null OR trim(IFNULL(ts.pertanyaan_img, '')) <> '')
            ) ORDER BY RAND()*/

            DefaultTypeMap.MatchNamesWithUnderscores = true;

            var result = await connection.QueryAsync<TemplateJawabanResponse>(sql, new { BankSoalUuid = request.BakSoalUuid });

            if (result == null || !result.Any())
            {
                return Result.Failure<List<TemplateJawabanResponse>>(TemplateJawabanErrors.EmptyData());
            }

            return Result.Success(result.ToList());
        }
    }
}
