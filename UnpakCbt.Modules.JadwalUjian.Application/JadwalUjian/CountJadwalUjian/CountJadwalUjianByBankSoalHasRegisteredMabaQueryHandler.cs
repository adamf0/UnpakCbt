using Dapper;
using System.Data.Common;
using UnpakCbt.Common.Application.Data;
using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Common.Domain;

namespace UnpakCbt.Modules.JadwalUjian.Application.JadwalUjian.CountJadwalUjian
{
    internal sealed class CountJadwalUjianByBankSoalHasRegisteredMabaQueryHandler(IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<CountJadwalUjianByBankSoalHasRegisteredMabaQuery, int>
    {
        public async Task<Result<int>> Handle(CountJadwalUjianByBankSoalHasRegisteredMabaQuery request, CancellationToken cancellationToken)
        {
            await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

            //CAST(NULLIF(id_group, '') AS CHAR(36)) -> guid
            const string sql =
                $"""
                 SELECT 
                     COUNT(*)
                 FROM jadwal_ujian ju 
                 LEFT JOIN bank_soal bs ON ju.id_bank_soal = bs.id
                 WHERE 
                     bs.uuid = @BankSoalUuid AND 
                     (
                 		SELECT COUNT(*) 
                         FROM ujian u 
                         WHERE u.id_jadwal_ujian = ju.id
                     ) > 0
                 """;

            DefaultTypeMap.MatchNamesWithUnderscores = true;

            int count = await connection.QuerySingleAsync<int>(sql, new { request.BankSoalUuid });

            return Result.Success(count);
        }
    }
}
