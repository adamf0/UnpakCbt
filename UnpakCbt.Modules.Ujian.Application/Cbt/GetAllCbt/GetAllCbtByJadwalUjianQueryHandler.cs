using Dapper;
using System.Data.Common;
using UnpakCbt.Common.Application.Data;
using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Common.Domain;
using UnpakCbt.Modules.Ujian.Application.Cbt.GetCbt;
using UnpakCbt.Modules.Ujian.Domain.Ujian;

namespace UnpakCbt.Modules.Ujian.Application.Cbt.GetAllCbt
{
    internal sealed class GetAllCbtByJadwalUjianQueryHandler(IDbConnectionFactory _dbConnectionFactory)
        : IQueryHandler<GetAllCbtByJadwalUjianQuery, List<CbtResponse>>
    {
        public async Task<Result<List<CbtResponse>>> Handle(GetAllCbtByJadwalUjianQuery request, CancellationToken cancellationToken)
        {
            await using DbConnection connection = await _dbConnectionFactory.OpenConnectionAsync();

            const string sql =
            """
            select 
                CAST(NULLIF(uc.uuid, '') AS VARCHAR(36)) as uuid,
                CAST(NULLIF(u.uuid, '') AS VARCHAR(36))  as uuidUjian,
                CAST(NULLIF(ts.uuid, '') AS VARCHAR(36))  as uuidTemplatePertanyaan,
                ts.pertanyaan_text as pertanyaanText,
                ts.pertanyaan_img as pertanyaanImg,
                CAST(NULLIF(tp.uuid, '') AS VARCHAR(36)) as uuidTemplatePilihan,
                tp.jawaban_text as jawabanText,
                tp.jawaban_img as jawabanImg,
                ts.tipe as tipe,
                CAST(NULLIF(ju.uuid, '') AS VARCHAR(36)) as uuidJadwalUjian,
                CAST(NULLIF(bs.uuid, '') AS VARCHAR(36)) as uuidBankSoal
            from ujian_cbt uc 
            left join ujian u on uc.id_ujian = u.id 
            left join template_soal ts on uc.id_template_soal = ts.id 
            left join template_pilihan tp on uc.jawaban_benar = tp.id
            left join jadwal_ujian ju on u.id_jadwal_ujian = ju.id
            left join bank_soal bs on ju.id_bank_soal = bs.id 
            WHERE u.uuid = @UuidUjian
            """;

            var queryResult = await connection.QueryAsync<CbtResponse>(sql, new { request.uuidUjian });

            if (!queryResult.Any())
            {
                return Result.Failure<List<CbtResponse>>(UjianErrors.EmptyData());
            }

            return Result.Success(queryResult.ToList());
        }
    }
}
