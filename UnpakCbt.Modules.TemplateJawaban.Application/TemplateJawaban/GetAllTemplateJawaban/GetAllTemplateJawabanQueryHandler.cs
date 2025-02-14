using Dapper;
using System.Data.Common;
using UnpakCbt.Common.Application.Data;
using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Common.Domain;
using UnpakCbt.Modules.TemplateJawaban.Domain.TemplateJawaban;
using UnpakCbt.Modules.TemplateJawaban.Application.TemplateJawaban.GetTemplateJawaban;

namespace UnpakCbt.Modules.TemplateJawaban.Application.TemplateJawaban.GetAllTemplateJawaban
{
    internal sealed class GetAllTemplateJawabanQueryHandler(IDbConnectionFactory _dbConnectionFactory) : IQueryHandler<GetAllTemplateJawabanQuery, List<TemplateJawabanResponse>>
    {
        public async Task<Result<List<TemplateJawabanResponse>>> Handle(GetAllTemplateJawabanQuery request, CancellationToken cancellationToken)
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
            """;

            DefaultTypeMap.MatchNamesWithUnderscores = true;

            var result = await connection.QueryAsync<TemplateJawabanResponse>(sql);

            if (result == null || !result.Any())
            {
                return Result.Failure<List<TemplateJawabanResponse>>(TemplateJawabanErrors.EmptyData());
            }

            return Result.Success(result.ToList());
        }
    }
}
