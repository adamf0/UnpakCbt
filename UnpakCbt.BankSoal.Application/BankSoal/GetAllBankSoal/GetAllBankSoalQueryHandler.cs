using Dapper;
using System.Data.Common;
using UnpakCbt.Common.Application.Data;
using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Common.Domain;
using UnpakCbt.Modules.BankSoal.Application.BankSoal.GetBankSoal;
using UnpakCbt.Modules.BankSoal.Domain.BankSoal;


namespace UnpakCbt.Modules.BankSoal.Application.BankSoal.GetAllBankSoal
{
    internal sealed class GetAllBankSoalQueryHandler(IDbConnectionFactory _dbConnectionFactory)
        : IQueryHandler<GetAllBankSoalQuery, List<BankSoalResponse>>
    {
        public async Task<Result<List<BankSoalResponse>>> Handle(GetAllBankSoalQuery request, CancellationToken cancellationToken)
        {
            await using DbConnection connection = await _dbConnectionFactory.OpenConnectionAsync();

            const string sql =
            """
            SELECT 
                uuid AS Uuid,
                judul AS Judul,
                rule AS Rule,
                status AS Status,
                (select count(*) from jadwal_ujian where jadwal_ujian.id_bank_soal = bank_soal.id) AS JadwalTerhubung
            FROM bank_soal
            """;

            var queryResult = await connection.QueryAsync<BankSoalResponse>(sql);

            if (!queryResult.Any())
            {
                return Result.Failure<List<BankSoalResponse>>(BankSoalErrors.EmptyData());
            }

            return Result.Success(queryResult.ToList());
        }
    }
}
