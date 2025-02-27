using Dapper;
using System.Data.Common;
using UnpakCbt.Common.Application.Data;
using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Common.Domain;
using UnpakCbt.Modules.TemplatePertanyaan.Domain.TemplatePertanyaan;

namespace UnpakCbt.Modules.TemplatePertanyaan.Application.TemplatePertanyaan.GetTemplatePertanyaan
{
    internal sealed class GetTemplatePertanyaanQueryHandler(IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetTemplatePertanyaanQuery, TemplatePertanyaanResponse>
    {
        public async Task<Result<TemplatePertanyaanResponse>> Handle(GetTemplatePertanyaanQuery request, CancellationToken cancellationToken)
        {
            await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

            //CAST(NULLIF(id_group, '') AS CHAR(36)) -> guid
            const string sql =
                $"""
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
                 WHERE ts.uuid = @Uuid
                 """;

            DefaultTypeMap.MatchNamesWithUnderscores = true;

            var result = await connection.QuerySingleOrDefaultAsync<TemplatePertanyaanResponse?>(sql, new { Uuid = request.TemplatePertanyaanUuid });
            if (result == null)
            {
                return Result.Failure<TemplatePertanyaanResponse>(TemplatePertanyaanErrors.NotFound(request.TemplatePertanyaanUuid));
            }

            return result;
        }
    }
}
