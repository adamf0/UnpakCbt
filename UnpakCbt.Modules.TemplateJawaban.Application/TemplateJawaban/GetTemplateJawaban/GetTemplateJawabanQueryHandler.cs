using Dapper;
using System.Data.Common;
using UnpakCbt.Common.Application.Data;
using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Common.Domain;
using UnpakCbt.Modules.TemplateJawaban.Domain.TemplateJawaban;

namespace UnpakCbt.Modules.TemplateJawaban.Application.TemplateJawaban.GetTemplateJawaban
{
    internal sealed class GetTemplateJawabanQueryHandler(IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetTemplateJawabanQuery, TemplateJawabanResponse>
    {
        public async Task<Result<TemplateJawabanResponse>> Handle(GetTemplateJawabanQuery request, CancellationToken cancellationToken)
        {
            await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

            //CAST(NULLIF(id_group, '') AS CHAR(36)) -> guid
            const string sql =
                $"""
                 SELECT 
                      ts.uuid as UuidTemplateSoal,
                     CAST(NULLIF(tp.uuid, '') AS VARCHAR(36)) AS Uuid,
                     tp.jawaban_text as JawabanText,
                     tp.jawaban_img AS JawabanImg 
                 FROM template_pilihan tp 
                 LEFT JOIN template_soal ts ON tp.id_template_soal = ts.id
                 WHERE tp.uuid = @Uuid
                 """;

            DefaultTypeMap.MatchNamesWithUnderscores = true;

            var result = await connection.QuerySingleOrDefaultAsync<TemplateJawabanResponse?>(sql, new { Uuid = request.TemplateJawabanUuid });
            if (result == null)
            {
                return Result.Failure<TemplateJawabanResponse>(TemplateJawabanErrors.NotFound(request.TemplateJawabanUuid));
            }

            return result;
        }
    }
}
