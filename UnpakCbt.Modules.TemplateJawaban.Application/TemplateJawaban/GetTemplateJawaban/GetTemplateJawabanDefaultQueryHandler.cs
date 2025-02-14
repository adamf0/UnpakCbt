using Dapper;
using System.Data.Common;
using UnpakCbt.Common.Application.Data;
using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Common.Domain;
using UnpakCbt.Modules.TemplateJawaban.Domain.TemplateJawaban;

namespace UnpakCbt.Modules.TemplateJawaban.Application.TemplateJawaban.GetTemplateJawaban
{
    internal sealed class GetTemplateJawabanDefaultQueryHandler(IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetTemplateJawabanDefaultQuery, TemplateJawabanDefaultResponse>
    {
        public async Task<Result<TemplateJawabanDefaultResponse>> Handle(GetTemplateJawabanDefaultQuery request, CancellationToken cancellationToken)
        {
            await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

            //CAST(NULLIF(id_group, '') AS CHAR(36)) -> guid
            const string sql =
                $"""
                 SELECT 
                     id as Id,
                     id_template_soal as IdTemplateSoal,
                     CAST(NULLIF(uuid, '') AS VARCHAR(36)) AS Uuid,
                     jawaban_text as JawabanText,
                     jawaban_img AS JawabanImg 
                 FROM template_pilihan 
                 WHERE uuid = @Uuid
                 """;

            DefaultTypeMap.MatchNamesWithUnderscores = true;

            var result = await connection.QuerySingleOrDefaultAsync<TemplateJawabanDefaultResponse?>(sql, new { Uuid = request.TemplateJawabanUuid });
            if (result == null)
            {
                return Result.Failure<TemplateJawabanDefaultResponse>(TemplateJawabanErrors.NotFound(request.TemplateJawabanUuid));
            }

            return result;
        }
    }
}
