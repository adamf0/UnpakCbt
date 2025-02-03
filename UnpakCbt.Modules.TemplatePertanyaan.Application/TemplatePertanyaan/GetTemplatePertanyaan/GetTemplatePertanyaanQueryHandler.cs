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
                     CAST(NULLIF(uuid, '') AS VARCHAR(36)) AS Uuid,
                     id_bank_soal as IdBankSoal,
                     tipe as Tipe,
                     pertanyaan_text as PertanyaanText,
                     pertanyaan_img AS PertanyaanImg,
                     state AS State
                 FROM template_soal 
                 WHERE uuid = @Uuid
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
