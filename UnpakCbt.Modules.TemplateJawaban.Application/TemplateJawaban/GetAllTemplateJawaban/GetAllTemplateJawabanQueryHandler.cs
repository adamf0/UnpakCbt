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
                id_template_soal as IdTemplateSoal,
                CAST(NULLIF(uuid, '') AS VARCHAR(36)) AS Uuid,
                jawaban_text as JawabanText,
                jawaban_img AS JawabanImg 
            FROM template_pilihan 
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
